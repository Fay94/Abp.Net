using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    [RemoteService(false)]
    [Authorize(MyIdentityPermissions.OrganitaionUnits.Default)]
    public class OrganizationUnitAppService : IdentityAppServiceBase, IOrganizationUnitAppService
    {
        protected OrganizationUnitManager OrgManager { get; }
        protected IOrganizationUnitRepository OrgRepository { get; }
        protected IIdentityUserAppService UserAppService { get; }
        protected IIdentityRoleAppService RoleAppService { get; }

        public OrganizationUnitAppService(
            OrganizationUnitManager orgManager,
            IOrganizationUnitRepository orgRepository,
            IIdentityUserAppService userAppService,
            IIdentityRoleAppService roleAppService)
        {
            OrgManager = orgManager;
            OrgRepository = orgRepository;
            UserAppService = userAppService;
            RoleAppService = roleAppService;
        }

        public virtual async Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(
                await OrgRepository.GetAsync(id)
            );
        }

        public virtual async Task<OrganizationUnitDto> GetDetailsAsync(Guid id)
        {
            var ou = await OrgRepository.GetAsync(id);
            var ouDto = ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(ou);
            await TraverseTreeAsync(ouDto, ouDto.Children);
            return ouDto;
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetRootListAsync()
        {
            var root = ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(await OrgRepository.GetChildrenAsync(null));
            await SetLeaf(root);
            return new PagedResultDto<OrganizationUnitDto>(
                root.Count,
                root
            );
        }

        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(GetOrganizationUnitInput input)
        {
            var count = await OrgRepository.GetCountAsync();
            var list = await OrgRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount);
            return new PagedResultDto<OrganizationUnitDto>(
                count,
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list)
            );
        }

        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListDetailsAsync(GetOrganizationUnitInput input)
        {
            var count = await OrgRepository.GetCountAsync();
            var list = await OrgRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount);
            var listDto = ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list);
            foreach (var ouDto in listDto)
            {
                await TraverseTreeAsync(ouDto, ouDto.Children);
            }
            return new PagedResultDto<OrganizationUnitDto>(
                count,
                listDto
            );
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync(GetAllOrgnizationUnitInput input)
        {
            var root = await GetRootListAsync();
            foreach (var ouDto in root.Items)
            {
                await TraverseTreeAsync(ouDto, ouDto.Children);
            }
            return root;
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllListDetailsAsync(GetAllOrgnizationUnitInput input)
        {
            var root = await GetRootListAsync();
            foreach (var ouDto in root.Items)
            {
                await TraverseTreeAsync(ouDto, ouDto.Children);
            }
            return root;
        }

        public Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            return OrgManager.GetNextChildCodeAsync(parentId);
        }

        public virtual async Task<List<OrganizationUnitDto>> GetChildrenAsync(Guid parentId)
        {
            var list = ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(await OrgManager.FindChildrenAsync(parentId));
            await SetLeaf(list);
            return list;
        }

        [Authorize(MyIdentityPermissions.OrganitaionUnits.Create)]
        public virtual async Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            var ou = new OrganizationUnit(
                GuidGenerator.Create(),
                input.DisplayName,
                input.ParentId,
                CurrentTenant.Id
            )
            {
            };

            input.MapExtraPropertiesTo(ou);

            await OrgManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(ou);
        }

        [Authorize(MyIdentityPermissions.OrganitaionUnits.Update)]
        public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            var ou = await OrgRepository.GetAsync(id);

            ou.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            ou.DisplayName = input.DisplayName;

            input.MapExtraPropertiesTo(ou);

            await OrgManager.UpdateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(ou);
        }

        [Authorize(MyIdentityPermissions.OrganitaionUnits.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var ou = await OrgRepository.GetAsync(id, false);
            if (ou == null)
            {
                return;
            }
            await OrgManager.DeleteAsync(id);
        }

        public async Task MoveAsync(Guid id, Guid? parentId)
        {
            var ou = await OrgRepository.GetAsync(id);
            if (ou == null)
            {
                return;
            }
            await OrgManager.MoveAsync(id, parentId);
        }

        /// <summary>
        /// 后面考虑处理存储leaf到数据库,避免这么进行处理
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual async Task SetLeaf(List<OrganizationUnitDto> list)
        {
            foreach (var item in list)
            {
                if ((await OrgRepository.GetChildrenAsync(item.Id)).Count == 0)
                {
                    item.SetLeaf();
                }
            }
        }

        protected virtual async Task TraverseTreeAsync(OrganizationUnitDto dto, List<OrganizationUnitDto> children)
        {
            if (dto.Children.Count == 0)
            {
                children = await GetChildrenAsync(dto.Id);
                dto.Children.AddRange(children);
            }
            if (children == null || !children.Any())
            {
                await Task.CompletedTask;
                return;
            }
            foreach (var child in children)
            {
                var next = await GetChildrenAsync(child.Id);
                child.Children.AddRange(next);
                await TraverseTreeAsync(dto, child.Children);
            }
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid? ouId, GetIdentityUsersInput userInput)
        {
            if (!ouId.HasValue)
            {
                return await UserAppService.GetListAsync(userInput);
            }
            IEnumerable<IdentityUser> list = new List<IdentityUser>();
            var ou = await OrgRepository.GetAsync(ouId.Value);
            var selfAndChildren = await OrgRepository.GetAllChildrenWithParentCodeAsync(ou.Code, ou.Id);
            selfAndChildren.Add(ou);
            //Consider submitting PR to get its own overloading method containing all the members of the child node
            foreach (var child in selfAndChildren)
            {
                // Find child nodes where users have duplicates (users can have multiple organizations)
                //count += await UnitRepository.GetMembersCountAsync(child, usersInput.Filter);
                list = Enumerable.Union(list, await OrgRepository.GetMembersAsync(
                       child,
                       userInput.Sorting,
                       //usersInput.MaxResultCount, // So let's think about looking up all the members of the subset
                       //usersInput.SkipCount,
                       filter: userInput.Filter
                ));
            }
            return new PagedResultDto<IdentityUserDto>(
                list.Count(),
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(
                    list.Skip(userInput.SkipCount).Take(userInput.MaxResultCount)
                    .ToList()
                )
            );
        }

        [Authorize(IdentityPermissions.Roles.Default)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid? ouId, GetIdentityRolesInput roleInput)
        {
            if (!ouId.HasValue)
            {
                return await RoleAppService.GetListAsync(roleInput);
            }
            IEnumerable<IdentityRole> list = new List<IdentityRole>();
            var ou = await OrgRepository.GetAsync(ouId.Value);
            var selfAndChildren = await OrgRepository.GetAllChildrenWithParentCodeAsync(ou.Code, ou.Id);
            selfAndChildren.Add(ou);
            foreach (var child in selfAndChildren)
            {
                list = Enumerable.Union(list, await OrgRepository.GetRolesAsync(
                       child,
                       roleInput.Sorting
                ));
            }
            return new PagedResultDto<IdentityRoleDto>(
                list.Count(),
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(
                    list.Skip(roleInput.SkipCount).Take(roleInput.MaxResultCount)
                    .ToList()
                )
            );
        }
    }
}