using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    [RemoteService(IsEnabled = false)]
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityUserAppService),
        typeof(IdentityUserAppService),
        typeof(IMyIdentityUserAppService),
        typeof(MyIdentityUserAppService))]
    public class MyIdentityUserAppService : IdentityUserAppService, IMyIdentityUserAppService
    {
        public MyIdentityUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IOptions<IdentityOptions> identityOptions) : base(userManager, userRepository, roleRepository, identityOptions)
        {
        }

        [Authorize(IdentityPermissions.Users.Create)]
        [Authorize(MyIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateOrgsDto input)
        {
            var identity = await CreateAsync(
                ObjectMapper.Map<IdentityUserCreateOrgsDto, IdentityUserCreateDto>(input)
            );
            if (input.OrgIds != null)
            {
                await UserManager.SetOrganizationUnitsAsync(identity.Id, input.OrgIds.ToArray());
            }
            return identity;
        }

        [Authorize(MyIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task AddToOrganizationUnitsAsync(Guid userId, List<Guid> ouIds)
        {
            await UserManager.SetOrganizationUnitsAsync(userId, ouIds.ToArray());
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetListOrganizationUnitsAsync(Guid id, bool includeDetails = false)
        {
            var list = await UserRepository.GetOrganizationUnitsAsync(id, includeDetails);
            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list)
            );
        }

        [Authorize(IdentityPermissions.Users.Update)]
        [Authorize(MyIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateOrgsDto input)
        {
            var update = ObjectMapper.Map<IdentityUserUpdateOrgsDto, IdentityUserUpdateDto>(input);
            var result = await base.UpdateAsync(id, update);
            await UserManager.SetOrganizationUnitsAsync(result.Id, input.OrgIds.ToArray());
            return result;
        }
    }
}