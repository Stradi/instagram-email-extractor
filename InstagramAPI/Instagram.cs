using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using InstagramAPI.Proxy;
using InstagramAPI.Models;

namespace InstagramAPI {
  public class Instagram {
    private HttpClient httpClient;
    private HttpClientHandler httpHandler;

    public Instagram(List<ProxyModel> proxies = null) {
      httpHandler = new HttpClientHandler();
      httpHandler.Proxy = new RotatingWebProxy(proxies);
      httpClient = new HttpClient(httpHandler);
    }

    public Task<HttpResponseMessage> SendGetRequest(string url) {      
      return httpClient.GetAsync(url);
    }
  }
}