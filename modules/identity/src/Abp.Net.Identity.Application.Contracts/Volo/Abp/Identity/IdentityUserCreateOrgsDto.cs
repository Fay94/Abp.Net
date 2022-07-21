using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityUserCreateOrgsDto : IdentityUserCreateDto
    {
        [Required]
        public List<Guid> OrgIds { get; set; }
    }
}
