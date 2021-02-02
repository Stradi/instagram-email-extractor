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

namespace InstagramEmailExtractor.CustomControls {
  public partial class NumberPicker : UserControl {
    private int _numValue = 0;

    public int NumValue {
      get {
        return _numValue;
      } set {
        _numValue = value;
        tb_Number.Text = _numValue.ToString();
      }
    }

    public new Brush BorderBrush {
      get {
        return tb_Number.BorderBrush;
      } set {
        tb_Number.BorderBrush = value;
      }
    }

    public NumberPicker() {
      InitializeComponent();

      tb_Number.Text = _numValue.ToString();
    }

    private void tb_Number_TextChanged(object sender, TextChangedEventArgs e) {
      if(!int.TryParse(tb_Number.Text, out _numValue)) {
        tb_Number.Text = _numValue.ToString();
      }
    }

    private void btn_Up_Click(object sender, RoutedEventArgs e) {
      NumValue++;
    }

    private void btn_Down_Click(object sender, RoutedEventArgs e) {
      NumValue--;
    }
  }
}
