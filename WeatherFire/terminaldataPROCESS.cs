using System;
using System.Collections.Generic;
using System.Text;

namespace CP_WSMonitor
{
   public class terminaldataPROCESS
    {
       public static terminalDATADTO tergetprocess(byte[] breceive,int tmaddr)
       {
           terminalDATADTO tmdto = new terminalDATADTO();
           if (tmaddr == 2)
           {
               tmdto.TerminaldataTEMP = (double)(breceive[3] * 256 + breceive[4]) / 10;
               tmdto.TerminaldataHUM = breceive[5] * 256 + breceive[6];
               tmdto.TerminaldataBAR = (double)(breceive[7] * 256 + breceive[8]) / 10;
               tmdto.TerminaldataWINDD = breceive[9] * 256 + breceive[10];
               tmdto.TerminaldataWINDS = (double)(breceive[11] * 256 + breceive[12]) / 10;
               tmdto.TerminaldataRAIN = (double)(breceive[13] * 256 + breceive[14]) / 10;
               //tmdto.TerminaldataUV = (breceive[15] * 256 + breceive[16])*4/550;//w/m2
               tmdto.TerminaldataSOLAR = (breceive[17] * 256 + breceive[18]) * 30 / 69;//w/m2
               tmdto.TerminaldataCO2 = breceive[19] * 256 + breceive[20];//ppm
              // tmdto.TerminaldataPM25 = breceive[21] * 256 + breceive[22];
              // tmdto.TerminaldataSOILMOISTURE = breceive[23] * 256 + breceive[24];
              // tmdto.TerminaldataSOILTEMP = breceive[25] * 256 + breceive[26];
           }
           if (tmaddr == 1)
           {
              // tmdto.TerminaldataTEMP =(double)(breceive[3] * 256 + breceive[4])/10;
              // tmdto.TerminaldataHUM = breceive[5] * 256 + breceive[6];
              // tmdto.TerminaldataBAR = breceive[7] * 256 + breceive[8];
              // tmdto.TerminaldataWINDD = breceive[9] * 256 + breceive[10];
              // tmdto.TerminaldataWINDS = breceive[11] * 256 + breceive[12];
              // tmdto.TerminaldataRAIN = breceive[13] * 256 + breceive[14];
               tmdto.TerminaldataUV = (double)((breceive[15] * 256 + breceive[16]) * 4 / 550);//w/m2
              // tmdto.TerminaldataSOLAR = (breceive[17] * 256 + breceive[18])*30/69;//w/m2
              // tmdto.TerminaldataCO2 = breceive[19] * 256 + breceive[20];
                 tmdto.TerminaldataPM25 = breceive[21] * 256 + breceive[22];
                 tmdto.TerminaldataSOILMOISTURE = (breceive[23] * 256 + breceive[24])/10;
                 tmdto.TerminaldataSOILTEMP = (double)(breceive[25] * 256 + breceive[26]) / 10;
           }
           return tmdto;
       }
       public static terminalDATADTO terdataADD(terminalDATADTO tmdto01, terminalDATADTO tmdto02)
       {
           terminalDATADTO tmdtoreturn = new terminalDATADTO();
           tmdtoreturn.TerminaldataTEMP = tmdto02.TerminaldataTEMP;
           tmdtoreturn.TerminaldataHUM = tmdto02.TerminaldataHUM;
            tmdtoreturn.TerminaldataBAR = tmdto02.TerminaldataBAR;
            tmdtoreturn.TerminaldataWINDD = tmdto02.TerminaldataWINDD;
            tmdtoreturn.TerminaldataWINDS = tmdto02.TerminaldataWINDS;
            tmdtoreturn.TerminaldataRAIN = tmdto02.TerminaldataRAIN;
           tmdtoreturn.TerminaldataUV = tmdto01.TerminaldataUV;
            tmdtoreturn.TerminaldataSOLAR = tmdto02.TerminaldataSOLAR;//w/m2
            tmdtoreturn.TerminaldataCO2 = tmdto02.TerminaldataCO2;
           tmdtoreturn.TerminaldataPM25 = tmdto01.TerminaldataPM25;
           tmdtoreturn.TerminaldataSOILMOISTURE = tmdto01.TerminaldataSOILMOISTURE;
           tmdtoreturn.TerminaldataSOILTEMP = tmdto01.TerminaldataSOILTEMP;
           tmdtoreturn.Tmdtodatetime = tmdto01.Tmdtodatetime;
           return tmdtoreturn;
       }
    }
}
