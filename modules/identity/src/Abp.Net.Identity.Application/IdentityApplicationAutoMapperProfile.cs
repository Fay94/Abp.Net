using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Abp.Net.Identity;

public class IdentityApplicationAutoMapperProfile : Profile
{
    public IdentityApplicationAutoMapperProfile()
    {
        //CreateMap<OrganizationUnit, OrganizationUnitDto>()
        //    .MapExtraProperties();

        CreateMap<IdentityUserCreateOrgsDto, IdentityUserCreateDto>();
        CreateMap<IdentityUserUpdateOrgsDto, IdentityUserUpdateDto>();

        CreateMap<IdentityRoleCreateOrgDto, IdentityRoleCreateDto>();

        CreateMap<IdentityClaimType, IdentityClaimTypeDto>()
            .Ignore(x => x.ValueTypeAsString);
        //CreateMap<IdentityClaimTypeCreateDto, IdentityClaimType>()
        //    .Ignore(x => x.IsStatic).Ignore(x => x.Id);
        //CreateMap<IdentityClaimTypeUpdateDto, IdentityClaimType>()
        //    .Ignore(x => x.IsStatic).Ignore(x => x.Id);

        CreateMap<IdentityUserClaim, IdentityUserClaimDto>();
        CreateMap<IdentityUserClaimDto, IdentityUserClaim>()
            .Ignore(x => x.TenantId).Ignore(x => x.Id);

        CreateMap<IdentityRoleClaim, IdentityRoleClaimDto>();
        CreateMap<IdentityRoleClaimDto, IdentityRoleClaim>()
            .Ignore(x => x.TenantId).Ignore(x => x.Id);
    }
}