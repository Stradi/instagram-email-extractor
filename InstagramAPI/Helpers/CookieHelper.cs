using System.Net;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace InstagramAPI.Helpers {
  public static class CookieHelper {
    public static string ExtractResponseCookies(HttpResponseMessage response) {
      IEnumerable<string> allCookiesString = response.Headers.SingleOrDefault(
        header => header.Key == "Set-Cookie"
      ).Value;

      string cookiesString = "";

      foreach(string cookieString in allCookiesString) {
        string[] cookieNameAndValue = cookieString.Split(';')[0].Split('=');
        cookiesString += string.Format("{0}={1};", cookieNameAndValue[0], cookieNameAndValue[1]);
      }

      return cookiesString;
    }

    public static Cookie[] CookieStringToArray(string cookieString) {
      List<Cookie> cookies = new List<Cookie>();
      string[] allCookies = cookieString.Split(';');
      foreach(string cookie in allCookies) {
        string[] nameValue = cookie.Split('=');
        if(nameValue.Length > 1) {
          cookies.Add(new Cookie(nameValue[0], nameValue[1] != null ? nameValue[1] : ""));
        }
      }

      return cookies.ToArray();
    }

    public static string CookieArrayToString(Cookie[] cookieArray) {
      string cookieString = "";
      foreach(Cookie c in cookieArray) {
        cookieString += string.Format("{0}={1};", c.Name, c.Value);
      }

      cookieString = cookieString.Remove(cookieString.Length - 1);

      return cookieString;
    }
  }
}