using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Library;

namespace TARge25Shop.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IBookServices _bookServices;
        private readonly ILoanServices _loanServices;
        private readonly IReaderServices _readerServices;

        public LibraryController(
            IBookServices bookServices,
            ILoanServices loanServices,
            IReaderServices readerServices)
        {
            _bookServices = bookServices;
            _loanServices = loanServices;
            _readerServices = readerServices;
        }

        // ---------- raamatud ----------

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await BookList(await _bookServices.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> ByYear(int year)
        {
            return await BookList(await _bookServices.GetByYear(year));
        }

        [HttpGet]
        public async Task<IActionResult> ByTitle(string title)
        {
            var book = await _bookServices.GetByTitle(title);
            var list = new List<Book>();

            if (book != null)
            {
                list.Add(book);
            }

            return await BookList(list);
        }

        [HttpGet]
        public async Task<IActionResult> Borrowed()
        {
            return await BookList(await _bookServices.GetBorrowed());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            await _bookServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTitle(Guid id, string title)
        {
            await _bookServices.UpdateTitle(id, title);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _bookServices.Delete(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------- laenutamine ----------

        [HttpPost]
        public async Task<IActionResult> Borrow(Guid bookId, Guid readerId)
        {
            try
            {
                await _loanServices.Borrow(bookId, readerId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Return(Guid bookId)
        {
            try
            {
                await _loanServices.Return(bookId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------- lugejad ----------

        [HttpGet]
        public async Task<IActionResult> Readers()
        {
            var readers = await _readerServices.GetAll();
            return View(readers.Select(MapReader).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateReader(ReaderDto dto)
        {
            await _readerServices.Create(dto);
            return RedirectToAction(nameof(Readers));
        }

        [HttpGet]
        public async Task<IActionResult> Loans(Guid readerId)
        {
            var loans = await _loanServices.GetByReader(readerId);

            var model = loans.Select(x => new LoanIndexViewModel
            {
                BookTitle = x.Book.Title,
                LoanDate = x.LoanDate,
                ReturnDate = x.ReturnDate
            }).ToList();

            return View(model);
        }

        // ---------- abi ----------

        private async Task<IActionResult> BookList(List<Book> books)
        {
            ViewBag.Readers = (await _readerServices.GetAll()).Select(MapReader).ToList();

            var model = books.Select(x => new BookIndexViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                PublishedYear = x.PublishedYear,
                IsAvailable = x.IsAvailable
            }).ToList();

            return View("Index", model);
        }

        private static ReaderIndexViewModel MapReader(Reader x)
        {
            return new ReaderIndexViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email
            };
        }
    }
}