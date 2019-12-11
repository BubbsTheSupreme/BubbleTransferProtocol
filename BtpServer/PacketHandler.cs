using System;
using System.IO;

namespace FtpProjectServer {
    public static class PacketHandler{
        public static void Process(Server server, byte[] packet){
            Config conf = new Config();
            var packetId = packet[0];
            switch(packetId){
                case 0: { //id 0 handles the file name packet
                    string fileName = "";
                    for(int i = 1; i < packet.Length; i++){ //loop starts at 1 because thats where the data that was transferred is located packet[0] is the id 
                        fileName += (char)packet[i];//converts the bytes in the packet to a character for the string
                    }
                    server.fileName = conf.GetPath() + fileName; //uses the Client.fileName variable to store the filename for use in other sections 
                    Console.WriteLine(fileName);
                    break;
                } 
                case 1: { //id 1 handles the file size packet
                    string fileSizeStr = "";
                    for(int i = 1; i < packet.Length; i++){
                        fileSizeStr += (char)packet[i];
                    }
                    server.fileSize = long.Parse(fileSizeStr);
                    break;
                } //we first convert the long to a string then we send it like we would the file name but then we reconstruct the string and parse the long from it
                case 2: { //id 2 handles the file contents
                    byte[] fileData = new byte[packet.Length - 1];
                    Array.Copy(packet, 1, fileData, 0, fileData.Length); //used array.copy() to create a new array without the id and only the payload
                    try{
                        using(FileStream newFile = new FileStream(server.fileName, FileMode.Append)) //Creates and uses a filestream that takes the buffer of bytes received and writes them to a file
                            newFile.Write(fileData, 0, fileData.Length);
                    }
                    catch(Exception e){
                        Console.WriteLine($"Error occurred while writing file, {e}");
                    }
                    break;
                }
                case 3: { //id 3 handles the packet that lets the user know the transfer was successful
                    string message = "";
                    for(int i = 1; i < packet.Length; i++){
                        message += (char)packet[i];
                    }
                    Console.WriteLine(message);
                    server.Disconnect();
                    break;
                }
            }
        }
    }
}
                    // server.bytesWritten += fileData.Length;
                    // using(ProgressBar progress = new ProgressBar())
                    //     progress.Report((double) server.bytesWritten / server.fileSize);