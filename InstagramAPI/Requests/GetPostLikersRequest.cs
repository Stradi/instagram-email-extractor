namespace InstagramAPI.Requests {
  public class GetPostLikersRequest : GetRequest {
    public GetPostLikersRequest(string shortcode, string endCursor = null) : base(Constants.GRAPHQL_URL) {
      this.url = Constants.GRAPHQL_URL;

      string variablesParam = "";
      if(endCursor == null) {
        variablesParam = string.Format("{{\"shortcode\":\"{0}\",\"first\":50}}", shortcode);
      } else {
        variablesParam = string.Format("{{\"shortcode\":\"{0}\",\"first\":50, \"after\":\"{1}\"}}", shortcode, endCursor);
      }

      this.parameters = new RequestParams[] {
        new RequestParams() { name = "query_hash", value = Constants.POST_LIKERS_QUERY_HASH },
        new RequestParams() { name = "variables", value = variablesParam }
      };
    }
  }
}