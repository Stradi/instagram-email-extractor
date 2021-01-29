using InstagramAPI.Models;

namespace InstagramAPI {
  public class CredentialManager {
    private CredentialModel[] credentials;
    private int requestLimit;

    private int currentCredentialIndex = 0;

    public int TotalCredentials {
      get {
        return credentials.Length;
      }
    }

    public CredentialManager(CredentialModel[] credentials, int requestLimit = 200) {
      this.credentials = credentials;
      this.requestLimit = requestLimit;
    }

    public CredentialModel GetCredential() {
      if(credentials[currentCredentialIndex].TotalRequests < TotalCredentials) {
        CredentialModel cred = credentials[currentCredentialIndex];
        
        currentCredentialIndex++;
        if(currentCredentialIndex >= credentials.Length) {
          currentCredentialIndex = 0;
        }
        
        return cred;
      }

      return null;
    }
  }
}