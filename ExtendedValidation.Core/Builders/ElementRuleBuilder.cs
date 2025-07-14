#region

using System.Linq.Expressions;
using CSharpFunctionalExtensions;

#endregion

namespace ExtendedValidation.Interfaces;

public class ElementRuleBuilder<TValidated, TElement> :
    IElementRuleBuilder<TValidated, TElement>,
    IDefinedElementRuleBuilder<TValidated, TElement>,
    IRuleExpressionGetter,
    IInvalidActionBuilder<TValidated>
{
    private readonly LabelTarget _end;
    private readonly Expression _getValueExpression;

    private readonly ParameterExpression _request;

    private readonly List<Expression> _checkedExpressions = new();

    private Expression invaliActionExpression;


    public ElementRuleBuilder(Expression getValueExpression, ParameterExpression request, LabelTarget end)
    {
        _getValueExpression = getValueExpression;
        _request = request;
        _end = end;
    }

    public IInvalidActionBuilder<TValidated> InOtherCase()
    {
        return this;
    }

    public IDefinedElementRuleBuilder<TValidated, TElement> Must(Predicate<TElement> predicate)
    {
        _checkedExpressions.Add((TElement value) => predicate(value));
        return this;
    }

    public void ReturnError()
    {
        invaliActionExpression =
            Expression.Return(_end, Expression.Constant(Result.Failure("Error while validate element")));
    }

    public void ReturnError(string message)
    {
        invaliActionExpression = Expression.Return(_end, Expression.Constant(Result.Failure(message)));
    }

    public void ModifyRequest(Action<TValidated> modifyAction)
    {
        invaliActionExpression = Expression.Invoke((TValidated request) => modifyAction(request), _request);
    }

    public Expression GetExpression()
    {
        if (_checkedExpressions.Count == 0) return Expression.Return(_end, Expression.Constant(Result.Success()));

        var expressionsInvocation = _checkedExpressions.Select(ex =>
            (Expression)Expression.Invoke(ex,
                Expression.Invoke(_getValueExpression, _request)
            )
        );

        Expression expressions;

        if (expressionsInvocation.Count() == 1)
            expressions = expressionsInvocation.First();
        else
            expressions = expressionsInvocation
                .Aggregate((first, second) => Expression.And(first, second));

        var success = Expression.Return(_end, Expression.Constant(Result.Success()));

        var failureAction = invaliActionExpression ??
                            Expression.Return(_end, Expression.Constant(Result.Failure("Error")));

        return Expression.IfThen(Expression.Not(expressions), failureAction);
    }
}