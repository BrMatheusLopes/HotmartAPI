using System;
using System.Text.Json;

namespace HotmartAPI.Helpers
{
    public static class JsonHelper
    {
        public static string Serialize(object obj, bool writeIndented = true)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = writeIndented,
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static T Deserialize<T>(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                throw new ArgumentException($"'{nameof(obj)}' não pode ser nulo nem vazio.", nameof(obj));

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };

#pragma warning disable CS8603 // Possível retorno de referência nula.
            return JsonSerializer.Deserialize<T>(obj, options);
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }
    }
}
