using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphML.Common
{
  public sealed class LookupSerializer<TElement> : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      var result = objectType
        .GetInterfaces()
        .Any(a => a.IsGenericType && a == typeof(ILookup<string, TElement>));
      return result;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var retval = new LookupEx<string, TElement>();
      var jsonObject = JObject.Load(reader);
      var properties = jsonObject.Properties().ToList();
      foreach (var property in properties)
      {
        var grping = retval.GetGrouping(property.Name, true);
        var vals = property.Value as JArray;
        foreach (var val in vals)
        {
          grping.Add(val.ToObject<TElement>());
        }
      }

      return retval;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      var target = value as ILookup<string, TElement>;
      var obj = new JObject();
      var keys = target
        .Select(a => a.Key)
        .Distinct();
      foreach (var key in keys)
      {
        obj.Add(key.ToString(), JArray.FromObject(target[key]));
      }

      obj.WriteTo(writer);
    }
  }
}
