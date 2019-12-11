using System;
using System.IO;
using System.Text;

namespace FtpProjectServer {
    class Config {
        public void ConfigWriter(string path){
            byte[] fileContents = Encoding.ASCII.GetBytes(path); //converts string to byte array
            using(FileStream file = File.OpenWrite("config.txt")) //opens file
                file.Write(fileContents); //writes byte array to file
            Console.ForegroundColor = ConsoleColor.Green; //changes word color to green
            Console.WriteLine("Download location updated!");
            Console.ResetColor(); //resets word color
        }
        public string GetPath(){
            byte[] fileData = File.ReadAllBytes("config.txt"); //reads all bytes and stores it into a byte array
            string path = Encoding.ASCII.GetString(fileData); // convert the byte array to a string
            return path; //return string
        }
    }
}