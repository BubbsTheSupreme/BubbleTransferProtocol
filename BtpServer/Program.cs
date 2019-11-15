using System;

namespace FtpProjectServer {
    class Program {
        static void Main(string[] args){
            FtpServer server = new FtpServer();
            Console.WriteLine("Welcome to the Server");
            server.StartListening(ushort.Parse("12345"));
            //add command line argument to decide where the file downloads to
            //add some kind of progress bar that tells you how much youve recieved out of the total.
        }
    }
}