using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace FtpProjectClient {
    public class FtpClient {

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Thread recvThread;

        public void Connect(string ip, ushort port){
            var ipAddress = IPAddress.Parse(ip);
            try{
                socket.Connect(ipAddress, port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Connected to: {ipAddress}");
                Console.ResetColor();
            }
            catch(Exception e){
                Console.WriteLine($"{e}");
            }
        }
        
        public void Send(byte[] packet){
            try{
                socket.Send(packet);
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }
        
        public void Disconnect(){
            try{
                socket.Shutdown(SocketShutdown.Both);
            }
           catch(Exception e){
               Console.WriteLine(e);
           }
           finally{
               socket.Close();
           }
        }

    }
}

