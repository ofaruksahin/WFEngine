﻿using System;
using Newtonsoft.Json;
using SharpYaml.Serialization;
using WFEngineCore.Models.DefinitionStorage.v1;

namespace WFEngineCore.Services.DefinitionStorage
{
    public static class Deserializers
    {
        private static Serializer yamlSerializer = new Serializer();

        public static Func<string, DefinitionSourceV1> Json = (source) => JsonConvert.DeserializeObject<DefinitionSourceV1>(source);

        public static Func<string, DefinitionSourceV1> Yaml = (source) => yamlSerializer.DeserializeInto(source, new DefinitionSourceV1());
    }
}
