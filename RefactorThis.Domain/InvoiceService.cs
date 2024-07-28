using System;
using System.Linq;

namespace RefactorThis.Domain
{
	public class InvoiceService
	{
		private readonly IInvoiceRepository _invoiceRepository;

		public InvoiceService( IInvoiceRepository invoiceRepository )
		{
			_invoiceRepository = invoiceRepository;
		}

        public string ProcessPayment(Payment payment)
        {
            var invoice = _invoiceRepository.GetInvoice(payment.ReferenceNumber);
            if (invoice == null)
            {
                throw new InvalidOperationException("There is no invoice matching this payment");
            }

            var responseMessage = string.Empty;

            if (invoice.Amount == 0)
            {
                return HandleInvoiceWithNoAmount(invoice);
            }

            responseMessage = HandleInvoiceWithAmount(payment, invoice);
            _invoiceRepository.SaveInvoice(invoice);
            return responseMessage;
        }

        private string HandleInvoiceWithAmount(Payment payment, Invoice invoice)
        {
            if (invoice.Payments != null && invoice.Payments.Any())
            {
                return HandleInvoiceWithExistingPayment(payment, invoice);
            }

            return HandleInvoiceWithNoExistingPayment(payment, invoice);
        }

        private string HandleInvoiceWithNoExistingPayment(Payment payment, Invoice invoice)
        {
            if (payment.Amount > invoice.Amount)
            {
                return "the payment is greater than the invoice amount";
            }

            if (invoice.Amount == payment.Amount)
            {
                 ApplyPayment(invoice, payment);
                return "invoice is now fully paid";
            }

            ApplyPartialPayment(invoice, payment);
            return "invoice is now partially paid";
        }

        private string HandleInvoiceWithExistingPayment(Payment payment, Invoice invoice)
        {
            if (invoice.Payments.Sum(x => x.Amount) != 0 && invoice.Amount == invoice.Payments.Sum(x => x.Amount))
            {
                return "invoice was already fully paid";
            }
            
            if (invoice.Payments.Sum(x => x.Amount) != 0 && payment.Amount > (invoice.Amount - invoice.AmountPaid))
            {
                return "the payment is greater than the partial amount remaining";
            }
             
            return HandlePartialPayment(payment, invoice);
        }

        private static string HandlePartialPayment(Payment payment, Invoice invoice)
        {
            invoice.Payments.Add(payment);

            if ((invoice.Amount - invoice.AmountPaid) == payment.Amount)
            {
                return "final partial payment received, invoice is now fully paid";
            }

            return "another partial payment received, still not fully paid";
        }

        private string HandleInvoiceWithNoAmount(Invoice inv)
        {
            if (inv.Payments == null || !inv.Payments.Any())
            {
                return "no payment needed";
            }

            throw new InvalidOperationException("The invoice is in an invalid state, it has an amount of 0 and it has payments.");
        }

        private void ApplyPartialPayment(Invoice invoice, Payment payment)
        {
            ApplyPayment(invoice, payment);
        }

        private void ApplyPayment(Invoice invoice, Payment payment)
        {
            invoice.AddPayment(payment);
        }
    }
}