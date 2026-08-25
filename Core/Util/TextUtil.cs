using System.Globalization;
using System.Text;

namespace Core.Util
{
    public static class TextUtil
    {
        /// <summary>
        /// Metni başlık biçimine çevirir. Kültür verilmezse isteğin geçerli kültürü
        /// kullanılır — bu, yapılandırılmış <c>UygulamaAyarlari:Kultur</c> değeridir.
        /// Kültür duyarlılığı önemlidir: Türkçede 'i' harfinin büyüğü 'İ'dir,
        /// değişmez kültürde ise 'I' olur.
        /// </summary>
        public static string ToTitleCase(string? text, CultureInfo? culture = null)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var kultur = culture ?? CultureInfo.CurrentCulture;
            var words = text.Split(' ');
            var result = new StringBuilder(text.Length);

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (!string.IsNullOrEmpty(word))
                {
                    result.Append(char.ToUpper(word[0], kultur));

                    if (word.Length > 1)
                        result.Append(word[1..].ToLower(kultur));
                }

                if (i < words.Length - 1)
                    result.Append(' ');
            }

            return result.ToString();
        }
    }
}
