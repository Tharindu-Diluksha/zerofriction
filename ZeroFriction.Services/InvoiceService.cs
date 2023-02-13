using ZeroFriction.DB.Domain.Contracts;
using ZeroFriction.DB.Domain.Documents;
using ZeroFriction.DB.Domain.Dtos;
using ZeroFriction.Domain.Contracts;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Domain.Exceptions;

namespace ZeroFriction.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IDocumentDbService _documentDbService;
        public InvoiceService(IDocumentDbService documentDbService)
        {
            _documentDbService = documentDbService;
        }

        #region CRUD Methods
        /// <summary>
        /// Create an invoice
        /// </summary>
        /// <param name="invoiceDto"></param>
        public async Task<DocumentUpdateResultDto> CreateAsync(InvoiceDto invoiceDto)
        {
            Validate(invoiceDto);

            Invoice invoice = new()
            {
                Id = Guid.NewGuid().ToString(),        
            };
            BuildInvoice(invoiceDto, invoice);
            var result = await _documentDbService.CreateDocumentAsync(invoice.Id, invoice);
            return result;
        }

        /// <summary>
        /// Deleet an invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(string id)
        {
            await _documentDbService.DeleteDocumentAsync<Invoice>(id, id);
        }

        /// <summary>
        /// Get an invoice by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<InvoiceDto> GetAsync(string id)
        {
            Invoice invoice = await _documentDbService.GetDocumentAsync<Invoice>(id, id);
            InvoiceDto invoiceDto = new();
            BuildInvoiceDto(invoiceDto, invoice);
            return invoiceDto;
        }


        /// <summary>
        /// Update an invoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceDto"></param>
        public async Task<DocumentUpdateResultDto> UpdateAsync(string id, InvoiceDto invoiceDto)
        {
            Validate(invoiceDto);
            Invoice invoice = await _documentDbService.GetDocumentAsync<Invoice>(id, id);
            BuildInvoice(invoiceDto, invoice);

            var result = await _documentDbService.ReplaceDocumentAsync(id, id, invoice, invoiceDto.ETag);
            return result;
        }
        #endregion

        #region private support methods

        /// <summary>
        /// Create the invoice DB document object with invoiceDto object data
        /// This also calcualte the total amount using invoice line amounts
        /// </summary>
        /// <param name="invoiceDto"></param>
        /// <param name="invoice"></param>
        private void BuildInvoice(InvoiceDto invoiceDto, Invoice invoice)
        {
            invoice.Date = invoiceDto.Date;
            invoice.Description = invoiceDto.Description;
            invoice.TotalAmount = invoiceDto.InvoiceLines.Sum(l => l.LineAmount);
            invoice.InvoiceLines = invoiceDto.InvoiceLines.Select(invoiceLine => new InvoiceLine
            {
                Amount = invoiceLine.Amount,
                Quantity = invoiceLine.Quantity,
                UnitPrice = invoiceLine.UnitPrice,
                LineAmount = invoiceLine.LineAmount
            }).ToList();
        }

        /// <summary>
        /// Build the invoiceDto
        /// </summary>
        /// <param name="invoiceDto"></param>
        /// <param name="invoice"></param>
        private void BuildInvoiceDto(InvoiceDto invoiceDto, Invoice invoice)
        {
            invoiceDto.Id = invoice.Id;
            invoiceDto.ETag = invoice.ETag;
            invoiceDto.Date = invoice.Date;
            invoiceDto.Description = invoice.Description;
            invoiceDto.TotalAmount = invoice.TotalAmount;
            invoiceDto.InvoiceLines = invoice.InvoiceLines.Select(invoiceLine => new InvoiceLineDto
            {
                Amount = invoiceLine.Amount,
                Quantity = invoiceLine.Quantity,
                UnitPrice = invoiceLine.UnitPrice,
                LineAmount = invoiceLine.LineAmount
            }).ToList();
        }

        /// <summary>
        /// Validate invoiceDto object
        /// </summary>
        /// <param name="invoiceDto"></param>
        /// <exception cref="BusinessException"></exception>
        private void Validate(InvoiceDto invoiceDto)
        {
            // Validate invoice date required condition
            if (invoiceDto.Date == DateTime.MinValue)
            {
                throw new BusinessException("ERROR_VALID_DATE_REQUIRED");
            }

            // Validate description required condition
            if (string.IsNullOrWhiteSpace(invoiceDto.Description))
            {
                throw new BusinessException("ERROR_VALID_DESCRIPTION_REQUIRED");
            }

            // Validate at least one invoice line should be included condition
            if (!invoiceDto.InvoiceLines.Any())
            {
                throw new BusinessException("ERROR_AT_LEAST_ONE_INVOICELINE_REQUIRED");
            }
        }
        #endregion
    }
}
