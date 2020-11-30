using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Ssepan.Graphics
{

    /// <summary>
    /// Based on 'Community Content' section on MSDN documentation for Image.Save method (http://msdn.microsoft.com/en-us/library/ytz20d80.aspx).
    /// Extension Methods to aide with encoders:
    ///"This may help to get:
    ///- mime type from Image
    ///- get an encoder for the mime type or a default encoder"
    /// </summary>
    public static class ImageExtensions
    {
        public static ImageCodecInfo GetImageCodecInfo(this Image image)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                //if (codec.FormatID == image.RawFormat.Guid)
                if (codec.FormatID.ToString() == image.RawFormat.Guid.ToString())
                {
                    return codec;
                }
            }
            return null;
        }

        public static String GetMimeType(this Image image)
        {
            string sReturn = string.Empty;
            if (image.RawFormat.Guid == ImageFormat.Bmp.Guid)
                sReturn = "image/bmp";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Emf.Guid))
                sReturn = "image/emf";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Exif.Guid))
                sReturn = "image/exif";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Gif.Guid))
                sReturn = "image/gif";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Icon.Guid))
                sReturn = "image/icon";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Jpeg.Guid))
                sReturn = "image/jpeg";
            else if (image.RawFormat.Guid.Equals(ImageFormat.MemoryBmp.Guid))
                sReturn = "image/membmp";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Png.Guid))
                sReturn = "image/png";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Tiff.Guid))
                sReturn = "image/tiff";
            else if (image.RawFormat.Guid.Equals(ImageFormat.Wmf.Guid))
                sReturn = "image/wmf";
            else
                sReturn = "image/jpeg";
            return sReturn;
        }

        public static ImageCodecInfo GetDecoderInfoOrDefault(this String mimeType)
        {
            var decoders = ImageCodecInfo.GetImageDecoders();
            var existingDecoder = decoders.FirstOrDefault(dec => dec.MimeType.Equals(mimeType));
            // should the specific encoder not be found, then return the JPG encoder by default.
            if (existingDecoder == null)
            {
                existingDecoder = decoders.SingleOrDefault(enc => enc.MimeType.Equals("image/jpeg"));
            }
            // should the default encoder not exist, then return any encoder or null if none is present.
            if (existingDecoder == null)
            {
                existingDecoder = (decoders.Length > 0) ? decoders[0] : null;
            }
            return existingDecoder;
        }

        public static ImageCodecInfo GetDecoderInfoOrDefault(this Image image)
        {
            return GetDecoderInfoOrDefault(image.GetMimeType());
        }

        public static ImageCodecInfo GetEncoderInfoOrDefault(this String mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            var existingEncoder = encoders.FirstOrDefault(enc => enc.MimeType.Equals(mimeType));
            // should the specific encoder not be found, then return the JPG encoder by default.
            if (existingEncoder == null)
            {
                existingEncoder = encoders.SingleOrDefault(enc => enc.MimeType.Equals("image/jpeg"));
            }
            // should the default encoder not exist, then return any encoder or null if none is present.
            if (existingEncoder == null)
            {
                existingEncoder = (encoders.Length > 0) ? encoders[0] : null;
            }
            return existingEncoder;
        }


        public static ImageCodecInfo GetEncoderInfoOrDefault(this Image image)
        {
            return GetEncoderInfoOrDefault(image.GetMimeType());
        }
    }
}
