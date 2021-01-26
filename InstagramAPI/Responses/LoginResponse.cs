using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {
  public struct LoginResponseObject {
    public string status;
    public bool user;
    public string userId;
    public bool authenticated;
  }

  public class LoginResponse : IResponse {
    public bool IsSuccessfull;
    public bool IsUserExists;
    public bool IsAuthenticated;
    public string UserID;
    
    public void ConvertFromJSON(string json) {
      LoginResponseObject obj = JsonHelper.ConvertFromJSON<LoginResponseObject>(json);
      if(obj.status == "ok") {
        this.IsSuccessfull = true;
        this.IsUserExists = obj.user;
        this.IsAuthenticated = obj.authenticated;
        this.UserID = obj.userId;
      } else {
        this.IsSuccessfull = false;
      }
    }
  }
}