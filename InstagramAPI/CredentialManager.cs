using System.Collections.Generic;

using InstagramAPI.Models;

namespace InstagramAPI {
  public class CredentialManager {
    private List<CredentialModel> credentials;
    private int requestLimit;

    private int currentCredentialIndex = 0;

    public int TotalCredentials {
      get {
        return credentials.Count;
      }
    }

    public CredentialManager(int requestLimit = 200) {
      this.requestLimit = requestLimit;

      this.credentials = new List<CredentialModel>();
    }

    public void AddCredential(CredentialModel cred) {
      credentials.Add(cred);
    }

    public CredentialModel GetCredential() {
      if(credentials.Count <= 0) {
        return null;
      }

      if(credentials[currentCredentialIndex].TotalRequests < requestLimit) {
        CredentialModel cred = credentials[currentCredentialIndex];
        
        currentCredentialIndex++;
        if(currentCredentialIndex >= credentials.Count) {
          currentCredentialIndex = 0;
        }
        
        return cred;
      }

      return null;
    }
  }
}