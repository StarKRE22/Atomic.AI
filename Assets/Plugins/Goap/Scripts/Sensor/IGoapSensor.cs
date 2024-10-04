namespace AI.Goap
{
    public interface IGoapSensor
    {
        string Name { get; }
        
        void PopulateState(WorldState worldState);
    }
}