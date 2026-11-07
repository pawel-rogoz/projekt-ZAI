namespace IoTAnalyzer.UseCases.AuthSessions;

public record AuthSessionDto(long SessionId, int UserId, DateTime ExpiresAt);
