namespace ExtendedValidation.Interfaces;

public interface IValidator<TValidated>
    where TValidated : class
{
    public void Validate(IRuleBuilder<TValidated> ruleBuilder);
}