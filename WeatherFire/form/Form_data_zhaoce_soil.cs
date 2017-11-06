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
    public partial class Form_data_zhaoce_soil : Form
    {
        public string sHeaderText = "站点ID,数据时间,土壤温度1(℃),土壤水分1(%),土壤盐分1(mg/L),土壤温度2(℃),土壤水分2(%),土壤盐分2(mg/L),土壤温度3(℃),土壤水分3(%),土壤盐分3(mg/L),土壤温度4(℃),土壤水分4(%),土壤盐分4(mg/L)";
        public Form_data_zhaoce_soil()
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
            using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Tuwen,CP_Data_Tushi,CP_Data_Tuyan1,CP_Data_Tuwen2,CP_Data_Tushi2,CP_Data_Tuyan2,CP_Data_Tuwen3,CP_Data_Tushi3,CP_Data_Tuyan3,CP_Data_Tuwen4,CP_Data_Tushi4,CP_Data_Tuyan4 from cp_sensor_data where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' order by CP_Data_DateTime desc", conn))
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
                //  DataGridViewImageColumn dVLColumnImage = new DataGridViewImageColumn();


                // dVLColumnImage.Name = "status_image";
                // dVLColumnImage.Image = imageList1.Images[4];
                // dataGridView1.Columns.Add(dVLColumnImage);
                DataView dv = myDs.Tables[0].DefaultView;
                dv.AllowNew = false;

                dataGridView1.DataSource = dv;
                dataGridView1.Columns["CP_Data_DateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss "; 
                //dataGridView1.Columns["CP_Data_DateTime"].fo
                string[] sHeaderTextList = sHeaderText.Split(',');

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].HeaderText = sHeaderTextList[i];
                }
                // dataGridView1.Columns["Station_Type"].CellType = typeof(string);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    // dataGridView1.Rows[i].Cells["Station_Type"].ValueType = typeof(string);
                    // dataGridView1.Rows[i].Cells["Station_Mode"].ValueType = typeof(string);
                    try
                    {

                        //    dataGridView1.Rows[i].Cells["Station_Type"].Value = Dic_Station_Type[dataGridView1.Rows[i].Cells["Station_Type"].Value.ToString()];

                    }
                    catch (Exception ex)
                    {

                        //  dataGridView1.Rows[i].Cells["Station_Type"].Value = "---";
                    }

                    try
                    {

                        //  dataGridView1.Rows[i].Cells["Station_Mode"].Value = Dic_Station_Mode[dataGridView1.Rows[i].Cells["Station_Mode"].Value.ToString()];
                    }
                    catch (Exception ex)
                    {
                        //  dataGridView1.Rows[i].Cells["Station_Mode"].Value = "---";
                    }
                }
                // textBox_zhaoce_sID.Text = Get_Selected_Station_ID();
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
                using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Tuwen,CP_Data_Tushi,CP_Data_Tuyan1,CP_Data_Tuwen2,CP_Data_Tushi2,CP_Data_Tuyan2,CP_Data_Tuwen3,CP_Data_Tushi3,CP_Data_Tuyan3,CP_Data_Tuwen4,CP_Data_Tushi4,CP_Data_Tuyan4 from cp_sensor_data where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' order by CP_Data_DateTime desc", conn))
                {

                    myDa.Fill(myDs);
                }
            }
            else
            {
                try
                {
                    using (MySqlDataAdapter myDa = new MySqlDataAdapter("select CP_Data_StationID,CP_Data_DateTime,CP_Data_Tuwen,CP_Data_Tushi,CP_Data_Tuyan1,CP_Data_Tuwen2,CP_Data_Tushi2,CP_Data_Tuyan2,CP_Data_Tuwen3,CP_Data_Tushi3,CP_Data_Tuyan3,CP_Data_Tuwen4,CP_Data_Tushi4,CP_Data_Tuyan4 from cp_sensor_data where CP_Data_DateTime>='" + dt_Start + "' and  CP_Data_DateTime<='" + dt_Stop + "' and CP_Data_StationID='" + textBox_zhaoce_sID.Text.Trim() + "' order by CP_Data_DateTime desc", conn))
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
