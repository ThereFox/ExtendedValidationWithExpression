#region

using CSharpFunctionalExtensions;
using ExtendedValidation.Interfaces;

#endregion

namespace ExtendedValidation;

public class Rule<TValidated, ElementT> : IRule<TValidated>
{
    private readonly Predicate<TValidated> applyCondition;
    private readonly Func<TValidated, Result> invalidAction;
    private readonly Predicate<ElementT> propertyCondition;
    private readonly Func<TValidated, ElementT> propertySelector;

    public Result Check(TValidated value)
    {
        if (applyCondition is not null && applyCondition(value) == false) return Result.Success();

        var propertyValue = propertySelector(value);

        if (propertyCondition(propertyValue) == false) return invalidAction(value);

        return Result.Success();
    }
}