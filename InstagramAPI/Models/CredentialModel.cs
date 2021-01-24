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
      if(this.cookies == null) {
        this.cookies = cookies;
      }
    }

    public void IncreaseRequestCount() {
      this.totalRequests++;
    }

    public string GetCsrfToken(string url) {
      if(cookies == null) {
        return null;
      }

      Cookie csrf = cookies.GetCookies(new System.Uri(url)).Cast<Cookie>()
        .FirstOrDefault(x => x.Name == "csrftoken");

      return csrf.Value;
    }
  }
}