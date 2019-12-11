using System;
using System.IO;

namespace FtpProjectClient {
    class Packet {

        private MemoryStream memoryStream; // writes/reads buffered data to/from memory
        private byte[] buffer;

        
        public Packet(uint packetSize) { //constructor
            packetSize = (uint)(packetSize + 5); //the extra 5 places to make room for the size which takes up 4 and the id which takes up 1
            //we need to cast (uint)(packetSize + 5) because 5 is an int and turns the number into an int
            buffer = new byte[packetSize]; // uses the packetSize to create a byte array the size of the packet
            memoryStream = new MemoryStream(buffer);
            memoryStream.Seek(0, SeekOrigin.Begin); // sets the current position for write()
            memoryStream.Write(BitConverter.GetBytes(packetSize));
        }

        public Packet WritePacketId(byte id) {
            memoryStream.Seek(4, SeekOrigin.Begin); //4 is the location of the id after size
            memoryStream.WriteByte(id);
            return this;
        }

        public Packet WritePacketPayload(byte[] data) {
            memoryStream.Write(data);
            return this;
        }

        public byte[] Finalize() { //safely shuts down the MemoryStream and disposes it
            memoryStream.Flush();
            memoryStream.Dispose();
            return buffer;
        }

    }
}