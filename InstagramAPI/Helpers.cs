using System.Net;
using System.Text.Json;

namespace InstagramAPI {
  public static class Helpers {
    public static string SerializeCookieContainer(CookieContainer cookieContainer) {
      return JsonSerializer.Serialize(cookieContainer, typeof(CookieContainer));
    }

    public static CookieContainer DeserializeCookieContainer(string serialized) {
      return (CookieContainer)JsonSerializer.Deserialize(serialized, typeof(CookieContainer));
    }

    public static CookieContainer CloneCookieContainer(CookieContainer original, string url) {
      CookieContainer newCookieContainer = new CookieContainer();
      newCookieContainer.Add(original.GetCookies(new System.Uri(url)));

      return newCookieContainer;
    }
  }
}