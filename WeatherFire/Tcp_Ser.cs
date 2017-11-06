using System;  
using System.Collections.Generic;  
using System.Text;  
using System.Net.Sockets;  
using System.IO;  
using System.Threading; 


namespace CP_WSMonitor
{
    class Tcp_Ser
    {
      
        TcpClient m_tcpClient;

        public Tcp_Ser(TcpClient tc)
        {
            m_tcpClient = tc;
        }

        public void communicate()
        {
            NetworkStream ns = m_tcpClient.GetStream();
            int recv;
            string mess;
            while (true)
            {
                byte[] bytes = new byte[1024];
                try
                {
                    if ((recv = ns.Read(bytes, 0, 1024)) == 0)
                    {
                        Console.WriteLine("disconnected");
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("missing");
                    break;
                }
                mess = System.Text.Encoding.ASCII.GetString(bytes, 0, recv);
                Console.WriteLine(m_tcpClient.Client.AddressFamily.ToString() + "::" + mess);
                bytes = System.Text.Encoding.ASCII.GetBytes(mess.ToUpper());
                try
                {
                    ns.Write(bytes, 0, bytes.Length);
                }
                catch
                {
                    Console.WriteLine("missing");
                    break;
                }
            }
            ns.Close();
            m_tcpClient.Close();
        }
    }
}

    

