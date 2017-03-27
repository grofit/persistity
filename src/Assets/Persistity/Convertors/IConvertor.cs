namespace Persistity.Convertors
{
    public interface IConvertor
    {
        object ConvertTo(object original);
        object ConvertFrom(object converted);
    }
}