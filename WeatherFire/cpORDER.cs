using System;
using System.Collections.Generic;
using System.Text;

namespace CP_WSMonitor
{
   public class cpORDER
    {
        //20170823记录：新版通讯协议，包含数据读取，雨量清零
        public static byte[] dataGETorder(byte terminalADDR)
        { 

            byte[] byteReturn = new byte[8];
            byte[] bytetemp = new byte[byteReturn.Length-2];
            byteReturn[0] = terminalADDR;
            byteReturn[1] = 0x03;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x01;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x02;
            for (int i = 0; i < bytetemp.Length; i++)
            { bytetemp[i] = byteReturn[i]; }
            string sCRC = CRC.ToModbusCRC16(bytetemp,true);
            //byte[] bs = new byte[] { byte.Parse(sCRC, System.Globalization.NumberStyles.AllowHexSpecifier) };
            byteReturn[6] = Convert.ToByte(sCRC.Substring(0,2),16);
            byteReturn[7] = Convert.ToByte(sCRC.Substring(2,2),16);
            //BitConverter.
            return byteReturn;
        }
        public static byte[] rainRSTorder(byte terminalADDR)
        {

            byte[] byteReturn = new byte[8];
            byte[] bytetemp = new byte[byteReturn.Length - 2];
            byteReturn[0] = terminalADDR;
            byteReturn[1] = 0x03;
            byteReturn[2] = 0x00;
            byteReturn[3] = 0x01;
            byteReturn[4] = 0x00;
            byteReturn[5] = 0x02;
            for (int i = 0; i < bytetemp.Length; i++)
            { bytetemp[i] = byteReturn[i]; }
            string sCRC = CRC.ToModbusCRC16(bytetemp, true);
            //byte[] bs = new byte[] { byte.Parse(sCRC, System.Globalization.NumberStyles.AllowHexSpecifier) };
            byteReturn[6] = Convert.ToByte(sCRC.Substring(0, 2), 16);
            byteReturn[7] = Convert.ToByte(sCRC.Substring(2, 2), 16);
            //BitConverter.
            return byteReturn;
        }

        public static void datahourGET()
        {

            DateTime dttem = DateTime.Now;
            DateTime dttem1 = new DateTime(dttem.Year, dttem.Month, dttem.Day, 0, 0, 0);

            int iiii = -1;
            while (true)
            {

                DateTime m_time = DateTime.Now;
                if (iiii == -1)
                {
                    if ( m_time.Minute == 0 )
                    {
                       //get 01
                          //Thread.Sleep(3000);
                        //get 02
                        iiii = 0;
                        dttem = DateTime.Now;
                    }
                }
                else
                {
                    if ( m_time.Minute == 0 && !dttem.Hour.Equals(m_time.Hour)) //判断是否指定时间(Invoke_Time)
                    {
                        //get 01
                        //Thread.Sleep(3000);
                        //get 02
                        dttem = DateTime.Now;
                    }

                }

                //Thread.Sleep(100);

            }



        }
    }
}
