using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

    //TODO: Create Response classes for each response.
    public async Task<HttpResponseMessage> LoginAsync(CredentialModel credential) {
      if(credential.IsLoggedIn) {
        return null;
      }
      
      HttpResponseMessage indexPage = await SendRequestAsync(new GetRequest(Constants.LOGIN_URL), credential);

      string rolloutHash = GetRolloutHash(await indexPage.Content.ReadAsStringAsync());      
      RequestHeader rolloutHashHeader = new RequestHeader {
        name = "x-instagram-ajax",
        value = rolloutHash
      };    

      LoginRequestParams loginRequestParams = new LoginRequestParams();
      loginRequestParams.credential = credential;
      loginRequestParams.rolloutHash = rolloutHashHeader;
      loginRequestParams.passwordEncryptionValues = GetPasswordEncryptionHeaders(indexPage);;

      LoginRequest loginRequest = new LoginRequest(loginRequestParams, Constants.LOGIN_POST_URL);
      
      HttpResponseMessage loginResponse = await SendRequestAsync(loginRequest, credential);

      return loginResponse;
    }

    public async Task<HttpResponseMessage> SendRequestAsync(BaseRequest request, CredentialModel credential = null) {      
      HttpMethod method = null;
      if(request.requestType == RequestType.GET) {
        method = HttpMethod.Get;
      } else {
        method = HttpMethod.Post;
      }

      HttpRequestMessage req = new HttpRequestMessage(method, request.URL);

      if(request.requestType == RequestType.POST) {
        PostRequest p = (PostRequest)request;
        req.Content = p.GetData();
      }

      SetHeaders(ref req, credential);
      if(request.additionalHeaders.Count != 0) {
        for(int i = 0; i < request.additionalHeaders.Count; i++) {
          AddHeader(ref req, request.additionalHeaders[i]);
        }
      }
      
      if(credential.cookies != null) {
        cookieContainer = credential.cookies;
      }

      HttpResponseMessage response = await httpClient.SendAsync(req);

      if(credential != null) {
        CookieContainer copyCookies = Helpers.CloneCookieContainer(cookieContainer, Constants.BASE_URL);
    
        credential.IncreaseRequestCount();
        credential.SetCookies(copyCookies);
      }

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

    public PasswordEncryptionModel GetPasswordEncryptionHeaders(HttpResponseMessage response) {
      PasswordEncryptionModel encParams = new PasswordEncryptionModel();
      
      encParams.keyId = response.Headers.GetValues("ig-set-password-encryption-web-key-id").First();
      encParams.keyVersion = response.Headers.GetValues("ig-set-password-encryption-web-key-version").First();
      encParams.publicKey = response.Headers.GetValues("ig-set-password-encryption-web-pub-key").First();

      return encParams;
    }

    private void SetHeaders(ref HttpRequestMessage request, CredentialModel credential) {
      //TODO: Assign random user agent to each credential..
      AddHeader(ref request, new RequestHeader() { name = "x-ig-appid", value = Constants.IG_APPID });
      AddHeader(ref request, new RequestHeader() { name = "x-requested-with", value = "XMLHttpRequest" });

      if(credential != null && credential.GetCsrfToken() != null) {
        AddHeader(ref request, new RequestHeader() { name = "x-csrftoken", value = credential.GetCsrfToken() });
      }
    }

    private void AddHeader(ref HttpRequestMessage request, RequestHeader header) {
      request.Headers.Add(header.name, header.value);
    }

    private string GetRolloutHash(string html) {
      return Regex.Match(html, "/\"rollout_hash\":\"([^\"]*\")/g").Groups[0].Value;
    }
  }
}