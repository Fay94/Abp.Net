using EasyAbp.Abp.Trees;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Abp.Net
{
    public class MyOrganizationUnit : FullAuditedAggregateRoot<Guid>, IMultiTenant, ITree<MyOrganizationUnit>
    {
        public Guid? TenantId { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public Guid? ParentId { get; set; }
        public MyOrganizationUnit Parent { get; set; }
        public ICollection<MyOrganizationUnit> Children { get; set; }
        public string DisplayName { get; set; }

    }
}
