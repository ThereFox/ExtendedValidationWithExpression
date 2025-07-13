using System.Linq.Expressions;

namespace ExtendedValidation;

public record DTO
(
    Expression applyCondition,
    Expression propertySelector,
    Expression propertyCondition,
    Expression invalidAction
);