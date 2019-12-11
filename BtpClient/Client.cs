using System;
using System.Net;
using System.Net.Sockets;

namespace FtpProjectClient {
    public class FtpClient {

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Connect(string ip, ushort port) { // a function for Socket.Connect() and error handling
            try {
                IPAddress ipAddress = IPAddress.Parse(ip);
                socket.Connect(ipAddress, port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Connected to: {ipAddress}");
                Console.ResetColor();
            }
            catch(SocketException se) {
                Console.WriteLine($"An error has occurred while trying to connect to {ip}, {se}");
            }
        }
        
        public void Send(byte[] packet) { // a function for Socket.Send() and error handling
            try {
                socket.Send(packet);
            }
            catch(SocketException se) {
                Console.WriteLine($"Communication error has occurred. Disconnecting now. {se}");
                Disconnect();
            }
        }
        
        public void Disconnect() {
             // a function for disconnecting and error handling
            try {
                socket.Shutdown(SocketShutdown.Both); //shutsdown the sending and receiving functions for a safe disconnect
            }
            catch(SocketException se) {
                Console.WriteLine($"Error occurred while trying to disconnect. {se}");
            }
            finally {
               socket.Close(); //officially closes the connection
            }
        }

    }
}

