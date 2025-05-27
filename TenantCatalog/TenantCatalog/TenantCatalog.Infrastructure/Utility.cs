// ----------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// ---------------------------------------------------------------------------------------------

using Azure.Messaging.ServiceBus;
using CDMS.CP.Platform.Common.Authentication;
using CDMS.CP.Platform.Common.Connectors;
using CDMS.CP.Platform.Common.Connectors.Standard.Interfaces;
using CDMS.CP.Platform.Common.Connectors.Standard.Models;
using CDMS.CP.Platform.Common.EventGridHandler.Handlers;
using CDMS.CP.Platform.Common.EventGridHandler.Interfaces;
using CDMS.CP.Platform.Common.EventGridHandler.Models;
using CDMS.CP.Platform.Common.Logging.V2.Interfaces;
using CDMS.CP.Platform.Common.ServiceBusConnector.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Transactions;

namespace TenantCatalog.Infrastructure
{
    /// <summary>
    /// Event Grid Helper
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Utility
    {
        /// <summary>
        /// Gets or sets a value indicating whether mock handler should be used or not.
        /// </summary>
        public static bool UseMockHandler { get; set; }

        /// <summary>
        /// Gets or sets a event grid handler instance. Used for unit testing purpose.
        /// </summary>
        public static EventGridHandler EventGridHandlerMock { get; set; }

        public static IEventGridConnector EventGridConnectorMock { get; set; }

        public static IEventBusConnector EventBusConnectorMock { get; set; }

        /// <summary>
        /// Appends the doctypesuffix
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="doctype">type of the document</param>
        /// <returns>
        /// the string
        /// </returns>
        public static string AppenddoctypeSuffix(string id, string doctype)
        {
            return string.Concat(id, "_", doctype);
        }

        /// <summary>
        /// Executes the action block.
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism.</param>
        /// <returns>
        /// Action Block
        /// </returns>
        public static ActionBlock<T> ExecuteActionBlock<T>(Action<T> action, int maxDegreeOfParallelism = -1)
        {
            var executionDataFlowOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                BoundedCapacity = maxDegreeOfParallelism
            };

            var actionBlock = new ActionBlock<T>(
                action,
                executionDataFlowOptions);

            return actionBlock;
        }

        /// <summary>
        /// Populates the request.
        /// </summary>
        /// <typeparam name="T">Request object</typeparam>
        /// <param name="logger">The logger instance.</param>
        /// <param name="req">The request.</param>
        /// <returns>
        /// Request object and user context.
        /// </returns>
        public static (T, ClaimsPrincipal) PopulateRequest<T>(ILogger logger, HttpRequest req)
        {
            try
            {
                return PopulateRequest<T>(req);
            }
            catch (Exception ex)
            {
                logger.TrackException(
                   ex,
                   $"Error in deserializing the request to [{nameof(T)}]. Message: [{ex?.Message}], Inner exception message: [{ex?.InnerException?.Message}], Stack trace: [{ex?.StackTrace}]");
                throw;
            }
        }

        /// <summary>
        /// Populates the request.
        /// </summary>
        /// <typeparam name="T">Request object</typeparam>
        /// <param name="req">The request.</param>
        /// <returns>
        /// Request object and user context.
        /// </returns>
        public static (T, ClaimsPrincipal) PopulateRequest<T>(HttpRequest req)
        {
            return PopulateRequest<T>(req, settings: null);
        }

        /// <summary>
        /// Populates the request.
        /// </summary>
        /// <typeparam name="T">Type of request body</typeparam>
        /// <param name="req">The req.</param>
        /// <param name="converters">The converters.</param>
        /// <returns>Object consisting request object and claims principal</returns>
        public static (T, ClaimsPrincipal) PopulateRequest<T>(HttpRequest req, params JsonConverter[] converters)
        {
            JsonSerializerSettings settings = (converters != null && converters.Length > 0)
                ? new JsonSerializerSettings { Converters = converters }
                : null;

            return PopulateRequest<T>(req, settings);
        }

