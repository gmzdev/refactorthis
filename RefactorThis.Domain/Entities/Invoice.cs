using System;
using System.Collections.Generic;

namespace RefactorThis.Domain.Entities
{
    public class Invoice
    {
        private const decimal TaxRate = 0.14m;

        public Invoice(decimal amount, InvoiceType type, string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentException("Invoice should have a reference number");
            }

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
            switch (Type)
            {
                case InvoiceType.Standard:
                    AmountPaid += payment.Amount;
                    Payments.Add(payment);
                    break;
                case InvoiceType.Commercial:
                    AmountPaid += payment.Amount;
                    TaxAmount += payment.Amount * TaxRate;
                    Payments.Add(payment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}