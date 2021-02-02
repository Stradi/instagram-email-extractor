namespace InstagramEmailExtractor.Models {  
  public class Proxy {
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
      public static StatusMessages Checking { get { return new StatusMessages("Checking"); }}
      public static StatusMessages Working { get { return new StatusMessages("Working"); }}
      public static StatusMessages NotWorking { get { return new StatusMessages("Not working"); }}
    }

    public int Id { get; set; }
    public string Host { get; set; }
    public StatusMessages Status { get; set; }
  }
}
