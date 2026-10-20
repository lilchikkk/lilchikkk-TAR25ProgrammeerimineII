using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IBookServices
    {
        Task<Book> Create(BookDto dto);
        Task<List<Book>> GetAll();
        Task<List<Book>> GetByYear(int year);
        Task<Book?> GetByTitle(string title);
        Task<List<Book>> GetBorrowed();
        Task<Book> UpdateTitle(Guid id, string title);
        Task<Book> Delete(Guid id);
    }
}