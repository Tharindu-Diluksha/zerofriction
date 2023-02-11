using ZeroFriction.DB.Domain.Documents;
using ZeroFriction.Domain.Contracts;
using ZeroFriction.Domain.Dtos;
using ZeroFriction.Domain.Exceptions;

namespace ZeroFriction.Services
{
    public class InvoiceService : IInvoiceService
    {
        public InvoiceService()
        {

        }

        #region CRUD Methods
        /// <summary>
        /// Create an invoice
        /// </summary>
        /// <param name="invoiceDto"></param>
        public async Task CreateAsync(InvoiceDto invoiceDto)
        {
            Validate(invoiceDto);

            Invoice invoice = new Invoice();
            BuildInvoice(invoiceDto, invoice);
        }

        /// <summary>
        /// Deleet an invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get an invoice by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<InvoiceDto> GetAsync(string id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Update an invoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceDto"></param>
        public async Task UpdateAsync(string id, InvoiceDto invoiceDto)
        {
            Validate(invoiceDto);

            Invoice invoice = new Invoice();
            BuildInvoice(invoiceDto, invoice);
        }
        #endregion

        #region private support methods

        /// <summary>
        /// Create the invoice DB document object with invoiceDto object data
        /// </summary>
        /// <param name="invoiceDto"></param>
        /// <param name="invoice"></param>
        private void BuildInvoice(InvoiceDto invoiceDto, Invoice invoice)
        {
            invoice.Description = invoiceDto.Description;
            invoice.TotalAmount = invoiceDto.TotalAmount;
            invoice.InvoiceLines = invoiceDto.InvoiceLines.Select(invoiceLine => new InvoiceLine
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
            if (invoiceDto.Date == DateTime.MinValue)
            {
                throw new BusinessException("ERROR_VALID_DATE_REQUIRED");
            }

            if (string.IsNullOrWhiteSpace(invoiceDto.Description))
            {
                throw new BusinessException("ERROR_VALID_DESCRIPTION_REQUIRED");
            }
        }
        #endregion
    }
}
