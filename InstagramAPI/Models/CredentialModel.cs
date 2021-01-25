using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace InstagramAPI.Models {
  public class CredentialModel {
    public string username;
    public string password;
    public CookieContainer cookies;

    public int TotalRequests { get { return totalRequests; } }
    private int totalRequests;
    
    public bool IsLoggedIn { get { return isLoggedIn; } set { isLoggedIn = value; } }
    private bool isLoggedIn;

    public CredentialModel(string username, string password) {
      this.username = username;
      this.password = password;
      this.cookies = null;
      this.totalRequests = 0;

      this.isLoggedIn = false;
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
        return null;
      }

      return csrf.Value;
    }
  }
}