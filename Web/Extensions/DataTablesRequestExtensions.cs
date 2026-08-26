using Dto.DTOs;

namespace Web.Extensions;

/// <summary>
/// DataTables'ın sunucu tarafı isteği düz form alanları olarak gelir
/// (<c>columns[0][data]</c> gibi). Model bağlayıcı bu biçimi karşılamadığı için
/// istek elle okunur.
/// </summary>
public static class DataTablesRequestExtensions
{
    /// <summary>Sayfa boyutu için üst sınır: "tümünü getir" isteği tabloyu kilitlemesin.</summary>
    private const int EnFazlaSatir = 500;

    public static DataTablesRequest DataTablesIstegiOku(this HttpRequest request)
    {
        var form = request.Form;

        var istek = new DataTablesRequest
        {
            Draw = SayiOku(form["draw"]),
            Start = Math.Max(0, SayiOku(form["start"])),
            Length = Uzunluk(SayiOku(form["length"])),
            Searchs = new DataTablesRequest.Search
            {
                Value = form["search[value]"].FirstOrDefault() ?? string.Empty
            },
            Columns = [],
            Orders = []
        };

        for (var i = 0; form.ContainsKey($"columns[{i}][data]"); i++)
        {
            istek.Columns.Add(new DataTablesRequest.Column
            {
                Data = form[$"columns[{i}][data]"].FirstOrDefault() ?? string.Empty,
                Name = form[$"columns[{i}][name]"].FirstOrDefault() ?? string.Empty,
                Searchable = BayrakOku(form[$"columns[{i}][searchable]"]),
                Orderable = BayrakOku(form[$"columns[{i}][orderable]"]),
                Search = new DataTablesRequest.Search
                {
                    Value = form[$"columns[{i}][search][value]"].FirstOrDefault() ?? string.Empty
                }
            });
        }

        for (var i = 0; form.ContainsKey($"order[{i}][column]"); i++)
        {
            istek.Orders.Add(new DataTablesRequest.Order
            {
                Column = SayiOku(form[$"order[{i}][column]"]),
                Dir = form[$"order[{i}][dir]"].FirstOrDefault() ?? "asc"
            });
        }

        return istek;
    }

    private static int SayiOku(Microsoft.Extensions.Primitives.StringValues deger) =>
        int.TryParse(deger.FirstOrDefault(), out var sonuc) ? sonuc : 0;

    private static bool BayrakOku(Microsoft.Extensions.Primitives.StringValues deger) =>
        bool.TryParse(deger.FirstOrDefault(), out var sonuc) && sonuc;

    /// <summary>Sayfa boyutu 0 ya da -1 gelirse (DataTables "tümü" der) sınıra çekilir.</summary>
    private static int Uzunluk(int deger) =>
        deger is <= 0 or > EnFazlaSatir ? EnFazlaSatir : deger;
}
