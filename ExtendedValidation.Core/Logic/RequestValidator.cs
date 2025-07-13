using CSharpFunctionalExtensions;
using ExtendedValidation.Interfaces;

namespace ExtendedValidation;

public class RequestValidator<ValidatedType> : IRequestValidator<ValidatedType>, IRequestValidator
{
    private readonly List<IRule<ValidatedType>> _rules;

    public RequestValidator(IReadOnlyList<IRule<ValidatedType>> rules)
    {
        _rules = rules.ToList();
    }
    
    public Result Validate(ValidatedType request)
    {
        var errors = _rules
            .Select(ex => ex.Check(request))
            .Where(ex => ex.IsFailure);

        if (errors.Any())
        {
            return Result.Combine(errors, " , ");
        }
        return Result.Success();
    }

    public Result Validate(object request)
    {
        if (request is ValidatedType typedRequest)
        {
            return Result.Failure("anouther request");
        }

        return Validate((ValidatedType)request);
    }
};