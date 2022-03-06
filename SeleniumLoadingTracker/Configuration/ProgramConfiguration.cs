// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace SeleniumLoadingTracker.Configuration;

/// <summary>
/// All configurations that can be set for this program
/// The configurations can be set through the commandline, <see cref="CommandLineUtilities"/>
/// </summary>
public class ProgramConfiguration : ITrackingConfiguration, IWebDriverConfiguration
{
	/// <summary>
	/// Target url to test loading time on
	/// </summary>
	public string Url { get; set; } = null!;

	/// <summary>
	/// Div id tracking points separated by spaces
	/// </summary>
	public string TrackingPoints { get; set; } = null!;
	public string[] TrackingPointsArray { get; private set; } = null!;

	/// <summary>
	/// Headless mode is used for CI runs without an actual display
	/// </summary>
	public bool Headless { get; set; }
		
	/// <summary>
	/// Use verbose output from browser, helps with debugging
	/// </summary>
	public bool Verbose { get; set; }

	/// <summary>
	/// Number of runs up front to load the website data
	/// </summary>
	public int WarmupRuns { get; set; }

	/// <summary>
	/// Number of runs to measure the loading times after warmup
	/// </summary>
	public int MeasurementRuns { get; set; }

	/// <summary>
	/// Culture used on the website to create the number strings
	/// </summary>
	public string WebsiteCultureCode { get; set; } = null!;
	public CultureInfo WebsiteCulture { get; private set; } = null!;

	public void Initialize()
	{
		TrackingPointsArray = TrackingPoints.Split(' ');
		WebsiteCulture = new CultureInfo(WebsiteCultureCode);
	}
}