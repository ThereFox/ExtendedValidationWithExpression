using CSharpFunctionalExtensions;

namespace ExtendedValidation.Interfaces;

public interface IRequestValidator<TRequest>
{
    public Result Validate(TRequest request);
}

public interface IRequestValidator
{
    public Result Validate(object request);
}