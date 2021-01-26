using System.Net;

namespace InstagramAPI.Helpers {
  public static class GenericHelper {
    public static CookieContainer CloneCookieContainer(CookieContainer original, string url) {
      CookieContainer newCookieContainer = new CookieContainer();
      newCookieContainer.Add(original.GetCookies(new System.Uri(url)));

      return newCookieContainer;
    }
  }
}