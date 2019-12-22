using System;

namespace FtpProjectServer {
    class Program {
        static void Help(){
            Console.WriteLine("Welcome to BTP!");
            Console.WriteLine("No arguments starts the server application.");
            Console.WriteLine("-c \"download path\" will set the default download location");
            Console.WriteLine("-h prompts the help menu.");
        }
        static void UpdatePath(string path){
            Config conf = new Config();
            conf.ConfigWriter(path);
        }
        static void Main(string[] args){
            if(args.Length > 0){
                if(args[0] == "-c"){
                    string path = args[1];
                    UpdatePath(path);
                }
                else if(args[0] == "-h"){
                    Help();
                }
            }
            else{
                FtpServer server = new FtpServer();
                server.StartListening(12345);
            }
        }
    }
}
            // if(args.Length < 2){
            //     if(args.Length < 1){
            //     }
            //     if(args[0] == "-c"){
            //         string path = args[1];
            //         UpdatePath(path); 
            //     }
            //     else if(args[0] == "-h"){
            //         Help();
            //     }
            //     else{
            //         Console.WriteLine("Incorrect arguments");
            //     }
            // }
            // else{
            //     Console.WriteLine("Too many arguments, try again, or -h for help");
            // }