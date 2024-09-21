using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudioStarter.Utils
{
    public static class StaticBinder
    {
        public static bool IsVs2019Installed { get; set; }
        public static bool IsVs2022Installed { get; set; }
        public static bool IsVs2022PreInstalled { get; set; }
        public static bool IsVsCodeInstalled { get; set; }
    }
}
