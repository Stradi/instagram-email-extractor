using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Proxy;
using InstagramAPI.Models;
using InstagramAPI.Helpers;
using InstagramAPI.Requests;
using InstagramAPI.Responses;

namespace InstagramAPI {
  public delegate void DataAcquired(PartialUserModel[] user, string endCursor);

  public class Instagram {
    public CredentialManager credentialManager;
    public RotatingWebProxy webProxy;
    private InstagramHttpClient client;

    public event DataAcquired OnUserFetched;
    
    public Instagram() {
      webProxy = new RotatingWebProxy();

      credentialManager = new CredentialManager();
      client = new InstagramHttpClient(webProxy);
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

        credentialManager.AddCredential(credential);
      }

      return response;
    }
  
    public async Task<PartialUserModel[]> GetPostLikers(string shortcode, int total = 100, string endCursor = "") {
      int totalScraped = 0;
      List<PartialUserModel> scrapedUsernames = new List<PartialUserModel>();

      while(totalScraped < total) {
        CredentialModel credential = credentialManager.GetCredential();

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

        if(response.Users.Length > 0) {
          scrapedUsernames.AddRange(response.Users);
          totalScraped += response.Users.Length;

          OnUserFetched?.Invoke(response.Users, endCursor);
        }

        if(response.HasNextPage) {
          endCursor = response.EndCursor;
        } else {
          return scrapedUsernames.ToArray();
        }
      }

      return scrapedUsernames.ToArray();
    }

    public async Task<PartialUserModel[]> GetPostCommenters(string shortcode, int total = 24, string endCursor = "") {
      int totalScraped = 0;
      List<PartialUserModel> scrapedUsernames = new List<PartialUserModel>();

      while(totalScraped < total) {
        CredentialModel credential = credentialManager.GetCredential();
        
        BaseRequest postCommentersRequest;

        if(endCursor == "") {
          postCommentersRequest = new PostInformationRequest(shortcode);
        } else {
          postCommentersRequest = new GetPostCommentersRequest(shortcode, endCursor);
        }

        HttpRequestMessage req = client.CreateHttpRequestMessage(postCommentersRequest);
        HttpResponseMessage httpResponse = await client.SendAuthenticatedRequestAsync(req, credential);

        GetPostCommentersResponse response = new GetPostCommentersResponse();
        response.ConvertFromJSON(await httpResponse.Content.ReadAsStringAsync());

        if(response.Users.Length > 0) {
          PartialUserModel[] distinct = response.Users.GroupBy(user => user.userId).Select(user => user.First()).ToArray();
          scrapedUsernames.AddRange(distinct);
          totalScraped += distinct.Length;

          OnUserFetched?.Invoke(distinct, endCursor);
        }

        if(response.HasNextPage) {
          endCursor = response.EndCursor;
        } else {
          return scrapedUsernames.ToArray();
        }
      }

      return scrapedUsernames.ToArray();
    }

    public async Task<PostModel[]> GetPostsFromHashtag(string hashtag, int total = 100, string endCursor = "") {      
      int totalScraped = 0;
      List<PostModel> scrapedPosts = new List<PostModel>();
      bool isTopPostsAdded = false;

      while(totalScraped < total) {
        CredentialModel credential = credentialManager.GetCredential();
        
        GetHashtagPostsRequest hashtagPostsRequest;
        if(endCursor == "") {
          hashtagPostsRequest = new GetHashtagPostsRequest(hashtag);
        } else {
          hashtagPostsRequest = new GetHashtagPostsRequest(hashtag, endCursor);
        }

        HttpRequestMessage req = client.CreateHttpRequestMessage(hashtagPostsRequest);
        HttpResponseMessage httpResponse = await client.SendAuthenticatedRequestAsync(req, credential);
      
        GetHashtagPostsResponse response = new GetHashtagPostsResponse();
        response.ConvertFromJSON(await httpResponse.Content.ReadAsStringAsync());

        if(!isTopPostsAdded) {
          totalScraped += response.TopPosts.Length;
          scrapedPosts.AddRange(response.TopPosts);
        
          isTopPostsAdded = true;
        }

        if(response.AllPosts.Length > 0) {
          totalScraped += response.AllPosts.Length;
          scrapedPosts.AddRange(response.AllPosts);
        }

        if(response.HasNextPage) {
          endCursor = response.EndCursor;
        } else {
          return scrapedPosts.ToArray();
        }
      }

      return scrapedPosts.ToArray();
    }
    
    public async Task<UserModel> ExtractUser(string userId) {
      CredentialModel credential = credentialManager.GetCredential();

      ExtractUserRequest extractUserRequest = new ExtractUserRequest(userId);
      HttpRequestMessage request = client.CreateHttpRequestMessage(extractUserRequest);

      HttpResponseMessage httpResponse = await client.SendAuthenticatedRequestAsync(request, credential);
   
      ExtractUserResponse response = new ExtractUserResponse();
      string responseContent = await httpResponse.Content.ReadAsStringAsync();
      
      response.ConvertFromJSON(responseContent);
      return response.User;
    }
  }
}