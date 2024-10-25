using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class SynchronousSocketClient
{
    public static void StartClient()
    {
        // Data buffer for incoming data.
        byte[] bytes = new byte[1024];
        int scelta;
        string messaggio;
        string nick;
        string ip;
        //legge nickname
        Console.WriteLine();
        Console.WriteLine("Mini Chat");
        Console.Write("Nickname:");
        nick = Convert.ToString(Console.ReadLine());
        Console.WriteLine();
        Console.Write("IP del server:");
        ip = Convert.ToString(Console.ReadLine());
        scelta = 0;
        // Connect to a remote device.
        try
        {
            while (scelta != 3)
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);



                //legge scelta
                Console.WriteLine();
                Console.WriteLine("1. Leggi messaggio");
                Console.WriteLine("2. Scrivi messaggio");
                Console.WriteLine("3. Esci");
                scelta = Convert.ToInt32(Console.ReadLine());
                messaggio = "<EOF>";
                if (scelta == 1)
                {
                    messaggio = "<RDMSG><EOF>";
                }
                if (scelta == 2)
                {
                    Console.WriteLine("Scrivi messaggio:");
                    messaggio = nick + ":" + Convert.ToString(Console.ReadLine()) + "<EOF>";
                }
                if (scelta == 3)
                {
                    Console.WriteLine("Fine");
                    Environment.Exit(0);
                }
                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());
                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.UTF8.GetBytes(messaggio);
                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);
                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.UTF8.GetString(bytes, 0, bytesRec));
                    // Release the socket.
                    // sender.Shutdown(SocketShutdown.Both);
                    // sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    public static int Main(String[] args)
    {
        StartClient();
        Console.ReadLine();
        return 0;
    }
}