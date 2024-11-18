// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingCollector.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumLoadingTracker.Configuration;
using System.Text.Json;

namespace SeleniumLoadingTracker;

/// <summary>
/// Runs and stores the time measurements with a given webdriver
/// The settings for what to track are set with <see cref="ITrackingConfiguration"/>
/// </summary>
public class TrackingCollector
{
	private WebDriver _driver;
	private ITrackingConfiguration _config;

	private Dictionary<string, List<float>> _warmup;
	private Dictionary<string, List<float>> _measurements;
	private BenchmarkResult _result;

	public TrackingCollector(WebDriver driver, ITrackingConfiguration config)
	{
		_driver = driver;
		_config = config;

		_warmup = new Dictionary<string, List<float>>();
		_measurements = new Dictionary<string, List<float>>();
		_result = new BenchmarkResult
		{
			Url = config.Url,
			Timestamp = DateTime.UtcNow,
			Configuration = new BenchmarkConfiguration
			{
				WarmupRuns = config.WarmupRuns,
				MeasurementRuns = config.MeasurementRuns,
				TrackingPoints = config.TrackingPointsArray,
				WebsiteCultureCode = config.WebsiteCulture.Name
			},
			WarmupResults = new List<TrackingPointResult>(),
			MeasurementResults = new List<TrackingPointResult>()
		};
	}

	/// <summary>
	/// Runs all warmup runs. This needs to be run before <see cref="RunMeasurements"/>.
	/// Is used to do the initial download and have everything cached, so the measurements afterwards are stable.
	/// </summary>
	public void RunWarmup()
	{
		Console.WriteLine($"--- run warmup ---");
		for (int i = 0; i < _config.WarmupRuns; i++)
		{
			MeasureLoadingTime(ref _warmup);
		}
	}

	/// <summary>
	/// Runs all measurements for the website. This is the main loop to generate the loading time information
	/// </summary>
	public void RunMeasurements()
	{
		Console.WriteLine($"--- run measurements ---");
		for (int i = 0; i < _config.MeasurementRuns; i++)
		{
			MeasureLoadingTime(ref _measurements);
		}
	}

	private void MeasureLoadingTime(ref Dictionary<string, List<float>> dataPoints)
	{
		_driver.Navigate().GoToUrl(_config.Url);

		foreach (string trackingTarget in _config.TrackingPointsArray)
		{
			AddTrackPoint(trackingTarget, ref dataPoints);
		}
	}

	private void AddTrackPoint(string trackingName, ref Dictionary<string, List<float>> dataPoints)
	{
		if (!dataPoints.ContainsKey(trackingName))
		{
			dataPoints[trackingName] = new List<float>();
		}

		var time = GetTrackingTime(trackingName);
		dataPoints[trackingName].Add(time);
	}

	private float GetTrackingTime(string trackingName)
	{
		WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
		var div = wait.Until(c => c.FindElement(By.CssSelector($"#tracking #tracking-{trackingName}")));
		IWebElement definitionData = div.FindElement(By.ClassName("tracking-milliseconds"));
		string millisecondsText = definitionData.GetAttribute("textContent");
		Console.WriteLine($"Loading time {trackingName}: {millisecondsText}ms");
		if (float.TryParse(millisecondsText, NumberStyles.Float, _config.WebsiteCulture, out float milliSeconds))
		{
			return milliSeconds;
		}

		return -1f;
	}

	public void LogResults()
	{
		Console.WriteLine("------ RESULTS ------");
		Console.WriteLine("--- warmup ---");
		LogDataPoints(_warmup);

		Console.WriteLine("--- measurements ---");
		LogDataPoints(_measurements);
	}

	private void LogDataPoints(Dictionary<string, List<float>> dataPoints)
	{
		foreach (var dataPoint in dataPoints)
		{
			var trackingName = dataPoint.Key;
			var averageTime = dataPoint.Value.Average();
			Console.WriteLine($"{trackingName}: {averageTime}ms ({dataPoint.Value.Count} data points)");
		}
	}

	public void SaveResultsToJson(string filePath)
	{
		_result.WarmupResults = CreateTrackingPointResults(_warmup);
		_result.MeasurementResults = CreateTrackingPointResults(_measurements);

		var options = new JsonSerializerOptions { WriteIndented = true };
		string jsonString = JsonSerializer.Serialize(_result, options);
		File.WriteAllText(filePath, jsonString);
	}

	private List<TrackingPointResult> CreateTrackingPointResults(Dictionary<string, List<float>> dataPoints)
	{
		var results = new List<TrackingPointResult>();
		foreach (var dataPoint in dataPoints)
		{
			results.Add(new TrackingPointResult
			{
				Name = dataPoint.Key,
				AverageTime = dataPoint.Value.Average(),
				MinTime = dataPoint.Value.Min(),
				MaxTime = dataPoint.Value.Max(),
				Samples = dataPoint.Value
			});
		}
		return results;
	}
}