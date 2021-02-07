using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using InstagramAPI.Models;

namespace InstagramEmailExtractor {
  public partial class Extractor : Page {
    private Channel<PartialUserModel> fetchedUsers;
    private List<UserModel> usersWithEmail;
    private int foundUsers;
    private int extractedUsers;

    private int extractionThreadCount;
    private Thread[] extractionThreads;

    public Extractor() {
      InitializeComponent();

      fetchedUsers = new Channel<PartialUserModel>();
      MainWindow.Instagram.OnUserFetched += Instagram_OnUserFetched;

      usersWithEmail = new List<UserModel>();
      FoundEmails_List.ItemsSource = usersWithEmail;

      foundUsers = 0;
      extractedUsers = 0;

      extractionThreadCount = 2;
      extractionThreads = new Thread[extractionThreadCount];
      InitiateExtractionThreads(extractionThreadCount);
    }

    private void InitiateExtractionThreads(int numThreads) {
      for(int i = 0; i < numThreads; i++) {
        Thread t = new Thread(new ThreadStart(StartEmailExtraction));
        t.Start();
        extractionThreads[i] = t;
      }
    }
    
    private async void StartEmailExtraction() {
      while(true) {
        if(fetchedUsers.Count > 0) {
          UserModel user = null;
          try {
            user = await MainWindow.Instagram.ExtractUser(fetchedUsers.Dequeue().userId);
          } catch(Exception e) {
            SaveToFile(string.Format("crash_{0}.csv", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")), usersWithEmail.ToArray());
            continue;
          }
          extractedUsers++;

          await this.Dispatcher.BeginInvoke((Action)(() => {
            if(!string.IsNullOrEmpty(user.email)) {

              if(DoesUserMatchCriterias(user)) {
                usersWithEmail.Add(user);
                FoundEmails_List.Items.Refresh();
              }
            }

            UpdateStatusLabel();
          }));
          Thread.Sleep(1000);
        }
      }
    }

    private bool DoesUserMatchCriterias(UserModel user) {
      bool criteria = true;
      if(!(bool)cb_IncludePrivateAccounts.IsChecked && user.isPrivate) {
        criteria = false; 
      }

      if(!(bool)cb_IncludeBussinessAccounts.IsChecked && user.isBussiness) {
        criteria = false;
      }

      if(!(bool)cb_IncludeVerifiedAccounts.IsChecked && user.isVerified) {
        criteria = false;
      }

      bool doesUserHasPhone = (!string.IsNullOrEmpty(user.phoneCountryCode) && !string.IsNullOrEmpty(user.phoneNumber));
      if((bool)cb_ShouldHavePhone.IsChecked && !doesUserHasPhone) {
        criteria = false;
      }

      //TODO: Implement website and location checks.

      return criteria;
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

    private void btn_PostLikers_SaveFoundEmails(object sender, RoutedEventArgs e) {
      UserModel[] usersToSave = usersWithEmail.ToArray();
      usersWithEmail.Clear();
      SaveToFile("extracted_users.csv", usersToSave);
    }

    private void SaveToFile(string fileName, UserModel[] users) {
      StreamWriter sw = new StreamWriter(fileName, true);
      for(int i = 0; i < users.Length; i++) {
        UserModel u = users[i];
        string csv = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
          u.username, u.fullName,
          string.IsNullOrEmpty(u.email) ? "" : u.email,
          u.follower, u.following, u.mediaCount,
          u.isPrivate, u.isBussiness, u.isVerified,
          string.IsNullOrEmpty(u.phoneCountryCode) ? "" : u.phoneCountryCode,
          string.IsNullOrEmpty(u.phoneNumber) ? "" : u.phoneNumber);
        
        sw.WriteLine(csv);
        sw.Flush();
      }

      sw.Close();
    }

    private void UpdateStatusLabel() {
      lbl_ExtractionStatus.Content = string.Format("Found {0} users.\nScanned {1} users, found {2} emails.", foundUsers, extractedUsers, usersWithEmail.Count);
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
