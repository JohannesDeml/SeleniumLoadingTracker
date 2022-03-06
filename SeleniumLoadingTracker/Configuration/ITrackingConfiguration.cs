// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITrackingConfiguration.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace SeleniumLoadingTracker.Configuration;

public interface ITrackingConfiguration
{
	/// <summary>
	/// Target url to test loading time on
	/// </summary>
	string Url { get; set; }

	string[] TrackingPointsArray { get; }

	/// <summary>
	/// Number of runs up front to load the website data
	/// </summary>
	int WarmupRuns { get; set; }

	/// <summary>
	/// Number of runs to measure the loading times after warmup
	/// </summary>
	int MeasurementRuns { get; set; }

	CultureInfo WebsiteCulture { get; }
}