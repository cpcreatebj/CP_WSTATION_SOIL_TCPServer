using System.Configuration;
namespace CP_WSMonitor
{
    partial class CP_WeatherStation
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CP_WeatherStation));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Set1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.set11 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_set13 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_set14 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_set16 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_set17 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_set21 = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_send = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_Restart = new System.Windows.Forms.Button();
            this.textBox_ID = new System.Windows.Forms.TextBox();
            this.button_IDSet = new System.Windows.Forms.Button();
            this.button_TimeSet = new System.Windows.Forms.Button();
            this.button_Download = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox_Datetime_Hour = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button_IPSet = new System.Windows.Forms.Button();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.textBox_IPAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_Download_All = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Data_Delete = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.button_Jishi_Get = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_Time_Get = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button_JianGe_Get = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_ComNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabeloffline = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.设置1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_data_select = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_data_zhengdian = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_data_zhaoce = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_data_jishi_soil = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_STATION_Select = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            contextMenuStrip1.SuspendLayout();
            this.Set1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.contextMenuStrip1_Click);
            // 
            // Set1
            // 
            this.Set1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.set11,
            this.ToolStripMenuItem_set13,
            this.ToolStripMenuItem_set14,
            this.ToolStripMenuItem_set16,
            this.ToolStripMenuItem_set17,
            this.ToolStripMenuItem_set21});
            this.Set1.Name = "contextMenuStrip2";
            this.Set1.Size = new System.Drawing.Size(149, 136);
            this.Set1.Text = "设置";
            this.Set1.Opening += new System.ComponentModel.CancelEventHandler(this.Set1_Opening);
            // 
            // set11
            // 
            this.set11.Name = "set11";
            this.set11.Size = new System.Drawing.Size(148, 22);
            this.set11.Text = "设置时间";
            this.set11.Click += new System.EventHandler(this.set11_Click);
            // 
            // ToolStripMenuItem_set13
            // 
            this.ToolStripMenuItem_set13.Name = "ToolStripMenuItem_set13";
            this.ToolStripMenuItem_set13.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_set13.Text = "设置ID";
            this.ToolStripMenuItem_set13.Click += new System.EventHandler(this.ToolStripMenuItem_set13_Click);
            // 
            // ToolStripMenuItem_set14
            // 
            this.ToolStripMenuItem_set14.Name = "ToolStripMenuItem_set14";
            this.ToolStripMenuItem_set14.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_set14.Text = "设备重启";
            // 
            // ToolStripMenuItem_set16
            // 
            this.ToolStripMenuItem_set16.Name = "ToolStripMenuItem_set16";
            this.ToolStripMenuItem_set16.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_set16.Text = "数据招测";
            this.ToolStripMenuItem_set16.Click += new System.EventHandler(this.ToolStripMenuItem_set16_Click);
            // 
            // ToolStripMenuItem_set17
            // 
            this.ToolStripMenuItem_set17.Name = "ToolStripMenuItem_set17";
            this.ToolStripMenuItem_set17.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_set17.Text = "数据下载";
            // 
            // ToolStripMenuItem_set21
            // 
            this.ToolStripMenuItem_set21.Name = "ToolStripMenuItem_set21";
            this.ToolStripMenuItem_set21.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_set21.Text = "站点信息配置";
            this.ToolStripMenuItem_set21.Click += new System.EventHandler(this.ToolStripMenuItem_set21_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.ReadTimeout = 1000;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(0, 524);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1124, 76);
            this.textBox1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(303, 53);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "间隔设置";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_send
            // 
            this.comboBox_send.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_send.ForeColor = System.Drawing.Color.Black;
            this.comboBox_send.FormattingEnabled = true;
            this.comboBox_send.Items.AddRange(new object[] {
            "1分钟",
            "10分钟",
            "30分钟",
            "1小时",
            "2小时"});
            this.comboBox_send.Location = new System.Drawing.Point(114, 55);
            this.comboBox_send.Name = "comboBox_send";
            this.comboBox_send.Size = new System.Drawing.Size(67, 20);
            this.comboBox_send.TabIndex = 9;
            this.comboBox_send.Text = "1分钟";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(32, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "存储间隔：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(32, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "开始时间：";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(207, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "下载数据：";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(32, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "及时数据：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(462, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "设 备 ID：";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Reset.ForeColor = System.Drawing.Color.Black;
            this.button_Reset.Location = new System.Drawing.Point(114, 19);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(75, 23);
            this.button_Reset.TabIndex = 21;
            this.button_Reset.Text = "初始化";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // button_Restart
            // 
            this.button_Restart.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Restart.ForeColor = System.Drawing.Color.Black;
            this.button_Restart.Location = new System.Drawing.Point(22, 19);
            this.button_Restart.Name = "button_Restart";
            this.button_Restart.Size = new System.Drawing.Size(75, 23);
            this.button_Restart.TabIndex = 22;
            this.button_Restart.Text = "重启";
            this.button_Restart.UseVisualStyleBackColor = true;
            this.button_Restart.Click += new System.EventHandler(this.button_Restart_Click);
            // 
            // textBox_ID
            // 
            this.textBox_ID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ID.ForeColor = System.Drawing.Color.Black;
            this.textBox_ID.Location = new System.Drawing.Point(528, 19);
            this.textBox_ID.Name = "textBox_ID";
            this.textBox_ID.ReadOnly = true;
            this.textBox_ID.Size = new System.Drawing.Size(100, 21);
            this.textBox_ID.TabIndex = 23;
            this.textBox_ID.TextChanged += new System.EventHandler(this.textBox_ID_TextChanged);
            // 
            // button_IDSet
            // 
            this.button_IDSet.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_IDSet.ForeColor = System.Drawing.Color.Black;
            this.button_IDSet.Location = new System.Drawing.Point(845, 19);
            this.button_IDSet.Name = "button_IDSet";
            this.button_IDSet.Size = new System.Drawing.Size(75, 23);
            this.button_IDSet.TabIndex = 24;
            this.button_IDSet.Text = "设置ID";
            this.button_IDSet.UseVisualStyleBackColor = true;
            this.button_IDSet.Click += new System.EventHandler(this.button_IDSet_Click);
            // 
            // button_TimeSet
            // 
            this.button_TimeSet.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_TimeSet.ForeColor = System.Drawing.Color.Black;
            this.button_TimeSet.Location = new System.Drawing.Point(303, 19);
            this.button_TimeSet.Name = "button_TimeSet";
            this.button_TimeSet.Size = new System.Drawing.Size(75, 23);
            this.button_TimeSet.TabIndex = 25;
            this.button_TimeSet.Text = "设置时间";
            this.button_TimeSet.UseVisualStyleBackColor = true;
            this.button_TimeSet.Click += new System.EventHandler(this.button_TimeSet_Click);
            // 
            // button_Download
            // 
            this.button_Download.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Download.ForeColor = System.Drawing.Color.Black;
            this.button_Download.Location = new System.Drawing.Point(303, 52);
            this.button_Download.Name = "button_Download";
            this.button_Download.Size = new System.Drawing.Size(75, 23);
            this.button_Download.TabIndex = 28;
            this.button_Download.Text = "下载";
            this.button_Download.UseVisualStyleBackColor = true;
            this.button_Download.Click += new System.EventHandler(this.button_Download_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(97, 53);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(100, 21);
            this.dateTimePicker1.TabIndex = 29;
            // 
            // comboBox_Datetime_Hour
            // 
            this.comboBox_Datetime_Hour.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_Datetime_Hour.FormattingEnabled = true;
            this.comboBox_Datetime_Hour.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.comboBox_Datetime_Hour.Location = new System.Drawing.Point(214, 53);
            this.comboBox_Datetime_Hour.Name = "comboBox_Datetime_Hour";
            this.comboBox_Datetime_Hour.Size = new System.Drawing.Size(46, 20);
            this.comboBox_Datetime_Hour.TabIndex = 30;
            this.comboBox_Datetime_Hour.Text = "0";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(759, 52);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "停止";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button_IPSet
            // 
            this.button_IPSet.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_IPSet.ForeColor = System.Drawing.Color.Black;
            this.button_IPSet.Location = new System.Drawing.Point(845, 52);
            this.button_IPSet.Name = "button_IPSet";
            this.button_IPSet.Size = new System.Drawing.Size(75, 23);
            this.button_IPSet.TabIndex = 9;
            this.button_IPSet.Text = "设置";
            this.button_IPSet.UseVisualStyleBackColor = true;
            this.button_IPSet.Visible = false;
            this.button_IPSet.Click += new System.EventHandler(this.button_IPSet_Click);
            // 
            // textBox_Port
            // 
            this.textBox_Port.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Port.ForeColor = System.Drawing.Color.Black;
            this.textBox_Port.Location = new System.Drawing.Point(683, 52);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(50, 21);
            this.textBox_Port.TabIndex = 8;
            this.textBox_Port.Visible = false;
            this.textBox_Port.TextChanged += new System.EventHandler(this.textBox_Port_TextChanged);
            // 
            // textBox_IPAddress
            // 
            this.textBox_IPAddress.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_IPAddress.ForeColor = System.Drawing.Color.Black;
            this.textBox_IPAddress.Location = new System.Drawing.Point(528, 52);
            this.textBox_IPAddress.Name = "textBox_IPAddress";
            this.textBox_IPAddress.Size = new System.Drawing.Size(100, 21);
            this.textBox_IPAddress.TabIndex = 7;
            this.textBox_IPAddress.Visible = false;
            this.textBox_IPAddress.TextChanged += new System.EventHandler(this.textBox_IPAddress_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(634, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "端  口：";
            this.label6.Visible = false;
            this.label6.Click += new System.EventHandler(this.label6_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(462, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "地    址：";
            this.label5.Visible = false;
            this.label5.Click += new System.EventHandler(this.label5_Click_1);
            // 
            // button_Download_All
            // 
            this.button_Download_All.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Download_All.ForeColor = System.Drawing.Color.Black;
            this.button_Download_All.Location = new System.Drawing.Point(303, 15);
            this.button_Download_All.Name = "button_Download_All";
            this.button_Download_All.Size = new System.Drawing.Size(75, 23);
            this.button_Download_All.TabIndex = 33;
            this.button_Download_All.Text = "下载全部";
            this.button_Download_All.UseVisualStyleBackColor = true;
            this.button_Download_All.Click += new System.EventHandler(this.button_Download_All_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_Data_Delete);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.button_Jishi_Get);
            this.groupBox1.Controls.Add(this.button_Download);
            this.groupBox1.Controls.Add(this.button_Download_All);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.comboBox_Datetime_Hour);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox1.Location = new System.Drawing.Point(0, 433);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1124, 85);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "站点数据";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button_Data_Delete
            // 
            this.button_Data_Delete.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Data_Delete.ForeColor = System.Drawing.Color.Black;
            this.button_Data_Delete.Location = new System.Drawing.Point(546, 15);
            this.button_Data_Delete.Name = "button_Data_Delete";
            this.button_Data_Delete.Size = new System.Drawing.Size(75, 23);
            this.button_Data_Delete.TabIndex = 35;
            this.button_Data_Delete.Text = "清空";
            this.button_Data_Delete.UseVisualStyleBackColor = true;
            this.button_Data_Delete.Click += new System.EventHandler(this.button_Data_Delete_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(462, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 34;
            this.label21.Text = "清空数据：";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // button_Jishi_Get
            // 
            this.button_Jishi_Get.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Jishi_Get.ForeColor = System.Drawing.Color.Black;
            this.button_Jishi_Get.Location = new System.Drawing.Point(114, 15);
            this.button_Jishi_Get.Name = "button_Jishi_Get";
            this.button_Jishi_Get.Size = new System.Drawing.Size(75, 23);
            this.button_Jishi_Get.TabIndex = 26;
            this.button_Jishi_Get.Text = "采集";
            this.button_Jishi_Get.UseVisualStyleBackColor = true;
            this.button_Jishi_Get.Click += new System.EventHandler(this.button_Jishi_Get_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.groupBox3.CausesValidation = false;
            this.groupBox3.ContextMenuStrip = contextMenuStrip1;
            this.groupBox3.Controls.Add(this.button_Restart);
            this.groupBox3.Controls.Add(this.button_Reset);
            this.groupBox3.Controls.Add(this.button_Time_Get);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button_IPSet);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button_JianGe_Get);
            this.groupBox3.Controls.Add(this.textBox_Port);
            this.groupBox3.Controls.Add(this.textBox_IPAddress);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button_IDSet);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.comboBox_send);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_ID);
            this.groupBox3.Controls.Add(this.button_TimeSet);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Maroon;
            this.groupBox3.Location = new System.Drawing.Point(0, 341);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1124, 86);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "站点设置";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // button_Time_Get
            // 
            this.button_Time_Get.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Time_Get.ForeColor = System.Drawing.Color.Black;
            this.button_Time_Get.Location = new System.Drawing.Point(209, 19);
            this.button_Time_Get.Name = "button_Time_Get";
            this.button_Time_Get.Size = new System.Drawing.Size(75, 23);
            this.button_Time_Get.TabIndex = 28;
            this.button_Time_Get.Text = "获取时间";
            this.button_Time_Get.UseVisualStyleBackColor = true;
            this.button_Time_Get.Click += new System.EventHandler(this.button_Time_Get_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(759, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 27;
            this.button4.Text = "获取ID";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button_JianGe_Get
            // 
            this.button_JianGe_Get.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_JianGe_Get.ForeColor = System.Drawing.Color.Black;
            this.button_JianGe_Get.Location = new System.Drawing.Point(209, 53);
            this.button_JianGe_Get.Name = "button_JianGe_Get";
            this.button_JianGe_Get.Size = new System.Drawing.Size(75, 23);
            this.button_JianGe_Get.TabIndex = 26;
            this.button_JianGe_Get.Text = "间隔获取";
            this.button_JianGe_Get.UseVisualStyleBackColor = true;
            this.button_JianGe_Get.Click += new System.EventHandler(this.button_JianGe_Get_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel_ComNumber,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabeloffline});
            this.statusStrip1.Location = new System.Drawing.Point(0, 603);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1124, 22);
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "站点统计: 在线-";
            // 
            // toolStripStatusLabel_ComNumber
            // 
            this.toolStripStatusLabel_ComNumber.ForeColor = System.Drawing.Color.Lime;
            this.toolStripStatusLabel_ComNumber.Name = "toolStripStatusLabel_ComNumber";
            this.toolStripStatusLabel_ComNumber.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabel_ComNumber.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabel2.Text = "不在线-";
            // 
            // toolStripStatusLabeloffline
            // 
            this.toolStripStatusLabeloffline.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabeloffline.Name = "toolStripStatusLabeloffline";
            this.toolStripStatusLabeloffline.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabeloffline.Text = "0";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "CP_WSMonitor";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.ToolStripMenuItem_data_select});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1124, 25);
            this.menuStrip1.TabIndex = 40;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.设置1ToolStripMenuItem,
            this.设置2ToolStripMenuItem});
            this.设置ToolStripMenuItem.Enabled = false;
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(104, 6);
            // 
            // 设置1ToolStripMenuItem
            // 
            this.设置1ToolStripMenuItem.Name = "设置1ToolStripMenuItem";
            this.设置1ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.设置1ToolStripMenuItem.Text = "设置1";
            // 
            // 设置2ToolStripMenuItem
            // 
            this.设置2ToolStripMenuItem.Name = "设置2ToolStripMenuItem";
            this.设置2ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.设置2ToolStripMenuItem.Text = "设置2";
            // 
            // ToolStripMenuItem_data_select
            // 
            this.ToolStripMenuItem_data_select.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_data_zhengdian,
            this.ToolStripMenuItem_data_zhaoce,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_data_jishi_soil});
            this.ToolStripMenuItem_data_select.Name = "ToolStripMenuItem_data_select";
            this.ToolStripMenuItem_data_select.Size = new System.Drawing.Size(68, 21);
            this.ToolStripMenuItem_data_select.Text = "数据查询";
            // 
            // ToolStripMenuItem_data_zhengdian
            // 
            this.ToolStripMenuItem_data_zhengdian.Name = "ToolStripMenuItem_data_zhengdian";
            this.ToolStripMenuItem_data_zhengdian.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItem_data_zhengdian.Text = "整点数据查询";
            this.ToolStripMenuItem_data_zhengdian.Click += new System.EventHandler(this.ToolStripMenuItem_data_zhengdian_Click);
            // 
            // ToolStripMenuItem_data_zhaoce
            // 
            this.ToolStripMenuItem_data_zhaoce.Name = "ToolStripMenuItem_data_zhaoce";
            this.ToolStripMenuItem_data_zhaoce.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItem_data_zhaoce.Text = "即时数据查询";
            this.ToolStripMenuItem_data_zhaoce.Click += new System.EventHandler(this.ToolStripMenuItem_data_zhaoce_Click);
            // 
            // ToolStripMenuItem_data_jishi_soil
            // 
            this.ToolStripMenuItem_data_jishi_soil.Name = "ToolStripMenuItem_data_jishi_soil";
            this.ToolStripMenuItem_data_jishi_soil.Size = new System.Drawing.Size(177, 22);
            this.ToolStripMenuItem_data_jishi_soil.Text = "即时数据查询-土壤";
            this.ToolStripMenuItem_data_jishi_soil.Click += new System.EventHandler(this.ToolStripMenuItem_data_jishi_soil_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "022.gif");
            this.imageList1.Images.SetKeyName(1, "014.gif");
            this.imageList1.Images.SetKeyName(2, "015.gif");
            this.imageList1.Images.SetKeyName(3, "online.gif");
            this.imageList1.Images.SetKeyName(4, "offline.gif");
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.Set1;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1124, 307);
            this.dataGridView1.TabIndex = 42;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(110, -18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 77);
            this.button3.TabIndex = 38;
            this.button3.Text = "清除文本框";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "波特率：";
            this.label2.Visible = false;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600"});
            this.comboBox2.Location = new System.Drawing.Point(191, 11);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(54, 20);
            this.comboBox2.TabIndex = 2;
            this.comboBox2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(254, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "打开串口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20"});
            this.comboBox1.Location = new System.Drawing.Point(71, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(67, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "端    口：";
            this.label1.Visible = false;
            // 
            // comboBox_STATION_Select
            // 
            this.comboBox_STATION_Select.FormattingEnabled = true;
            this.comboBox_STATION_Select.Location = new System.Drawing.Point(71, 11);
            this.comboBox_STATION_Select.Name = "comboBox_STATION_Select";
            this.comboBox_STATION_Select.Size = new System.Drawing.Size(93, 20);
            this.comboBox_STATION_Select.TabIndex = 15;
            this.comboBox_STATION_Select.SelectedIndexChanged += new System.EventHandler(this.comboBox_STATION_Select_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "站点：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.comboBox_STATION_Select);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(455, 553);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 89);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "站点IP地址";
            this.groupBox2.Visible = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItem1.Text = "整点数据查询-土壤";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // CP_WeatherStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1124, 625);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CP_WeatherStation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CP_WSMonitor-北京长平创新科技有限公司";
            this.Load += new System.EventHandler(this.CP_WeatherStation_Load);
            contextMenuStrip1.ResumeLayout(false);
            this.Set1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox comboBox_send;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Button button_Restart;
        private System.Windows.Forms.TextBox textBox_ID;
        private System.Windows.Forms.Button button_IDSet;
        private System.Windows.Forms.Button button_TimeSet;
        private System.Windows.Forms.Button button_Download;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox_Datetime_Hour;
        private System.Windows.Forms.Button button_Download_All;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_Time_Get;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button_JianGe_Get;
        private System.Windows.Forms.Button button_Data_Delete;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button button_Jishi_Get;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_ComNumber;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.TextBox textBox_IPAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button_IPSet;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 设置1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置2ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip Set1;
        private System.Windows.Forms.ToolStripMenuItem set11;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_set13;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_set14;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_set16;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_set17;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_STATION_Select;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabeloffline;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_data_select;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_data_zhengdian;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_data_zhaoce;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_set21;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_data_jishi_soil;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

