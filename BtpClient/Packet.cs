using System;
using System.IO;

namespace FtpProjectClient {
    class Packet {

        private MemoryStream memoryStream; // writes/reads buffered data to/from memory
        private byte[] buffer;

        
        public Packet(ushort packetSize) { //constructor
            packetSize = (ushort)(packetSize + 3); //the extra 3 places to make room for the size which takes up 2 and the id which takes up 1
            //we need to cast (ushort)(packetSize + 3) because 3 is an int and turns the number into an int
            buffer = new byte[packetSize]; // uses the packetSize to create a byte array the size of the packet
            memoryStream = new MemoryStream(buffer);
            memoryStream.Seek(0, SeekOrigin.Begin); // sets the current position for write()
            memoryStream.Write(BitConverter.GetBytes(packetSize));
        }

        public Packet WritePacketId(byte id) {
            memoryStream.Seek(2, SeekOrigin.Begin); //2 is the location of the id after
            memoryStream.WriteByte(id);
            return this;
        }

        public Packet WritePacketPayload(byte[] data) {
            memoryStream.Write(data);
            return this;
        }

        public byte[] Finalize() { //safely shuts down the MemoryStream
            memoryStream.Flush();
            memoryStream.Dispose();
            return buffer;
        }

    }
}