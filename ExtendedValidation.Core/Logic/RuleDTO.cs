using CSharpFunctionalExtensions;
using ExtendedValidation.Interfaces;

namespace ExtendedValidation;

public class Rule<TValidated, ElementT> : IRule<TValidated>
{
    private readonly Predicate<TValidated> applyCondition;
    private readonly Func<TValidated, ElementT> propertySelector;
    private readonly Predicate<ElementT> propertyCondition;
    private readonly Func<TValidated, Result> invalidAction;
    
    public Result Check(TValidated value)
    {
        if (applyCondition is not null && applyCondition(value) == false)
        {
            return Result.Success();
        }
        
        var propertyValue = propertySelector(value);

        if (propertyCondition(propertyValue) == false)
        {
            return invalidAction(value);
        }
        
        return Result.Success();
    }
}