# ---------- derleme ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Önce yalnızca proje dosyaları kopyalanır; kaynak değişse de
# paket geri yükleme katmanı önbellekten gelir.
COPY VarlikEnvanteri.sln ./
COPY Core/Core.csproj              Core/
COPY Dto/Dto.csproj                Dto/
COPY Entity/Entity.csproj          Entity/
COPY Repository/Repository.csproj  Repository/
COPY Business/Business.csproj      Business/
COPY Util/Util.csproj              Util/
COPY Web/Web.csproj                Web/
RUN dotnet restore VarlikEnvanteri.sln

COPY . .
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish --no-restore

# ---------- çalıştırma ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# libgssapi-krb5-2: Npgsql açılışta GSSAPI/Kerberos kimlik doğrulamasını yoklar.
#   Kütüphane yoksa şifre doğrulamasına düşer ama her başlangıçta stderr'e hata basar.
# curl: .NET çalışma zamanı imajında hiçbir HTTP istemcisi yoktur; sağlık
#   kontrolünün /health ucunu çağırabilmesi için gereklidir.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# /app root'a aittir ve uygulama root olmayan kullanıcıyla çalışır. Yazılması
# gereken şeyler (istatistik dosyası, DataProtection anahtarları) bu ayrı
# dizine gider; docker-compose bunu kalıcı bir hacme bağlar.
RUN mkdir -p /var/lib/varlik && chown -R $APP_UID:$APP_UID /var/lib/varlik

# .NET imajları root olmayan hazır bir kullanıcı sağlar ($APP_UID).
USER $APP_UID

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    UygulamaAyarlari__VeriDizini=/var/lib/varlik

EXPOSE 8080

ENTRYPOINT ["dotnet", "Web.dll"]
