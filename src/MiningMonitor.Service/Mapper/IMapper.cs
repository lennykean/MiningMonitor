namespace MiningMonitor.Service.Mapper
{
    public interface IMapper<in TSource, out TResult>
    {
        TResult Map(TSource source);
    }
}