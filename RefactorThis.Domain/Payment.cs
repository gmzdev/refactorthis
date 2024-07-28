using System;

namespace RefactorThis.Domain
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