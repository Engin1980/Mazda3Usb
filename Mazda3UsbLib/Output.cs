using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENG.Mazda3usb.Lib
{
  public static class Output
  {
    public enum LevelInfo
    {
      Info,
      Verbose,
      Error
    }

    public delegate void PrintDelegate (string text, LevelInfo level);
    public static PrintDelegate PrintMethod { get; set; }

    public static void Print() { Print(""); }

    public static void Print(string text)
    {
      Print(text, LevelInfo.Verbose);
    }

    public static void Print(string text, LevelInfo level)
    {
      if (PrintMethod != null)
        PrintMethod(text, level);
    }
  }
}
