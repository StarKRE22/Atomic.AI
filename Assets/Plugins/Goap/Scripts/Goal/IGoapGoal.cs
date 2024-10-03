namespace AI.Goap
{
    public interface IGoapGoal
    {
        string Name { get; }
        
        LocalState Result { get; }
        bool IsValid { get; }
        int Priority { get; }
    }
}