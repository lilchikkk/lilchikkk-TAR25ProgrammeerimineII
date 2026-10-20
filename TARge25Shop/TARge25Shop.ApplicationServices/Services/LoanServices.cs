using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class LoanServices : ILoanServices
    {
        private readonly TARge25ShopContext _context;

        public LoanServices(TARge25ShopContext context)
        {
            _context = context;
        }

        public async Task<Loan> Borrow(Guid bookId, Guid readerId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            if (!book.IsAvailable)
            {
                throw new InvalidOperationException("Book is already borrowed.");
            }

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                ReaderId = readerId,
                LoanDate = DateTime.Now,
                ReturnDate = null
            };

            book.IsAvailable = false;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        // Märgib raamatu tagastatuks ja salvestab tagastamise kuupäeva
        public async Task<Loan> Return(Guid bookId)
        {
            var loan = await _context.Loans
                .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.ReturnDate == null);

            if (loan == null)
            {
                throw new InvalidOperationException("No active loan for this book.");
            }

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return loan;
        }

        // Ühe lugeja kõik laenutused
        public async Task<List<Loan>> GetByReader(Guid readerId)
        {
            return await _context.Loans
                .Include(x => x.Book)
                .Where(x => x.ReaderId == readerId)
                .ToListAsync();
        }
    }
}