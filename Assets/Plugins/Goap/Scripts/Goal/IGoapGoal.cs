namespace AI.Goap
{
    public interface IGoapGoal
    {
        string Name { get; }
        bool IsValid { get; }
        int Priority { get; }
        
        LocalState Result { get; } //KeyValuePair<string, bool>[]
    }
}