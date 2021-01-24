namespace InstagramAPI.Models {
  public class ProxyModel {
    public string ipAddress;
    public int port;

    public ProxyModel(string ipAddress, int port) {
      this.ipAddress = ipAddress;
      this.port = port;
    }
  }
}