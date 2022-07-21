using System.Threading.Tasks;
using Abp.Net.Products.Dtos;
using Volo.Abp.Application.Services;

namespace Abp.Net.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task<CreateOutput> CreateAsync(CreateInput input);

        Task<UpdateOutput> UpdateAsync(UpdateInput input);

        Task<DeleteOutput> DeleteAsync(DeleteInput input);
    }
}
