#region

using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using ExtendedValidation.Interfaces;

#endregion

namespace ExtendedValidation.AspNet;

public class ValidateService
{
    private readonly Dictionary<Type, Delegate> _requestValidators = new();

    public void addValidator<T>(IValidator<T> validatorType)
        where T : class
    {
        var request = Expression.Parameter(typeof(T), "request");
        var end = Expression.Label(typeof(Result), "End");

        var builder = new RuleBuilder<T>(request, end);

        validatorType.Validate(builder);

        var expression = Expression.Block(builder.GetExpression(),
            Expression.Label(end, Expression.Constant(Result.Success())));

        var delegateForSave = Expression.Lambda(expression, request);

        _requestValidators.Add(typeof(T), delegateForSave.Compile());
    }


    public Result Validate(object request, Type requestType)
    {
        var validator = _requestValidators[requestType];


        var validateResult = validator.DynamicInvoke(request);

        if (validateResult is Result result) return result;

        throw new AggregateException();
    }

    public Result Validate(object request)
    {
        return Validate(request, request.GetType());
    }

    public Result Validate<T>(T value)
    {
        return Validate(value, typeof(T));
    }
}