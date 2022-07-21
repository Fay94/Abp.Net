using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Net.Trees.Dto
{
    public class MyOrganizationUnitDto
    {
        public Guid? TenantId { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public Guid? ParentId { get; set; }
        public MyOrganizationUnitDto Parent { get; set; }
        public ICollection<MyOrganizationUnitDto> Children { get; set; }
        public string DisplayName { get; set; }
    }

    public class CreateOrganizationUnitDto1
    {

    }
}
