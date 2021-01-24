using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Models;
using InstagramAPI.Requests;

namespace InstagramAPI.ConsoleTest {
  class Program {
    static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();    
    }

    private static async Task MainAsync() {
      List<ProxyModel> proxies = new List<ProxyModel>();

      Instagram ig = new Instagram(proxies);

      GetRequest r = new GetRequest(
        "https://www.instagram.com"
      );

      CredentialModel cred = new CredentialModel("username", "password");

      await ig.SendRequestAsync(r, cred);
      Console.WriteLine(cred.GetCsrfToken("https://www.instagram.com"));
    }
  }
}