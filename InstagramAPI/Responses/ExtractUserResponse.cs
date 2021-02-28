using System.Linq;

using Newtonsoft.Json.Linq;

using InstagramAPI.Models;
using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {
  public class ExtractUserResponse : IResponse {
    public UserModel User;

    public void ConvertFromJSON(string json) {
      JObject obj = JsonHelper.ConvertFromJSON(json);
      JObject userObject = (JObject)obj["user"];

      UserModel user = new UserModel();
      user.userId = (string)userObject["pk"];
      user.username = (string)userObject["username"];
      user.fullName = (string)userObject["full_name"];
      user.isPrivate = (bool)userObject["is_private"];
      user.isVerified = (bool)userObject["is_verified"];
      user.following = (int)userObject["media_count"];
      user.follower = (int)userObject["follower_count"];
      user.mediaCount = (int)userObject["following_count"];
      
      if(userObject["public_email"] != null) {
        user.email = (string)userObject["public_email"];
      }

      if(userObject["public_phone_country_code"] != null) {
        user.phoneCountryCode = (string)userObject["public_phone_country_code"];
      }

      if(userObject["public_phone_number"] != null) {
        user.phoneNumber = (string)userObject["public_phone_number"];
      }

      if(userObject["is_bussiness"] != null) {
        user.isBussiness = (bool)userObject["is_bussiness"];
      } else {
        user.isBussiness = false;
      }
      
      this.User = user;
    }
  }
}