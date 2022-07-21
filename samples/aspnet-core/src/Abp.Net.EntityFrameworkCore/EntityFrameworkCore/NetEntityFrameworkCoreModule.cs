using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using EasyAbp.FileManagement.EntityFrameworkCore;
using EasyAbp.Abp.Trees.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Abp.Net.Identity.EntityFrameworkCore;

namespace Abp.Net.EntityFrameworkCore;

[DependsOn(
    typeof(NetDomainModule),
    typeof(IdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
[DependsOn(typeof(FileManagementEntityFrameworkCoreModule))]
[DependsOn(typeof(AbpTreesEntityFrameworkCoreModule))]
public class NetEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        NetEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<NetDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddDefaultTreeRepositories();//add `ITreeRepository<TEntity>` for all Entity with implement `ITree<TEntity>`
            options.TreeEntity<MyOrganizationUnit>(x => x.CodeLength = 5);//set CodeLength for each Entity(Default:5)
        });


        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also NetMigrationsDbContextFactory for EF Core tooling. */
            options.UseMySQL();
        });
    }
}
