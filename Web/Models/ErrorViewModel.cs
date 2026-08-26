namespace Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public string? StackTrace { get; set; }

        /// <summary>
        /// Kullanıcıya gösterilen hata referansı. Yönetim tarafındaki
        /// **Hata Logları** ekranı bu kodla aranır.
        /// </summary>
        public string? HataKodu { get; set; }
    }
}
