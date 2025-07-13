namespace ExtendedValidation.Interfaces;

public interface IInvalidActionBuilder<TValidated>
{
    public void ReturnError();
    public void ReturnError(string message);
    public void ModifyRequest(Action<TValidated> modifyAction);
}