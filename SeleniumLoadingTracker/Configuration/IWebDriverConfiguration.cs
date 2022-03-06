// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWebDriverConfiguration.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace SeleniumLoadingTracker.Configuration;

public interface IWebDriverConfiguration
{
	/// <summary>
	/// Headless mode is used for CI runs without an actual display
	/// </summary>
	bool Headless { get; set; }

	/// <summary>
	/// Use verbose output from browser, helps with debugging
	/// </summary>
	bool Verbose { get; set; }
}