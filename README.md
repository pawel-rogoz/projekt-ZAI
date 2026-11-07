# IoT Analyzer

## Struktura

Backend jest zbudowany w układzie Clean Architecture.
Zastosowany został jasny podział na projekty ze względu na ich odpowiedzialność.
W projekcie IotAnalyzer.Core znajdziemy klasy odpowiadające tabelom w bazie danych, jak również specyfikacje zapytań.
W IoTAnalyzer.UseCases znajdują się pliki Commands i Queries, oraz odpowiadające im handlery.
Projekt API zawiera kontrolery REST, udostępnianie "na zewnątrz aplikacji", natomiast Infrastructure zawiera przede wszystkim konfigurację 
bazy danych przy użyciu narzędzia ORM: EF Core.

## Uruchomienie lokalne

Wymagania: .NET SDK 10 i PostgreSQL (np. z Dockera).

```bash
docker compose up -d db
cd backend/IoTAnalyzer
dotnet run --project IoTAnalyzer.API
```

API startuje pod `http://localhost:5279`. Przy starcie aplikacja stosuje migracje bazy i, jeśli tabela `users` jest pusta, tworzy konto administratora.
W trybie Development dane konta i bazy pochodzą z `appsettings.Development.json` (`admin` / `admin-dev-password`).

Całość w kontenerach (API pod `http://localhost:8080`):

```bash
docker compose --profile api up -d --build
```

### Konfiguracja

| Zmienna | Opis |
| --- | --- |
| `ConnectionStrings__Default` | connection string do PostgreSQL |
| `Auth__AdminUsername`, `Auth__AdminPassword` | konto administratora tworzone przy pierwszym uruchomieniu |
| `Auth__TokenLifetimeMinutes` | czas ważności tokenu (domyślnie 60) |
| `PORT` | port HTTP |

Wzór wartości: znajduje się w pliku `.env.example`.

### Migracje

```bash
cd backend/IoTAnalyzer
dotnet tool restore
dotnet ef migrations add <Nazwa> -p IoTAnalyzer.Infrastructure -s IoTAnalyzer.API -o Persistance/Migrations
dotnet ef database update -p IoTAnalyzer.Infrastructure -s IoTAnalyzer.API
```

## Generator danych

Generator wysyła wyniki wyłącznie przez API (`POST /api/measurements` z nagłówkiem `X-API-Key`).
Klucz czujnika podaje się parametrem `--api-key` albo zmienną `IOT_API_KEY`.

```bash
cd generator/IoTAnalyzer.Generator
dotnet run -- --help
```

| Parametr | Opis |
| --- | --- |
| `--api-url` | adres API (`IOT_API_URL`) |
| `--api-key` | klucz API czujnika (`IOT_API_KEY`) |
| `--count` | liczba wyników (w trybie `live` 0 oznacza wysyłanie bez końca) |

### Dane przykładowe

Polecenie `seed` loguje się jako administrator, tworzy przez API trzy serie (temperatura, wilgotność, ciśnienie), rejestruje dla każdej czujnik i wysyła jego kluczem wyniki z przeszłości:

```bash
dotnet run -- seed --api-url https://adres-api --username admin --password <haslo> --count 48
```

Klucze utworzonych czujników są wypisywane na ekranie i można ich potem użyć w trybie `live`.

## Bezpieczeństwo

1. Hasła haszowane funkcją bcrypt
2. Dostęp do bazy wyłącznie przez narzędzie ORM EF Core.
3. Token dostępowy: 256 losowych bitów, ograniczony czas ważności, w bazie zachowywany jest wyłącznie skrót SHA-256
4. Ograniczenie dostępu do zasobów wrażliwych tylko dla administratora przy użyciu nagłówka Authorize
