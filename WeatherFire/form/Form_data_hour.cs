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
    public partial class Form_data_hour : Form
    {
        public string sHeaderText = "站点ID,数据时间,温度(℃),湿度(%),风速(m/s),风向(度),太阳辐射(w/m2),光照强度(lx),雨量(mm),土壤温度(℃),土壤湿度(%)";
        public Form_data_hour()
        {
            InitializeComponent();
            dateTimePicker1.Text = DateTime.Now.AddDays(-1).ToString();
            string cnString_Mysql = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
            DateTime dt_Start = dateTimePicker1.Value;
            DateTime dt_Stop = dateTimePicker2.Value;
            //SqlConnection conn = new SqlConnection(cnString_Mysql);
            MySqlConnection conn = new MySqlConnection(cnString_Mysql);
            DataSet myDs = new DataSet();
            conn.Open();
            // using (SqlDataAdapter myDa = new SqlDataAdapter("select Station_ID,Station_Name,Station_Type,Station_Mode,Station_DataTime_Last,Station_Lan,Station_Lat from CP_Station", conn))
            using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Temp,CP_Data_Hum,CP_Data_WindSpeed,CP_Data_WindDirection,CP_Data_Solar,CP_Data_More1,CP_Data_Rain,CP_Data_Tuwen,CP_Data_Tushi from cp_sensor_data_hour where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' order by CP_Data_DateTime desc", conn))
            {


                myDa.Fill(myDs);
            }
            conn.Close();
            DataGridViewIni(myDs);
        }

        private void DataGridViewIni(DataSet myDs)
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();

                DataView dv = myDs.Tables[0].DefaultView;
                dv.AllowNew = false;

                dataGridView1.DataSource = dv;
                dataGridView1.Columns["CP_Data_DateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss "; 
                string[] sHeaderTextList = sHeaderText.Split(',');

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].HeaderText = sHeaderTextList[i];
                }

            }
            catch (Exception ex)
            { }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cnString_Mysql = ConfigurationManager.ConnectionStrings["MYSQLConnectionString"].ConnectionString;
            DateTime dt_Start = dateTimePicker1.Value;
            DateTime dt_Stop = dateTimePicker2.Value;
            //SqlConnection conn = new SqlConnection(cnString_Mysql);
            MySqlConnection conn = new MySqlConnection(cnString_Mysql);
            DataSet myDs = new DataSet();
            conn.Open();
            // using (SqlDataAdapter myDa = new SqlDataAdapter("select Station_ID,Station_Name,Station_Type,Station_Mode,Station_DataTime_Last,Station_Lan,Station_Lat from CP_Station", conn))
            if (textBox_zhaoce_sID.Text.Trim() == "")
            {
                using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Temp,CP_Data_Hum,CP_Data_WindSpeed,CP_Data_WindDirection,CP_Data_Solar,CP_Data_More1,CP_Data_Rain,CP_Data_Tuwen,CP_Data_Tushi from cp_sensor_data_hour where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' order by CP_Data_DateTime desc", conn))
                {

                    myDa.Fill(myDs);
                }
            }
            else
            {
                try
                {
                    using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Temp,CP_Data_Hum,CP_Data_WindSpeed,CP_Data_WindDirection,CP_Data_Solar,CP_Data_More1,CP_Data_Rain,CP_Data_Tuwen,CP_Data_Tushi from cp_sensor_data_hour where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' and CP_Data_StationID='" + textBox_zhaoce_sID.Text.Trim() + "' order by CP_Data_DateTime desc", conn))
                    {

                        myDa.Fill(myDs);
                    }
                }
                catch (Exception ex)
                { }
            }
            conn.Close();
            DataGridViewIni(myDs);
        }
    }
}
