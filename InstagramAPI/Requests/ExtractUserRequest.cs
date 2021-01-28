namespace InstagramAPI.Requests {
  public class ExtractUserRequest : GetRequest {
    public ExtractUserRequest(string userId) : base("") {
      this.url = string.Format(Constants.USER_INFO_URL, userId);
    }
  }
}