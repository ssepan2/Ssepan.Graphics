using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ssepan.Graphics
{
    public static class Convert
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// From System.Drawing.Image
        /// To System.Windows.Media.Imaging.BitmapImage
        /// </summary>
        /// <param name="winformsImage"></param>
        /// <returns>WPF Image</returns>
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(this System.Drawing.Image winformsImage)
        {
            BitmapImage returnValue = default(BitmapImage);
            MemoryStream memoryStream = default(MemoryStream);  // no using here! BitmapImage will dispose the stream after loading
            
            returnValue = new BitmapImage();
            memoryStream = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            winformsImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            returnValue.BeginInit();
            returnValue.CacheOption = BitmapCacheOption.OnLoad;
            returnValue.StreamSource = memoryStream;
            returnValue.EndInit();
            
            return returnValue;
        }

        /// <summary>
        /// From System.Drawing.Image
        /// To System.Windows.Controls.Image
        /// </summary>
        /// <param name="winformsImage"></param>
        /// <returns>WPF Image</returns>
        public static System.Windows.Controls.Image ToWpfImage(this System.Drawing.Image winformsImage)
        {
            //WPF Image control
            System.Windows.Controls.Image returnValue = new System.Windows.Controls.Image();
            IntPtr hBitmap = default(IntPtr);

            if (winformsImage != null)
            {
                //convert System.Drawing.Image to WPF image
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(winformsImage))
                {
                    hBitmap = bitmap.GetHbitmap();

                    System.Windows.Media.ImageSource wpfBitmap =
                        System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                        (
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions()
                        );

                    returnValue.Source = wpfBitmap;

                    DeleteObject(hBitmap);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// From System.Windows.Media.Imaging.BitmapSource
        /// To System.Drawing.Bitmap
        /// </summary>
        /// <param name="wpfBitmap">BitmapSource to convert</param>
        /// <returns>GDI Bitmap</returns>
        public static System.Drawing.Bitmap ToGdiBitmap(this System.Windows.Media.Imaging.BitmapSource wpfBitmap)
        {
            System.Drawing.Bitmap returnValue = default(System.Drawing.Bitmap);
            PngBitmapEncoder pngBitmapEncoder = default(PngBitmapEncoder);
            MemoryStream memoryStream = default(MemoryStream);
           
            pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(wpfBitmap));
            memoryStream = new MemoryStream();
            pngBitmapEncoder.Save(memoryStream);
            
            returnValue = new System.Drawing.Bitmap(memoryStream);

            memoryStream.Close();
            memoryStream.Dispose();

            return returnValue;
        }
    }
}
