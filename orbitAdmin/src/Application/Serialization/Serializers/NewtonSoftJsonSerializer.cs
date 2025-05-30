﻿using SchoolV01.Application.Interfaces.Serialization.Serializers;
using SchoolV01.Application.Serialization.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Runtime;

namespace SchoolV01.Application.Serialization.Serializers
{
    public class NewtonSoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public NewtonSoftJsonSerializer(IOptions<NewtonsoftJsonSettings> settings)
        {
            _settings = settings.Value.JsonSerializerSettings;
            _settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public T Deserialize<T>(string text)
            => JsonConvert.DeserializeObject<T>(text, _settings);

        public string Serialize<T>(T obj)
            => JsonConvert.SerializeObject(obj, _settings);
    }
}