using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphML.Common
{
  public sealed class FindDuplicatesResultSerializer : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      var result = objectType
        .GetInterfaces()
        .Any(a => a.IsGenericType && a == typeof(IList<IGrouping<string, string[]>>));
      return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      var target = (List<IGrouping<string, string[]>>) value;
      var obj = new JObject();
      foreach (var grping in target)
      {
        obj.Add(grping.Key, JArray.FromObject(grping.ToList()));
      }

      obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      var retval = new List<IGrouping<string, string[]>>();
      var jsonObject = JObject.Load(reader);
      var properties = jsonObject.Properties().ToList();
      foreach (var property in properties)
      {
        var grping = new Grouping<string, string[]>(property.Name, 0);
        var vals = property.Value as JArray;
        foreach (var val in vals)
        {
          grping.Add(val.ToObject<string[]>());
        }
        retval.Add(grping);
      }

      return retval;
    }
  }
}
