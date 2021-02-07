using System;
using System.Collections.Generic;
using System.Linq;

namespace ServisApp.Util
{
    public static class ImageHelper
    {
        public static string GetImageType(this byte[] imageBytes)
        {
            foreach (var imageType in imageFormatDecoders)
            {
                if (imageType.Key.SequenceEqual(imageBytes.Take(imageType.Key.Length)))
                    return imageType.Value;
            }

            throw new ArgumentException("unknown");
        }

        private static readonly Dictionary<byte[], string> imageFormatDecoders = new Dictionary<byte[], string>()
        {
            { new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, "png" },
            { new byte[]{ 0xff, 0xd8 }, "jpg" }
        };
    }
}
