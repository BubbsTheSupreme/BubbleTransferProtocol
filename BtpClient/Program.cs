using System;
using System.IO;
using System.Text;


namespace FtpProjectClient {

    class Program {
        static void Main(string[] args) {
            FtpClient client = new FtpClient();
            Console.WriteLine("Welcome to Client.");
            Console.WriteLine("To start sending files type 's', to quit type 'q'.");
            Console.Write("> ");
            string input = Console.ReadLine();
            if(input == "s" || input == "S"){
                Console.WriteLine("What is the server ip address");
                Console.Write("> ");
                string serverIP = Console.ReadLine();
                client.Connect(serverIP, 12345);
                Console.WriteLine("Whats the name of the file?");
                Console.Write("> ");
                string fileName = Console.ReadLine();
                byte[] data = Encoding.ASCII.GetBytes(fileName);
                Packet packet = new Packet((ushort)data.Length).WritePacketId(0).WritePacketPayload(data);
                client.Send(packet.Finalize());
                FileInfo f = new FileInfo(fileName); 
                string fileSize = f.Length.ToString(); //gets the file size and sets it as a string
                data = Encoding.ASCII.GetBytes(fileSize);
                packet = new Packet((ushort)data.Length).WritePacketId(1).WritePacketPayload(data);
                client.Send(packet.Finalize());
                int bytesRead;
                byte[] fileContents;
                byte[] fileData = new byte[32768]; //creates a 32kb array for file contents
                try{
                    using(FileStream file = File.OpenRead(fileName)){ //add a progress bar for sending progress too
                        while((bytesRead = file.Read(fileData, 0, fileData.Length)) > 0){
                            fileContents = new byte[bytesRead];
                            Array.Copy(fileData, 0, fileContents, 0, fileContents.Length);
                            Packet filePacket = new Packet((ushort)fileContents.Length).WritePacketId(2).WritePacketPayload(fileContents);
                            client.Send(filePacket.Finalize());
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
                packet = new Packet((ushort)data.Length).WritePacketId(3).WritePacketPayload(data);
                client.Send(packet.Finalize());
                client.Disconnect();
            }
            else if(input == "q" || input == "Q"){
                Console.WriteLine("Are you sure? type y/n");
                input = Console.ReadLine();
                if(input == "y" || input == "Y"){
                    Console.WriteLine("Needs updated");
                }
            }
            else {
                Console.WriteLine("Input invalid.. Please try again.");
            } 
        }
    }
}

        // static void SendFile(FtpClient client, string fileName){
        //     byte[] data = Encoding.ASCII.GetBytes(fileName);
        //     Packet packet = new Packet((ushort)data.Length).WritePacketId(0).WritePacketPayload(data);
        //     client.Send(packet.Finalize());
        //     int bytesRead;
        //     byte[] fileContents;
        //     byte[] fileData = new byte[1024];
        //     using(FileStream file = File.OpenRead(fileName)){
        //         while((bytesRead = file.Read(fileData, 0, fileData.Length)) > 0){
        //             fileContents = new byte[bytesRead];
        //             Array.Copy(fileData, 0, fileContents, 0, fileContents.Length);
        //             Packet filePacket = new Packet((ushort)fileContents.Length).WritePacketId(1).WritePacketPayload(fileContents);
        //             client.Send(filePacket.Finalize());
        //         }
        //     }
        //     data = Encoding.ASCII.GetBytes("File Transfer Complete.");
        //     packet = new Packet((ushort)data.Length).WritePacketId(0).WritePacketPayload(data);
        //     client.Send(packet.Finalize());
        // }
        // static void Main(string[] args) {
        //     FtpClient client = new FtpClient();
        //     ushort defaultPort = 12345;
        //     if(args.Length > 0){
        //         if(args[0] == "-ip" && args[2] == "-f"){
        //             string ip = args[1];
        //             string fileName = args[3];
        //             client.Connect(ip, defaultPort);
        //             SendFile(client, fileName);
        //         }
        //     }
        //     else{
        //         Console.WriteLine("No command line arguments provided..");
        //         Console.WriteLine("use flag -h for help");
        //     }
        // }
