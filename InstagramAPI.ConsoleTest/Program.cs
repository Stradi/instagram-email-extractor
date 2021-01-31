using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Models;
using InstagramAPI.Responses;

namespace InstagramAPI.ConsoleTest {
  class Program {
    private static Instagram instagram;
    private const string DATA_DIRECTORY = "./data";
    private const string CRED_FILENAME = "credentials.json";
    private static string CRED_PATH = string.Format("{0}/{1}", DATA_DIRECTORY, CRED_FILENAME);

    static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();    
    }

    private static async Task LoadCredentials(CredentialModel[] credentials) {
      //Check if we logged in before.      
      if(Directory.Exists(DATA_DIRECTORY)) {
        //Read cred.json file
        string[] credentialJsons = File.ReadAllLines(CRED_PATH);
        foreach(string credentialJson in credentialJsons) {
          instagram.credentialManager.AddCredential(CredentialModel.Deserialize(credentialJson));
        }
      } else {
        Directory.CreateDirectory(DATA_DIRECTORY);
        //Login with provided credentials and add them to cred.json file
        List<CredentialModel> loggedInCredentials = new List<CredentialModel>();
        foreach(CredentialModel credential in credentials) {
          LoginResponse response = await instagram.LoginAsync(credential);
          if(response.IsAuthenticated) {
            loggedInCredentials.Add(credential);
            Console.WriteLine("Login successfull with {0}.", credential.username);
          } else {
            Console.WriteLine("Login failed with {0}.", credential.username);
          }
        }

        File.AppendAllLines(CRED_PATH, loggedInCredentials.Select(c => c.Serialize()));
      }
    }

    private static async Task MainAsync() {
      List<ProxyModel> proxies = new List<ProxyModel>();
      List<CredentialModel> credentials = new List<CredentialModel>();
      credentials.Add(new CredentialModel("", ""));
      
      instagram = new Instagram(proxies);

      await LoadCredentials(credentials.ToArray());

      instagram.OnUserFetched += OnUserFetched;

      PartialUserModel[] users = await instagram.GetPostLikers("", total: 250);
      Console.WriteLine("Got likers of post.");
      Console.WriteLine("Total user count is {0}.", users.Length);
    }

    private static void OnUserFetched(PartialUserModel[] users, string endCursor) {
      Console.WriteLine("Fetched {0} users.", users.Length);
      Console.WriteLine("{0} of them are private.", users.Where(user => user.isPrivate == true).Select(s => s).ToArray().Length); 
    }
  }
}