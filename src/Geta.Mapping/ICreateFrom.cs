namespace Geta.Mapping
{
    public interface ICreateFrom<in TFrom, out TTo>
    {
        TTo Create(TFrom from);
    }
}