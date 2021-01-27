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
using InstagramAPI.Responses;

namespace InstagramAPI {
  public class Instagram {
    private InstagramHttpClient client;
    
    private HttpClient httpClient;

    private CookieContainer cookieContainer;
    private HttpClientHandler httpHandler;

    public Instagram(List<ProxyModel> proxies = null) {
      client = new InstagramHttpClient(proxies);

      httpHandler = new HttpClientHandler();
      cookieContainer = new CookieContainer();
      httpHandler.CookieContainer = cookieContainer;
      if(proxies != null) {
        httpHandler.Proxy = new RotatingWebProxy(proxies);
      }
      httpClient = new HttpClient(httpHandler);
    }

    //TODO: Create Response classes for each response.
    public async Task<LoginResponse> LoginAsync(CredentialModel credential) {
      if(credential.IsLoggedIn) {
        return null;
      }

      // Send request to index page in order to get cookies and rolloutHash.
      HttpRequestMessage indexPageRequest = client.CreateHttpRequestMessage(new GetRequest(Constants.LOGIN_URL));
      HttpResponseMessage indexPageResponse = await client.SendRequestAsync(indexPageRequest);

      string cookiesString = CookieHelper.ExtractResponseCookies(indexPageResponse);
      credential.cookies = CookieHelper.CookieStringToArray(cookiesString);

      RequestHeader rolloutHashHeader = new RequestHeader {
        name = "x-instagram-ajax",
        value = client.GetRolloutHash(await indexPageResponse.Content.ReadAsStringAsync())
      };

      // Prepare for Login request
      LoginRequestParams loginRequestParams = new LoginRequestParams();
      loginRequestParams.credential = credential;
      loginRequestParams.rolloutHash = rolloutHashHeader;
      loginRequestParams.passwordEncryptionValues = client.GetPasswordEncryptionHeaders(indexPageResponse);

      LoginRequest loginRequest = new LoginRequest(loginRequestParams, Constants.LOGIN_POST_URL);
      HttpRequestMessage loginPostRequest = client.CreateHttpRequestMessage(loginRequest);
      HttpResponseMessage loginPostResponse = await client.SendAuthenticatedRequestAsync(loginPostRequest, credential);
      
      LoginResponse response = new LoginResponse();
      response.ConvertFromJSON(await loginPostResponse.Content.ReadAsStringAsync());
      if(response.IsAuthenticated) {
        string loginCookies = CookieHelper.ExtractResponseCookies(loginPostResponse);
        credential.SetCookies(CookieHelper.CookieStringToArray(loginCookies));
        credential.IsLoggedIn = true;
      }

      return response;
    }
  
    public async Task<string[]> GetPostLikers(CredentialModel credential, string shortcode, int total = 100) {
      int totalScraped = 0;
      List<string> scrapedUsernames = new List<string>();
      string endCursor = "";

      while(totalScraped < total) {
        GetPostLikersRequest postLikersRequest;
        if(endCursor == "") {
          postLikersRequest = new GetPostLikersRequest(shortcode);
        } else {
          postLikersRequest = new GetPostLikersRequest(shortcode, endCursor);
        }

        HttpRequestMessage req = client.CreateHttpRequestMessage(postLikersRequest);
        HttpResponseMessage httpResponse = await client.SendAuthenticatedRequestAsync(req, credential);

        GetPostLikersResponse response = new GetPostLikersResponse();
        response.ConvertFromJSON(await httpResponse.Content.ReadAsStringAsync());

        if(response.Usernames.Length > 0) {
          scrapedUsernames.AddRange(response.Usernames);
          totalScraped += response.Usernames.Length;
        }

        if(response.HasNextPage) {
          endCursor = response.EndCursor;
        } else {
          return scrapedUsernames.ToArray();
        }
      }

      return scrapedUsernames.ToArray();
    }
  }
}