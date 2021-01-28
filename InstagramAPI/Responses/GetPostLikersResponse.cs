using System.Linq;

using Newtonsoft.Json.Linq;

using InstagramAPI.Models;
using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {  
  public class GetPostLikersResponse : IResponse {
    public int TotalLikers;
    public bool HasNextPage;
    public string EndCursor;
    public PartialUserModel[] Users;

    public void ConvertFromJSON(string json) {
      JObject obj = JsonHelper.ConvertFromJSON(json);
      if((string)obj["status"] == "ok") {
        JObject postObject = (JObject)obj["data"]["shortcode_media"]["edge_liked_by"];
        TotalLikers = (int)postObject["count"];
        HasNextPage = (bool)postObject["page_info"]["has_next_page"];
        if(HasNextPage) {
          EndCursor = (string)postObject["page_info"]["end_cursor"];
        }

        JArray usersArray = (JArray)postObject["edges"];
        Users = usersArray.Select(user => new PartialUserModel() {
          userId = (string)user["node"]["id"],
          username = (string)user["node"]["username"],
          fullName = (string)user["node"]["full_name"],
          isPrivate = (bool)user["node"]["is_private"],
          isVerified = (bool)user["node"]["is_verified"]
        }).ToArray();
      }
    }
  }
}