namespace InstagramEmailExtractor.Models {  
  public class Credential {
    public class StatusMessages {
      private StatusMessages(string val) {
        Value = val;
      }

      public string Value {
        get; set;
      }

      public override string ToString() {
        return Value;
      }

      public static StatusMessages NewlyAdded { get { return new StatusMessages("Newly added"); }}
      public static StatusMessages LoggingIn { get { return new StatusMessages("Logging in"); }}
      public static StatusMessages LoggedIn { get { return new StatusMessages("Logged in"); }}
      public static StatusMessages Error { get { return new StatusMessages("Login failed"); }}
      public static StatusMessages LoadedFromFile { get { return new StatusMessages("Loaded from file"); }}
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public StatusMessages Status { get; set; }
  }
}
