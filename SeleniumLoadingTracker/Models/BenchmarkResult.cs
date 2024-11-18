public class BenchmarkResult
{
    public string Url { get; set; }
    public DateTime Timestamp { get; set; }
    public List<TrackingPointResult> WarmupResults { get; set; }
    public List<TrackingPointResult> MeasurementResults { get; set; }
    public BenchmarkConfiguration Configuration { get; set; }
}

public class TrackingPointResult
{
    public string Name { get; set; }
    public float AverageTime { get; set; }
    public float MinTime { get; set; }
    public float MaxTime { get; set; }
    public List<float> Samples { get; set; }
}

public class BenchmarkConfiguration
{
    public int WarmupRuns { get; set; }
    public int MeasurementRuns { get; set; }
    public string[] TrackingPoints { get; set; }
    public string WebsiteCultureCode { get; set; }
}