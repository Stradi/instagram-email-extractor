using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using InstagramAPI.Proxy;
using InstagramAPI.Models;
using InstagramAPI.Helpers;
using InstagramAPI.Requests;

namespace InstagramAPI {
  public class InstagramHttpClient {
    private HttpClient httpClient;
    private HttpClientHandler httpClientHandler;

    private PasswordEncryptionModel passwordEncryptionKeys;

    public InstagramHttpClient(List<ProxyModel> proxies) {
      httpClientHandler = new HttpClientHandler();

      if(proxies.Count != 0) {
        httpClientHandler.Proxy = new RotatingWebProxy(proxies);
      }

      httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request) {
      HttpResponseMessage response = await httpClient.SendAsync(request);
      ClearCookies();
      return response;
    }

    public async Task<HttpResponseMessage> SendAuthenticatedRequestAsync(HttpRequestMessage request, CredentialModel credential) {
      SetCookies(ref request, CookieHelper.CookieArrayToString(credential.cookies));
      SetHeaders(ref request, new RequestHeader[] { new RequestHeader() { name="x-csrftoken", value=credential.GetCsrfToken()}});
      credential.IncreaseRequestCount();
      return await SendRequestAsync(request);
    }

    public HttpRequestMessage CreateHttpRequestMessage(BaseRequest req) {
      HttpRequestMessage request = new HttpRequestMessage();
      if(req.requestType == RequestType.GET) {
        request.Method = HttpMethod.Get;
      } else {
        request.Method = HttpMethod.Post;

        PostRequest postRequest = (PostRequest)req;
        request.Content = postRequest.GetData();
      }

      request.RequestUri = new System.Uri(req.ToString());

      SetDefaultHeaders(ref request);
      if(req.additionalHeaders.Count > 0) {
        SetHeaders(ref request, req.additionalHeaders.ToArray());
      }

      return request;
    }

    public void SetHeaders(ref HttpRequestMessage request, RequestHeader[] headers) {      
      foreach(RequestHeader header in headers) {
        request.Headers.Add(header.name, header.value);
      }
    }

    public void SetCookies(ref HttpRequestMessage request, string cookies) {
      request.Headers.Add("Cookie", cookies);
    }

    public void ClearCookies() {
      httpClientHandler.CookieContainer.GetCookies(new System.Uri(Constants.BASE_URL))
        .Cast<Cookie>()
        .ToList()
        .ForEach(c => c.Expired = true);
    }

    public string GetRolloutHash(string html) {
      return Regex.Match(html, "/\"rollout_hash\":\"([^\"]*\")/g").Groups[0].Value;
    }

    public PasswordEncryptionModel GetPasswordEncryptionHeaders(HttpResponseMessage response) {
      if(string.IsNullOrEmpty(passwordEncryptionKeys.keyId) ||
         string.IsNullOrEmpty(passwordEncryptionKeys.keyVersion) || 
         string.IsNullOrEmpty(passwordEncryptionKeys.publicKey)
      ) {
        passwordEncryptionKeys.keyId = response.Headers.GetValues("ig-set-password-encryption-web-key-id").First();
        passwordEncryptionKeys.keyVersion = response.Headers.GetValues("ig-set-password-encryption-web-key-version").First();
        passwordEncryptionKeys.publicKey = response.Headers.GetValues("ig-set-password-encryption-web-pub-key").First();
      }

      return passwordEncryptionKeys;
    }

    private void SetDefaultHeaders(ref HttpRequestMessage request) {
      List<RequestHeader> defaultHeaders = new List<RequestHeader>();

      defaultHeaders.Add(new RequestHeader() { name = "x-ig-appid", value = Constants.IG_APPID });
      defaultHeaders.Add(new RequestHeader() { name = "x-requested-with", value = "XMLHttpRequest" });
      defaultHeaders.Add(new RequestHeader() { name = "user-agent", value = Constants.USER_AGENT });

      SetHeaders(ref request, defaultHeaders.ToArray());
    }
  }
}