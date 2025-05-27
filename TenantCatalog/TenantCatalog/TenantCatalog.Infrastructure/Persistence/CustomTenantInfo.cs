using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantCatalog.Infrastructure.Persistence
{
    public class CustomTenantInfo : TenantInfo
    {
        public int CompanyId { get; set; }
        public int IsDel { get; set; }
    }
}
