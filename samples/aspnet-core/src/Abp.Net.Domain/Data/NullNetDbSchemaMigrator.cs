﻿using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Abp.Net.Data;

/* This is used if database provider does't define
 * INetDbSchemaMigrator implementation.
 */
public class NullNetDbSchemaMigrator : INetDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
