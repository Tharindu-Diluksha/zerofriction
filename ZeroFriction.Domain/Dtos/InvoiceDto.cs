namespace ZeroFriction.Domain.Dtos
{
    public class InvoiceDto : BaseDto
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public float TotalAmount { get; set; }

        public List<InvoiceLineDto> InvoiceLines { get; set; } = new List<InvoiceLineDto>();
    }

    public class InvoiceLineDto
    {
        public float Amount { get; set; }

        public float Quantity { get; set; }

        public float UnitPrice { get; set; }

        public float LineAmount { get; set; }
    }
}
