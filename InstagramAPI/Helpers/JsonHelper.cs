using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstagramAPI.Helpers {
  public static class JsonHelper {
    public static T ConvertFromJSON<T>(string json) {
      T obj = JsonConvert.DeserializeObject<T>(json);
      return obj;
    }

    public static JObject ConvertFromJSON(string json) {
      JObject obj = JObject.Parse(json);
      return obj;
    }

    public static string SerializeObject<T>(T obj) {
      return JsonConvert.SerializeObject(obj);
    }

    public static T DeserializeObject<T>(string json) {
      return JsonConvert.DeserializeObject<T>(json);
    }
  }
}