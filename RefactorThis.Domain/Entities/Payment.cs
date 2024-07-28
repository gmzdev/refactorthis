using System;

namespace RefactorThis.Domain.Entities
{
    public class Payment
    {
        public Payment(decimal amount, Invoice invoice)
        {
            Amount = amount;
            ReferenceNumber = invoice.ReferenceNumber;
        }

        public decimal Amount { get; private set; }
        public string ReferenceNumber { get; private set; }
    }
}