using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        private static RealEstateDto NewDto() => new()
        {
            Area = 120.5,
            Location = "Test location",
            RoomNumber = 3,
            BuildingType = "house",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };

        // Create: запись создаётся и возвращается
        [Fact]
        public async Task Should_AddRealEstate_WhenReturnResult()
        {
            var result = await Svc<IRealEstateServices>().Create(NewDto());

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
        }

        // DetailAsync: находит созданную запись по Id
        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            var created = await Svc<IRealEstateServices>().Create(NewDto());

            var result = await Svc<IRealEstateServices>().DetailAsync((Guid)created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Test location", result.Location);
        }

        // DetailAsync: такого Id в базе нет -> null
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenIdDoesNotExist()
        {
            var result = await Svc<IRealEstateServices>().DetailAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        // Delete: после удаления DetailAsync возвращает null
        [Fact]
        public async Task Should_DeleteRealEstate_WhenDetailReturnsNull()
        {
            var created = await Svc<IRealEstateServices>().Create(NewDto());

            await Svc<IRealEstateServices>().Delete((Guid)created.Id);
            var result = await Svc<IRealEstateServices>().DetailAsync((Guid)created.Id);

            Assert.Null(result);
        }
    }
}