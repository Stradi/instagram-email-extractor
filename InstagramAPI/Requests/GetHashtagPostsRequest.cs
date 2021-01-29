namespace InstagramAPI.Requests {
  public class GetHashtagPostsRequest: GetRequest {
    public GetHashtagPostsRequest(string hashtagName, string endCursor = "") : base("") {
      if(string.IsNullOrEmpty(endCursor)) {
        this.url = Constants.BASE_URL + string.Format("/explore/tags/{0}/?__a=1", hashtagName);
      } else {
        this.url = Constants.GRAPHQL_URL;

        string variablesParams = "";
        variablesParams = string.Format("{{\"tag_name\":\"{0}\",\"first\":24,\"after\":\"{1}\"}}", hashtagName, endCursor);

        this.parameters = new RequestParams[] {
          new RequestParams() { name = "query_hash", value = Constants.HASHTAG_POSTS_QUERY_HASH },
          new RequestParams() { name = "variables", value = variablesParams }
        };
      }
    }
  }
}