using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ENG.Mazda3usb.Lib;

namespace ENG.Mazda3usb.Win
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Brush defaultBackBrush;
    private ProcessorSettings settings = ProcessorSettings.CreateDefault();

    public MainWindow()
    {
      InitializeComponent();
      defaultBackBrush = txtFolder.Background;
      txtFolder_TextChanged(null, null);
      chkProcessFiles.IsChecked = settings.OrderFiles;
      chkProcessFolders.IsChecked = settings.OrderFolders;
      chkRecursiveFolders.IsChecked = settings.Recursive;
      chkUseHash.IsChecked = settings.UseStoredHashes;
    }

    private void txtFolder_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!System.IO.Directory.Exists(txtFolder.Text))
      {
        txtFolder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 150, 150));
        btnGo.IsEnabled = false;
      }
      else
      {
        txtFolder.Background = this.defaultBackBrush;
        btnGo.IsEnabled = true;
      }
    }

    private void btnBrowse_Click(object sender, RoutedEventArgs e)
    {
      SelectFolderWindow f = new SelectFolderWindow();
      f.ShowDialog();
      string tmp = f.GetSelectedFolder();
      if (tmp != null)
        txtFolder.Text = tmp;
    }

    private void btnGo_Click(object sender, RoutedEventArgs e)
    {
      this.settings.OrderFiles = chkProcessFiles.IsChecked.Value;
      this.settings.OrderFolders = chkProcessFolders.IsChecked.Value;
      this.settings.Recursive = chkRecursiveFolders.IsChecked.Value;
      this.settings.UseStoredHashes = chkUseHash.IsChecked.Value;

      ProcessWindow f = new ProcessWindow();
      f.Show();
      this.Close();
      f.StartProcess(txtFolder.Text, this.settings);
    }
  }
}
