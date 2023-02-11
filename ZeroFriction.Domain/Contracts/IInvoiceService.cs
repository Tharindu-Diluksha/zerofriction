using ZeroFriction.Domain.Dtos;

namespace ZeroFriction.Domain.Contracts
{
    public interface IInvoiceService
    {
        Task CreateAsync(InvoiceDto invoice);

        Task DeleteAsync(string id);

        Task<InvoiceDto> GetAsync(string id);

        Task UpdateAsync(string id, InvoiceDto invoice);
    }
}
