namespace ExtendedValidation.Interfaces;

public interface IRuleBuilder<TValidated>
{
    public IElementRuleBuilder<TValidated, ValidatedType> Element<ValidatedType>(Func<TValidated, ValidatedType> predicate);
    public IRuleBuilder<TValidated> If(Predicate<TValidated> predicate);
}