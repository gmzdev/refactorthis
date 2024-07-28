using System;
using NUnit.Framework;
using RefactorThis.Domain.Entities;
using RefactorThis.Persistence;

namespace RefactorThis.Domain.Tests
{
    [TestFixture]
	public class InvoicePaymentProcessorTests
	{
		[Test]
        public void ProcessPayment_Should_ThrowException_When_NoInvoiceFoundForPaymentReference()
        {
			//Arrange
            var repo = new InvoiceRepository();
            var paymentProcessor = new InvoiceService(repo);
            var failureMessage = string.Empty;

			var nonExistingInvoice = new Invoice(2, InvoiceType.Standard, "INV-001");

			// Act
            try
            {
                var payment = new Payment(1.50m, nonExistingInvoice);
                var result = paymentProcessor.ProcessPayment(payment);
            }
            catch (InvalidOperationException e)
            {
                failureMessage = e.Message;
            }

			// Assert
            Assert.AreEqual("There is no invoice matching this payment", failureMessage);
        }

        [Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded( )
		{
            // Arrange
            var repo = new InvoiceRepository();
            var invoice = new Invoice(0, InvoiceType.Standard, "INV-001");
            repo.Add(invoice);
            var paymentProcessor = new InvoiceService(repo);
            var payment = new Payment(1.50m, invoice);
            
			// Act
            var result = paymentProcessor.ProcessPayment(payment);

			// Assert
            Assert.AreEqual( "no payment needed", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
		{
			// Arrange
			var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
            var fullPayment = new Payment(10, invoice);
			invoice.AddPayment(fullPayment);
			repo.Add(invoice);
			var paymentProcessor = new InvoiceService(repo);
			var anotherPayment = new Payment(10, invoice);
            
			// Act
            var result = paymentProcessor.ProcessPayment(anotherPayment);

			// Asssert
			Assert.AreEqual("invoice was already fully paid", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
		{
			var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
			var partialPayment = new Payment(5, invoice);
			invoice.AddPayment(partialPayment);
			repo.Add(invoice);

			var paymentProcessor = new InvoiceService(repo);
			var newPayment = new Payment(6, invoice);
			var result = paymentProcessor.ProcessPayment(newPayment);

			Assert.AreEqual("the payment is greater than the partial amount remaining", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
		{
			// Arrange
			var repo = new InvoiceRepository();
			var invoice = new Invoice(5, InvoiceType.Standard, "INV-001");
			repo.Add(invoice);

			var paymentProcessor = new InvoiceService(repo);
			var newPayment = new Payment(6, invoice);

            // Act
            var result = paymentProcessor.ProcessPayment(newPayment);

			// Assert
			Assert.AreEqual("the payment is greater than the invoice amount", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
		{
            // Arrange
            var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
            var partialPayment = new Payment(5, invoice);
            invoice.AddPayment(partialPayment);
			repo.Add(invoice);
			var paymentProcessor = new InvoiceService(repo);
            var finalPayment = new Payment(5, invoice);

			// Act
            var result = paymentProcessor.ProcessPayment(finalPayment);

			// Assert
			Assert.AreEqual("final partial payment received, invoice is now fully paid", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
		{
			// Arrange
			var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
			repo.Add(invoice);
			var paymentProcessor = new InvoiceService(repo);
			var fullPayment = new Payment(10, invoice);

			// Act
			var result = paymentProcessor.ProcessPayment(fullPayment);

			// Assert
			Assert.AreEqual("invoice is now fully paid", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
		{
			// Arrange
			var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
			var partialPayment = new Payment(5, invoice);
			invoice.AddPayment(partialPayment);
			repo.Add(invoice);

			var paymentProcessor = new InvoiceService(repo);
			var anotherPartialPayment = new Payment(2, invoice);

			// Act
			var result = paymentProcessor.ProcessPayment(anotherPartialPayment);

			Assert.AreEqual("another partial payment received, still not fully paid", result);
		}

		[Test]
		public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
		{
			// Arrange
			var repo = new InvoiceRepository();
			var invoice = new Invoice(10, InvoiceType.Standard, "INV-001");
			repo.Add(invoice);
			var paymentProcessor = new InvoiceService(repo);
			var payment = new Payment(1, invoice);

			// Act
			var result = paymentProcessor.ProcessPayment(payment);

			// Assert
			Assert.AreEqual("invoice is now partially paid", result);
		}
	}
}