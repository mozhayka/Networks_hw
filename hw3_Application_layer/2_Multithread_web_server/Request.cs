using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Request
    {
        public void Answer(Socket socket)
        {
            try
            {
                TryAnswer(socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void TryAnswer(Socket socket)
        {
            NetworkStream stream = new(socket);
            StreamReader reader = new(stream);

            string request = reader.ReadLine();

            string answer = CreateAnswer(request);
            byte[] data = Encoding.ASCII.GetBytes(answer);
            socket.Send(data);

            stream.Close();
            reader.Close();
            socket.Close();
        }

        private string CreateAnswer(string request)
        {
            if (request == null)
                throw new Exception("Empty request");
            Console.WriteLine(request);

            string fileName = request.Split(' ')[1];
            if (!string.IsNullOrEmpty(fileName))
                fileName = $"../../..{fileName}";

            if (File.Exists(fileName))
                return AnswerOK(fileName);
            else
                return AnswerNotFound();
        }

        private string AnswerNotFound()
        {
            return "HTTP/1.0 404 Not Found\r\n" +
                "Content-Type: text/html\r\n\r\n" +
                "<HTML><HEAD><TITLE>Not Found</TITLE></HEAD>" +
                "<BODY>Not Found</BODY></HTML>";
        }

        private string AnswerOK(string fileName)
        {
            return "HTTP/1.0 200 OK\r\n" +
                ContentType(fileName) +
                FileBody(fileName);
        }

        private string ContentType(string fileName)
        {
            if (fileName.EndsWith(".txt") || fileName.EndsWith(".htm") || fileName.EndsWith(".html"))
                return "Content-Type: text/html\r\n\r\n";

            if (fileName.EndsWith(".ram") || fileName.EndsWith(".ra"))
                return "Content-Type: audio/x-pn-realaudio\r\n\r\n";

            return "Content-Type: application/octet-stream\r\n\r\n";
        }

        private string FileBody(string fileName)
        {
            return new StreamReader(fileName).ReadToEnd();
        }
    }
}