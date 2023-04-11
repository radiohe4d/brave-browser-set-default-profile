using BoruSoft.BraveBrowserSetDefaultProfile.Service;
using BoruSoft.BraveBrowserSetDefaultProfile.Service.Configuration;
using CliWrap;

const string ServiceName = "BoruSoft - Brave Browser Set Default Profile";

if (args is { Length: 1 })
{
	string executablePath =
		Path.Combine(AppContext.BaseDirectory, "BoruSoft.BraveBrowserSetDefaultProfile.Service.exe");

	if (args[0] is "/Install")
	{
		await Cli.Wrap("sc")
			.WithArguments(new[] { "create", ServiceName, $"binPath={executablePath}", "start=auto" })
			.ExecuteAsync();

		await Cli.Wrap("sc")
			.WithArguments(new[] { "start", ServiceName })
			.ExecuteAsync();
	}
	else if (args[0] is "/Uninstall")
	{
		try
		{
		await Cli.Wrap("sc")
			.WithArguments(new[] { "stop", ServiceName })
			.ExecuteAsync();
		}
		catch (Exception)
		{
			Console.WriteLine("Could not stop the service!");
		}

		await Cli.Wrap("sc")
			.WithArguments(new[] { "delete", ServiceName })
			.ExecuteAsync();
	}

	return;
}

IHost host = Host
	.CreateDefaultBuilder(args)
	.ConfigureSettings()
	.ConfigureServices(services =>
	{
		services.AddWindowsService(options =>
		{
			options.ServiceName = ServiceName;
		});

		services.AddHostedService<Worker>();
	})
	.Build();

host.Run();
