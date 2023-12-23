using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSamples
{
    public partial class Welcome7
    {
        [JsonProperty("Configuration")]
        public Configuration Configuration { get; set; }
    }

    public partial class Configuration
    {
        [JsonProperty("ApplicationSettings")]
        public ApplicationSettings ApplicationSettings { get; set; }

        [JsonProperty("Modules")]
        public Modules Modules { get; set; }

        [JsonProperty("DatabaseConnections")]
        public DatabaseConnections DatabaseConnections { get; set; }

        [JsonProperty("UserPreferences")]
        public UserPreferences UserPreferences { get; set; }
    }

    public partial class ApplicationSettings
    {
        [JsonProperty("AppName")]
        public string AppName { get; set; }

        [JsonProperty("AppVersion")]
        public string AppVersion { get; set; }

        [JsonProperty("LogLevel")]
        public string LogLevel { get; set; }
    }

    public partial class DatabaseConnections
    {
        [JsonProperty("Connection")]
        public Connection[] Connection { get; set; }
    }

    public partial class Connection
    {
        [JsonProperty("Server")]
        public string Server { get; set; }

        [JsonProperty("Port")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Port { get; set; }

        [JsonProperty("DatabaseName")]
        public string DatabaseName { get; set; }

        [JsonProperty("Credentials")]
        public Credentials Credentials { get; set; }

        [JsonProperty("_name")]
        public string Name { get; set; }
    }

    public partial class Credentials
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }

    public partial class Modules
    {
        [JsonProperty("Module")]
        public Module[] Module { get; set; }
    }

    public partial class Module
    {
        [JsonProperty("Enabled")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool Enabled { get; set; }

        [JsonProperty("Options")]
        public Options Options { get; set; }

        [JsonProperty("_name")]
        public string Name { get; set; }
    }

    public partial class Options
    {
        [JsonProperty("Option")]
        public Option[] Option { get; set; }
    }

    public partial class Option
    {
        [JsonProperty("_key")]
        public string Key { get; set; }

        [JsonProperty("__text")]
        public string Text { get; set; }
    }

    public partial class UserPreferences
    {
        [JsonProperty("Theme")]
        public string Theme { get; set; }

        [JsonProperty("FontSize")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long FontSize { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("AutoSaveEnabled")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool AutoSaveEnabled { get; set; }
    }

    public partial class Welcome7
    {
        public static Welcome7 FromJson(string json) => JsonConvert.DeserializeObject<Welcome7>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome7 self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}
