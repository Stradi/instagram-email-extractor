using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace InstagramAPI.Models {
  public class CredentialModel {
    public string username;
    public string password;
    public Cookie[] cookies;

    public int TotalRequests { get { return totalRequests; } }
    private int totalRequests;
    
    public bool IsLoggedIn { get { return isLoggedIn; } set { isLoggedIn = value; } }
    private bool isLoggedIn;

    public CredentialModel(string username, string password) {
      this.username = username;
      this.password = password;
      this.totalRequests = 0;

      this.isLoggedIn = false;
    }

    public void SetCookies(Cookie[] cookies) {
      this.cookies = cookies;
    }

    public void IncreaseRequestCount() {
      this.totalRequests++;
    }

    public string GetCsrfToken() {
      if(cookies == null) {
        return null;
      }

      Cookie c = cookies.First(cookie => cookie.Name == "csrftoken");
      return c.Value;
    }
  }
}