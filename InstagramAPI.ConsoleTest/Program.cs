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
        "https://httpbin.org/get",
        new RequestParams() { name = "Name1", value = "Value1" },
        new RequestParams() { name = "Name2", value = "Value2" }
      );

      PostRequest pr = new PostRequest(
        "https://httpbin.org/post",
        new StringContent("{\"data\": \"value\"}")
      );

      HttpResponseMessage resp = await ig.PostRequestAsync(pr);
      Console.WriteLine(await resp.Content.ReadAsStringAsync());
    }
  }
}