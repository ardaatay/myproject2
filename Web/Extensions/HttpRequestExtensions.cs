namespace Web.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
               request.Headers["Content-Type"].ToString().Contains("application/json") ||
               (request.Path.Value ?? "").Contains("api/");
    }
}
