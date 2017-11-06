using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace CP_WSMonitor
{
    public class TCP
    {

        /// <summary>
        /// IP
        /// </summary>
        public static string IP = ConfigurationManager.AppSettings.GetValues(3)[0];
       // public delegate void UPDate_TextBox(object sender, SerialDataReceivedEventArgs e);
        /// <summary>
        /// Port
        /// </summary>
        public static int Port=Convert.ToInt32( ConfigurationManager.AppSettings.GetValues(4)[0]);
        //public CP_WeatherStation cp_WeatherStation = new CP_WeatherStation();

        public  TcpClient  Tcp_Connect;
        public NetworkStream NClient_Stream;
        //public TCP(string ipAddress, int iPort)
        //{
        //    Tcp_Connect = new TcpClient(ipAddress, iPort);
        //}

        //public TcpClient Tcp_Connect;
        
        public TCP ()
        {
            Tcp_Connect = new TcpClient(IP, Port);
           NClient_Stream = Tcp_Connect.GetStream();
            ThreadStart ts = new ThreadStart(Thread_Start);
            Thread thread = new Thread(ts);
            thread.IsBackground = true;
            thread.Start();
        }
        //TcpClient tcpclient = new TcpClient(this.IP, this.Port);

        private System.Threading.Timer timerWait;

        //public void Tcp_Connect()
        //{
        //    ThreadStart ts = new ThreadStart(Thread_Start);
        //    Thread thread = new Thread(ts);
        //    thread.IsBackground = true;
        //    thread.Start();

        //}
        bool b = false;
        public bool IsStarted()
        {
           // Tcp_Connect = new TcpClient(this.IP, this.Port);

            return true;
        }

        public bool Connect()
        {
            if(IsStarted())
            {
                b = true;
            }
            return b;
        }
        

        
        public void Thread_Start()
        {
            //Tcp_Connect = new TcpClient(IP, Port);
            // NetworkStream NClient_Stream = Tcp_Connect.GetStream();
           // CP_WeatherStation cp_WeatherStation = new CP_WeatherStation();
            while (true)
            {
                try
                {
                   // if (b)
                   // {
                    Thread.Sleep(60000);
                        //timerWait = new System.Threading.Timer(new TimerCallback(timerCall), this, 5000, 0);
                        //int bytes = tcpclient.ReceiveBufferSize;
                        byte[] buffer = new byte[1024];
                        //if (bytes == 0)
                        //{ continue; }
                       
                        //this..Thread.Sleep(10000);

                        int iCount = NClient_Stream.Read(buffer, 0, buffer.Length);
                       // CP_WeatherStation cp_WeatherStation = new CP_WeatherStation();
                        Data_Process_Socket(buffer, 96, "CP_Sensor_Data_Hour");
                        //cp_WeatherStation.textBox1.Text = "111111111111111111111111111";
                        NClient_Stream.Flush();
                       
                        //  Form frm = new Form1();
                        //Data_Process(buffer, 96, "CP_Sensor_Data_Hour");
                  //  }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : " + ex.Message);
                }
            }
        }

        public void Data_Process_Socket(byte[] bReceive, int iLengthDataOne, string sTable_Name)
        {
            order order1 = new order();
            string sReceiveData = order1.JishiData_Return_Pro(bReceive);
            sReceiveData = sReceiveData.Trim(new char[] { '\0', ' ' }).ToUpper();
            //string sReceiveData = "HDATA,110101003,09/05/18,15:46:15,+22.7,30,00,00.0,018,00.0,009,00.0,002,000.0,0000.0,000,008,\r\n";
            if (!string.IsNullOrEmpty(sReceiveData))
            {
                if (sReceiveData.Contains("HEART"))
                { MsgRecord("收到心跳信息:" + sReceiveData); }
                if (sReceiveData.Contains("GPSDATA"))
                { MsgRecord("收到注册信息:" + sReceiveData); }
                for (int i = 0; i < sReceiveData.Length / iLengthDataOne; i++)
                {
                    string[] sReceiveDataArray = sReceiveData.Substring((0 + iLengthDataOne * i), iLengthDataOne).Split(',');
                    if ((sReceiveDataArray[0] == "HDATA" || sReceiveDataArray[0] == "PDATA") && sReceiveDataArray[sReceiveDataArray.Length - 1] == "\r\n")
                    {

                        // MsgRecord("整点数据信息未解析:" + sReceiveData);//正确回复应该为94长度，带回车换行96
                        MsgRecord("收到数据:" + sReceiveData);//正确回复应该为94长度，带回车换行96
                        // MsgRecord("收到数据");
                        string sDataFormat = "站点ID:" + sReceiveDataArray[1] + ",";
                        sDataFormat += "数据时间:" + sReceiveDataArray[2] + sReceiveDataArray[3] + ",";
                        string sDT = sReceiveDataArray[2] + " " + sReceiveDataArray[3];
                        sDataFormat += "温度:" + sReceiveDataArray[4] + "℃,";
                        sDataFormat += "湿度:" + sReceiveDataArray[5] + "%\n\r";
                        //sDataFormat += "3秒平均风速:" + sReceiveDataArray[7] + ",";
                        //sDataFormat += "3秒平均风向:" + sReceiveDataArray[8] + ",";
                        //sDataFormat += "2分平均风速:" + sReceiveDataArray[9] + ",";
                        //sDataFormat += "2分平均风向:" + sReceiveDataArray[10] + ",";
                        sDataFormat += "10分平均风速:" + sReceiveDataArray[11] + "m/s,";
                        sDataFormat += "10分平均风向:" + sReceiveDataArray[12] + "度\n\r";
                        sDataFormat += "日雨量:" + Convert.ToDouble(sReceiveDataArray[13]) * 2 + "mm,";
                        sDataFormat += "气压:" + sReceiveDataArray[14] + "hPa\n\r";
                        //sDataFormat += "连续无雨日:" + sReceiveDataArray[15] + ",";
                        sDataFormat += "状态码:" + sReceiveDataArray[16];
                        // MsgRecord("整点数据解析后:" + sDataFormat);

                        /*接收数据成功后，发送数据接收成功指令，判断是否整点，是-存入整点数据表，并校时;否-存入即时数据表*/
                        //serialPort1.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                        NClient_Stream.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                        MsgRecord("发送成功解析数据确认！");

                        DateTime dt_Date = Convert.ToDateTime(sDT);
                        if (dt_Date.Minute == 0 && dt_Date.Second == 0)
                        {
                           // Command_Set_Send_Receive(0x1E);
                            RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[11]), Convert.ToInt32(sReceiveDataArray[12]), Convert.ToDouble(sReceiveDataArray[14]), 0, Convert.ToDouble(sReceiveDataArray[13]) * 2, 0, "", "", "CP_Sensor_Data_Hour");
                        }
                        else
                        {
                            RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[11]), Convert.ToInt32(sReceiveDataArray[12]), Convert.ToDouble(sReceiveDataArray[14]), 0, Convert.ToDouble(sReceiveDataArray[13]) * 2, 0, "", "", "CP_Sensor_Data");
                        }


                        //RecordDataInDB()
                    }
                    else
                    { MsgRecord("错误提示:数据有误" + "详细:" + sReceiveData); }
                }
            }
        }
        public void RecordDataInDB(DateTime dateTime, string stationID, double temp, int hum, double windSpeed, int windDirection, double bar, double solar, double rain, double c02, string more1 = "", string more2 = "", string sTable_Name = "")
        {
            string cnString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
            string sql = string.Format(" if not  exists (select 1 from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + dateTime + "')  Insert into " + sTable_Name + " " + "(CP_Data_DateTime, CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2) Values" + " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')", dateTime, stationID, temp, hum, windSpeed, windDirection, bar, solar, rain, c02, more1, more2);

            SqlConnection conn = new SqlConnection(cnString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public void MsgRecord(string str)
        {
           // if (this.textBox1.Text.Length > 10000)
           //     this.textBox1.Text = "";
            string sData = DateTime.Now.ToString() + ":";
            string FILE_NAME = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";

          //  this.textBox1.AppendText(sData);
           // this.textBox1.AppendText(str + "\r\n");
           // this.textBox1.ScrollToCaret();
            WriteFile(sData + str + "\r\n", FILE_NAME);
        }
        public void WriteFile(string str, string FILE_NAME)
        {
            StreamWriter sr;
            if (File.Exists(FILE_NAME)) //如果文件存在,则创建File.AppendText对象 
            {
                sr = File.AppendText(FILE_NAME);
            }
            else    //如果文件不存在,则创建File.CreateText对象 
            {
                sr = File.CreateText(FILE_NAME);
            }
            sr.WriteLine(str);
            sr.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        //void timerCall(object state)
        //{
        //    if (!tcpclient.Connected)
        //    {
        //        tcpclient.Connect(this.IP);
        //    }
        //}
    }
}
