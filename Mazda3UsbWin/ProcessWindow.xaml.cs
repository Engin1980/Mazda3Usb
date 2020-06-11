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
using ENG.Mazda3usb;
using ENG.Mazda3usb.Lib;

namespace ENG.Mazda3usb.Win
{
  /// <summary>
  /// Interaction logic for ProcessWindow.xaml
  /// </summary>
  public partial class ProcessWindow : Window
  {
    private string path;
    private ProcessorSettings sett;
    private Brush InfoBrush = new SolidColorBrush(Colors.Yellow);
    private Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(255, 150, 150));
    private Brush VerboseBrush = new SolidColorBrush(Colors.White);


    public ProcessWindow()
    {
      InitializeComponent();
    }

    public void StartProcess(string path, ProcessorSettings settings)
    {
      Output.PrintMethod = this.Print;

      this.path = path;
      this.sett = settings;

      System.Threading.ThreadStart ts = new System.Threading.ThreadStart(this.Run);
      System.Threading.Thread t = new System.Threading.Thread(ts);
      t.Start();
    }

    private void Run()
    {
      Processor.ProcessPath(this.path, this.sett);
      Print("--- Done. ---", Output.LevelInfo.Verbose);
    }

    public void Print(string text, Output.LevelInfo level)
    {
      if (CheckAccess())
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString());
        sb.Append("\t");
        sb.Append(text);

        ListViewItem lvi = new ListViewItem();
        lvi.Content = sb.ToString();
        switch (level)
        {
          case Output.LevelInfo.Error:
            lvi.Background = this.ErrorBrush;
            break;
          case Output.LevelInfo.Info:
            lvi.Background = this.InfoBrush;
            break;
          case Output.LevelInfo.Verbose:
            lvi.Background = this.VerboseBrush;
            break;
        }

        lst.Items.Add(lvi);
        lst.ScrollIntoView(lvi);
      }
      else
      {
        Action a = () => Print(text, level);
        Dispatcher.BeginInvoke(a);        
      }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}
