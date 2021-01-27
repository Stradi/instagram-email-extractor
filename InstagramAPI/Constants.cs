namespace InstagramAPI {
  public static class Constants {
    public static string BASE_URL = "https://www.instagram.com";
    public static string LOGIN_URL = BASE_URL + "/accounts/login/";
    public static string LOGIN_POST_URL = LOGIN_URL + "ajax/";
    public static string GRAPHQL_URL = BASE_URL + "/graphql/query/";

    public static string POST_LIKERS_QUERY_HASH = "d5d763b1e2acf209d62d22d184488e57";

    public static string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.141 Safari/537.36";
    public static string IG_APPID = "936619743392459";
  }
}