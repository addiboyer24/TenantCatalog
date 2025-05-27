// <copyright file="AppTemplateController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TenantCatalog.Web.API.Controllers
{
    using CDMS.CP.Platform.Common.Helpers;
    using Finbuckle.MultiTenant;
    using Finbuckle.MultiTenant.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using System.Data.Common;
    using System.Web.Http;
    using TenantCatalog.Infrastructure.Persistence;

    /// <summary>
    /// The authorization controller.
    /// </summary>
    [ApiController]
    [Route("apptemplate")]
    public class AppTemplateController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppTemplateController"/> class.
        /// </summary>
        /// <param name="tenantContext">The tenant context.</param>
        public AppTemplateController()
        {
        }

        /// <summary>
        /// The get tenant info.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet("{tenantId}")]
        public IActionResult GetTenantInfo([FromUri] string tenantId)
        {
            var tenantInfo = this.HttpContext.GetMultiTenantContext<CustomTenantInfo>().TenantInfo;

            return new OkObjectResult(tenantInfo);
        }
    }
}
