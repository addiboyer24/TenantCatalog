//----------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//---------------------------------------------------------------------------------------------

using CDMS.CP.Platform.Common.Connectors;
using CDMS.CP.Platform.Common.Connectors.Interfaces;
using CDMS.CP.Platform.Common.Connectors.Standard.Interfaces;
using CDMS.CP.Platform.Common.Connectors.Standard.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantCatalog.Domain.Constants;

namespace TenantCatalog.Infrastructure
{
    /// <summary>
    /// User service
    /// </summary>
    /// <seealso cref="CoreAuthentication.IAuthUserService" />
    public class UserService : IAuthUserService
    {
        /// <summary>
        /// The stored procedure repository
        /// </summary>
        private readonly IStoredProcedureRepository storedProcedureRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public UserService(IServiceProvider serviceProvider)
        {
            var values = serviceProvider.GetServices<IStoredProcedureRepository>();
            this.storedProcedureRepository = values.First(x => ((StoredProcedureRepository)x).DbType == CDMS.CP.Platform.Common.Connectors.DbType.U2Role.ToString());
        }

        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <param name="role">The role.</param>
        private static void CreateCompany(IGrouping<int, UserAllScreen> roles, ScreenRole role)
        {
            var companyList = roles.GroupBy(a => a.CompanyId);
            if (companyList.Any())
            {
                var companyDtoList = new List<ScreenCompany>();
                foreach (var company in companyList)
                {
                    companyDtoList.Add(new ScreenCompany
                    {
                        CompanyID = Convert.ToInt32(company.Key, CultureInfo.InvariantCulture),
                        CompanyName = company.First().CompanyName
                    });
                }

                role.Companies = companyDtoList;
            }
        }

        /// <summary>
        /// Creates the screens.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <param name="role">The role.</param>
        private static void CreateScreens(IGrouping<int, UserAllScreen> roles, ScreenRole role)
        {
            var screenList = roles.GroupBy(a => a.ScreenId);
            if (screenList.Any())
            {
                var screensList = new List<Screens>();
                foreach (var screen in screenList)
                {
                    var firstScreen = screen.First();
                    var s = new Screens
                    {
                        ScreenId = screen.Key,
                        ScreenName = firstScreen.ScreenName,
                        ScreenCode = firstScreen.ScreenCode,
                        CssClass = firstScreen.CssName,
                        Path = firstScreen.ScreenPath,
                        ParentId = firstScreen.ParentId,
                        Display = firstScreen.Display,
                        IsAuthorized = firstScreen.IsAuthorized,
                        DisplayOrder = firstScreen.DisplayOrder
                    };
                    var permissionsList = screen.GroupBy(a => a.PermissionId);
                    if (permissionsList.Any())
                    {
                        var permissionDTOList = new List<ScreenPermission>();
                        foreach (var permissions in permissionsList)
                        {
                            permissionDTOList.Add(new ScreenPermission
                            {
                                PermissionId = Convert.ToInt32(permissions.Key, CultureInfo.InvariantCulture),
                                PermissionName = permissions.First().PermissionName
                            });
                        }

                        s.Permissions = permissionDTOList;
                    }

                    screensList.Add(s);
                }

                role.Screens = screensList;
            }
        }

        /// <summary>
        /// Populates the SQL parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="sqlParameterList">The SQL parameter list.</param>
        /// <param name="query">The query.</param>
        private static void PopulateSqlParameters(string userId, int companyId, out List<SqlParameter> sqlParameterList, out StringBuilder query)
        {
            sqlParameterList = new List<SqlParameter>();
            query = new StringBuilder("[dbo].[SPGetUserAllScreen]");
            if (companyId == 0)
            {
                sqlParameterList.Add(new SqlParameter("@Id", userId));
                query.Append(" @Id");
            }
            else
            {
                sqlParameterList.Add(new SqlParameter("@Id", userId));
                sqlParameterList.Add(new SqlParameter("@CompanyId", companyId));
                query.Append(" @Id");
                query.Append(",@CompanyId");
            }
        }

