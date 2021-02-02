using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using System.Text.RegularExpressions;

using InstagramAPI.Models;

namespace InstagramEmailExtractor {
  /// <summary>
  /// Interaction logic for Extractor.xaml
  /// </summary>
  public partial class Extractor : Page {
    private Channel<PartialUserModel> fetchedUsers;
    private List<UserModel> usersWithEmail;
    private int foundUsers;
    private int extractedUsers;

    public Extractor() {
      InitializeComponent();

      fetchedUsers = new Channel<PartialUserModel>();
      MainWindow.Instagram.OnUserFetched += Instagram_OnUserFetched;

      usersWithEmail = new List<UserModel>();
      FoundEmails_List.ItemsSource = usersWithEmail;

      foundUsers = 0;
      extractedUsers = 0;

      Thread extractThread1 = new Thread(new ThreadStart(EmailExtraction));
      extractThread1.Start();
    }
    
    private async void EmailExtraction() {
      while(true) {
        if(fetchedUsers.Count > 0) {
          UserModel user = await MainWindow.Instagram.ExtractUser(fetchedUsers.Dequeue().userId);
          extractedUsers++;
          if(!string.IsNullOrEmpty(user.email)) {
            AddExtractedUserToList(user);
          }

          await this.Dispatcher.BeginInvoke((Action)(() => {
            UpdateStatusLabel();
          }));
        }
      }
    }

    private void AddExtractedUserToList(UserModel user) {
      this.Dispatcher.BeginInvoke((Action)(() => {
        AddUserToListView(user);
      }));
    }

    private void Instagram_OnUserFetched(PartialUserModel[] user, string endCursor) {
      user.ToList().ForEach(fetchedUsers.Enqueue);
      foundUsers += user.Length;
    }

    private async void btn_PostLikers_Extract_Click(object sender, RoutedEventArgs e) {
      if(!FormHelpers.ValidateTextboxes(tb_PostLikers_URL)) {
        return;
      }

      string url = tb_PostLikers_URL.Text;
      //Parse the url
      string shortcode = ExtractShortcodeFromURL(url);
      if(string.IsNullOrEmpty(shortcode)) {
        MessageBox.Show("URL is wrong.");
      }

      int totalExtractions = np_PostLikers_ExtractionCount.NumValue;
      if(totalExtractions < 0) {
        np_PostLikers_ExtractionCount.BorderBrush = Brushes.Red;
        return;
      }
      
      np_PostLikers_ExtractionCount.BorderBrush = Brushes.LightGray;

      //TODO: You know what to do ;)
      
      //TODO: If no credential available in Instagram.credentialManager
      //show a message.
      //If throws Json exception that probably means
      //account is logged out or requires checkpoint (sms or email).
      await MainWindow.Instagram.GetPostLikers(shortcode, totalExtractions);
    }

    private void AddUserToListView(UserModel user) {
      usersWithEmail.Add(user);
      FoundEmails_List.Items.Refresh();
    }

    private void UpdateStatusLabel() {
      lbl_ExtractionStatus.Content = string.Format("Found {0} likers\n Email/Extraction Ratio {1}/{2}.", foundUsers, usersWithEmail.Count, extractedUsers);
    }

    private string ExtractShortcodeFromURL(string url) {
      Match m = Regex.Match(url, "^(?:.*\\/p\\/)([\\d\\w\\-_]+)");
      if(m.Success) {
        return m.Groups[1].Value;
      } else {
        return "";
      }
    }
  }
}
