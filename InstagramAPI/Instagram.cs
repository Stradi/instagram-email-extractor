using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Proxy;
using InstagramAPI.Models;
using InstagramAPI.Requests;

namespace InstagramAPI {
  public class Instagram {
    private HttpClient httpClient;

    private CookieContainer cookieContainer;
    private HttpClientHandler httpHandler;

    public Instagram(List<ProxyModel> proxies = null) {
      httpHandler = new HttpClientHandler();
      cookieContainer = new CookieContainer();
      httpHandler.CookieContainer = cookieContainer;
      if(proxies != null) {
        httpHandler.Proxy = new RotatingWebProxy(proxies);
      }
      httpClient = new HttpClient(httpHandler);
    }

    public async Task<HttpResponseMessage> SendRequestAsync(BaseRequest request, CredentialModel credential) {
      HttpMethod method = null;
      if(request.requestType == RequestType.GET) {
        method = HttpMethod.Get;
      } else {
        method = HttpMethod.Post;
      }

      if(credential.cookies != null) {
        cookieContainer = credential.cookies;
      }

      HttpRequestMessage req = new HttpRequestMessage(method, request.URL);
      HttpResponseMessage response = await httpClient.SendAsync(req);
      credential.IncreaseRequestCount();

      CookieContainer copyCookies = Helpers.CloneCookieContainer(cookieContainer, Constants.BASE_URL);
      credential.SetCookies(copyCookies);

      ClearCookieContainer();

      //TODO: Return Custom Response according to status code..
      return response;
    }

    private void ClearCookieContainer() {
      cookieContainer.GetCookies(new System.Uri(Constants.BASE_URL))
        .Cast<Cookie>()
        .ToList()
        .ForEach(c => c.Expired = true);
    }
  }
}