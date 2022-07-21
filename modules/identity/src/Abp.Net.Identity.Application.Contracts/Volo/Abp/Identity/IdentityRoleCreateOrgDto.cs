using System;

namespace Volo.Abp.Identity
{
    public class IdentityRoleCreateOrgDto : IdentityRoleCreateDto
    {
        public IdentityRoleCreateOrgDto()
        {

        }

        public Guid? OrgId { get; set; }
    }
}
