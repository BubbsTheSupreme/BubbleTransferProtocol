using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace FtpProjectServer{

    public class Server { 
        public Socket Socket;
        public string fileName;
        public long fileSize;
        public long bytesWritten;

        public Server(Socket socket) {
            Socket = socket; 
        }

        public void Send(byte[] packet) {
            try {
                Socket.Send(packet);
            }
            catch(SocketException se) {
                Console.WriteLine($"Error occurred: {se}");
            }
        }
        public void Disconnect() {
             // a function for disconnecting and error handling
            try {
                Socket.Shutdown(SocketShutdown.Both); //shutsdown the sending and receiving functions for a safe disconnect
            }
            catch(SocketException se) {
                Console.WriteLine($"Error occurred while trying to disconnect. {se}");
            }
            finally {
               Socket.Close(); //officially closes the connection
            }
        }
    }
    public class FtpServer {

        public Thread recvThread;
        private IPEndPoint clientEndPoint;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        public void StartListening(ushort port){
            recvThread = new Thread(Receive);
            try{
                clientEndPoint = new IPEndPoint(IPAddress.Any, port);
                socket.Bind(clientEndPoint);
                Console.WriteLine("Listening for connections..");
                socket.Listen(15);
                Socket clientSocket = socket.Accept();
                Server server = new Server(clientSocket);
                Console.WriteLine("Connected!");
                recvThread.Start(server); // acts as the parameter for Receive()
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }

        public void Receive(object args){
            var server = (Server)args;
            try{
                while(true){
                    byte[] buffer = new byte[4]; // 4 is to let the buffer only let the size in when receiving 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Starting to receive packets..");
                    Console.ResetColor();
                    int receivePacket = server.Socket.Receive(buffer);
                    uint requiredBytes = (uint)(BitConverter.ToUInt32(buffer, 0) - 2); //casting because 2 is an int  
                    buffer = new byte[requiredBytes]; //uses 16bit size variable to create a new buffer with the size of the packet to receive the rest of the packet with
                    int received = 0;
                    int size;
                    while(received < requiredBytes){
                        size = buffer.Length - received; //subtracts how much was received from the total so it knows how much to receive in the current loop
                        received += server.Socket.Receive(buffer, received, size, SocketFlags.None);
                    }
                    PacketHandler.Process(server, buffer);
                }
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }
    }
}