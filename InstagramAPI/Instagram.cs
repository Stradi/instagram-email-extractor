using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using InstagramAPI.Proxy;
using InstagramAPI.Models;
using InstagramAPI.Requests;

namespace InstagramAPI {
  public class Instagram {
    private HttpClient httpClient;
    private HttpClientHandler httpHandler;

    public Instagram(List<ProxyModel> proxies = null) {
      httpHandler = new HttpClientHandler();
      if(proxies != null) {
        httpHandler.Proxy = new RotatingWebProxy(proxies);
      }
      httpClient = new HttpClient(httpHandler);
    }

    public Task<HttpResponseMessage> GetRequestAsync(GetRequest request) {      
      return httpClient.GetAsync(request.ToString());
    }

    public Task<HttpResponseMessage> PostRequestAsync(PostRequest request) {
      return httpClient.PostAsync(request.ToString(), request.Data);
    }
  }
}