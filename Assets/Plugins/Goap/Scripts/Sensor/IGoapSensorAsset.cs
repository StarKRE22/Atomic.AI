namespace AI.Goap
{
    public interface IGoapSensorAsset<in TSource>
    {
        IGoapSensor Create(TSource source);
    }
}