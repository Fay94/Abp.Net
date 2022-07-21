using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;

namespace Abp.Net.Identity.Permissions;

public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var identityGroup = context.GetGroup(IdentityPermissions.GroupName);

        var ouPermission = identityGroup.AddPermission(MyIdentityPermissions.OrganitaionUnits.Default, IdentityL("Permission:OrganizationUnitManagement"));
        ouPermission.AddChild(MyIdentityPermissions.OrganitaionUnits.Create, IdentityL("Permission:Create"));
        ouPermission.AddChild(MyIdentityPermissions.OrganitaionUnits.Update, IdentityL("Permission:Edit"));
        ouPermission.AddChild(MyIdentityPermissions.OrganitaionUnits.Delete, IdentityL("Permission:Delete"));

        var userPermission = identityGroup.GetPermissionOrNull(IdentityPermissions.Users.Default);
        userPermission?.AddChild(MyIdentityPermissions.Users.DistributionOrganizationUnit, IdentityL("Permission:DistributionOrganizationUnit"));

        var rolePermission = identityGroup.GetPermissionOrNull(IdentityPermissions.Roles.Default);
        rolePermission?.AddChild(MyIdentityPermissions.Roles.AddOrganizationUnitRole, IdentityL("Permission:AddOrganizationUnitRole"));

        var claimPermission = identityGroup.AddPermission(MyIdentityPermissions.ClaimTypes.Default, IdentityL("Permission:ClaimManagement"));
        claimPermission.AddChild(MyIdentityPermissions.ClaimTypes.Create, IdentityL("Permission:Create"));
        claimPermission.AddChild(MyIdentityPermissions.ClaimTypes.Update, IdentityL("Permission:Edit"));
        claimPermission.AddChild(MyIdentityPermissions.ClaimTypes.Delete, IdentityL("Permission:Delete"));
    }

    private static LocalizableString IdentityL(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<Localization.IdentityResource>(name);
    }
}
