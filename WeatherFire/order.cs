using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace CP_WSMonitor
{
   public class order
    {
        public byte[] bCommand_Send_Restart = { 0x7E, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//重启
        public byte[] bCommand_Send_ReSet = { 0x7E, 00, 00, 0x11, 00, 00, 00, 00, 00, 00 };//复位
        public byte[] bCommand_Send_LanMsg = { 0x7E, 00, 00, 0x12, 00, 00, 00, 00, 00, 00 };//获取经纬度、海拔信息
        public byte[] bCommand_Send_SensorStatus = { 0x7E, 00, 00, 0x15, 00, 00, 00, 00, 00, 00 };//获取传感器状态，开或关
        public byte[] bCommand_Send_SaveEnergy = { 0x7E, 00, 00, 0x17, 00, 00, 00, 00, 00, 00 };//获取省电模式状态
        public byte[] bCommand_Send_DataDelete = { 0x7E, 00, 00, 0x18, 00, 00, 00, 00, 00, 00 };//清楚采集器所有已存储数据
        public byte[] bCommand_Send_IDGet = { 0x7E, 00, 00, 0x19, 00, 00, 00, 00, 00, 00 };//获取设备ID
        public byte[] bCommand_Send_SensorBatteryStatus = { 0x7E, 00, 00, 0x1A, 00, 00, 00, 00, 00, 00 };//获取传感器电池状态
        public byte[] bCommand_Send_DataGet = { 0x7E, 00, 00, 0x1B, 00, 00, 00, 00, 00, 00 };//采集一条数据
        public byte[] bCommand_Send_TimeGet = { 0x7E, 00, 00, 0x1D, 00, 00, 00, 00, 00, 00 };//获取采集器时间
        public byte[] bCommand_Send_JiangeGet = { 0x7E, 00, 00, 0x21, 00, 00, 00, 00, 00, 00 };//获取采集间隔
        public byte[] bCommand_Send_AutoModeSet_Open = { 0x7E, 00, 00, 0x23, 00, 00, 00, 00, 01, 01,01 };//自动发送模式查看
        public byte[] bCommand_Send_AutoModeSet_Close = { 0x7E, 00, 00, 0x23, 00, 00, 00, 00, 01, 00,00 };//自动发送模式查看
        public byte[] bCommand_Send_AutoModeGet = { 0x7E, 00, 00, 0x24, 00, 00, 00, 00, 00, 00 };//自动发送模式查看
        public byte[] bCommand_Send_AllDataGet = { 0x7E, 00, 00, 0x25, 00, 00, 00, 00, 01, 01,01 };//获取所有历史数据
        public byte[] bCommand_Send_Data_Receive_Success = { 0x7E, 00, 00, 00, 00, 00, 00, 00, 00, 00 };//如解析成功数据，发送
        public byte[] Download_Data_Record(int iNumber,byte bCommand)
        {
            byte[] byteReturn = { 0x7E, 00, 00, bCommand, 00, (byte)iNumber, 00, 00, 01, 06, 06 };
            return byteReturn;

        }
        //public byte[] Download_Data_Record(int iNumber)
        //{
        //    byte[] byteReturn = { 0x7E, 00, 00, 0x27, 00,(byte)iNumber, 00, 00, 01, 06, 06 };
        //    return byteReturn;
 
        //}
        public Dictionary<byte, string> DicCommand_Number = new Dictionary<byte, string>
        { {0x10,"设备重启"},{0x11,"设备初始化"},
          {0x12,"设备基本参数"}, {0x13,"设备基本参数设置"}, 
          {0x14,"传感器状态设置"},{0x15,"传感器状态"},
          {0x16,"省电模式设置"},{0x17,"省电模式"},
          {0x18,"清空数据"},{0x19,"设备基本信息"},
          {0x1A,"设备传感器电池状态"}, {0x1B,"及时数据"},
          {0x1C,"设备ID"},{0x1D,"设备时间获取"},
          {0x1E,"设备时间"},
          {0x21,"发送间隔读取"},{0x22,"发送间隔"},
          {0x23,"自动发送模式开关设置"},{0x24,"自动发送模式开关获取"}, {0x25,"下载全部数据"},
          {0x27,"下载数据"}
        };

        public Dictionary<byte, string> DicONOFF_Number = new Dictionary<byte, string>
        { {0x00,"关"},{0x01,"开"}
        };
        public Dictionary<string, byte> DicONOFF_Number1 = new Dictionary<string, byte>
        { {"关",0x00},{"开",0x01}
        };
       
        public byte[] Longitude_Latitude_Elevation_Set(double fLongi, double fLati, double fEleva)
        {
            byte[] byteReturn=new byte[24];
            byte[] bLongi=DataForCh(fLongi);
            byte[] bLati=DataForCh(fLati);
            byte[] bEleva=DataForChSeaLevel(fEleva);
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x13;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x0E;
            byteReturn[9] = ZFPanDuan(fLongi);
            byteReturn[10] = bLongi[0];
            byteReturn[11] = bLongi[1];
            byteReturn[12] = bLongi[2];
            byteReturn[13] = ZFPanDuan(fLati);
            byteReturn[14] = bLati[0];
            byteReturn[15] = bLati[1];
            byteReturn[16] = bLati[2];
            byteReturn[17] = ZFPanDuan(fEleva);
            byteReturn[18] = bEleva[0];
            byteReturn[19] = bEleva[1];
            byteReturn[20] = bEleva[2];
            byteReturn[21] = bEleva[3];
            byteReturn[22] = bEleva[4];
            byteReturn[23] =(byte)( byteReturn[9]^byteReturn[10]^byteReturn[11]^byteReturn[12]^byteReturn[13]^byteReturn[14]^byteReturn[15]^byteReturn[16]^byteReturn[17]^byteReturn[18]^byteReturn[19]^byteReturn[20]^byteReturn[21]^byteReturn[22]);

            return byteReturn;
        }
        public byte[] Sensor_Status_Set(byte[] boolSensor_Status)
        {
            byte[] byteReturn = new byte[16];
           
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x14;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x06;
            byteReturn[9] = boolSensor_Status[0];
            byteReturn[10] = boolSensor_Status[1];
            byteReturn[11] = boolSensor_Status[2];
            byteReturn[12] = boolSensor_Status[3];
            byteReturn[13] = boolSensor_Status[4];
            byteReturn[14] = boolSensor_Status[5];
            byteReturn[15] = Check_Data(boolSensor_Status, 0, 5);
           

            return byteReturn;
        }
        public byte[] Save_Energy_Set(byte bEnergy_Status)
        {
            byte[] byteReturn = new byte[11];

            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x16;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x01;
            byteReturn[9] = bEnergy_Status;
            byteReturn[10] = bEnergy_Status;
           


            return byteReturn;
        }
        public byte[] ID_Set(string sID)
        {
            byte[] byteReturn = new byte[19];
            byte[] array = System.Text.Encoding.ASCII.GetBytes(sID); 
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x1C;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x09;
            byteReturn[9] = array[0];
            byteReturn[10] = array[1];
            byteReturn[11] = array[2];
            byteReturn[12] = array[3];
            byteReturn[13] = array[4];
            byteReturn[14] = array[5];
            byteReturn[15] = array[6];
            byteReturn[16] = array[7];
            byteReturn[17] = array[8];
            byteReturn[18] = (byte)(array[0] ^ array[1] ^ array[2] ^ array[3] ^ array[4] ^ array[5] ^ array[6] ^ array[7] ^ array[8]);
          



            return byteReturn;
        }
        public byte[] Time_Set(DateTime dt)
        {
            string sTime = "";
            sTime+=dt.Year.ToString().Substring(2, 2);
            sTime += sADDzero(dt.Month);
            sTime += sADDzero(dt.Day);
            sTime += sADDzero(dt.Hour);
            sTime += sADDzero(dt.Minute);
            sTime += sADDzero(dt.Second);
            byte[] byteReturn = new byte[22];
            byte[] array = System.Text.Encoding.ASCII.GetBytes(sTime);
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x1E;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x0C;
            byteReturn[9] = array[0];
            byteReturn[10] = array[1];
            byteReturn[11] = array[2];
            byteReturn[12] = array[3];
            byteReturn[13] = array[4];
            byteReturn[14] = array[5];
            byteReturn[15] = array[6];
            byteReturn[16] = array[7];
            byteReturn[17] = array[8];
            byteReturn[18] = array[9];
            byteReturn[19] = array[10];
            byteReturn[20] = array[11];
            byteReturn[21] = (byte)(array[0] ^ array[1] ^ array[2] ^ array[3] ^ array[4] ^ array[5] ^ array[6] ^ array[7] ^ array[8] ^ array[9] ^ array[10] ^ array[11]);




            return byteReturn;
        }
        public byte[] JianGe_Set(string sTimeSet)
        {
            sTimeSet=sTimeSet.PadLeft(4,'0');
            byte[] byteReturn = new byte[14];
            byte[] array = { 0x00, 0x00, 0x00, 0x00 };
            byte[] array1 = System.Text.Encoding.ASCII.GetBytes(sTimeSet);
            //byte bTest=0x02;
            for (int i = 0; i < array1.Length; i++)
            { array[3 - i] = array1[array1.Length - i-1]; }
            //for (int i = 0; i < array.Length; i++)
            //{if(i<array.Length- array1.Length)
            //    array[i] = 0x00;
            //else
            //    array[i] = array1[i - array1.Length];
            //}
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x22;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x04;
            byteReturn[9] = array[0];
            byteReturn[10] = array[1];
            byteReturn[11] = array[2];
            byteReturn[12] = array[3];
            byteReturn[13] = (byte)(array[0] ^ array[1]^array[2] ^ array[3]);
           



            return byteReturn;
        }
        public byte[] AutoMode_Set(byte bMode)
        {
            byte[] byteReturn = new byte[11];
          
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x23;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x01;
            byteReturn[9] = bMode;
            byteReturn[10] = 0x00;
         




            return byteReturn;
        }
        public byte[] Data_Download(DateTime dtStart,DateTime dtEnd)
        {
            string sTimeStart = "";
            string sTimeEnd = "";
            sTimeStart += dtStart.Year.ToString().Substring(2, 2);
            sTimeStart += sADDzero(dtStart.Month);
            sTimeStart += sADDzero(dtStart.Day);
            sTimeStart += sADDzero(dtStart.Hour);
            sTimeStart += sADDzero(dtStart.Minute);
            sTimeStart += sADDzero(dtStart.Second);

            sTimeEnd += dtEnd.Year.ToString().Substring(2, 2);
            sTimeEnd += sADDzero(dtEnd.Month);
            sTimeEnd += sADDzero(dtEnd.Day);
            sTimeEnd += sADDzero(dtEnd.Hour);
            sTimeEnd += sADDzero(dtEnd.Minute);
            sTimeEnd += sADDzero(dtEnd.Second);
            byte[] byteReturn = new byte[34];
            byte[] arrayStart = System.Text.Encoding.ASCII.GetBytes(sTimeStart);
            byte[] arrayEnd = System.Text.Encoding.ASCII.GetBytes(sTimeEnd);
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x27;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x18;
            byteReturn[9] = arrayStart[0];
            byteReturn[10] = arrayStart[1];
            byteReturn[11] = arrayStart[2];
            byteReturn[12] = arrayStart[3];
            byteReturn[13] = arrayStart[4];
            byteReturn[14] = arrayStart[5];
            byteReturn[15] = arrayStart[6];
            byteReturn[16] = arrayStart[7];
            byteReturn[17] = arrayStart[8];
            byteReturn[18] = arrayStart[9];
            byteReturn[19] = arrayStart[10];
            byteReturn[20] = arrayStart[11];

            for (int i = 0; i < 12; i++)
            {
                byteReturn[i + 21] = arrayEnd[i];
            }
            int iYiHuo = 0;
            for (int i = 9; i < 33; i++)
            { iYiHuo = iYiHuo ^ byteReturn[i]; }
            byteReturn[33] = (byte)iYiHuo;




            return byteReturn;
        }

        public byte[] Data_Download_Modify(DateTime dtStart, DateTime dtEnd,int iCount)
        {
            //string sTimeStart = "";
            //string sTimeEnd = "";
            //sTimeStart += dtStart.Year.ToString().Substring(2, 2);
            //sTimeStart += sADDzero(dtStart.Month);
            //sTimeStart += sADDzero(dtStart.Day);
            //sTimeStart += sADDzero(dtStart.Hour);
            //sTimeStart += sADDzero(dtStart.Minute);
            //sTimeStart += sADDzero(dtStart.Second);

            //sTimeEnd += dtEnd.Year.ToString().Substring(2, 2);
            //sTimeEnd += sADDzero(dtEnd.Month);
            //sTimeEnd += sADDzero(dtEnd.Day);
            //sTimeEnd += sADDzero(dtEnd.Hour);
            //sTimeEnd += sADDzero(dtEnd.Minute);
            //sTimeEnd += sADDzero(dtEnd.Second);

            TimeSpan ts = dtEnd - dtStart;
            byte[] bTime_Count = DataForDownloadCount((ts.TotalHours)*iCount);
            byte[] byteReturn = new byte[12];
           // byte[] arrayStart = System.Text.Encoding.ASCII.GetBytes(sTimeStart);
           // byte[] arrayEnd = System.Text.Encoding.ASCII.GetBytes(sTimeEnd);
            byteReturn[0] = 0x7E;
            byteReturn[1] = 0x00;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x27;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x00;
            byteReturn[6] = 0x00;
            byteReturn[7] = 0x00;
            byteReturn[8] = 0x02;
            byteReturn[9] = bTime_Count[0];
            byteReturn[10] = bTime_Count[1];

            byteReturn[11] = (byte)(byteReturn[9] ^ byteReturn[10]);
           
           // int iYiHuo = 0;
           // for (int i = 9; i < 33; i++)
           // { iYiHuo = iYiHuo ^ byteReturn[i]; }
           // byteReturn[11] = (byte)iYiHuo;




            return byteReturn;
        }
        public  string sADDzero(int izero)
        {
            string szero = "00";

            if (izero < 10 && izero > 0)
            {
                szero = "0" + izero.ToString();
                return szero;
            }
            else if (izero >= 10 && izero < 100)
            {
                szero = izero.ToString();

                return szero;
            }
            else
            { return szero; }

        }
        public byte ZFPanDuan(double fZf)
        {
            byte bReturn = 0x2B;
            if (fZf >= 0)
                bReturn = 0x2B;
            else
                fZf = 0x2D;
            return bReturn;
        }

        public byte[] DataForCh(double fZf)
        {
            byte[] bReturn = new byte[3];
            string sZf = fZf.ToString("0.00000");
            string[] sTemp1 = sZf.Split('.');
            string sTemp2 = Convert.ToInt32(sTemp1[1]).ToString("X2").PadLeft(4, '0');
            bReturn[0] = Convert.ToByte(Convert.ToInt32(sTemp1[0]).ToString("X"), 16);
            bReturn[1] = Convert.ToByte(sTemp2.Substring(0, 2),16);
            bReturn[2] = Convert.ToByte(sTemp2.Substring(2, 2),16);
            return bReturn;
        }
        public byte[] DataForDownloadCount(double fZf)
        {
            byte[] bReturn = new byte[2];
            string sZf = fZf.ToString("0.00");
            string[] sTemp1 = sZf.Split('.');
            string sTemp2 = Convert.ToInt32(sTemp1[0]).ToString("X2").PadLeft(4, '0');
            //bReturn[0] = Convert.ToByte(Convert.ToInt32(sTemp1[0]).ToString("X"), 16);
            bReturn[0] = Convert.ToByte(sTemp2.Substring(0, 2), 16);
            bReturn[1] = Convert.ToByte(sTemp2.Substring(2, 2), 16);
            return bReturn;
        }
        public byte[] DataForChSeaLevel(double fZf)
        {
            byte[] bReturn = new byte[5];
            string sZf = fZf.ToString("0.0");
            string[] sTemp1 = sZf.Split('.');
            string sTemp2 = Convert.ToInt32(sTemp1[0]).ToString("X4").PadLeft(8, '0');
          
            bReturn[0] = Convert.ToByte(sTemp2.Substring(0, 2), 16);
            bReturn[1] = Convert.ToByte(sTemp2.Substring(2, 2), 16);
            bReturn[2] = Convert.ToByte(sTemp2.Substring(4, 2), 16);
            bReturn[3] = Convert.ToByte(sTemp2.Substring(6, 2), 16);
            bReturn[4] = Convert.ToByte(Convert.ToInt32(sTemp1[1]).ToString("X"), 16);
            return bReturn;
        }


       /*接收信息处理*/
        public string JishiData_Return_Pro(byte[] bReceive)
        {
            string sReturn = ByteArrayToHexString_Get1(bReceive);

            return sReturn;
        }
        public string DownLoadData_Return_Pro(byte[] bReceive)
        {
            string sReturn = ByteArrayToHexString_Get(bReceive);

            return sReturn;
        }

        public string Basic_Msg_Return_Pro(byte[] bReceive)
        {
           
            string sReturn = ByteArrayToHexString_Get(bReceive);
           // bReceive = Longitude_Latitude_Elevation_Set(23.45833, 23.03, 100.8);
            if (bReceive.Length == 24)
            {
                string sLan = "";
                string sLat = "";
                string sSeaLevel = "";
                if ((bReceive[9] ^ bReceive[10] ^ bReceive[11] ^ bReceive[12] ^ bReceive[13] ^ bReceive[14] ^ bReceive[15] ^ bReceive[16]
                    ^ bReceive[17] ^ bReceive[18] ^ bReceive[19] ^ bReceive[20] ^ bReceive[21] ^ bReceive[22]) == (int)bReceive[23])
                {
                    if (bReceive[9] == 0x2B)
                    {
                        string sZheng = Convert.ToString(bReceive[10], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[11], 16).PadLeft(2, '0') + Convert.ToString(bReceive[12], 16).PadLeft(2, '0');
                        sLan = Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }
                    else if (bReceive[9] == 0x2D)
                    {
                        string sZheng = Convert.ToString(bReceive[10], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[11], 16).PadLeft(2, '0') + Convert.ToString(bReceive[12], 16).PadLeft(2, '0');
                        sLan = "-" + Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }
                    if (bReceive[13] == 0x2B)
                    {
                        string sZheng = Convert.ToString(bReceive[14], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[15], 16).PadLeft(2, '0') + Convert.ToString(bReceive[16], 16).PadLeft(2, '0');
                        sLat = Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }
                    else if (bReceive[13] == 0x2D)
                    {
                        string sZheng = Convert.ToString(bReceive[14], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[15], 16).PadLeft(2, '0') + Convert.ToString(bReceive[16], 16).PadLeft(2, '0');
                        sLat = "-" + Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }

                    if (bReceive[17] == 0x2B)
                    {
                        string sZheng = Convert.ToString(bReceive[18], 16).PadLeft(2, '0')
                                      + Convert.ToString(bReceive[19], 16).PadLeft(2, '0')
                                      + Convert.ToString(bReceive[20], 16).PadLeft(2, '0')
                                      + Convert.ToString(bReceive[21], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[22], 16).PadLeft(2, '0');
                        sSeaLevel = Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }
                    else if (bReceive[17] == 0x2D)
                    {
                        string sZheng = Convert.ToString(bReceive[18], 16).PadLeft(2, '0')
                                       + Convert.ToString(bReceive[19], 16).PadLeft(2, '0')
                                       + Convert.ToString(bReceive[20], 16).PadLeft(2, '0')
                                       + Convert.ToString(bReceive[21], 16).PadLeft(2, '0');
                        string sXiaoshu = Convert.ToString(bReceive[22], 16).PadLeft(2, '0');
                        sSeaLevel = "-" + Convert.ToInt32(sZheng, 16).ToString() + "." + Convert.ToInt32(sXiaoshu, 16).ToString();
                    }
                }
                sReturn = "经度:" + sLan + ",纬度:" + sLat + ",海拔:" + sSeaLevel;
            }
            else
                sReturn += "收到参数信息有误";
           
            return sReturn;
        }
        /*接收信息处理 ---End---*/

        public  string ByteArrayToHexString_Get(byte[] data)//字节数组转为十六进制字符串
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                // sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }
        public string ByteArrayToHexString_Get1(byte[] data)//字节数组转为十六进制字符串
        {
           
            StringBuilder sb = new StringBuilder(data.Length * 3);
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
           string sReturn = asciiEncoding.GetString(data);
          //  foreach (byte b in data)
                // sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
             //   sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
           // return sb.ToString().ToUpper();
           return sReturn;
        }
        public byte Check_Data(byte[] bData,int iStart,int iEnd)
        {
            byte bReturn = 0;
            for (int i = iStart; i <= iEnd; i++)
            {
                bReturn =(byte)(bReturn ^ bData[i]);
            }
            return bReturn;
        }
    }
}
