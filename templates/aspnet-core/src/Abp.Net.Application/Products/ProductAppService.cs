using System.Threading.Tasks;
using Abp.Net.Products.Dtos;
namespace Abp.Net.Products
{
    public class ProductAppService : NetAppService, IProductAppService
    {
        public virtual Task<CreateOutput> CreateAsync(CreateInput input)
        {
             throw new System.NotImplementedException();
        }

        public virtual Task<UpdateOutput> UpdateAsync(UpdateInput input)
        {
             throw new System.NotImplementedException();
        }

        public virtual Task<DeleteOutput> DeleteAsync(DeleteInput input)
        {
             throw new System.NotImplementedException();
        }
    }
}
