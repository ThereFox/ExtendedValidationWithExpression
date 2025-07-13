using ExtendedValidation.Example.Requests;
using ExtendedValidation.Interfaces;

namespace ExtendedValidation.Example.Validators;

public class ExampleRequestValidator : IValidator<ExampleRequest>
{
    public void Validate(IRuleBuilder<ExampleRequest> ruleBuilder)
    {
        ruleBuilder
            .If(ex => ex.Condition)
            .Element(ex => ex.TestValue)
            .Must(ex => ex > 0)
            .InOtherCase()
            .ReturnError("TestValue must be greater than or equal to 0");
        
        ruleBuilder
            .If(ex => ex.Condition)
            .Element(ex => ex.TestValue)
            .Must(ex => ex > 10)
            .InOtherCase()
            .ReturnError("TestValue must be greater than or equal to 10");
    }
}