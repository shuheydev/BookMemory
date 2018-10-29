using System;
using com.google.zxing;

namespace BookMemory
{
    public class PreviewFrameLuminanceSource : LuminanceSource
    {
        public byte[] Buffer { get; private set; }

        public PreviewFrameLuminanceSource(int width, int height)
            : base(width, height)
        {
            Buffer = new byte[width * height];
        }

        public override sbyte[] Matrix
        {
            get
            {
                return ((Buffer as Array) as sbyte[]);//(sbyte[])(Array)PreviewBufferY; 
            }
        }

        public override sbyte[] getRow(int y, sbyte[] row)
        {
            if (row == null || row.Length < Width)
            {
                row = new sbyte[Width];
            }

            for (int i = 0; i < Height; i++)
                row[i] = (sbyte)Buffer[i * Width + y];

            return row;
        }
    }
}
