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
      credentials.Add(new CredentialModel("USERNAME", "PASSWORD"));

      LoginResponse loginResponse = await ig.LoginAsync(credentials[0]);
      if(loginResponse.IsAuthenticated) {
        Console.WriteLine("Successfully logged in as {0}", credentials[0].username);
      } else {
        Console.WriteLine("Not logged in :(");
        return;
      }

      string[] likers = await ig.GetPostLikers(credentials[0], "SHORTCODE");
      if(likers.Length > 0) {
        for(int i = 0; i < likers.Length; i++) {
          Console.WriteLine("{0}\t:{1}", i, likers[i]);
        }
      } else {
        Console.WriteLine("Somehow likers length is 0 :(");
      }
    }
  }
}