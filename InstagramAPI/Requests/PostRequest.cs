using System.Net.Http;

namespace InstagramAPI.Requests {
  public abstract class PostRequest : BaseRequest {
    private FormUrlEncodedContent content;
    public FormUrlEncodedContent Data {
      get {
        return content;
      }
      set {
        content = Data;
      }
    }
    
    public PostRequest(string url, FormUrlEncodedContent content, params RequestParams[] parameters) : base(url, RequestType.POST, parameters) {
      this.content = content;
    }

    public abstract HttpContent GetData();
  }
}