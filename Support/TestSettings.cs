namespace SauceDemoBDD.Support;

public class TestSettings
{
    public string Browser { get; set; } = "chromium";
    public bool Headless { get; set; } = false;
    public int SlowMoMilliseconds { get; set; } = 0;
}