using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Ssepan.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Ssepan.Graphics
{
    public static class Transform
    {
        /// <summary>
        /// Rotate an image in memory.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rotateFlipType"></param>
        public static void RotateImage(ref Image image, RotateFlipType rotateFlipType)
        {
            try
            {
                image.RotateFlip(rotateFlipType);
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Rotate an image in a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="rotateFlipType"></param>
        public static void RotateImageFile(String filename, RotateFlipType rotateFlipType)
        {
            Image image = default(Image);
            String errorMessage = String.Empty;

            try
            {
                if (!Transform.LoadImageUnlocked(ref image, filename, ref errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                RotateImage(ref image, rotateFlipType);

                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);//TODO use config param
                image.Save(filename, image.GetEncoderInfoOrDefault(), encoderParameters);
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
            finally
            {
                image.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Load image from filename without locking it.
        /// If error, return false; caller will present error image.
        /// Inspired by Sciolist on stackoverflow.com: http://stackoverflow.com/questions/253435/saving-a-modified-image-to-the-original-file-using-gdi
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean LoadImageUnlocked(ref Image image, String filePath, ref String errorMessage)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //get image from file (without locking it)
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                Byte[] bytes = new Byte[fileStream.Length];
                fileStream.Read(bytes, 0, (Int32)fileStream.Length);
                fileStream.Dispose();

                MemoryStream memoryStream = new MemoryStream(bytes);
                image = Image.FromStream(memoryStream);

                returnValue = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }

        /// <summary>
        /// Load image from filename while locking it.
        /// If error, return false; caller will present error image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean LoadImageLocked(ref Image image, String filePath, ref String errorMessage)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                //get image from file (this method leave the filed locked, and changes cannot be saved)
                image = Image.FromFile(filePath);

                returnValue = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }

    }

}
