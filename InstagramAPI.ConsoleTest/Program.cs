using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net.Http;

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
      credentials.Add(new CredentialModel("", ""));
      
      HttpResponseMessage loginResponse = await ig.LoginAsync(credentials[0]);
      Console.WriteLine(await loginResponse.Content.ReadAsStringAsync());
    }
  }
}