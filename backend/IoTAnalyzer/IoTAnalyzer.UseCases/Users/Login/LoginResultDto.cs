namespace IoTAnalyzer.UseCases.Users.Login;

public record LoginResultDto(string AccessToken, string TokenType, int ExpiresIn);
