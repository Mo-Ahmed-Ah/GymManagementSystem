
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.CustomAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IsUniqueAttribute<TEntity> : ValidationAttribute
    where TEntity : GymUser, new()
{
    private readonly string _propertyName;

    public IsUniqueAttribute(string propertyName)
    {
        _propertyName = propertyName;
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null ||
            (value is string text && string.IsNullOrWhiteSpace(text)))
        {
            return ValidationResult.Success;
        }

        var property = typeof(TEntity).GetProperty(_propertyName);

        if (property is null)
        {
            throw new InvalidOperationException(
                $"Property '{_propertyName}' does not exist on {typeof(TEntity).Name}.");
        }

        var unitOfWork = validationContext.GetService(
            typeof(IUnitOfWork)) as IUnitOfWork
            ?? throw new InvalidOperationException(
                "IUnitOfWork is not registered.");

        var model = validationContext.ObjectInstance;

        int.TryParse(
            model.GetType().GetProperty("Id")?.GetValue(model)?.ToString(),
            out int id);

        bool isUnique = !unitOfWork
            .GetRepository<TEntity>()
            .GetAll()
            .AsEnumerable()
            .Any(entity =>
                Equals(property.GetValue(entity), value)
                && entity.Id != id);

        if (!isUnique)
        {
            return new ValidationResult(
                ErrorMessage ??
                $"The {_propertyName} '{value}' is already in use.");
        }

        return ValidationResult.Success;
    }
}
