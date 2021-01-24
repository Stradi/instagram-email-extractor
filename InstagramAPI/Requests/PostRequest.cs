using System.Net.Http;

namespace InstagramAPI.Requests {
  public class PostRequest : BaseRequest {
    private StringContent content;
    public StringContent Data {
      get {
        return content;
      }
    }
    
    public PostRequest(string url, StringContent content, params RequestParams[] parameters) : base(url, RequestType.POST, parameters) {
      this.content = content;
    }
  }
}