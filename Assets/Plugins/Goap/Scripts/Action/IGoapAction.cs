namespace AI.Goap
{
    public interface IGoapAction
    {
        string Name { get; }
        bool IsValid { get; }
        int Cost { get; }
        
        LocalState Effects { get; }
        LocalState Conditions { get; }
    }
}


