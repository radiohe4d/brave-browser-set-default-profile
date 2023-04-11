using BoruSoft.BraveBrowserSetDefaultProfile.Service.Settings;

namespace BoruSoft.BraveBrowserSetDefaultProfile.Service.Configuration;

public static class SettingsConfig
{
	/// <summary>
	/// Configures the specified settings from the appsettings.json.
	/// </summary>
	/// <param name="builder">The IHostBuilder.</param>
	/// <returns>
	/// An IHostBuilder.
	/// </returns>
	public static IHostBuilder ConfigureSettings(this IHostBuilder builder)
	{
		builder
			.ConfigureServices((ctx, services) =>
			{
				var serviceSettings = ctx.Configuration.GetSection(nameof(ServiceSettings));

				services.Configure<ServiceSettings>(serviceSettings);
			});

		return builder;
	}
}
