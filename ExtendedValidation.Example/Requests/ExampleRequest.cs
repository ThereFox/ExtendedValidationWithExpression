#region

using System.Text.Json.Serialization;

#endregion

namespace ExtendedValidation.Example.Requests;

public class ExampleRequest
{
    [JsonPropertyName("value")] public int TestValue { get; set; }

    [JsonPropertyName("condition")] public bool Condition { get; set; }
}