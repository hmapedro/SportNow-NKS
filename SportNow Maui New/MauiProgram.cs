using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace SportNow;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        Microsoft.Maui.Hosting.MauiAppBuilder builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
                fonts.AddFont("futura medium condensed bt.ttf", "futuracondensedmedium");
				//fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
        builder.Logging.AddDebug();

#endif
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjc5Mjk2OUAzMjMzMmUzMDJlMzBNdjFwbWtrYTdSVGcyZG9Lamt0NHlyNm9DUDVRRWNhbHZoTXdadXk0NERvPQ==");

        return builder.Build();
	}
}

