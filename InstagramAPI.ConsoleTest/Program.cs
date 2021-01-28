using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net.Http;

using InstagramAPI.Models;
using InstagramAPI.Responses;

namespace InstagramAPI.ConsoleTest {
  class Program {
    static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();    
    }

    private static async Task MainAsync() {
      List<ProxyModel> proxies = new List<ProxyModel>();

      Instagram ig = new Instagram(proxies);

      List<CredentialModel> credentials = new List<CredentialModel>();
      credentials.Add(new CredentialModel("", ""));

      await ig.LoginAsync(credentials[0]);
      PartialUserModel[] comms = await ig.GetPostCommenters(credentials[0], "");

      for(int i = 0; i < comms.Length; i++) {
        Console.WriteLine(comms[i].username);
      }
    }
  }
}