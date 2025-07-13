namespace ExtendedValidation.Interfaces;

public interface IElementRuleBuilder<TValidated, ElementType>
{
    public IElementRuleBuilder<TValidated, ElementType> Must(Predicate<ElementType> predicate);
    public IInvalidActionBuilder<TValidated> InOtherCase();
}