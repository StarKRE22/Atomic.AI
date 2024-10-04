namespace AI.Goap
{
    public interface IGoapGoalAsset<in TSource>
    {
        IGoapGoal Create(TSource source);
    }
}