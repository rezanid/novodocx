using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Novo.AzureFunctions.OpenApi;
[OpenApiExample(typeof(WordRequestModelExample))]
public class WordRequestModel : DynamicObject
{
    private readonly Dictionary<string, object> payload = new(StringComparer.OrdinalIgnoreCase)
    {
        ["file"] = "Base64 encoded binary of a docx file.",
        ["parameters"] = 
            new
            {
                firstName = "Reza",
                lastName = "Niroomand",
                repeatingSection1 =
                    new[]
                    {
                        new
                        {
                            monthly = 1000.0,
                            remaining = 2000.0,
                        },
                        new
                        {
                            monthly = 1000.0,
                            remaining = 1000.0
                        }
                    }
            }
    };

    public override bool TryGetMember(GetMemberBinder binder, out object result) =>
        payload.TryGetValue(binder.Name, out result);

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        if (!payload.ContainsKey(binder.Name)) { return false; }
        payload[binder.Name] = value;
        return true;
    }

    public override IEnumerable<string> GetDynamicMemberNames() => payload.Keys;
}

public class WordRequestModelExample : OpenApiExample<WordRequestModel>
{
    public override IOpenApiExample<WordRequestModel> Build(NamingStrategy namingStrategy = null)
    {
        dynamic example1 = new WordRequestModel();
        Examples.Add(OpenApiExampleResolver.Resolve("sample1", example1, namingStrategy));
        return this;
    }
}
