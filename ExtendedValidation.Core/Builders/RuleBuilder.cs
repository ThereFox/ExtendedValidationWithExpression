using System.Linq.Expressions;
using CSharpFunctionalExtensions;

namespace ExtendedValidation.Interfaces;

public class RuleBuilder<TEntity> : IRuleBuilder<TEntity>, IRuleExpressionGetter
{
    private readonly List<IRuleExpressionGetter>  _elementsBuilders = new ();
    private readonly List<IRuleExpressionGetter> _conditionRulesBuilders = new ();
    
    private readonly Expression _condition;
    
    private readonly ParameterExpression _request;
    private readonly LabelTarget _end;
    
    public RuleBuilder(Expression condition, ParameterExpression request, LabelTarget end)
    {
        _condition = condition;
        _request = request;
        _end = end;
    }
    public RuleBuilder(ParameterExpression request, LabelTarget end)
    {
        _condition = Expression.Constant(true);
        _request = request;
        _end = end;
    }
    
    public IElementRuleBuilder<TEntity, ValidatedType> Element<ValidatedType>(Func<TEntity, ValidatedType> predicate)
    {
        var elementValidator = new ElementRuleBuilder<TEntity, ValidatedType>((TEntity request) => predicate(request), _request, _end);
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
        
        var subBuilders = _conditionRulesBuilders.Select(builder => builder.GetExpression()).ToArray();

        var Block = elementsExpression.Concat(subBuilders).Aggregate((first, second) => Expression.And(first, second));
        
        var ifBlock = Expression.IfThenElse(_condition, Block, Expression.Return(_end, Expression.Constant(Result.Success())));
        
        return ifBlock;
    }
}