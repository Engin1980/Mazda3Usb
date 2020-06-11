using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENG.Mazda3usb.Lib
{
  public static class Processor
  {
    public static void ProcessPath(string path, ProcessorSettings settings)
    {
      ProcessPath(path, settings, true);
    }

    private static void ProcessPath(string parent, ProcessorSettings settings, bool topLevel)
    {
      parent = NormalizePath(parent);
      // check dir
      if (System.IO.Directory.Exists(parent) == false)
      {
        Output.Print("Folder " + parent + " does not exist.");
        Output.Print("\t... skipped.");
        return;
      }

      if (topLevel && settings.UseStoredHashes)
        HashManager.TryLoadHashes(parent);

      if (settings.OrderFolders)
        FolderMover.ProcessFolder(parent);

      if (settings.OrderFiles)
      {
        FilesMover.ProcessFolder(parent);
      }

      if (settings.Recursive)
      {
        var dirs = System.IO.Directory.GetDirectories(parent);
        foreach (var dir in dirs)
        {
          Processor.ProcessPath(dir, settings, false);
        }
      }

      if (topLevel)
        HashManager.TrySaveHashes(parent);
    }

    private static string NormalizePath(string path)
    {
      if (path.EndsWith("\\") == false) path = path + "\\";
      return path;
    }
  }
}
