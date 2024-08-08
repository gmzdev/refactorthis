using System;

namespace RefactorThis.Domain.Entities
{
    public class Payment
    {
        public Payment(decimal amount, Invoice invoice)
        {
            if(amount == 0)
            {
                throw new ArgumentException("Zero payment is not allowed");
            }

            if (invoice == null)
            {
                throw new ArgumentNullException("Payment should be associated a valid invoice");
            }

            Amount = amount;
            ReferenceNumber = invoice.ReferenceNumber;
        }

        public decimal Amount { get; private set; }
        public string ReferenceNumber { get; private set; }
    }
}