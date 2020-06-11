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
using System.Windows.Shapes;

namespace ENG.Mazda3usb.Win
{
  /// <summary>
  /// Interaction logic for SelectFolderWindow.xaml
  /// </summary>
  public partial class SelectFolderWindow : Window
  {
    private const string EMPTY_NODE_TEXT = "(loading)";
    private bool dialogOk = false;

    public SelectFolderWindow()
    {
      InitializeComponent();
      InitTree();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      
    }

    private void InitTree()
    {
      var roots = System.IO.DriveInfo.GetDrives();

      roots = roots.OrderBy(q => q.Name).ToArray();

      foreach (var item in roots)
      {
        TreeViewItem tvi = new TreeViewItem();
        string volLabel;
        try
        {
          volLabel = item.VolumeLabel;
        }
        catch (Exception)
        {
          volLabel = "???";
        }
        tvi.Header = item.RootDirectory + " (" + volLabel + ")";
        tvi.Tag = item.RootDirectory.FullName;
        tvi.Items.Add(GenerateEmptyTVI());
        tvi.Expanded += new RoutedEventHandler(tvi_Expanded);

        tvw.Items.Add(tvi);
      }
    }

    void tvi_Expanded(object sender, RoutedEventArgs e)
    {
      TreeViewItem tvi = sender as TreeViewItem;
      if (tvi.Items.Count == 1 && IsEmptyTVI(tvi.Items[0] as TreeViewItem))
      {
        ExpandNode(tvi);
      }
    }

    private void ExpandNode(TreeViewItem parent)
    {
      parent.Items.Clear();
      var subdirs = System.IO.Directory.GetDirectories(parent.Tag as string);
      foreach (var item in subdirs)
      {
        TreeViewItem tvi = new TreeViewItem();
        tvi.Header = System.IO.Path.GetFileName(item);
        tvi.Tag = item;
        tvi.Items.Add(GenerateEmptyTVI());
        tvi.Expanded += new RoutedEventHandler(tvi_Expanded);

        parent.Items.Add(tvi);
      }
    }

    private bool IsEmptyTVI(TreeViewItem tvi)
    {
      return ((string) tvi.Header) == EMPTY_NODE_TEXT;
    }

    private TreeViewItem GenerateEmptyTVI()
    {
      TreeViewItem tvi = new TreeViewItem();
      tvi.Header = EMPTY_NODE_TEXT;
      return tvi;
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      this.dialogOk = true;
      this.Hide();
    }

    public string GetSelectedFolder()
    {
      TreeViewItem tvi = tvw.SelectedItem as TreeViewItem;
      string ret;
      if (tvi != null && dialogOk)
        ret = tvi.Tag as string;
      else
        ret = null;
      return ret;
    }

    private void tvw_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (tvw.SelectedItem != null)
      {
        btnOk_Click(null, null);
      }
    }
  }
}
