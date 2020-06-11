using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENG.Mazda3usb.Lib
{
  public static class FolderMover
  {
    public static void ProcessFolder(string folder)
    {
      Output.Print(folder, Output.LevelInfo.Info);

      Output.Print("\tgetting subfolders");
      string[] folders = System.IO.Directory.GetDirectories(folder);

      int savedHash = HashManager.Get(folder);
      int currentHash = HashManager.JoinHashes(folders);
      if (savedHash == currentHash)
      {
        Output.Print("\t\tno change of subfolders, deleting temp and skipping");
        DeleteTempFolders(folder);
        return;
      }

      Array.Sort(folders);

      bool hasSomeErrorOccured = false;
      for (int i = 0; i < folders.Length; i++)
      {
        try
        {
          string name;
          string fullTempName;
          PrepareNamesForFolder(folder, folders, i, out name, out fullTempName);

          Output.Print("\t" + name + " (updating location)");
          ProcessFolderMoving(folders, i, fullTempName);
        }
        catch (Exception ex)
        {
          Output.Print("\t\t - error: " + ex.Message, Output.LevelInfo.Error);
          hasSomeErrorOccured = true;
        }
      }

      DeleteTempFolders(folder);

      folders = System.IO.Directory.GetDirectories(folder);
      if (!hasSomeErrorOccured)
      {
        currentHash = HashManager.JoinHashes(folders);
        HashManager.Set(folder, currentHash);
      }
    }

    private static void ProcessFolderMoving(string[] folders, int i, string fullTempName)
    {
      // prejmenuju puvodni na temp
      System.IO.Directory.Move(folders[i], fullTempName);
      // udelam novy spravny nazev
      System.IO.Directory.CreateDirectory(folders[i]);
      // presunu obsah tempu do noveho
      MoveBack(fullTempName, folders[i]);
      // smazu temp
      System.IO.Directory.Delete(fullTempName);
    }

    private static void PrepareNamesForFolder(string folder, string[] folders, int i, out string name, out string fullTempName)
    {
      name = System.IO.Path.GetFileName(folders[i]);
      string tempName = "_TMP"; // + i.ToString("000");
      fullTempName = System.IO.Path.Combine(folder, tempName);
    }

    private static void DeleteTempFolders(string folder)
    {
      var flds = System.IO.Directory.GetDirectories(folder);
      foreach (var tmp in flds)
      {
        var folderName = System.IO.Path.GetFileName(tmp);
        if (folderName.StartsWith("_TMP") == false) continue;
        if (IsFolderEmpty(folderName) == false) continue;
        try
        {
          System.IO.Directory.Delete(tmp);
        }
        catch (Exception ex)
        {
          Output.Print("Failed to remove temp folder " + tmp + ": " + ex.Message, Output.LevelInfo.Error);
        }
      }
    }

    private static bool IsFolderEmpty(string folderName)
    {
      return
        System.IO.Directory.GetFileSystemEntries(folderName).Length == 0;
    }

    private static void MoveBack(string source, string target)
    {
      foreach (var dir in System.IO.Directory.GetDirectories(source))
      {
        string pom = System.IO.Path.Combine(target, System.IO.Path.GetFileName(dir));
        System.IO.Directory.Move(dir, pom);
      }
      foreach (var file in System.IO.Directory.GetFiles(source))
      {
        string pom = System.IO.Path.Combine(target, System.IO.Path.GetFileName(file));
        System.IO.File.Move(file, pom);
      }
    }
  }
}
