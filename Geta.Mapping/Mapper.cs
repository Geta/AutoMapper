namespace Geta.Mapping
{
    public abstract class Mapper<TFrom, TTo> : IMapper<TFrom, TTo>, ICreateFrom<TFrom, TTo>
        where TTo : new()
    {
        public virtual TTo Create(TFrom from)
        {
            var to = new TTo();
            Map(from, to);
            return to;
        }

        public abstract void Map(TFrom from, TTo to);
    }
}