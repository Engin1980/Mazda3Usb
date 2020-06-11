using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENG.Mazda3usb.Lib
{
  public class HashManager
  {
    private const char SEPARATOR = ':';

    private static Dictionary<string, int> inner = new Dictionary<string,int>();

    public static string GetHashFile(string path)
    {
      string ret = System.IO.Path.Combine(path, "hash.txt");
      return ret;
    }

    public static void TryLoadHashes(string path)
    {
      string hf = GetHashFile(path);
      if (System.IO.File.Exists(hf) == false)
        inner = new Dictionary<string, int>();
      else
        LoadHashes(hf);
    }

    public static void TrySaveHashes(string path)
    {
      string hf = GetHashFile(path);

      StringBuilder sb = new StringBuilder();
      foreach (var key in inner.Keys)
      {
        sb.Append(key);
        sb.Append(SEPARATOR);
        sb.Append(inner[key].ToString());
        sb.AppendLine();
      }

      if (System.IO.File.Exists(hf))
        System.IO.File.Delete(hf);
      System.IO.File.WriteAllText(hf, sb.ToString());
    }

    private static void LoadHashes(string hf)
    {
      var lines = System.IO.File.ReadAllLines(hf);
      inner = new Dictionary<string, int>();
      foreach (var line in lines)
      {
        var spl = line.Split(SEPARATOR);
        inner.Add(spl[0], int.Parse(spl[1]));
      }
    }

    public static int JoinHashes(string[] data)
    {
      if (data.Length == 0) return 0;
      int ret = EvalHash(System.IO.Path.GetFileName(data[0]));

      for (int i = 1; i < data.Length; i++)
      {
        int pom = EvalHash(System.IO.Path.GetFileName(data[i]));
        ret = ret + pom;
      }

      return ret;
    }

    public static int EvalHash(string data)
    {
      int ret = 0;
      for (int i = 0; i < data.Length; i++)
      {
        ret = ret + (int)data[i];
      }
      return ret;
    }

    public static int Get(string path)
    {
      path = NormKey(path);
      if (inner.ContainsKey(path)) return inner[path]; else return 0;
    }

    public static void Set(string path, int value)
    {
      path = NormKey(path);
      if (inner.ContainsKey(path)) inner[path] = value; else inner.Add(path, value);
    }

    private static string NormKey(string path)
    {
      return path.Substring(2);
    }
  }
}
