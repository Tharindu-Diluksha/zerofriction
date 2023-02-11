using Microsoft.AspNetCore.Mvc;
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

        // GET api/<InvoiceController>/5
        [HttpGet("{id}")]
        public async Task<InvoiceDto> GetAsync(string id)
        {
            return await _invoiceService.GetAsync(id);
        }

        // POST api/<InvoiceController>
        [HttpPost]
        public async Task PostAsync([FromBody] InvoiceDto invoice)
        {
            await _invoiceService.CreateAsync(invoice);
        }

        // PUT api/<InvoiceController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(string id, [FromBody] InvoiceDto invoice)
        {
            await _invoiceService.UpdateAsync(id, invoice);
        }

        // DELETE api/<InvoiceController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _invoiceService.DeleteAsync(id);
        }
    }
}
