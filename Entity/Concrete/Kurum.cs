using Entity.Concrete.Base;

namespace Entity.Concrete;

public class Kurum : BaseListe
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}