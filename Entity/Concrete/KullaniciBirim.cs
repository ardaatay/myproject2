using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;

namespace Entity.Concrete;

[Table("kullanici_birimler")]
public class KullaniciBirim : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    public int KullaniciId { get; set; }
    public int BirimId { get; set; }
    public string? BirimAd { get; set; }
    public bool Durum { get; set; }
    
    public virtual Kullanici? Kullanici { get; set; }
}