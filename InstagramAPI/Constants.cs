namespace InstagramAPI {
  public static class Constants {
    public static string BASE_URL = "https://www.instagram.com";
    public static string API_URL = "https://i.instagram.com/api/v1/";

    public static string LOGIN_URL = BASE_URL + "/accounts/login/";
    public static string LOGIN_POST_URL = LOGIN_URL + "ajax/";

    public static string GRAPHQL_URL = BASE_URL + "/graphql/query/";
    public static string POST_INFO_QUERY_HASH = "2c4c2e343a8f64c625ba02b2aa12c7f8";
    public static string POST_LIKERS_QUERY_HASH = "d5d763b1e2acf209d62d22d184488e57";
    public static string POST_COMMENTERS_QUERY_HASH = "bc3296d1ce80a24b1b6e40b1e72903f5";

    public static string USER_INFO_URL = API_URL + "users/{0}/info/";

    public static string USER_AGENT = "Mozilla/5.0 (Linux; Android 8.1.0; motorola one Build/OPKS28.63-18-3; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/70.0.3538.80 Mobile Safari/537.36 Instagram 72.0.0.21.98 Android (27/8.1.0; 320dpi; 720x1362; motorola; motorola one; deen_sprout; qcom; pt_BR; 132081645)";
    public static string IG_APPID = "936619743392459";
  }
}