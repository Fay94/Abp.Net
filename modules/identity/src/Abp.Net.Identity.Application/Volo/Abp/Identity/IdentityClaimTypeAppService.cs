using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    [RemoteService(false)]
    [Authorize(MyIdentityPermissions.ClaimTypes.Default)]
    public class IdentityClaimTypeAppService : IdentityAppServiceBase, IIdentityClaimTypeAppService
    {
        protected IdentityClaimTypeManager IdenityClaimTypeManager { get; }
        protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }

        public IdentityClaimTypeAppService(
            IdentityClaimTypeManager idenityClaimTypeManager,
            IIdentityClaimTypeRepository identityClaimTypeRepository)
        {
            IdenityClaimTypeManager = idenityClaimTypeManager;
            IdentityClaimTypeRepository = identityClaimTypeRepository;
        }

        public virtual async Task<IdentityClaimTypeDto> GetAsync(Guid id)
        {
            var claimType = await IdentityClaimTypeRepository.GetAsync(id);
            return MapClaimTypeToDto(claimType);
        }

        public virtual async Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input)
        {
            var count = await IdentityClaimTypeRepository.GetCountAsync(input.Filter);
            var source = await IdentityClaimTypeRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
            return new PagedResultDto<IdentityClaimTypeDto>(count, this.MapListClaimTypeToListDto(source));
        }

        [Authorize(MyIdentityPermissions.ClaimTypes.Create)]
        public virtual async Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
        {
            var identityClaimType = ObjectMapper.Map<IdentityClaimTypeCreateDto, IdentityClaimType>(input);

            input.MapExtraPropertiesTo(identityClaimType);

            var claimType = await IdenityClaimTypeManager.CreateAsync(identityClaimType);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapClaimTypeToDto(claimType);
        }

        [Authorize(MyIdentityPermissions.ClaimTypes.Update)]
        public virtual async Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
        {
            var identityClaimType = await IdentityClaimTypeRepository.GetAsync(id);

            ObjectMapper.Map(input, identityClaimType);

            input.MapExtraPropertiesTo(identityClaimType);

            var claimType = await IdenityClaimTypeManager.UpdateAsync(identityClaimType);

            return MapClaimTypeToDto(claimType);
        }

        [Authorize(MyIdentityPermissions.ClaimTypes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await IdentityClaimTypeRepository.DeleteAsync(id);
        }

        public virtual async Task<List<IdentityClaimTypeDto>> GetAllListAsync()
        {
            var claimTypes = await IdentityClaimTypeRepository.GetListAsync();
            return MapListClaimTypeToListDto(claimTypes);
        }

        protected virtual IdentityClaimTypeDto MapClaimTypeToDto(IdentityClaimType claimType)
        {
            var claimTypeDto = ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(claimType);
            claimTypeDto.ValueTypeAsString = claimTypeDto.ValueType.ToString();
            return claimTypeDto;
        }

        protected virtual List<IdentityClaimTypeDto> MapListClaimTypeToListDto(List<IdentityClaimType> claimTypes)
        {
            var list = ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(claimTypes);
            foreach (var claimTypeDto in list)
            {
                claimTypeDto.ValueTypeAsString = claimTypeDto.ValueType.ToString();
            }
            return list;
        }
    }
}