#region

using CSharpFunctionalExtensions;

#endregion

namespace ExtendedValidation.Interfaces;

public interface IRule<TValidated>
{
    public Result Check(TValidated value);
}