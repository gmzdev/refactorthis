using RefactorThis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Persistence {
    public class InvoiceRepository : IInvoiceRepository
    {
        private List<Invoice> _invoices = new List<Invoice>();

        public Invoice GetInvoice(string referenceNumber)
        {
            var invoice = _invoices.FirstOrDefault(i => i.ReferenceNumber == referenceNumber);

            if (invoice == null)
            {
                throw new InvalidOperationException("There is no invoice matching this payment");
            }

            return invoice;
        }

        public void SaveInvoice(Invoice invoice)
        {
            var existingInvoice = this._invoices.FirstOrDefault(i => i.ReferenceNumber == invoice.ReferenceNumber);
            if (existingInvoice == null)
            {
                _invoices.Add(invoice);
            }

            existingInvoice = invoice;
        }

        public void Add(Invoice invoice)
        {
            _invoices.Add(invoice);
        }
    }
}