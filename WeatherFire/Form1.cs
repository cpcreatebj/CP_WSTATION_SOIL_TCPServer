using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using WeatherFire;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace CP_WSMonitor
{
    public partial class CP_WeatherStation : Form
    {
        /**********test start 20170823**************/
        public string sssss = "";
        public byte[] bttttt;
        /**********test end 20170823**************/
        int iFlag = 0;
        private TCP tcp;
        /**************服务器端口start**********************/
        public int iServer_Com =Convert.ToInt32( ConfigurationManager.AppSettings.GetValues(10)[0]);
        /**************服务器端口end**********************/
       /********************************************/
        List<string> sStation_ID_GPRS = new List<string>();
        List<string> sStation_ID = new List<string>();
        List<string> sStation_IP = new List<string>();
        List<string> sStation_StationType = new List<string>();
        DataSet[] DataSet = new DataSet[2];
        public Thread[] threads = null;
        public Dictionary<string, Thread> dic_thread = new Dictionary<string, Thread>();
        public Dictionary<string, Thread> dic_thread_check = new Dictionary<string, Thread>();
        public Dictionary<string, int> dic_Net_Status = new Dictionary<string, int>();
        public Dictionary<string, int> dic_Station_flag = new Dictionary<string, int>();
        ThreadStart ts = null;
        /********************************************/
        /*******新加2014年6月27日***********/
        public Dictionary<string, Client> dic_Client = new Dictionary<string, Client>();
        public string sHeaderText = "状态,站点ID,站点名称,站点类型,通讯方式,最新数据时间,经度,纬度";
        public Dictionary<string, string> Dic_Station_Type = new Dictionary<string, string> { { "0", "气象站" }, { "1", "气象土壤站" }, { "2", "土壤站" } };
        public Dictionary<string, string> Dic_Station_Mode = new Dictionary<string, string> {  { "0", "GPRS" }, { "1", "TCP/IP" }, { "2", "串口" } };
        int iCount_Online = 0;//在线站点数
        // public List<Client> list_Client = new List<Client>();
        /*******************/
        private System.Threading.Timer timerWait;
        public int iTimeOut_Receive_Wait = 1000;
        public int iData_Hour_Download = 0;
        public byte bCommand_Download = 0x25;
        public int Download_Fail_Try_Count = 2;
        public Thread thread_Download;
        public int iThread_Flag = 0;
        public bool bThread_Status = false;
        public byte bCommand_Flag = 0;
        public TcpClient[] Tcp_Connect=new TcpClient[2];
        public int iFlagThread = 0;
        NetworkStream[] NClient_Stream=new NetworkStream[2];
        NetworkStream nc1;
        public Thread[] thread=new Thread[2];
        public Dictionary<string, string> DicStation = new Dictionary<string, string>
          
        { {ConfigurationManager.AppSettings.GetValues(2)[0],ConfigurationManager.AppSettings.GetValues(3)[0]},{ConfigurationManager.AppSettings.GetValues(5)[0],ConfigurationManager.AppSettings.GetValues(6)[0]}
        };
        public Dictionary<int, string> DicStation_1 = new Dictionary<int, string>
        { {0,ConfigurationManager.AppSettings.GetValues(3)[0]},{1,ConfigurationManager.AppSettings.GetValues(6)[0]}
        };
        public Dictionary<string, int> DicStationFlag = new Dictionary<string, int>
        { {ConfigurationManager.AppSettings.GetValues(2)[0],0},{ConfigurationManager.AppSettings.GetValues(5)[0],1}
        };
        public Dictionary<string, bool> DicStation_status_online = new Dictionary<string, bool>();
        public byte[] bDownload_Test = { 0x7E, 00 ,00 ,0x25 ,00, 03, 00 ,00 ,0xC0,
0x0D, 0x0C ,0x15, 00 ,00 ,0x1E, 00, 0xC9, 0x12, 00, 00, 00, 00, 00 ,00, 00, 00, 00 ,00, 00, 00 ,0xff ,00, 00 ,00 ,0x28 ,0x23 ,00 ,00, 00 ,00 ,00, 
0x0D, 0x0C, 0x15, 00, 01, 00, 00 ,0xC9 ,0x12, 00, 00 ,00 ,00, 00 ,00, 00, 00 ,00 ,00, 00, 00 ,00, 00 ,00, 00, 0x28, 0x24, 00 ,00, 00, 00 ,00 ,
0x0D, 0x0C ,0x15, 00 ,01 ,0x36 ,00, 0xC9, 0x12, 00 ,00 ,00, 00 ,00, 00 ,00, 00, 00 ,00, 00 ,00, 00 ,00, 00, 00 ,0x28 ,0x20, 00, 00, 00, 00, 00 ,
0x0D ,0x0C ,0x15, 00 ,02, 0x18 ,00 ,0xCA ,0x12 ,00 ,00 ,00 ,00 ,00, 00 ,00, 00 ,00 ,00, 00 ,00, 00 ,00, 00, 00 ,0x28 ,0x20, 00, 00, 00, 00, 00, 
0x0D, 0x0C ,0x15 ,00 ,02 ,0x36 ,00, 0xCA ,0x12 ,00 ,00, 00, 00 ,00, 00 ,00, 00 ,00, 00, 00 ,00, 00 ,00, 00, 00 ,0x28 ,0x21, 00, 00, 00, 00, 00, 
0x0D ,0x0C ,0x15, 00, 03 ,0x18 ,00, 0xCA ,0x12 ,00 ,00, 00, 00 ,00, 00 ,00, 00 ,00 ,00 ,00, 00, 00, 00, 00, 00 ,0x28 ,0x21, 00, 00, 00, 00, 00, 0x19 };
        public CP_WeatherStation()
        {
            InitializeComponent();

            Process[] localByName = Process.GetProcessesByName("CP_WSMonitor_BackGround");

            //这里的360tray.exe就是你想要执行的程序的进程的名称。基本上就是.exe文件的文件名。

            //localByName得到的是进程中所有名称为"360tray.exe"的进程。
            if (localByName.Length > 0) //如果得到的进程数是0, 那么说明程序未启动，需要启动程序
            {
                //MsgRecord("存在");
                //localByName[0].CloseMainWindow();
                localByName[0].Kill();
                // Process.Start("E://开发/CP_WSTATION/WeatherFire/bin/Release/CP_WSMonitor_BackGround.exe"); //启动程序 "c://360tray.exe" 是程序的路径
            }

            else
            {
               // MsgRecord("不存在");
                //如果程序已经启动，则执行这一部分代码

            }

            dataGridView1.CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridView1_CellMouseDown);
            dataGridView1.Sorted += new EventHandler(dataGridView1_Sorted);
            string cnString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
            string cnString_Mysql = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
            //SqlConnection conn = new SqlConnection(cnString_Mysql);
            MySqlConnection conn = new MySqlConnection(cnString_Mysql);
            DataSet myDs = new DataSet();
            conn.Open();
            // using (SqlDataAdapter myDa = new SqlDataAdapter("select Station_ID,Station_Name,Station_Type,Station_Mode,Station_DataTime_Last,Station_Lan,Station_Lat from CP_Station", conn))
            using (MySqlDataAdapter myDa = new MySqlDataAdapter("select Station_ID,Station_Name,Station_Type,Station_Mode,Station_DataTime_Last,Station_Lan,Station_Lat from CP_Station", conn))
            {


                myDa.Fill(myDs);
            }
            conn.Close();
            DataGridViewIni(myDs);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);

            this.comboBox1.Text = ConfigurationManager.AppSettings.GetValues(0)[0];
            textBox_ID.Text = Get_Selected_Station_ID();
            this.comboBox2.Text = ConfigurationManager.AppSettings.GetValues(1)[0];
            this.textBox_IPAddress.Text = ConfigurationManager.AppSettings.GetValues(3)[0];
            this.textBox_Port.Text = ConfigurationManager.AppSettings.GetValues(4)[0];
            this.comboBox_STATION_Select.Text = ConfigurationManager.AppSettings.GetValues(2)[0];
            comboBox_STATION_Select.Items.Add(ConfigurationManager.AppSettings.GetValues(2)[0]);
            comboBox_STATION_Select.Items.Add(ConfigurationManager.AppSettings.GetValues(5)[0]);
          
            Control.CheckForIllegalCrossThreadCalls = false;
            order order1 = new order();

            /*tcplistener 服务器端作server start*/
            DataSet[0] = ExecuteQueryForDataSet(0);
            sStation_ID_GPRS = GetIP(DataSet[0], "sID");
            sStation_StationType = GetIP(DataSet[0], "sType");
            Thread th1 = new Thread(new ThreadStart(Thread_Listener));
            th1.IsBackground = true;
            th1.Start();
            Thread.Sleep(1000);
            /*tcplistener end*/

            /*tcp/ip 服务器端作为client start*/
            DataSet[1] = ExecuteQueryForDataSet(1);
            sStation_ID = GetIP(DataSet[1], "sID");
            sStation_IP = GetIP(DataSet[1], "IP");

            threads = new Thread[sStation_IP.Count];
            for (int i = 0; i < sStation_IP.Count; i++)
            {
                iFlagThread = i;
                Client client = new Client();
                client.sStation_ID = sStation_ID[i];
                client.sStation_Mode = 1;
                dic_Client.Add(sStation_ID[i], client);
                MsgRecord(client.sStation_ID);
                try
                {
                    ts = new ThreadStart(Thread_Start);
                    //threads[i] = new Thread(ts);
                    //threads[i].IsBackground = true;
                    //threads[i].Start();

                    dic_thread.Add(sStation_ID[i],new Thread(ts));
                    //dic_Net_Status.Add(sStation_ID[i],-1);
                    dic_Net_Status.Add(sStation_ID[i], -1);
                    dic_Station_flag.Add(sStation_ID[i], i);
                    dic_thread_check.Add(sStation_ID[i], new Thread(new ThreadStart(thread_check_start)));
                   
                    dic_thread_check[sStation_ID[i]].IsBackground = true;
                    dic_thread_check[sStation_ID[i]].Start();
                    Thread.Sleep(100);
                    dic_thread[sStation_ID[i]].IsBackground = true;
                    dic_thread[sStation_ID[i]].Start();
                    Thread.Sleep(100);
                   
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
            /*tcp/ip end*/



        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        } 
        private void timerCall(object obj)
        {
            MessageBox.Show("端口连接超时", "系统提示");
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text == "打开串口")
            {
                try
                {
                   // button5.Click+=new EventHandler(button5_Click);
                    serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    if (serialPort1.IsOpen)
                        serialPort1.Close();
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.PortName = comboBox1.Text;
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["COM_NUMBER"].Value = comboBox1.Text;
                  
                    config.AppSettings.Settings["COM_BAUT"].Value = comboBox2.Text;
                    config.Save(ConfigurationSaveMode.Modified);

                   
                    

                    serialPort1.Open();
                    button1.Text = "关闭串口";
                    Command_Set_Send_Receive(0x1E);
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口被占用");
                    // textBox1.text+=
                }
            }
            else
            {
                serialPort1.Close();
                button1.Text = "打开串口";
            }
        }
        public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //if (button1.Text == "关闭串口")
            //{
            System.Threading.Thread.Sleep(200);
            int bytes = serialPort1.BytesToRead;
            byte[] buffer = new byte[bytes];
            if (bytes == 0)
            { return; }
            serialPort1.Read(buffer, 0, bytes);
            //string s100 = ByteArrayToHexString(buffer);//字节数组转为十六进制字符串
            Data_Process(buffer, 96, "CP_Sensor_Data_Hour");
            // System.Threading.Thread.Sleep(1000);
            // }
        }


        public static string ByteArrayToHexString(byte[] data)//字节数组转为十六进制字符串
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            // sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().Trim(new char[] { '\0', ' ' }).ToUpper(); //sReceiveData = sReceiveData.Trim(new char[] { '\0', ' ' }).ToUpper();
        }
        public static string ByteArrayToHexString_Get(byte[] data)//字节数组转为十六进制字符串
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                // sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().Trim(new char[] { '\0', ' ' }).ToUpper();
        }
        public static string DataProccess(byte[] data)
        {
            string sData = DateTime.Now.ToString() + ":";
           // string sDataWD = "";
            order order1 = new order();

            return sData;
        }
        public static float DataChange(byte[] dataInput)
        {
            try
            {
                if (dataInput.Length == 4)
                {
                    byte[] bLinshi = new byte[4];
                    bLinshi[0] = dataInput[3];
                    bLinshi[1] = dataInput[2];
                    bLinshi[2] = dataInput[1];
                    bLinshi[3] = dataInput[0];
                    float fReturn = BitConverter.ToSingle(bLinshi, 0);
                    return fReturn;
                }
                else
                    return -32768;
            }
            catch (Exception ex)
            { return -32768; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x22,dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            bCommand_Flag = 0x22;

        }
        /*串口发送处理函数 ---开始*/
        public void SendSerial(byte bFlag)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.DtrEnable = true;
                    serialPort1.RtsEnable = true;
                    serialPort1.Open();
                }


                order order1 = new order();
                // string str = ByteArrayToHexString(data);
                switch (bFlag)
                {
                    case 0x10:
                        byte[] data_Restart = order1.bCommand_Send_Restart;
                        serialPort1.Write(data_Restart, 0, data_Restart.Length);
                       // MsgRecord(order1.DicCommand_Number[0x10] + ByteArrayToHexString(data_Restart));
                        MsgRecord(order1.DicCommand_Number[0x10]);
                        break;
                    case 0x11:
                        byte[] data_Reset = order1.bCommand_Send_ReSet;
                        serialPort1.Write(data_Reset, 0, data_Reset.Length);
                        //MsgRecord(order1.DicCommand_Number[0x10] + ByteArrayToHexString(data_Reset));
                        MsgRecord(order1.DicCommand_Number[0x10]);
                        break;

                    case 0x12:

                        byte[] data_Data_JiBenCanshu_Get = order1.bCommand_Send_LanMsg;
                        serialPort1.Write(data_Data_JiBenCanshu_Get, 0, data_Data_JiBenCanshu_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x12] + ByteArrayToHexString(data_Data_JiBenCanshu_Get));
                        MsgRecord(order1.DicCommand_Number[0x12]);
                        break;
                    case 0x13:
                        try
                        {
                           // byte[] data_Lat_Set = order1.Longitude_Latitude_Elevation_Set(Convert.ToDouble(textBox_Lan.Text), Convert.ToDouble(textBox_Lat.Text), Convert.ToDouble(textBox_Sealevel.Text));
                           // serialPort1.Write(data_Lat_Set, 0, data_Lat_Set.Length);
                            //MsgRecord(order1.DicCommand_Number[0x13] + ByteArrayToHexString(data_Lat_Set));
//MsgRecord(order1.DicCommand_Number[0x13]);
                        }
                        catch (Exception ex)
                        {
                            MsgRecord(ex.ToString());
                        }
                        break;
                    case 0x14:

                        byte[] data_Sensor_Status_Set = order1.Sensor_Status_Set(Set_Sensor_Status());
                        serialPort1.Write(data_Sensor_Status_Set, 0, data_Sensor_Status_Set.Length);
                       // MsgRecord(order1.DicCommand_Number[0x14] + ByteArrayToHexString(data_Sensor_Status_Set));
                        MsgRecord(order1.DicCommand_Number[0x14]);
                        break;
                    case 0x15:

                        byte[] data_Sensor_Status_Get = order1.bCommand_Send_SensorStatus;
                        serialPort1.Write(data_Sensor_Status_Get, 0, data_Sensor_Status_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x15] + ByteArrayToHexString(data_Sensor_Status_Get));
                        MsgRecord(order1.DicCommand_Number[0x15]);
                        break;
                    case 0x16:

                       // byte[] data_Save_Set = order1.Save_Energy_Set(Convert.ToByte(comboBox_Save_Set.Text));
                       // serialPort1.Write(data_Save_Set, 0, data_Save_Set.Length);
                       // MsgRecord(order1.DicCommand_Number[0x14] + ByteArrayToHexString(data_Save_Set));
                       // MsgRecord(order1.DicCommand_Number[0x14]);
                        break;
                    case 0x17:
                        byte[] data_Save_Get = order1.bCommand_Send_SaveEnergy;
                        serialPort1.Write(data_Save_Get, 0, data_Save_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x17] + ByteArrayToHexString(data_Save_Get));
                        MsgRecord(order1.DicCommand_Number[0x17]);
                        break;
                    case 0x18:

                        byte[] data_Data_Delete_Get = order1.bCommand_Send_DataDelete;
                        serialPort1.Write(data_Data_Delete_Get, 0, data_Data_Delete_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x18] + ByteArrayToHexString(data_Data_Delete_Get));
                        MsgRecord(order1.DicCommand_Number[0x18]);
                        break;
                    case 0x19:
                        byte[] data_ID_Get = order1.bCommand_Send_IDGet;
                        serialPort1.Write(data_ID_Get, 0, data_ID_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x19] + ByteArrayToHexString(data_ID_Get));
                        MsgRecord(order1.DicCommand_Number[0x19]);
                        break;
                    case 0x1A:
                        byte[] data_Status_Get = order1.bCommand_Send_SensorBatteryStatus;
                        serialPort1.Write(data_Status_Get, 0, data_Status_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1A] + ByteArrayToHexString(data_Status_Get));
                        MsgRecord(order1.DicCommand_Number[0x1A]);
                        break;
                    case 0x1B:

                        byte[] data_Data_Jishi_Get = order1.bCommand_Send_DataGet;
                        serialPort1.Write(data_Data_Jishi_Get, 0, data_Data_Jishi_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1B] + ByteArrayToHexString(data_Data_Jishi_Get));
                        MsgRecord(order1.DicCommand_Number[0x1B]);
                        break;
                    case 0x1C:

                        string str_ID = textBox_ID.Text;
                        if (str_ID.Length == 9)
                        {
                            // order order1 = new order();
                            byte[] data_ID_Set = order1.ID_Set(str_ID);
                            serialPort1.Write(data_ID_Set, 0, data_ID_Set.Length);
                            MsgRecord("设置设备ID" + str_ID);
                        }
                        else
                            MsgRecord("设置设备ID" + "长度只支持9位");

                        break;
                    case 0x1D:
                        byte[] data_Time_Get = order1.bCommand_Send_TimeGet;
                        serialPort1.Write(data_Time_Get, 0, data_Time_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1D] + ByteArrayToHexString(data_Time_Get));
                        MsgRecord(order1.DicCommand_Number[0x1D]);
                        break;
                    case 0x1E:

                        //string str = sJianGe_Set(comboBox_send.Text.Trim());

                        byte[] data_Time_Set = order1.Time_Set(DateTime.Now);
                        serialPort1.Write(data_Time_Set, 0, data_Time_Set.Length);
                        //MsgRecord("设置" + order1.DicCommand_Number[0x1E] + ByteArrayToHexString(data_Time_Set));
                        MsgRecord(order1.DicCommand_Number[0x1E]);
                        break;
                    case 0x21:
                        byte[] data_Jiange_Get = order1.bCommand_Send_JiangeGet;
                        serialPort1.Write(data_Jiange_Get, 0, data_Jiange_Get.Length);
                       // MsgRecord(order1.DicCommand_Number[0x21] + ByteArrayToHexString(data_Jiange_Get));
                        MsgRecord(order1.DicCommand_Number[0x21]);
                        break;
                    case 0x22:

                        string str = sJianGe_Set(comboBox_send.Text.Trim());

                        byte[] data = order1.JianGe_Set(str);
                        serialPort1.Write(data, 0, data.Length);
