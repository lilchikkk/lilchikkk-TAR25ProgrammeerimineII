using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.Kindergarten;

namespace TARge25Shop.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly TARge25ShopContext _context;
        private readonly IKindergartenServices _kindergartenServices;

        public KindergartenController(
            TARge25ShopContext context,
            IKindergartenServices kindergartenServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kindergartens = await _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return View(kindergartens);
        }

        [HttpGet]
        public IActionResult CreateUpdate()
        {
            return View(new KindergartenCreateUpdateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(
            KindergartenCreateUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dto = new KindergartenDto
            {
                Id = viewModel.Id,
                GroupName = viewModel.GroupName,
                ChildrenCount = viewModel.ChildrenCount,
                KindergartenName = viewModel.KindergartenName,
                TeacherName = viewModel.TeacherName,
                CreatedAt = viewModel.CreatedAt,
                UpdatedAt = viewModel.UpdatedAt
            };

            if (viewModel.Id == null)
            {
                await _kindergartenServices.Create(dto);
            }
            else
            {
                await _kindergartenServices.Update(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var kindergarten =
                await _kindergartenServices.DetailAsync(id);

            var viewModel = new KindergartenCreateUpdateViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            return View("CreateUpdate", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten =
                await _kindergartenServices.DetailAsync(id);

            var viewModel = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten =
                await _kindergartenServices.DetailAsync(id);

            var viewModel = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _kindergartenServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}