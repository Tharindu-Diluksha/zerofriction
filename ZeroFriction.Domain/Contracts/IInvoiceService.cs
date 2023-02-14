using System.Threading.Tasks;
using ZeroFriction.DB.Domain.Dtos;
using ZeroFriction.Domain.Dtos;

namespace ZeroFriction.Domain.Contracts
{
    public interface IInvoiceService
    {
        Task<DocumentUpdateResultDto> CreateAsync(InvoiceDto invoice);

        Task DeleteAsync(string id);

        Task<InvoiceDto> GetAsync(string id);

        Task<DocumentUpdateResultDto> UpdateAsync(string id, InvoiceDto invoice);
    }
}
