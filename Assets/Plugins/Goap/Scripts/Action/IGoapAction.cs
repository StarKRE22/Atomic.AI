namespace AI.Goap
{
    public interface IGoapAction
    {
        string Name { get; }
        
        LocalState Effects { get; }
        LocalState Conditions { get; }

        bool IsValid { get; }
        int Cost { get; }
    }
}


