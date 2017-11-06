using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace CP_WSMonitor
{
  public  class Client
    {
      public string sStation_ID;
      public int sStation_Mode;
      public int sStation_Type;
      public string sStation_IP;
      public int sStation_Port;
      public NetworkStream ntStream;
      public TcpClient tcp_Connect;
      //20170824 新加 开始
      public terminalDATADTO tmdto;
      public terminalDATADTO[] tmdtos=new terminalDATADTO[2];
     
      public int terminalDATAgetflag=0;
      //20170824 新加 结束
     
      public void tcp_Connect_Config(string sIP, int iPort)
      {
          //IPAddress myIPAddress = (IPAddress)Dns.GetHostAddresses(Dns.GetHostName()).GetValue(0);
          //IPEndPoint iep = new IPEndPoint(IPAddress.Parse(Dns.GetHostName()), 8000);//指定客户端地址与端口
           tcp_Connect = new TcpClient(sIP, iPort);
           ntStream = tcp_Connect.GetStream();
      }
    
    }
}
