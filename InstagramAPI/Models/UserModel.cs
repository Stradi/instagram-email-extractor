namespace InstagramAPI.Models {
  public class UserModel {
    public string userId;
    public string username;
    public string fullName;
    public bool isPrivate;
    public bool isVerified;

    public int following;
    public int follower;
    public int mediaCount;

    public string email;
    public string phoneCountryCode;
    public string phoneNumber;
    public bool isBussiness; 
  }
}