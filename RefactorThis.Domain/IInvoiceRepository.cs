using RefactorThis.Domain.Entities;

namespace RefactorThis.Domain
{
    public interface IInvoiceRepository
    {
        void Add(Invoice invoice);
        Invoice GetInvoice(string reference);
        void SaveInvoice(Invoice invoice);
    }
}