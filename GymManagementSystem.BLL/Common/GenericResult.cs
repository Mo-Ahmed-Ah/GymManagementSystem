namespace GymManagementSystem.BLL.Common;

public record Result<T>(bool Success, T? Value, string? error = null, ResultKind Kind = ResultKind.OK)
{
    public static Result<T> Ok(T value) => new(true, value);
    public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, default, error, kind);
    public static Result<T> NotFound(string error = "Not Found") => new(false, default, error, ResultKind.NotFound);
}
