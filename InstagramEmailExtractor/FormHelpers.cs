using System.Windows.Controls;
using System.Windows.Media;

namespace InstagramEmailExtractor {
  public static class FormHelpers {
    public static bool ValidateTextboxes(params TextBox[] inputs) {
      bool hasErrors = false;
      for(int i = 0; i < inputs.Length; i++) {
        if(string.IsNullOrEmpty(inputs[i].Text)) {
          inputs[i].BorderBrush = Brushes.Red;
          hasErrors = true;
        }
      }

      return !hasErrors;
    }

    public static void ResetTextboxColors(params TextBox[] inputs) {
      for(int i = 0; i < inputs.Length; i++) {
        inputs[i].BorderBrush = Brushes.LightGray;
      }
    }
  }
}
