namespace MiningMonitor.Service.Mapper
{
    public interface IUpdateMapper<in TSource, TResult> : IMapper<TSource, TResult>
    {
        void Update(TSource source, TResult result);
    }
}