//MsgRecord("设置" + order1.DicCommand_Number[0x22] + str + ByteArrayToHexString(data))
                              MsgRecord("设置-"+order1.DicCommand_Number[0x22]);
                        break;
                    //case 0x23:
                    //    if (comboBox_AutoMode.Text == "开")
                    //    {

                    //        byte[] data_Auto_Set = order1.bCommand_Send_AutoModeSet_Open;
                    //        serialPort1.Write(data_Auto_Set, 0, data_Auto_Set.Length);
                    //        //MsgRecord(order1.DicCommand_Number[0x23] + ByteArrayToHexString(data_Auto_Set));
                    //        MsgRecord(order1.DicCommand_Number[0x23]);
                    //    }
                    //    else if (comboBox_AutoMode.Text == "关")
                    //    {
                    //        byte[] data_Auto_Set = order1.bCommand_Send_AutoModeSet_Close;
                    //        serialPort1.Write(data_Auto_Set, 0, data_Auto_Set.Length);
                    //      //  MsgRecord(order1.DicCommand_Number[0x23] + ByteArrayToHexString(data_Auto_Set));
                    //        MsgRecord(order1.DicCommand_Number[0x23]);
                    //    }
                    //    break;
                    case 0x24:
                        byte[] data_Auto_Get = order1.bCommand_Send_AutoModeGet;
                        serialPort1.Write(data_Auto_Get, 0, data_Auto_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x24] + ByteArrayToHexString(data_Auto_Get));
                        MsgRecord(order1.DicCommand_Number[0x24]);
                        break;
                    case 0x25:

                        byte[] data_Data_AllData_Get = order1.bCommand_Send_AllDataGet;
                        serialPort1.Write(data_Data_AllData_Get, 0, data_Data_AllData_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x25] + ByteArrayToHexString(data_Data_AllData_Get));
                        MsgRecord(order1.DicCommand_Number[0x25]);
                        break;
                    case 0x27:

                        //string str = sJianGe_Set(comboBox_send.Text.Trim());

                        string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                        DateTime dt = Convert.ToDateTime(sDatetime);
                        byte[] data_Download_Send = order1.Data_Download_Modify(dt, DateTime.Now, 1);
                        serialPort1.Write(data_Download_Send, 0, data_Download_Send.Length);
                        //MsgRecord(order1.DicCommand_Number[0x27] + ByteArrayToHexString(data_Download_Send));
                        MsgRecord(order1.DicCommand_Number[0x27]);
                        break;
                }


                // System.Threading.Thread.Sleep(100);



            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(), "系统提示"); }
        }
        /*串口发送处理函数 ---结束*/
        public bool ReceivePro(byte[] bReceive, byte bCommand)
        {
            bool boolReturn = false;
            order order1 = new order();
           // string clientStr = Encoding.GetEncoding("GB18030").GetString(bReceive);
            string clientStr = ByteArrayToHexString(bReceive).Trim(new char[] { '\0', ' ' }).ToUpper();
            MsgRecord("设置" + order1.DicCommand_Number[bCommand]);
            if (bReceive[0] == 0x7E && bReceive[3] == bCommand)
            {
                if (bReceive[6] == 0)
                {
                    MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "成功！");
                    boolReturn = true;

                }
                else if (bReceive[6] == 0x80)
                {
                    MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "失败！" + ByteArrayToHexString(bReceive));

                }
                else if (bReceive[6] == 0x81)
                {
                    MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "失败！错误代码:指令错误（指令码错误或设备不支持改指令）" + ByteArrayToHexString(bReceive));

                }
                else if (bReceive[6] == 0x82)
                {
                    MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "失败！错误代码:校验码错误" + ByteArrayToHexString(bReceive));

                }
                else if (bReceive[6] == 0x84)
                {
                    MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "失败！错误代码:参数错误" + ByteArrayToHexString(bReceive));

                }

            }
            else
            {
                MsgRecord("设置" + order1.DicCommand_Number[bCommand] + "失败！" + ByteArrayToHexString(bReceive));

            }
            return boolReturn;

        }
        /*下载数据处理函数 ---开始*/
        public bool ReceiveDownload(byte[] bReceive, int bCount, int bRecord,string sID)
        {
            bool boolReturn = false;
            order order1 = new order();
            Math math1 = new Math();
            string sReceiveData = ByteArrayToHexString(bReceive);
            sReceiveData.TrimEnd();
            // byte bJiaoyan = 0;
            double iSolar = 673.3;
            if (sID == ConfigurationManager.AppSettings.GetKey(8))
            { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(8)[0]); }
            else if (sID == ConfigurationManager.AppSettings.GetKey(9))
            { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(9)[0]); }
            string[] sReceiveData_Pro = sReceiveData.Split(' ');
            if (bRecord > 255)
                bRecord -= 255;
            if (bReceive[5] == (byte)bRecord)
            {
                // if (order1.Check_Data(bReceive, 9, bReceive.Length - 2) != bReceive[bReceive.Length - 1])
                // {
                string sTem = "", sHum, sWS, sWD, sBar, sRain,sSoiltem;
                MsgRecord("下载数据第" + bRecord + "包未解析:");// + sReceiveData正确回复应该为94长度，带回车换行96
                //string sDataFormat = "站点ID:" + bReceive[1] + ",";
                for (int i = 0; i < bReceive[8] / 32; i++)
                {

                    string sDataFormat = "数据时间:" + bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i] + ",";

                    if (sReceiveData_Pro[15 + 32 * i].Substring(0, 1) == "0")
                    {
                        sDataFormat += "温度:" + (double)(Convert.ToInt32((sReceiveData_Pro[15 + 32 * i].Substring(1, 1) + sReceiveData_Pro[16 + 32 * i]), 16)) / 10 + ",";
                        sTem = ((double)(Convert.ToInt32((sReceiveData_Pro[15 + 32 * i].Substring(1, 1) + sReceiveData_Pro[16 + 32 * i]), 16)) / 10).ToString();
                    }
                    else if (sReceiveData_Pro[15 + 32 * i].Substring(0, 1) == "F")
                    {
                        sDataFormat += "温度:" + (double)(0 - Convert.ToInt32((sReceiveData_Pro[15 + 32 * i].Substring(1, 1) + sReceiveData_Pro[16 + 32 * i]), 16)) / 10 + ",";
                        sTem = ((double)(0 - Convert.ToInt32((sReceiveData_Pro[15 + 32 * i].Substring(1, 1) + sReceiveData_Pro[16 + 32 * i]), 16)) / 10).ToString();
                    }
                    sDataFormat += "湿度:" + bReceive[17 + 32 * i] + ",";
                    sHum = bReceive[17 + 32 * i].ToString();
                    sDataFormat += "风速:" + (double)(Convert.ToInt32((sReceiveData_Pro[19 + 32 * i] + sReceiveData_Pro[20 + 32 * i]), 16)) / 10 + ",";

                    sDataFormat += "风向:" + Convert.ToInt32((sReceiveData_Pro[21 + 32 * i] + sReceiveData_Pro[22 + 32 * i]), 16) + ",";
                    sDataFormat += "保留:" + (double)(Convert.ToInt32((sReceiveData_Pro[23 + 32 * i] + sReceiveData_Pro[24 + 32 * i]), 16)) / 10 + ",";
                    sDataFormat += "土壤湿度:" + Convert.ToInt32((sReceiveData_Pro[25 + 32 * i] + sReceiveData_Pro[26 + 32 * i]), 16) /10+ ",";
                    sDataFormat += "10分平均风速:" + (double)(Convert.ToInt32((sReceiveData_Pro[27 + 32 * i] + sReceiveData_Pro[28 + 32 * i]), 16)) / 10 + ",";
                    /*土壤温度---开始---*/
                   // sDataFormat += "土壤温度:" + Convert.ToInt32((sReceiveData_Pro[29 + 32 * i] + sReceiveData_Pro[30 + 32 * i]), 16)/10 + ",";
                    if (sReceiveData_Pro[29 + 32 * i].Substring(0, 1) == "0")
                    {
                        sDataFormat += "土壤温度:" + (double)(Convert.ToInt32((sReceiveData_Pro[29 + 32 * i].Substring(1, 1) + sReceiveData_Pro[30 + 32 * i]), 16)) / 10 + ",";
                        sSoiltem = ((double)(Convert.ToInt32((sReceiveData_Pro[29 + 32 * i].Substring(1, 1) + sReceiveData_Pro[30 + 32 * i]), 16)) / 10).ToString();
                    }
                    else
                    {
                        sDataFormat += "土壤温度:" + (double)(0 - Convert.ToInt32((sReceiveData_Pro[29 + 32 * i].Substring(1, 1) + sReceiveData_Pro[30 + 32 * i]), 16)) / 10 + ",";
                        sSoiltem = ((double)(0 - Convert.ToInt32((sReceiveData_Pro[29 + 32 * i].Substring(1, 1) + sReceiveData_Pro[30 + 32 * i]), 16)) / 10).ToString();
                    }
                    /*土壤温度---结束*/
                    sDataFormat += "日雨量:" + (double)(Convert.ToInt32((sReceiveData_Pro[31 + 32 * i] + sReceiveData_Pro[32 + 32 * i]), 16)) / 10*3 + ",";
                    sDataFormat += "气压:" + (double)(Convert.ToInt32((sReceiveData_Pro[33 + 32 * i] + sReceiveData_Pro[34 + 32 * i] + sReceiveData_Pro[35 + 32 * i]), 16)) / 10 + ",";
                    sDataFormat += "连续无雨日:" + Convert.ToInt32((sReceiveData_Pro[36 + 32 * i] + sReceiveData_Pro[37 + 32 * i]), 16) + ",";
                    sDataFormat += "状态码:" + bReceive[38 + 32 * i] + ",";
                    MsgRecord(sID+"下载数据解析后:" + sDataFormat);
                    UpdataInDB(Convert.ToDateTime(bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i]),sID);
                    if (dic_Client[sID].sStation_Type == 0)
                    {
                        if (bReceive[13 + 32 * i] == 0 && bReceive[14 + 32 * i] == 0)
                        {
                            RecordDataInDB(
                                Convert.ToDateTime(bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i]),//数据时间
                                sID,//站点ID
                                Convert.ToDouble(sTem),//温度
                                Convert.ToInt32(sHum),//湿度
                                (double)(Convert.ToInt32((sReceiveData_Pro[19 + 32 * i] + sReceiveData_Pro[20 + 32 * i]), 16)) / 10,//风速
                                Convert.ToInt32((sReceiveData_Pro[21 + 32 * i] + sReceiveData_Pro[22 + 32 * i]), 16),//风向
                                (double)(Convert.ToInt32((sReceiveData_Pro[33 + 32 * i] + sReceiveData_Pro[34 + 32 * i] + sReceiveData_Pro[35 + 32 * i]), 16)) / 10,//气压
                                 (double)(Convert.ToInt32((sReceiveData_Pro[27 + 32 * i] + sReceiveData_Pro[28 + 32 * i]), 16)) * 100 / iSolar,//太阳辐射
                                (double)(Convert.ToInt32((sReceiveData_Pro[31 + 32 * i] + sReceiveData_Pro[32 + 32 * i]), 16)) / 10 * 3,//日雨量
                                0,//二氧化碳

                               Convert.ToDouble(sSoiltem),//土壤温度
                                 math1.soilmoist_Math((double)(Convert.ToInt32((sReceiveData_Pro[25 + 32 * i] + sReceiveData_Pro[26 + 32 * i]), 16)) / 10),//土壤湿度
                                "", "", "CP_Sensor_Data_Hour");
                        }
                        else
                        {
                            RecordDataInDB(
                               Convert.ToDateTime(bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i]),
                              sID,
                               Convert.ToDouble(sTem), Convert.ToInt32(sHum),
                               (double)(Convert.ToInt32((sReceiveData_Pro[19 + 32 * i] + sReceiveData_Pro[20 + 32 * i]), 16)) / 10,
                               Convert.ToInt32((sReceiveData_Pro[29 + 32 * i] + sReceiveData_Pro[30 + 32 * i]), 16),
                               (double)(Convert.ToInt32((sReceiveData_Pro[21 + 32 * i] + sReceiveData_Pro[22 + 32 * i] + sReceiveData_Pro[35 + 32 * i]), 16)) / 10,
                               (double)(Convert.ToInt32((sReceiveData_Pro[27 + 32 * i] + sReceiveData_Pro[28 + 32 * i]), 16)) * 100 / iSolar,
                               (double)(Convert.ToInt32((sReceiveData_Pro[31 + 32 * i] + sReceiveData_Pro[32 + 32 * i]), 16)) / 10 * 3,
                               0,
                              Convert.ToDouble(sSoiltem),//土壤温度
                                  math1.soilmoist_Math((double)(Convert.ToInt32((sReceiveData_Pro[25 + 32 * i] + sReceiveData_Pro[26 + 32 * i]), 16)) / 10),//土壤湿度
                                "", "", "CP_Sensor_Data");
                        }
                    }
                    else
                    {
                        if (bReceive[13 + 32 * i] == 0 && bReceive[14 + 32 * i] == 0)
                        {
                            RecordDataInDB_SoilStation(
                                Convert.ToDateTime(bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i]),//数据时间
                                sID,//站点ID
                               (double)(bReceive[15 + 32 * i]*256 + bReceive[16 + 32 * i]) / 100,//土温1
                               (double)(bReceive[17 + 32 * i]*256 + bReceive[18 + 32 * i])/100,//土湿1
                               (double)(bReceive[19 + 32 * i]*256 + bReceive[20 + 32 * i]),//土盐1
                               (double)(bReceive[21 + 32 * i] * 256 + bReceive[22 + 32 * i]) / 100,//土温2
                               (double)(bReceive[23 + 32 * i] * 256 + bReceive[24 + 32 * i]) / 100,//土湿2
                               (double)(bReceive[25 + 32 * i] * 256 + bReceive[26 + 32 * i]),//土盐2
                               (double)(bReceive[27 + 32 * i] * 256 + bReceive[28 + 32 * i]) / 100,//土温3
                               (double)(bReceive[29 + 32 * i] * 256 + bReceive[30 + 32 * i]) / 100,//土湿3
                               (double)(bReceive[31 + 32 * i] * 256 + bReceive[32 + 32 * i]),//土盐3
                               (double)(bReceive[33 + 32 * i] * 256 + bReceive[34 + 32 * i]) / 100,//土温4
                               (double)(bReceive[35 + 32 * i] * 256 + bReceive[36 + 32 * i]) / 100,//土湿4
                               (double)(bReceive[37 + 32 * i] * 256 + bReceive[38 + 32 * i]),//土盐4
 "", "", "CP_Sensor_Data_Hour");
                        }
                        else
                        {
                            RecordDataInDB_SoilStation(
                                Convert.ToDateTime(bReceive[9 + 32 * i] + "/" + bReceive[10 + 32 * i] + "/" + bReceive[11 + 32 * i] + " " + bReceive[12 + 32 * i] + ":" + bReceive[13 + 32 * i] + ":" + bReceive[14 + 32 * i]),//数据时间
                                sID,//站点ID
                               (double)(bReceive[15 + 32 * i] * 256 + bReceive[16 + 32 * i]) / 100,//土温1
                               (double)(bReceive[17 + 32 * i] * 256 + bReceive[18 + 32 * i]) / 100,//土湿1
                               (double)(bReceive[19 + 32 * i] * 256 + bReceive[20 + 32 * i]),//土盐1
                               (double)(bReceive[21 + 32 * i] * 256 + bReceive[22 + 32 * i]) / 100,//土温2
                               (double)(bReceive[23 + 32 * i] * 256 + bReceive[24 + 32 * i]) / 100,//土湿2
                               (double)(bReceive[25 + 32 * i] * 256 + bReceive[26 + 32 * i]),//土盐2
                               (double)(bReceive[27 + 32 * i] * 256 + bReceive[28 + 32 * i]) / 100,//土温3
                               (double)(bReceive[29 + 32 * i] * 256 + bReceive[30 + 32 * i]) / 100,//土湿3
                               (double)(bReceive[31 + 32 * i] * 256 + bReceive[32 + 32 * i]),//土盐3
                               (double)(bReceive[33 + 32 * i] * 256 + bReceive[34 + 32 * i]) / 100,//土温4
                               (double)(bReceive[35 + 32 * i] * 256 + bReceive[36 + 32 * i]) / 100,//土湿4
                               (double)(bReceive[37 + 32 * i] * 256 + bReceive[38 + 32 * i]),//土盐4
 "", "", "CP_Sensor_Data");
                        }
                    }
                    boolReturn = true;

                }

                // }
                // else
                // { MsgRecord("错误提示:校验码错误"); }
            }
            else
            { MsgRecord("错误提示:数据有误" + order1.JishiData_Return_Pro(bReceive)); }
            return boolReturn;
        }
        /*下载数据处理函数 ---结束*/
        public bool ReceiveGet(byte[] bReceive, byte bCommand)
        {
            bool boolReturn = false;
            order order1 = new order();
            if (bCommand == 0x1B)
            {
                Data_Process(bReceive, 96, "CP_Sensor_Data");
              //  Data_Process_Socket(buffer, 96, "CP_Sensor_Data_Hour", NClient_Stream);
                
            }
            else if (bCommand == 0x10)
            {
                //MsgRecord("设备重启:" + ByteArrayToHexString(bReceive));
                MsgRecord("设备重启成功！");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x11)
            {
               // MsgRecord("设备初始化:" + ByteArrayToHexString(bReceive));
                MsgRecord("设备初始化成功！");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x12)
            {
                MsgRecord("设备基本参数信息:" + order1.Basic_Msg_Return_Pro(bReceive));
                boolReturn = Check_Optcode(bReceive, bCommand, "获取");
            }
            else if (bCommand == 0x13)
            {
                MsgRecord("设备基本参数信息:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x14)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord("设备传感器状态:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x15)
            {
                Check_Sensor_Status(bReceive);
                MsgRecord("设备传感器状态:");
                boolReturn = Check_Optcode(bReceive, bCommand, "获取");
            }
            else if (bCommand == 0x16)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord("设备省电模式:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x17)
            {

                if (bReceive[3] == 0x17)
                {

                    MsgRecord("设备省电模式:" + bReceive[9] + "档");
                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x18)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord("清空数据:" );
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x19)
            {

                if (bReceive[3] == 0x19 && order1.Check_Data(bReceive, 9, 23) == bReceive[24])
                {
                    string sID = ""; string sGujian = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 9);


                    // System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sGujian += asciiEncoding.GetString(bReceive, 18, 6);

                    MsgRecord("设备ID:" + sID + ",固件:" + sGujian);

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x1A)
            {

                if (bReceive[3] == 0x1A)
                {

                    MsgRecord("设备传感器电池状态码:" + bReceive[9]);
                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x1D)
            {

                if (bReceive[3] == 0x1D && order1.Check_Data(bReceive, 9, 20) == bReceive[21])
                {
                    string sID = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 12);


                    MsgRecord("设备时间:" + sID + " 原始数据:");

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x16)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord("设备时间:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x21)
            {

                if (bReceive[3] == 0x21 && order1.Check_Data(bReceive, 9, 12) == bReceive[13])
                {
                    string sID = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 4);


                    MsgRecord("发送间隔" + sID + "分钟" + ByteArrayToHexString(bReceive));

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x24)
            {
                if (bReceive[3] == 0x24 && (bReceive[9] == bReceive[10]))
                {
                    if (bReceive[9] == 0)
                    { MsgRecord("自动发送模式关"); }
                    else
                    { MsgRecord("自动发送模式开"); }
                }
            }
            else if (bCommand == 0x27)
            {
                Command_Get_Download_Data_Bag(bReceive, bCommand);
            }
            else if (bCommand == 0x25)
            {
                // Thread DownLoad_Data_Hour = new Thread(new ParameterizedThreadStart(Command_Get_Download_Data_Bag(bReceive, bCommand)));
                Command_Get_Download_Data_Bag(bReceive, bCommand);
            }
            else
            {
                //if (bReceive[0] == 0x7E && bReceive[3] == bCommand)
                //{

            }
            return boolReturn;

        }

        public void MsgRecord(string str)
        {
            if (textBox1.Lines.Length >10)
                textBox1.Text = "";
            string sData = DateTime.Now.ToString() + ":";
            string FILE_NAME = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";

            textBox1.AppendText(sData);
            textBox1.AppendText(str + "\r\n");
            textBox1.ScrollToCaret();
            WriteFile(sData + str + "\r\n", FILE_NAME);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = new byte[1024];
                string sss = "";
                int iaa = serialPort1.Read(data, 0, serialPort1.BytesToRead);
                textBox1.Text += iaa + "\r\n";
                textBox1.Text += ByteArrayToHexString(data) + "\r\n";
                serialPort1.Close();

                MessageBox.Show("数据接收成功！", "系统提示");
            }
            catch (Exception ex)
            { /*FileLog.WriteError("button3", ex.ToString());*/ }

        }


        private static string[] sSensor(byte bSensor_Number)
        {
            try
            {
                Dictionary<byte, string[]> DicSensor_Number = new Dictionary<byte, string[]>();
                DicSensor_Number.Add(101, "温度,℃".Split(','));
                DicSensor_Number.Add(102, "湿度,％".Split(','));
                DicSensor_Number.Add(111, "气压,hPa".Split(','));
                DicSensor_Number.Add(112, "气压温度,℃".Split(','));
                DicSensor_Number.Add(211, "风向,度".Split(','));
                DicSensor_Number.Add(212, "ADC1,待定".Split(','));
                DicSensor_Number.Add(221, "风速,m/s".Split(','));
                DicSensor_Number.Add(222, "雨量,mm".Split(','));

                string[] sReurn = { DicSensor_Number[bSensor_Number][0], DicSensor_Number[bSensor_Number][1] };

                return sReurn;
            }
            catch (Exception ex)
            {
                string[] sReturn = { "无效", "无效" };
                return sReturn;
            }
        }

        private static string sJianGe_Set(string sJianGe)
        {
            try
            {
                Dictionary<string, string> DicSensor_Number = new Dictionary<string, string>();
                DicSensor_Number.Add("1分钟", "1");
                DicSensor_Number.Add("10分钟", "10");
                DicSensor_Number.Add("30分钟", "30");
                DicSensor_Number.Add("1小时", "60");
                DicSensor_Number.Add("2小时", "120");


                string sReurn = DicSensor_Number[sJianGe];

                return sReurn;
            }
            catch (Exception ex)
            {
                string sReturn = "无效";
                return sReturn;
            }
        }

        private void button_IDSet_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x1C, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            bCommand_Flag = 0x1C;

        }
        private void Command_Set_Send_Receive(byte bCommand)
        {
            try
            {
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // for (int i = 0; i < 3; i++)
                //{
                SendSerial(bCommand);

                System.Threading.Thread.Sleep(iTimeOut_Receive_Wait);
                if (serialPort1.BytesToRead > 0)
                {

                    int bytes = serialPort1.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    serialPort1.Read(buffer, 0, bytes);
                    bool boolSuccess = ReceivePro(buffer, bCommand);

                    if (boolSuccess)
                    {
                        serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        return;
                    }
                }
                else
                {

                    // System.Threading.Thread.Sleep(2000);
                    serialPort1.DiscardInBuffer();
                    MsgRecord("端口连接超时");
                    //  MessageBox.Show("端口连接超时", "系统提示");
                    // return;
                }

                // }

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                //  System.Threading.Thread.Sleep(1000);


            }
            catch (Exception ex)
            { MsgRecord("端口有误"); }
        }
        private void Command_Get_Send_Receive1(byte[] bCommand)
        {
            TcpClient tclient = new TcpClient();
            try
            {
                //string sRead = "";
                // tclient = new TcpClient();
                tclient.Connect("192.168.1.133", 4001);
                //tclient.Connect("192.168.0.111", 60000);
                NetworkStream ns = tclient.GetStream();
                // StreamWriter sw = new StreamWriter(ns);

               // string content = sData;
               // byte[] data = Encoding.Default.GetBytes(content);
                ns.Write(bCommand, 0, bCommand.Length);
                // NetworkStream ns = tclient.GetStream(); 
                
                // StreamWriter sw = new StreamWriter(ns);
                 StreamReader sr = new StreamReader(ns);
                // sw.Write(sData);
               // int bytes=sr.
                 System.Threading.Thread.Sleep(2000);
                char[] recvBytes=new char[1024];
              int icount=  sr.Read(recvBytes,0,recvBytes.Length);
                byte[] data = Encoding.Unicode.GetBytes(recvBytes);
                //  ns.Write(data, 0, data.Length);
                MsgRecord("设备初始化:" + ByteArrayToHexString(data));
                // sw.Close();
                //  ns.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Command_Get_Send_Receive(byte bCommand)
        {
            try
            {
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // for (int i = 0; i < 3; i++)
                //{
                SendSerial(bCommand);

                System.Threading.Thread.Sleep(iTimeOut_Receive_Wait);
                if (serialPort1.BytesToRead > 0)
                {

                    int bytes = serialPort1.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    serialPort1.Read(buffer, 0, bytes);
                    bool boolSuccess = ReceiveGet(buffer, bCommand);

                    if (boolSuccess)
                    {
                        serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        return;
                    }
                }
                else
                {

                    // System.Threading.Thread.Sleep(1000);
                    serialPort1.DiscardInBuffer();
                    //  if (i == 2)
                    MsgRecord("端口连接超时");
                    // MessageBox.Show("端口连接超时", "系统提示");
                    // return;
                }

                // }

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                //  System.Threading.Thread.Sleep(1000);


            }
            catch (Exception ex)
            { MsgRecord(ex.ToString()); }
        }

        private void Command_Get_Download_Send_Receive(byte bCommand)
        {
            try
            {
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // for (int i = 0; i < 3; i++)
                // {
                SendSerial(bCommand);

                System.Threading.Thread.Sleep(iTimeOut_Receive_Wait);
                if (serialPort1.BytesToRead > 0)
                {

                    int bytes = serialPort1.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    serialPort1.Read(buffer, 0, bytes);
                    MsgRecord(ByteArrayToHexString(buffer));
                    bool boolSuccess = ReceiveGet(buffer, bCommand);

                    if (boolSuccess)
                    {
                        //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        return;
                    }
                }
                else
                {

                    // System.Threading.Thread.Sleep(1000);
                    serialPort1.DiscardInBuffer();
                    // if (i == 2)
                    MsgRecord("端口连接超时");
                    // MessageBox.Show("端口连接超时", "系统提示");
                    // return;
                }

                //}

                //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                //  System.Threading.Thread.Sleep(1000);


            }
            catch (Exception ex)
            { MsgRecord(ex.ToString()); }
        }

        public void Command_Get_Download_Data_Bag(byte[] bReceive, byte bCommand)
        {
            order order1 = new order();
           
            string sReceiveData = order1.DownLoadData_Return_Pro(bReceive);
            if (bReceive.Length >= 14)
            {
                if (sReceiveData.Substring(0, 10) == "4844415441")
                {
                    Data_Process(bReceive, 96, "CP_Sensor_Data_Hour");
                    sReceiveData = sReceiveData.Substring(192);
                    if (sReceiveData.Substring(0, 8) == "7E000025")
                    {
                        int dData_Length = Convert.ToInt32(sReceiveData.Substring(18, 8), 16);
                        MsgRecord(dData_Length.ToString());
                        MsgRecord(ByteArrayToHexString(bReceive));
                        iData_Hour_Download = dData_Length;
                        bCommand_Download = bCommand;
                        serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        if (iData_Hour_Download > 0)
                        {
                            thread_Download = new Thread(new ThreadStart(Thread_Download));
                            thread_Download.Start();
                        }
                        else
                        {
                            MsgRecord("无数据下载！");
                        }
                    }
                }
                else if (bReceive[0] == 0x7E && bReceive[3] == bCommand)
                {
                    int dData_Length = Convert.ToInt32(sReceiveData.Substring(18, 8), 16);
                    MsgRecord(dData_Length.ToString());
                    MsgRecord(ByteArrayToHexString(bReceive));
                    iData_Hour_Download = dData_Length;
                    bCommand_Download = bCommand;
                    serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    if (iData_Hour_Download > 0)
                    {
                        thread_Download = new Thread(new ThreadStart(Thread_Download));
                        thread_Download.Start();


                    }
                    else
                    { MsgRecord("无数据下载！"); }
                }
                else
                {
                    serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                }
            }
        }
        private void button_TimeSet_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x1E, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            bCommand_Flag = 0x1E;
           // iThread_Flag = 1;
            //thread.
           // Command_Set_Send_Receive_TCPIP(0x1E, NClient_Stream);
        }

        private void button_Jishi_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x1B);
        }

        private void button_Basic_Msg_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x12);
        }

        public void Data_Process(byte[] bReceive, int iLengthDataOne, string sTable_Name)
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
                        sDataFormat += "土壤温度:" + sReceiveDataArray[10] + "℃,";
                        sDataFormat += "太阳辐射:" + sReceiveDataArray[11] + "m/s,";
                        sDataFormat += "土壤湿度:" + sReceiveDataArray[12] + "%\n\r";
                        sDataFormat += "日雨量:" + Convert.ToDouble(sReceiveDataArray[13]) * 2 + "mm,";
                        sDataFormat += "气压:" + sReceiveDataArray[14] + "hPa\n\r";
                        //sDataFormat += "连续无雨日:" + sReceiveDataArray[15] + ",";
                        sDataFormat += "状态码:" + sReceiveDataArray[16];
                        // MsgRecord("整点数据解析后:" + sDataFormat);

                        /*接收数据成功后，发送数据接收成功指令，判断是否整点，是-存入整点数据表，并校时;否-存入即时数据表*/
                        //serialPort1.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                        MsgRecord("发送成功解析数据确认！");

                        DateTime dt_Date = Convert.ToDateTime(sDT);
                       // UpdataInDB(dt_Date,
                        if (dt_Date.Minute == 0 && dt_Date.Second == 0)
                        {
                            Command_Set_Send_Receive(0x1E);
                            RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[11]), Convert.ToInt32(sReceiveDataArray[12]), Convert.ToDouble(sReceiveDataArray[14]), 0, Convert.ToDouble(sReceiveDataArray[13]) * 2, 0, Convert.ToDouble(sReceiveDataArray[10]), Convert.ToDouble(sReceiveDataArray[12]), "", "", "CP_Sensor_Data_Hour");
                        }
                        else
                        {
                            RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[11]), Convert.ToInt32(sReceiveDataArray[12]), Convert.ToDouble(sReceiveDataArray[14]), 0, Convert.ToDouble(sReceiveDataArray[13]) * 2, 0, Convert.ToDouble(sReceiveDataArray[10]), Convert.ToDouble(sReceiveDataArray[12]), "", "", "CP_Sensor_Data_Hour");
                        }


                        //RecordDataInDB()
                    }
                    else
                    { MsgRecord("错误提示:数据有误" + "详细:" + sReceiveData); }
                }
            }
        }

        public void RecordDataInDB(DateTime dateTime, string stationID, double temp, int hum, double windSpeed, int windDirection, double bar, double solar, double rain, double c02,double soilTemp, double soilhum,string more1 = "", string more2 = "", string sTable_Name = "")
        {
            Math math1 = new Math();
            string cnString = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
           // string sql = string.Format(" if not  exists (select 1 from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + dateTime + "')  Insert into " + sTable_Name + " " + "(CP_Data_DateTime, CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2) Values" + " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')", dateTime, stationID, temp, hum, windSpeed, windDirection, bar, solar, rain, c02, more1, more2);
            string sql = string.Format("  Insert into " + sTable_Name + " " + "(CP_Data_DateTime, CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2,CP_Data_Tuwen,CP_Data_Tushi) select" + " '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}' from dual where not exists (select * from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + dateTime + "')", dateTime, stationID, temp, hum, windSpeed, windDirection, bar,Convert.ToDouble(solar.ToString("0.0")), rain, c02, math1.solar_qiangdu(solar), more2, soilTemp, soilhum);

            MySqlConnection conn = new MySqlConnection(cnString);
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public void RecordDataInDB_SoilStation(DateTime dateTime, string stationID, double temp1, double Hum1, double Yan1, double temp2, double Hum2, double Yan2, double temp3, double Hum3, double Yan3, double temp4, double Hum4, double Yan4, string more1 = "", string more2 = "", string sTable_Name = "")
        {
            Math math1 = new Math();
            string cnString = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
            int iRRR = -32768;
            // string sql = string.Format(" if not  exists (select 1 from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + dateTime + "')  Insert into " + sTable_Name + " " + "(CP_Data_DateTime, CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2) Values" + " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')", dateTime, stationID, temp, hum, windSpeed, windDirection, bar, solar, rain, c02, more1, more2);
            string sql = string.Format("  Insert into " + sTable_Name + " " + "(CP_Data_DateTime,CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2, CP_Data_Tuwen, CP_Data_Tushi, CP_Data_Tuwen2,CP_Data_Tushi2,CP_Data_Tuwen3, CP_Data_Tushi3,CP_Data_Tuwen4,CP_Data_Tushi4,CP_Data_Tuyan1,CP_Data_Tuyan2,CP_Data_Tuyan3,CP_Data_Tuyan4) select" + " '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}' from dual where not exists (select * from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + dateTime + "')", dateTime, stationID, iRRR, iRRR, iRRR, iRRR, iRRR, iRRR, iRRR, iRRR, more1, more2, temp1, Hum1, temp2, Hum2, temp3, Hum3, temp4, Hum4, Yan1, Yan2, Yan3, Yan4);

            MySqlConnection conn = new MySqlConnection(cnString);
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public void RecordDataInDB_YYSTATION( string stationID,terminalDATADTO tmdto, string sTable_Name = "")
        {
            Math math1 = new Math();
            string cnString = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
           //温度、湿度、气压、风向、风速、雨量、uv、solar、co2、pm2.5、土湿、土温
            string sql = string.Format("  Insert into " + sTable_Name + " " + "(CP_Data_DateTime, CP_Data_StationID, CP_Data_Temp, CP_Data_Hum, CP_Data_WindSpeed, CP_Data_WindDirection, CP_Data_Bar, CP_Data_Solar, CP_Data_Rain, CP_Data_CO2, CP_Data_More1, CP_Data_More2,CP_Data_Tuwen,CP_Data_Tushi) select" + " '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}' from dual where not exists (select * from " + sTable_Name + "  where  CP_Data_StationID='" + stationID + "' and CP_Data_DateTime='" + tmdto.Tmdtodatetime + "')", tmdto.Tmdtodatetime, stationID, tmdto.TerminaldataTEMP, tmdto.TerminaldataHUM, tmdto.TerminaldataWINDS, tmdto.TerminaldataWINDD, tmdto.TerminaldataBAR, tmdto.TerminaldataSOLAR, tmdto.TerminaldataRAIN, tmdto.TerminaldataCO2, tmdto.TerminaldataUV, tmdto.TerminaldataPM25, tmdto.TerminaldataSOILTEMP, tmdto.TerminaldataSOILMOISTURE);

            MySqlConnection conn = new MySqlConnection(cnString);
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public void UpdataInDB(DateTime dateTime, string sID)
        {
            string cnString = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
            string sql = " update  CP_Station set Station_DataTime_Last='" + dateTime.ToString() + "' where Station_ID="+sID+" ";

            MySqlConnection conn = new MySqlConnection(cnString);
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            Status_Change(sID, true, dateTime, 2);
        }
        private void button_Download_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x27, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 2;
            bCommand_Flag = 0x27;
        }

        private void button_Download_All_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x25, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 2;
            bCommand_Flag = 0x25;
        }

        private void button_Jibencanshu_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x13);
        }

        private void button_Restart_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x10, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x10;
        }
        private bool Check_Optcode(byte[] bReceive, byte bCommand, string sMode)
        {
            bool boolReturn = false;
            order order1 = new order();
            byte bJiaoyan = order1.Check_Data(bReceive, 9, bReceive.Length - 2);
            if (bReceive[3] == bCommand)
            {
                if (bReceive[bReceive.Length - 1] == bJiaoyan)
                {
                    if (bReceive[6] == 0)
                    {
                       // MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "成功！" + ByteArrayToHexString(bReceive));
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "成功！");
                        boolReturn = true;

                    }
                    else if (bReceive[6] == 0x80)
                    {
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "失败！" + ByteArrayToHexString(bReceive));

                    }
                    else if (bReceive[6] == 0x81)
                    {
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "失败！错误代码:指令错误（指令码错误或设备不支持改指令）" + ByteArrayToHexString(bReceive));

                    }
                    else if (bReceive[6] == 0x82)
                    {
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "失败！错误代码:校验码错误");//+ ByteArrayToHexString(bReceive)

                    }
                    else if (bReceive[6] == 0x84)
                    {
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "失败！错误代码:参数错误" );

                    }


                    else
                    {
                        MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "失败！" );

                    }
                }
                else
                { MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "接收数据校验码错误" ); }
            }
            else
                MsgRecord(sMode + order1.DicCommand_Number[bCommand] + "命令错误！");
            return boolReturn;
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x11, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x11;
        }

        private void button_Sensor_Status_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x15);
        }
        private void Check_Sensor_Status(byte[] bReceive)
        {
            order order1 = new order();

            if (bReceive[3] == 0x15)
            {
                //comboBox_Sensor_1.Text = order1.DicONOFF_Number[bReceive[9]];
               // comboBox_Sensor_2.Text = order1.DicONOFF_Number[bReceive[10]];
               // comboBox_Sensor_3.Text = order1.DicONOFF_Number[bReceive[11]];
               // comboBox_Sensor_4.Text = order1.DicONOFF_Number[bReceive[12]];
               // comboBox_Sensor_5.Text = order1.DicONOFF_Number[bReceive[13]];
               // comboBox_Sensor_6.Text = order1.DicONOFF_Number[bReceive[14]];
               // comboBox_Sensor_7.Text = order1.DicONOFF_Number[bReceive[15]];
            }
        }
        private byte[] Set_Sensor_Status()
        {
            order order1 = new order();
            byte[] bReturn = new byte[6];
           // bReturn[0] = order1.DicONOFF_Number1[comboBox_Sensor_1.Text];
           // bReturn[1] = order1.DicONOFF_Number1[comboBox_Sensor_2.Text];
           // bReturn[2] = order1.DicONOFF_Number1[comboBox_Sensor_3.Text];
           /// bReturn[3] = order1.DicONOFF_Number1[comboBox_Sensor_4.Text];
//bReturn[4] = order1.DicONOFF_Number1[comboBox_Sensor_5.Text];
           // bReturn[5] = order1.DicONOFF_Number1[comboBox_Sensor_6.Text];

            return bReturn;


        }
        private void button_Save_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x17);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x19, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x19;
        }

        private void button_Status_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x1A);
        }

        private void button_Time_Get_Click(object sender, EventArgs e)
        {
           // order order1 = new order();
            Send_TCPIP(0x1D, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x1D;
           // Command_Get_Send_Receive1(order1.bCommand_Send_TimeGet);
        }

        private void button_JianGe_Get_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x21, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x21;
        }

        private void button_Auto_Send_Get_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x24);
        }

        private void button_Sensor_Status_Set_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x14);
        }

        private void button_Save_Set_Click(object sender, EventArgs e)
        {
            Command_Get_Send_Receive(0x16);
        }

        private void button_Data_Delete_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x18, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x18;
        }

        // private string FILE_NAME = "ErroLog.txt"; 
        public void WriteFile(string str, string FILE_NAME)
        {
            StreamWriter sr;
            try
            {
                if (File.Exists(FILE_NAME)) //如果文件存在,则创建File.AppendText对象 
                {
                    //if (File.ReadAllLines(FILE_NAME).Length < 1000)
                   // {
                        sr = File.AppendText(FILE_NAME);
                   // }
                   // else
                    //    sr = File.CreateText(FILE_NAME);
                }
                else    //如果文件不存在,则创建File.CreateText对象 
                {
                    sr = File.CreateText(FILE_NAME);
                }
                sr.WriteLine(str);
                sr.Close();
            }
            catch (Exception ex)
            { }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button_Auto_Send_Set_Click(object sender, EventArgs e)
        {
            Command_Set_Send_Receive(0x23);
        }
        private void Thread_Download()
        {
            order order1 = new order();
            for (int iRecord = 1; iRecord <= iData_Hour_Download; iRecord++)
            {

                try
                {
                    serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                    // for (int i = 0; i < 3; i++)
                    // {
                    // SendSerial(bCommand);
                    if (iRecord <= 255)
                    {
                        byte[] bDownload_Send = order1.Download_Data_Record(iRecord, bCommand_Download);
                        MsgRecord("下载第" + iRecord + "页" + ByteArrayToHexString(bDownload_Send));
                        serialPort1.Write(bDownload_Send, 0, bDownload_Send.Length);
                    }
                    else
                    {
                        byte[] bDownload_Send = order1.Download_Data_Record(iRecord - 255, bCommand_Download);
                        MsgRecord("下载第" + iRecord + "页" + ByteArrayToHexString(bDownload_Send));
                        serialPort1.Write(bDownload_Send, 0, bDownload_Send.Length);
                    }
                    System.Threading.Thread.Sleep(iTimeOut_Receive_Wait);
                    if (serialPort1.BytesToRead > 0)
                    {

                        int bytes = serialPort1.BytesToRead;
                        byte[] buffer = new byte[bytes];
                        serialPort1.Read(buffer, 0, bytes);
                      
                        bool boolSuccess = ReceiveDownload(buffer, iData_Hour_Download, iRecord,Get_Selected_Station_ID());
                      
                        if (iRecord == iData_Hour_Download && boolSuccess)
                        { MsgRecord("下载完成！"); }
                        if (!boolSuccess)
                        {
                            MsgRecord("数据有误,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                            System.Threading.Thread.Sleep(10000);
                            if (!boolSuccess && iRecord > 1)
                            {

                                Download_Fail_Try_Count -= 1;
                                string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                // DateTime dt = Convert.ToDateTime(sDatetime);
                                DateTime dt = DateTime.Now;
                                DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                Command_Get_Download_Send_Receive(0x27);
                                break;
                            }
                            else if (!boolSuccess && iRecord == 1)
                            {
                                Download_Fail_Try_Count -= 1;
                                Command_Get_Download_Send_Receive(bCommand_Download);
                                break;
                            }
                        }
                        else
                        {
                            Download_Fail_Try_Count = 2;
                         
                        }

                    }
                    else
                    {

                        // System.Threading.Thread.Sleep(1000);
                        serialPort1.DiscardInBuffer();
                        //  if (i == 2)
                        MsgRecord("端口连接超时,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                        if (Download_Fail_Try_Count > 0)
                        {
                            System.Threading.Thread.Sleep(10000);
                            if (iRecord > 1)
                            {
                                // Thread.CurrentThread.Abort();
                                Download_Fail_Try_Count -= 1;
                                string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                DateTime dt = DateTime.Now;
                                DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                Command_Get_Download_Send_Receive(0x27);
                                break;
                            }
                            else if (iRecord == 1)
                            {
                                // Thread.CurrentThread.Abort();
                                Download_Fail_Try_Count -= 1;
                                Command_Get_Download_Send_Receive(bCommand_Download);
                                break;
                            }
                        }
                        else
                        {
                            Download_Fail_Try_Count = 2;
                            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                            Thread.CurrentThread.Abort();
                        }
                        // MessageBox.Show("端口连接超时", "系统提示");
                        // return;
                    }

                    // }

                    if (iRecord == iData_Hour_Download)
                    {

                        serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        Thread.CurrentThread.Abort();
                       
                    }

                }
                catch (Exception ex)
                { MsgRecord(ex.ToString()); }
            }

        }

        private void button_Jishi_Get_Click_1(object sender, EventArgs e)
        {

            Send_TCPIP(0x1B, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            Thread.Sleep(100);
            Send_TCPIP(0x30, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            Thread.Sleep(2000);
            //get 02
            Send_TCPIP(0x31, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x1B;

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_Sensor_7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_Sensor_1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }










        public void Thread_Listener()
        {
            IPAddress ipAddress = IPAddress.Any;
            TcpListener tcpListener = new TcpListener(ipAddress, iServer_Com);
            tcpListener.Start();
           
            for (int i = 0; i < sStation_ID_GPRS.Count; i++)
            {
                Client client = new Client();
                client.sStation_ID = sStation_ID_GPRS[i];
                client.sStation_Mode = 0;
                client.sStation_Type =Convert.ToInt32( sStation_StationType[i]);
                //client.sStation_ID = "100000003";
                dic_Client.Add(sStation_ID_GPRS[i], client);
            }
           // dic_Client.Add("100000003", client);

            // List<Thread> l_ThreadServer=new List<Thread>();
            while (true)
            {
                /*****20170824新加新协议站点 start*********/
               
              

                //Thread.Sleep(100);

                /*********20170824新加新协议站点 end****************/
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                //  Tcp_Ser newClient = new Tcp_Ser(tcpClient);
                // dic_Client["100000003"].tcp_Connect = tcpClient;
                byte[] buffer = new byte[1024];

               // int iCount = -1;
               // iCount = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                iFlagThread = Register_Process_Socket(buffer, tcpClient.GetStream(), tcpClient);
                if (iFlagThread < 0)
                { }
                else 
                {
                    try
                    {
                       
                       // Thread t = new Thread(new ThreadStart(Thread_Start));
                       // t.IsBackground = true;
                        //t.Start();
                      
                        //threads[i] = new Thread(ts);
                        //threads[i].IsBackground = true;
                        //threads[i].Start();
                        if (!dic_thread.ContainsKey(sStation_ID_GPRS[iFlagThread]))
                        {
                            try
                            {
                                dic_Client[sStation_ID_GPRS[iFlagThread]].tcp_Connect = tcpClient;
                                ts = new ThreadStart(Thread_Start_Server);
                                dic_thread.Add(sStation_ID_GPRS[iFlagThread], new Thread(ts));
                                if (!dic_Net_Status.ContainsKey(sStation_ID_GPRS[iFlagThread]) && !dic_thread_check.ContainsKey(sStation_ID_GPRS[iFlagThread]))
                                {
                                    dic_Net_Status.Add(sStation_ID_GPRS[iFlagThread], -1);

                                    //  dic_Station_flag.Add(sStation_ID_GPRS[iFlagThread], iFlagThread);
                                    dic_thread_check.Add(sStation_ID_GPRS[iFlagThread], new Thread(new ThreadStart(thread_check_start_server)));

                                    dic_thread_check[sStation_ID_GPRS[iFlagThread]].IsBackground = true;
                                    dic_thread_check[sStation_ID_GPRS[iFlagThread]].Start();
                                    Thread.Sleep(100);
                                }
                                dic_thread[sStation_ID_GPRS[iFlagThread]].IsBackground = true;
                                dic_thread[sStation_ID_GPRS[iFlagThread]].Start();
                                Thread.Sleep(100);
                            }
                            catch (Exception ex)
                            { MsgRecord(sStation_ID_GPRS[iFlagThread] + ex.Message+"1"); }
                        }
                        else if (dic_thread.ContainsKey(sStation_ID_GPRS[iFlagThread]))
                        {
                            try
                            {
                               bool bthread_remove= dic_thread.Remove(sStation_ID_GPRS[iFlagThread]);
                                dic_Client[sStation_ID_GPRS[iFlagThread]].tcp_Connect = tcpClient;
                                ts = new ThreadStart(Thread_Start_Server);
                                dic_thread.Add(sStation_ID_GPRS[iFlagThread], new Thread(ts));
                                if (!dic_Net_Status.ContainsKey(sStation_ID_GPRS[iFlagThread]) && !dic_thread_check.ContainsKey(sStation_ID_GPRS[iFlagThread]))
                                {
                                    dic_Net_Status.Add(sStation_ID_GPRS[iFlagThread], -1);

                                    // dic_Station_flag.Add(sStation_ID_GPRS[iFlagThread], iFlagThread);
                                    dic_thread_check.Add(sStation_ID_GPRS[iFlagThread], new Thread(new ThreadStart(thread_check_start_server)));

                                    dic_thread_check[sStation_ID_GPRS[iFlagThread]].IsBackground = true;
                                    dic_thread_check[sStation_ID_GPRS[iFlagThread]].Start();
                                    Thread.Sleep(100);
                                }
                                dic_thread[sStation_ID_GPRS[iFlagThread]].IsBackground = true;
                                dic_thread[sStation_ID_GPRS[iFlagThread]].Start();
                                Thread.Sleep(100);
                            }
                            catch (Exception ex)
                            { MsgRecord(sStation_ID_GPRS[iFlagThread] + ex.Message + "1"); }
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgRecord(sStation_ID_GPRS[iFlagThread] + ex.Message+"2");

                    }
                }
               
            }
        }

        public void Thread_Start()
        {
            int iFlag = iFlagThread;
            bool iOnlineFlag_Add = false;
            //MsgRecord(iFlag.ToString());
            Client client_Station = new Client();
            string sID = sStation_ID[iFlag];
            //int iReTry_Delay = 1000;
            int iReTry_Flag = 0;
          
          
         

            while (true)
            {

          
                if (dic_Client[sID].tcp_Connect == null || !dic_Client[sID].tcp_Connect.Connected)
                {

                    try
                    {
                        dic_Client[sID].tcp_Connect = new TcpClient(sStation_IP[iFlag].Split(',')[0], Convert.ToInt32(sStation_IP[iFlag].Split(',')[1]));
                        dic_Client[sID].ntStream = dic_Client[sID].tcp_Connect.GetStream();
                        // dic_Net_Status[sID] = 1;
                        bThread_Status = true;
                        iOnlineFlag_Add = true;
                        if (iCount_Online < sStation_ID.Count)
                            iCount_Online += 1;
                        else
                            iCount_Online = sStation_ID.Count;
                        toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                        toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                        iReTry_Flag = 0;
                       // dic_Net_Status[sID] = 1;
                        Thread.Sleep(800);
                        Status_Change(sID, true, DateTime.Now, 1);
                        try
                        {
                            DicStation_status_online.Add(sID, true);
                        }
                        catch (Exception exx)
                        {
                            DicStation_status_online[sID] = true;
                        }
                         Send_TCPIP(0x1E, dic_Client[sID].ntStream, sID);
                         Send_TCPIP(0x1C, dic_Client[sID].ntStream, sID);
                    }
                    catch (Exception ex)
                    {
                        bThread_Status = false;
                        MsgRecord(sID + ex.Message);
                        if (iOnlineFlag_Add)
                        {
                            if (iCount_Online >= 1)
                                iCount_Online -= 1;
                            else
                                iCount_Online = 0;

                            toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                            toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                            iOnlineFlag_Add = false;
                        }
                        iReTry_Flag += 1;
                        if (iReTry_Flag > 9)
                        { Thread.Sleep(10000); }
                        else
                        { Thread.Sleep(1000); }
                        Status_Change(sID, false, DateTime.Now, 1);
                        try
                        {
                            DicStation_status_online.Add(sID, false);
                        }
                        catch (Exception exx)
                        {
                            DicStation_status_online[sID] = false;
                        }

                    }
            
                }

                else if (dic_Client[sID].tcp_Connect.Connected)
                {
                    if ( dic_Client[sID].tcp_Connect.Client.Available > 0)
                    {
                        Thread.Sleep(500);
                        byte[] buffer = new byte[1024];

                        int iCount = -1;
                        // try
                        // {
                        iCount = Tcp_Read(dic_Client[sID], buffer);
                        // dic_Client[sID].ntStream.Read(buffer, 0, buffer.Length);
                       if(iCount>0) dic_Net_Status[sID] = 1;
                        //  }
                        //  catch (Exception ex)
                        //  { MsgRecord(sID + "连接中断133"); }
                        if (iCount == -1)
                        { continue; }
                        else if (iCount > 0 && iCount < 10)
                        {
                            Thread.Sleep(1000);
                            iCount += Tcp_Read(dic_Client[sID], buffer);
                            MsgRecord(sID + "接收中");
                        }
                        else if (iCount > 10 && iCount < 96 && buffer[0] != 0x7E)
                        {

                            string clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                            clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                            MsgRecord(clientStr);
                            Thread.Sleep(600);
                            iCount += dic_Client[sID].ntStream.Read(buffer, iCount, buffer.Length - iCount);
                            while (clientStr.Contains("HDATA") && iCount < 96)
                            {

                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);
                                Thread.Sleep(200);
                                iCount += Tcp_Read(dic_Client[sID], buffer);
                            }
                            if (iCount >= 96)
                            {
                                clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);
                                try
                                {
                                    Data_Process_Socket(buffer, 96, "cp_sensor_data", dic_Client[sID].ntStream, sID);
                                }
                                catch (Exception ex)
                                { }
                                break;
                            }

                        }
                        else if (iCount == 0)
                        {
                            MsgRecord(sID + "连接中断");
                            if (iOnlineFlag_Add)
                            {
                                if (iCount_Online >= 1)
                                    iCount_Online -= 1;
                                else
                                    iCount_Online = 0;
                                toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                                toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                                iOnlineFlag_Add = false;
                            }
                            Status_Change(sID, false, DateTime.Now, 1);
                            try
                            {
                                DicStation_status_online.Add(sID, false);
                            }
                            catch (Exception exx)
                            {
                                DicStation_status_online[sID] = false;
                            }
                            try
                            {
                                //button5.Text = "连接";
                                dic_Client[sID].tcp_Connect.Close();
                                dic_Client[sID].ntStream.Close();

                            }
                            catch (Exception ex)
                            {
                                MsgRecord(ex.Message);
                                iReTry_Flag += 1;
                                if (iReTry_Flag > 9)
                                { Thread.Sleep(10000); }
                                else
                                { Thread.Sleep(1000); }
                            }
                        }
                        else
                        {
                           

                            if (buffer[0] == 0x7E)
                            {
                                ReceiveGet_Socket(buffer, bCommand_Flag, dic_Client[sID].ntStream, dic_Client[sID].sStation_ID);
                                iThread_Flag = 0;
                            }

                            else
                            {
                                string clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);
                               
                                Data_Process_Socket(buffer, 96, "cp_sensor_data", dic_Client[sID].ntStream, sID);
                            }

                        }

                        dic_Client[sID].ntStream.Flush();


                    }
                    else if ((dic_Client[sID].tcp_Connect.Client.Poll(1000, SelectMode.SelectRead) && (dic_Client[sID].tcp_Connect.Client.Available == 0)))
                    {
                        MsgRecord(sID + "连接中断");
                        if (iOnlineFlag_Add)
                        {
                            if (iCount_Online >= 1)
                                iCount_Online -= 1;
                            else
                                iCount_Online = 0;
                            toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                            toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                            iOnlineFlag_Add = false;
                        }
                        Status_Change(sID, false, DateTime.Now, 1);
                        try
                        {
                            DicStation_status_online.Add(sID, false);
                        }
                        catch (Exception exx)
                        {
                            DicStation_status_online[sID] = false;
                        }
                        try
                        {
                            //button5.Text = "连接";
                            dic_Client[sID].tcp_Connect.Close();
                            dic_Client[sID].ntStream.Close();

                        }
                        catch (Exception ex)
                        {
                            MsgRecord(ex.Message);
                            iReTry_Flag += 1;
                            if (iReTry_Flag > 9)
                            { Thread.Sleep(10000); }
                            else
                            { Thread.Sleep(1000); }
                        }
                    }

                }
            }



        }

        public void thread_check_start_server()
        {
            int iFlag = iFlagThread;

            string sID = sStation_ID_GPRS[iFlag];
            //dic_Net_Status[sID] = 1;
            int ifalg = 0;
            while (true)
            {
                switch (dic_Net_Status[sID])
                {
                    case 1:
                        ifalg = 0;
                        dic_Net_Status[sID] = 2;
                        Thread.Sleep(1000);
                        break;
                    case 0:
                        ifalg = 0;
                        dic_Net_Status[sID] = 3;
                        Thread.Sleep(1000);
                        break;
                    case 3:
                        if (ifalg > 299)
                        {
                            // dic_Net_Status[sID] = 4;
                            // ifalg = 0;
                            MsgRecord(sID + "超过5分钟无数据，断开连接");
                            try
                            {
                                ifalg = 0;
                                if (iCount_Online > 0)
                                    iCount_Online -= 1;
                                else

                                    iCount_Online = 0;
                                toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                                toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();


                                dic_thread[sID].Abort();
                               bool bRemove_thread= dic_thread.Remove(sID);

                                dic_Client[sID].tcp_Connect.Close();
                                bool bRemove_thread_check = dic_thread_check.Remove(sID);
                                Thread.CurrentThread.Abort();
                                
                            }
                            catch (Exception exx)
                            {
                               
                                Thread.Sleep(1000);
                            }

                        }
                        else if (ifalg <= 299)
                        {
                            ifalg += 1;
                            Thread.Sleep(1000);
                        }
                        break;
                    case 2:
                        ifalg += 1;
                        Thread.Sleep(1000);
                       // if (ifalg > 59)
                       // {
                        //    Send_TCPIP(0xdd, dic_Client[sID].ntStream, sID);
                        //    ifalg = 0;
                       // }

                        break;
                    case -1:
                        Thread.Sleep(1000);
                        break;


                }
            }
        }
        public void thread_check_start()
        {
            int iFlag = iFlagThread;
            
            string sID = sStation_ID[iFlag];
            //dic_Net_Status[sID] = 1;
            int ifalg = 0;
            while (true)
            {
                switch (dic_Net_Status[sID])
                {
                    case 1:
                        ifalg = 0;
                        dic_Net_Status[sID] = 2;
                        Thread.Sleep(1000);
                        break;
                    case 0:
                        ifalg = 0;
                        dic_Net_Status[sID] = 3;
                        Thread.Sleep(1000);
                        break;
                    case 3:
                        if (ifalg > 59)
                        {
                           // dic_Net_Status[sID] = 4;
                           // ifalg = 0;
                            try
                            {
                                ifalg = 0;
                                if (iCount_Online > 0)
                                    iCount_Online -= 1;
                                else

                                    iCount_Online = 0;
                                    toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                                    toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                                  
                                
                                dic_thread[sID].Abort();
                                dic_Client[sID].tcp_Connect.Close();
                                try
                                {
                                    dic_Net_Status[sID] = -1;
                                    //dic_Client[sID].tcp_Connect = null;
                                    iFlagThread = dic_Station_flag[sID];
                                    dic_thread[sID] = new Thread(new ThreadStart(Thread_Start));
                                    dic_thread[sID].IsBackground = true;
                                    dic_thread[sID].Start();
                                }
                                catch (Exception exx)
                                {



                                }
                            }
                            catch (Exception exx)
                            {
                                iFlagThread = dic_Station_flag[sID];
                                dic_thread[sID] = new Thread(new ThreadStart(Thread_Start));
                                dic_thread[sID].IsBackground = true;
                                dic_thread[sID].Start();
                                Thread.Sleep(1000);
                            }
                            
                        }
                        ifalg += 1;
                        Thread.Sleep(1000);
                        break;
                    case 2:
                         ifalg += 1;
                        Thread.Sleep(1000);
                        if (ifalg > 59)
                        {
                            Send_TCPIP(0xdd, dic_Client[sID].ntStream, sID);
                            ifalg = 0;
                        }
                       
                        break;
                    case -1:
                        Thread.Sleep(1000);
                        break;
                 

                }
            }
        }
        public void Thread_Start_Server()
        {
            int iFlag_data = 0;//判断规定时间内是否有数据参数，暂定为5分钟
            int iFlag = iFlagThread;
            bool iOnlineFlag_Add = false;
            MsgRecord(iFlag.ToString());
            Client client_Station = new Client();
            string sID = sStation_ID_GPRS[iFlag];
            DateTime dttem = DateTime.Now;
          
            int iReTry_Flag = 0;

            DateTime dtteeem = DateTime.Now;
         
            int itete = -1;
            try
            {

               
               // dic_Client[sID].tcp_Connect = new TcpClient(sStation_IP[iFlag].Split(',')[0], Convert.ToInt32(sStation_IP[iFlag].Split(',')[1]));
                dic_Client[sID].ntStream = dic_Client[sID].tcp_Connect.GetStream();

                if (dic_Client[sID].tcp_Connect != null || !dic_Client[sID].tcp_Connect.Connected)
                {
                    bThread_Status = true;
                    iOnlineFlag_Add = true;
                
                    if (iCount_Online <sStation_ID_GPRS.Count)
                        iCount_Online += 1;
                    else
                        iCount_Online = sStation_ID_GPRS.Count;
                    toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                    toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();
                
                    Status_Change(sID, true, DateTime.Now, 1);
                    // Command_Set_Send_Receive_TCPIP(0x1E, dic_Client[sID].ntStream);
                    Send_TCPIP(0x1E, dic_Client[sID].ntStream, sID);
                    Send_TCPIP(0x1C, dic_Client[sID].ntStream, sID);
                    Thread.Sleep(1000); 
                    Send_TCPIP(0x1B, dic_Client[sID].ntStream, sID);
                    iReTry_Flag = 0;
                    dic_Client[sID].ntStream.Flush();

                }
            }
            catch (Exception ex)
            {
                bThread_Status = false;
                MsgRecord(sID + ex.Message+"3");
             
                if (iOnlineFlag_Add)
                {
                    if (iCount_Online >= 1)
                        iCount_Online -= 1;
                    else
                        iCount_Online = 0;
                    toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                    toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();
                    iOnlineFlag_Add = false;
                }
                iReTry_Flag += 1;
                Status_Change(sID, false, DateTime.Now, 1);

                if (iReTry_Flag > 9)
                { Thread.Sleep(10000); }
                else
                { Thread.Sleep(1000); }
            }


            while (true)
            {
              

                try
               {
                    if (dic_Client[sID].tcp_Connect == null || !dic_Client[sID].tcp_Connect.Connected)
                    {
                      
                        dic_Client[sID].ntStream = dic_Client[sID].tcp_Connect.GetStream();
                        bThread_Status = true;
                        iOnlineFlag_Add = true;
                        if (iCount_Online <sStation_ID_GPRS.Count)
                            iCount_Online += 1;
                        else
                            iCount_Online = sStation_ID_GPRS.Count;
                        toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                        toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();
                        iReTry_Flag = 0;
                        Status_Change(sID, true, DateTime.Now, 1);
                   

                    }
                    else
                    {
                       

                        byte[] buffer = new byte[1024];

                        int iCount = -1;

                        /*****20170824新加新协议站点 start*********/
                        DateTime m_timme = DateTime.Now;

                        if (itete == -1)
                        {
                            if ((m_timme.Minute % 10) == 0)
                            {
                                //get 01
                                        Send_TCPIP(0x30, dic_Client[sID].ntStream, sID);
                                        Thread.Sleep(2000);
                                //get 02
                                        Send_TCPIP(0x31, dic_Client[sID].ntStream, sID);
                               

                                itete = 0;
                                dtteeem = DateTime.Now;
                            }
                            else
                            { }
                        }
                        else if (itete == 0)
                        {
                            if ((m_timme.Minute%10) == 0 && !(dtteeem.Minute.Equals(m_timme.Minute))) //判断是否指定时间(Invoke_Time)
                            {
                                
                                //get 01
                                Send_TCPIP(0x30, dic_Client[sID].ntStream, sID);
                                Thread.Sleep(2000);
                                //get 02
                                Send_TCPIP(0x31, dic_Client[sID].ntStream, sID);
                              

                                dtteeem = DateTime.Now;
                            }
                            else
                            { }

                        }

                        Thread.Sleep(100);

                            /*********20170824新加新协议站点 end****************/
                        iCount = dic_Client[sID].ntStream.Read(buffer, 0, buffer.Length);
                     

                        if (iCount == -1)
                        { 
                            continue; }
                        else if (iCount > 0 && iCount < 5 && !Encoding.GetEncoding("GB18030").GetString(buffer).Contains("HEART"))
                        {
                            dic_Net_Status[sID] = 0;
                            iFlag_data = 0;
                            Thread.Sleep(1000);
                            iCount += Tcp_Read(dic_Client[sID], buffer);
                            MsgRecord(sID + "接收中");
                        }
                        else if (iCount > 10 && iCount < 66 && buffer[0] != 0x7E )
                        {
                            dic_Net_Status[sID] = 0;
                            iFlag_data = 0;
                            string clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                            clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                            MsgRecord(clientStr);
                             /*****20170824新加新协议站点 start*********/
                             if (buffer[0] == 1 || buffer[0] == 2)
                            {
                                MsgRecord(sID+":"+ ByteArrayToHexString(buffer));
                                try
                                {
                                    client_Station.tmdtos[buffer[0] - 1] = terminaldataPROCESS.tergetprocess(buffer, buffer[0]);
                                    if (client_Station.terminalDATAgetflag == 0)
                                    {
                                        if (DateTime.Now.Minute == 0)
                                        {
                                            DateTime dtt = DateTime.Now;
                                            client_Station.tmdtos[buffer[0] - 1].Tmdtodatetime = dtt.AddSeconds(-dtt.Second);

                                        }
                                        else
                                        { client_Station.tmdtos[buffer[0] - 1].Tmdtodatetime = DateTime.Now; }

                                        client_Station.terminalDATAgetflag = 1;
                                    }
                                    else if (client_Station.terminalDATAgetflag == 1)
                                    {
                                        if (DateTime.Now.Minute == 0)
                                        {
                                            DateTime dtt = DateTime.Now;
                                            client_Station.tmdtos[buffer[0] - 1].Tmdtodatetime = dtt.AddSeconds(-dtt.Second);

                                        }
                                        else
                                        { client_Station.tmdtos[buffer[0] - 1].Tmdtodatetime = DateTime.Now; }
                                        //client_Station.terminalDATAgetflag = 2;
                                        client_Station.tmdto = terminaldataPROCESS.terdataADD(client_Station.tmdtos[0], client_Station.tmdtos[1]);
                                        if (client_Station.tmdto.Tmdtodatetime.Minute == 0)
                                        {
                                            RecordDataInDB_YYSTATION(sID, client_Station.tmdto, "cp_sensor_data_hour");
                                        }
                                        else
                                        { RecordDataInDB_YYSTATION(sID, client_Station.tmdto, "cp_sensor_data"); }
                                        client_Station.terminalDATAgetflag = 0;
                                    }
                                }
                                catch (Exception ex) { MsgRecord(ex.Message); }
                            }

                            /*********20170824新加新协议站点 end****************/
                            //Thread.Sleep(600);
                           // iCount += dic_Client[sID].ntStream.Read(buffer, iCount, buffer.Length - iCount);
                            while (clientStr.Contains("DATA") && iCount < 66)
                            {

                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);
                                Thread.Sleep(200);
                                iCount += Tcp_Read(dic_Client[sID], buffer);
                            }
                            if (iCount >= 66)
                            {
                                clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);
                                try
                                {
                                    Data_Process_Socket(buffer, 66, "cp_sensor_data", dic_Client[sID].ntStream, sID);
                                }
                                catch (Exception ex)
                                { }
                                break;
                            }

                        }
                        else if (iCount == 0)
                        {
                            MsgRecord(sID + "连接中断");
                            if (iOnlineFlag_Add)
                            {
                                if (iCount_Online >= 1)
                                    iCount_Online -= 1;
                                else
                                    iCount_Online = 0;
                                toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                                toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();
                                iOnlineFlag_Add = false;
                            }
                            Status_Change(sID, false, DateTime.Now, 1);
                            try
                            {
                                DicStation_status_online.Add(sID, false);
                            }
                            catch (Exception exx)
                            {
                                DicStation_status_online[sID] = false;
                            }
                            try
                            {
                              
                                dic_Client[sID].tcp_Connect.Close();
                                dic_Client[sID].ntStream.Close();
                                bool bThread_Remove = dic_thread.Remove(sID);
                                Thread.CurrentThread.Abort();
                                dic_thread_check[sID].Abort();
                                bool bThread_Remove_check = dic_thread_check.Remove(sID);

                            }
                            catch (Exception ex)
                            {
                                MsgRecord(ex.Message);
                                iReTry_Flag += 1;
                                if (iReTry_Flag > 9)
                                { Thread.Sleep(10000); }
                                else
                                { Thread.Sleep(1000); }
                            }
                        }
                        else
                        {
                            iFlag_data = 0;
                            dic_Net_Status[sID] = 0;
                            if (buffer[0] == 0x7E)
                            {
                                ReceiveGet_Socket(buffer, bCommand_Flag, dic_Client[sID].ntStream, dic_Client[sID].sStation_ID);
                                iThread_Flag = 0;
                            }
                           
                            else
                            {
                                string clientStr = Encoding.GetEncoding("GB18030").GetString(buffer);
                                clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                                MsgRecord(clientStr);

                                Data_Process_Socket(buffer, 66, "cp_sensor_data", dic_Client[sID].ntStream, sID);
                            }

                        }


                    }
               }
                catch (Exception ex)
                {
                    bThread_Status = false;
                    MsgRecord(sID + ex.Message+"4");
                   
                    if (iOnlineFlag_Add)
                    {
                        if (iCount_Online >= 1)
                            iCount_Online -= 1;
                        else
                            iCount_Online = 0;

                        toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                        toolStripStatusLabeloffline.Text = (sStation_ID_GPRS.Count - iCount_Online).ToString();
                        iOnlineFlag_Add = false;
                    }
                    iReTry_Flag += 1;
                  
                    dic_Client[sID].tcp_Connect.Close();
                    dic_Client[sID].ntStream.Close();
                    Status_Change(sID, false, DateTime.Now, 1);
                    Thread.CurrentThread.Abort();
                    dic_thread_check[sID].Abort();
                    bool bThread_Remove_check = dic_thread_check.Remove(sID);
                 
                }


            }



        }
        public void Data_Process_Socket(byte[] bReceive, int iLengthDataOne, string sTable_Name,NetworkStream NS_TCPClient,string sID)
        {
            order order1 = new order();
            Math math1 = new Math();
            string sReceiveData = order1.JishiData_Return_Pro(bReceive);
            sReceiveData = sReceiveData.Trim(new char[] { '\0', ' ' }).ToUpper();
            string clientStr = Encoding.GetEncoding("GB18030").GetString(bReceive);
            clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
            //string sReceiveData = "HDATA,110101003,09/05/18,15:46:15,+22.7,30,00,00.0,018,00.0,009,00.0,002,000.0,0000.0,000,008,\r\n";
            try
            {
                if (!string.IsNullOrEmpty(sReceiveData))
                {
                    if (sReceiveData.Contains("HEART"))
                    { MsgRecord("收到心跳信息:" + sReceiveData); }
                    if (sReceiveData.Contains("GPSDATA"))
                    { MsgRecord("收到注册信息:" + sReceiveData); }
                    sReceiveData.Replace("\r\n", ";");
                    string[] sRtemp = sReceiveData.Split(';');
                    for (int i = 0; i < sRtemp.Length; i++)
                    {
                        string[] sReceiveDataArray = sRtemp[i].Split(',');
                        if (sReceiveDataArray[0] == "HDATA")
                        {

                            // MsgRecord("整点数据信息未解析:" + sReceiveData);//正确回复应该为94长度，带回车换行96
                          
                            // MsgRecord("收到数据");
                            string sDataFormat = "站点ID:" + sReceiveDataArray[1] + ",";
                            double iSolar = 673.3;
                            if (sReceiveDataArray[1] == ConfigurationManager.AppSettings.GetKey(8))
                            { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(8)[0]); }
                            else if (sReceiveDataArray[1] == ConfigurationManager.AppSettings.GetKey(9))
                            { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(9)[0]); }
                            sDataFormat += "数据时间:" + sReceiveDataArray[2] + sReceiveDataArray[3] + ",";
                            string sDT = sReceiveDataArray[2] + " " + sReceiveDataArray[3];
                            sDataFormat += "温度:" + sReceiveDataArray[4] + "℃,";
                            if (sReceiveDataArray[5].Contains(":0"))
                            {
                                sReceiveDataArray[5] = "100";
                            }
                            sDataFormat += "湿度:" + sReceiveDataArray[5] + "%\n\r";
                            sDataFormat += "风速:" + sReceiveDataArray[7] + ",";
                            sDataFormat += "风向:" + sReceiveDataArray[8] + ",";
                            //sDataFormat += "2分平均风速:" + sReceiveDataArray[9] + ",";
                            sDataFormat += "土壤湿度:" + sReceiveDataArray[10] + "%,";
                           // char[] ctemp = sReceiveDataArray[10].ToCharArray();
                            double dsoilhum = 0;
                            try
                            {
                                dsoilhum = (double)(bReceive[61] * 256 + bReceive[60]) / 10;
                            }
                            catch (Exception ex)
                            { }
                            sReceiveDataArray[10] = dsoilhum.ToString();
                            sDataFormat += "太阳辐射:" + sReceiveDataArray[11] + "w/m2,";
                            sDataFormat += "土壤温度:" + sReceiveDataArray[12] + "℃\n\r";
                            double dsoiltemp = 0;
                            try
                            {
                                if (Convert.ToInt32(bReceive[70].ToString(), 16) < 0x02)

                                    dsoiltemp = (double)(Convert.ToInt32(bReceive[70].ToString(), 16) * 256 + bReceive[69]) / 10;
                                else
                                {
                                    dsoiltemp = -(double)( bReceive[69]) / 10;
                                }
                            }
                            catch (Exception ex)
                            { }
                           
                            sReceiveDataArray[12] = dsoiltemp.ToString();
                           // if()
                           // sDataFormat += "日雨量:" + Convert.ToDouble(sReceiveDataArray[13]) * 2 + "mm,";
                            //sDataFormat += "气压:" + sReceiveDataArray[14] + "hPa\n\r";
                            //sDataFormat += "连续无雨日:" + sReceiveDataArray[15] + ",";
                            sDataFormat += "状态码:" + sReceiveDataArray[16];
                            // MsgRecord("整点数据解析后:" + sDataFormat);
                            MsgRecord("收到数据:");//正确回复应该为94长度，带回车换行96
                            /*接收数据成功后，发送数据接收成功指令，判断是否整点，是-存入整点数据表，并校时;否-存入即时数据表*/
                            //serialPort1.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                            NS_TCPClient.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                            MsgRecord("发送成功解析数据确认！");
                            NS_TCPClient.Flush();

                            DateTime dt_Date = Convert.ToDateTime(sDT);
                            if (!sReceiveDataArray[1].Trim().Equals(sID.Trim()))
                            {
                                sReceiveDataArray[1] = sID;
                                // Send_TCPIP(0x1E, dic_Client[sID].ntStream, sID);
                            }
                            UpdataInDB(dt_Date, sReceiveDataArray[1]);
                            double drain1 = 0;
                            //if(Convert.ToDouble(sReceiveDataArray[13])
                            if (dt_Date.Minute == 0 && dt_Date.Second == 0)
                            {
                                // Command_Set_Send_Receive(0x1E);

                                RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[7]), Convert.ToInt32(sReceiveDataArray[8]), Convert.ToDouble(sReceiveDataArray[14]), Convert.ToDouble((Convert.ToDouble(sReceiveDataArray[11]) * 100 / iSolar).ToString("0.0")), 0, 0, Convert.ToDouble(sReceiveDataArray[12]), math1.soilmoist_Math(Convert.ToDouble(sReceiveDataArray[10])), "", "", "cp_sensor_data_hour");
                                // Send_TCPIP(0x1E, NClient_Stream);
                                // bCommand_Flag = 0x1E;
                                // Thread.Sleep(1000);
                                // Send_TCPIP(0x1E, NClient_Stream);
                                // bCommand_Flag = 0x1E;
                                // Command_Set_Send_Receive_TCPIP(0x1E, dic_Client[sID].ntStream);
                            }
                            else
                            {
                                RecordDataInDB(Convert.ToDateTime(sDT), sReceiveDataArray[1], Convert.ToDouble(sReceiveDataArray[4]), Convert.ToInt32(sReceiveDataArray[5]), Convert.ToDouble(sReceiveDataArray[7]), Convert.ToInt32(sReceiveDataArray[8]), Convert.ToDouble(sReceiveDataArray[14]), Convert.ToDouble((Convert.ToDouble(sReceiveDataArray[11]) * 100 / iSolar).ToString("0.0")), 0, 0, Convert.ToDouble(sReceiveDataArray[12]), math1.soilmoist_Math(Convert.ToDouble(sReceiveDataArray[10])), "", "", "cp_sensor_data");

                                //   Command_Set_Send_Receive_TCPIP(0x1E, dic_Client[sID].ntStream);
                                //   Form_data_jishi form_jishi=new Form_data_jishi();
                                //   form_jishi.Show();

                            }


                            //RecordDataInDB()
                        }
                        else if (sReceiveDataArray[0] == "SDATA")
                        {// MsgRecord("错误提示:数据有误" + "详细:" + sReceiveData); 
                            string sDataFormat = "站点ID:" + sReceiveDataArray[1] + ",";
                            double iSolar = 673.3;
                           // if (sReceiveDataArray[1] == ConfigurationManager.AppSettings.GetKey(8))
                           // { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(8)[0]); }
                           // else if (sReceiveDataArray[1] == ConfigurationManager.AppSettings.GetKey(9))
                           // { iSolar = Convert.ToDouble(ConfigurationManager.AppSettings.GetValues(9)[0]); }
                            sDataFormat += "数据时间:" + sReceiveDataArray[2] + sReceiveDataArray[3] + ",";
                            string sDT = sReceiveDataArray[2] + " " + sReceiveDataArray[3];
                            sDataFormat += "土温1:" + (double)(bReceive[34] * 256 + bReceive[35]) / 100 + "℃,";
                            sDataFormat += "水分1:" + (double)(bReceive[36] * 256 + bReceive[37]) / 100 + "%,";
                            sDataFormat += "盐分1:" + (double)(bReceive[38] * 256 + bReceive[39]) + "mg/L,";

                            sDataFormat += "土温2:" + (double)(bReceive[41] * 256 + bReceive[42]) / 100 + "℃,";
                            sDataFormat += "水分2:" + (double)(bReceive[43] * 256 + bReceive[44]) / 100 + "%,";
                            sDataFormat += "盐分2:" + (double)(bReceive[45] * 256 + bReceive[46]) + "mg/L,";

                            sDataFormat += "土温3:" + (double)(bReceive[48] * 256 + bReceive[49]) / 100 + "℃,";
                            sDataFormat += "水分3:" + (double)(bReceive[50] * 256 + bReceive[51]) / 100 + "%,";
                            sDataFormat += "盐分3:" + (double)(bReceive[52] * 256 + bReceive[53]) + "mg/L,";

                            sDataFormat += "土温4:" + (double)(bReceive[55] * 256 + bReceive[56]) / 100 + "℃,";
                            sDataFormat += "水分4:" + (double)(bReceive[57] * 256 + bReceive[58]) / 100 + "%,";
                            sDataFormat += "盐分4:" + (double)(bReceive[59] * 256 + bReceive[60]) + "mg/L,";
                           
                          
                          
                          //  sDataFormat += "状态码:" + sReceiveDataArray[16];
                            // MsgRecord("整点数据解析后:" + sDataFormat);
                            MsgRecord("收到数据:" + sDataFormat);//正确回复应该为94长度，带回车换行96
                            /*接收数据成功后，发送数据接收成功指令，判断是否整点，是-存入整点数据表，并校时;否-存入即时数据表*/
                            //serialPort1.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                            NS_TCPClient.Write(order1.bCommand_Send_Data_Receive_Success, 0, order1.bCommand_Send_Data_Receive_Success.Length);
                            MsgRecord("发送成功解析数据确认！");
                            NS_TCPClient.Flush();

                            DateTime dt_Date = Convert.ToDateTime(sDT);
                            if (!sReceiveDataArray[1].Trim().Equals(sID.Trim()))
                            {
                                sReceiveDataArray[1] = sID;
                                // Send_TCPIP(0x1E, dic_Client[sID].ntStream, sID);
                            }
                            UpdataInDB(dt_Date, sReceiveDataArray[1]);

                            if (dt_Date.Minute == 0 && dt_Date.Second == 0)
                            {
                                // Command_Set_Send_Receive(0x1E);

                                RecordDataInDB_SoilStation(Convert.ToDateTime(sDT), sReceiveDataArray[1],
                                    (double)(bReceive[34] * 256 + bReceive[35]) / 100,
                                    (double)(bReceive[36] * 256 + bReceive[37]) / 100,
                                    (double)(bReceive[38] * 256 + bReceive[39]),
                                    (double)(bReceive[41] * 256 + bReceive[42]) / 100,
                                    (double)(bReceive[43] * 256 + bReceive[44]) / 100,
                                    (double)(bReceive[45] * 256 + bReceive[46]),
                                    (double)(bReceive[48] * 256 + bReceive[49]) / 100,
                                    (double)(bReceive[50] * 256 + bReceive[51]) / 100,
                                    (double)(bReceive[52] * 256 + bReceive[53]), 
                                    (double)(bReceive[55] * 256 + bReceive[56]) / 100,
                                    (double)(bReceive[57] * 256 + bReceive[58]) / 10,//由于水分4出现故障，故*10测试
                                    (double)(bReceive[59] * 256 + bReceive[60]),
                                    "", "", "cp_sensor_data_hour");
                                // Send_TCPIP(0x1E, NClient_Stream);
                                // bCommand_Flag = 0x1E;
                                // Thread.Sleep(1000);
                                // Send_TCPIP(0x1E, NClient_Stream);
                                // bCommand_Flag = 0x1E;
                                // Command_Set_Send_Receive_TCPIP(0x1E, dic_Client[sID].ntStream);
                            }
                            else
                            {
                                RecordDataInDB_SoilStation(Convert.ToDateTime(sDT), sReceiveDataArray[1],
                                   (double)(bReceive[34] * 256 + bReceive[35]) / 100,
                                    (double)(bReceive[36] * 256 + bReceive[37]) / 100,
                                    (double)(bReceive[38] * 256 + bReceive[39]),
                                    (double)(bReceive[41] * 256 + bReceive[42]) / 100,
                                    (double)(bReceive[43] * 256 + bReceive[44]) / 100,
                                    (double)(bReceive[45] * 256 + bReceive[46]),
                                    (double)(bReceive[48] * 256 + bReceive[49]) / 100,
                                    (double)(bReceive[50] * 256 + bReceive[51]) / 100,
                                    (double)(bReceive[52] * 256 + bReceive[53]),
                                    (double)(bReceive[55] * 256 + bReceive[56]) / 100,
                                    (double)(bReceive[57] * 256 + bReceive[58]) / 10,
                                    (double)(bReceive[59] * 256 + bReceive[60]),
                                  "", "", "cp_sensor_data");

                                //   Command_Set_Send_Receive_TCPIP(0x1E, dic_Client[sID].ntStream);
                                //   Form_data_jishi form_jishi=new Form_data_jishi();
                                //   form_jishi.Show();

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgRecord("错误提示:数据有误"+ex.Message); }
        }
        public int Register_Process_Socket(byte[] bReceive,NetworkStream NS_TCPClient,TcpClient TCPClient)
        {
            //byte[] buffer = new byte[1024];
            int iCount = -1;
            int iReturn = -1;
            iCount = NS_TCPClient.Read(bReceive,0,bReceive.Length);

            if (iCount > 0 && iCount < 10)
            {
                Thread.Sleep(1000);
                iCount = NS_TCPClient.Read(bReceive, iCount, bReceive.Length - iCount);
            }
             if (iCount == 0)
            {
                MsgRecord( "非法连接，已中断");
                iReturn = -2;
                try
                {
                    //button5.Text = "连接";
                    TCPClient.Close();
                    NS_TCPClient.Close();

                }
                catch (Exception ex)
                {
                   // MsgRecord(ex.Message);
                    //iReTry_Flag += 1;
                    //if (iReTry_Flag > 9)
                    //{ Thread.Sleep(10000); }
                   // else
                   // { Thread.Sleep(1000); }
                }
            }
            else
            {
                
               
                
                    string clientStr = Encoding.GetEncoding("GB18030").GetString(bReceive);
                    clientStr = clientStr.Trim(new char[] { '\0', ' ' }).ToUpper();
                    if (clientStr.Contains("GPSDATA"))
                    {
                        string[] sReceive = clientStr.Split(',');
                        if (dic_Client.ContainsKey(sReceive[1]))
                        {
                          iReturn=  sStation_ID_GPRS.IndexOf(sReceive[1]);
                         
                        }
                    }
                     
                    // MsgRecord(clientStr);

                    //Data_Process_Socket(buffer, 96, "CP_Sensor_Data_Hour", dic_Client[sID].ntStream, sID);
                
                NS_TCPClient.Flush();
            }
             return iReturn;
        }
    
        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "连接")
            {
                try
                {
                    // button5.Click+=new EventHandler(button5_Click);
                  //  serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                  //  if (serialPort1.IsOpen)
                  //      serialPort1.Close();
                 
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["STATION_IPADDRESS"].Value = textBox_IPAddress.Text;

                    config.AppSettings.Settings["STATION_IPORT"].Value = textBox_Port.Text;
                   
                    config.Save(ConfigurationSaveMode.Modified);




                   // serialPort1.Open();
                    button5.Text = "停止";
                    try
                    {

                        ThreadStart ts = new ThreadStart(Thread_Start);
                        thread[DicStationFlag[this.textBox_ID.Text]] = new Thread(ts);
                        thread[DicStationFlag[this.textBox_ID.Text]].IsBackground = true;
                        thread[DicStationFlag[this.textBox_ID.Text]].Start();
                      //  bThread_Status = true;
                       
                    }
                    catch (Exception ex)
                    { }
                   
                   // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口被占用");
                    // textBox1.text+=
                }
            }
            else
            {
                try
                {
                    button5.Text = "连接";
                    Tcp_Connect[DicStationFlag[this.textBox_ID.Text]].Close();
                    thread[DicStationFlag[this.textBox_ID.Text]].Abort();

                }
                catch (Exception ex)
                { MsgRecord(ex.Message); }
                
            }
        }


        private void Command_Set_Send_Receive_TCPIP(byte bCommand, NetworkStream NS_Send)
        {
            try
            {
               // serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // for (int i = 0; i < 3; i++)
                //{
               // order order1 = new order();
                Send_TCPIP(bCommand, NS_Send, Get_Selected_Station_ID());
              
                Thread.Sleep(iTimeOut_Receive_Wait);
                byte[] buffer = new byte[10];
                int iCount_Receive_Data = NS_Send.Read(buffer, 0, buffer.Length);
                if (iCount_Receive_Data > 0)
                {

                   // int bytes = serialPort1.BytesToRead;
                   // byte[] buffer = new byte[bytes];
                   // serialPort1.Read(buffer, 0, bytes);
                    bool boolSuccess = ReceivePro(buffer, bCommand);

                    if (boolSuccess)
                    {
                       // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        return;
                    }
                }
                else
                {

                    // System.Threading.Thread.Sleep(2000);
                   // serialPort1.DiscardInBuffer();
                    MsgRecord("连接超时");
                    //  MessageBox.Show("端口连接超时", "系统提示");
                    // return;
                }

                // }

               
                //  System.Threading.Thread.Sleep(1000);


            }
            catch (Exception ex)
            { MsgRecord("网络有误"); }
        }
        private void Command_Get_Send_Receive_TCPIP(byte bCommand, NetworkStream NS_Send)
        {
            try
            {
                // serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // for (int i = 0; i < 3; i++)
                //{
                Send_TCPIP(bCommand, NS_Send, Get_Selected_Station_ID());

                System.Threading.Thread.Sleep(iTimeOut_Receive_Wait);
                byte[] buffer = new byte[1024];
                int iCount_Receive_Data = NS_Send.Read(buffer, 0, buffer.Length);
                if (iCount_Receive_Data > 0)
                {

                    // int bytes = serialPort1.BytesToRead;
                    // byte[] buffer = new byte[bytes];
                    // serialPort1.Read(buffer, 0, bytes);
                    bool boolSuccess = ReceiveGet(buffer, bCommand);

                    if (boolSuccess)
                    {
                        // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        return;
                    }
                }
                else
                {

                    // System.Threading.Thread.Sleep(2000);
                    // serialPort1.DiscardInBuffer();
                    MsgRecord("连接超时");
                    //  MessageBox.Show("端口连接超时", "系统提示");
                    // return;
                }

                // }


                //  System.Threading.Thread.Sleep(1000);


            }
            catch (Exception ex)
            { MsgRecord("网络有误"); }
        }
        public void Send_TCPIP(byte bFlag, NetworkStream NS_Send,string sID)
        {
            try
            {
              


                order order1 = new order();
                // string str = ByteArrayToHexString(data);
                switch (bFlag)
                {
                    case 0xdd:
                        // byte[] data_Restart = order1.bCommand_Send_Restart;
                        NS_Send.WriteByte(0xdd);
                        // MsgRecord(order1.DicCommand_Number[0x10] + ByteArrayToHexString(data_Restart));
                        MsgRecord(sID + ":心跳" );
                        break;
                    case 0x10:
                        byte[] data_Restart = order1.bCommand_Send_Restart;
                        NS_Send.Write(data_Restart, 0, data_Restart.Length);
                        // MsgRecord(order1.DicCommand_Number[0x10] + ByteArrayToHexString(data_Restart));
                        MsgRecord(sID+":"+order1.DicCommand_Number[0x10]);
                        break;
                    case 0x11:
                        byte[] data_Reset = order1.bCommand_Send_ReSet;
                        NS_Send.Write(data_Reset, 0, data_Reset.Length);
                        //MsgRecord(order1.DicCommand_Number[0x10] + ByteArrayToHexString(data_Reset));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x10]);
                        break;

                    case 0x12:

                        byte[] data_Data_JiBenCanshu_Get = order1.bCommand_Send_LanMsg;
                        NS_Send.Write(data_Data_JiBenCanshu_Get, 0, data_Data_JiBenCanshu_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x12] + ByteArrayToHexString(data_Data_JiBenCanshu_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x12]);
                        break;
                    case 0x13:
                        try
                        {
                            // byte[] data_Lat_Set = order1.Longitude_Latitude_Elevation_Set(Convert.ToDouble(textBox_Lan.Text), Convert.ToDouble(textBox_Lat.Text), Convert.ToDouble(textBox_Sealevel.Text));
                            // serialPort1.Write(data_Lat_Set, 0, data_Lat_Set.Length);
                            //MsgRecord(order1.DicCommand_Number[0x13] + ByteArrayToHexString(data_Lat_Set));
                            //MsgRecord(order1.DicCommand_Number[0x13]);
                        }
                        catch (Exception ex)
                        {
                            MsgRecord(ex.ToString());
                        }
                        break;
                    case 0x14:

                        byte[] data_Sensor_Status_Set = order1.Sensor_Status_Set(Set_Sensor_Status());
                        NS_Send.Write(data_Sensor_Status_Set, 0, data_Sensor_Status_Set.Length);
                        // MsgRecord(order1.DicCommand_Number[0x14] + ByteArrayToHexString(data_Sensor_Status_Set));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x14]);
                        break;
                    case 0x15:

                        byte[] data_Sensor_Status_Get = order1.bCommand_Send_SensorStatus;
                        NS_Send.Write(data_Sensor_Status_Get, 0, data_Sensor_Status_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x15] + ByteArrayToHexString(data_Sensor_Status_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x15]);
                        break;
                    case 0x16:

                       // byte[] data_Save_Set = order1.Save_Energy_Set(Convert.ToByte(comboBox_Save_Set.Text));
                       // NS_Send.Write(data_Save_Set, 0, data_Save_Set.Length);
                        // MsgRecord(order1.DicCommand_Number[0x14] + ByteArrayToHexString(data_Save_Set));
                       // MsgRecord(order1.DicCommand_Number[0x14]);
                        break;
                    case 0x17:
                        byte[] data_Save_Get = order1.bCommand_Send_SaveEnergy;
                        NS_Send.Write(data_Save_Get, 0, data_Save_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x17] + ByteArrayToHexString(data_Save_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x17]);
                        break;
                    case 0x18:

                        byte[] data_Data_Delete_Get = order1.bCommand_Send_DataDelete;
                        NS_Send.Write(data_Data_Delete_Get, 0, data_Data_Delete_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x18] + ByteArrayToHexString(data_Data_Delete_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x18]);
                        break;
                    case 0x19:
                        byte[] data_ID_Get = order1.bCommand_Send_IDGet;
                        NS_Send.Write(data_ID_Get, 0, data_ID_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x19] + ByteArrayToHexString(data_ID_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x19]);
                        break;
                    case 0x1A:
                        byte[] data_Status_Get = order1.bCommand_Send_SensorBatteryStatus;
                        NS_Send.Write(data_Status_Get, 0, data_Status_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1A] + ByteArrayToHexString(data_Status_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x1A]);
                        break;
                    case 0x1B:

                        byte[] data_Data_Jishi_Get = order1.bCommand_Send_DataGet;
                        NS_Send.Write(data_Data_Jishi_Get, 0, data_Data_Jishi_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1B] + ByteArrayToHexString(data_Data_Jishi_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x1B]);
                        break;
                    case 0x1C:

                       // string str_ID = textBox_ID.Text;
                        if (sID.Length == 9)
                        {
                            // order order1 = new order();
                            byte[] data_ID_Set = order1.ID_Set(sID);
                            NS_Send.Write(data_ID_Set, 0, data_ID_Set.Length);
                            MsgRecord(sID + ":" + "设置设备ID:" + sID);
                        }
                        else
                            MsgRecord(sID + ":" + "设置设备ID:" + "长度只支持9位");

                        break;
                    case 0x1D:
                        byte[] data_Time_Get = order1.bCommand_Send_TimeGet;
                        NS_Send.Write(data_Time_Get, 0, data_Time_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x1D] + ByteArrayToHexString(data_Time_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x1D]);
                        break;
                    case 0x1E:

                        //string str = sJianGe_Set(comboBox_send.Text.Trim());

                        byte[] data_Time_Set = order1.Time_Set(DateTime.Now);
                        NS_Send.Write(data_Time_Set, 0, data_Time_Set.Length);
                        MsgRecord(sID+"设置" + order1.DicCommand_Number[0x1E] + ByteArrayToHexString(data_Time_Set));
                        MsgRecord(order1.DicCommand_Number[0x1E]);
                       // textBox1.AppendText("455");
                        //MsgRecord("qqqqqqqqqqqqqqqqqq");
                        break;
                    case 0x21:
                        byte[] data_Jiange_Get = order1.bCommand_Send_JiangeGet;
                        NS_Send.Write(data_Jiange_Get, 0, data_Jiange_Get.Length);
                        // MsgRecord(order1.DicCommand_Number[0x21] + ByteArrayToHexString(data_Jiange_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x21]);
                        break;
                    case 0x22:

                        string str = sJianGe_Set(comboBox_send.Text.Trim());

                        byte[] data = order1.JianGe_Set(str);
                        NS_Send.Write(data, 0, data.Length);
                        //MsgRecord("设置" + order1.DicCommand_Number[0x22] + str + ByteArrayToHexString(data))
                        MsgRecord(sID + ":" + "设置-" + order1.DicCommand_Number[0x22]);
                        break;
                    //case 0x23:
                    //    if (comboBox_AutoMode.Text == "开")
                    //    {

                    //        byte[] data_Auto_Set = order1.bCommand_Send_AutoModeSet_Open;
                    //        serialPort1.Write(data_Auto_Set, 0, data_Auto_Set.Length);
                    //        //MsgRecord(order1.DicCommand_Number[0x23] + ByteArrayToHexString(data_Auto_Set));
                    //        MsgRecord(order1.DicCommand_Number[0x23]);
                    //    }
                    //    else if (comboBox_AutoMode.Text == "关")
                    //    {
                    //        byte[] data_Auto_Set = order1.bCommand_Send_AutoModeSet_Close;
                    //        serialPort1.Write(data_Auto_Set, 0, data_Auto_Set.Length);
                    //      //  MsgRecord(order1.DicCommand_Number[0x23] + ByteArrayToHexString(data_Auto_Set));
                    //        MsgRecord(order1.DicCommand_Number[0x23]);
                    //    }
                    //    break;
                    case 0x24:
                        byte[] data_Auto_Get = order1.bCommand_Send_AutoModeGet;
                        NS_Send.Write(data_Auto_Get, 0, data_Auto_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x24] + ByteArrayToHexString(data_Auto_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x24]);
                        break;
                    case 0x25:

                        byte[] data_Data_AllData_Get = order1.bCommand_Send_AllDataGet;
                        NS_Send.Write(data_Data_AllData_Get, 0, data_Data_AllData_Get.Length);
                        //MsgRecord(order1.DicCommand_Number[0x25] + ByteArrayToHexString(data_Data_AllData_Get));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x25]);
                        break;
                    case 0x27:

                        //string str = sJianGe_Set(comboBox_send.Text.Trim());

                        string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                        DateTime dt = Convert.ToDateTime(sDatetime);
                        byte[] data_Download_Send = order1.Data_Download_Modify(dt, DateTime.Now, 1);
                        NS_Send.Write(data_Download_Send, 0, data_Download_Send.Length);
                        //MsgRecord(order1.DicCommand_Number[0x27] + ByteArrayToHexString(data_Download_Send));
                        MsgRecord(sID + ":" + order1.DicCommand_Number[0x27]);
                        break;
                    //20170824新 start
                    case 0x30:

                        byte[] data_yy01_get = cpORDER.dataGETorder(0x01);
                        NS_Send.Write(data_yy01_get, 0, data_yy01_get.Length);
                        MsgRecord(sID + ":" + "01站点数据");
                        break;
                    case 0x31:

                        byte[] data_yy02_get = cpORDER.dataGETorder(0x02);
                        NS_Send.Write(data_yy02_get, 0, data_yy02_get.Length);
                        MsgRecord(sID + ":" + "02站点数据");
                        break;
                    //20170824新 end
                }
                if(bFlag!=0xdd)
                dic_Net_Status[sID]=0;
              
                // System.Threading.Thread.Sleep(100);
                //dic_thread_check[sID].IsBackground = true;
               // dic_thread_check[sID].Start();
               // Thread.Sleep(1000);


            }
            catch (Exception ex)
            { 
                MsgRecord(ex.Message);
                try
                {
                    dic_Net_Status[sID]=-1;
                    if (iCount_Online > 0)
                        iCount_Online -= 1;
                    else

                        iCount_Online = 0;
                    toolStripStatusLabel_ComNumber.Text = iCount_Online.ToString();
                    toolStripStatusLabeloffline.Text = (sStation_ID.Count - iCount_Online).ToString();
                    dic_thread[sID].Abort();
                    iFlagThread = dic_Station_flag[sID];
                    dic_thread[sID] = new Thread(new ThreadStart(Thread_Start));
                    dic_thread[sID].IsBackground = true;
                    dic_thread[sID].Start();
                }
                catch (Exception exx)
                {
                    //iFlagThread = dic_Station_flag[sID];
                   // dic_thread[sID] = new Thread(new ThreadStart(Thread_Start));
                   // dic_thread[sID].IsBackground = true;
                   // dic_thread[sID].Start();
                   // Thread.Sleep(1000);
                }
               
                //dic_Net_Status[sID] = 4;
            }
        }
        //20170824 new start
        public bool ReceiveGet_Socket20170824(byte[] bReceive, byte bCommand, NetworkStream NS_Send, string sID_Station)
        {
            return true;
        }
        //20170824 new start
        public bool ReceiveGet_Socket(byte[] bReceive, byte bCommand, NetworkStream NS_Send,string sID_Station)
        {
            bool boolReturn = false;
            order order1 = new order();
            if (bCommand == 0x1B)
            {
               // Data_Process(bReceive, 96, "CP_Sensor_Data");
                Data_Process_Socket(bReceive, 96, "CP_Sensor_Data_Hour", NS_Send, sID_Station);

            }
            else if (bCommand == 0x10)
            {
                //MsgRecord("设备重启:" + ByteArrayToHexString(bReceive));
                MsgRecord(sID_Station + ":" + "设备重启成功！");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x11)
            {
                // MsgRecord("设备初始化:" + ByteArrayToHexString(bReceive));
                MsgRecord(sID_Station + ":" + "设备初始化成功！");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x12)
            {
                MsgRecord(sID_Station + ":" + "设备基本参数信息:" + order1.Basic_Msg_Return_Pro(bReceive));
                boolReturn = Check_Optcode(bReceive, bCommand, "获取");
            }
            else if (bCommand == 0x13)
            {
                MsgRecord(sID_Station + ":" + "设备基本参数信息:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x14)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord(sID_Station + ":" + "设备传感器状态:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x15)
            {
                Check_Sensor_Status(bReceive);
                MsgRecord(sID_Station + ":" + "设备传感器状态:");
                boolReturn = Check_Optcode(bReceive, bCommand, "获取");
            }
            else if (bCommand == 0x16)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord(sID_Station + ":" + "设备省电模式:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x17)
            {

                if (bReceive[3] == 0x17)
                {

                    MsgRecord(sID_Station + ":" + "设备省电模式:" + bReceive[9] + "档");
                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x18)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord(sID_Station + ":" + "清空数据:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x19)
            {

                if (bReceive[3] == 0x19 && order1.Check_Data(bReceive, 9, 23) == bReceive[24])
                {
                    string sID = ""; string sGujian = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 9);


                    // System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sGujian += asciiEncoding.GetString(bReceive, 18, 6);

                    MsgRecord(sID_Station + ":" + "设备ID:" + sID + ",固件:" + sGujian);

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x1A)
            {

                if (bReceive[3] == 0x1A)
                {

                    MsgRecord(sID_Station + ":" + "设备传感器电池状态码:" + bReceive[9]);
                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x1D)
            {

                if (bReceive[3] == 0x1D && order1.Check_Data(bReceive, 9, 20) == bReceive[21])
                {
                    string sID = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 12);


                    MsgRecord(sID_Station + ":" + "设备时间:" + sID);

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x16)
            {
                //Check_Sensor_Status(bReceive);
                MsgRecord(sID_Station + ":" + "设备时间:");
                boolReturn = Check_Optcode(bReceive, bCommand, "设置");
            }
            else if (bCommand == 0x21)
            {

                if (bReceive[3] == 0x21 && order1.Check_Data(bReceive, 9, 12) == bReceive[13])
                {
                    string sID = "";

                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    sID += asciiEncoding.GetString(bReceive, 9, 4);


                    MsgRecord(sID_Station + ":" + "发送间隔" + sID + "分钟" + ByteArrayToHexString(bReceive));

                    boolReturn = Check_Optcode(bReceive, bCommand, "获取");
                }
            }
            else if (bCommand == 0x24)
            {
                if (bReceive[3] == 0x24 && (bReceive[9] == bReceive[10]))
                {
                    if (bReceive[9] == 0)
                    { MsgRecord(sID_Station+":"+"自动发送模式关"); }
                    else
                    { MsgRecord(sID_Station + ":" + "自动发送模式开"); }
                }
            }
            else if (bCommand == 0x27)
            {
                Command_Get_Download_Data_Bag_Socket(bReceive, bCommand, dic_Client[Get_Selected_Station_ID()].ntStream, sID_Station);
            }
            else if (bCommand == 0x25)
            {
                // Thread DownLoad_Data_Hour = new Thread(new ParameterizedThreadStart(Command_Get_Download_Data_Bag(bReceive, bCommand)));
                Command_Get_Download_Data_Bag_Socket(bReceive, bCommand, dic_Client[Get_Selected_Station_ID()].ntStream, sID_Station);
            }
            else
            {
                //if (bReceive[0] == 0x7E && bReceive[3] == bCommand)
                //{

            }
            return boolReturn;

        }

        public void Command_Get_Download_Data_Bag_Socket(byte[] bReceive, byte bCommand, NetworkStream NS_Send,string sID)
        {
            order order1 = new order();

            string sReceiveData = order1.DownLoadData_Return_Pro(bReceive);
            if (bReceive.Length >= 14)
            {
                if (sReceiveData.Substring(0, 10) == "4844415441")
                {
                    Data_Process(bReceive, 96, "CP_Sensor_Data_Hour");
                    sReceiveData = sReceiveData.Substring(192);
                    if (sReceiveData.Substring(0, 8) == "7E000025")
                    {
                        int dData_Length = Convert.ToInt32(sReceiveData.Substring(18, 8), 16);
                        MsgRecord("总共"+dData_Length.ToString()+"页");
                       // MsgRecord(ByteArrayToHexString(bReceive));
                        iData_Hour_Download = dData_Length;
                        bCommand_Download = bCommand;
                      //  serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                        if (iData_Hour_Download > 0)
                        {
                           // thread_Download = new Thread(new ThreadStart(Thread_Download));
                           // thread_Download.Start();
                            for (int iRecord = 1; iRecord <= iData_Hour_Download; iRecord++)
                            {

                                try
                                {
                                    //  serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                                    // for (int i = 0; i < 3; i++)
                                    // {
                                    // SendSerial(bCommand);
                                    if (iRecord <= 255)
                                    {
                                        byte[] bDownload_Send = order1.Download_Data_Record(iRecord, bCommand_Download);
                                        MsgRecord("下载第" + iRecord + "页");//+ ByteArrayToHexString(bDownload_Send)
                                        NS_Send.Write(bDownload_Send, 0, bDownload_Send.Length);
                                    }
                                    else
                                    {
                                        byte[] bDownload_Send = order1.Download_Data_Record(iRecord - 255, bCommand_Download);
                                        MsgRecord("下载第" + iRecord + "页");// + ByteArrayToHexString(bDownload_Send)
                                        NS_Send.Write(bDownload_Send, 0, bDownload_Send.Length);
                                    }
                                    Thread.Sleep(iTimeOut_Receive_Wait);
                                    byte[] buffer = new byte[255];
                                    int iData_Length = NS_Send.Read(buffer, 0, buffer.Length);
                                    if (iData_Length > 0)
                                    {

                                        bool boolSuccess = ReceiveDownload(buffer, iData_Hour_Download, iRecord,sID);

                                        if (iRecord == iData_Hour_Download && boolSuccess)
                                        { MsgRecord("下载完成！"); }
                                        if (!boolSuccess)
                                        {
                                            MsgRecord("数据有误,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                                            Thread.Sleep(10000);
                                            if (!boolSuccess && iRecord > 1)
                                            {

                                                Download_Fail_Try_Count -= 1;
                                                string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                                // DateTime dt = Convert.ToDateTime(sDatetime);
                                                DateTime dt = DateTime.Now;
                                                DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                                dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                                comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                                // Command_Get_Download_Send_Receive(0x27);
                                                Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                                iThread_Flag = 2;
                                                bCommand_Flag = 0x27;
                                                break;
                                            }
                                            else if (!boolSuccess && iRecord == 1)
                                            {
                                                Download_Fail_Try_Count -= 1;
                                                // Command_Get_Download_Send_Receive(bCommand_Download);
                                                Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                                iThread_Flag = 2;
                                                bCommand_Flag = 0x27;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Download_Fail_Try_Count = 2;

                                        }

                                    }
                                    else
                                    {

                                        // System.Threading.Thread.Sleep(1000);
                                      //  serialPort1.DiscardInBuffer();
                                        //  if (i == 2)
                                        MsgRecord("端口连接超时,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                                        if (Download_Fail_Try_Count > 0)
                                        {
                                            Thread.Sleep(10000);
                                            if (iRecord > 1)
                                            {
                                                // Thread.CurrentThread.Abort();
                                                Download_Fail_Try_Count -= 1;
                                                string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                                DateTime dt = DateTime.Now;
                                                DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                                dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                                comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                                Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                                iThread_Flag = 2;
                                                bCommand_Flag = 0x27;
                                                break;
                                            }
                                            else if (iRecord == 1)
                                            {
                                                // Thread.CurrentThread.Abort();
                                                Download_Fail_Try_Count -= 1;
                                                Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                                iThread_Flag = 2;
                                                bCommand_Flag = 0x27;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Download_Fail_Try_Count = 2;
                                            // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                                            // Thread.CurrentThread.Abort();
                                        }
                                        // MessageBox.Show("端口连接超时", "系统提示");
                                        // return;
                                    }

                                    // }

                                    if (iRecord == iData_Hour_Download)
                                    {

                                        // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                                        //  Thread.CurrentThread.Abort();

                                    }

                                }
                                catch (Exception ex)
                                { MsgRecord(ex.ToString()); }
                            }
                        }
                        else
                        {
                            MsgRecord("无数据下载！");
                        }
                    }
                }
                else if (bReceive[0] == 0x7E && bReceive[3] == bCommand)
                {
                    int dData_Length = Convert.ToInt32(sReceiveData.Substring(18, 8), 16);
                    MsgRecord(dData_Length.ToString());
                   // MsgRecord(ByteArrayToHexString(bReceive));
                    iData_Hour_Download = dData_Length;
                    bCommand_Download = bCommand;
                   // serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    if (iData_Hour_Download > 0)
                    {
                       // thread_Download = new Thread(new ThreadStart(Thread_Download));
                       // thread_Download.Start();

                        for (int iRecord = 1; iRecord <= iData_Hour_Download; iRecord++)
                        {

                            try
                            {
                              //  serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                                // for (int i = 0; i < 3; i++)
                                // {
                                // SendSerial(bCommand);
                                if (iRecord <= 255)
                                {
                                    byte[] bDownload_Send = order1.Download_Data_Record(iRecord, bCommand_Download);
                                    MsgRecord("下载第" + iRecord + "页");// + ByteArrayToHexString(bDownload_Send)
                                    NS_Send.Write(bDownload_Send, 0, bDownload_Send.Length);
                                }
                                else
                                {
                                    byte[] bDownload_Send = order1.Download_Data_Record(iRecord - 255, bCommand_Download);
                                    MsgRecord("下载第" + iRecord + "页");
                                    NS_Send.Write(bDownload_Send, 0, bDownload_Send.Length);
                                }
                                Thread.Sleep(iTimeOut_Receive_Wait);
                                byte[] buffer = new byte[255];
                             int iData_Length =  NS_Send.Read(buffer, 0, buffer.Length);
                             if (iData_Length > 0)
                                {

                                    bool boolSuccess = ReceiveDownload(buffer, iData_Hour_Download, iRecord, sID);

                                    if (iRecord == iData_Hour_Download && boolSuccess)
                                    { MsgRecord("下载完成！"); }
                                    if (!boolSuccess)
                                    {
                                        MsgRecord("数据有误,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                                       Thread.Sleep(10000);
                                        if (!boolSuccess && iRecord > 1)
                                        {

                                            Download_Fail_Try_Count -= 1;
                                            string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                            // DateTime dt = Convert.ToDateTime(sDatetime);
                                            DateTime dt = DateTime.Now;
                                            DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                            dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                            comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                           // Command_Get_Download_Send_Receive(0x27);
                                            Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                            iThread_Flag = 2;
                                            bCommand_Flag = 0x27;
                                            break;
                                        }
                                        else if (!boolSuccess && iRecord == 1)
                                        {
                                            Download_Fail_Try_Count -= 1;
                                           // Command_Get_Download_Send_Receive(bCommand_Download);
                                            Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                            iThread_Flag = 2;
                                            bCommand_Flag = 0x27;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Download_Fail_Try_Count = 2;

                                    }

                                }
                                else
                                {

                                    // System.Threading.Thread.Sleep(1000);
                                    serialPort1.DiscardInBuffer();
                                    //  if (i == 2)
                                    MsgRecord("端口连接超时,重试第" + (3 - Download_Fail_Try_Count) + "遍");
                                    if (Download_Fail_Try_Count > 0)
                                    {
                                     Thread.Sleep(10000);
                                        if (iRecord > 1)
                                        {
                                            // Thread.CurrentThread.Abort();
                                            Download_Fail_Try_Count -= 1;
                                            string sDatetime = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " " + comboBox_Datetime_Hour.Text + ":00:00";
                                            DateTime dt = DateTime.Now;
                                            DateTime dt_Record = dt.AddHours(0 - (iData_Hour_Download - iRecord) * 6);
                                            dateTimePicker1.Text = dt_Record.Year.ToString() + "/" + dt_Record.Month.ToString() + "/" + dt_Record.Day.ToString();
                                            comboBox_Datetime_Hour.Text = dt_Record.Hour.ToString();
                                            Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                            iThread_Flag = 2;
                                            bCommand_Flag = 0x27;
                                            break;
                                        }
                                        else if (iRecord == 1)
                                        {
                                            // Thread.CurrentThread.Abort();
                                            Download_Fail_Try_Count -= 1;
                                            Send_TCPIP(0x27, NS_Send, Get_Selected_Station_ID());
                                            iThread_Flag = 2;
                                            bCommand_Flag = 0x27;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Download_Fail_Try_Count = 2;
                                       // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                                       // Thread.CurrentThread.Abort();
                                    }
                                    // MessageBox.Show("端口连接超时", "系统提示");
                                    // return;
                                }

                                // }

                                if (iRecord == iData_Hour_Download)
                                {

                                   // serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                                  //  Thread.CurrentThread.Abort();

                                }

                            }
                            catch (Exception ex)
                            { MsgRecord(ex.ToString()); }
                        }

                    }
                    else
                    { MsgRecord("无数据下载！"); }
                }
                else
                {
                   // serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.TopMost = false;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_IPSet_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_STATION_Select_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.textBox_ID.Text = comboBox_STATION_Select.Text;
            iFlagThread = DicStationFlag[this.textBox_ID.Text];
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void CP_WeatherStation_Load(object sender, EventArgs e)
        {

        }

        private void set11_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x1E, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            bCommand_Flag = 0x1E;
        }
        private void DataGridViewIni(DataSet myDs)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            DataGridViewImageColumn dVLColumnImage = new DataGridViewImageColumn();
            

            dVLColumnImage.Name = "status_image";
            dVLColumnImage.Image = imageList1.Images[4];
            dataGridView1.Columns.Add(dVLColumnImage);
            DataView dv = myDs.Tables[0].DefaultView;
            dv.AllowNew = false;
           
            dataGridView1.DataSource = dv;
            dataGridView1.Columns["Station_DataTime_Last"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss "; 
            string[] sHeaderTextList = sHeaderText.Split(',');
           
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = sHeaderTextList[i];
            }
           // dataGridView1.Columns["Station_Type"].CellType = typeof(string);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Station_Type"].ValueType = typeof(string);
                dataGridView1.Rows[i].Cells["Station_Mode"].ValueType = typeof(string);
                try
                {
                   
                    dataGridView1.Rows[i].Cells["Station_Type"].Value = Dic_Station_Type[dataGridView1.Rows[i].Cells["Station_Type"].Value.ToString()];
                   
                }
                catch (Exception ex)
                {
                   
                    dataGridView1.Rows[i].Cells["Station_Type"].Value = "---"; }

                try
                {

                    dataGridView1.Rows[i].Cells["Station_Mode"].Value = Dic_Station_Mode[dataGridView1.Rows[i].Cells["Station_Mode"].Value.ToString()];
                }
                catch (Exception ex)
                { 
                    dataGridView1.Rows[i].Cells["Station_Mode"].Value = "---"; }
               
            }
            textBox_ID.Text = Get_Selected_Station_ID();
        }
        private void Status_Change(string sID,bool bStatus, DateTime dt, int istatus)
        {
           // BindingSource bindingsource = new BindingSource();
           // int iRows_Flag = bindingsource.Find(sID, dataGridView1);
           // try
           // {
                switch (istatus)
                {
                    case 1:
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells["Station_ID"].Value.ToString().Trim() == sID.Trim())
                            {
                                if (bStatus)
                                {
                                    dataGridView1.Rows[i].Cells["status_image"].Value = imageList1.Images[3];
                                    //break;
                                }
                                else
                                {
                                    dataGridView1.Rows[i].Cells["status_image"].Value = imageList1.Images[4];
                                    //break;
                                }
                                dataGridView1.Update();
                                return;
                            }
                        }
                        break;
                    case 2:
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells["Station_ID"].Value.ToString() == sID)
                            {

                                dataGridView1.Rows[i].Cells["Station_DataTime_Last"].Value = dt;

                                return;
                            }
                        }
                        break;
                }
            //}
           // catch (Exception ex)
           // {
           //     MsgRecord("cuowu");
          //  }
        }
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    textBox_ID.Text =  Get_Selected_Station_ID();
                   // contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        } 
        private void Set1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox_ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox_IPAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox_Port_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }


        static DataSet ExecuteQueryForDataSet(int iStation_Mode)
        {
            DataSet ds = new DataSet();
            string conStr = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ToString();
            using (MySqlConnection sqlc = new MySqlConnection(conStr))
            {
                MySqlCommand cmd = new MySqlCommand(@"select Station_IP as IP,Station_ID as sID,Station_Type as sType from CP_Station where Station_Mode = '" + iStation_Mode + "' order by Station_ID asc", sqlc);

                MySqlDataAdapter sqld = new MySqlDataAdapter(cmd);
                sqld.Fill(ds);
                return ds;
            }
        }

        static List<string> GetIP(DataSet ds, string columnName)
        {
            DataTable dt = ds.Tables[0];
            List<string> f = new List<string>();
            //List<string> l = new List<string>();
            DataRow[] dr = dt.Select();
            for (int i = 0; i < dr.Length; i++)
            {
                f.Add((dr[i][columnName]).ToString());
                //l.Add((dr[i]["LastName"]).ToString());
            }
            return f;
        }

        static Dictionary<string, string> GetValues(List<string> list1, List<string> list2)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (list1.Count == list2.Count)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    dic.Add(list1[i], list2[i]);
                }
            }
            //DataTable dt = ds.Tables[0];
            //List<string> f = new List<string>();
            ////List<string> l = new List<string>();
            //DataRow[] dr = dt.Select();
            //for (int i = 0; i < dr.Length; i++)
            //{
            //    f.Add((dr[i][columnName]).ToString());
            //    //l.Add((dr[i]["LastName"]).ToString());
            //}
            //for (int i = 0; i < f.Count; i++)
            //{
            //    dic.Add(i, f[i].Split(new string {","})[]);
            //}
            return dic;
        }
       
        /**************获取选中站点ID 开始*******************/
        public string Get_Selected_Station_ID()
        {
            string sID_Return = "";
            try
            {
                sID_Return = dataGridView1.SelectedRows[0].Cells["Station_ID"].Value.ToString();
            }
            catch (Exception ex) { }
            return sID_Return;
        }

       

        private void toolStripTextBox1_Click_1(object sender, EventArgs e)
        {

        }

      

        private void ToolStripMenuItem_data_zhengdian_Click(object sender, EventArgs e)
        {
            Form_data_hour form111 = new Form_data_hour();
            form111.Show();
        }

        private void ToolStripMenuItem_data_zhaoce_Click(object sender, EventArgs e)
        {
            Form_data_zhaoce form111 = new Form_data_zhaoce();
            form111.Show();
        }

        private void ToolStripMenuItem_set21_Click(object sender, EventArgs e)
        {
            Form_station_config form1 = new Form_station_config();
           
            //form_station form111 = new Form_data_zhaoce();
            form1.Show();
        }

        private void ToolStripMenuItem_set16_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x1B, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            iThread_Flag = 1;
            bCommand_Flag = 0x1B;
        }

        private void ToolStripMenuItem_set13_Click(object sender, EventArgs e)
        {
            Send_TCPIP(0x1C, dic_Client[Get_Selected_Station_ID()].ntStream, Get_Selected_Station_ID());
            bCommand_Flag = 0x1C;
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    if(DicStation_status_online.ContainsKey(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString()))
                    Status_Change(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString(), DicStation_status_online[(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString())], DateTime.Now, 1);
                }

            }
            catch (Exception ex)
            {

            }
            
        }

        private void Status_Image_Refresh()
        {
            try
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    if (DicStation_status_online.ContainsKey(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString()))
                        Status_Change(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString(), DicStation_status_online[(dataGridView1.Rows[j].Cells["Station_ID"].Value.ToString())], DateTime.Now, 1);
                }

            }
            catch (Exception ex)
            {

            }
        }

      
        /**************获取选中站点ID 结束*******************/

        public int Tcp_Read(Client client, byte[] buffer_tem)
        {
            int icount_tem=-1;
            try
            {
                if(client.ntStream.DataAvailable)
               icount_tem= client.ntStream.Read(buffer_tem, 0, buffer_tem.Length);
            }
            catch (Exception ex)
            {
                try
                {
                    client.tcp_Connect.Close();
                    client.ntStream.Close();
                }
                catch (Exception ex1)
                { }
            }
      return icount_tem;
        }

        private void ToolStripMenuItem_data_zhengdian_soil_Click(object sender, EventArgs e)
        {
            Form_data_hour form111 = new Form_data_hour();
            form111.Show();
        }

        private void ToolStripMenuItem_data_jishi_soil_Click(object sender, EventArgs e)
        {
            Form_data_zhaoce_soil form111 = new Form_data_zhaoce_soil();
            form111.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_data_hour_soil form111 = new Form_data_hour_soil();
            form111.Show();
        }


        public void ServiceStart()
        {

            Thread TaskThread = new Thread(new ThreadStart(ThreadInvoke));
            TaskThread.IsBackground = true;
            TaskThread.Start();

        }

        public void ThreadInvoke()
        {

            DateTime dtteeem = DateTime.Now;
           // DateTime dttem1 = new DateTime(dttem.Year, dttem.Month, dttem.Day, 0, 0, 0);

            int itete = -1;
            while (true)
            {

                DateTime m_timme = DateTime.Now;

                if (itete == -1)
                {
                    if ((m_timme.Minute % 2) == 0)
                    {
                        //get 01
                        MsgRecord("testttt");
                        Thread.Sleep(500);
                        //get 02

                        itete = 0;
                        dtteeem = DateTime.Now;
                    }
                    else
                    { }
                }
                else if (itete == 0)
                {
                    if ((m_timme.Minute % 2) == 0 && !(dtteeem.Minute.Equals(m_timme.Minute))) //判断是否指定时间(Invoke_Time)
                    {
                        //get 01
                        MsgRecord("testttt");
                        Thread.Sleep(500);
                        //get 02

                        dtteeem = DateTime.Now;
                    }
                    else
                    { }

                }
                //DateTime m_time = DateTime.Now;
                //if (iiii == -1)
                //{
                //    if (m_time.Minute == 0)
                //    {
                //        //get 01
                //      
                //        Send_TCPIP(0x30, dic_Client[sID].ntStream, sID);
                //        Thread.Sleep(3000);
                //        //get 02
                //        Send_TCPIP(0x31, dic_Client[sID].ntStream, sID);
                //        iiii = 0;
                //        dttem = DateTime.Now;
                //    }
                //}
                //else
                //{
                //    if (m_time.Minute == 0 && !dttem.Hour.Equals(m_time.Hour)) //判断是否指定时间(Invoke_Time)
                //    {
                //        //get 01
                //        Send_TCPIP(0x30, dic_Client[sID].ntStream, sID);
                //        Thread.Sleep(3000);
                //        //get 02
                //        Send_TCPIP(0x31, dic_Client[sID].ntStream, sID);
                //        dttem = DateTime.Now;
                //    }

                //}

                Thread.Sleep(100);

            }



        }


    }
}
