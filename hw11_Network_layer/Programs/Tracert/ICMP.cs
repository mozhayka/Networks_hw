using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracert
{
    class ICMP
    {
        public byte Type;
        public byte Code;
        public ushort Checksum;
        public int MessageSize;
        public byte[] Message = new byte[1024];

        public ICMP()
        {
            Type = 0x08;
            Code = 0x00;

            Buffer.BlockCopy(BitConverter.GetBytes(1), 0, Message, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(1), 0, Message, 2, 2);

            byte[] data = Encoding.ASCII.GetBytes("init packet");
            Buffer.BlockCopy(data, 0, Message, 4, data.Length);
            MessageSize = data.Length + 4;

            Checksum = GetChecksum();
        }

        public ICMP(byte[] data, int size)
        {
            Type = data[20];
            Code = data[21];
            Checksum = BitConverter.ToUInt16(data, 22);
            MessageSize = size - 24;
            Buffer.BlockCopy(data, 24, Message, 0, MessageSize);
        }

        public byte[] GetBytes()
        {
            byte[] data = new byte[MessageSize + 9];

            Buffer.BlockCopy(BitConverter.GetBytes(Type), 0, data, 0, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(Code), 0, data, 1, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(Checksum), 0, data, 2, 2);
            Buffer.BlockCopy(Message, 0, data, 4, MessageSize);

            return data;
        }

        public ushort GetChecksum()
        {
            uint chcksm = 0;
            byte[] data = GetBytes();
            int packetsize = MessageSize + 8;
            int index = 0;

            while (index < packetsize)
            {
                chcksm += Convert.ToUInt32(BitConverter.ToUInt16(data, index));
                index += 2;
            }
            chcksm = (chcksm >> 16) + (chcksm & 0xffff);
            chcksm += (chcksm >> 16);
            return (ushort)(~chcksm);
        }
    }
}
