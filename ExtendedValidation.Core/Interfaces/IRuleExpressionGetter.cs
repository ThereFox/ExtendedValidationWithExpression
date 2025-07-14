#region

using System.Linq.Expressions;

#endregion

namespace ExtendedValidation.Interfaces;

public interface IRuleExpressionGetter
{
    public Expression GetExpression();
}