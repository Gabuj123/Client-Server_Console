using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
public class SynchronousSocketListener
{
    // Incoming data from the client.
    public static string data = null;
    // memorizza ultimo messaggio
    public static string messaggio = "<EOF>";
    // prepara testo per la risposta
    public static string risposta = "<EOF>";

    public static void StartListening()
    {
        // Data buffer for incoming data.
        byte[] bytes = new Byte[2048];
        // Establish the local endpoint for the socket.
        // Dns.GetHostName returns the name of the 
        // host running the application.
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress[] addr = ipHostInfo.AddressList;
        for (int i = 0; i < addr.Length; i++)
        {
            Console.WriteLine("IP Address {0}: {1} ", i, addr[i].ToString());
        }
        IPAddress ipAddress = ipAddress = addr[addr.Length - 2];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Console.WriteLine("Server IP {0} ", ipAddress.ToString());
        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        listener.ReceiveTimeout = 500;
        // Bind the socket to the local endpoint and 
        // listen for incoming connections.
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            // Start listening for connections.
            while (true)
            {
                Console.WriteLine("In ascolto su {0} ...", localEndPoint.ToString());
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();
                data = null;
                // An incoming connection needs to be processed.
                while (true)
                {
                    bytes = new byte[2048];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                // interpreto il comando
                if (data == "<RDMSG><EOF>")
                {
                    risposta = messaggio;
                }
                else
                {
                    messaggio = data;
                    risposta = "<ACK><EOF>";
                    // Show the data on the console.
                    Console.WriteLine("{0} - {1}", handler.RemoteEndPoint.ToString(), messaggio);
                    //Console.WriteLine(messaggio);
                }


                // Echo the data back to the client.
                byte[] msg = Encoding.UTF8.GetBytes(risposta);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        //Console.WriteLine("\nPress ENTER to continue...");
        //Console.Read();


        //var fileName = GetExecutingAssembly().Location;
        //System.Diagnostics.Process.Start(fileName);
        // Closes the current process
        //Environment.Exit(0);
        string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        System.Diagnostics.Process.Start(strExeFilePath);
    }
    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}
