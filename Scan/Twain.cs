using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Ssepan.Utility;

namespace Ssepan.Graphics.Scan
{
    public static class Twain
    {
        public const String VirtualScanner = "(virtual)";

        /// <summary>
        /// Obtain an Image from the specified Twain scanner.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="image"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean Capture(ScanOptions options, out Image image, ref String errorMessage)
        {
            Boolean returnValue = default(Boolean);
            String filePath = String.Empty;
            Int32 randomNumber = default(Int32);
            image = default(Image);
            
            try
            {
                if (options.ScannerName == VirtualScanner)
                {
                    //perform virtual scan
                    Random r = new Random();
                    randomNumber = r.Next(0, 9);
                    filePath = Path.Combine(options.VirtualScanPath, String.Format("{0}.bmp", randomNumber));
                    image = Image.FromFile(filePath);
                }
                else
                {
                    //TODO:scan image via twain
                    //image = ???;
                }

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
        /// Obtain a list of available Twain scanner names.
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Boolean Devices(out List<String> devices, ref String errorMessage)
        { 
            Boolean returnValue = default(Boolean);
            devices = new List<string>();
            
            try
            {
                devices.Add(VirtualScanner);
                //TODO:read device list via twain

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
