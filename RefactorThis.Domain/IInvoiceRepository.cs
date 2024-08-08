using RefactorThis.Domain.Entities;

namespace RefactorThis.Domain
{
    public interface IInvoiceRepository
    {
        void SaveInvoice(Invoice invoice);
        Invoice GetInvoice(string reference);
    }
}