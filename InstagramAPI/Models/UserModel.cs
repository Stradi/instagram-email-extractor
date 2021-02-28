namespace InstagramAPI.Models {
  public class UserModel {
    public string userId { get; set; }
    public string username { get; set; }
    public string fullName;
    public bool isPrivate;
    public bool isVerified;

    public int following;
    public int follower;
    public int mediaCount;

    public string email { get; set; }
    public string phoneCountryCode { get; set; }
    public string phoneNumber { get; set; }
    public bool isBussiness; 
  }
}