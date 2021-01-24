namespace InstagramAPI.Requests {
  public class GetRequest : BaseRequest {
    public GetRequest(string url, params RequestParams[] parameters) : base(url, RequestType.GET, parameters) { }
  }
}