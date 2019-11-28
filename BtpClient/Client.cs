using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace FtpProjectClient {
    public class FtpClient {

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Thread recvThread;

        public void Connect(string ip, ushort port) { // a function for Socket.Connect() and error handling
            try {
                IPAddress ipAddress = IPAddress.Parse(ip);
                socket.Connect(ipAddress, port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Connected to: {ipAddress}");
                Console.ResetColor();
            }
            catch(SocketException) {
                Console.WriteLine($"An error has occurred while trying to connect to {ip}");
            }
        }
        
        public void Send(byte[] packet) { // a function for Socket.Send() and error handling
            try {
                socket.Send(packet);
            }
            catch(SocketException) {
                Console.WriteLine("Communication error has occurred. Disconnecting now.");
                Disconnect();
            }
        }
        
        public void Disconnect() { // a function for disconnecting and error handling
            try {
                socket.Shutdown(SocketShutdown.Both); //shutsdown the sending and receiving functions for a safe disconnect
            }
           catch(SocketException) {
               Console.WriteLine("Error occurred while trying to disconnect.");
           }
           finally {
               socket.Close(); //officially closes the connection
           }
        }

    }
}

