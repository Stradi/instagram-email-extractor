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
      
      LoginResponse loginResponse = await ig.LoginAsync(credentials[0]);
      Console.WriteLine("IsSuccessfull   : {0}", loginResponse.IsSuccessfull);
      Console.WriteLine("IsUserExists    : {0}", loginResponse.IsUserExists);
      Console.WriteLine("IsAuthenticated : {0}", loginResponse.IsAuthenticated);
      Console.WriteLine("UserID          : {0}", loginResponse.UserID);
    }
  }
}