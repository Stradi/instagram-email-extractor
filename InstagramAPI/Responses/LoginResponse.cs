using InstagramAPI.Helpers;

namespace InstagramAPI.Responses {
  public struct LoginResponseObject {
    public string status;
    public bool user;
    public string userId;
    public bool authenticated;
    public string message;
  }

  public class LoginResponse : IResponse {
    public bool IsSuccessfull;
    public bool IsUserExists;
    public bool IsAuthenticated;
    public string UserID;
    public string ErrorMessage;
    
    public void ConvertFromJSON(string json) {
      LoginResponseObject obj = JsonHelper.ConvertFromJSON<LoginResponseObject>(json);
      if(obj.status == "ok") {
        this.IsSuccessfull = true;
        this.IsUserExists = obj.user;
        this.IsAuthenticated = obj.authenticated;
        this.UserID = obj.userId;
      } else {
        this.IsSuccessfull = false;
        this.ErrorMessage = obj.message;
      }
    }
  }
}