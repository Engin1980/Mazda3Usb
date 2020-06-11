using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ENG.Mazda3usb.Lib
{
  public static class FilesMover
  {

    internal static void ProcessFolder(string folder)
    {
      Output.Print(folder, Output.LevelInfo.Info);
      Output.Print("\tgetting files");

      string[] files = System.IO.Directory.GetFiles(folder);
      Array.Sort(files);

      int savedHash = HashManager.Get(folder + "*");
      int currentHash = HashManager.JoinHashes(files);
      if (savedHash == currentHash)
      {
        Output.Print("\t\tno change of files, skipping");
        return;
      }

      foreach (var fileName in files)
      {
        ProcessFile(fileName);
      }

      files = System.IO.Directory.GetFiles(folder);
      currentHash = HashManager.JoinHashes(files);
      HashManager.Set(folder + "*", currentHash);
    }

    private static void ProcessFile(string fullFileName)
    {
      Output.Print("\t\tmoving file " + fullFileName);

      string origPath = System.IO.Path.GetDirectoryName(fullFileName);
      string tmpPath = System.IO.Path.GetPathRoot(fullFileName);
      string tmpName = "TMP.mp3";

      string fullTmpName = System.IO.Path.Combine(tmpPath, tmpName);

      System.IO.File.Move(fullFileName, fullTmpName);
      System.IO.File.Move(fullTmpName, fullFileName);

      //string fileName = System.IO.Path.GetFileName(fullFileName);
      //string pattern = ("^(\\d)\\D.+");
      //var m = Regex.Match(fileName, pattern);
      //if (m.Success)
      //{
      //  string newFileName = "0" + fileName;
      //  string newFullFileName =
      //    System.IO.Path.Combine(
      //      System.IO.Path.GetDirectoryName(fullFileName),
      //      newFileName);

      //  Output.Print("\t" + fileName + " (renaming)", Output.LevelInfo.Info);
      //  System.IO.File.Move(fullFileName, newFullFileName);
      //}
    }
  }
}
