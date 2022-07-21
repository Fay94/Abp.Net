using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Abp.Net.Users
{
    public class User : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {

    }
}