        /// <summary>
        /// Populates the request.
        /// </summary>
        /// <typeparam name="T">Type of the request body</typeparam>
        /// <param name="req">The req.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Object consisting request object and claims principal</returns>
        public static (T, ClaimsPrincipal) PopulateRequest<T>(HttpRequest req, JsonSerializerSettings settings)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            T request = JsonConvert.DeserializeObject<T>(requestBody, settings);
            return (request, req.HttpContext.User);
        }

        /// <summary>
        /// Sets the data table parameter.
        /// </summary>
        /// <param name="paramTypeName">Name of the parameter type.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetDataTableParameter(string paramTypeName, DataTable dataTable, string paramName)
        {
            return new SqlParameter
            {
                SqlDbType = SqlDbType.Structured,
                ParameterName = paramName,
                TypeName = paramTypeName,
                Value = dataTable
            };
        }

        /// <summary>
        /// Sets the custom parameter.
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="paramObject">The parameter object.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetCustomOutputParameter<T>(T paramObject, string paramName)
        {
            return new SqlParameter
            {
                SqlDbType = object.Equals(paramObject.GetType(), typeof(bool)) ? SqlDbType.Bit : SqlDbType.Structured,
                ParameterName = paramName,
                Direction = ParameterDirection.Output,
                Value = paramObject.DbNullIfNull() ? Convert.DBNull : paramObject
            };
        }

        /// <summary>
        /// DbNullIfNull.
        /// </summary>
        /// <param name="obj">The parameter object.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static bool DbNullIfNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Sets the company identifier parameter.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetCompanyIdParameter(int companyId, string paramName = "@CompanyId")
        {
            return new SqlParameter(paramName, companyId);
        }

        /// <summary>
        /// Sets the identifier parameter.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetIdParameter(object companyId, string paramName = "@Id")
        {
            return new SqlParameter(paramName, companyId);
        }

        /// <summary>
        /// Sets the user identifier parameter.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetUserIdParameter(object userId, string paramName = "@UserId")
        {
            return new SqlParameter(paramName, userId);
        }

        /// <summary>
        /// Sets the custom parameter.
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="paramObject">The parameter object.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetCustomParameter<T>(T paramObject, string paramName)
        {
            return new SqlParameter(paramName, paramObject == null ? Convert.DBNull : paramObject);
        }

        /// <summary>
        /// Sets the boolean parameter.
        /// </summary>
        /// <param name="paramObj">if set to <c>true</c> [parameter object].</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <returns>
        /// SQL Parameter
        /// </returns>
        public static SqlParameter SetBooleanParameter(bool paramObj, string paramName)
        {
            return new SqlParameter(paramName, paramObj);
        }

        /// <summary>
        /// Formats the date
        /// </summary>
        /// <param name="dateValue">Date string to be formatted</param>
        /// <param name="dateDelimiter">Delimiter of the date string</param>
        /// <returns>
        /// Formatted date string
        /// </returns>
        public static string FormatDate(string dateValue, string dateDelimiter = "/")
        {
            if (string.IsNullOrWhiteSpace(dateValue))
            {
                return dateValue;
            }

            string[] dateArr = dateValue.Split(dateDelimiter);
            int.TryParse(dateArr[0], out int month);
            int.TryParse(dateArr[1], out int day);
            return string.Concat(
                month < 10 ? string.Concat(0, month) : month.ToString(),
                dateDelimiter,
                day < 10 ? string.Concat(0, day) : day.ToString(),
                dateDelimiter,
                dateArr[2]);
        }

        /// <summary>
        /// Gets the stored procedure repository
        /// </summary>
        /// <param name="serviceProvider">service provider</param>
        /// <param name="typeOfDb">DB type</param>
        /// <returns>
        /// StoredProcedure object
        /// </returns>
        public static IStoredProcedureRepository GetStoredProcedureRepository(IServiceProvider serviceProvider, string typeOfDb)
        {
            var values = serviceProvider.GetServices<IStoredProcedureRepository>();
            var repoType = values.Where(a => a.GetType() == typeof(StoredProcedureRepository));
            return repoType.First(x => ((StoredProcedureRepository)x).DbType == typeOfDb);
        }

        /// <summary>
        /// Convert string date to proper dateTime format.
        /// </summary>
        /// <param name="date">The string date.</param>
        /// <returns>The dateTime</returns>
        public static DateTime ConvertStringToDateTimeFormat(string date)
        {
            string formattedDateString = date?.Insert(4, "-").Insert(7, "-");
            DateTime.TryParse(formattedDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime formattedDate);
            return formattedDate;
        }

        /// <summary>
        /// Parses the invalid exception.
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="ex">The exception.</param>
        /// <returns>
        /// Exception Custom Response
        /// </returns>
        public static ApiResponse<T> ParseInvalidOperationException<T>(InvalidOperationException ex)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = ex.Message,
                Success = false,
            };
        }

        /// <summary>
        /// Adds the authentication header asynchronous.
        /// </summary>
        /// <param name="tokenHelper">The token helper.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>Authorization Headers</returns>
        private static async Task<IDictionary<string, string>> AddAuthenticationHeaderAsync(ITokenHelper tokenHelper, IDictionary<string, string> headers, string resource)
        {
            ValidateHeaders(headers);
            headers.Add("Authorization", $"Bearer {await tokenHelper.GetMsiTokenAsync(resource).ConfigureAwait(false)}");
            return headers;
        }

        /// <summary>
        /// Validates the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        private static void ValidateHeaders(IDictionary<string, string> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }
        }
    }
}
