using System.Net;
using System.Linq;

namespace InstagramAPI.Models {
  public class CredentialModel {
    public string username;
    public string password;
    public CookieContainer cookies;

    private int totalRequests;
    public int TotalRequests { get { return totalRequests; } }

    public CredentialModel(string username, string password) {
      this.username = username;
      this.password = password;
      this.cookies = null;
      this.totalRequests = 0;
    }

    public void SetCookies(CookieContainer cookies) {
      this.cookies = cookies;
    }

    public void IncreaseRequestCount() {
      this.totalRequests++;
    }

    public string GetCsrfToken() {
      if(cookies == null) {
        return null;
      }

      Cookie csrf = cookies.GetCookies(new System.Uri(Constants.BASE_URL)).Cast<Cookie>()
        .FirstOrDefault(x => x.Name == "csrftoken");

      if(csrf == null) {
        return "CSRF is NULL";
      }

      return csrf.Value;
    }
  }
}