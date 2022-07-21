using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    [RemoteService(IsEnabled = false)]
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityRoleAppService),
        typeof(IdentityRoleAppService),
        typeof(IMyIdentityRoleAppService),
        typeof(MyIdentityRoleAppService))]
    public class MyIdentityRoleAppService : IdentityRoleAppService, IMyIdentityRoleAppService
    {
        protected OrganizationUnitManager OrgManager { get; }

        public MyIdentityRoleAppService(
            IdentityRoleManager roleManager,
            IIdentityRoleRepository roleRepository,
            OrganizationUnitManager orgManager) : base(roleManager, roleRepository)
        {
            OrgManager = orgManager;
        }

        [Authorize(MyIdentityPermissions.Roles.AddOrganizationUnitRole)]
        public Task AddToOrganizationUnitAsync(Guid roleId, Guid ouId)
        {
            return OrgManager.AddRoleToOrganizationUnitAsync(roleId, ouId);
        }

        [Authorize(IdentityPermissions.Roles.Create)]
        [Authorize(MyIdentityPermissions.Roles.AddOrganizationUnitRole)]
        public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateOrgDto input)
        {
            var role = await base.CreateAsync(
                ObjectMapper.Map<IdentityRoleCreateOrgDto, IdentityRoleCreateDto>(input)
            );
            if (input.OrgId.HasValue)
            {
                await OrgManager.AddRoleToOrganizationUnitAsync(role.Id, input.OrgId.Value);
            }
            return role;
        }
    }
}