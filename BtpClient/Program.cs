using System;
using System.IO;
using System.Text;


namespace FtpProjectClient {

    class Program {

        static void Help(){
            Console.WriteLine("Welcome to the BTP Client!");
            Console.WriteLine("To start the client you need to use the command line arguments like this.");
            Console.WriteLine("btpclient \"127.0.0.1\" -f \"D:/test.txt\"");
        }

        static void SendFile(string fileName){
            byte[] data = Encoding.ASCII.GetBytes(fileName);
            Packet packet = new Packet((uint)data.Length).WritePacketId(0).WritePacketPayload(data);
            client.Send(packet.Finalize());
            FileInfo f = new FileInfo(fileName); 
            string fileSize = f.Length.ToString(); //gets the file size and sets it as a string
            data = Encoding.ASCII.GetBytes(fileSize);
            packet = new Packet((uint)data.Length).WritePacketId(1).WritePacketPayload(data);
            client.Send(packet.Finalize());
            long fileSizeNum = long.Parse(fileSize);
            int bytesRead;
            byte[] fileContents;
            long totalBytes = 0;
            byte[] fileData = new byte[8388608]; //creates a 8MB array for file contents
            try {
                using(FileStream file = File.OpenRead(fileName)){ //add a progress bar for sending progress too
                    while((bytesRead = file.Read(fileData, 0, fileData.Length)) > 0) {
                        fileContents = new byte[bytesRead];
                        Array.Copy(fileData, 0, fileContents, 0, fileContents.Length);
                        Packet filePacket = new Packet((uint)fileContents.Length).WritePacketId(2).WritePacketPayload(fileContents);
                        client.Send(filePacket.Finalize());
                        totalBytes += bytesRead; //keeps track of bytes read for the progress bar
                        progress.Report((double)totalBytes / fileSizeNum);
                    }
                }
            }
            catch(FileNotFoundException){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The file {fileName} not found");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("File Transfer Complete.");
            Console.ResetColor();
            data = Encoding.ASCII.GetBytes("File Transfer Complete.");
            packet = new Packet((uint)data.Length).WritePacketId(3).WritePacketPayload(data);
            client.Send(packet.Finalize());
            client.Disconnect();
        }

        static ProgressBar progress = new ProgressBar();
        static FtpClient client = new FtpClient();

        static void Main(string[] args) {
            if(args.Length > 0){
                if(args[0] == "-h" || args[0] == "-H"){
                    Help();
                }
                else{
                    string ip = args[0];
                    if(args[1] == "-f" || args[1] == "-F"){
                        string fileName = args[2];
                        client.Connect(ip, 12345);
                        SendFile(fileName);
                    }
                }
                
            }
            else{
                Console.WriteLine("No command line arguments provided..");
                Console.WriteLine("use flag -h for help");
            }
        }
    }
}