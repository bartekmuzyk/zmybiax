using System;
using System.Collections.Generic;
using System.Text;
using IL2CPU.API.Attribs;

namespace Zmybiax.Graphics
{
    public static class ShellResources
    {
        [ManifestResourceStream(ResourceName = "Zmybiax.Graphics.Shell.Dock.bmp")]
        public static byte[] Dock;

        [ManifestResourceStream(ResourceName = "Zmybiax.Graphics.Shell.App.bmp")]
        public static byte[] AppIcon;

        [ManifestResourceStream(ResourceName = "Zmybiax.Graphics.Shell.Cursor.bmp")]
        public static byte[] Cursor;
    }
}
