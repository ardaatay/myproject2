using Dto.Kullanici.Enum;

namespace Dto.Kullanici
{
    public class ListKullaniciDto
    {
        public int Id { get; set; }
        public int OrganizasyonId { get; set; }
        public string Username { get; set; } = null!;
        public GirisYontemi GirisYontemi { get; set; }
        public string? ActiveDirectoryKullaniciAdi { get; set; }
        public string? AdSoyad { get; set; }
        public string? Eposta { get; set; }
        public int BirimId { get; set; }
        public string BirimAd { get; set; } = null!;
        public bool Durum { get; set; }
        public string DurumStr { get; set; } = null!;

        public string GirisYontemiStr => GirisYontemi.Ad();
    }
}
