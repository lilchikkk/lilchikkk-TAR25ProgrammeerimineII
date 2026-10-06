using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;


namespace TARge25Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly TARge25ShopContext _context;

        public FileServices
            (
                IHostEnvironment webHost,
                TARge25ShopContext context
            )
        {
            _webHost = webHost;
            _context = context;
        }


        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //tuleb teha muutuja, kus on failide asukoht e kuhu hakatakse salvestama
                string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

                //kui Directoryt ei ole olemas, siis tee Directory
                Directory.CreateDirectory(uploadsFolder);

                foreach (var file in dto.Files)
                {
                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    //tuleb kaks ülevalpool olevat muutujat kombineerida üheks
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    //tuleb Domaini teha class FileToApi, 
                    //kus on muutujad Id, ExistingFilePath ja SpaceshipId
                    FileToApi path = new FileToApi
                    {
                        Id = Guid.NewGuid(),
                        ExistingFilePath = uniqueFileName,
                        SpaceshipId = domain.Id
                    };

                    //tuleb lisada context construktorisse
                    _context.FileToApis.Add(path);
                }
            }
        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            //kui soovin kustutada faili, siis pean läbi Id pildi ülesse otsima
            var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            //teha muutuja filePath, mis näitab failide asukohta
            var filePath = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload", imageId.ExistingFilePath ?? string.Empty);

            //kui fail asub selles kaustas, siis kustuta
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(imageId);
            await _context.SaveChangesAsync();

            return null;
        }

        //<List<FileToApi>> lisati sellepärast, et faile on mitu, mida kustutada
        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos)
        {
            //kui on mitu pilti, siis itereerib need ükshavaal läbi
            //ja kustutab need ära
            foreach (var dto in dtos)
            {
                //kui soovin kustutada faili, siis pean läbi Id pildi ülesse otsima
                var imageId = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                //teha muutuja filePath, mis näitab failide asukohta
                var filePath = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload", imageId.ExistingFilePath ?? string.Empty);

                //kui fail asub selles kaustas, siis kustuta
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }
    }
}