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
        private readonly IFileServices _fileServices;

        public KindergartenController(
            TARge25ShopContext context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
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
                if (viewModel.Id.HasValue)
                {
                    viewModel.Image = await GetImagesAsync(viewModel.Id.Value);
                }

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
                UpdatedAt = viewModel.UpdatedAt,
                //failide edasiandmine dto-le
                Files = viewModel.Files
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
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await GetImagesAsync(id)
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
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await GetImagesAsync(id)
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
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await GetImagesAsync(id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            //Delete kustutab ka kõik pildid (fail + andmebaasi kirje)
            await _kindergartenServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        //ühe pildi kustutamine ükshaaval
        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid kindergartenId)
        {
            var exists = await _context.FileToApis
                .AnyAsync(x => x.Id == imageId);

            if (exists)
            {
                await _fileServices.RemoveImageFromApi(
                    new FileToApiDto { Id = imageId });
            }

            //pärast kustutamist tagasi muutmise lehele
            return RedirectToAction(nameof(Edit), new { id = kindergartenId });
        }

        private async Task<List<KindergartenImageViewModel>> GetImagesAsync(Guid id)
        {
            return await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId
                }).ToListAsync();
        }
    }
}