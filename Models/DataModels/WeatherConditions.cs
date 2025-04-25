namespace Models.DataModels
{
    public record WeatherConditions
    {
        public Temperature Temperature { get; init; }
        public Precipitation Precipitation { get; init; }
        public int HumidityPercent { get; init; }
        public int WindSpeedKmh { get; init; }
        public PitchCondition Pitch { get; init; }
    }

    public enum Temperature
    {
        BelowZero,
        ZeroToTen,
        ElevenToTwenty,
        AboveTwenty
    }

    public enum Precipitation
    {
        None,
        LightRain,
        HeavyRain,
        Snow
    }

    public enum PitchCondition
    {
        Perfect,
        Wet,
        Frozen,
        Patchy
    }
}
