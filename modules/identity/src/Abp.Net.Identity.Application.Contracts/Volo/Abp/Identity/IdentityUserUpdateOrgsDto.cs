using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityUserUpdateOrgsDto : IdentityUserUpdateDto
    {
        [Required]
        public List<Guid> OrgIds { get; set; }
    }
}
