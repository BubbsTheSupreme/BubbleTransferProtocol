using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace FtpProjectServer{

    public class Server { 
        public Socket Socket;
        public string fileName;
        public long fileSize; 

        public Server(Socket socket) {
            Socket = socket; 
        }

        public void Send(byte[] packet) {
            try {
                Socket.Send(packet);
            }
            catch(SocketException) {
                Console.WriteLine("Communication error has occurred. Disconnecting now.");
                Disconnect();
            }
        }

        public void Disconnect(){
            try {
                Socket.Shutdown(SocketShutdown.Both);
            }
            catch(Exception) {
                Console.WriteLine($"Error occurred while trying to disconnect");
            }
            finally {
                Socket.Close();
            }

        }

    }
    public class FtpServer {

        private Thread recvThread;
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
                recvThread.Start(server);
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }

        public void Receive(object args){
            var server = (Server)args;
            try{
                while(true){
                    byte[] buffer = new byte[2]; // 2 is to let the buffer only let the size in when receiving 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Starting to receive packets..");
                    Console.ResetColor();
                    int receivePacket = server.Socket.Receive(buffer);
                    ushort requiredBytes = (ushort)(BitConverter.ToUInt16(buffer, 0) - 2); //casting because 2 is an int  
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