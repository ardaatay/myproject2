namespace Dto.Kullanici
{
    public class UpdateKullaniciDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public int BirimId { get; set; }
        public string BirimAd { get; set; } = default!;
        public bool Durum { get; set; }
    }
}
