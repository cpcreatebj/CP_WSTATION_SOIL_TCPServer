using System;
using System.Collections.Generic;
using System.Text;

namespace CP_WSMonitor
{
   public class terminalDATADTO
    {
       //温度、湿度、气压、风向、风速、rain、UV、solar、co2、pm2.5、土壤水分、土壤温度
       private DateTime tmdtodatetime;

        public DateTime Tmdtodatetime
        {
            get { return tmdtodatetime; }
            set { tmdtodatetime = value; }
        }
        private double terminaldataTEMP;

        public double TerminaldataTEMP
        {
            get { return terminaldataTEMP; }
            set { terminaldataTEMP = value; }
        }
        private double terminaldataHUM;

        public double TerminaldataHUM
        {
            get { return terminaldataHUM; }
            set { terminaldataHUM = value; }
        }
        private double terminaldataBAR;

        public double TerminaldataBAR
        {
            get { return terminaldataBAR; }
            set { terminaldataBAR = value; }
        }
        private double terminaldataWINDD;

        public double TerminaldataWINDD
        {
            get { return terminaldataWINDD; }
            set { terminaldataWINDD = value; }
        }
        private double terminaldataWINDS;

        public double TerminaldataWINDS
        {
            get { return terminaldataWINDS; }
            set { terminaldataWINDS = value; }
        }
        private double terminaldataRAIN;

        public double TerminaldataRAIN
        {
            get { return terminaldataRAIN; }
            set { terminaldataRAIN = value; }
        }
        private double terminaldataUV;

        public double TerminaldataUV
        {
            get { return terminaldataUV; }
            set { terminaldataUV = value; }
        }
        private double terminaldataSOLAR;

        public double TerminaldataSOLAR
        {
            get { return terminaldataSOLAR; }
            set { terminaldataSOLAR = value; }
        }
        private double terminaldataCO2;

        public double TerminaldataCO2
        {
            get { return terminaldataCO2; }
            set { terminaldataCO2 = value; }
        }
        private double terminaldataPM25;

        public double TerminaldataPM25
        {
            get { return terminaldataPM25; }
            set { terminaldataPM25 = value; }
        }
        private double terminaldataSOILMOISTURE;

        public double TerminaldataSOILMOISTURE
        {
            get { return terminaldataSOILMOISTURE; }
            set { terminaldataSOILMOISTURE = value; }
        }
       private double terminaldataSOILTEMP;

       public double TerminaldataSOILTEMP
       {
           get { return terminaldataSOILTEMP; }
           set { terminaldataSOILTEMP = value; }
       }
    }
}
