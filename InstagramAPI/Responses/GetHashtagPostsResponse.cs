using System.Linq;
using Newtonsoft.Json.Linq;

using InstagramAPI.Models;
using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {
  public class GetHashtagPostsResponse : IResponse {
    public int TotalPosts;
    public string HashtagName;
    public string HashtagID;
    public PostModel[] TopPosts;
    public PostModel[] AllPosts;

    public bool HasNextPage;
    public string EndCursor;
    
    public void ConvertFromJSON(string json) {
      JObject obj = JsonHelper.ConvertFromJSON(json);
      JObject hashtagObject = null;
      if(obj["graphql"] == null && obj["data"] != null) {
        //We requested with query_hash
        hashtagObject = (JObject)obj["data"]["hashtag"];
      } else {
        //We requested with ?__a=1
        hashtagObject = (JObject)obj["graphql"]["hashtag"];
      }

      HashtagID = (string)hashtagObject["id"];
      HashtagName = (string)hashtagObject["name"];
      TotalPosts = (int)hashtagObject["edge_hashtag_to_media"]["count"];
      
      HasNextPage = (bool)hashtagObject["edge_hashtag_to_media"]["page_info"]["has_next_page"];
      if(HasNextPage) {
        EndCursor = (string)hashtagObject["edge_hashtag_to_media"]["page_info"]["end_cursor"];
      }

      JArray topPostsArray = (JArray)hashtagObject["edge_hashtag_to_top_posts"]["edges"];
      TopPosts = JArrayToPosts(topPostsArray);

      JArray postsArray = (JArray)hashtagObject["edge_hashtag_to_media"]["edges"];
      AllPosts = JArrayToPosts(postsArray);
    }

    private PostModel[] JArrayToPosts(JArray arr) {
      return arr.Select(post => new PostModel() {
        totalLikes = (int)post["node"]["edge_liked_by"]["count"],
        totalComments = (int)post["node"]["edge_media_to_comment"]["count"],
        shortcode = (string)post["node"]["shortcode"],
        ownerId = (string)post["node"]["owner"]["id"]
      }).ToArray();
    }
  }
}