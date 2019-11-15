using System;
using System.IO;

namespace FtpProjectServer {
    public static class PacketHandler{
        public static void Process(Client client, byte[] packet){
            var packetId = packet[0];
            switch(packetId){
                case 0:{
                    string fileName = "";
                    for(int i = 1; i < packet.Length; i++){
                        fileName += (char)packet[i];
                    }
                    client.fileName = fileName;
                    Console.WriteLine(fileName);
                    break;
                }
                case 1:{
                    //receives the file size so the server can keep track of transfer progress
                    string fileSizeStr = "";
                    for(int i = 0; i < packet.Length; i++){
                        fileSizeStr += (char)packet[i];
                    }
                    client.fileSize = long.Parse(fileSizeStr);
                    break;
                }
                case 2:{
                    byte[] fileData = new byte[packet.Length - 1];
                    Array.Copy(packet, 1, fileData, 0, fileData.Length); //used array.copy() to create a new array without the id and only the payload
                    using(FileStream newFile = new FileStream(client.fileName, FileMode.Append))
                        newFile.Write(fileData, 0, fileData.Length);
                    break;
                }
                case 3:{
                    string message = "";
                    for(int i = 1; i < packet.Length; i++){
                        message += (char)packet[i];
                    }
                    Console.WriteLine(message);
                    client.Disconnect();
                    break;
                }
            }
        }
    }
}