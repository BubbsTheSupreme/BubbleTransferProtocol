using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;


namespace FtpProjectServer{

    public class Client{
        public Socket Socket;
        public string fileName;
        public long fileSize; 

        public Client(Socket socket){
            Socket = socket;
        }

        public void Send(byte[] packet){
            try{
                Socket.Send(packet);
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }

        public void Disconnect(){
            try {
                Socket.Shutdown(SocketShutdown.Both);
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
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
                var clientSocket = socket.Accept();
                var client = new Client(clientSocket);
                Console.WriteLine("Connected!");
                recvThread.Start(client);
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }

        public void Receive(object args){
            var client = (Client)args;
            try{
                while(true){
                    byte[] buffer = new byte[2];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Starting to receive packets..");
                    Console.ResetColor();
                    int receivePacket = client.Socket.Receive(buffer);
                    ushort requiredBytes = (ushort)(BitConverter.ToUInt16(buffer, 0) - 2);
                    buffer = new byte[requiredBytes];
                    int received = 0;
                    int size;
                    while(received < requiredBytes){
                        size = buffer.Length - received;
                        received += client.Socket.Receive(buffer, received, size, SocketFlags.None);
                    }
                    PacketHandler.Process(client, buffer);
                }
            }
            catch(Exception e){
                Console.WriteLine($"Error occurred: {e}");
            }
        }

        
    }
}