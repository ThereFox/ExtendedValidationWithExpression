namespace ExtendedValidation.Interfaces;

public interface IDefinedElementRuleBuilder<TValidated, ElementType>
{
    public IDefinedElementRuleBuilder<TValidated, ElementType> Must(Predicate<ElementType> predicate);
    public IInvalidActionBuilder<TValidated> InOtherCase();
}