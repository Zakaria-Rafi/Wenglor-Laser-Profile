namespace EthernetScannnerDemo
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using System.Globalization;
    using Timer = System.Windows.Forms.Timer;

    public partial class weCat3D_SDK : Form
    {
        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_Connect", CallingConvention = CallingConvention.StdCall)]
        //private unsafe static extern IntPtr EthernetScanner_Connect(StringBuilder chIP, StringBuilder chPort, int uiNonBlockingTimeOut);
        public static extern IntPtr EthernetScanner_Connect(string ipAddress, string port, int nonBlockingTimeOut);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_Disconnect", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr EthernetScanner_Disconnect(IntPtr hScanner);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_GetConnectStatus", CallingConvention = CallingConvention.StdCall)]
        public static extern void EthernetScanner_GetConnectStatus(IntPtr hScanner, int[] uiConnectStatus);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_WriteData", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_WriteData(IntPtr hScanner, byte[] chWriteBuffer, int uiWriteBufferLength);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_GetVersion", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_GetVersion(StringBuilder ucBuffer, int uiBuffer);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_GetXZIExtended", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_GetXZIExtended(IntPtr sensorHandle,
                                                                double[] x,
                                                                double[] z,
                                                                int[] intensity,
                                                                int[] peakWidth,
                                                                int buffer,
                                                                int[] encoder,
                                                                byte[] pucUSRIO,
                                                                int timeOut,
                                                                byte[] ucBufferRaw,
                                                                int iBufferRaw,
                                                                int[] picCount);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_ReadData", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_ReadData(IntPtr sensorHandle, string strPropertyName, StringBuilder RetBuf, int iRetBuf, int iCashTime);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_GetInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_GetInfo(IntPtr sensorHandle, StringBuilder info, int buffer, string mode);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_ResetDllFiFo", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_ResetDllFiFo(IntPtr sensorHandle);

        [DllImport("EthernetScanner.dll", EntryPoint = "EthernetScanner_GetDllFiFoState", CallingConvention = CallingConvention.StdCall)]
        public static extern int EthernetScanner_GetDllFiFoState(IntPtr sensorHandle);

        internal int iETHERNETSCANNER_TCPSCANNERDISCONNECTED = 0;

        internal int iETHERNETSCANNER_TCPSCANNERCONNECTED = 3;

        internal int iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX = 2;

        internal int iETHERNETSCANNER_SCANXMAX = 2048;

        public int iETHERNETSCANNER_GETINFOSIZEMAX = 128 * 1024;

        public int iETHERNETSCANNER_GETINFONOVALIDINFO = -3;

        public int iETHERNETSCANNER_GETINFOSMALLBUFFER = -2;

        public int iETHERNETSCANNER_ERROR = -1;

        internal int m_iScannerDataLen = 0;

        internal int m_iScannerDataLen2 = 0;

        internal double[] m_doX = null;

        internal double[] m_doZ = null;

        internal int[] m_iIntensity = null;

        internal int[] m_iPeakWidth = null;

        internal int[] m_iEncoder = null;

        internal byte[] m_bUSRIO = null;

        internal int[] m_iPicCnt = null;

        internal int[] m_iPicCntTemp = null;

        internal double[] m_doX2 = null;

        internal double[] m_doZ2 = null;

        internal int[] m_iIntensity2 = null;

        internal int[] m_iPeakWidth2 = null;

        internal int[] m_iEncoder2 = null;

        internal byte[] m_bUSRIO2 = null;

        internal int[] m_iPicCnt2 = null;

        internal int[] m_iPicCntTemp2 = null;

        internal int m_iPicCntErr = 0;

        internal int m_iPicCntErr2 = 0;

        internal float m_CScanView1_Z_Start = 0;

        internal float m_CScanView1_Z_Range = 0;

        internal float m_CScanView1_X_Range_At_Start = 0;

        internal float m_CScanView1_X_Range_At_End = 0;

        internal float m_CScanView1_WidthX = 0;

        internal float m_CScanView1_WidthZ = 0;

        internal float m_CScanView1_Z_Start2 = 0;

        internal float m_CScanView1_Z_Range2 = 0;

        internal float m_CScanView1_X_Range_At_Start2 = 0;

        internal float m_CScanView1_X_Range_At_End2 = 0;

        internal float m_CScanView1_WidthX2 = 0;

        internal float m_CScanView1_WidthZ2 = 0;

        public IntPtr ScannerHandle;

        public IntPtr ScannerHandle2;

        private Thread ScannerThread = null;

        private Thread ScannerThread2 = null;

        private Thread ScannerThread3 = null;

        private Thread ScannerThread4 = null;

        private Thread ScannerThread5 = null;

        private Thread ScannerThread6 = null;

        private bool m_bScannerThreadRunning = false;

        private bool m_bScannerThreadRunning2 = false;

        private bool m_bScannerThreadRunning3 = false;

        private bool m_bScannerThreadRunning4 = false;

        private bool m_bScannerThreadRunning5 = false;

        private bool m_bScannerThreadRunning6 = false;

        internal int[] iConnectionStatus = null;

        internal int[] iConnectionStatus2 = null;

        public String strApplicationPath = "";

        public String strModelNumber = "";

        public String strProductVersion = "";

        public String strVendorName = "";

        public String strDescription = "";

        public String strHardwareVersion = "";

        public String strFirwareVersion = "";

        public String strScannerMAC = "";

        public String strSerialNummber = "";

        public String strIPAddress = "";

        public String strIPAddress2 = "";

        public double distancex;

        public double distancez;

        internal ConcurrentQueue<double> tempDoZ1 = new ConcurrentQueue<double>();

        internal ConcurrentQueue<double> tempDoX1 = new ConcurrentQueue<double>();

        internal ConcurrentQueue<int> tempDoI1 = new ConcurrentQueue<int>();

        internal ConcurrentQueue<double> tempDoZ2 = new ConcurrentQueue<double>();

        internal ConcurrentQueue<double> tempDoX2 = new ConcurrentQueue<double>();

        internal ConcurrentQueue<int> tempDoI2 = new ConcurrentQueue<int>();

        internal bool tempowrite = false;

        internal float calc = 1;

        internal float profilesnb = 1280;

        internal Graphics m_gImage = null;

        internal Graphics m_gImage2 = null;

        internal Graphics m_gImage3 = null;

        internal System.Windows.Forms.Timer GUI_Timer;

        internal weCat3D_SDK_Expert pScannerSettigs = null;

        internal weCat3D_SDK_Expert2 pScannerSettigs2 = null;

        internal syncsetteings pScannerSettigs3 = null;

        internal int m_iScanner_Frequeny = 0;

        internal int m_iProfile_Counter = 0;

        internal int m_iScanner_Frequeny2 = 0;

        internal int m_iProfile_Counter2 = 0;

        public StringBuilder m_strScannerInfoXML = null;

        public StringBuilder m_strScannerInfoXML2 = null;

        internal bool m_bSaveOnce = false;

        internal int m_iNumberProfilesToSaveCnt = 0;


        internal int m_iNumberProfilesToSaveCntpretrig = 0;

        internal float m_iNumberProfilesToSaveMax = 0;


        internal float m_iNumberProfilesToSaveMaxpretrig = 0;

        internal int m_iNumber2ProfilesToSaveCnt = 0;

        internal float m_iNumber2ProfilesToSaveMax = 0;


        internal float m_iNumberProfilesToSaveCnttrig = 0;

        internal float m_iNumberProfilesToSaveMaxtrig = 0;


        internal int variable = 0;

        internal int variable2 = 0;

        internal int variabledoublesave = 0;

        internal int variablepretrigdouble = 0;

        internal int variablescan1pretrig = 0;

        internal int variablescan2pretrig = 0;


        internal bool m_bSaveOnce2 = false;

        internal int m_iNumberProfilesToSaveCnt2 = 0;

        internal int m_iNumberProfilesToSaveCntpretrig2 = 0;

        internal float m_iNumberProfilesToSaveMax2 = 0;

        internal float m_iNumberProfilesToSaveMaxpretrig2 = 0;

        internal bool m_bSaveOnce3 = false;//save tow profiles

        internal bool m_bSaveOncetrig = false;

        internal bool m_bSaveOncetrigscan1 = false;

        internal bool m_bSaveOncetrigscan2 = false;

        internal bool con2 = false;

        internal System.IO.StreamWriter file;

        internal System.IO.StreamWriter file2;

        internal System.IO.StreamWriter file3;

        internal System.IO.StreamWriter file4;

        internal System.IO.StreamWriter file5;

        internal System.IO.StreamWriter file6;

        internal System.IO.StreamWriter sep1;

        internal System.IO.StreamWriter sep2;

        internal System.IO.StreamWriter sep1pretrig;

        internal System.IO.StreamWriter sep2pretrig;


        public String[] data1 = new string[1280];

        public String[] data2 = new string[1280];

        internal int debut = 0;

        internal int m_iEncoderHTL = 0;

        internal int m_iEncoderTTL = 0;

        internal int m_iEncoderHTL2 = 0;

        internal int m_iEncoderTTL2 = 0;

        internal int m_iDLLFiFo = 0;

        internal int m_iCPUFiFo = 0;

        internal int m_iDLLFiFo2 = 0;

        internal int m_iCPUFiFo2 = 0;

        internal int m_iScannerState = 0;

        internal int m_iScannerState2 = 0;

        internal float saveprofilestrig;

        public float frequencyHz = 0;

        internal int freq;

        internal int ea1Value;

        internal int ea1Valuesave;

        internal int lastvalueea;

        internal int lastvalueeascan1;

        internal int ea1Valuesavescan2;

        internal int lastvalueeascan2;
        float max;

        public float frequencyHzscan1;


        public float frequencyHzscan2;


        internal float queueLengthThreshold;

        internal float queuetimetrigger;

        internal float saveprofilestrig1;

        internal float saveprofilestrig2;


        internal bool scan1queuestop = true;

        internal bool doublescanstop = true;

        internal float queuetimetriggerscan2;

        internal bool scan2queuestop = true;

        CultureInfo point = CultureInfo.InvariantCulture;

        public int max_len;

        private const int ThreadJoinTimeoutMilliseconds = 500;

        public int max_len2;



        private const int WM_CLOSE = 0x0010;


        private Timer timer;
        private bool isMenuOpen;
        private int targetHeight;
        private const int TimerInterval = 10;
        private const int AnimationStep = 15;
        private Panel dropdownPanel1;
        private Panel dropdownPanel2;
        private Panel dropdownPanel3;


        private TextBox textBoxfreqscan1;
        private ComboBox comboBoxledstate;
        private TextBox textBoxNumberOfProfilesToSave;
        private Label label26;
        private Button buttonenvoieFreqscan1;
        private Button btnidlaser;
        private Button buttonSaveOnce;
        private Label label1NumberOfProfileToSave;
        private TextBox textBoxFileNameScan1;
        private Label label43;
        private CheckBox checkBoxpretriggerscan1;
        private TextBox textBoxtimetrigger;
        private Button btnvalidatenewtimepretriggerscan1;
        private Button button7;
        private Label label47;
        private Label label48;
        private Label label25;
        private TextBox textBoxfreqscan2;
        private Button buttonenvoieFreqscan2;
        private ComboBox comboBoxledstate2;
        private Button btnidlaser2;
        private TextBox textBoxNumberOfProfilesToSave2;
        private Button buttonSaveOnce2;
        private TextBox textBoxFileNameScan2;
        private Label label10;
        private Label label39;
        private CheckBox checkBoxpretriggerscan2;
        private TextBox textBoxtimetrigger2;
        private Button button8;
        private Button button9;
        private Label label49;
        private Label label51;
        private Label startpretrig1;
        private Label startpretrig2;
        private TextBox textBox3;
        private Button button4;
        private TextBox textBoxFileNameScan3;
        private CheckBox sepfichiers;
        private Label label33;
        private Label label27;
        private CheckBox savepretrigger;
        private TextBox queueLengthTextBox;
        private TextBox textboxafterpretrigger;
        private Label textboxlimitaftertrig;
        private Button btnvaliderpretrigger;
        private Label labelpretrigstatu;
        private Label labelstartpretrig;
        private Label labelatttrigdouble;
        private Button brnatttriger;
        private CheckBox seppretrigfichiers;
        private Label label4;
        private Label label7;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private ProgressBar progressbartrig;
        private Label pretriggerlabelstat;
        private Button button2;
        private TextBox text_pretrig2scan;
        private TextBox textBoxfreq;
        private Button buttonenvoieFreq;
        private Label freqlabel;
        private TextBox textBoxFrequency;
        private TextBox textBoxFIFO;
        private Button buttonResetStatistic;
        private Button buttonDllFiFoReset;
        private TextBox textBoxFrequency2;
        private TextBox textBoxFIFO2;
        private Button buttonResetStatistic2;
        private Button buttonDllFiFoReset2;
        private ProgressBar progressbartrigsca1;
        private TextBox textboxstatusscan1trigger;
        private Label pretriggerstatusforscan1;

        private ProgressBar progressbartrigsca2;
        private TextBox textboxstatusscan1trigger2;
        private Label pretriggerstatusforscan2;
        private TextBox textboxafterpretriggerscan1;
        private TextBox textboxafterpretriggerscan2;

        private Button toggleMenuButton;
        private bool isMenuVisible;

        public float startrangezmm = 0;
        public float endrangezmm = 0;
        public float startrangezmm2 = 0;
        public float endrangezmm2 = 0;

        public weCat3D_SDK()
        {
            pScannerSettigs = new weCat3D_SDK_Expert(this);
            pScannerSettigs2 = new weCat3D_SDK_Expert2(this);
            pScannerSettigs3 = new syncsetteings(this);

            InitializeComponent();
            InitializeDropdownPanel();
            InitializeToggleMenuButton();
            ToggleMenuButton_Click(toggleMenuButton, EventArgs.Empty);
            //a effacer plus tard car il s'agit de lancement de mon programme et moi je l'ai met a deuxeime ecran
            /*Screen screen = Screen.AllScreens[1];
            this.StartPosition = FormStartPosition.Manual;
            this.Location = screen.Bounds.Location;
            this.Size = screen.Bounds.Size;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.StartPosition = FormStartPosition.CenterScreen;*/
            //escoffier


            this.AutoScaleMode = AutoScaleMode.Dpi;
            //Pointer to the EthernetScanner
            ScannerHandle = (IntPtr)null;
            //EthernetScanner connection status
            iConnectionStatus = new int[1];
            iConnectionStatus2 = new int[1];
            //Current Path of the exe-File
            strApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            //show the Dll-Version-String
            StringBuilder strDllVersion = new StringBuilder(new String(' ', 1024));
            EthernetScanner_GetVersion(strDllVersion, 1024);
            textBoxDllVersion.Text = strDllVersion.ToString();

            //Arrays for the Coordinates: X, Z, Intensity
            //MultiPeakDetection: up to 2 Z-Values on the same X-Position: environment light and/or reflexions 
            m_doX = new double[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_doZ = new double[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iIntensity = new int[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iPeakWidth = new int[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iEncoder = new int[1];
            m_bUSRIO = new byte[1];
            m_iPicCnt = new int[1];
            m_iPicCntTemp = new int[1];

            m_doX2 = new double[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_doZ2 = new double[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iIntensity2 = new int[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iPeakWidth2 = new int[iETHERNETSCANNER_SCANXMAX * iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX + 1];
            m_iEncoder2 = new int[1];
            m_bUSRIO2 = new byte[1];
            m_iPicCnt2 = new int[1];
            m_iPicCntTemp2 = new int[1];


            m_strScannerInfoXML = new StringBuilder(new String(' ', iETHERNETSCANNER_GETINFOSIZEMAX));
            m_strScannerInfoXML2 = new StringBuilder(new String(' ', iETHERNETSCANNER_GETINFOSIZEMAX));



            GUI_Timer = new System.Windows.Forms.Timer();
            GUI_Timer.Interval = 1000; // specify interval time as you want
            GUI_Timer.Tick += new EventHandler(Timer_function);


            distancex = 150;

            distancez = 0;


            text_eng_scan1.Text = "Statut d'enregistrement";
            text_eng_scan2.Text = "Statut d'enregistrement";
            text_eng_scan3.Text = "Statut d'enregistrement";
            text_pretrig2scan.Text = "Statut de Pre-Trigger";



            int initialQueueLengthThreshold;
            if (!int.TryParse(queueLengthTextBox.Text, out initialQueueLengthThreshold))
            {
                initialQueueLengthThreshold = 0;
            }
            queueLengthThreshold = initialQueueLengthThreshold;


            float initialqueuetimetrigger;
            if (!float.TryParse(textBoxtimetrigger.Text, out initialqueuetimetrigger))
            {
                initialqueuetimetrigger = 0;
            }
            queuetimetrigger = initialqueuetimetrigger;






            float initialqueuetimetrigger2;
            if (!float.TryParse(textBoxtimetrigger2.Text, out initialqueuetimetrigger2))
            {
                initialqueuetimetrigger2 = 0;
            }
            queuetimetriggerscan2 = initialqueuetimetrigger2;



            timer = new Timer();
            timer.Interval = TimerInterval;
            timer.Tick += Timer_Tick;
            isMenuOpen = false;
            targetHeight = 0;


        }

        private void InitializeToggleMenuButton()
        {
            toggleMenuButton = new Button();
            toggleMenuButton.Text = "▲";
            toggleMenuButton.Size = new Size(20, 20);
            toggleMenuButton.Location = new Point(1446, -3);
            toggleMenuButton.FlatStyle = FlatStyle.Flat;
            toggleMenuButton.ForeColor = Color.White;
            toggleMenuButton.BackColor = Color.FromArgb(53, 59, 72);
            toggleMenuButton.FlatAppearance.BorderSize = 0;
            toggleMenuButton.Click += ToggleMenuButton_Click;
            mainpanel.Controls.Add(toggleMenuButton);
        }

        private void ToggleMenuButton_Click(object sender, EventArgs e)
        {
            if (isMenuVisible)
            {
                panel2.Visible = false;
                mainpanel.Visible = true;
                mainpanel.Dock = DockStyle.Fill;
                toggleMenuButton.Text = "▼";
            }
            else
            {
                panel2.Visible = true;
                mainpanel.Dock = DockStyle.None;
                toggleMenuButton.Text = "▲";
            }

            isMenuVisible = !isMenuVisible;
        }

        private void InitializeDropdownPanel()
        {

            dropdownPanel1 = new Panel();
            dropdownPanel1.BackColor = Color.FromArgb(148, 145, 145);
            dropdownPanel1.Height = 0;
            dropdownPanel1.Visible = false;
            dropdownPanel1.Dock = DockStyle.Top;
            #region
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Fréquence et identification";
            groupBox.Size = new Size(280, 110);
            groupBox.Location = new Point(20, 0);

            label26 = new Label();
            label26.Size = new Size(60, 20);
            label26.Location = new Point(10, 20);
            label26.Text = "Fréquence:";

            textBoxfreqscan1 = new TextBox();
            textBoxfreqscan1.Size = new Size(120, 20);
            textBoxfreqscan1.Location = new Point(80, 20);
            textBoxfreqscan1.KeyDown += textBoxfreqscan1_KeyDown;


            buttonenvoieFreqscan1 = new Button();
            buttonenvoieFreqscan1.Size = new Size(50, 25);
            buttonenvoieFreqscan1.Location = new Point(210, 18);
            buttonenvoieFreqscan1.Text = ">>";
            buttonenvoieFreqscan1.UseVisualStyleBackColor = true;
            buttonenvoieFreqscan1.Click += buttonenvoieFreqscan1_Click;

            comboBoxledstate = new ComboBox();
            comboBoxledstate.Size = new Size(120, 20);
            comboBoxledstate.Location = new Point(10, 60);
            comboBoxledstate.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxledstate.Items.Add("Désactivé");
            comboBoxledstate.Items.Add("rouge");
            comboBoxledstate.Items.Add("vert");
            comboBoxledstate.Items.Add("orange");

            comboBoxledstate.SelectedIndex = 0;

            btnidlaser = new Button();
            btnidlaser.Size = new Size(120, 25);
            btnidlaser.Location = new Point(140, 58);
            btnidlaser.Text = "Identifier";
            btnidlaser.UseVisualStyleBackColor = true;
            btnidlaser.Click += btnidlaser_Click;





            groupBox.Controls.Add(label26);
            groupBox.Controls.Add(textBoxfreqscan1);
            groupBox.Controls.Add(buttonenvoieFreqscan1);
            groupBox.Controls.Add(btnidlaser);
            groupBox.Controls.Add(comboBoxledstate);

            dropdownPanel1.Controls.Add(groupBox);



            GroupBox groupBoxsavescan1 = new GroupBox();
            groupBoxsavescan1.Text = "Enregistrer des données";
            groupBoxsavescan1.Size = new Size(400, 110);
            groupBoxsavescan1.Location = new Point(320, 0);

            label1NumberOfProfileToSave = new Label();
            label1NumberOfProfileToSave.Size = new Size(140, 30);
            label1NumberOfProfileToSave.Location = new Point(5, 20);
            label1NumberOfProfileToSave.Text = "Nombre de profils à sauvegarder (s):";



            textBoxNumberOfProfilesToSave = new TextBox();
            textBoxNumberOfProfilesToSave.Size = new Size(120, 20);
            textBoxNumberOfProfilesToSave.Location = new Point(150, 25);
            textBoxNumberOfProfilesToSave.Text = "1";



            buttonSaveOnce = new Button();
            buttonSaveOnce.Size = new Size(98, 29);
            buttonSaveOnce.Location = new Point(280, 20);
            buttonSaveOnce.Text = "Enregistrer ...";
            buttonSaveOnce.UseVisualStyleBackColor = true;
            buttonSaveOnce.Click += buttonSaveOnce_Click;


            textBoxFileNameScan1 = new TextBox();
            textBoxFileNameScan1.Size = new Size(120, 20);
            textBoxFileNameScan1.Location = new Point(150, 60);


            label43 = new Label();
            label43.Size = new Size(140, 25);
            label43.Location = new Point(5, 60);
            label43.Text = "Nom du fichier:";


            groupBoxsavescan1.Controls.Add(label1NumberOfProfileToSave);
            groupBoxsavescan1.Controls.Add(textBoxNumberOfProfilesToSave);
            groupBoxsavescan1.Controls.Add(buttonSaveOnce);
            groupBoxsavescan1.Controls.Add(textBoxFileNameScan1);
            groupBoxsavescan1.Controls.Add(label43);



            dropdownPanel1.Controls.Add(groupBoxsavescan1);



            GroupBox groupBoxsavescan1pretrig = new GroupBox();
            groupBoxsavescan1pretrig.Text = "Pre-Trigger";
            groupBoxsavescan1pretrig.Size = new Size(400, 110);
            groupBoxsavescan1pretrig.Location = new Point(735, 0);


            checkBoxpretriggerscan1 = new CheckBox();
            checkBoxpretriggerscan1.Size = new Size(120, 20);
            checkBoxpretriggerscan1.Location = new Point(150, 15);
            checkBoxpretriggerscan1.Text = "Pre-Trigger";


            textBoxtimetrigger = new TextBox();
            textBoxtimetrigger.Size = new Size(120, 20);
            textBoxtimetrigger.Location = new Point(160, 38);
            textBoxtimetrigger.Text = "1";
            textBoxtimetrigger.TextChanged += TextBoxTextChanged;


            textboxafterpretriggerscan1 = new TextBox();
            textboxafterpretriggerscan1.Size = new Size(120, 20);
            textboxafterpretriggerscan1.Location = new Point(160, 68);


            button7 = new Button();
            button7.Size = new Size(100, 34);
            button7.Location = new Point(290, 50);
            button7.Text = "Attendre le déclenchement";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;

            label47 = new Label();
            label47.Size = new Size(147, 29);
            label47.Location = new Point(10, 40);
            label47.Text = "Temps de Pre-Trigger (s) :";


            label48 = new Label();
            label48.Size = new Size(160, 29);
            label48.Location = new Point(10, 70);
            label48.Text = "Temps après le Pre-Trigger(s):";

            startpretrig2 = new Label();
            startpretrig2.Size = new Size(147, 29);
            startpretrig2.Location = new Point(10, 15);
            startpretrig2.Text = "Démarrer le Pre-Trigger :";


            groupBoxsavescan1pretrig.Controls.Add(checkBoxpretriggerscan1);
            groupBoxsavescan1pretrig.Controls.Add(textBoxtimetrigger);
            groupBoxsavescan1pretrig.Controls.Add(textboxafterpretriggerscan1);
            groupBoxsavescan1pretrig.Controls.Add(button7);
            groupBoxsavescan1pretrig.Controls.Add(label47);
            groupBoxsavescan1pretrig.Controls.Add(label48);
            groupBoxsavescan1pretrig.Controls.Add(startpretrig2);
            dropdownPanel1.Controls.Add(groupBoxsavescan1pretrig);

            GroupBox groupaffichagefreqfifo = new GroupBox();
            groupaffichagefreqfifo.Text = "Affichage";
            groupaffichagefreqfifo.Size = new Size(300, 110);
            groupaffichagefreqfifo.Location = new Point(1150, 0);


            textBoxFrequency = new TextBox();
            textBoxFrequency.Size = new Size(184, 20);
            textBoxFrequency.Location = new Point(10, 15);
            textBoxFrequency.Text = "...";

            textBoxFIFO = new TextBox();
            textBoxFIFO.Size = new Size(184, 20);
            textBoxFIFO.Location = new Point(10, 35);
            textBoxFIFO.Text = "...";

            buttonResetStatistic = new Button();
            buttonResetStatistic.Size = new Size(96, 20);
            buttonResetStatistic.Location = new Point(200, 14);
            buttonResetStatistic.Text = "Reset Statistic";
            buttonResetStatistic.UseVisualStyleBackColor = true;
            buttonResetStatistic.Click += buttonResetStatistic_Click;



            buttonDllFiFoReset = new Button();
            buttonDllFiFoReset.Size = new Size(96, 20);
            buttonDllFiFoReset.Location = new Point(200, 34);
            buttonDllFiFoReset.Text = "DLL-FiFo-Reset";
            buttonDllFiFoReset.UseVisualStyleBackColor = true;
            buttonDllFiFoReset.Click += buttonDllFiFoReset_Click;


            progressbartrigsca1 = new ProgressBar();
            progressbartrigsca1.Size = new Size(180, 23);
            progressbartrigsca1.Location = new Point(100, 60);

            textboxstatusscan1trigger = new TextBox();
            textboxstatusscan1trigger.Size = new Size(180, 23);
            textboxstatusscan1trigger.Location = new Point(100, 86);
            textboxstatusscan1trigger.Text = "Statut de Pre-Trigger ";

            pretriggerstatusforscan1 = new Label();
            pretriggerstatusforscan1.Size = new Size(69, 30);
            pretriggerstatusforscan1.Location = new Point(10, 70);
            pretriggerstatusforscan1.Text = "Statut de Pre-Trigger :";


            groupaffichagefreqfifo.Controls.Add(textBoxFrequency);
            groupaffichagefreqfifo.Controls.Add(textBoxFIFO);
            groupaffichagefreqfifo.Controls.Add(buttonResetStatistic);
            groupaffichagefreqfifo.Controls.Add(buttonDllFiFoReset);
            groupaffichagefreqfifo.Controls.Add(progressbartrigsca1);
            groupaffichagefreqfifo.Controls.Add(textboxstatusscan1trigger);
            groupaffichagefreqfifo.Controls.Add(pretriggerstatusforscan1);

            dropdownPanel1.Controls.Add(groupaffichagefreqfifo);



            panel2.Controls.Add(dropdownPanel1);
            #endregion


            dropdownPanel2 = new Panel();
            dropdownPanel2.BackColor = Color.FromArgb(148, 145, 145);
            dropdownPanel2.Height = 0;
            dropdownPanel2.Visible = false;
            dropdownPanel2.Dock = DockStyle.Top;
            #region
            GroupBox groupBoxscan2 = new GroupBox();
            groupBoxscan2.Text = "Fréquence et identification";
            groupBoxscan2.Size = new Size(280, 110);
            groupBoxscan2.Location = new Point(20, 0);

            label25 = new Label();
            label25.Size = new Size(60, 20);
            label25.Location = new Point(10, 20);
            label25.Text = "Fréquence:";


            textBoxfreqscan2 = new TextBox();
            textBoxfreqscan2.Size = new Size(120, 20);
            textBoxfreqscan2.Location = new Point(80, 20);
            textBoxfreqscan2.KeyDown += textBoxfreqscan2_KeyDown;

            buttonenvoieFreqscan2 = new Button();
            buttonenvoieFreqscan2.Size = new Size(50, 25);
            buttonenvoieFreqscan2.Location = new Point(210, 18);
            buttonenvoieFreqscan2.Text = ">>";
            buttonenvoieFreqscan2.UseVisualStyleBackColor = true;
            buttonenvoieFreqscan2.Click += buttonenvoieFreqscan2_Click;

            comboBoxledstate2 = new ComboBox();
            comboBoxledstate2.Size = new Size(120, 20);
            comboBoxledstate2.Location = new Point(10, 60);
            comboBoxledstate2.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxledstate2.Items.Add("Désactivé");
            comboBoxledstate2.Items.Add("rouge");
            comboBoxledstate2.Items.Add("vert");
            comboBoxledstate2.Items.Add("orange");

            comboBoxledstate2.SelectedIndex = 0;

            btnidlaser2 = new Button();
            btnidlaser2.Size = new Size(120, 25);
            btnidlaser2.Location = new Point(140, 58);
            btnidlaser2.Text = "Identifier";
            btnidlaser2.UseVisualStyleBackColor = true;
            btnidlaser2.Click += btnidlaser2_Click;

            groupBoxscan2.Controls.Add(label25);
            groupBoxscan2.Controls.Add(textBoxfreqscan2);
            groupBoxscan2.Controls.Add(buttonenvoieFreqscan2);
            groupBoxscan2.Controls.Add(comboBoxledstate2);
            groupBoxscan2.Controls.Add(btnidlaser2);

            dropdownPanel2.Controls.Add(groupBoxscan2);



            GroupBox groupBoxsavescan2 = new GroupBox();
            groupBoxsavescan2.Text = "Enregistrer des données";
            groupBoxsavescan2.Size = new Size(400, 110);
            groupBoxsavescan2.Location = new Point(320, 0);


            textBoxNumberOfProfilesToSave2 = new TextBox();
            textBoxNumberOfProfilesToSave2.Size = new Size(120, 20);
            textBoxNumberOfProfilesToSave2.Location = new Point(150, 25);
            textBoxNumberOfProfilesToSave2.Text = "1";



            buttonSaveOnce2 = new Button();
            buttonSaveOnce2.Size = new Size(98, 29);
            buttonSaveOnce2.Location = new Point(280, 20);
            buttonSaveOnce2.Text = "Enregistrer ...";
            buttonSaveOnce2.UseVisualStyleBackColor = true;
            buttonSaveOnce2.Click += buttonSaveOnce2_Click;

            textBoxFileNameScan2 = new TextBox();
            textBoxFileNameScan2.Size = new Size(120, 20);
            textBoxFileNameScan2.Location = new Point(150, 60);


            label10 = new Label();
            label10.Size = new Size(140, 30);
            label10.Location = new Point(5, 20);
            label10.Text = "Nombre de profils à sauvegarder (s):";

            label39 = new Label();
            label39.Size = new Size(140, 25);
            label39.Location = new Point(5, 60);
            label39.Text = "Nom du fichier:";


            groupBoxsavescan2.Controls.Add(textBoxNumberOfProfilesToSave2);
            groupBoxsavescan2.Controls.Add(buttonSaveOnce2);
            groupBoxsavescan2.Controls.Add(textBoxFileNameScan2);
            groupBoxsavescan2.Controls.Add(label10);
            groupBoxsavescan2.Controls.Add(label39);

            dropdownPanel2.Controls.Add(groupBoxsavescan2);




            GroupBox groupBoxsavescan2pretrig = new GroupBox();
            groupBoxsavescan2pretrig.Text = "Pre-Trigger";
            groupBoxsavescan2pretrig.Size = new Size(400, 110);
            groupBoxsavescan2pretrig.Location = new Point(735, 0);

            checkBoxpretriggerscan2 = new CheckBox();
            checkBoxpretriggerscan2.Size = new Size(120, 20);
            checkBoxpretriggerscan2.Location = new Point(150, 15);
            checkBoxpretriggerscan2.Text = "Pre-Trigger";

            textBoxtimetrigger2 = new TextBox();
            textBoxtimetrigger2.Size = new Size(120, 20);
            textBoxtimetrigger2.Location = new Point(160, 38);
            textBoxtimetrigger2.Text = "1";
            textBoxtimetrigger2.TextChanged += TextBox2TextChanged;

            textboxafterpretriggerscan2 = new TextBox();
            textboxafterpretriggerscan2.Size = new Size(120, 20);
            textboxafterpretriggerscan2.Location = new Point(160, 68);


            button9 = new Button();
            button9.Size = new Size(100, 34);
            button9.Location = new Point(290, 50);
            button9.Text = "Attendre le déclenchement";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;

            startpretrig1 = new Label();
            startpretrig1.Size = new Size(147, 29);
            startpretrig1.Location = new Point(10, 15);
            startpretrig1.Text = "Démarrer le Pre-Trigger :";

            label49 = new Label();
            label49.Size = new Size(147, 29);
            label49.Location = new Point(10, 40);
            label49.Text = "Temps de Pre-Trigger(s) :";


            label51 = new Label();
            label51.Size = new Size(160, 29);
            label51.Location = new Point(10, 70);
            label51.Text = "Temps après le Pre-Trigger(s):";

            groupBoxsavescan2pretrig.Controls.Add(checkBoxpretriggerscan2);
            groupBoxsavescan2pretrig.Controls.Add(textBoxtimetrigger2);
            groupBoxsavescan2pretrig.Controls.Add(textboxafterpretriggerscan2);
            groupBoxsavescan2pretrig.Controls.Add(button9);
            groupBoxsavescan2pretrig.Controls.Add(label49);
            groupBoxsavescan2pretrig.Controls.Add(label51);
            groupBoxsavescan2pretrig.Controls.Add(startpretrig1);
            dropdownPanel2.Controls.Add(groupBoxsavescan2pretrig);



            GroupBox groupaffichagefreqfifo2 = new GroupBox();
            groupaffichagefreqfifo2.Text = "Affichage Fréquence et FIFO";
            groupaffichagefreqfifo2.Size = new Size(300, 110);
            groupaffichagefreqfifo2.Location = new Point(1150, 0);


            textBoxFrequency2 = new TextBox();
            textBoxFrequency2.Size = new Size(184, 20);
            textBoxFrequency2.Location = new Point(10, 15);
            textBoxFrequency2.Text = "...";

            textBoxFIFO2 = new TextBox();
            textBoxFIFO2.Size = new Size(184, 20);
            textBoxFIFO2.Location = new Point(10, 35);
            textBoxFIFO2.Text = "...";


            buttonResetStatistic2 = new Button();
            buttonResetStatistic2.Size = new Size(96, 20);
            buttonResetStatistic2.Location = new Point(200, 14);
            buttonResetStatistic2.Text = "Reset Statistic";
            buttonResetStatistic2.UseVisualStyleBackColor = true;
            buttonResetStatistic2.Click += buttonResetStatistic2_Click;

            buttonDllFiFoReset2 = new Button();
            buttonDllFiFoReset2.Size = new Size(96, 20);
            buttonDllFiFoReset2.Location = new Point(200, 34);
            buttonDllFiFoReset2.Text = "DLL-FiFo-Reset";
            buttonDllFiFoReset2.UseVisualStyleBackColor = true;
            buttonDllFiFoReset2.Click += buttonDllFiFoReset2_Click;


            progressbartrigsca2 = new ProgressBar();
            progressbartrigsca2.Size = new Size(180, 23);
            progressbartrigsca2.Location = new Point(100, 60);

            textboxstatusscan1trigger2 = new TextBox();
            textboxstatusscan1trigger2.Size = new Size(180, 23);
            textboxstatusscan1trigger2.Location = new Point(100, 86);
            textboxstatusscan1trigger2.Text = "Statut de Pre-Trigger ";



            pretriggerstatusforscan2 = new Label();
            pretriggerstatusforscan2.Size = new Size(69, 30);
            pretriggerstatusforscan2.Location = new Point(10, 70);
            pretriggerstatusforscan2.Text = "Statut de Pre-Trigger :";





            groupaffichagefreqfifo2.Controls.Add(buttonDllFiFoReset2);

            groupaffichagefreqfifo2.Controls.Add(buttonResetStatistic2);
            groupaffichagefreqfifo2.Controls.Add(textBoxFIFO2);
            groupaffichagefreqfifo2.Controls.Add(textBoxFrequency2);


            groupaffichagefreqfifo2.Controls.Add(progressbartrigsca2);
            groupaffichagefreqfifo2.Controls.Add(textboxstatusscan1trigger2);
            groupaffichagefreqfifo2.Controls.Add(pretriggerstatusforscan2);


            dropdownPanel2.Controls.Add(groupaffichagefreqfifo2);



            panel2.Controls.Add(dropdownPanel2);

            #endregion


            dropdownPanel3 = new Panel();
            dropdownPanel3.BackColor = Color.FromArgb(148, 145, 145);
            dropdownPanel3.Height = 0;
            dropdownPanel3.Visible = false;
            dropdownPanel3.Dock = DockStyle.Top;
            #region

            GroupBox groupfreq = new GroupBox();
            groupfreq.Text = "Fréquence";
            groupfreq.Size = new Size(150, 110);
            groupfreq.Location = new Point(10, 0);

            textBoxfreq = new TextBox();
            textBoxfreq.Size = new Size(90, 20);
            textBoxfreq.Location = new Point(10, 40);
            textBoxfreq.KeyDown += textBoxfreq_KeyDown;

            buttonenvoieFreq = new Button();
            buttonenvoieFreq.Size = new Size(29, 23);
            buttonenvoieFreq.Location = new Point(110, 38);
            buttonenvoieFreq.Text = ">>";
            buttonenvoieFreq.UseVisualStyleBackColor = true;
            buttonenvoieFreq.Click += buttonenvoieFreq_Click;

            freqlabel = new Label();
            freqlabel.Text = "Fréquence :";
            freqlabel.Size = new Size(120, 20);
            freqlabel.Location = new Point(10, 15);

            groupfreq.Controls.Add(freqlabel);
            groupfreq.Controls.Add(textBoxfreq);
            groupfreq.Controls.Add(buttonenvoieFreq);

            dropdownPanel3.Controls.Add(groupfreq);

            GroupBox groupBoxfeqandsave = new GroupBox();
            groupBoxfeqandsave.Text = "Enregistrer des données";
            groupBoxfeqandsave.Size = new Size(420, 110);
            groupBoxfeqandsave.Location = new Point(180, 0);

            textBox3 = new TextBox();
            textBox3.Size = new Size(120, 20);
            textBox3.Location = new Point(150, 15);

            button4 = new Button();
            button4.Size = new Size(98, 29);
            button4.Location = new Point(280, 10);
            button4.Text = "Enregistrer ...";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;

            textBoxFileNameScan3 = new TextBox();
            textBoxFileNameScan3.Size = new Size(120, 20);
            textBoxFileNameScan3.Location = new Point(150, 50);

            sepfichiers = new CheckBox();
            sepfichiers.Size = new Size(128, 17);
            sepfichiers.Location = new Point(280, 50);
            sepfichiers.Text = "Séparation de fichiers";


            label33 = new Label();
            label33.Size = new Size(132, 39);
            label33.Location = new Point(10, 20);
            label33.Text = "Temps à sauvegarder en seconde:";

            label27 = new Label();
            label27.Size = new Size(132, 39);
            label27.Location = new Point(10, 56);
            label27.Text = "Nom du fichier:";


            groupBoxfeqandsave.Controls.Add(textBox3);
            groupBoxfeqandsave.Controls.Add(button4);
            groupBoxfeqandsave.Controls.Add(textBoxFileNameScan3);
            groupBoxfeqandsave.Controls.Add(sepfichiers);
            groupBoxfeqandsave.Controls.Add(label33);
            groupBoxfeqandsave.Controls.Add(label27);

            dropdownPanel3.Controls.Add(groupBoxfeqandsave);


            GroupBox grouppretrigdoublescan = new GroupBox();
            grouppretrigdoublescan.Text = "Pre-Trigger";
            grouppretrigdoublescan.Size = new Size(450, 110);
            grouppretrigdoublescan.Location = new Point(620, 0);

            savepretrigger = new CheckBox();
            savepretrigger.Size = new Size(120, 20);
            savepretrigger.Location = new Point(150, 15);
            savepretrigger.Text = "Pre-Trigger";




            queueLengthTextBox = new TextBox();
            queueLengthTextBox.Size = new Size(120, 20);
            queueLengthTextBox.Location = new Point(160, 42);
            queueLengthTextBox.Text = "1";
            queueLengthTextBox.TextChanged += QueueLengthTextBox_TextChanged;


            textboxafterpretrigger = new TextBox();
            textboxafterpretrigger.Size = new Size(120, 20);
            textboxafterpretrigger.Location = new Point(160, 70);


            /*btnvaliderpretrigger = new Button();
            btnvaliderpretrigger.Size = new Size(98, 29);
            btnvaliderpretrigger.Location = new Point(160, 70);
            btnvaliderpretrigger.Text = "Valider";
            btnvaliderpretrigger.UseVisualStyleBackColor = true;
            btnvaliderpretrigger.Click += btnvaliderpretrigger_Click;*/

            labelpretrigstatu = new Label();
            labelpretrigstatu.Size = new Size(140, 20);
            labelpretrigstatu.Location = new Point(10, 44);
            labelpretrigstatu.Text = "Temps de Pre-Trigger(s) :";


            textboxlimitaftertrig = new Label();
            textboxlimitaftertrig.Size = new Size(155, 29);
            textboxlimitaftertrig.Location = new Point(10, 72);
            textboxlimitaftertrig.Text = "Temps Après le Pre-Trigger(s) :";

            labelstartpretrig = new Label();
            labelstartpretrig.Size = new Size(147, 29);
            labelstartpretrig.Location = new Point(10, 15);
            labelstartpretrig.Text = "Démarrage du Pre-Trigger:";



            brnatttriger = new Button();
            brnatttriger.Size = new Size(100, 34);
            brnatttriger.Location = new Point(320, 45);
            brnatttriger.Text = "Attendre le déclenchement";
            brnatttriger.UseVisualStyleBackColor = true;
            brnatttriger.Click += brnatttriger_Click;

            labelatttrigdouble = new Label();
            labelatttrigdouble.Size = new Size(147, 29);
            labelatttrigdouble.Location = new Point(295, 10);
            labelatttrigdouble.Text = "Création du fichier et attente du déclenchement :";

            seppretrigfichiers = new CheckBox();
            seppretrigfichiers.Size = new Size(128, 17);
            seppretrigfichiers.Location = new Point(300, 85);
            seppretrigfichiers.Text = "Séparation de fichiers";


            grouppretrigdoublescan.Controls.Add(savepretrigger);
            grouppretrigdoublescan.Controls.Add(queueLengthTextBox);
            grouppretrigdoublescan.Controls.Add(textboxafterpretrigger);
            grouppretrigdoublescan.Controls.Add(brnatttriger);
            grouppretrigdoublescan.Controls.Add(labelpretrigstatu);
            grouppretrigdoublescan.Controls.Add(labelstartpretrig);
            grouppretrigdoublescan.Controls.Add(labelatttrigdouble);
            grouppretrigdoublescan.Controls.Add(seppretrigfichiers);
            grouppretrigdoublescan.Controls.Add(textboxlimitaftertrig);


            dropdownPanel3.Controls.Add(grouppretrigdoublescan);

            GroupBox groupboxlables = new GroupBox();
            groupboxlables.Text = "File d'attente (Affichage)";
            groupboxlables.Size = new Size(350, 110);
            groupboxlables.Location = new Point(1100, 0);


            int horizontalSpacing = 30;

            label4 = new Label();
            label4.Size = new Size(40, 20);
            label4.Location = new Point(20, 15);
            label4.Text = "X1";

            label7 = new Label();
            label7.Size = new Size(40, 20);
            label7.Location = new Point(label4.Right + horizontalSpacing, 15);
            label7.Text = "Z1";

            label18 = new Label();
            label18.Size = new Size(40, 20);
            label18.Location = new Point(label7.Right + horizontalSpacing, 15);
            label18.Text = "I1";

            label19 = new Label();
            label19.Size = new Size(40, 20);
            label19.Location = new Point(20, 40);
            label19.Text = "X2";

            label20 = new Label();
            label20.Size = new Size(40, 20);
            label20.Location = new Point(label19.Right + horizontalSpacing, 40);
            label20.Text = "Z2";

            label21 = new Label();
            label21.Size = new Size(40, 20);
            label21.Location = new Point(label20.Right + horizontalSpacing, 40);
            label21.Text = "I2";


            progressbartrig = new ProgressBar();
            progressbartrig.Size = new Size(180, 23);
            progressbartrig.Location = new Point(150, 60);

            pretriggerlabelstat = new Label();
            pretriggerlabelstat.Size = new Size(120, 17);
            pretriggerlabelstat.Location = new Point(10, 75);
            pretriggerlabelstat.Text = "Statut de Pre-Trigger :";

            button2 = new Button();
            button2.Size = new Size(80, 40);
            button2.Location = new Point(230, 15);
            button2.Text = "Effacer la file d'attente";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;

            text_pretrig2scan = new TextBox();
            text_pretrig2scan.Size = new Size(180, 23);
            text_pretrig2scan.Location = new Point(150, 85);


            groupboxlables.Controls.Add(label4);
            groupboxlables.Controls.Add(label7);
            groupboxlables.Controls.Add(label18);
            groupboxlables.Controls.Add(label19);
            groupboxlables.Controls.Add(label20);
            groupboxlables.Controls.Add(label21);
            groupboxlables.Controls.Add(progressbartrig);
            groupboxlables.Controls.Add(pretriggerlabelstat);
            groupboxlables.Controls.Add(button2);
            groupboxlables.Controls.Add(text_pretrig2scan);

            dropdownPanel3.Controls.Add(groupboxlables);
            #endregion

            panel2.Controls.Add(dropdownPanel3);
        }


        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxtimetrigger.Text))
                    return;

                waitenqueupretrig1 = true;
                while (tempDoZ1.TryDequeue(out _)) ;
                while (tempDoX1.TryDequeue(out _)) ;
                while (tempDoI1.TryDequeue(out _)) ;
                progressbartrigsca1.Invoke((MethodInvoker)delegate { progressbartrigsca1.Value = 0; });
                textboxstatusscan1trigger.Text = "Attendre pré trigger";

                string input = textBoxtimetrigger.Text.Replace(",", ".");
                if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float newQueuetime) && newQueuetime > 0)
                {
                    queuetimetrigger = newQueuetime;
                    waitenqueupretrig1 = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void TextBox2TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxtimetrigger2.Text))
                    return;

                waitenqueupretrig2 = true;
                while (tempDoZ2.TryDequeue(out _)) ;
                while (tempDoX2.TryDequeue(out _)) ;
                while (tempDoI2.TryDequeue(out _)) ;
                progressbartrigsca2.Invoke((MethodInvoker)delegate { progressbartrigsca2.Value = 0; });
                textboxstatusscan1trigger2.Text = "Attendre pré trigger";

                float newQueuetime;
                if (float.TryParse(textBoxtimetrigger2.Text, out newQueuetime) && newQueuetime > 0)
                {
                    queuetimetriggerscan2 = newQueuetime;
                    waitenqueupretrig2 = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }
        private void QueueLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string input = queueLengthTextBox.Text.Trim();

                if (string.IsNullOrEmpty(input))
                    return;

                input = input.Replace(',', '.');

                waitenqueu = true;
                while (tempDoZ1.TryDequeue(out _)) ;
                while (tempDoX1.TryDequeue(out _)) ;
                while (tempDoI1.TryDequeue(out _)) ;
                while (tempDoZ2.TryDequeue(out _)) ;
                while (tempDoX2.TryDequeue(out _)) ;
                while (tempDoI2.TryDequeue(out _)) ;
                text_pretrig2scan.Text = "Attendre pré trigger";
                progressbartrig.Invoke((MethodInvoker)delegate { progressbartrig.Value = 0; });

                if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float newQueuetime))
                {
                    queueLengthThreshold = newQueuetime;
                    waitenqueu = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void OpenMenu(Panel dropdownPanel)
        {
            if (!isMenuOpen)
            {
                isMenuOpen = true;

                // Hide controls in panel2
                foreach (Control control in panel2.Controls)
                {
                    control.Visible = false;
                }

                // Determine the combined height of the open menus
                int totalHeight = dropdownPanel1.PreferredSize.Height + dropdownPanel2.PreferredSize.Height + dropdownPanel3.PreferredSize.Height;

                dropdownPanel.Visible = true;
                dropdownPanel.Size = new Size(panel2.ClientSize.Width, 0);
                targetHeight = totalHeight;

                Point mainButtonLocation = btnmenucpt1save.PointToScreen(Point.Empty);
                Point dropdownLocation = panel2.PointToClient(mainButtonLocation);
                dropdownPanel.Location = new Point(dropdownLocation.X, dropdownLocation.Y + btnmenucpt1save.Height);

                timer.Start();
            }
        }

        private void CloseMenu()
        {
            if (isMenuOpen)
            {
                isMenuOpen = false;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int currentHeight = dropdownPanel1.Height;
            bool isClosing = !isMenuOpen;

            if (isClosing)
            {
                if (currentHeight <= 0)
                {
                    timer.Stop();
                    dropdownPanel1.Visible = false;
                    dropdownPanel2.Visible = false;
                    dropdownPanel3.Visible = false;
                    RestorePanel2ControlsVisibility();
                }
                else
                {
                    dropdownPanel1.Height = Math.Max(0, currentHeight - AnimationStep);
                    dropdownPanel2.Height = Math.Max(0, currentHeight - AnimationStep);
                    dropdownPanel3.Height = Math.Max(0, currentHeight - AnimationStep);
                }
            }
            else
            {
                if (currentHeight >= targetHeight)
                {
                    timer.Stop();
                    dropdownPanel1.Height = targetHeight;
                    dropdownPanel2.Height = targetHeight;
                    dropdownPanel3.Height = targetHeight;
                }
                else
                {
                    dropdownPanel1.Height = Math.Min(targetHeight, currentHeight + AnimationStep);
                    dropdownPanel2.Height = Math.Min(targetHeight, currentHeight + AnimationStep);
                    dropdownPanel3.Height = Math.Min(targetHeight, currentHeight + AnimationStep);
                }
            }
        }

        private void RestorePanel2ControlsVisibility()

        {
            foreach (Control control in panel2.Controls)
            {
                control.Visible = true;
            }
        }

        private void btnmenucpt1save_Click(object sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                if (dropdownPanel1.Visible)
                {
                    CloseMenu();
                }
                else
                {
                    CloseMenu();
                    OpenMenu(dropdownPanel1);
                }
            }
            else
            {
                OpenMenu(dropdownPanel1);
            }
        }
        private void btnmenucpt2save_Click(object sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                if (dropdownPanel2.Visible)
                {
                    CloseMenu();
                }
                else
                {
                    CloseMenu();
                    OpenMenu(dropdownPanel2);
                }
            }
            else
            {
                OpenMenu(dropdownPanel2);
            }
        }
        private void doublecptpara_Click(object sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                if (dropdownPanel3.Visible)
                {
                    CloseMenu();
                }
                else
                {
                    CloseMenu();
                    OpenMenu(dropdownPanel3);
                }
            }
            else
            {
                OpenMenu(dropdownPanel3);
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CLOSE)
            {
                DialogResult result = MessageBox.Show("Voulez-vous fermer l'application ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Environment.Exit(0);
            }

        }



        private void btnidlaser_Click(object sender, EventArgs e)
        {

            int selectedIndex = comboBoxledstate.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 0;
                    break;
                case 1:
                    valueToSend = 1;
                    break;
                case 2:
                    valueToSend = 2;
                    break;
                case 3:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetUserLED=" + valueToSend.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);

        }


        private void btnidlaser2_Click(object sender, EventArgs e)
        {
            int selectedIndex2 = comboBoxledstate2.SelectedIndex;
            if (selectedIndex2 == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex2)
            {
                case 0:
                    valueToSend = 0;
                    break;
                case 1:
                    valueToSend = 1;
                    break;
                case 2:
                    valueToSend = 2;
                    break;
                case 3:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetUserLED=" + valueToSend.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);
        }

        int flagled;
        int flagled2;

        private void ShowScanXZI(float roiStart, float roiEnd)
        {
            try
            {
                double faktorX = 0;
                double faktorZ = 0;
                double faktorI = 0;

                int dwPixelSize = 1;
                int gridSizeZ = 50;
                double minZscan1 = double.MaxValue;
                double maxZscan1 = double.MinValue;

                Bitmap bitmapScan1 = new Bitmap(m_pictureBoxScanView.Width, m_pictureBoxScanView.Height);

                using (Graphics g1 = Graphics.FromImage(bitmapScan1))
                {
                    g1.FillRectangle(Brushes.Black, 0, 0, bitmapScan1.Width, bitmapScan1.Height);

                    faktorX = (double)(bitmapScan1.Width - dwPixelSize) / (double)m_CScanView1_X_Range_At_End;
                    faktorZ = (double)(bitmapScan1.Height - dwPixelSize) / (double)m_CScanView1_Z_Range;
                    faktorI = (double)(bitmapScan1.Height - dwPixelSize) / (double)1024;

                    for (int i = gridSizeZ; i <= m_CScanView1_Z_Range; i += gridSizeZ)
                    {
                        int z = (int)(faktorZ * (double)(i - m_CScanView1_Z_Start));
                        if (z >= bitmapScan1.Height)
                        {
                            break;
                        }

                        g1.DrawLine(Pens.Blue, 0, z, (int)(0.1 * bitmapScan1.Width), z);

                        g1.DrawString(i.ToString(), DefaultFont, Brushes.White, 0, z);
                    }

                    if (true)
                    {
                        for (int i = 0; i < m_iScannerDataLen; i++)
                        {
                            int x;
                            if (inverse_scan1.Checked)
                            {
                                x = (int)(faktorX * (double)(-m_doX[i] + m_CScanView1_X_Range_At_End / 2));
                            }
                            else
                            {
                                x = (int)(faktorX * (double)(m_doX[i] + m_CScanView1_X_Range_At_End / 2));
                            }

                            if ((int)(m_doZ[i] - m_CScanView1_Z_Start) > 0)
                            {
                                int z = (int)(faktorZ * (double)(m_doZ[i] - m_CScanView1_Z_Start));
                                if (z >= bitmapScan1.Height)
                                {
                                    z = bitmapScan1.Height - 1;
                                }
                                else if (z < 0)
                                {
                                    z = 0;
                                }
                                if (x >= 0 && x < bitmapScan1.Width)
                                {
                                    bitmapScan1.SetPixel(x, z, Color.White);
                                }




                                if (m_doZ[i] < minZscan1)
                                {
                                    minZscan1 = m_doZ[i];
                                }

                                if (m_doZ[i] > maxZscan1)
                                {
                                    maxZscan1 = m_doZ[i];
                                }

                            }
                        }




                    }

                    if (true)
                    {
                        for (int i = 0; i < m_iScannerDataLen; i++)
                        {
                            int x = (int)(faktorX * (double)(m_doX[i] + m_CScanView1_X_Range_At_End / 2));
                            int intensity = (int)(faktorI * (1024 - (double)m_iIntensity[i]));
                            if (intensity >= 0 && intensity < bitmapScan1.Height && x >= 0 && x < bitmapScan1.Width)
                            {
                                bitmapScan1.SetPixel(x, intensity, Color.Yellow);
                            }
                        }
                    }

                    int zStart = (int)(faktorZ * (double)(roiStart - m_CScanView1_Z_Start));
                    int zEnd = (int)(faktorZ * (double)(roiEnd - m_CScanView1_Z_Start));
                    if (roilimit.Checked)
                    {
                        g1.DrawLine(Pens.Red, 0, zStart, bitmapScan1.Width, zStart);
                        g1.DrawLine(Pens.Red, 0, zEnd, bitmapScan1.Width, zEnd);
                    }

                    if (minZscan1 > 600 || minZscan1 < 0)
                    {


                        flagled = 1;
                    }
                    else if (maxZscan1 > 600 || maxZscan1 < 0)
                    {

                        flagled = 1;
                    }
                    else
                    {
                        flagled = 2;


                    }
                }


                m_pictureBoxScanView.Invoke((MethodInvoker)delegate
                {
                    m_pictureBoxScanView.Image = bitmapScan1;
                    m_pictureBoxScanView.Invalidate();
                });

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error during scan 1: " + ex.Message);
                MessageBox.Show("Erreur lors de Scan 1: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void ShowScanXZI2(float roiStart2, float roiEnd2)
        {
            try
            {
                double faktorX2 = 0;
                double faktorZ2 = 0;
                double faktorI2 = 0;

                int dwPixelSize = 1;
                int gridSizeZ = 50;

                double minZ2scan2 = double.MaxValue;
                double maxZ2scan2 = double.MinValue;
                Bitmap bitmapScan2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                using (Graphics g2 = Graphics.FromImage(bitmapScan2))
                {
                    g2.FillRectangle(Brushes.Black, 0, 0, bitmapScan2.Width, bitmapScan2.Height);

                    faktorX2 = (double)(bitmapScan2.Width - dwPixelSize) / (double)m_CScanView1_X_Range_At_End;
                    faktorZ2 = (double)(bitmapScan2.Height - dwPixelSize) / (double)m_CScanView1_Z_Range;
                    faktorI2 = (double)(bitmapScan2.Height - dwPixelSize) / (double)1024;


                    for (int i = gridSizeZ; i <= m_CScanView1_Z_Range; i += gridSizeZ)
                    {
                        int z = (int)(faktorZ2 * (double)(i - m_CScanView1_Z_Start));
                        if (z >= bitmapScan2.Height)
                        {
                            break;
                        }

                        g2.DrawLine(Pens.Blue, 0, z, (int)(0.1 * bitmapScan2.Width), z);

                        g2.DrawString(i.ToString(), DefaultFont, Brushes.White, 0, z);
                    }

                    if (true)
                    {
                        for (int i = 0; i < m_iScannerDataLen2; i++)
                        {
                            int x;
                            if (inverse_scan2.Checked)
                            {
                                x = (int)(faktorX2 * (double)(-m_doX2[i] + m_CScanView1_X_Range_At_End / 2));
                            }
                            else
                            {
                                x = (int)(faktorX2 * (double)(m_doX2[i] + m_CScanView1_X_Range_At_End / 2));
                            }

                            if ((int)(m_doZ2[i] - m_CScanView1_Z_Start) > 0)
                            {
                                int z = (int)(faktorZ2 * (double)(m_doZ2[i] - m_CScanView1_Z_Start));
                                if (z >= bitmapScan2.Height)
                                {
                                    z = bitmapScan2.Height - 1;
                                }
                                else if (z < 0)
                                {
                                    z = 0;
                                }
                                if (x >= 0 && x < bitmapScan2.Width)
                                {
                                    bitmapScan2.SetPixel(x, z, Color.DeepSkyBlue);
                                }
                                if (m_doZ2[i] < minZ2scan2)
                                {
                                    minZ2scan2 = m_doZ2[i];
                                }
                                if (m_doZ2[i] > maxZ2scan2)
                                {
                                    maxZ2scan2 = m_doZ2[i];
                                }
                            }
                        }
                    }

                    if (true)
                    {
                        for (int i = 0; i < m_iScannerDataLen2; i++)
                        {
                            int x = (int)(faktorX2 * (double)(m_doX2[i] + m_CScanView1_X_Range_At_End / 2));
                            int intensity = (int)(faktorI2 * (1024 - (double)m_iIntensity2[i]));
                            if (intensity >= 0 && intensity < bitmapScan2.Height && x >= 0 && x < bitmapScan2.Width)
                            {
                                bitmapScan2.SetPixel(x, intensity, Color.Yellow);
                            }
                        }
                    }

                    int zStart = (int)(faktorZ2 * (double)(roiStart2 - m_CScanView1_Z_Start));
                    int zEnd = (int)(faktorZ2 * (double)(roiEnd2 - m_CScanView1_Z_Start));
                    if (roilimit2.Checked)
                    {
                        g2.DrawLine(Pens.Red, 0, zStart, bitmapScan2.Width, zStart);
                        g2.DrawLine(Pens.Red, 0, zEnd, bitmapScan2.Width, zEnd);
                    }

                    if (minZ2scan2 > 600 || minZ2scan2 < 0)
                    {


                        flagled2 = 1;
                    }
                    else if (maxZ2scan2 > 600 || maxZ2scan2 < 0)
                    {

                        flagled2 = 1;
                    }
                    else
                    {
                        flagled2 = 2;


                    }

                }

                pictureBox1.Invoke((MethodInvoker)delegate
                {
                    pictureBox1.Image = bitmapScan2;
                    pictureBox1.Invalidate();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de Scan 2: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowScanXZI3()
        {
            double faktorX = 0;
            double faktorZ = 0;
            double faktorI = 0;

            int dwPixelSize = 1;

            Bitmap bitmapScan3 = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            using (Graphics g3 = Graphics.FromImage(bitmapScan3))
            {
                g3.FillRectangle(Brushes.Black, 0, 0, bitmapScan3.Width, bitmapScan3.Height);

                faktorX = (double)(bitmapScan3.Width - dwPixelSize) / (double)m_CScanView1_X_Range_At_End;
                faktorX = faktorX / 2;
                faktorZ = (double)(bitmapScan3.Height - dwPixelSize) / (double)m_CScanView1_Z_Range;
                faktorI = (double)(bitmapScan3.Height - dwPixelSize) / (double)1024;
                double minZ = double.MaxValue;
                double maxZ = double.MinValue;
                double minZ2 = double.MaxValue;
                double maxZ2 = double.MinValue;
                if (true)
                {

                    for (int i = 0; i < m_iScannerDataLen; i++)
                    {
                        int x;
                        if (inverse_scan3.Checked)
                        {
                            x = (int)(faktorX * (double)(-m_doX[i] + m_CScanView1_X_Range_At_End / 2));
                        }
                        else
                        {
                            x = (int)(faktorX * (double)(m_doX[i] + m_CScanView1_X_Range_At_End / 2));
                        }
                        if ((int)(m_doZ[i] - m_CScanView1_Z_Start) > 0)
                        {
                            int z = (int)(faktorZ * (double)(m_doZ[i] - m_CScanView1_Z_Start));
                            if (z >= bitmapScan3.Height)
                            {
                                z = bitmapScan3.Height - 1;
                            }
                            else if (z < 0)
                            {
                                z = 0;
                            }
                            if (x < 0)
                            {
                                x = 0;
                            }

                            bitmapScan3.SetPixel(x, z, Color.White);

                            if (m_doZ[i] < minZ)
                            {
                                minZ = m_doZ[i];

                            }


                            if (m_doZ[i] > maxZ)
                            {
                                maxZ = m_doZ[i];
                            }


                        }
                    }
                }

                if (true)
                {
                    for (int i = 0; i < m_iScannerDataLen2; i++)
                    {
                        int x;
                        if (inverse_scan31.Checked)
                        {
                            x = (int)(faktorX * (double)(-m_doX2[i] + distancex + m_CScanView1_X_Range_At_End / 2));
                        }
                        else
                        {
                            x = (int)(faktorX * (double)(m_doX2[i] + distancex + m_CScanView1_X_Range_At_End / 2));
                        }
                        if (x >= 0 && x < bitmapScan3.Width)
                        {
                            if ((int)(m_doZ2[i] - m_CScanView1_Z_Start) > 0)
                            {
                                int z = (int)(faktorZ * (double)(m_doZ2[i] - distancez - m_CScanView1_Z_Start));
                                if (z >= bitmapScan3.Height)
                                {
                                    z = bitmapScan3.Height - 1;
                                }
                                else if (z < 0)
                                {
                                    z = 0;
                                }
                                bitmapScan3.SetPixel(x, z, Color.DeepSkyBlue);
                                if (m_doZ2[i] < minZ2)
                                {
                                    minZ2 = m_doZ2[i];
                                }
                                if (m_doZ2[i] > maxZ2)
                                {
                                    maxZ2 = m_doZ2[i];
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur : la valeur x est hors plage", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            distancex = 150;
                        }
                    }
                }
                string minZStr = (minZ > 600 || minZ < 0) ? "Z min en mm (Capteur 1): N/A" : string.Format("Z min en mm (Capteur 1): {0:0.00}", minZ);
                string minZStr3 = (minZ2 > 600 || minZ2 < 0) ? "Z min en mm (Capteur 2): N/A" : string.Format("Z min en mm (Capteur 2): {0:0.00}", minZ2);
                string maxZStr1 = (maxZ > 600 || maxZ < 0) ? "Z max en mm (Capteur 1): N/A" : string.Format("Z max en mm (Capteur 1): {0:0.00}", maxZ);
                string maxZStr2 = (maxZ2 > 600 || maxZ2 < 0) ? "Z max en mm (Capteur 2): N/A" : string.Format("Z max en mm (Capteur 2): {0:0.00}", maxZ2);

                using (var font = new Font("Arial", 10))
                using (var brush = new SolidBrush(Color.White))
                {
                    g3.DrawString(minZStr, font, brush, 5, 5);
                    g3.DrawString(minZStr3, font, new SolidBrush(Color.DeepSkyBlue), 5, 20);
                    g3.DrawString(maxZStr1, font, brush, 5, bitmapScan3.Height - 40);
                    g3.DrawString(maxZStr2, font, new SolidBrush(Color.DeepSkyBlue), 5, bitmapScan3.Height - 20);
                }
            }


            if (!pictureBox2.IsDisposed && pictureBox2.InvokeRequired)
            {
                pictureBox2.Invoke((MethodInvoker)delegate
                {
                    pictureBox2.Image = bitmapScan3;
                    pictureBox2.Invalidate();
                });
            }

        }




        int ea3valuefunction;
        int ea4valuefunction;
        int sourcetrigscan1;

        int ea3valuefunctionscan2;
        int ea4valuefunctionscan2;
        int sourcetrigscan2;


        private void Timer_function(object sender, EventArgs e)//gui timer 
        {

            textBoxFrequency.Text = String.Format("{0:0.0} Hz  PicCntErr: {1} Cnt: {2}",
                                                (float)m_iScanner_Frequeny / GUI_Timer.Interval * 1000,
                                                m_iPicCntErr,
                                                m_iProfile_Counter);

            textBoxFrequency1.Text = String.Format("{0:0.0} Hz", (float)m_iScanner_Frequeny / GUI_Timer.Interval * 1000);
            //textBoxFrequency1.Font = new Font("Arial", 12, FontStyle.Regular); 


            textBoxFIFO.Text = String.Format("CPU FiFo: {0} % DLL FiFo: {1} %",
                                                m_iCPUFiFo,
                                                m_iDLLFiFo);



            textBoxFrequency2.Text = String.Format("{0:0.0} Hz  PicCntErr: {1} Cnt: {2}",
                                    (float)m_iScanner_Frequeny2 / GUI_Timer.Interval * 1000,
                                    m_iPicCntErr2,
                                    m_iProfile_Counter2);

            textBoxFrequency0.Text = String.Format("{0:0.0} Hz", (float)m_iScanner_Frequeny2 / GUI_Timer.Interval * 1000);

            textBoxFIFO2.Text = String.Format("CPU FiFo: {0} % DLL FiFo: {1} %",
                                                m_iCPUFiFo2,
                                                m_iDLLFiFo2);

            freq = m_iScanner_Frequeny;


            if (flagled == 1)
            {
                labelplagecapteur1.ForeColor = Color.Red;
                labelplagecapteur1.Text = "Hors Plage";
            }
            if (flagled == 2)
            {
                labelplagecapteur1.ForeColor = Color.Green;
                labelplagecapteur1.Text = "Plage";


            }

            if (flagled2 == 1)
            {
                labelplagecapteur2.ForeColor = Color.Red;
                labelplagecapteur2.Text = "Hors Plage";
            }
            if (flagled2 == 2)
            {
                labelplagecapteur2.ForeColor = Color.Green;
                labelplagecapteur2.Text = "Plage";

            }


            int iRetBuf = 128;
            StringBuilder strRetBuf = new StringBuilder(new String(' ', iRetBuf));

            int iRetVal = EthernetScanner_ReadData(ScannerHandle, "EA3Function", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                ea3valuefunction = (int)UInt32.Parse(strRetBuf.ToString());
            }

            iRetVal = EthernetScanner_ReadData(ScannerHandle, "EA4Function", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                ea4valuefunction = (int)UInt32.Parse(strRetBuf.ToString());
            }

            iRetVal = EthernetScanner_ReadData(ScannerHandle, "TriggerSource", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                sourcetrigscan1 = (int)UInt32.Parse(strRetBuf.ToString());
            }

            ///
            iRetVal = EthernetScanner_ReadData(ScannerHandle2, "EA3Function", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                ea3valuefunctionscan2 = (int)UInt32.Parse(strRetBuf.ToString());
            }

            iRetVal = EthernetScanner_ReadData(ScannerHandle2, "EA4Function", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                ea4valuefunctionscan2 = (int)UInt32.Parse(strRetBuf.ToString());
            }

            iRetVal = EthernetScanner_ReadData(ScannerHandle2, "TriggerSource", strRetBuf, iRetBuf, -1);
            if (iRetVal == 0)
            {
                sourcetrigscan2 = (int)UInt32.Parse(strRetBuf.ToString());
            }



            if (ea3valuefunction == 2 && sourcetrigscan1 == 0 && ea4valuefunctionscan2 == 1 && sourcetrigscan2 == 1)
            {
                syncstatus.Text = "Capteur 1 -> Capteur 2";
                syncstatus.BackColor = Color.Green;
            }
            else
            {
                syncstatus.BackColor = Color.Red;

            }

            if (ea3valuefunctionscan2 == 2 && sourcetrigscan2 == 0 && ea4valuefunction == 1 && sourcetrigscan1 == 1)
            {
                syncstatus.Text = "Capteur 2 -> Capteur 1";
                syncstatus.BackColor = Color.Green;
            }





            if (string.IsNullOrEmpty(textBoxfreq.Text))
            {
                getfreq();
            }



            double averageCount = (tempDoX1.Count + tempDoZ1.Count + tempDoI1.Count + tempDoX2.Count + tempDoZ2.Count + tempDoI2.Count) / 6.0;
            double averageCount1 = (tempDoX1.Count + tempDoZ1.Count + tempDoI1.Count) / 3.0;
            double averageCount2 = (tempDoX2.Count + tempDoZ2.Count + tempDoI2.Count) / 3.0;
            double valuelimit = queueLengthThreshold * max_len * frequencyHz;
            double valuelimit1 = queuetimetrigger * m_iScannerDataLen * frequencyHzscan1;
            double valuelimit2 = queuetimetriggerscan2 * m_iScannerDataLen2 * frequencyHzscan2;


            if (averageCount >= valuelimit)
            {

                text_pretrig2scan.Text = "Pre-Trigger prêt";

            }

            if (averageCount1 >= valuelimit1)
            {

                textboxstatusscan1trigger.Text = "Pre-Trigger prêt";

            }
            if (averageCount2 >= valuelimit2)
            {

                textboxstatusscan1trigger2.Text = "Pre-Trigger prêt";

            }


            int progress = (int)(((float)m_iNumberProfilesToSaveCnt / (float)(m_iNumberProfilesToSaveMax * frequencyHzscan1)) * 100);
            if (progress < 0)
            {
                progress = 0;
            }
            else if (progress > 100)
            {
                progress = 100;
            }
            progressBar1.Value = progress;



            if (progressBar1.Value > 0 && progressBar1.Value < 100)
            {
                text_eng_scan1.Text = "En cours d'enregistrement :" + progressBar1.Value.ToString() + "%";
            }


            int progress2 = (int)(((float)m_iNumberProfilesToSaveCnt2 / (float)(m_iNumberProfilesToSaveMax2 * frequencyHzscan2)) * 100);
            if (progress2 < 0)
            {
                progress2 = 0;
            }
            else if (progress2 > 100)
            {
                progress2 = 100;
            }
            progressBar2.Value = progress2;

            if (progressBar2.Value > 0 && progressBar2.Value < 100)
            {
                text_eng_scan2.Text = "En cours d'enregistrement :" + progressBar2.Value.ToString() + "%";
            }


            int progress3 = (int)(((float)m_iNumber2ProfilesToSaveCnt / (float)(m_iNumber2ProfilesToSaveMax * frequencyHz)) * 100);
            if (progress3 < 0)
            {
                progress3 = 0;
            }
            else if (progress3 > 100)
            {
                progress3 = 100;
            }
            progressBar3.Value = progress3;

            if (progressBar3.Value > 0 && progressBar3.Value < 100)
            {
                text_eng_scan3.Text = "En cours d'enregistrement :" + progressBar3.Value.ToString() + "%";
            }


            if (savepretrigger.Checked)
            {
                int totalCount = tempDoZ1.Count + tempDoX1.Count + tempDoI1.Count + tempDoZ2.Count + tempDoX2.Count + tempDoI2.Count;
                int value = (int)Math.Round((double)totalCount / (double)(valuelimit * 6) * 100);
                int progress77 = (int)Math.Round((double)value / 100 * progressbartrig.Maximum);

                if (progress77 > 100)
                {
                    progress77 = 100;
                }
                else if (progress77 < 0)
                {
                    progress77 = 0;

                }
                progressbartrig.Value = progress77;

            }



            if (checkBoxpretriggerscan1.Checked)
            {
                int totalCount1 = tempDoZ1.Count + tempDoX1.Count + tempDoI1.Count;
                int value1 = (int)Math.Round((double)totalCount1 / (double)(valuelimit1 * 3) * 100);
                int progressscan1 = (int)Math.Round((double)value1 / 100 * progressbartrigsca1.Maximum);

                if (progressscan1 > 100)
                {
                    progressscan1 = 100;
                }
                else if (progressscan1 < 0)
                {
                    progressscan1 = 0;

                }
                progressbartrigsca1.Value = progressscan1;

            }
            if (checkBoxpretriggerscan2.Checked)
            {
                int totalCount2 = tempDoZ2.Count + tempDoX2.Count + tempDoI2.Count;
                int value2 = (int)Math.Round((double)totalCount2 / (double)(valuelimit2 * 3) * 100);
                int progressscan2 = (int)Math.Round((double)value2 / 100 * progressbartrigsca2.Maximum);

                if (progressscan2 > 100)
                {
                    progressscan2 = 100;
                }
                else if (progressscan2 < 0)
                {
                    progressscan2 = 0;

                }
                progressbartrigsca2.Value = progressscan2;

            }








            label4.Text = tempDoX1.Count.ToString();
            label7.Text = tempDoZ1.Count.ToString();
            label18.Text = tempDoI1.Count.ToString();

            label19.Text = tempDoX2.Count.ToString();
            label20.Text = tempDoZ2.Count.ToString();
            label21.Text = tempDoI2.Count.ToString();

            //label3.Text = max_len.ToString();




            if (m_bUSRIO != null)
            {
                ea1Value = (m_bUSRIO[0] & (1 << 0)) != 0 ? 1 : 0;
                int ea2Value = (m_bUSRIO[0] & (1 << 1)) != 0 ? 1 : 0;
                int ea3Value = (m_bUSRIO[0] & (1 << 2)) != 0 ? 1 : 0;
                int ea4Value = (m_bUSRIO[0] & (1 << 3)) != 0 ? 1 : 0;

                textBoxEA1.Text = ea1Value.ToString();
                textBoxEA2.Text = ea2Value.ToString();
                textBoxEA3.Text = ea3Value.ToString();
                textBoxEA4.Text = ea4Value.ToString();
            }
            else
            {
                textBoxEA1.Text = "N/A";
                textBoxEA2.Text = "N/A";
                textBoxEA3.Text = "N/A";
                textBoxEA4.Text = "N/A";
            }


            if (m_bUSRIO2 != null)
            {
                int ea1Valuescan2 = (m_bUSRIO2[0] & (1 << 0)) != 0 ? 1 : 0;
                int ea2Valuescan2 = (m_bUSRIO2[0] & (1 << 1)) != 0 ? 1 : 0;
                int ea3Valuescan2 = (m_bUSRIO2[0] & (1 << 2)) != 0 ? 1 : 0;
                int ea4Valuescan2 = (m_bUSRIO2[0] & (1 << 3)) != 0 ? 1 : 0;

                textBoxEA1scan2.Text = ea1Valuescan2.ToString();
                textBoxEA2scan2.Text = ea2Valuescan2.ToString();
                textBoxEA3scan2.Text = ea3Valuescan2.ToString();
                textBoxEA4scan2.Text = ea4Valuescan2.ToString();
            }
            else
            {
                textBoxEA1scan2.Text = "N/A";
                textBoxEA2scan2.Text = "N/A";
                textBoxEA3scan2.Text = "N/A";
                textBoxEA4scan2.Text = "N/A";
            }




            if (ScannerHandle != (IntPtr)null)
            {
                //show the state of the connection
                EthernetScanner_GetConnectStatus(ScannerHandle, iConnectionStatus);
                if (iConnectionStatus[0] == iETHERNETSCANNER_TCPSCANNERDISCONNECTED)
                {
                    labelConnectionStatus.Text = "Déconnecter";

                }
                else if (iConnectionStatus[0] == iETHERNETSCANNER_TCPSCANNERCONNECTED)
                {
                    labelConnectionStatus.Text = "Connecté";
                    labelConnectionStatus.BackColor = Color.Green;
                }
            }
            else
            {
                labelConnectionStatus.Text = "Déconnecter";
                labelConnectionStatus.BackColor = Color.Red;

            }

            if (ScannerHandle2 != (IntPtr)null)
            {
                //show the state of the connection
                EthernetScanner_GetConnectStatus(ScannerHandle2, iConnectionStatus2);
                if (iConnectionStatus2[0] == iETHERNETSCANNER_TCPSCANNERDISCONNECTED)
                {
                    labelConnectionStatus2.Text = "Déconnecter";

                }
                else if (iConnectionStatus2[0] == iETHERNETSCANNER_TCPSCANNERCONNECTED)
                {
                    labelConnectionStatus2.Text = "Connecté";
                    labelConnectionStatus2.BackColor = Color.Green;


                }
            }
            else
            {
                labelConnectionStatus2.Text = "Déconnecter";
                labelConnectionStatus2.BackColor = Color.Red;

            }

            m_iScanner_Frequeny = 0;
            m_iScanner_Frequeny2 = 0;
        }

        private void GetXMLParser(StringBuilder strXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strXML.ToString());

            XmlNodeList nodes1 = doc.DocumentElement.SelectNodes("/device/general");
            foreach (XmlNode node in nodes1)
            {
                string strTemp = node.SelectSingleNode("workingrange_z_start").InnerText;
                //take care about the flaot format number! '83.0' or '83,0'
                m_CScanView1_Z_Start = float.Parse(node.SelectSingleNode("workingrange_z_start").InnerText.Replace('.', ','));
                m_CScanView1_Z_Range = float.Parse(node.SelectSingleNode("measuringrange_z").InnerText.Replace('.', ','));
                m_CScanView1_X_Range_At_Start = float.Parse(node.SelectSingleNode("fieldwidth_x_start").InnerText.Replace('.', ','));
                m_CScanView1_X_Range_At_End = float.Parse(node.SelectSingleNode("fieldwidth_x_end").InnerText.Replace('.', ','));
                m_CScanView1_WidthX = float.Parse(node.SelectSingleNode("pixel_x_max").InnerText.Replace('.', ','));
                m_CScanView1_WidthZ = float.Parse(node.SelectSingleNode("pixel_z_max").InnerText.Replace('.', ','));
                strModelNumber = node.SelectSingleNode("ordernumber").InnerText;
                strProductVersion = node.SelectSingleNode("productversion").InnerText;
                strVendorName = node.SelectSingleNode("producer").InnerText;
                strDescription = node.SelectSingleNode("description").InnerText;
                strHardwareVersion = node.SelectSingleNode("hardwareversion").InnerText;
                strFirwareVersion = node.SelectSingleNode("firmwareversion/general").InnerText;
                strScannerMAC = node.SelectSingleNode("mac").InnerText;
                strSerialNummber = node.SelectSingleNode("serialnumber").InnerText;

                /*string strScannerInfo = strModelNumber + " " + strSerialNummber + " " + strDescription + " " + strVendorName;
                textBoxLinearizationTableSerialNumber1.Text = strScannerInfo;

                strScannerInfo = "ProductVersion: " + strProductVersion + " HW: " + strHardwareVersion + " FW: " + strFirwareVersion + " MAC: " + strScannerMAC;
                textBoxLinearizationTableSerialNumber2.Text = strScannerInfo;

                strScannerInfo = "Range-Z-Start: " + m_CScanView1_Z_Start.ToString() + " mm " +
                                    "Range-Z: " + m_CScanView1_Z_Range.ToString() + " mm " +
                                    "X-Start: " + m_CScanView1_X_Range_At_Start.ToString() + " mm " +
                                    "X-End: " + m_CScanView1_X_Range_At_End.ToString() + " mm " +
                                    "Pixel-Max: X: " + m_CScanView1_WidthX.ToString() + " " +
                                    "Z: " + m_CScanView1_WidthZ;
                textBoxLinearizationTableSerialNumber3.Text = strScannerInfo;*/

            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (ScannerHandle != (IntPtr)null)
            {
                return;
            }

            /*textBoxLinearizationTableSerialNumber1.Text = "";
            textBoxLinearizationTableSerialNumber2.Text = "";
            textBoxLinearizationTableSerialNumber3.Text = "";*/

            strIPAddress = textBoxIPAddress.Text;
            string strPort = "32001";
            //int iTimeOut = Int32.Parse(textBoxTimeOut.Text);
            int iTimeOut = 0;

            //start the connection to the Scanner
            ScannerHandle = EthernetScanner_Connect(strIPAddress, strPort, iTimeOut);

            //check the connection state with timeout 3000 ms
            DateTime startConnectTime = DateTime.Now;
            TimeSpan connectTime = new TimeSpan();
            do
            {
                if (connectTime.TotalMilliseconds > 1500)
                {
                    ScannerHandle = EthernetScanner_Disconnect(ScannerHandle);
                    MessageBox.Show("Erreur : aucune connexion !!!", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Thread.Sleep(10);
                EthernetScanner_GetConnectStatus(ScannerHandle, iConnectionStatus);
                connectTime = DateTime.Now - startConnectTime;
            } while (iConnectionStatus[0] != iETHERNETSCANNER_TCPSCANNERCONNECTED);


            int iGetInfoRes = EthernetScanner_GetInfo(ScannerHandle, m_strScannerInfoXML, iETHERNETSCANNER_GETINFOSIZEMAX, "xml");
            if (iGetInfoRes > 0)
            {
                GetXMLParser(m_strScannerInfoXML);
                //start the ReadOut-Thread
                m_bScannerThreadRunning = true;
                m_bScannerThreadRunning5 = true;
                ScannerThread = new Thread(new ThreadStart(ScannerThreadProc));
                ScannerThread.Start();
                ScannerThread5 = new Thread(new ThreadStart(ScannerThreadProc5));
                ScannerThread5.Start();
                if (!GUI_Timer.Enabled)
                {
                    GUI_Timer.Start();
                }
                getfreqscan1();
            }
            else
            {
                m_bScannerThreadRunning = false;
                m_bScannerThreadRunning5 = false;
                MessageBox.Show("Erreur : aucun paquet d'informations valide !!!", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (ScannerThread != null && ScannerThread.IsAlive)
            {
                m_bScannerThreadRunning = false;

                if (!ScannerThread.Join(ThreadJoinTimeoutMilliseconds))
                {

                }

                ScannerThread = null;
                ea4valuefunction = 0;
            }

            if (ScannerThread5 != null && ScannerThread5.IsAlive)
            {
                m_bScannerThreadRunning5 = false;

                if (!ScannerThread5.Join(ThreadJoinTimeoutMilliseconds))
                {

                }

                ScannerThread5 = null;
            }

            if (ScannerHandle != IntPtr.Zero)
            {
                ScannerHandle = EthernetScanner_Disconnect(ScannerHandle);
            }
        }

        int numEnqueued = 0;
        public bool stopEnqueuing = false;
        double maxEnqueued;

        int numEnqueuedscan1 = 0;
        public bool stopEnqueuingscan1 = false;
        double maxEnqueuedscan1;


        int numEnqueuedscan2 = 0;
        public bool stopEnqueuingscan2 = false;
        double maxEnqueuedscan2;


        int numEnqueuedscan1trig = 0;
        public bool stopEnqueuingscan1trig = false;
        double maxEnqueuedscan1trig;


        public bool waitenqueu = false;

        public bool waitenqueupretrig1 = false;

        public bool waitenqueupretrig2 = false;


        public bool withoutpretrigger = false;

        public bool withoutpretriggerscan1 = false;

        public bool withoutpretriggerscan2 = false;


        internal bool misvalue = false;


        private void ScannerThreadProc()
        {
            //if the scanner was disconnected try to resetart the sending of the data
            //Boolean bSendDataTransferEnableAlreadyDone = false;
            int iRetBuf = 128;
            StringBuilder strRetBuf = new StringBuilder(new String(' ', iRetBuf));

            DateTime timeGetInfoTemp = DateTime.Now;
            TimeSpan timeDiff = new TimeSpan();
            while (m_bScannerThreadRunning)
            {


                //état actuel de la connexion
                EthernetScanner_GetConnectStatus(ScannerHandle, iConnectionStatus);
                if (iConnectionStatus[0] == iETHERNETSCANNER_TCPSCANNERCONNECTED)
                {
                    //EthernetScanner_GetXZIExtended : les données sont linéarisées
                    m_iScannerDataLen = EthernetScanner_GetXZIExtended(
                                                                    ScannerHandle,
                                                                    m_doX,
                                                                    m_doZ,
                                                                    m_iIntensity,
                                                                    m_iPeakWidth,
                                                                    iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX * iETHERNETSCANNER_SCANXMAX,
                                                                    m_iEncoder,
                                                                    m_bUSRIO,
                                                                    100,
                                                                    null,
                                                                    0,
                                                                    m_iPicCnt);


                    if (m_iScannerDataLen <= 0)
                    {
                        continue;
                    }
                    max_len = m_iScannerDataLen;
                    maxEnqueued = frequencyHz * m_iNumber2ProfilesToSaveMax * 1.30;
                    maxEnqueuedscan1 = frequencyHzscan1 * m_iNumberProfilesToSaveMax * 1.10;


                    if ((m_iPicCnt[0] - m_iPicCntTemp[0]) != 1)
                    {
                        if ((m_iPicCnt[0] != 0) && (m_iPicCntTemp[0] != 0xFFFF))
                        {
                            m_iPicCntErr++;
                        }
                    }
                    m_iPicCntTemp[0] = m_iPicCnt[0];




                    //read the Encoder values using the function EthernetScanner_ReadData
                    int iRetVal = EthernetScanner_ReadData(ScannerHandle, "EncoderHTL", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iEncoderHTL = (int)UInt32.Parse(strRetBuf.ToString());
                    }

                    iRetVal = EthernetScanner_ReadData(ScannerHandle, "EncoderTTL", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iEncoderTTL = (int)UInt32.Parse(strRetBuf.ToString());
                    }





                    //CPUFIFO shows the internal FiFO state in the scanner, if the CPUFiFo is increasing, it  means
                    //that the scanner is generating data more than what it can transfer.
                    //please note that the maximum data transfer in the scanner is 30 MByte/s
                    //If you want to increase the output rate of the scanner, beside decreasinf the ROI size, remember to switch off
                    // non necessary data content like peak end position and peak width
                    iRetVal = EthernetScanner_ReadData(ScannerHandle, "CPUFiFo", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iCPUFiFo = Int32.Parse(strRetBuf.ToString());
                    }

                    //ScannerState is used in this example to check if the scanner is over triggered
                    iRetVal = EthernetScanner_ReadData(ScannerHandle, "ScannerState", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iScannerState = Int32.Parse(strRetBuf.ToString());
                    }



                    m_iDLLFiFo = EthernetScanner_GetDllFiFoState(ScannerHandle);



                    if (m_bSaveOnce3 == true && stopEnqueuing == false)
                    {
                        numEnqueued++;

                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {

                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);

                        }
                        if (numEnqueued >= maxEnqueued)
                        {
                            stopEnqueuing = true;

                        }

                    }



                    if (withoutpretriggerscan1 == true && lastvalueeascan1 == 1)
                    {
                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {
                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);
                        }
                    }



                    //ici
                    if (checkBoxpretriggerscan1.Checked && waitenqueupretrig1 == false)
                    {

                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {
                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);

                        }

                    }

                    max = queueLengthThreshold * m_iScannerDataLen * frequencyHzscan1;


                    float timequeue = queuetimetrigger * m_iScannerDataLen * frequencyHzscan1;


                    if (checkBoxpretriggerscan1.Checked && (tempDoZ1.Count > timequeue) && (scan1queuestop == true))
                    {
                        try
                        {

                            double value1;
                            double value2;
                            int value3;


                            for (int k = 0; k < m_iScannerDataLen; k++)
                            {

                                tempDoZ1.TryDequeue(out value1);
                                tempDoX1.TryDequeue(out value2);
                                tempDoI1.TryDequeue(out value3);


                            }


                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                    }



                    if ((m_bSaveOnce) && (file != null) && (stopEnqueuingscan1 == false))
                    {

                        numEnqueuedscan1++;


                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {
                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);
                        }


                        if (numEnqueuedscan1 >= maxEnqueuedscan1)
                        {
                            stopEnqueuingscan1 = true;

                        }
                    }


                    if (m_bUSRIO != null)
                    {
                        ea1Valuesave = (m_bUSRIO[0] & (1 << 0)) != 0 ? 1 : 0;


                    }







                    if (savepretrigger.Checked && waitenqueu == false)
                    {

                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {
                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);
                        }
                    }

                    if (withoutpretrigger == true && lastvalueea == 1)
                    {
                        for (int k = 0; k < m_iScannerDataLen; k++)
                        {
                            tempDoZ1.Enqueue(m_doZ[k]);
                            tempDoX1.Enqueue(m_doX[k]);
                            tempDoI1.Enqueue(m_iIntensity[k]);
                        }
                    }

                    if (savepretrigger.Checked && (tempDoZ1.Count > queueLengthThreshold * max_len * frequencyHz) && (doublescanstop == true))
                    {
                        try
                        {
                            int k;
                            double value1;
                            double value2;
                            int value3;

                            for (k = 0; k < m_iScannerDataLen; k++)
                            {

                                tempDoZ1.TryDequeue(out value1);
                                tempDoX1.TryDequeue(out value2);
                                tempDoI1.TryDequeue(out value3);


                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                    }



                    //if the scan data was received: do anything
                    if (m_iScannerDataLen > 0)
                    {
                        ///count the frequency
                        m_iScanner_Frequeny++;
                        m_iProfile_Counter++;

                        //with higher frequency or slower PC-CPU the view of scans can be disabled
                        if (checkBoxViewDisable.Checked == false)
                        {
                            //decrease or disable the view of the scans if the scanner frequency go high!
                            timeDiff = DateTime.Now - timeGetInfoTemp;
                            if (timeDiff.TotalMilliseconds > 100)
                            {
                                timeGetInfoTemp = DateTime.Now;




                                ShowScanXZI(startrangezmm, endrangezmm);


                                ShowScanXZI3();

                            }
                        }
                    }


                }
            }
        }

        private void ScannerThreadProc2()
        {

            int iRetBuf = 128;
            StringBuilder strRetBuf = new StringBuilder(new String(' ', iRetBuf));

            DateTime timeGetInfoTemp = DateTime.Now;
            TimeSpan timeDiff = new TimeSpan();
            while (m_bScannerThreadRunning2)
            {
                //current state of the connection
                EthernetScanner_GetConnectStatus(ScannerHandle2, iConnectionStatus2);
                if (iConnectionStatus2[0] == iETHERNETSCANNER_TCPSCANNERCONNECTED)
                {


                    //EthernetScanner_GetXZIExtended: the Data are linearized
                    m_iScannerDataLen2 = EthernetScanner_GetXZIExtended(
                                                                    ScannerHandle2,
                                                                    m_doX2,
                                                                    m_doZ2,
                                                                    m_iIntensity2,
                                                                    m_iPeakWidth2,
                                                                    iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX * iETHERNETSCANNER_SCANXMAX,
                                                                    m_iEncoder2,
                                                                    m_bUSRIO2,
                                                                    100,
                                                                    null,
                                                                    0,
                                                                    m_iPicCnt2);

                    if (m_iScannerDataLen2 <= 0)
                    {
                        continue;
                    }
                    //max_len2 = m_iScannerDataLen2;
                    max_len2 = m_iScannerDataLen2;
                    maxEnqueuedscan2 = frequencyHzscan2 * m_iNumberProfilesToSaveMax2 * 1.10;


                    if ((m_iPicCnt2[0] - m_iPicCntTemp2[0]) != 1)
                    {
                        if ((m_iPicCnt2[0] != 0) && (m_iPicCntTemp2[0] != 0xFFFF))
                        {
                            m_iPicCntErr2++;
                        }
                    }
                    m_iPicCntTemp2[0] = m_iPicCnt2[0];

                    int iRetVal = EthernetScanner_ReadData(ScannerHandle2, "EncoderHTL", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iEncoderHTL2 = (int)UInt32.Parse(strRetBuf.ToString());
                    }

                    iRetVal = EthernetScanner_ReadData(ScannerHandle, "EncoderTTL", strRetBuf, iRetBuf, -1);
                    if (iRetVal == 1)
                    {
                        m_iEncoderTTL2 = (int)UInt32.Parse(strRetBuf.ToString());
                    }


                    m_iDLLFiFo = EthernetScanner_GetDllFiFoState(ScannerHandle2);




                    if (m_bSaveOnce3 == true && stopEnqueuing == false)
                    {
                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {

                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);


                        }
             

                    }


                    if (m_bUSRIO2 != null)
                    {
                        ea1Valuesavescan2 = (m_bUSRIO2[0] & (1 << 0)) != 0 ? 1 : 0;
                    }

                    if (checkBoxpretriggerscan2.Checked && waitenqueupretrig2 == false)
                    {


                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {
                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);
                        }
                    }

                    float timequeue2 = queuetimetriggerscan2 * m_iScannerDataLen2 * frequencyHzscan2;


                    if (checkBoxpretriggerscan2.Checked && (tempDoZ2.Count > timequeue2) && (scan2queuestop == true))
                    {
                        double value4;
                        double value5;
                        int value6;
                        try
                        {

                            for (int k = 0; k < m_iScannerDataLen2; k++)
                            {

                                tempDoZ2.TryDequeue(out value4);
                                tempDoX2.TryDequeue(out value5);
                                tempDoI2.TryDequeue(out value6);


                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                    }





                    if (savepretrigger.Checked && waitenqueu == false)
                    {
                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {

                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);
                        }
                    }
                    if (withoutpretrigger == true && lastvalueea == 1)
                    {
                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {

                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);
                        }
                    }

                    if (withoutpretriggerscan2 == true && lastvalueeascan2 == 1)
                    {
                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {

                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);
                        }
                    }
                    //int queueLengthThreshold = int.Parse(queueLengthTextBox.Text);
                    float test = queueLengthThreshold * max_len * frequencyHz;


                    if (savepretrigger.Checked && (tempDoZ2.Count > test) && (doublescanstop == true))
                    {
                        try
                        {
                            double value4;
                            double value5;
                            int value6;
                            int k;
                            for (k = 0; k < m_iScannerDataLen2; k++)
                            {

                                tempDoZ2.TryDequeue(out value4);
                                tempDoX2.TryDequeue(out value5);
                                tempDoI2.TryDequeue(out value6);

                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                    }

                    //Save number of profiles to the file: "ScanData.txt"
                    //can be mofified to save additional informations/values/calculations
                    if ((m_bSaveOnce2) && (file6 != null) && (stopEnqueuingscan2 == false))
                    {
                        numEnqueuedscan2++;

                        for (int k = 0; k < m_iScannerDataLen2; k++)
                        {

                            tempDoZ2.Enqueue(m_doZ2[k]);
                            tempDoX2.Enqueue(m_doX2[k]);
                            tempDoI2.Enqueue(m_iIntensity2[k]);
                        }
                        if (numEnqueuedscan2 >= maxEnqueuedscan2)
                        {
                            stopEnqueuingscan2 = true;
                        }

                    }

                    //if the scan data was received: do anything
                    if (m_iScannerDataLen2 > 0)
                    {
                        ///count the frequency
                        m_iScanner_Frequeny2++;
                        m_iProfile_Counter2++;

                        //with higher frequency or slower PC-CPU the view of scans can be disabled
                        if (checkBoxViewDisable2.Checked == false)
                        {
                            //decrease or disable the view of the scans if the scanner frequency go high!
                            timeDiff = DateTime.Now - timeGetInfoTemp;
                            if (timeDiff.TotalMilliseconds > 100)
                            {
                                timeGetInfoTemp = DateTime.Now;


                                ShowScanXZI2(startrangezmm2, endrangezmm2);




                            }
                        }
                    }
                }

            }
        }



        private void ScannerThreadProc3()
        {
            while (m_bScannerThreadRunning3)
            {

                if ((m_bSaveOnce3) && (file2 != null) && (tempDoZ1.Count >= max_len) && (tempDoZ2.Count >= max_len2))
                {
                    try
                    {
                        m_iNumber2ProfilesToSaveCnt++;

                        variabledoublesave++;
                        text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "En cours d'enregistrement"; });

                        double value1;
                        double value2;
                        int value3;


                        double value4;
                        double value5;
                        int value6;

                        for (int k = 0; k < max_len; k++)
                        {
                            do
                            {
                                misvalue = !tempDoZ1.TryDequeue(out value1);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoX1.TryDequeue(out value2);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoI1.TryDequeue(out value3);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            } while (misvalue);



                            if (inverse_scan3.Checked && inverse_scan31.Checked)
                            {



                                file2.WriteLine(variabledoublesave + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                            }
                            else if (inverse_scan3.Checked)
                            {




                                file2.WriteLine(variabledoublesave + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));

                            }
                            else
                            {

                                file2.WriteLine(variabledoublesave + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));


                            }



                        }

                        for (int k = 0; k < max_len2; k++)
                        {
                            do
                            {
                                misvalue = !tempDoZ2.TryDequeue(out value4);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoX2.TryDequeue(out value5);

                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);



                            do
                            {
                                misvalue = !tempDoI2.TryDequeue(out value6);

                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);

                            if (inverse_scan3.Checked && inverse_scan31.Checked)
                            {




                                file2.WriteLine(variabledoublesave + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                            }
                            else if (inverse_scan31.Checked)
                            {



                                file2.WriteLine(variabledoublesave + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                            }
                            else
                            {


                                file2.WriteLine(variabledoublesave + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                            }
                        }

                        float limitprof = m_iNumber2ProfilesToSaveMax * frequencyHz;
                        if (m_iNumber2ProfilesToSaveCnt >= limitprof)
                        {
                            text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "Données enregistrées ! "; });
                            m_bSaveOnce3 = false;
                            file2.Close();
                            progressBar3.Invoke((MethodInvoker)delegate { progressBar3.Value = 0; });

                            while (tempDoZ1.TryDequeue(out _)) ;
                            while (tempDoX1.TryDequeue(out _)) ;
                            while (tempDoI1.TryDequeue(out _)) ;
                            while (tempDoZ2.TryDequeue(out _)) ;
                            while (tempDoX2.TryDequeue(out _)) ;
                            while (tempDoI2.TryDequeue(out _)) ;
                            numEnqueued = 0;
                            stopEnqueuing = false;
                        }

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



                if ((sepfichiers.Checked) && (m_bSaveOnce3) && (sep1 != null) && (sep2 != null) && (tempDoZ1.Count >= max_len) && (tempDoZ2.Count >= max_len))
                {
                    try
                    {
                        m_iNumber2ProfilesToSaveCnt++;

                        variabledoublesave++;
                        text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "En cours d'enregistrement"; });

                        double value1;
                        double value2;
                        int value3;


                        double value4;
                        double value5;
                        int value6;

                        for (int k = 0; k < max_len; k++)
                        {
                            do
                            {
                                misvalue = !tempDoZ1.TryDequeue(out value1);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoX1.TryDequeue(out value2);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoI1.TryDequeue(out value3);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            } while (misvalue);



                            if (inverse_scan3.Checked && inverse_scan31.Checked)
                            {



                                sep1.WriteLine(variabledoublesave + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                            }
                            else if (inverse_scan3.Checked)
                            {




                                sep1.WriteLine(variabledoublesave + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));

                            }
                            else
                            {

                                sep1.WriteLine(variabledoublesave + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));


                            }



                        }





                        for (int k = 0; k < max_len; k++)
                        {
                            do
                            {
                                misvalue = !tempDoZ2.TryDequeue(out value4);
                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            } while (misvalue);

                            do
                            {
                                misvalue = !tempDoX2.TryDequeue(out value5);

                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);



                            do
                            {
                                misvalue = !tempDoI2.TryDequeue(out value6);

                                if (misvalue)
                                {
                                    MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            } while (misvalue);

                            if (inverse_scan3.Checked && inverse_scan31.Checked)
                            {




                                sep2.WriteLine(variabledoublesave + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                            }
                            else if (inverse_scan31.Checked)
                            {



                                sep2.WriteLine(variabledoublesave + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                            }
                            else
                            {




                                sep2.WriteLine(variabledoublesave + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                            }
                        }


                        if (m_iNumber2ProfilesToSaveCnt >= m_iNumber2ProfilesToSaveMax * frequencyHz)
                        {
                            text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "Données enregistrées ! "; });
                            m_bSaveOnce3 = false;
                            sep1.Close();
                            sep2.Close();


                            while (tempDoZ1.TryDequeue(out _)) ;
                            while (tempDoX1.TryDequeue(out _)) ;
                            while (tempDoI1.TryDequeue(out _)) ;
                            while (tempDoZ2.TryDequeue(out _)) ;
                            while (tempDoX2.TryDequeue(out _)) ;
                            while (tempDoI2.TryDequeue(out _)) ;
                            numEnqueued = 0;
                            stopEnqueuing = false;
                            progressBar3.Invoke((MethodInvoker)delegate { progressBar3.Value = 0; });

                        }

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



            }
        }
        private void ScannerThreadProc4()
        {
            while (m_bScannerThreadRunning4)
            {
                if (ea1Valuesave == 1)
                {
                    lastvalueea = 1;
                }
                if ((m_bSaveOncetrig) && (file3 != null) && (tempDoZ1.Count > max_len) && (tempDoZ2.Count > max_len))
                {

                    if (lastvalueea == 1)
                    {
                        double value1;
                        double value2;
                        int value3;

                        double value4;
                        double value5;
                        int value6;

                        doublescanstop = false;
                        text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "En cours d'enregistrement de pré-trigger !"; });


                        try
                        {
                            m_iNumberProfilesToSaveCnttrig++;

                            variablepretrigdouble++;

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ1.TryDequeue(out value1);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX1.TryDequeue(out value2);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoI1.TryDequeue(out value3);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);


                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {


                                    file3.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                                }
                                else if (inverse_scan3.Checked)
                                {



                                    file3.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                       );

                                }
                                else
                                {

                                    file3.WriteLine(variablepretrigdouble + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                      );


                                }



                            }

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ2.TryDequeue(out value4);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX2.TryDequeue(out value5);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);



                                do
                                {
                                    misvalue = !tempDoI2.TryDequeue(out value6);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {





                                    file3.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                                }
                                else if (inverse_scan31.Checked)
                                {





                                    file3.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {




                                    file3.WriteLine(variablepretrigdouble + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }

                            saveprofilestrig = queueLengthThreshold * frequencyHz;

                            if (m_iNumberProfilesToSaveCnttrig >= m_iNumberProfilesToSaveMaxtrig * frequencyHz + saveprofilestrig)//(10)
                            {
                                m_bSaveOncetrig = false;
                                file3.Close();
                                doublescanstop = true;
                                numEnqueuedscan1trig = 0;
                                stopEnqueuingscan1trig = false;
                                waitenqueu = true;
                                text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "Données enregistrées ! "; });
                                brnatttriger.BackColor = Color.Red;

                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;
                                numEnqueued = 0;
                                stopEnqueuing = false;
                                lastvalueea = ea1Valuesave;
                                waitenqueu = false;



                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                if ((seppretrigfichiers.Checked) && (m_bSaveOncetrig) && (sep1pretrig != null) && (sep2pretrig != null) && (tempDoZ1.Count > max_len) && (tempDoZ2.Count > max_len))
                {


                    if (lastvalueea == 1)
                    {
                        double value1;
                        double value2;
                        int value3;

                        double value4;
                        double value5;
                        int value6;

                        doublescanstop = false;
                        text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "En cours d'enregistrement de pré-trigger !"; });


                        try
                        {
                            m_iNumberProfilesToSaveCnttrig++;

                            variablepretrigdouble++;

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ1.TryDequeue(out value1);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX1.TryDequeue(out value2);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoI1.TryDequeue(out value3);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);


                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {


                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                                }
                                else if (inverse_scan3.Checked)
                                {



                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                       );

                                }
                                else
                                {

                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                      );


                                }



                            }

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ2.TryDequeue(out value4);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX2.TryDequeue(out value5);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);



                                do
                                {
                                    misvalue = !tempDoI2.TryDequeue(out value6);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {





                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                                }
                                else if (inverse_scan31.Checked)
                                {





                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {




                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }

                            saveprofilestrig = queueLengthThreshold * frequencyHz;

                            if (m_iNumberProfilesToSaveCnttrig >= m_iNumberProfilesToSaveMaxtrig * frequencyHz + saveprofilestrig)//(10)
                            {
                                //text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "Données enregistrées ! "; });
                                m_bSaveOncetrig = false;
                                sep1pretrig.Close();
                                sep2pretrig.Close();
                                doublescanstop = true;
                                numEnqueuedscan1trig = 0;
                                stopEnqueuingscan1trig = false;
                                waitenqueu = true;
                                text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "Données enregistrées ! "; });
                                brnatttriger.BackColor = Color.Red;

                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;
                                numEnqueued = 0;
                                stopEnqueuing = false;
                                lastvalueea = ea1Valuesave;
                                waitenqueu = false;
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }

                if ((withoutpretrigger == true) && (file3 != null) && (tempDoZ1.Count > max_len) && (tempDoZ2.Count > max_len))
                {


                    if (lastvalueea == 1)
                    {
                        double value1;
                        double value2;
                        int value3;

                        double value4;
                        double value5;
                        int value6;

                        text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "En cours d'enregistrement de pré-trigger !"; });


                        try
                        {
                            m_iNumberProfilesToSaveCnttrig++;

                            variablepretrigdouble++;

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ1.TryDequeue(out value1);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX1.TryDequeue(out value2);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoI1.TryDequeue(out value3);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);


                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {


                                    file3.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                                }
                                else if (inverse_scan3.Checked)
                                {



                                    file3.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                       );

                                }
                                else
                                {

                                    file3.WriteLine(variablepretrigdouble + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                      );


                                }



                            }

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ2.TryDequeue(out value4);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX2.TryDequeue(out value5);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);



                                do
                                {
                                    misvalue = !tempDoI2.TryDequeue(out value6);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {
                                    file3.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                                }
                                else if (inverse_scan31.Checked)
                                {

                                    file3.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {

                                    file3.WriteLine(variablepretrigdouble + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }

                            saveprofilestrig = queueLengthThreshold * frequencyHz;

                            if (m_iNumberProfilesToSaveCnttrig >= m_iNumberProfilesToSaveMaxtrig * frequencyHz)//(10)
                            {
                                //text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "Données enregistrées ! "; });
                                withoutpretrigger = false;
                                file3.Close();



                                text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "Données enregistrées ! "; });
                                brnatttriger.BackColor = Color.Red;

                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;
                                numEnqueued = 0;
                                stopEnqueuing = false;
                                lastvalueea = ea1Valuesave;
                                waitenqueu = false;


                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                if ((seppretrigfichiers.Checked) && (withoutpretrigger == true) && (sep1pretrig != null) && (sep2pretrig != null) && (tempDoZ1.Count > max_len) && (tempDoZ2.Count > max_len))
                {


                    if (lastvalueea == 1)
                    {
                        double value1;
                        double value2;
                        int value3;

                        double value4;
                        double value5;
                        int value6;

                        text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "En cours d'enregistrement de pré-trigger !"; });


                        try
                        {
                            m_iNumberProfilesToSaveCnttrig++;

                            variablepretrigdouble++;

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ1.TryDequeue(out value1);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX1.TryDequeue(out value2);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoI1.TryDequeue(out value3);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);


                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {


                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));



                                }
                                else if (inverse_scan3.Checked)
                                {



                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                       );

                                }
                                else
                                {

                                    sep1pretrig.WriteLine(variablepretrigdouble + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point)
                                      );


                                }



                            }

                            for (int k = 0; k < max_len; k++)
                            {
                                do
                                {
                                    misvalue = !tempDoZ2.TryDequeue(out value4);
                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoZ2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                } while (misvalue);

                                do
                                {
                                    misvalue = !tempDoX2.TryDequeue(out value5);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoX2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);



                                do
                                {
                                    misvalue = !tempDoI2.TryDequeue(out value6);

                                    if (misvalue)
                                    {
                                        MessageBox.Show("Valeur manquante dans la file d'attente tempDoI2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                } while (misvalue);

                                if (inverse_scan3.Checked && inverse_scan31.Checked)
                                {





                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));



                                }
                                else if (inverse_scan31.Checked)
                                {





                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (-value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {




                                    sep2pretrig.WriteLine(variablepretrigdouble + " " + (value4 + distancez).ToString("F4", point) + " " + (value5 + distancex).ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }

                            saveprofilestrig = queueLengthThreshold * frequencyHz;

                            if (m_iNumberProfilesToSaveCnttrig >= m_iNumberProfilesToSaveMaxtrig * frequencyHz)//(10)
                            {
                                //text_eng_scan3.Invoke((MethodInvoker)delegate { text_eng_scan3.Text = "Données enregistrées ! "; });
                                withoutpretrigger = false;
                                sep1pretrig.Close();
                                sep2pretrig.Close();



                                text_pretrig2scan.Invoke((MethodInvoker)delegate { text_pretrig2scan.Text = "Données enregistrées ! "; });
                                brnatttriger.BackColor = Color.Red;

                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;
                                numEnqueued = 0;
                                stopEnqueuing = false;
                                waitenqueu = false;
                                lastvalueea = ea1Valuesave;

                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ScannerThreadProc5()
        {
            while (m_bScannerThreadRunning5)
            {

                if ((m_bSaveOnce) && (file != null) && (tempDoZ1.Count > max_len))
                {
                    double value1;
                    double value2;
                    int value3;


                    try
                    {
                        m_iNumberProfilesToSaveCnt++;
                        variable++;

                        for (int k = 0; k < max_len; k++)
                        {
                            tempDoZ1.TryDequeue(out value1);
                            tempDoX1.TryDequeue(out value2);
                            tempDoI1.TryDequeue(out value3);

                            if (inverse_scan1.Checked)
                            {
                                file.WriteLine(variable + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));
                            }
                            else
                            {
                                file.WriteLine(variable + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));


                            }
                        }


                        if (m_iNumberProfilesToSaveCnt >= m_iNumberProfilesToSaveMax * frequencyHzscan1)
                        {
                            m_bSaveOnce = false;
                            file.Close();
                            m_iNumberProfilesToSaveCnt = 0;
                            while (tempDoZ1.TryDequeue(out _)) ;
                            while (tempDoX1.TryDequeue(out _)) ;
                            while (tempDoI1.TryDequeue(out _)) ;
                            text_eng_scan1.Invoke((MethodInvoker)delegate { text_eng_scan1.Text = "Données enregistrées ! "; });
                            numEnqueuedscan1 = 0;
                            stopEnqueuingscan1 = false;

                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                if (ea1Valuesave == 1)
                {
                    lastvalueeascan1 = 1;
                }

                if ((m_bSaveOncetrigscan1) && (file4 != null) && (tempDoZ1.Count > max_len))
                {

                    if (lastvalueeascan1 == 1)
                    {
                        double value1;
                        double value2;
                        int value3;
                        scan1queuestop = false;
                        textboxstatusscan1trigger.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger.Text = "En cours d'enregistrement de pré-trigger !"; });

                        try
                        {
                            m_iNumberProfilesToSaveCntpretrig++;
                            variablescan1pretrig++;

                            for (int k = 0; k < max_len; k++)
                            {
                                tempDoZ1.TryDequeue(out value1);
                                tempDoX1.TryDequeue(out value2);
                                tempDoI1.TryDequeue(out value3);

                                if (inverse_scan1.Checked)
                                {
                                    file4.WriteLine(variablescan1pretrig + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));
                                }
                                else
                                {
                                    file4.WriteLine(variablescan1pretrig + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));


                                }
                            }

                            saveprofilestrig1 = queuetimetrigger * frequencyHzscan1;

                            if (m_iNumberProfilesToSaveCntpretrig >= m_iNumberProfilesToSaveMaxpretrig * frequencyHzscan1 + saveprofilestrig1)
                            {

                                m_bSaveOncetrigscan1 = false;
                                file4.Close();
                                scan1queuestop = true;

                                waitenqueupretrig1 = true;
                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                textboxstatusscan1trigger.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger.Text = "Données enregistrées ! "; });
                                lastvalueeascan1 = ea1Valuesave;

                                button7.BackColor = Color.Red;

                                waitenqueupretrig1 = false;

                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }


                if ((withoutpretriggerscan1) && (file4 != null) && (tempDoZ1.Count > max_len))
                {

                    if (lastvalueeascan1 == 1)
                    {
                        double value1;
                        double value2;
                        int value3;
                        textboxstatusscan1trigger.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger.Text = "En cours d'enregistrement de pré-trigger !"; });
                        try
                        {
                            m_iNumberProfilesToSaveCntpretrig++;
                            variablescan1pretrig++;

                            for (int k = 0; k < max_len; k++)
                            {
                                tempDoZ1.TryDequeue(out value1);
                                tempDoX1.TryDequeue(out value2);
                                tempDoI1.TryDequeue(out value3);

                                if (inverse_scan1.Checked)
                                {
                                    file4.WriteLine(variablescan1pretrig + " " + (-value1).ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));
                                }
                                else
                                {
                                    file4.WriteLine(variablescan1pretrig + " " + value1.ToString("F4", point) + " " + value2.ToString("F4", point) + " " + value3.ToString(point));


                                }
                            }

                            if (m_iNumberProfilesToSaveCntpretrig >= m_iNumberProfilesToSaveMaxpretrig * frequencyHzscan1)
                            {

                                withoutpretriggerscan1 = false;
                                file4.Close();


                                while (tempDoZ1.TryDequeue(out _)) ;
                                while (tempDoX1.TryDequeue(out _)) ;
                                while (tempDoI1.TryDequeue(out _)) ;
                                text_eng_scan1.Invoke((MethodInvoker)delegate { text_eng_scan1.Text = "Données enregistrées ! "; });

                                button7.BackColor = Color.Red;


                                lastvalueeascan1 = ea1Valuesave;
                                textboxstatusscan1trigger.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger.Text = "Données enregistrées ! "; });


                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }



            }

        }

        private void ScannerThreadProc6()
        {
            while (m_bScannerThreadRunning6)
            {

                if ((m_bSaveOnce2) && (file6 != null) && (tempDoZ2.Count >= max_len2))
                {
                    double value4;
                    double value5;
                    int value6;
                    //text_eng_scan2.Invoke((MethodInvoker)delegate { text_eng_scan2.Text = "En cours d'enregistrement"; });


                    try
                    {
                        m_iNumberProfilesToSaveCnt2++;
                        variable2++;
                        for (int k = 0; k < max_len2; k++)
                        {
                            tempDoZ2.TryDequeue(out value4);
                            tempDoX2.TryDequeue(out value5);
                            tempDoI2.TryDequeue(out value6);

                            if (inverse_scan2.Checked)
                            {
                                file6.WriteLine(variable2 + " " + (-value4).ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));
                            }
                            else
                            {
                                file6.WriteLine(variable2 + " " + value4.ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));


                            }
                        }

                        if (m_iNumberProfilesToSaveCnt2 >= m_iNumberProfilesToSaveMax2 * frequencyHzscan2)
                        {
                            m_bSaveOnce2 = false;
                            file6.Close();
                            m_iNumberProfilesToSaveCnt2 = 0;
                            stopEnqueuingscan2 = false;
                            numEnqueuedscan2 = 0;

                            while (tempDoZ2.TryDequeue(out _)) ;
                            while (tempDoX2.TryDequeue(out _)) ;
                            while (tempDoI2.TryDequeue(out _)) ;
                            text_eng_scan2.Invoke((MethodInvoker)delegate { text_eng_scan2.Text = "Données enregistrées ! "; });
                            progressBar2.Invoke((MethodInvoker)delegate { progressBar2.Value = 0; });

                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }


                if (ea1Valuesavescan2 == 1)
                {
                    lastvalueeascan2 = 1;
                }
                if ((m_bSaveOncetrigscan2) && (file5 != null) && (tempDoZ2.Count >= max_len2))
                {

                    if (lastvalueeascan2 == 1)
                    {
                        double value4;
                        double value5;
                        int value6;

                        scan2queuestop = false;
                        textboxstatusscan1trigger2.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger2.Text = "En cours d'enregistrement de pré-trigger !"; });

                        try
                        {
                            m_iNumberProfilesToSaveCntpretrig2++;
                            variablescan2pretrig++;
                            for (int k = 0; k < max_len2; k++)
                            {
                                tempDoZ2.TryDequeue(out value4);
                                tempDoX2.TryDequeue(out value5);
                                tempDoI2.TryDequeue(out value6);

                                if (inverse_scan1.Checked)
                                {
                                    file5.WriteLine(variablescan2pretrig + " " + (-value4).ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {
                                    file5.WriteLine(variablescan2pretrig + " " + value4.ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }
                            saveprofilestrig2 = queuetimetriggerscan2 * frequencyHzscan2;

                            if (m_iNumberProfilesToSaveCntpretrig2 >= m_iNumberProfilesToSaveMaxpretrig2 * frequencyHzscan2 + saveprofilestrig2)
                            {

                                m_bSaveOncetrigscan2 = false;
                                file5.Close();
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;

                                textboxstatusscan1trigger2.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger2.Text = "Données enregistrées ! "; });

                                scan2queuestop = true;

                                button9.BackColor = Color.Red;


                                lastvalueeascan2 = ea1Valuesavescan2;

                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                }

                if ((withoutpretriggerscan2 == true) && (file5 != null) && (tempDoZ2.Count >= max_len2))
                {
                    if (lastvalueeascan2 == 1)
                    {
                        double value4;
                        double value5;
                        int value6;
                        textboxstatusscan1trigger2.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger2.Text = "En cours d'enregistrement de pré-trigger !"; });

                        try
                        {
                            m_iNumberProfilesToSaveCntpretrig2++;
                            variablescan2pretrig++;
                            for (int k = 0; k < max_len2; k++)
                            {
                                tempDoZ2.TryDequeue(out value4);
                                tempDoX2.TryDequeue(out value5);
                                tempDoI2.TryDequeue(out value6);

                                if (inverse_scan2.Checked)
                                {
                                    file5.WriteLine(variablescan2pretrig + " " + (-value4).ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));
                                }
                                else
                                {
                                    file5.WriteLine(variablescan2pretrig + " " + value4.ToString("F4", point) + " " + value5.ToString("F4", point) + " " + value6.ToString(point));


                                }
                            }


                            if (m_iNumberProfilesToSaveCntpretrig2 >= m_iNumberProfilesToSaveMaxpretrig2 * frequencyHzscan2)
                            {

                                withoutpretriggerscan2 = false;
                                file5.Close();
                                while (tempDoZ2.TryDequeue(out _)) ;
                                while (tempDoX2.TryDequeue(out _)) ;
                                while (tempDoI2.TryDequeue(out _)) ;

                                textboxstatusscan1trigger2.Invoke((MethodInvoker)delegate { textboxstatusscan1trigger2.Text = "Données enregistrées ! "; });


                                button9.BackColor = Color.Red;


                                lastvalueeascan2 = ea1Valuesavescan2;

                            }



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur :" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }

            }

        }

        public float getfreq()
        {
            StringBuilder retBuf = new StringBuilder(256);
            StringBuilder retBuf2 = new StringBuilder(256);

            if (ea3valuefunction == 2 && sourcetrigscan1 == 0 && ea4valuefunctionscan2 == 1 && sourcetrigscan2 == 1)
            {
                int iRes = weCat3D_SDK.EthernetScanner_ReadData(ScannerHandle, "GetAcquisitionLineTime", retBuf, retBuf.Capacity, 0);
                if (iRes == 0)
                {
                    float acquisitionLineTimeUs = float.Parse(retBuf.ToString());
                    frequencyHz = (float)(1.0 / (acquisitionLineTimeUs * 0.000001));
                    frequencyHz = (float)Math.Round(frequencyHz);
                    textBoxfreq.Text = frequencyHz.ToString();
                }

            }
            if (ea3valuefunctionscan2 == 2 && sourcetrigscan2 == 0 && ea4valuefunction == 1 && sourcetrigscan1 == 1)
            {

                int iRes2 = weCat3D_SDK.EthernetScanner_ReadData(ScannerHandle2, "GetAcquisitionLineTime", retBuf2, retBuf2.Capacity, 0);
                if (iRes2 == 0)
                {
                    float acquisitionLineTimeUs = float.Parse(retBuf2.ToString());
                    frequencyHz = (float)(1.0 / (acquisitionLineTimeUs * 0.000001));
                    frequencyHz = (float)Math.Round(frequencyHz);
                    textBoxfreq.Text = frequencyHz.ToString();
                }
            }



            return 0.0f;

        }

        private void getfreqscan1()
        {
            StringBuilder retBuf = new StringBuilder(256);

            int iRes = weCat3D_SDK.EthernetScanner_ReadData(ScannerHandle, "GetAcquisitionLineTime", retBuf, retBuf.Capacity, 0);
            if (iRes == 0)
            {
                float acquisitionLineTimeUs = float.Parse(retBuf.ToString());
                frequencyHzscan1 = (float)(1.0 / (acquisitionLineTimeUs * 0.000001));
                frequencyHzscan1 = (float)Math.Round(frequencyHzscan1);
                textBoxfreqscan1.Text = frequencyHzscan1.ToString();
            }
        }

        private void getfreqscan2()
        {
            StringBuilder retBuf2 = new StringBuilder(256);

            int iRes = weCat3D_SDK.EthernetScanner_ReadData(ScannerHandle2, "GetAcquisitionLineTime", retBuf2, retBuf2.Capacity, 0);
            if (iRes == 0)
            {
                float acquisitionLineTimeUs2 = float.Parse(retBuf2.ToString());
                frequencyHzscan2 = (float)(1.0 / (acquisitionLineTimeUs2 * 0.000001));
                frequencyHzscan2 = (float)Math.Round(frequencyHzscan2);
                if (allCommandsSent)
                {
                    frequencyHzscan2 = frequencyHzscan1;
                }
                textBoxfreqscan2.Text = frequencyHzscan2.ToString();

            }
        }

        private void FormDemo_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            GUI_Timer.Stop();
            pScannerSettigs.Close();
            pScannerSettigs2.Close();
            pScannerSettigs3.Close();

            buttonDisconnect.PerformClick();
            button6_Click(sender, e);

        }

        private void buttonExpert_Click(object sender, EventArgs e)
        {
            if (m_bScannerThreadRunning == true)
            {
                pScannerSettigs.Show();
            }
            else
            {
                MessageBox.Show("Le laser n'est pas connecté. Veuillez connecter le laser pour ouvrir les paramètres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void buttonResetStatistic_Click(object sender, EventArgs e)
        {
            m_iScanner_Frequeny = 0;
            m_iProfile_Counter = 0;
            m_iPicCntErr = 0;
        }

        private void buttonDllFiFoReset_Click(object sender, EventArgs e)
        {
            if (ScannerHandle != (IntPtr)null)
            {
                EthernetScanner_ResetDllFiFo(ScannerHandle);
            }
        }

        private void buttonDllFiFoReset2_Click(object sender, EventArgs e)
        {
            if (ScannerHandle != (IntPtr)null)
            {
                EthernetScanner_ResetDllFiFo(ScannerHandle);
            }
        }

        /*private void buttonSaveOnce_Click(object sender, EventArgs e)
        {
            m_iNumberProfilesToSaveCnt = 0;
            StringBuilder strScanDataFileName = new StringBuilder();
            strScanDataFileName.Append(strApplicationPath);
            strScanDataFileName.Append("\\ScanData.txt");
            file = new System.IO.StreamWriter(strScanDataFileName.ToString());

            m_iNumberProfilesToSaveMax = Int32.Parse(textBoxNumberOfProfilesToSave.Text);
            m_bSaveOnce = true;
        }*/

        private void buttonSaveOnce_Click(object sender, EventArgs e)
        {
            m_iNumberProfilesToSaveCnt = 0;
            variable = -1;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files|*.txt";
            saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                string fileFormat = Path.GetExtension(saveFileDialog1.FileName);
                textBoxFileNameScan1.Text = fileName + fileFormat;

                file = new System.IO.StreamWriter(saveFileDialog1.FileName);
                file.WriteLine("Frequence :" + frequencyHzscan1);
                file.WriteLine("nb\tZ\tX\tI");

                if (string.IsNullOrEmpty(textBoxNumberOfProfilesToSave.Text))
                {
                    MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    m_iNumberProfilesToSaveMax = Int32.Parse(textBoxNumberOfProfilesToSave.Text);
                    m_bSaveOnce = true;
                }

            }
        }

        public void StopResetStart()
        {

            //this code is an example on what is the best behaviour to reset the Picture counter or any other counter in the scanner.
            //normally when the client sends SetResetPicutreCounter command, there could be already scans in the DLL
            //FiFo or in the TCP stack in the operating system that are transmited before the scanner received the
            //command. Thus it is importent when reseting a counter to do the follwing:
            // 1. Stop Acquistion
            // 2. make sure the scanner does not send any new scans and the DLL FiFo is empty
            // 3. send the appropriate counter reset command
            // 4. Start acquisition
            // after that, the scans will be transmitted with new counter value

            //Reset event
            //ResetEvent(m_Event);

            //send stop
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStop\r");
            EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);

            //clear buffer
            while (true)
            {
                int iRet = EthernetScanner_GetXZIExtended(ScannerHandle,
                                                            m_doX,
                                                            m_doZ,
                                                            m_iIntensity,
                                                            m_iPeakWidth,
                                                            iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX * iETHERNETSCANNER_SCANXMAX,
                                                            m_iEncoder,
                                                            m_bUSRIO,
                                                            0,
                                                            null,
                                                            0,
                                                            m_iPicCnt);
                if (iRet == -1)
                {
                    break;
                }
            }
            //send reset
            buffer = Encoding.ASCII.GetBytes("SetResetPictureCounter\rSetResetBaseTimeCounter\r");
            EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);

            //send start
            buffer = Encoding.ASCII.GetBytes("SetAcquisitionStart\r");
            EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
            Debug.WriteLine("EthernetScanner_WriteData: SetAcquisitionStart\r");
        }

        public void StopResetStart2()
        {

            //this code is an example on what is the best behaviour to reset the Picture counter or any other counter in the scanner.
            //normally when the client sends SetResetPicutreCounter command, there could be already scans in the DLL
            //FiFo or in the TCP stack in the operating system that are transmited before the scanner received the
            //command. Thus it is importent when reseting a counter to do the follwing:
            // 1. Stop Acquistion
            // 2. make sure the scanner does not send any new scans and the DLL FiFo is empty
            // 3. send the appropriate counter reset command
            // 4. Start acquisition
            // after that, the scans will be transmitted with new counter value

            //Reset event
            //ResetEvent(m_Event);

            //send stop
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStop\r");
            EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);

            //clear buffer
            while (true)
            {
                int iRet = EthernetScanner_GetXZIExtended(ScannerHandle2,
                                                            m_doX2,
                                                            m_doZ2,
                                                            m_iIntensity2,
                                                            m_iPeakWidth2,
                                                            iETHERNETSCANNER_PEAKSPERCMOSSCANLINEMAX * iETHERNETSCANNER_SCANXMAX,
                                                            m_iEncoder2,
                                                            m_bUSRIO2,
                                                            0,
                                                            null,
                                                            0,
                                                            m_iPicCnt2);


                if (iRet == -1)
                {
                    break;
                }
            }
            //send reset
            buffer = Encoding.ASCII.GetBytes("SetResetPictureCounter\rSetResetBaseTimeCounter\r");
            EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);

            //send start
            buffer = Encoding.ASCII.GetBytes("SetAcquisitionStart\r");
            EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);
            Debug.WriteLine("EthernetScanner_WriteData: SetAcquisitionStart\r");
        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (ScannerHandle2 != (IntPtr)null)
            {
                return;
            }

            /*textBoxLinearizationTableSerialNumber1.Text = "";
            textBoxLinearizationTableSerialNumber2.Text = "";
            textBoxLinearizationTableSerialNumber3.Text = "";*/

            strIPAddress2 = textBox5.Text;
            string strPort2 = "32001";
            //int iTimeOut2 = Int32.Parse(textBoxTimeOut.Text);
            int iTimeOut2 = 0;
            //start the connection to the Scanner
            ScannerHandle2 = EthernetScanner_Connect(strIPAddress2, strPort2, iTimeOut2);

            //check the connection state with timeout 3000 ms
            DateTime startConnectTime2 = DateTime.Now;
            TimeSpan connectTime2 = new TimeSpan();
            do
            {
                if (connectTime2.TotalMilliseconds > 1500)
                {
                    ScannerHandle2 = EthernetScanner_Disconnect(ScannerHandle2);
                    MessageBox.Show("Erreur : aucune connexion !!!", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Thread.Sleep(10);
                EthernetScanner_GetConnectStatus(ScannerHandle2, iConnectionStatus2);
                connectTime2 = DateTime.Now - startConnectTime2;
            } while (iConnectionStatus2[0] != iETHERNETSCANNER_TCPSCANNERCONNECTED);


            int iGetInfoRes2 = EthernetScanner_GetInfo(ScannerHandle2, m_strScannerInfoXML2, iETHERNETSCANNER_GETINFOSIZEMAX, "xml");
            if (iGetInfoRes2 > 0)
            {
                GetXMLParser(m_strScannerInfoXML2);
                //start the ReadOut-Thread
                m_bScannerThreadRunning2 = true;
                m_bScannerThreadRunning6 = true;
                ScannerThread2 = new Thread(new ThreadStart(ScannerThreadProc2));
                ScannerThread2.Start();
                ScannerThread6 = new Thread(new ThreadStart(ScannerThreadProc6));
                ScannerThread6.Start();
                if (!GUI_Timer.Enabled)
                {
                    GUI_Timer.Start();
                }

                getfreqscan2();

            }
            else
            {
                m_bScannerThreadRunning2 = false;
                m_bScannerThreadRunning6 = false;
                MessageBox.Show("Erreur : aucun paquet d'informations valide !!!", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ScannerThread2 != null && ScannerThread2.IsAlive)
            {
                m_bScannerThreadRunning2 = false;

                if (!ScannerThread2.Join(ThreadJoinTimeoutMilliseconds))
                {


                }

                ScannerThread2 = null;
                ea4valuefunctionscan2 = 0;
            }

            if (ScannerThread6 != null && ScannerThread6.IsAlive)
            {
                m_bScannerThreadRunning6 = false;

                if (!ScannerThread6.Join(ThreadJoinTimeoutMilliseconds))
                {

                }

                ScannerThread6 = null;
            }

            if (ScannerHandle2 != IntPtr.Zero)
            {
                ScannerHandle2 = EthernetScanner_Disconnect(ScannerHandle2);
            }
        }



        private void buttonResetStatistic2_Click(object sender, EventArgs e)
        {
            m_iScanner_Frequeny2 = 0;
            m_iProfile_Counter2 = 0;
            m_iPicCntErr2 = 0;
        }

        private void buttonSaveOnce2_Click(object sender, EventArgs e)
        {

            m_iNumberProfilesToSaveCnt2 = 0;
            variable2 = -1;

            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Filter = "Text Files|*.txt";
            saveFileDialog2.RestoreDirectory = true;
            //saveFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string fileName2 = Path.GetFileNameWithoutExtension(saveFileDialog2.FileName);
                string fileFormat2 = Path.GetExtension(saveFileDialog2.FileName);
                textBoxFileNameScan2.Text = fileName2 + fileFormat2;

                file6 = new System.IO.StreamWriter(saveFileDialog2.FileName);
                file6.WriteLine("Frequence :" + frequencyHzscan2);
                file6.WriteLine("nb\tZ\tX\tI");


                if (string.IsNullOrEmpty(textBoxNumberOfProfilesToSave2.Text))
                {
                    MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    m_iNumberProfilesToSaveMax2 = Int32.Parse(textBoxNumberOfProfilesToSave2.Text);
                    m_bSaveOnce2 = true;

                }
            }
        }




        private bool isSaving = false;

        private void button4_Click(object sender, EventArgs e)
        {
            variabledoublesave = -1;
            m_iNumber2ProfilesToSaveCnt = 0;
            numEnqueued = 0;

            SaveFileDialog saveFileDialog3 = new SaveFileDialog();
            saveFileDialog3.Filter = "Text Files|*.txt";
            saveFileDialog3.RestoreDirectory = true;
            if (saveFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string fileName3 = Path.GetFileNameWithoutExtension(saveFileDialog3.FileName);
                string fileFormat3 = Path.GetExtension(saveFileDialog3.FileName);
                textBoxFileNameScan3.Text = fileName3 + fileFormat3;

                StringBuilder strScanDataFileName = new StringBuilder();
                strScanDataFileName.Append(saveFileDialog3.FileName);

                if (sepfichiers.Checked)
                {
                    sep1 = new System.IO.StreamWriter(strScanDataFileName.ToString());
                    sep1.WriteLine("nb\tZ\tX\tI");

                    string sep2FileName = Path.GetDirectoryName(strScanDataFileName.ToString()) + "\\" +
                                           Path.GetFileNameWithoutExtension(strScanDataFileName.ToString()) + "1" +
                                           Path.GetExtension(strScanDataFileName.ToString());

                    sep2 = new System.IO.StreamWriter(sep2FileName);
                    sep2.WriteLine("distancex(mm) :" + distancex + " " + "distancez(mm) :" + distancez + " " + "Frequence(hz) :" + frequencyHz);
                    sep2.WriteLine("nb\tZ\tX\tI");
                    if (file2 != null)
                    {
                        file2.Close();
                        file2 = null;
                    }
                }
                else
                {
                    file2 = new System.IO.StreamWriter(strScanDataFileName.ToString());
                    file2.WriteLine("distancex(mm) :" + distancex + " " + "distancez(mm) :" + distancez + " " + "Frequence(hz) :" + frequencyHz);
                    file2.WriteLine("nb\tZ\tX\tI");

                    if (sep1 != null)
                    {
                        sep1.Close();
                        sep1 = null;
                    }
                    if (sep2 != null)
                    {
                        sep2.Close();
                        sep2 = null;
                    }
                }
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    m_iNumber2ProfilesToSaveMax = Int32.Parse(textBox3.Text);
                    m_bSaveOnce3 = true;

                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ScannerThread3 != null && ScannerThread3.IsAlive)
            {
                m_bScannerThreadRunning3 = false;

                if (!ScannerThread3.Join(ThreadJoinTimeoutMilliseconds))
                {

                }

                ScannerThread3 = null;
            }

            if (ScannerThread4 != null && ScannerThread4.IsAlive)
            {
                m_bScannerThreadRunning4 = false;

                if (!ScannerThread4.Join(ThreadJoinTimeoutMilliseconds))
                {

                }

                ScannerThread4 = null;
            }

            if (ScannerHandle != IntPtr.Zero)
            {
                ScannerHandle = EthernetScanner_Disconnect(ScannerHandle);
            }

            if (ScannerHandle2 != IntPtr.Zero)
            {
                ScannerHandle2 = EthernetScanner_Disconnect(ScannerHandle2);
            }

            buttonDisconnect_Click(sender, e);
            button3_Click(sender, e);
        }


        private void Connecttow_Click(object sender, EventArgs e)
        {

            buttonConnect_Click(sender, e);
            Thread.Sleep(200);
            button5_Click(sender, e);


            int iGetInfoRes = EthernetScanner_GetInfo(ScannerHandle, m_strScannerInfoXML, iETHERNETSCANNER_GETINFOSIZEMAX, "xml");
            if (iGetInfoRes > 0)
            {
                m_bScannerThreadRunning3 = true;
                m_bScannerThreadRunning4 = true;
                ScannerThread3 = new Thread(new ThreadStart(ScannerThreadProc3));
                ScannerThread3.Start();
                ScannerThread4 = new Thread(new ThreadStart(ScannerThreadProc4));
                ScannerThread4.Start();
                Thread.Sleep(200);

                getfreq();

            }
            else
            {
                m_bScannerThreadRunning3 = false;
                m_bScannerThreadRunning4 = false;
                MessageBox.Show("Erreur : aucun paquet d'informations valide !!!", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void text_distancex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                distancex = double.Parse(text_distancex.Text);

            }
        }



        private void savepretrigger_CheckedChanged(object sender, EventArgs e)
        {
            debut = 1;
        }


        /*float.TryParse(txtMaxBufferSize.Text, out calc);
        if (string.IsNullOrEmpty(txtMaxBufferSize.Text))
        {
            calc = 1.0f;//il faut , et pas un point (.)
        }*/

        private void profilenb_TextChanged(object sender, EventArgs e)
        {
            /*float.TryParse(queueLengthTextBox.Text, out profilesnb);
            if (string.IsNullOrEmpty(queueLengthTextBox.Text))
            {
                calc = 1.0f;//il faut , et pas un point (.)
            }*/
        }

        private void buttonExpert2_Click(object sender, EventArgs e)
        {
            if (m_bScannerThreadRunning2 == true)
            {
                pScannerSettigs2.Show();

            }
            else
            {
                MessageBox.Show("Le laser n'est pas connecté. Veuillez connecter le laser pour ouvrir les paramètres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void parasyncscan2_Click(object sender, EventArgs e)
        {
            if (m_bScannerThreadRunning && m_bScannerThreadRunning2 == true)
            {

                pScannerSettigs3.Show();


            }
            else
            {
                MessageBox.Show("Les lasers ne sont pas connectés. Veuillez connecter les lasers pour ouvrir les Paramètres de Synchronisation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
        bool allCommandsSent = false;


        private void buttonenvoieFreq_Click(object sender, EventArgs e)
        {

            double frequency;
            if (!double.TryParse(textBoxfreq.Text, out frequency))
            {
                return;
            }

            if (ea3valuefunction == 2 && sourcetrigscan1 == 0 && ea4valuefunctionscan2 == 1 && sourcetrigscan2 == 1)
            {
                double microseconds = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
            }
            else if (ea3valuefunctionscan2 == 2 && sourcetrigscan2 == 0 && ea4valuefunction == 1 && sourcetrigscan1 == 1)
            {
                double microseconds = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);
            }
            else
            {
                double microseconds = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
            }


            Thread.Sleep(1500);
            getfreq();

        }





        private string fileName;

        private void brnatttriger_Click(object sender, EventArgs e)
        {
            try
            {
                variablepretrigdouble = -1;
                m_iNumberProfilesToSaveCnttrig = 0;
                lastvalueea = 0;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;

                }
                else
                {
                    return;
                }

                if (seppretrigfichiers.Checked)
                {
                    sep1pretrig = new System.IO.StreamWriter(fileName.ToString());
                    sep1pretrig.WriteLine("distancex :" + distancex + " " + "distancez :" + distancez + " " + "Frequence :" + frequencyHz);
                    sep1pretrig.WriteLine("nb\tZ\tX\tI");
                    string sep2FileName = Path.GetDirectoryName(fileName.ToString()) + "\\" +
                                           Path.GetFileNameWithoutExtension(fileName.ToString()) + "-2" +
                                           Path.GetExtension(fileName.ToString());

                    sep2pretrig = new System.IO.StreamWriter(sep2FileName);
                    sep2pretrig.WriteLine("distancex :" + distancex + " " + "distancez :" + distancez + " " + "Frequence :" + frequencyHz);
                    sep2pretrig.WriteLine("nb\tZ\tX\tI");

                    if (file3 != null)
                    {
                        file3.Close();
                        file3 = null;
                    }
                }
                else
                {

                    file3 = new System.IO.StreamWriter(fileName);
                    file3.WriteLine("distancex :" + distancex + " " + "distancez :" + distancez + " " + "Frequence :" + frequencyHz);
                    file3.WriteLine("nb\tZ\tX\tI");

                    if (sep1pretrig != null)
                    {
                        sep1pretrig.Close();
                        sep1pretrig = null;
                    }
                    if (sep2pretrig != null)
                    {
                        sep2pretrig.Close();
                        sep2pretrig = null;
                    }
                }


                if (string.IsNullOrEmpty(textboxafterpretrigger.Text))
                {
                    MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    m_iNumberProfilesToSaveMaxtrig = Int32.Parse(textboxafterpretrigger.Text);

                    if (savepretrigger.Checked)
                    {
                        m_bSaveOncetrig = true;
                    }
                    else
                    {
                        withoutpretrigger = true;
                    }
                }

                if (m_bSaveOncetrig || withoutpretrigger)
                {
                    brnatttriger.BackColor = Color.Green;
                }
                else
                {
                    brnatttriger.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }


        private void btnvaliderpretrigger_Click(object sender, EventArgs e)
        {
            try
            {

                waitenqueu = true;
                while (tempDoZ1.TryDequeue(out _)) ;
                while (tempDoX1.TryDequeue(out _)) ;
                while (tempDoI1.TryDequeue(out _)) ;
                while (tempDoZ2.TryDequeue(out _)) ;
                while (tempDoX2.TryDequeue(out _)) ;
                while (tempDoI2.TryDequeue(out _)) ;
                progressbartrig.Value = 0;
                text_pretrig2scan.Text = "Attendre pré trigger";

                progressbartrig.Invoke((MethodInvoker)delegate { progressbartrig.Value = 0; });
                float newQueuetime;
                if (float.TryParse(queueLengthTextBox.Text, out newQueuetime) && newQueuetime > 0)
                {
                    queueLengthThreshold = newQueuetime;
                    waitenqueu = false;
                }
                else
                {
                    MessageBox.Show("Entrée invalide. La longueur de la file d'attente doit être un nombre positif.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (tempDoZ1.TryDequeue(out _)) ;
            while (tempDoX1.TryDequeue(out _)) ;
            while (tempDoI1.TryDequeue(out _)) ;
            while (tempDoZ2.TryDequeue(out _)) ;
            while (tempDoX2.TryDequeue(out _)) ;
            while (tempDoI2.TryDequeue(out _)) ;

            text_pretrig2scan.Text = "Attendre pré trigger";

            progressbartrig.Value = 0;
        }

        private void btnvalidatenewtimepretriggerscan1_Click(object sender, EventArgs e)
        {
            try
            {
                waitenqueupretrig1 = true;
                while (tempDoZ1.TryDequeue(out _)) ;
                while (tempDoX1.TryDequeue(out _)) ;
                while (tempDoI1.TryDequeue(out _)) ;

                float newQueuetime;
                if (float.TryParse(textBoxtimetrigger.Text, out newQueuetime) && newQueuetime > 0)
                {
                    queuetimetrigger = newQueuetime;
                    waitenqueupretrig1 = false;
                }
                else
                {
                    MessageBox.Show("Entrée invalide. La longueur de la file d'attente doit être un nombre positif.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            variablescan1pretrig = -1;
            m_iNumberProfilesToSaveCntpretrig = 0;
            lastvalueeascan1 = 0;
            string fileName;


            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                file4 = new StreamWriter(fileName);
                file4.WriteLine("Frequence(hz) :" + frequencyHzscan1);
                file4.WriteLine("nb\tZ\tX\tI");
            }
            else
            {
                return;
            }




            if (string.IsNullOrEmpty(textboxafterpretriggerscan1.Text))
            {
                MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                m_iNumberProfilesToSaveMaxpretrig = Int32.Parse(textboxafterpretriggerscan1.Text);

                if (checkBoxpretriggerscan1.Checked)
                {
                    m_bSaveOncetrigscan1 = true;
                }
                else
                {
                    withoutpretriggerscan1 = true;
                }
            }

            if (m_bSaveOncetrigscan1 || withoutpretriggerscan1)
            {
                button7.BackColor = Color.Green;
            }
            else
            {
                button7.BackColor = Color.Red;
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            variablescan2pretrig = -1;
            m_iNumberProfilesToSaveCntpretrig2 = 0;
            lastvalueeascan2 = 0;
            string fileName2;


            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName2 = saveFileDialog1.FileName;
                file5 = new StreamWriter(fileName2);
                file5.WriteLine("Frequence :" + frequencyHzscan2);
                file5.WriteLine("nb\tZ\tX\tI");
            }
            else
            {
                return;
            }


            if (string.IsNullOrEmpty(textboxafterpretriggerscan2.Text))
            {
                MessageBox.Show("Veuillez spécifier le nombre de profils à enregistrer en secondes.\nREMARQUE : 1 profil = 1 seconde.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                m_iNumberProfilesToSaveMaxpretrig2 = Int32.Parse(textboxafterpretriggerscan2.Text);

                if (checkBoxpretriggerscan2.Checked)
                {
                    m_bSaveOncetrigscan2 = true;
                }
                else
                {
                    withoutpretriggerscan2 = true;
                }
            }

            if (m_bSaveOncetrigscan2 || withoutpretriggerscan2)
            {
                button9.BackColor = Color.Green;
            }
            else
            {
                button9.BackColor = Color.Red;
            }
        }



        private void buttonenvoieFreqscan2_Click(object sender, EventArgs e)
        {
            double frequency;
            if (!double.TryParse(textBoxfreqscan2.Text, out frequency))
            {
                return;
            }


            double microseconds = 1000000 / frequency;
            int microsecondsRounded = (int)Math.Round(microseconds);
            string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(200);

            getfreqscan2();

        }

        private void buttonenvoieFreqscan1_Click(object sender, EventArgs e)
        {
            if (m_bScannerThreadRunning)
            {
                double frequency;
                if (!double.TryParse(textBoxfreqscan1.Text, out frequency))
                {
                    return;
                }
                double microseconds = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
                Thread.Sleep(200);
                getfreqscan1();
            }



        }


        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void textBoxfreq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonenvoieFreq_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxfreqscan1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonenvoieFreqscan1_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxfreqscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonenvoieFreqscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxIPAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonConnect_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button5_Click(sender, e);
                e.Handled = true;
            }

        }



        private void text_distancez_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                distancez = double.Parse(text_distancez.Text);
            }
        }
        private bool reverseFlag = true;

        private void button1_Click(object sender, EventArgs e)
        {
            button6_Click(sender, e);

            string ipAddress = textBoxIPAddress.Text;
            string textBox5Value = textBox5.Text;

            if (reverseFlag == true)
            {
                textBoxIPAddress.Text = textBox5Value;
                textBox5.Text = ipAddress;
            }
            else
            {
                textBox5.Text = textBoxIPAddress.Text;
                textBoxIPAddress.Text = textBox5Value;
            }

            Invoke((MethodInvoker)delegate
            {
                inverse_scan3.Checked = false;
                inverse_scan31.Checked = false;
            });

            reverseFlag = !reverseFlag;

            Connecttow_Click(sender, e);

        }

        private void syncstatus_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3Function=" + 2);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(50);

            byte[] buffer2 = Encoding.ASCII.GetBytes("SetEA4Function=" + 1);
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(50);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 0);
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer3, buffer3.Length);
            Thread.Sleep(50);

            byte[] buffer4 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 1);
            int iRes4 = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer4, buffer4.Length);
            Thread.Sleep(50);




            byte[] buffer5 = Encoding.ASCII.GetBytes("SetEA1Function=" + 3);
            int iRes5 = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer5, buffer5.Length);
            Thread.Sleep(50);

            byte[] buffer6 = Encoding.ASCII.GetBytes("SetEA1InputLoad=" + 1);
            int iRes6 = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer6, buffer6.Length);
            Thread.Sleep(50);
            allCommandsSent = true;
        }


        private void textBoxstartmmscan1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(textBoxstartmmscan1.Text) || string.IsNullOrWhiteSpace(textBoxendmmscan1.Text))
                    {
                        MessageBox.Show("Veuillez entrer des valeurs pour le début et la fin.");
                        return;
                    }

                    if (!int.TryParse(textBoxstartmmscan1.Text, out int startValue) || !int.TryParse(textBoxendmmscan1.Text, out int endValue))
                    {
                        MessageBox.Show("Entrée invalide. Veuillez entrer des valeurs entières valides.");
                        return;
                    }

                    string command = $"SetROI1Z_mm={startValue},{endValue}\r";

                    startrangezmm = startValue;
                    endrangezmm = endValue;

                    byte[] buffer = Encoding.ASCII.GetBytes(command);
                    int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle, buffer, buffer.Length);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Une erreur s'est produite : " + ex.Message);
                }
                e.Handled = true;
            }
        }

        private void textBoxstartmmscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(textBoxstartmmscan2.Text) || string.IsNullOrWhiteSpace(textBoxendmmscan2.Text))
                    {
                        MessageBox.Show("Veuillez entrer des valeurs pour le début et la fin.");
                        return;
                    }

                    if (!int.TryParse(textBoxstartmmscan2.Text, out int startValue2) || !int.TryParse(textBoxendmmscan2.Text, out int endValue2))
                    {
                        MessageBox.Show("Entrée invalide. Veuillez entrer des valeurs entières valides.");
                        return;
                    }

                    string command2 = $"SetROI1Z_mm={startValue2},{endValue2}\r";

                    startrangezmm2 = startValue2;
                    endrangezmm2 = endValue2;

                    byte[] buffer = Encoding.ASCII.GetBytes(command2);
                    int iRes = weCat3D_SDK.EthernetScanner_WriteData(ScannerHandle2, buffer, buffer.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite : " + ex.Message);
                }
                e.Handled = true;
            }
        }

        private void btnhelp_Click(object sender, EventArgs e)
        {
            string pdfFileName = "guide.pdf";
            string filePath = Path.Combine(Application.StartupPath, pdfFileName);

            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
            else
            {
                MessageBox.Show("Le fichier spécifié n'existe pas.");
            }

        }

        /*private Color currentFillColor = Color.WhiteSmoke;
        private Color currentFillColor2 = Color.WhiteSmoke;


        private void panelled1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int circleSize = Math.Min(panelled1.Width, panelled1.Height) - 10;

            Brush fillBrush = new SolidBrush(currentFillColor);

            Color borderColor = Color.Black;
            int borderWidth = 1;
            Pen borderPen = new Pen(borderColor, borderWidth);

            int xPos = (panelled1.Width - circleSize) / 2;
            int yPos = (panelled1.Height - circleSize) / 2;

            g.FillEllipse(fillBrush, xPos, yPos, circleSize, circleSize);

            g.DrawEllipse(borderPen, xPos, yPos, circleSize, circleSize);

            borderPen.Dispose();
            fillBrush.Dispose();
        }



        private void panelled2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int circleSize = Math.Min(panelled2.Width, panelled2.Height) - 10;

            Brush fillBrush = new SolidBrush(currentFillColor2);

            Color borderColor = Color.Black;
            int borderWidth = 1;
            Pen borderPen = new Pen(borderColor, borderWidth);

            int xPos = (panelled2.Width - circleSize) / 2;
            int yPos = (panelled2.Height - circleSize) / 2;

            g.FillEllipse(fillBrush, xPos, yPos, circleSize, circleSize);

            g.DrawEllipse(borderPen, xPos, yPos, circleSize, circleSize);

            borderPen.Dispose();
            fillBrush.Dispose();
        }*/
    }


}
