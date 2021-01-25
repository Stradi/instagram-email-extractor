using System;
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

      GetRequest r = new GetRequest(Constants.BASE_URL);

      List<CredentialModel> credentials = new List<CredentialModel>();
      credentials.Add(new CredentialModel("username", "password"));
      credentials.Add(new CredentialModel("username", "password"));
      credentials.Add(new CredentialModel("username", "password"));

      for(int i = 0; i < 3; i++) {
        await ig.SendRequestAsync(r, credentials[i]);
        Console.WriteLine(credentials[i].GetCsrfToken());
      }
    }
  }
}