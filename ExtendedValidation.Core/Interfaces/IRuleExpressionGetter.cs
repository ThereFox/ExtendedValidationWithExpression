using System.Linq.Expressions;

namespace ExtendedValidation.Interfaces;

public interface IRuleExpressionGetter
{
    public Expression GetExpression();
}