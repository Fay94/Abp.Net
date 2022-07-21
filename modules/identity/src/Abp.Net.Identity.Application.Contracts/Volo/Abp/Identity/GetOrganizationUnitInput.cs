using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class GetOrganizationUnitInput : PagedAndSortedResultRequestDto
    {
        public Guid? ParentId { get; set; }

        public string Filter { get; set; }
    }
}
