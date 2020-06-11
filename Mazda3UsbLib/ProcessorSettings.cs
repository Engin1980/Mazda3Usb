using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENG.Mazda3usb.Lib
{
  public struct ProcessorSettings
  {
    public bool Recursive { get; set; }
    public bool OrderFolders { get; set; }
    public bool OrderFiles { get; set; }
    public bool UseStoredHashes { get; set; }

    public static ProcessorSettings CreateDefault()
    {
      ProcessorSettings ret = new ProcessorSettings();

      ret.Recursive = true;
      ret.OrderFiles = true;
      ret.OrderFolders = true;
      ret.UseStoredHashes = true;

      return ret;
    }
  }
}
