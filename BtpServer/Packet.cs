using System;
using System.IO;

namespace FtpProjectServer {
    class Packet{ 

        private MemoryStream memoryStream;
        private byte[] buffer;
        
        public Packet(ushort packetSize){ //constructor
            packetSize = (ushort)(packetSize + 3); 
            buffer = new byte[packetSize];
            memoryStream = new MemoryStream(buffer);
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Write(BitConverter.GetBytes(packetSize));
        }

        public Packet WritePacketId(byte id){
            memoryStream.Seek(2, SeekOrigin.Begin);
            memoryStream.WriteByte(id);
            return this;
        }

        public Packet WritePacketPayload(byte[] data){
            memoryStream.Write(data);
            return this;
        }

        public byte[] Finalize(){
            memoryStream.Flush();
            memoryStream.Dispose();
            return buffer;
        }

    }
}