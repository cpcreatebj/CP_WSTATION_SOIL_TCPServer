using System;
using System.Collections.Generic;
using System.Text;

namespace CP_WSMonitor
{
    class Math
    {
        public double soilmoist_Math(double d_Receive)
        {
            double soilMoist = 0;
            try
            {
                double soilMoist_tem = d_Receive ;
                //soilMoist = 0.0000043 * soilMoist_tem * soilMoist_tem * soilMoist_tem
                    // - 0.00055 * soilMoist_tem * soilMoist_tem
                    // + 0.0292 * soilMoist_tem
                    // - 0.053;
                soilMoist = soilMoist_tem;
                soilMoist = Convert.ToDouble(soilMoist.ToString("0.00"));
                if (soilMoist < 0)
                { soilMoist = 0; }
            }
            catch (Exception ex)
            { return 0; }
            return soilMoist;
        }
        public double solar_qiangdu(double solar_fushe)
        {
            double solar_Tem = 0;
            solar_Tem = solar_fushe * 683;
            solar_Tem = Convert.ToDouble(solar_Tem.ToString("0.0"));
            return solar_Tem;
        }
    }
}
