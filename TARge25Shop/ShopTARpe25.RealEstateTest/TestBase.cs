// Sõltuvuste süstimise (DI) teenuste kaasamine | Подключение сервисов внедрения зависимостей (DI)
using Microsoft.Extensions.DependencyInjection;
// Rakenduse teenuste klasside kaasamine | Подключение классов сервисов приложения
using TARge25Shop.ApplicationServices.Services;
// Teenuste liideste kaasamine | Подключение интерфейсов сервисов
using TARge25Shop.Core.ServiceInterface;
// Andmebaasi konteksti kaasamine | Подключение контекста базы данных
using TARge25Shop.Data;
// Entity Framework Core kaasamine | Подключение Entity Framework Core
using Microsoft.EntityFrameworkCore;
// EF Core hoiatuste seadistamise kaasamine | Подключение настройки предупреждений EF Core
using Microsoft.EntityFrameworkCore.Diagnostics;
// Testide makrode kaasamine | Подключение макросов для тестов
using ShopTARpe25.RealEstateTest.Macros;

namespace ShopTARpe25.RealEstateTest
{
    // Abstraktne baasklass testide jaoks ühise seadistusega | Абстрактный базовый класс для тестов с общей настройкой
    public abstract class TestBase
    {
        // Omadus teenusepakkuja hoidmiseks | Свойство для хранения провайдера сервисов
        protected IServiceProvider ServiceProvider { get; set; }

        // Konstruktor: loob DI konteineri ja seadistab teenused | Конструктор: создает DI-контейнер и настраивает сервисы
        protected TestBase()
        {
            // Loob uue teenuste kollektsiooni | Создает новую коллекцию сервисов
            var services = new ServiceCollection();
            // Registreerib teenused ja andmebaasi | Регистрирует сервисы и базу данных
            SetupServices(services);
            // Ehitab valmis teenusepakkuja | Собирает готовый провайдер сервисов
            ServiceProvider = services.BuildServiceProvider();
        }

        // Virtuaalne meetod teenuste ja testandmebaasi registreerimiseks | Виртуальный метод для регистрации сервисов и тестовой БД
        public virtual void SetupServices(IServiceCollection services)
        {
            // Seob liidese IRealEstateServices klassiga RealEstateServices | Связывает интерфейс IRealEstateServices с классом RealEstateServices
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            // Registreerib andmebaasi konteksti | Регистрирует контекст базы данных
            services.AddDbContext<TARge25ShopContext>(x =>
            {
                // Kasutab testideks kiiret mälusisest andmebaasi | Использует быструю БД в памяти (In-Memory) для тестов
                x.UseInMemoryDatabase("Test");
                // Ignoreerib mälusisese andmebaasi transaktsioonihoiatusi | Игнорирует предупреждения о транзакциях в In-Memory БД
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Registreerib kõik testmakrod | Регистрирует все тестовые макросы
            RegisterMacros(services);
        }

        // Meetod ressursside vabastamiseks pärast testi | Метод для освобождения ресурсов после теста
        public void Dispose()
        {

        }

        // Abimeetod soovitud teenuse kiireks kättesaamiseks | Вспомогательный метод для быстрого получения нужного сервиса
        protected T Svc<T>()
        {
            // Tagastab T-tüüpi teenuse eksemplari | Возвращает экземпляр сервиса типа T
            return ServiceProvider.GetService<T>();
        }

        // Privaatne meetod kõigi makrode automaatseks leidmiseks ja registreerimiseks | Приватный метод для автоматического поиска и регистрации всех макросов
        private void RegisterMacros(IServiceCollection services)
        {
            // Määrab baasliidese, mille järgi makrosid otsida | Определяет базовый интерфейс для поиска макросов
            var macroBaseType = typeof(IMacros);

            // Leiab kõik mitte-abstraktsed klassid, mis kasutavad IMacros liidest | Находит все неабстрактные классы, реализующие интерфейс IMacros
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            // Käib läbi kõik leitud makrod | Перебирает все найденные макросы
            foreach (var macro in macros)
            {
                // Registreerib iga makro ühe eksemplarina (Singleton) | Регистрирует каждый макрос в единственном экземпляре (Singleton)
                services.AddSingleton(macro);
            }
        }
    }
}