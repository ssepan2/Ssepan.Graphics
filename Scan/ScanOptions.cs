using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ssepan.Graphics.Scan
{
    public class ScanOptions
    {
        private String _ScannerName = default(String);
        public String ScannerName
        {
            get { return _ScannerName; }
            set { _ScannerName = value; }
        }

        private String _VirtualScanPath = default(String);
        public String VirtualScanPath
        {
            get { return _VirtualScanPath; }
            set { _VirtualScanPath = value; }
        }

        private Boolean _Color = default(Boolean);
        public Boolean Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        private Int32 _BitDepth = default(Int32);
        public Int32 BitDepth
        {
            get { return _BitDepth; }
            set { _BitDepth = value; }
        }
    }
}
