using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ZeroFriction.DB.Domain.Constants;

namespace ZeroFriction.DB.Domain.Documents
{
    public class Invoice : DocumentBase
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("totalAmount")]
        public float TotalAmount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("invoiceLines")]
        public List<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

        public override string DocType => DocumentType.Invoice;
    }

    public class InvoiceLine
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("quantity")]
        public float Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public float UnitPrice { get; set; }

        [JsonProperty("lineAmount")]
        public float LineAmount { get; set; }
    }
}
