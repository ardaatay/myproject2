using Util.Query;

namespace Business.Abstract;

public interface IExcelService
{
    Task<MemoryStream> GenerateExcelAgveSistem();

    Task<MemoryStream> GenerateExcelAgveSistem(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelUygulama();

    Task<MemoryStream> GenerateExcelUygulama(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelTasinabilirCihaz();

    Task<MemoryStream> GenerateExcelTasinabilirCihaz(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelIoT();

    Task<MemoryStream> GenerateExcelIoT(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelFizikselMekan();

    Task<MemoryStream> GenerateExcelFizikselMekan(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelPersonel();

    Task<MemoryStream> GenerateExcelPersonel(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelKriptografiEnvanteri();

    Task<MemoryStream> GenerateExcelKriptografiEnvanteri(
        string search);

    Task<MemoryStream> GenerateExcelBasiliBilgi();

    Task<MemoryStream> GenerateExcelBasiliBilgi(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelElektronikBilgi();

    Task<MemoryStream> GenerateExcelElektronikBilgi(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelVeritabani();

    Task<MemoryStream> GenerateExcelVeritabani(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelSurec();

    Task<MemoryStream> GenerateExcelSurec(
        string search, FilterBag filterBag);

    Task<MemoryStream> GenerateExcelRaporlama(
        string? search = null, FilterBag? filterBag = null);

    Task<MemoryStream> GenerateExcelEpostaTalepleri(string? search = null);
}