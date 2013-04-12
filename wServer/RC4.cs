using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace wServer
{
    public class RC4
    {
        private byte[] m_State = new byte[256];

        public int X { get; set; }
        public int Y { get; set; }

        public byte[] State
        {
            get
            {
                byte[] buf = new byte[256];
                Array.Copy(this.m_State, buf, 256);
                return buf;
            }
            set
            {
                Array.Copy(value, this.m_State, 256);
            }
        }

        public RC4(byte[] key)
        {
            for (int i = 0; i < 256; i++)
            {
                this.m_State[i] = (byte)i;
            }

            this.X = 0;
            this.Y = 0;

            int index1 = 0;
            int index2 = 0;

            byte tmp;

            if (key == null || key.Length == 0)
            {
                throw new Exception();
            }

            for (int i = 0; i < 256; i++)
            {
                index2 = ((key[index1] & 0xff) + (this.m_State[i] & 0xff) + index2) & 0xff;

                tmp = this.m_State[i];
                this.m_State[i] = this.m_State[index2];
                this.m_State[index2] = tmp;

                index1 = (index1 + 1) % key.Length;
            }
        }

        public byte[] Crypt(byte[] buf)
        {
            int xorIndex;
            byte tmp;

            if (buf == null)
            {
                return null;
            }

            byte[] result = new byte[buf.Length];

            for (int i = 0; i < buf.Length; i++)
            {

                this.X = (this.X + 1) & 0xff;
                this.Y = ((this.m_State[this.X] & 0xff) + this.Y) & 0xff;

                tmp = this.m_State[this.X];
                this.m_State[this.X] = this.m_State[this.Y];
                this.m_State[this.Y] = tmp;

                xorIndex = ((this.m_State[this.X] & 0xff) + (this.m_State[this.Y] & 0xff)) & 0xff;
                result[i] = (byte)(buf[i] ^ this.m_State[xorIndex]);
            }

            return result;
        }
    }
}
