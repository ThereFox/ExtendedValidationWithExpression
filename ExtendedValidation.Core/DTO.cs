#region

using System.Linq.Expressions;

#endregion

namespace ExtendedValidation;

public record DTO(
    Expression applyCondition,
    Expression propertySelector,
    Expression propertyCondition,
    Expression invalidAction
);