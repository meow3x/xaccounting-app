using Api.Features.PurchaseOrderMaintenance;
using FluentValidation.Results;
using System.Runtime.CompilerServices;

namespace Api.Extensions;

public static class ValidationResultExtensions
{
    public static void AddError(this ValidationResult validationResult,
        string propertyName,
        AppErrorCode errorCode)
    {
        validationResult.Errors.Add(new ValidationFailure(propertyName, errorCode.ToString()));
    }

    public static void AddErrorIfNull<T>(this ValidationResult validationResult, T? element, string propertyName, AppErrorCode errorCode)
    {
        if (element == null) AddError(validationResult, propertyName, errorCode);
    }
}
