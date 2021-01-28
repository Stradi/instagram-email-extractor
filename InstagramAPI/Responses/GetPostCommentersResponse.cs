using System.Linq;
using Newtonsoft.Json.Linq;

using InstagramAPI.Models;
using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {
  class GetPostCommentersResponse : IResponse {
    public int TotalCommenters;
    public bool HasNextPage;
    public string EndCursor;
    public PartialUserModel[] Users;
    
    public void ConvertFromJSON(string json) {
      JObject obj = JsonHelper.ConvertFromJSON(json);
      if((string)obj["status"] == "ok") {
        JObject postObject = (JObject)obj["data"]["shortcode_media"]["edge_media_to_parent_comment"];
        TotalCommenters = (int)postObject["count"];
        HasNextPage = (bool)postObject["page_info"]["has_next_page"];
        if(HasNextPage) {
          EndCursor = (string)postObject["page_info"]["end_cursor"];
        }

        JArray usersArray = (JArray)postObject["edges"];
        Users = usersArray.Select(user => new PartialUserModel() {
          userId = (string)user["node"]["owner"]["id"],
          username = (string)user["node"]["owner"]["username"],
          isVerified = (bool)user["node"]["owner"]["is_verified"]
        }).ToArray();
      }
    }
  }
}