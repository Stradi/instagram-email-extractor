namespace InstagramAPI.Requests {
  public class GetPostCommentersRequest : GetRequest {
    public GetPostCommentersRequest(string shortcode, string endCursor) : base("") {
      this.url = Constants.GRAPHQL_URL;

      //{\"cached_comments_cursor\": \"17881671446073003\", \"bifilter_token\": \"KCwBDAAwACAAGAAQAAgACAC-G_X69mVNi-fSX53d-2_2_zSdYIHhwWu0DAsigAA=\"}
      //Doing this Replace because end_cursor needs to have unescaped double quotes.
      endCursor = endCursor.Replace("\"", "\\\"");

      string variablesParam = "";
      variablesParam = string.Format("{{\"shortcode\":\"{0}\",\"first\":50,\"after\":\"{1}\"}}", shortcode, endCursor);
      System.Console.WriteLine(variablesParam);
    
      this.parameters = new RequestParams[] {
        new RequestParams() { name = "query_hash", value = Constants.POST_COMMENTERS_QUERY_HASH },
        new RequestParams() { name = "variables", value = variablesParam }
      };
    }
  }
}