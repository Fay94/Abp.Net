using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService(true, Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area(IdentityRemoteServiceConsts.ModuleName)]
    [ControllerName("ClaimType")]
    [Route("api/identity/claim-types")]

    public class IdentityClaimTypeController : AbpController, IIdentityClaimTypeAppService
    {
        protected IIdentityClaimTypeAppService ClaimTypeAppService { get; }

        public IdentityClaimTypeController(IIdentityClaimTypeAppService claimTypeAppService)
        {
            ClaimTypeAppService = claimTypeAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(GetIdentityClaimTypesInput input)
        {
            return this.ClaimTypeAppService.GetListAsync(input);
        }

        [Route("all")]
        [HttpGet]
        public virtual Task<List<IdentityClaimTypeDto>> GetAllListAsync()
        {
            return this.ClaimTypeAppService.GetAllListAsync();
        }

        [Route("{id}")]
        [HttpGet]
        public virtual Task<IdentityClaimTypeDto> GetAsync(Guid id)
        {
            return this.ClaimTypeAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
        {
            return this.ClaimTypeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
        {
            return this.ClaimTypeAppService.UpdateAsync(id, input);
        }

        [Route("{id}")]
        [HttpDelete]
        public virtual Task DeleteAsync(Guid id)
        {
            return this.ClaimTypeAppService.DeleteAsync(id);
        }
    }
}
