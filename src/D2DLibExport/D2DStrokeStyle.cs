using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using FLOAT = System.Single;
using UINT = System.UInt32;
using UINT32 = System.UInt32;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HRESULT = System.Int64;
using BOOL = System.Int32;


namespace nud2dlib
{
    public class D2DStrokeStyle : D2DObject
    {
        public D2DDevice Device { get; }

        public float[] Dashes { get; }

        public float DashOffset { get; }

        public D2DCapStyle StartCap { get; } = D2DCapStyle.Flat;

        public D2DCapStyle EndCap { get; } = D2DCapStyle.Flat;


        internal D2DStrokeStyle(D2DDevice Device, HANDLE handle, float[] dashes, float dashOffset, D2DCapStyle startCap, D2DCapStyle endCap)
            : base(handle)
        {
            this.Dashes = dashes;
            this.DashOffset = dashOffset;
            this.StartCap = startCap;
            this.EndCap = endCap;
        }
    }
}
