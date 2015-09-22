using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace AppService
{
    public class SocketListener
    {
        public void OpenServer()
        {
            TcpListener server = null;
            // Set the TcpListener on port 13000.
            int port = 13000;

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(GetLocalIPAddress(), port);

            // Start listening for client requests.
            server.Start();
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while(true)
            {
                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    /*stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);*/
                }
                //string id = "87d6609e-9165-4ba4-add2-eac3e9e3ee4a";
                string[] resuilt = data.Split(':'); 
                string url = "http://192.168.120.18:8083/s/hoivien/detailshoivien/?APK=" + resuilt[0];
                Process.Start(resuilt[1], url);

                // Shutdown and end connection
                client.Close();
            }
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }


    }
}
