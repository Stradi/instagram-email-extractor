using Newtonsoft.Json;

namespace InstagramAPI.Helpers {
  public static class JsonHelper {
    public static T ConvertFromJSON<T>(string json) {
      T obj = JsonConvert.DeserializeObject<T>(json);
      return obj;
    }
  }
}