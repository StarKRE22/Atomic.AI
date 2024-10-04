namespace AI.Goap
{
    public interface IGoapActionAsset<in TSource>
    {
        IGoapAction Create(TSource source);
    }
}