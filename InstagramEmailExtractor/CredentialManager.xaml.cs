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

using InstagramEmailExtractor.Models;

namespace InstagramEmailExtractor {
  public partial class CredentialManager : Page {
    private List<Credential> credentialItems;
    private int idCounter;

    public CredentialManager() {
      InitializeComponent();

      credentialItems = new List<Credential>();
      CredentialList.ItemsSource = credentialItems;

      idCounter = 0;
    }

    private void btn_AddCredential_AddOne(object sender, RoutedEventArgs e) {
      if(!ValidateTextboxes(tb_AddCredential_Username, tb_AddCredential_Password)) {
        return;
      }

      string username = tb_AddCredential_Username.Text;
      string password = tb_AddCredential_Password.Text;

      AddNewCredential(username, password);

      ResetTextboxColors(tb_AddCredential_Username, tb_AddCredential_Password);
      tb_AddCredential_Username.Text = "";
      tb_AddCredential_Password.Text = "";
    }

    private void btn_AddCredential_Load(object sender, RoutedEventArgs e) {
      //TODO: Fill this.
    }

    private void AddNewCredential(string username, string password) {
      bool alreadyExists = credentialItems.Where(cred => cred.Username == username).ToArray().Length > 0;
      if(alreadyExists) {
        MessageBox.Show("The account you've tried to add already added.", "Could not add new Credential", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return;
      }

      Credential c = new Credential() {
        Id = idCounter++,
        Username = username,
        Password = password,
        Status = "Newly added"
      };

      credentialItems.Add(c);
      CredentialList.Items.Refresh();
    }

    private bool ValidateTextboxes(params TextBox[] inputs) {
      bool hasErrors = false;
      for(int i = 0; i < inputs.Length; i++) {
        if(string.IsNullOrEmpty(inputs[i].Text)) {
          inputs[i].BorderBrush = Brushes.Red;
          hasErrors = true;
        }
      }

      return !hasErrors;
    }

    private void ResetTextboxColors(params TextBox[] inputs) {
      for(int i = 0; i < inputs.Length; i++) {
        inputs[i].BorderBrush = Brushes.LightGray;
      }
    }
  }
}
