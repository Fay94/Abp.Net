using System.Threading.Tasks;

namespace Abp.Net.Data;

public interface INetDbSchemaMigrator
{
    Task MigrateAsync();
}
