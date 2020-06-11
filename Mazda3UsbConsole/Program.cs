using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENG.Mazda3usb.Lib;

namespace ENG.Mazda3usb.ConsoleApp
{
  class Program
  {
    static void Main(string[] args)
    {
      Output.PrintMethod = Program.Print;

      // get dir
      string parent = GetDirectory(args);
      Output.Print("Directory to process is :", Output.LevelInfo.Info);
      Output.Print("\t" + parent, Output.LevelInfo.Info);
      Output.Print("Is ty correct? Press \"y\" to continue");
      string answer = Console.ReadLine();
      if (answer.ToLower() != "y")
      {
        Print("Program quit.", Output.LevelInfo.Info);
        return;
      }

      bool recursive = GetRecursive(args);
      Output.Print(recursive ? "Program will run recursively over folders" : "Program will run only for selected path.", 
        Output.LevelInfo.Info);

      ProcessorSettings sett = ProcessorSettings.CreateDefault();
      sett.Recursive = recursive;
      sett.OrderFiles = true;
      sett.OrderFolders = true;
      Processor.ProcessPath(parent, sett);
    }

    private static void Print(string text, Output.LevelInfo level)
    {
      var col = Console.ForegroundColor;

      switch (level)
      {
        case Output.LevelInfo.Error:
          Console.ForegroundColor = ConsoleColor.Red;
          break;
        case Output.LevelInfo.Info:
          Console.ForegroundColor = ConsoleColor.Gray;
          break;
        case Output.LevelInfo.Verbose:
          Console.ForegroundColor = ConsoleColor.Yellow;
          break;
      }

      Console.WriteLine(text);

      Console.ForegroundColor = col;
    }

    private static string GetDirectory(string[] args)
    {
      string ret;
      if (args.Length < 1)
      {
        Console.WriteLine("Set name of the directory to work with:");
        ret = Console.ReadLine();
      }
      else
      {
        ret = args[0];
      }
      return ret;
    }

    private static bool GetRecursive(string[] args)
    {
      bool ret;
      if (args.Length < 2)
      {
        Console.WriteLine("Would you like to process folders recursively? Enter \"R\" or \"r\" to do so, or just enter otherwise:");
        string pom = Console.ReadLine();
        ret = (pom == "r" || pom == "R");
      }
      else
      {
        ret = (args[1] == "/r" || args[1] == "/R");
      }
      return ret;
    }
  }
}
