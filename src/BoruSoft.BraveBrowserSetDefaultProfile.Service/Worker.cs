namespace BoruSoft.BraveBrowserSetDefaultProfile.Service;

using BoruSoft.BraveBrowserSetDefaultProfile.Service.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> logger;
	private readonly ServiceSettings serviceSettings;

	private const string RegistryPath = @"SOFTWARE\\Classes\\BraveHTML\\shell\\open\\command";
	private const string RegistryName = null; // (Default)
	private const string RegistryValue = @"""C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe"" --profile-directory=""{{PROFILE_NAME}}"" --single-argument %1";

	public Worker(
		ILogger<Worker> logger,
		IOptions<ServiceSettings> options)
	{
		this.logger = logger;
		
		serviceSettings = options.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("BoruSoft | Brave Browser Set Default Profile - Running...");

		if (string.IsNullOrWhiteSpace(serviceSettings.ProfileName))
		{
			logger.LogError(
				"You must provide a valid value for \"{ProfileName}\" in the appsettings.json!",
				nameof(serviceSettings.ProfileName));
			return;
		}

		if (serviceSettings.IntervalSeconds <= 0)
		{
			logger.LogError(
				"You must provide a valid value for \"{IntervalSeconds}\" in the appsettings.json!",
				nameof(serviceSettings.IntervalSeconds));
			return;
		}

		while (!stoppingToken.IsCancellationRequested)
		{
			logger.LogDebug("Checking if registry entry has changed...");

			try
			{
				RegistryKey? key = Registry.LocalMachine.OpenSubKey(RegistryPath, true);

				if (key == null)
					throw new Exception($"Could not find the registry entry. Path: {RegistryPath}");

				var newRegistryValue = RegistryValue.Replace("{{PROFILE_NAME}}", serviceSettings.ProfileName);

				if (key.GetValue(RegistryName)?.ToString() != newRegistryValue)
				{
					key.SetValue(RegistryName, newRegistryValue);

					logger.LogInformation("Updated registry entry.");
				}

				key.Close();

				await Task.Delay(serviceSettings.IntervalSeconds * 1000, stoppingToken);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Service encountered and error and has stopped!");
				throw;
			}
		}

		logger.LogInformation("Service Stopped.");
	}
}