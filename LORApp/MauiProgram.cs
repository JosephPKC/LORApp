using Microsoft.Extensions.Logging;

using LORApp.Views.Listing;
using LORApp.UseCases.Listing;
using LORApp.UseCases.Services;
using LORApp.Services.CardRepo;
using LORApp.Adapters.Sqlite;
using LORApp.Services.CardRepo.Mappers;

namespace LORApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = "CardDb.db";
            IRepo dbRepo = new SqliteAdapter(dbPath);
            ICardMapperManager cardMapper = new CardMapperManager();
            ICardGateway cardGateway = new CardRepository(dbRepo, cardMapper);

            builder.Services.AddSingleton<IListCards>(new ListCardsUseCase(cardGateway));
            builder.Services.AddSingleton<CardListingViewModel>();
            builder.Services.AddSingleton<CardListingPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
