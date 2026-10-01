using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly TARge25ShopContext _context;

        public KindergartenServices(TARge25ShopContext context)
        {
            _context = context;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            var kindergarten = new Kindergarten
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount,
                KindergartenName = dto.KindergartenName,
                TeacherName = dto.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (kindergarten == null)
            {
                throw new KeyNotFoundException("Kindergarten not found.");
            }

            return kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            if (!dto.Id.HasValue)
            {
                throw new ArgumentException("Kindergarten ID is required.");
            }

            var kindergarten = await DetailAsync(dto.Id.Value);

            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var kindergarten = await DetailAsync(id);

            _context.Kindergartens.Remove(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }
    }
}