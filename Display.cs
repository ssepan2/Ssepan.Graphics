using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace Ssepan.Graphics
{
    public class Display
    {
        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        private struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel; //color depth
            public int dmPelsWidth; //width
            public int dmPelsHeight; //height
            public int dmDisplayFlags;
            public int dmDisplayFrequency; //frequency
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public static Boolean GetDisplayModes(ref List<KeyValuePair<String, String>> XYResolution)
        {
            DEVMODE devModeStruct = new DEVMODE();

            XYResolution = new List<KeyValuePair<String, String>>();
            int i = 0;

            while (EnumDisplaySettings(null, i, ref devModeStruct))
            {
                int colorDepth = (1 << devModeStruct.dmBitsPerPel);
                string key = String.Format("{0} X {1}", devModeStruct.dmPelsWidth, devModeStruct.dmPelsHeight);
                string value = String.Format("{0},{1}", devModeStruct.dmPelsWidth, devModeStruct.dmPelsHeight);

                if (colorDepth == 65536)
                {
                    if (!XYResolution.Exists(k => k.Key == key))
                    {
                        XYResolution.Add(new KeyValuePair<String, String>(key, value));
                    }
                }
                i++;
            }
            return true;
        }
    }
}
