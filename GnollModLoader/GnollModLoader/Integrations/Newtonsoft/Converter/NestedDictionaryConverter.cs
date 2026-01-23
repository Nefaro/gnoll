using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GnollModLoader.Integrations.Newtonsoft.Converter
{
    // Because Newtonsoft does not support deserializing unknown nested Dictionaries
    public class NestedDictionaryConverter : CustomCreationConverter<IDictionary<string, object>>
    {
        public override IDictionary<string, object> Create(Type objectType)
        {
            return new Dictionary<string, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.DateParseHandling = DateParseHandling.None;
            reader.DateParseHandling = DateParseHandling.None;
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);
            if (reader.TokenType == JsonToken.StartArray)
                return serializer.Deserialize(reader, typeof(object[]));
            return serializer.Deserialize(reader);
        }
    }
}
