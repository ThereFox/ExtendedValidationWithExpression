#region

using System.Linq.Expressions;
using CSharpFunctionalExtensions;

#endregion

namespace ExtendedValidation.Interfaces;

public class RuleBuilder<TEntity> : IRuleBuilder<TEntity>, IRuleExpressionGetter
{
    private readonly Expression _condition;
    private readonly List<IRuleExpressionGetter> _conditionRulesBuilders = new();
    private readonly List<IRuleExpressionGetter> _elementsBuilders = new();
    private readonly LabelTarget _end;

    private readonly ParameterExpression _request;

    public RuleBuilder(Expression condition, ParameterExpression request, LabelTarget end)
    {
        _condition = condition;
        _request = request;
        _end = end;
    }

    public RuleBuilder(ParameterExpression request, LabelTarget end)
    {
        _condition = null;
        _request = request;
        _end = end;
    }

    public IElementRuleBuilder<TEntity, ValidatedType> Element<ValidatedType>(Func<TEntity, ValidatedType> predicate)
    {
        var elementValidator =
            new ElementRuleBuilder<TEntity, ValidatedType>((TEntity request) => predicate(request), _request, _end);
        _elementsBuilders.Add(elementValidator);
        return elementValidator;
    }

    public IRuleBuilder<TEntity> If(Predicate<TEntity> predicate)
    {
        var conditionBuilder = new RuleBuilder<TEntity>(
            Expression.Invoke((TEntity request) => predicate(request),
                _request), _request, _end);
        _conditionRulesBuilders.Add(conditionBuilder);
        return conditionBuilder;
    }

    public Expression GetExpression()
    {
        var elementsExpression = _elementsBuilders.Select(ex => ex.GetExpression()).ToArray();

        var subBuilders = _conditionRulesBuilders
            .Select(builder => builder.GetExpression())
            .ToArray();

        var conditionExpression = _condition;

        if (elementsExpression.Length + subBuilders.Length == 0)
            throw new InvalidCastException("Condition must have at least one element");

        if (elementsExpression.Length + subBuilders.Length > 1)
            conditionExpression = groupConditions(elementsExpression
                .Concat(subBuilders).ToArray());
        else
            conditionExpression = elementsExpression.Any() ? elementsExpression.First() : subBuilders.First();

        if (_condition != null)
            return Expression.IfThenElse(
                _condition,
                conditionExpression,
                Expression.Return(
                    _end,
                    Expression.Constant(Result.Success())
                )
            );

        return conditionExpression;
    }

    private Expression groupConditions(Expression[] conditions)
    {
        if (conditions.All(ex => ex is ConditionalExpression) == false)
            throw new InvalidCastException("is not conditional expression's");

        var guards = conditions
            .Select(ex => ex as ConditionalExpression)
            .Select(ex => (Expression)Expression.IfThen(ex.Test, ex.IfTrue));

        return Expression.Block(guards.Append(Expression.Return(_end, Expression.Constant(Result.Success()))));
    }
}