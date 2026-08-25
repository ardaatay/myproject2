namespace Dto.Kullanici
{
    public class CreateKullaniciDto
    {
        public string Username { get; set; } = default!;
        public int BirimId { get; set; }
        public string BirimAd { get; set; } = default!;
        public bool Durum { get; set; }
    }
}
