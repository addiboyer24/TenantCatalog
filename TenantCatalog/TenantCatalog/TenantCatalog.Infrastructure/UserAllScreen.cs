//----------------------------------------------------------------------------------------------
// <copyright file="UserAllScreen.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//---------------------------------------------------------------------------------------------

namespace TenantCatalog.Infrastructure
{
    /// <summary>
    /// User All Screen
    /// </summary>
    public partial class UserAllScreen
    {
        /// <summary>
        /// Gets or sets the screen identifier.
        /// </summary>
        /// <value>
        /// The screen identifier.
        /// </value>
        public int ScreenId { get; set; }

        /// <summary>
        /// Gets or sets the name of the screen.
        /// </summary>
        /// <value>
        /// The name of the screen.
        /// </value>
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets or sets the code of the screen.
        /// </summary>
        /// <value>
        /// The code of the screen.
        /// </value>
        public string ScreenCode { get; set; }

        /// <summary>
        /// Gets or sets the screen path.
        /// </summary>
        /// <value>
        /// The screen path.
        /// </value>
        public string ScreenPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the CSS.
        /// </summary>
        /// <value>
        /// The name of the CSS.
        /// </value>
        public string CssName { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        /// <value>
        /// The name of the permission.
        /// </value>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        /// <value>
        /// The permission identifier.
        /// </value>
        public int? PermissionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screens"/> is display.
        /// </summary>
        /// <value>
        ///   <c>true</c> if display; otherwise, <c>false</c>.
        /// </value>
        public bool Display { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Gets or sets the user role identifier.
        /// </summary>
        /// <value>
        /// The user role identifier.
        /// </value>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can impersonate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can impersonate; otherwise, <c>false</c>.
        /// </value>
        public bool CanImpersonate { get; set; }
    }
}
