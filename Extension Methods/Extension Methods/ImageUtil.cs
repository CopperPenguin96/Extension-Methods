using System;
using System.Drawing;
using System.IO;

namespace Extension_Methods
{
    public static class ImageUtil
    {
        public static string ToString(this Image image)
        { 
            if (image == null) throw new ArgumentNullException(nameof(image));
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            byte[] array = ms.ToArray();
            return Convert.ToBase64String(array);
        }
    }
}
