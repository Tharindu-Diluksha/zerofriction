using Microsoft.AspNetCore.Mvc;
using ZeroFriction.DB.Domain.Dtos;
using ZeroFriction.Domain.Contracts;
using ZeroFriction.Domain.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroFriction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET api/invoice/{id}
        [HttpGet("{id}")]
        public async Task<InvoiceDto> GetAsync(string id)
        {
            return await _invoiceService.GetAsync(id);
        }

        // POST api/invoice
        [HttpPost]
        public async Task<DocumentUpdateResultDto> PostAsync([FromBody] InvoiceDto invoice)
        {
            return await _invoiceService.CreateAsync(invoice);
        }

        // PUT api/invoice/{id}
        [HttpPut("{id}")]
        public async Task<DocumentUpdateResultDto> PutAsync(string id, [FromBody] InvoiceDto invoice)
        {
            return await _invoiceService.UpdateAsync(id, invoice);
        }

        // DELETE api/invoice/{id}
        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _invoiceService.DeleteAsync(id);
        }
    }
}