        /// <summary>
        /// Creates the roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleList">The role list.</param>
        private static void CreateRoles(ScreenUser user, IEnumerable<IGrouping<int, UserAllScreen>> roleList)
        {
            if (roleList.Any())
            {
                var roleDtoList = new List<ScreenRole>();
                foreach (var roles in roleList)
                {
                    var role = new ScreenRole
                    {
                        RoleId = roles.Key,
                        RoleName = roles.First().RoleName
                    };

                    CreateCompany(roles, role);
                    CreateScreens(roles, role);
                    roleDtoList.Add(role);
                }

                user.Roles = roleDtoList;
            }
        }

        /// <summary>
        /// Gets all permission screen .
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>
        /// Screen User List
        /// </returns>
        public ScreenUser GetAllPermissionsFromDB(string userId, int companyId)
        {
            PopulateSqlParameters(userId, companyId, out List<SqlParameter> sqlParameterList, out StringBuilder query);

            var result = this.storedProcedureRepository.ExecuteResultSet<UserAllScreen>(query.ToString(), parameters: sqlParameterList.ToArray());
            ScreenUser user = new ScreenUser();
            if (result.Data.Any())
            {
                user.UserId = result.Data.First().UserId;
                user.UserName = result.Data.First().UserName;

                var roleList = result.Data.GroupBy(a => a.RoleId);
                CreateRoles(user, roleList);
            }

            return user;
        }

        /// <summary>
        /// Gets all screen permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>list of permissions</returns>
        public ICollection<ScreenPermissions> GetAllScreePermission(string userId, int companyId)
        {
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("@Id", userId));
            sqlParameterList.Add(new SqlParameter("@CompanyId", companyId));

            var result = this.storedProcedureRepository.ExecuteResultSet<ScreenPermissions>(AppTemplateConstants.GetAllUsersScreenPermission, parameters: sqlParameterList.ToArray()).Data;

            return result.ToList();
        }

        /// <summary>
        /// Gets all companies.
        /// </summary>
        /// <param name="userObjId">user object id</param>
        /// <returns>Company Model</returns>
        public List<CompanyModel> GetAllCompanies(string userObjId)
        {
            SqlParameter paramUserObjId = new SqlParameter()
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@UserId",
                Value = userObjId
            };

            return this.storedProcedureRepository.ExecuteSqlQuery<CompanyModel>(AppTemplateConstants.GetAllCompanies, parameters: paramUserObjId).ToList();
        }

        /// <summary>
        /// Gets the user name by email.
        /// </summary>
        /// <param name="emailList">The email list.</param>
        /// <returns>List of User Object</returns>
        public async Task<IEnumerable<UserDetailDto>> GetAllUserNameForEmailAsync(List<string> emailList)
        {
            DataTable dataTableEmail = new DataTable();
            dataTableEmail.Columns.Add("Email", typeof(string));
            foreach (var email in emailList)
            {
                dataTableEmail.Rows.Add(email);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               Utility.SetDataTableParameter(AppTemplateConstants.UserEmailType, dataTableEmail, "@UserEmail")
            };

            return await this.storedProcedureRepository.ExecuteSqlQueryAsync<UserDetailDto>(AppTemplateConstants.GetUserByEmail, parameters: parameters.ToArray()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>User details</returns>
        public UserDetails GetUserDetails(string userId, int companyId)
        {
            var sqlParameterList = new List<SqlParameter>
            {
                Utility.SetCustomParameter(userId, "@Id"),
                Utility.SetCompanyIdParameter(companyId)
            };
            return this.storedProcedureRepository.ExecuteResultSet<UserDetails>(AppTemplateConstants.GetUserDetails, parameters: sqlParameterList.ToArray()).Data.FirstOrDefault();
        }
    }
}
