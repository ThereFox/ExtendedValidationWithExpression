namespace ExtendedValidation.Interfaces;

public interface IElementRuleBuilder<TValidated, ElementType>
{
    public IDefinedElementRuleBuilder<TValidated, ElementType> Must(Predicate<ElementType> predicate);
}