using System;
using System.Collections.Generic;

namespace RefactorThis.Domain
{
    public class Invoice
    {
        public Invoice(decimal amount, InvoiceType type, string referenceNumber)
        {
            Amount = amount;
            Type = type;
            ReferenceNumber = referenceNumber;
        }

        public string ReferenceNumber { get; private set; }
        public decimal Amount { get; private set; }
        public decimal AmountPaid { get; private set; }
        public decimal TaxAmount { get; private set; }
        public List<Payment> Payments { get; private set; } = new List<Payment>();
        public InvoiceType Type { get; }

        public void AddPayment(Payment payment)
        {
            switch (this.Type)
            {
                case InvoiceType.Standard:
                    AmountPaid += payment.Amount;
                    Payments.Add(payment);
                    break;
                case InvoiceType.Commercial:
                    AmountPaid += payment.Amount;
                    TaxAmount += payment.Amount * 0.14m;
                    Payments.Add(payment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum InvoiceType
    {
        Standard,
        Commercial
    }
}