namespace IoTAnalyzer.Generator;

public static class HelpText
{
    public const string Value = """
        IoTAnalyzer.Generator - emulator czujnika wysyłający wyniki przez API (POST /api/measurements).

        Użycie:
          dotnet run -- send [parametry]     wysyłanie wyników kluczem czujnika
          dotnet run -- seed [parametry]     dane przykładowe: 3 serie z czujnikami i wynikami z przeszłości

        send:
          --api-url <url>        adres API (lub zmienna IOT_API_URL)
          --api-key <klucz>      klucz API czujnika (lub zmienna IOT_API_KEY)
          --mode history|live    history: wyniki z przeszłości, live: bieżące wyniki co --interval (domyślnie history)
          --count <n>            liczba wyników (history: domyślnie 30, live: 0 = bez końca)
          --interval <czas>      odstęp, np. 30s, 5m, 1h (history: 30m, live: 10s)
          --start <data>         history: znacznik pierwszego wyniku (domyślnie teraz minus count * interval)
          --pattern <typ>        random | sine | walk (domyślnie sine)
          --min <x> --max <y>    zakres generowanych wartości (domyślnie 0-100)
          --period <n>           sine: liczba wyników na pełny okres (domyślnie 24)
          --noise <0-1>          sine: udział szumu (domyślnie 0.1)
          --step <x>             walk: maksymalna zmiana między wynikami (domyślnie 1/20 zakresu)
          --seed <n>             ziarno generatora liczb losowych

        seed:
          --api-url <url>        adres API (lub IOT_API_URL)
          --username <login>     konto administratora (lub IOT_ADMIN_USERNAME)
          --password <hasło>     hasło administratora (lub IOT_ADMIN_PASSWORD)
          --count <n>            liczba wyników na serię (domyślnie 48)
          --interval <czas>      odstęp między wynikami (domyślnie 30m)

        Przykłady:
          dotnet run -- send --api-url http://localhost:5279 --api-key iot_... --pattern sine --min 18 --max 26 --count 20
          dotnet run -- send --api-url http://localhost:5279 --api-key iot_... --mode live --interval 5s --pattern walk
          dotnet run -- seed --api-url http://localhost:5279 --username admin --password admin-dev-password
        """;
}
