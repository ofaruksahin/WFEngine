using System;
using WFEngineCore.Models;
using WFEngineCore.Models.DefinitionStorage.v1;

namespace WFEngineCore.Interface
{
    public interface IDefinitionLoader
    {
        WorkflowDefinition LoadDefinition(string source, Func<string, DefinitionSourceV1> deserializer);
    }
}