namespace WebServiceComposition.Domain
{
    public interface IAlgorithm<in T, in TConfig, out TResult>
    {
        TResult Execute(T input, TConfig config);
    }
}
