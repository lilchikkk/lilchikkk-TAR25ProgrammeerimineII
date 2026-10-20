using TARge25Shop.Core.Domain;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface ILoanServices
    {
        Task<Loan> Borrow(Guid bookId, Guid readerId);
        Task<Loan> Return(Guid bookId);
        Task<List<Loan>> GetByReader(Guid readerId);
    }
}