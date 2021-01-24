using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Models;

namespace InstagramAPI.ConsoleTest {
  class Program {
    static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();    
    }

    private static async Task MainAsync() {
      List<ProxyModel> proxies = new List<ProxyModel>();

      Instagram ig = new Instagram(proxies);

      HttpResponseMessage resp = await ig.SendGetRequest("https://httpbin.org/ip");
      Console.WriteLine(await resp.Content.ReadAsStringAsync());
    }
  }
}