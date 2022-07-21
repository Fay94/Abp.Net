//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Volo.Abp.EntityFrameworkCore;
//using Volo.Abp.Identity;
//using Volo.Abp.Identity.EntityFrameworkCore;
//using System.Linq.Dynamic.Core;
//using Volo.Abp.DependencyInjection;

//namespace Abp.Net.Identity.EntityFrameworkCore
//{
//    [Dependency(ReplaceServices = true)]
//    [ExposeServices(typeof(IOrganizationUnitRepository), typeof(EfCoreOrganizationUnitRepository), typeof(MyEfCoreOrganizationUnitRepository))]
//    public class MyEfCoreOrganizationUnitRepository : EfCoreOrganizationUnitRepository
//    {
//        public MyEfCoreOrganizationUnitRepository(IDbContextProvider<Volo.Abp.Identity.EntityFrameworkCore.IIdentityDbContext> dbContextProvider)
//            : base(dbContextProvider)
//        {

//        }

//        public virtual async Task<long> GetCountAsync(
//            string filter = null,
//            CancellationToken cancellationToken = default)
//        {
//            return await (await GetDbSetAsync())
//                .WhereIf(!filter.IsNullOrWhiteSpace(),
//                    x => x.DisplayName.Contains(filter))
//                .LongCountAsync(GetCancellationToken(cancellationToken));
//        }

//        public virtual async Task<List<OrganizationUnit>> GetListAsync(
//            string sorting = null,
//            int maxResultCount = int.MaxValue,
//            int skipCount = 0,
//            string filter = null,
//            bool includeDetails = true,
//            CancellationToken cancellationToken = default)
//        {
//            return await (await GetDbSetAsync())
//                .IncludeDetails(includeDetails)
//                .WhereIf(!filter.IsNullOrWhiteSpace(),
//                        x => x.DisplayName.Contains(filter))
//                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(OrganizationUnit.Code) : sorting)
//                .PageBy(skipCount, maxResultCount)
//                .ToListAsync(GetCancellationToken(cancellationToken));
//        }
//    }
//}
