using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupApp.Managers
{
    public static class CommonManager
    {
        public static string GetDirectory(DirectoryMode mode)
        {
            var dir = string.Format(@"{0}\DB", Environment.CurrentDirectory);

            switch (mode)
            {
                case DirectoryMode.Icons:
                    dir += @"\Icons";
                    break;
            }

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

    }
}
