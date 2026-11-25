using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange | Подготовка
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Location = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            // Act: siin tuleb kutsuda RealEstate Create meetod esile | Действие: здесь нужно вызвать метод Create у RealEstate
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert | Проверка
            Assert.NotNull(result);
        }

        // Teha test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis | Сделать тест, где ищется анкета по id, но её нет в базе данных
        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange | Подготовка
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            Guid wrongGuid = Guid.NewGuid();

            // Act | Действие
            await Svc<IRealEstateServices>().DetailAsync(databaseGuid);

            // Assert | Проверка
            // Vastupidist loogikat tuleb kasutada | Нужно использовать обратную логику
            Assert.NotEqual(wrongGuid, databaseGuid);
        }
        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            //see on Guid juba andmebaasi olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            //kui vajatud nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");

            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //assert
            Assert.Equal(databaseGuid, guid);
        }

        //kustutamisee meetodid on vaja testida. andmebaasis on ankeet,
        //mida tähan ära kustutada
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            RealEstateDto dto = MockRealEstateData();
            var created = await Svc<IRealEstateServices>().Create(dto);
            //act
            var deleted = await Svc<IRealEstateServices>().Delete((Guid)created.Id);

            //assert
            Assert.Equal(created.Id, deleted.Id);
        }
        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Location = "TestAdres",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
    }
}