using Microsoft.AspNetCore.Http.HttpResults;
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
        //-----------------------------
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
        //-----------------------------
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
        //-------------------------------
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
        //--------------------------------
        //mul on kaks ankeeti loodud ja ühe kustutan ära
        //assertis võrdlen, kas need kaks on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //arrange
            RealEstateDto dto = MockRealEstateData();

            //act
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }
        //---------------------------------
        //uuendab andmeid
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //arrange
            var guid = new Guid("d3661e93-4701-40a7-9770-ef18cd3a3253");

            RealEstateDto dto = MockRealEstateData();
            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Location = "Update Adres";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //võrrelda RoomNumbrit ja kasutada DoesNotMatch
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Location.ToString(), domain.Location.ToString());
        }
        //---------------------------------
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {
            //tuleb kasutada MockRealEstateDataUpdate ja MockRealEstateData2
            //arrange 

            RealEstateDto dto = MockRealEstateData2();
            var created = await Svc<IRealEstateServices>().Create(dto);

            RealEstateDto updateDto = MockRealEstateDataUpdate();
            updateDto.Id = created.Id; 

            //act 
            var updatedResult = await Svc<IRealEstateServices>().Update(updateDto);

            //assert
            Assert.Equal(created.Id, updatedResult.Id);
            Assert.Equal(updateDto.Area, updatedResult.Area);
            Assert.Equal(updateDto.Location, updatedResult.Location);
            Assert.NotEqual(dto.Area, updatedResult.Area);
        }
        //-------------------------------
        //võtke LAN juhe ära
        //kasutame ankeeti, kus on kõik andmed on NULL
        //tuleb kasutada Create ja Update ära 
        [Fact]
        public async Task Should_UpdateRealEstate_WhenCreateUpdate()
        {
            //arrange 

            RealEstateDto dto = MockRealEstateData();
            RealEstateDto nulldto = MockRealEstateDataCreateUpdate();

            //act
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(nulldto);
            

            //assert
            Assert.NotEqual(createRealEstate.Id, result.Id);
            Assert.NotEqual(createRealEstate.Location, result.Location);
            Assert.NotEqual(createRealEstate.Area, result.Area);
            Assert.NotEqual(createRealEstate.RoomNumber, result.RoomNumber);
            Assert.DoesNotMatch(createRealEstate.RoomNumber.ToString(), result.RoomNumber.ToString());
    
        }
        // Create | Update
        private RealEstateDto MockRealEstateDataCreateUpdate()
        {
            return new RealEstateDto
            {
                Area = null,
                Location = null,
                RoomNumber = 0,
                BuildingType = null,
                CreatedAt = null,
                ModifiedAt = null,
            };
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
        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area = 50.0,
                Location = "Original Location",
                RoomNumber = 2,
                BuildingType = "Apartment",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 150.0,
                Location = "Updated Location Version 2",
                RoomNumber = 4,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
    }
}