using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using ExtendedValidation.Interfaces;

namespace ExtendedValidation.AspNet;

public class ValidateService
{
    private readonly Dictionary<Type, Delegate> _requestValidators = new Dictionary<Type, Delegate>();
    
    public void addValidator<T>(IValidator<T> validatorType)
    where T : class
    {
        var request = Expression.Parameter(typeof(T), "request");
        var end = Expression.Label(typeof(Result), "End");
        
        var builder = new RuleBuilder<T>(request, end);
        
        validatorType.Validate(builder);

        var expression = Expression.Block(builder.GetExpression(), Expression.Label(end, Expression.Constant(Result.Success())));
        
    }

    public Result Validate<T>(T value)
    {
        var validator = _requestValidators[typeof(T)];
        
        
        var invoker = Expression.Lambda(Expression.Invoke(validator, [Expression.Constant(value)])));
        
        var result = validator.DynamicInvoke(value);
        
        return Result.Success();
        
    }
}