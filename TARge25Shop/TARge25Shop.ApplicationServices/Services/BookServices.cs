using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class BookServices : IBookServices
    {
        private readonly TARge25ShopContext _context;

        public BookServices(TARge25ShopContext context)
        {
            _context = context;
        }

        public async Task<Book> Create(BookDto dto)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Author = dto.Author,
                PublishedYear = dto.PublishedYear,
                IsAvailable = true
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }

        // Raamatud ilmumisaasta järgi
        public async Task<List<Book>> GetByYear(int year)
        {
            return await _context.Books
                .Where(x => x.PublishedYear == year)
                .ToListAsync();
        }

        // Raamat pealkirja järgi
        public async Task<Book?> GetByTitle(string title)
        {
            return await _context.Books
                .FirstOrDefaultAsync(x => x.Title == title);
        }

        // Laenutuses olevad raamatud
        public async Task<List<Book>> GetBorrowed()
        {
            return await _context.Books
                .Where(x => x.IsAvailable == false)
                .ToListAsync();
        }

        public async Task<Book> UpdateTitle(Guid id, string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            book.Title = title;
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> Delete(Guid id)
        {
            var book = await _context.Books
                .Include(x => x.Loans)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            // Enne kustutamist kontrollin, kas raamat on laenutatud (ReturnDate == null)
            if (book.Loans.Any(x => x.ReturnDate == null))
            {
                throw new InvalidOperationException("Book is currently borrowed, cannot delete.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }
    }
}