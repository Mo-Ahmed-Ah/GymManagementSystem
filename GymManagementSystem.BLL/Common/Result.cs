namespace GymManagementSystem.BLL.Common;

public record Result(bool Success, string? error = null, ResultKind Kind = ResultKind.OK)
{
    public static Result Ok() => new(true);
    public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);
    public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFound);
    public static Result Validation(string error) => new(false, error, ResultKind.ValidationFailed);
}
