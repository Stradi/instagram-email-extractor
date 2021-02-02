using System.Windows.Controls;
using System.Windows.Media;

namespace InstagramEmailExtractor {
  public static class FormHelpers {
    public static bool ValidateTextboxes(params TextBox[] inputs) {
      bool hasErrors = false;
      for(int i = 0; i < inputs.Length; i++) {
        if(string.IsNullOrEmpty(inputs[i].Text)) {
          ChangeControlsBorderColor(Brushes.Red, inputs[i]);
          hasErrors = true;
        }
      }

      return !hasErrors;
    }

    public static void ResetTextboxColors(params TextBox[] inputs) {
      ChangeControlsBorderColor(Brushes.LightGray, inputs);
    }

    public static void ChangeControlsBorderColor(Brush color, params Control[] controls) {
      for(int i = 0; i < controls.Length; i++) {
        controls[i].BorderBrush = color;
      }
    }
  }
}
