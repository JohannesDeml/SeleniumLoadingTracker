// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebDriverConfigurator.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using OpenQA.Selenium.Chrome;
using SeleniumLoadingTracker.Configuration;

namespace SeleniumLoadingTracker;

/// <summary>
/// Configures the webdriver with settings from <see cref="IWebDriverConfiguration"/>
/// </summary>
public static class WebDriverConfigurator
{
	public static ChromeDriver ConfigureChromeDriver(IWebDriverConfiguration config)
	{
		var chromeOptions = new ChromeOptions();

		if (config.Headless)
		{
			chromeOptions.AddArgument("--enable-webgl");
			chromeOptions.AddArgument("--no-sandbox");
			chromeOptions.AddArgument("--window-size=1920,1080");
			chromeOptions.AddArgument("--headless");
			chromeOptions.AddArgument("--disable-extensions");
			chromeOptions.AddArgument("--disable-dev-shm-usage");
			chromeOptions.AddArgument("--disable-setuid-sandbox");
			// This will be probably necessary for CI builds, but is not properly working
			// chromeOptions.AddArgument("--disable-gpu");
		}
			
		if (config.Verbose)
		{
			chromeOptions.AddArgument("--verbose");
		}
		else
		{
			// INFO = 0, WARNING = 1, LOG_ERROR = 2, LOG_FATAL = 3
			chromeOptions.AddArgument("--log-level=2");
		}

		return new ChromeDriver(chromeOptions);
	}
}