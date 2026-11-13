using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly TARge25ShopContext _context;

        public RealEstateServices
            (
                TARge25ShopContext context
            )
        {
            _context = context;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            //info kui ankeedis on pildid, siis kutsu faili salvestamine välja 
            if (dto.Files != null)
            {
                UploadFilesToDatabase(dto, realEstate);
            }


            _context.RealEstates.Add(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = dto.Id;
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = dto.CreatedAt;
            realEstate.ModifiedAt = DateTime.Now;

            UploadFilesToDatabase(dto, realEstate);

            _context.RealEstates.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var realestate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return realestate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var realestate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .ToListAsync();

            _context.FileToDatabases.RemoveRange(images);
            _context.RealEstates.Remove(realestate);
            await _context.SaveChangesAsync();

            return realestate;
        }

        public void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain)
        {
            //toimub kontroll, kas vähemalt üks fail või mitu
            if (dto.Files == null || dto.Files.Count == 0)
                return;

            foreach (var file in dto.Files)
            {
                if (file.Length == 0)
                    continue;

                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);

                var uploadedFile = new FileToDatabase
                {
                    Id = Guid.NewGuid(),
                    ImageTitle = file.FileName,
                    ImageData = memoryStream.ToArray(),
                    RealEstateId = domain.Id
                };

                _context.FileToDatabases.Add(uploadedFile);
            }
        }
    }
}