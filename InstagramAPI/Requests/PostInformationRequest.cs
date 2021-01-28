namespace InstagramAPI.Requests {
  public class PostInformationRequest : GetRequest {
    public PostInformationRequest(string shortcode) : base("") {
      this.url = Constants.GRAPHQL_URL;

      string variablesParam = "";
      variablesParam = string.Format("{{\"shortcode\":\"{0}\",\"fetch_comment_count\":50,\"parent_comment_count\":50,\"has_threaded_comments\":true}}", shortcode);

      this.parameters = new RequestParams[] {
        new RequestParams() { name = "query_hash", value = Constants.POST_INFO_QUERY_HASH },
        new RequestParams() { name = "variables", value = variablesParam }
      };
    }
  }
}