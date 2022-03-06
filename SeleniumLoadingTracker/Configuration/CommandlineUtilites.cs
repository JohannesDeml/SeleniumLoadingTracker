// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandlineUtilites.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.CommandLine;

namespace SeleniumLoadingTracker.Configuration;

/// <summary>
/// Defines the possible values that can be used from the commandline
/// Those will then be set in <see cref="ProgramConfiguration"/>
/// </summary>
public static class CommandLineUtilities
{
	public static RootCommand GenerateRootCommand()
	{
		var rootCommand = new RootCommand
		{
			new Option<string>(
				"--url",
				getDefaultValue: () => "https://deml.io/experiments/unity-webgl/2020.3.23f1",
				"Target url to test loading time on"),
			new Option<bool>(
				"--headless",
				getDefaultValue: () => false,
				"Headless mode is used for CI builds without an actual display"),
			new Option<bool>(
				"--verbose",
				getDefaultValue: () => false,
				"Use verbose output from browser, helps with debugging"),
			new Option<string>(
				"--tracking-points",
				getDefaultValue: () => "Awake Start",
				"Div id tracking points separated by spaces"),
			new Option<int>(
				"--warmup-runs",
				getDefaultValue: () => 2,
				"Number of runs up front to load the website data"),
			new Option<int>(
				"--measurement-runs",
				getDefaultValue: () => 10,
				"Number of runs to measure the loading times after warmup"),
			new Option<string>(
				"--website-culture-code",
				getDefaultValue: () => "en-US",
				"Culture used on the website to create the number strings")
		};

		rootCommand.Name = "SeleniumLoadingTracker";
		rootCommand.Description = "Tracks the creation of predefined divs in multiple runs";

		return rootCommand;
	}
}