// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs">
//   Copyright (c) 2021 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using OpenQA.Selenium;
using SeleniumLoadingTracker.Configuration;

namespace SeleniumLoadingTracker;

public static class Program
{

	public static async Task<int> Main(string[] args)
	{
		RootCommand rootCommand = CommandLineUtilities.GenerateRootCommand();

		rootCommand.Handler = CommandHandler.Create<ProgramConfiguration>((config) =>
		{
			config.Initialize();
			Run(config);
			return 0;
		});

		return await rootCommand.InvokeAsync(args);
	}

	private static void Run(ProgramConfiguration config)
	{
		Console.WriteLine($"Running loading tracker for {config.Url}");
			
		WebDriver driver = WebDriverConfigurator.ConfigureChromeDriver(config);
		var trackingCollector = new TrackingCollector(driver, config);

		trackingCollector.RunWarmup();
		trackingCollector.RunMeasurements();
		trackingCollector.LogResults();
		driver.Quit();
	}
}