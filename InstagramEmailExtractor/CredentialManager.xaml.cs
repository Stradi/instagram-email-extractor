using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

using InstagramAPI.Responses;

using InstagramEmailExtractor.Models;

namespace InstagramEmailExtractor {
  public partial class CredentialManager : Page {
    private const string DATA_DIRECTORY = "./data";
    private const string CRED_FILENAME = "creds.json";
    private string CRED_PATH = string.Format("{0}/{1}", DATA_DIRECTORY, CRED_FILENAME); 

    private List<Credential> credentialItems;
    private int idCounter;

    public CredentialManager() {
      InitializeComponent();

      credentialItems = new List<Credential>();
      CredentialList.ItemsSource = credentialItems;

      idCounter = 0;
      LoadCredentials();
    }

    private void LoadCredentials() {
      if(Directory.Exists(DATA_DIRECTORY)) {
        //If directory exists but file does not exists
        //that means structure is corrupted. Just
        //create a file and exit function.
        if(!File.Exists(CRED_PATH)) {
          File.Create(CRED_PATH);
          return;
        }

        //Read all lines from file and add that
        //logged in credentials to Instagram.credentialManager
        //also add them to ListView.
        string[] credsJson = File.ReadAllLines(CRED_PATH);
        for(int i = 0; i < credsJson.Length; i++) {
          string currentCredential = credsJson[i];
          InstagramAPI.Models.CredentialModel credModel = InstagramAPI.Models.CredentialModel.Deserialize(currentCredential);
          MainWindow.Instagram.credentialManager.AddCredential(credModel);
          
          AddNewCredential(credModel.username, credModel.password, true);
        }
      } else {
        //If directory not exists create.
        Directory.CreateDirectory(DATA_DIRECTORY);
      }
    }

    private void btn_AddCredential_AddOne(object sender, RoutedEventArgs e) {
      if(!FormHelpers.ValidateTextboxes(tb_AddCredential_Username, tb_AddCredential_Password)) {
        return;
      }

      string username = tb_AddCredential_Username.Text;
      string password = tb_AddCredential_Password.Text;

      AddNewCredential(username, password);

      FormHelpers.ResetTextboxColors(tb_AddCredential_Username, tb_AddCredential_Password);
      tb_AddCredential_Username.Text = "";
      tb_AddCredential_Password.Text = "";
    }

    private void btn_AddCredential_Load(object sender, RoutedEventArgs e) {
      //TODO: Fill this.
    }

    private void btn_AddProxy_AddOne(object sender, RoutedEventArgs e) {
    
    }

    private void btn_AddProxy_Load(object sender, RoutedEventArgs e) {
    
    }
    
    private async void AddNewCredential(string username, string password, bool loadedFromFile = false) {
      bool alreadyExists = credentialItems.Where(cred => cred.Username == username).ToArray().Length > 0;
      if(alreadyExists) {
        //Log error
        return;
      }

      Credential.StatusMessages status = !loadedFromFile ? Credential.StatusMessages.NewlyAdded : Credential.StatusMessages.LoadedFromFile;
      Credential c = new Credential() {
        Id = idCounter++,
        Username = username,
        Password = password,
        Status = status
      };

      credentialItems.Add(c);
      CredentialList.Items.Refresh();

      if(!loadedFromFile) {
        //await LoginCredential(c);
      }
    }

    private async Task LoginCredential(Credential cred) {
      ChangeCredentialStatus(cred, Credential.StatusMessages.LoggingIn);
      LoginResponse resp = await MainWindow.Instagram.LoginAsync(new InstagramAPI.Models.CredentialModel(cred.Username, cred.Password));

      if(!resp.IsSuccessfull) {
        ChangeCredentialStatus(cred, Credential.StatusMessages.Error);
        MessageBox.Show(resp.ErrorMessage);
        return;
      }

      if(resp.IsAuthenticated) {
        ChangeCredentialStatus(cred, Credential.StatusMessages.LoggedIn);
        //Save to file
        string serialized = MainWindow.Instagram.credentialManager.FindWithUsername(cred.Username).Serialize();
        await File.AppendAllLinesAsync(CRED_PATH, new[] { serialized });

      } else {
        ChangeCredentialStatus(cred, Credential.StatusMessages.Error);
        //Username or password is wrong.
      }
    }

    private void ChangeCredentialStatus(Credential cred, Credential.StatusMessages statusMessage) {
      cred.Status = statusMessage;
      CredentialList.Items.Refresh();
    }
  }
}
