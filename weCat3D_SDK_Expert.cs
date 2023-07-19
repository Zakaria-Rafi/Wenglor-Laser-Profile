using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EthernetScannnerDemo
{
    public partial class weCat3D_SDK_Expert : Form
    {
        private weCat3D_SDK FormMain = null;

        float m_CScanView1_Z_Start = 0;
        float m_CScanView1_Z_Range = 0;
        float m_CScanView1_X_Range_At_Start = 0;
        float m_CScanView1_X_Range_At_End = 0;
        float m_CScanView1_WidthX = 0;
        float m_CScanView1_WidthZ = 0;

        public String strApplicationPath = "";
        public String strModelNumber = "";
        public String strProductVersion = "";
        public String strVendorName = "";
        public String strDescription = "";
        public String strHardwareVersion = "";
        public String strFirwareVersion = "";
        public String strScannerMAC = "";
        public String strSerialNummber = "";





        public weCat3D_SDK_Expert(Form pFormMain)
        {
            FormMain = pFormMain as weCat3D_SDK;
            InitializeComponent();


            textBoxLinearizationTableSerialNumber1.Text = "";
            textBoxLinearizationTableSerialNumber2.Text = "";
            textBoxLinearizationTableSerialNumber3.Text = "";

            comboBox1.Items.Insert(0, "Peak 1");
            comboBox1.Items.Insert(1, "Intensity");
            comboBox1.Items.Insert(2, "Width");
            comboBox1.Items.Insert(3, "Peak 2");

            comboBoxsyncmode.Items.Insert(0, "Internal");
            comboBoxsyncmode.Items.Insert(1, "SyncIn");
            comboBoxsyncmode.Items.Insert(2, "Encoder");
            comboBoxsyncmode.Items.Insert(3, "Software");



            combopinFunctionea1.Items.Insert(0, "sync_in");
            combopinFunctionea1.Items.Insert(1, "sync_out");
            combopinFunctionea1.Items.Insert(2, "input");
            combopinFunctionea1.Items.Insert(3, "output");
            combopinFunctionea1.Items.Insert(4, "encoder_ab");

            comboinmputloadea1.Items.Insert(0, "OFF");
            comboinmputloadea1.Items.Insert(1, "ON");


            comboBoxEA1InputFunction.Items.Insert(0, "Ub inactive");
            comboBoxEA1InputFunction.Items.Insert(1, "Ub active");

            comboBoxEA1Output.Items.Insert(0, "Push-Pull");
            comboBoxEA1Output.Items.Insert(1, "PNP");
            comboBoxEA1Output.Items.Insert(2, "NPN");


            comboBoxEA1OutputFunction.Items.Insert(0, "NO");
            comboBoxEA1OutputFunction.Items.Insert(1, "NC");




            combopinFunctionea2.Items.Insert(0, "sync_in");
            combopinFunctionea2.Items.Insert(1, "sync_out");
            combopinFunctionea2.Items.Insert(2, "input");
            combopinFunctionea2.Items.Insert(3, "output");
            combopinFunctionea2.Items.Insert(4, "encoder_ab");

            comboinmputloadea2.Items.Insert(0, "OFF");
            comboinmputloadea2.Items.Insert(1, "ON");


            comboBoxEA2InputFunction.Items.Insert(0, "Ub inactive");
            comboBoxEA2InputFunction.Items.Insert(1, "Ub active");

            comboBoxEA2Output.Items.Insert(0, "Push-Pull");
            comboBoxEA2Output.Items.Insert(1, "PNP");
            comboBoxEA2Output.Items.Insert(2, "NPN");


            comboBoxEA2OutputFunction.Items.Insert(0, "NO");
            comboBoxEA2OutputFunction.Items.Insert(1, "NC");



            ///////////

            combopinFunctionea3.Items.Insert(0, "sync_in");
            combopinFunctionea3.Items.Insert(1, "sync_out");
            combopinFunctionea3.Items.Insert(2, "input");
            combopinFunctionea3.Items.Insert(3, "output");
            combopinFunctionea3.Items.Insert(4, "encoder_ab");

            comboinmputloadea3.Items.Insert(0, "OFF");
            comboinmputloadea3.Items.Insert(1, "ON");


            comboBoxEA3InputFunction.Items.Insert(0, "Ub inactive");
            comboBoxEA3InputFunction.Items.Insert(1, "Ub active");

            comboBoxEA3Output.Items.Insert(0, "Push-Pull");
            comboBoxEA3Output.Items.Insert(1, "PNP");
            comboBoxEA3Output.Items.Insert(2, "NPN");


            comboBoxEA3OutputFunction.Items.Insert(0, "NO");
            comboBoxEA3OutputFunction.Items.Insert(1, "NC");



            ////
            combopinFunctionea4.Items.Insert(0, "sync_in");
            combopinFunctionea4.Items.Insert(1, "sync_out");
            combopinFunctionea4.Items.Insert(2, "input");
            combopinFunctionea4.Items.Insert(3, "output");
            combopinFunctionea4.Items.Insert(4, "encoder_ab");

            comboinmputloadea4.Items.Insert(0, "OFF");
            comboinmputloadea4.Items.Insert(1, "ON");


            comboBoxEA4InputFunction.Items.Insert(0, "Ub inactive");
            comboBoxEA4InputFunction.Items.Insert(1, "Ub active");

            comboBoxEA4Output.Items.Insert(0, "Push-Pull");
            comboBoxEA4Output.Items.Insert(1, "PNP");
            comboBoxEA4Output.Items.Insert(2, "NPN");


            comboBoxEA4OutputFunction.Items.Insert(0, "NO");
            comboBoxEA4OutputFunction.Items.Insert(1, "NC");


            settingsComboBox.Items.Add("0: Default");
            settingsComboBox.Items.Add("1: Set1");
            settingsComboBox.Items.Add("2: Set2");

            settingsComboBox.SelectedIndex = 0;

            //textBoxSendRawCommand.Text = "SetExposureTime=100\r\nSetAcquisitionLineTime=10000";



        }

        private void pScannerSettigs_Load(object sender, EventArgs e)
        {
            GetXMLDescriptor_Click(null, null);
        }

        private void EthernetScannerDllTestExpert_FormClosing(object sender, FormClosingEventArgs e)
        {
            //the window form should be active all the time but if neccessary unvisible
            //will be closed in EthernetScannerDllTest@FormDemo_FormClosed()
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void GetXMLDescriptor_Click(object sender, EventArgs e)
        {
            int iRes = weCat3D_SDK.EthernetScanner_GetInfo(FormMain.ScannerHandle,
                                                                        FormMain.m_strScannerInfoXML,
                                                                        FormMain.iETHERNETSCANNER_GETINFOSIZEMAX,
                                                                        "xml");

            if (iRes == FormMain.iETHERNETSCANNER_GETINFONOVALIDINFO)
            {
                Debug.WriteLine("ETHERNETSCANNER_GETINFONOVALIDINFO\n");
            }
            else if (iRes == FormMain.iETHERNETSCANNER_GETINFOSMALLBUFFER)
            {
                Debug.WriteLine("ETHERNETSCANNER_GETINFOSMALLBUFFER\n");
            }
            else
            {
                ShowExpertData();




            }
        }
        /*
        private void getea1()
        {
            StringBuilder retBuf = new StringBuilder(256);
            StringBuilder retBuf1 = new StringBuilder(256);
            StringBuilder retBuf2 = new StringBuilder(256);
            StringBuilder retBuf3 = new StringBuilder(256);
            StringBuilder retBuf4 = new StringBuilder(256);
            StringBuilder retBuf5 = new StringBuilder(256);
            StringBuilder retBuf6 = new StringBuilder(256);
            StringBuilder retBuf7 = new StringBuilder(256);
            int iRes = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1Function", retBuf, retBuf.Capacity, 0);
            int iRes2 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1InputLoad", retBuf1, retBuf1.Capacity, 0);
            int iRes3 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1InputFunction", retBuf2, retBuf2.Capacity, 0);
            int iRes4 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1Output", retBuf3, retBuf3.Capacity, 0);
            int iRes5 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1OutputFunction", retBuf4, retBuf4.Capacity, 0);
            int iRes6 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1FunctionLaserOff", retBuf5, retBuf5.Capacity, 0);
            int iRes7 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1FunctionProfileEnable", retBuf6, retBuf6.Capacity, 0);
            int iRes8 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA1ResetCounterEncoderHTL", retBuf7, retBuf7.Capacity, 0);
            if (iRes == 0)
            {
                int ea1FunctionValue;
                if (int.TryParse(retBuf.ToString(), out ea1FunctionValue))
                {
                    if (ea1FunctionValue >= 1 && ea1FunctionValue <= combopinFunctionea1.Items.Count)
                    {
                        combopinFunctionea1.SelectedIndex = ea1FunctionValue - 1;
                    }
                    else
                    {
                        combopinFunctionea1.SelectedIndex = -1;
                    }
       
                    textBoxpinfunctionea1.Text = ea1FunctionValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Function as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Function");
            }

            if (iRes2 == 0)
            {
                int ea1InputLoadValue;
                if (int.TryParse(retBuf1.ToString(), out ea1InputLoadValue))
                {
                    if (ea1InputLoadValue == 0 || ea1InputLoadValue == 1)
                    {
                        comboinmputloadea1.SelectedIndex = ea1InputLoadValue;
                    }
                    else
                    {
                        comboinmputloadea1.SelectedIndex = -1;
                    }
                    textBoxEA1InputLoad.Text = ea1InputLoadValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1InputLoad as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1InputLoad");
            }



            if (iRes3 == 0)
            {
                int EA1InputFunctionval;
                if (int.TryParse(retBuf2.ToString(), out EA1InputFunctionval))
                {
                    if (EA1InputFunctionval == 0 || EA1InputFunctionval == 1)
                    {
                        comboBoxEA1InputFunction.SelectedIndex = EA1InputFunctionval;
                    }
                    else
                    {
                        comboBoxEA1InputFunction.SelectedIndex = -1;
                    }
                    textBoxEA1InputFunction.Text = EA1InputFunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }



            if (iRes4 == 0)
            {
                int EA1Outputval;
                if (int.TryParse(retBuf3.ToString(), out EA1Outputval))
                {
                    if (EA1Outputval >= 1 && EA1Outputval <= comboBoxEA1Output.Items.Count)
                    {
                        comboBoxEA1Output.SelectedIndex = EA1Outputval - 1;
                    }
                    else
                    {
                        comboBoxEA1Output.SelectedIndex = -1;
                    }
                    textBoxEA1Output.Text = EA1Outputval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Output as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Output");
            }

            if (iRes5 == 0)
            {
                int EA1Outputfunctionval;
                if (int.TryParse(retBuf4.ToString(), out EA1Outputfunctionval))
                {
                    if (EA1Outputfunctionval == 0 || EA1Outputfunctionval == 1)
                    {
                        comboBoxEA1OutputFunction.SelectedIndex = EA1Outputfunctionval;
                    }
                    else
                    {
                        comboBoxEA1OutputFunction.SelectedIndex = -1;
                    }
                    textBoxEA1OutputFunction.Text = EA1Outputfunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes6 == 0)
            {
                int EA1FunctionLaserOff;
                if (int.TryParse(retBuf5.ToString(), out EA1FunctionLaserOff))
                {
                    textBoxLaserOff.Text = EA1FunctionLaserOff.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes7 == 0)
            {
                int EA1FunctionProfileEnable;
                if (int.TryParse(retBuf6.ToString(), out EA1FunctionProfileEnable))
                {
                    textBoxProfileEnable.Text = EA1FunctionProfileEnable.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes8 == 0)
            {
                int EA1ResetCounterEncoderHTL;
                if (int.TryParse(retBuf7.ToString(), out EA1ResetCounterEncoderHTL))
                {
                    textBoxResetCounter.Text = EA1ResetCounterEncoderHTL.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }

        }


        private void getea2()
        {
            StringBuilder retBuf8 = new StringBuilder(256);
            StringBuilder retBuf9 = new StringBuilder(256);
            StringBuilder retBuf10 = new StringBuilder(256);
            StringBuilder retBuf11 = new StringBuilder(256);
            StringBuilder retBuf12 = new StringBuilder(256);
            StringBuilder retBuf13 = new StringBuilder(256);
            StringBuilder retBuf14 = new StringBuilder(256);
            StringBuilder retBuf15 = new StringBuilder(256);
            int iRes9 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2Function", retBuf8, retBuf8.Capacity, 0);
            int iRes10 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2InputLoad", retBuf9, retBuf9.Capacity, 0);
            int iRes11 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2InputFunction", retBuf10, retBuf10.Capacity, 0);
            int iRes12 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2Output", retBuf11, retBuf11.Capacity, 0);
            int iRes13 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2OutputFunction", retBuf12, retBuf12.Capacity, 0);
            int iRes14 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2FunctionLaserOff", retBuf13, retBuf13.Capacity, 0);
            int iRes15 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2FunctionProfileEnable", retBuf14, retBuf14.Capacity, 0);
            int iRes16 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA2ResetCounterEncoderHTL", retBuf15, retBuf15.Capacity, 0);
            if (iRes9 == 0)
            {
                int ea2FunctionValue;
                if (int.TryParse(retBuf8.ToString(), out ea2FunctionValue))
                {
                    if (ea2FunctionValue >= 1 && ea2FunctionValue <= combopinFunctionea2.Items.Count)
                    {
                        combopinFunctionea2.SelectedIndex = ea2FunctionValue - 1;
                    }
                    else
                    {
                        combopinFunctionea2.SelectedIndex = -1;
                    }

                    textBoxpinfunctionea2.Text = ea2FunctionValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Function as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Function");
            }

            if (iRes10 == 0)
            {
                int ea2InputLoadValue;
                if (int.TryParse(retBuf9.ToString(), out ea2InputLoadValue))
                {
                    if (ea2InputLoadValue == 0 || ea2InputLoadValue == 1)
                    {
                        comboinmputloadea2.SelectedIndex = ea2InputLoadValue;
                    }
                    else
                    {
                        comboinmputloadea2.SelectedIndex = -1;
                    }
                    textBoxEA2InputLoad.Text = ea2InputLoadValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1InputLoad as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1InputLoad");
            }



            if (iRes11 == 0)
            {
                int EA2InputFunctionval;
                if (int.TryParse(retBuf10.ToString(), out EA2InputFunctionval))
                {
                    if (EA2InputFunctionval == 0 || EA2InputFunctionval == 1)
                    {
                        comboBoxEA2InputFunction.SelectedIndex = EA2InputFunctionval;
                    }
                    else
                    {
                        comboBoxEA2InputFunction.SelectedIndex = -1;
                    }
                    textBoxEA2InputFunction.Text = EA2InputFunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }



            if (iRes12 == 0)
            {
                int EA2Outputval;
                if (int.TryParse(retBuf11.ToString(), out EA2Outputval))
                {
                    if (EA2Outputval >= 1 && EA2Outputval <= comboBoxEA1Output.Items.Count)
                    {
                        comboBoxEA2Output.SelectedIndex = EA2Outputval - 1;
                    }
                    else
                    {
                        comboBoxEA2Output.SelectedIndex = -1;
                    }
                    textBoxEA2Output.Text = EA2Outputval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Output as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Output");
            }

            if (iRes13 == 0)
            {
                int EA2Outputfunctionval;
                if (int.TryParse(retBuf13.ToString(), out EA2Outputfunctionval))
                {
                    if (EA2Outputfunctionval == 0 || EA2Outputfunctionval == 1)
                    {
                        comboBoxEA2OutputFunction.SelectedIndex = EA2Outputfunctionval;
                    }
                    else
                    {
                        comboBoxEA2OutputFunction.SelectedIndex = -1;
                    }
                    textBoxEA2OutputFunction.Text = EA2Outputfunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes14 == 0)
            {
                int EA2FunctionLaserOff;
                if (int.TryParse(retBuf13.ToString(), out EA2FunctionLaserOff))
                {
                    textBoxLaserOff2.Text = EA2FunctionLaserOff.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes15 == 0)
            {
                int EA2FunctionProfileEnable;
                if (int.TryParse(retBuf14.ToString(), out EA2FunctionProfileEnable))
                {
                    textBoxProfileEnable2.Text = EA2FunctionProfileEnable.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes16== 0)
            {
                int EA2ResetCounterEncoderHTL;
                if (int.TryParse(retBuf15.ToString(), out EA2ResetCounterEncoderHTL))
                {
                    textBoxResetCounter2.Text = EA2ResetCounterEncoderHTL.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }

        }

        private void getea3()
        {
            StringBuilder retBuf16 = new StringBuilder(256);
            StringBuilder retBuf17 = new StringBuilder(256);
            StringBuilder retBuf18 = new StringBuilder(256);
            StringBuilder retBuf19 = new StringBuilder(256);
            StringBuilder retBuf20 = new StringBuilder(256);
            StringBuilder retBuf21 = new StringBuilder(256);
            StringBuilder retBuf22 = new StringBuilder(256);
            StringBuilder retBuf23= new StringBuilder(256);
            int iRes17 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3Function", retBuf16, retBuf16.Capacity, 0);
            int iRes18 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3InputLoad", retBuf17, retBuf17.Capacity, 0);
            int iRes19 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3InputFunction", retBuf18, retBuf18.Capacity, 0);
            int iRes20 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3Output", retBuf19, retBuf19.Capacity, 0);
            int iRes21 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3OutputFunction", retBuf20, retBuf20.Capacity, 0);
            int iRes22 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3FunctionLaserOff", retBuf21, retBuf21.Capacity, 0);
            int iRes23 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3FunctionProfileEnable", retBuf22, retBuf22.Capacity, 0);
            int iRes24 = weCat3D_SDK.EthernetScanner_ReadData(FormMain.ScannerHandle, "EA3ResetCounterEncoderHTL", retBuf23, retBuf23.Capacity, 0);
            if (iRes17 == 0)
            {
                int ea3FunctionValue;
                if (int.TryParse(retBuf16.ToString(), out ea3FunctionValue))
                {
                    if (ea3FunctionValue >= 1 && ea3FunctionValue <= combopinFunctionea3.Items.Count)
                    {
                        combopinFunctionea3.SelectedIndex = ea3FunctionValue - 1;
                    }
                    else
                    {
                        combopinFunctionea3.SelectedIndex = -1;
                    }

                    textBoxpinfunctionea3.Text = ea3FunctionValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Function as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Function");
            }

            if (iRes18 == 0)
            {
                int ea3InputLoadValue;
                if (int.TryParse(retBuf17.ToString(), out ea3InputLoadValue))
                {
                    if (ea3InputLoadValue == 0 || ea3InputLoadValue == 1)
                    {
                        comboinmputloadea3.SelectedIndex = ea3InputLoadValue;
                    }
                    else
                    {
                        comboinmputloadea3.SelectedIndex = -1;
                    }
                    textBoxEA3InputLoad.Text = ea3InputLoadValue.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1InputLoad as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1InputLoad");
            }



            if (iRes19 == 0)
            {
                int EA3InputFunctionval;
                if (int.TryParse(retBuf18.ToString(), out EA3InputFunctionval))
                {
                    if (EA3InputFunctionval == 0 || EA3InputFunctionval == 1)
                    {
                        comboBoxEA3InputFunction.SelectedIndex = EA3InputFunctionval;
                    }
                    else
                    {
                        comboBoxEA3InputFunction.SelectedIndex = -1;
                    }
                    textBoxEA3InputFunction.Text = EA3InputFunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }



            if (iRes20 == 0)
            {
                int EA3Outputval;
                if (int.TryParse(retBuf19.ToString(), out EA3Outputval))
                {
                    if (EA3Outputval >= 1 && EA3Outputval <= comboBoxEA3Output.Items.Count)
                    {
                        comboBoxEA3Output.SelectedIndex = EA3Outputval - 1;
                    }
                    else
                    {
                        comboBoxEA3Output.SelectedIndex = -1;
                    }
                    textBoxEA3Output.Text = EA3Outputval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the EA1Output as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the EA1Output");
            }

            if (iRes21 == 0)
            {
                int EA3Outputfunctionval;
                if (int.TryParse(retBuf20.ToString(), out EA3Outputfunctionval))
                {
                    if (EA3Outputfunctionval == 0 || EA3Outputfunctionval == 1)
                    {
                        comboBoxEA3OutputFunction.SelectedIndex = EA3Outputfunctionval;
                    }
                    else
                    {
                        comboBoxEA3OutputFunction.SelectedIndex = -1;
                    }
                    textBoxEA3OutputFunction.Text = EA3Outputfunctionval.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes22 == 0)
            {
                int EA3FunctionLaserOff;
                if (int.TryParse(retBuf21.ToString(), out EA3FunctionLaserOff))
                {
                    textBoxLaserOff3.Text = EA3FunctionLaserOff.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes23 == 0)
            {
                int EA3FunctionProfileEnable;
                if (int.TryParse(retBuf22.ToString(), out EA3FunctionProfileEnable))
                {
                    textBoxProfileEnable3.Text = EA3FunctionProfileEnable.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }


            if (iRes24 == 0)
            {
                int EA3ResetCounterEncoderHTL;
                if (int.TryParse(retBuf23.ToString(), out EA3ResetCounterEncoderHTL))
                {
                    textBoxResetCounter3.Text = EA3ResetCounterEncoderHTL.ToString();
                }
                else
                {
                    Console.WriteLine("Error: Could not parse the value of the InputFunction as an integer.");
                }
            }
            else
            {
                Console.WriteLine("Error reading the value of the InputFunction");
            }
        }

        private void getea4()
        {

        }

    */
        private void ShowExpertData()
        {


            textBoxHTLEncoder.Text = String.Format("{0}", FormMain.m_iEncoderHTL);
            textBoxTTLEncoder.Text = String.Format("{0}", FormMain.m_iEncoderTTL);
            string strTemp_1 = "";

            XmlDocument doc = new XmlDocument();
            XmlNodeList nodes = null;
            doc.LoadXml(FormMain.m_strScannerInfoXML.ToString());
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

                string strScannerInfo = strModelNumber + " " + strSerialNummber + " " + strDescription + " " + strVendorName;
                textBoxLinearizationTableSerialNumber1.Text = strScannerInfo;

                strScannerInfo = "ProductVersion: " + strProductVersion + " HW: " + strHardwareVersion + " FW: " + strFirwareVersion + " MAC: " + strScannerMAC;
                textBoxLinearizationTableSerialNumber2.Text = strScannerInfo;

                strScannerInfo = "Range-Z-Start: " + m_CScanView1_Z_Start.ToString() + " mm " +
                    "Range-Z: " + m_CScanView1_Z_Range.ToString() + " mm " +
                    "X-Start: " + m_CScanView1_X_Range_At_Start.ToString() + " mm " +
                    "X-End: " + m_CScanView1_X_Range_At_End.ToString() + " mm " +
                    "Pixel-Max: X: " + m_CScanView1_WidthX.ToString() + " " +
                    "Z: " + m_CScanView1_WidthZ;
                textBoxLinearizationTableSerialNumber3.Text = strScannerInfo;
            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalselection");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);
                comboBox1.SelectedIndex = currentValue;

            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);
                comboBoxsyncmode.SelectedIndex = currentValue;

            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;:
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxTriggerSourceCurrent.Text = strTemp_1;

            /*
            XmlNode currentFunctionNode = doc.SelectSingleNode("/device/settings/usrio/ea1/function/current");
            if (currentFunctionNode != null)
            {
                string currentFunctionValue = currentFunctionNode.InnerText;
                int selectedFunctionIndex;
                if (int.TryParse(currentFunctionValue, out selectedFunctionIndex))
                {
                    // Since the indices of ComboBox start from 0, we need to subtract 1 from the selected index
                    combopinFunctionea1.SelectedIndex = selectedFunctionIndex - 1;
                    textBoxpinfunctionea1.Text = currentFunctionValue;
                }
                else
                {
                    Console.WriteLine("Invalid function value.");
                }
            }
            else
            {
                Console.WriteLine("Current function node not found.");
            }
            */

            nodes = doc.DocumentElement.SelectNodes("/device/settings/acquisitionlinetime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxAcquisitionLineTimeCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/exposuretime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxExposureTimeCurrent.Text = strTemp_1;
            double exposureTime = float.Parse(strTemp_1);
            int exposuretimeinHz = (int)(1000000 / exposureTime);

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncout");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSyncOutCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncoutdelay");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSyncOutDelayCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/protocol/profile/signal/enable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSignalEnableCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalselection");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSignalSelectionCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalwidthmin");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSignalWidthMinCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalwidthmax");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSignalWidthMaxCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalstrengthmin");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxSignalStrengthCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/height");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxROIHeightZCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxROIOffsetZCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/width");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxROIWidthXCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxROIOffsetXCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/step");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
                //strTemp_2 = node.SelectSingleNode("default").InnerText;
                //strTemp_3 = node.SelectSingleNode("command").InnerText;
                //strTemp_4 = node.SelectSingleNode("min").InnerText;
                //strTemp_5 = node.SelectSingleNode("max").InnerText;
            }
            textBoxROIStepXCurrent.Text = strTemp_1;





            //////////// ea1 /////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea1.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea1.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea1.SelectedIndex = selectedIndex;
            }
            textBoxEA1InputLoad.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA1InputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA1InputFunction.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA1Output.SelectedIndex = selectedIndex;
            }
            textBoxEA1Output.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA1OutputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA1OutputFunction.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter.Text = strTemp_1;




            //////////// ea2 /////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea2.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea2.SelectedIndex = selectedIndex;
            }
            textBoxEA2InputLoad.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA2InputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA2InputFunction.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA2Output.SelectedIndex = selectedIndex;
            }
            textBoxEA2Output.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA2OutputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA2OutputFunction.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable2.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter2.Text = strTemp_1;




            //////////// ea3 /////////////


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea3.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea3.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea3.SelectedIndex = selectedIndex;
            }
            textBoxEA3InputLoad.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA3InputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA3InputFunction.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA3Output.SelectedIndex = selectedIndex;
            }
            textBoxEA3Output.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA3OutputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA3OutputFunction.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff3.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable3.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter3.Text = strTemp_1;


            //////////// ea4 /////////////


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea4.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea4.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea4.SelectedIndex = selectedIndex;
            }
            textBoxEA4InputLoad.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA4InputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA4InputFunction.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA4Output.SelectedIndex = selectedIndex;
            }
            textBoxEA4Output.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA4OutputFunction.SelectedIndex = selectedIndex;
            }
            textBoxEA4OutputFunction.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff4.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable4.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter4.Text = strTemp_1;








            double heightZ = float.Parse(textBoxROIHeightZCurrent.Text);
            double widthX = float.Parse(textBoxROIWidthXCurrent.Text);

            String m_sModelNumber = "";
            nodes = doc.DocumentElement.SelectNodes("/device/general");
            foreach (XmlNode node in nodes)
            {
                m_sModelNumber = node.SelectSingleNode("ordernumber").InnerText;
            }

            if (m_sModelNumber.StartsWith("MLWL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor(149359.5 * Math.Pow(heightZ, -0.8678007147) - 20.0), exposuretimeinHz);
                textBoxMaxMeasurementRate.Text = String.Format("{0}", iMaxMeasureRate);
            }
            else if (m_sModelNumber.StartsWith("MLSL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor((1000000.0 / ((0.003458273 * widthX + 0.073443424) * heightZ + 56.0)) * 0.95), exposuretimeinHz);
                textBoxMaxMeasurementRate.Text = String.Format("{0}", iMaxMeasureRate);
            }
        }

        private void buttonSetTriggerSource_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetTriggerSource.Text))
            {
                MessageBox.Show("Veuillez entrer une source de déclenchement.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetTriggerSource=" + textBoxSetTriggerSource.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }
        private void textBoxSetTriggerSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetTriggerSource_Click(sender, e);
                e.Handled = true;
            }
        }

        private void buttonSetAcquisitionLineTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetAcquisitionLineTime.Text))
            {
                MessageBox.Show("Veuillez entrer un temps de ligne d'acquisition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionLineTime=" + textBoxSetAcquisitionLineTime.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void textBoxSetAcquisitionLineTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetAcquisitionLineTime_Click(sender, e);
                e.Handled = true;
            }
        }


        private void buttonSetExposureTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetExposureTime.Text))
            {
                MessageBox.Show("Veuillez entrer un temps d'exposition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetExposureTime=" + textBoxSetExposureTime.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSyncOut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOut.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOut=" + textBoxSetSyncOut.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSyncOutDelay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOutDelay.Text))
            {
                MessageBox.Show("Veuillez entrer un délai de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOutDelay=" + textBoxSetSyncOutDelay.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSignalEnable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalEnable.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur d'activation du signal.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalEnable=" + textBoxSetSignalEnable.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSignalSelection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalSelection.Text))
            {
                MessageBox.Show("Veuillez entrer une sélection de signal.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalSelection=" + textBoxSetSignalSelection.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSignalWidthMin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalWidthMin.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de signal minimale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalWidthMin=" + textBoxSetSignalWidthMin.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSignalWidthMax_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalWidthMax.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de signal maximale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalWidthMax=" + textBoxSetSignalWidthMax.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSignalStrengthMin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalStrengthMin.Text))
            {
                MessageBox.Show("Veuillez entrer une intensité de signal minimale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalSstrengthMin=" + textBoxSetSignalStrengthMin.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetROIHeightZ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetROIHeightZ.Text))
            {
                MessageBox.Show("Veuillez entrer une hauteur de ROI Z.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1HeightZ=" + textBoxSetROIHeightZ.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }



        private void buttonSetROIOffsetZ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetZ.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI Z.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetZ=" + textBoxSetROIOffsetZ.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);

        }

        private void buttonSetROIWidthXCurrent_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIWidthX.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1WidthX=" + textBoxSetROIWidthX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }

        private void buttonSetROIStepX_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIStepX.Text))
            {
                MessageBox.Show("Veuillez entrer un pas de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1StepX=" + textBoxSetROIStepX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }

        private void buttonSsetROIOffsetX_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetX.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetX=" + textBoxSetROIOffsetX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }

        private void buttonSendRawCommand_Click(object sender, EventArgs e)
        {
            //NewLineControlSequence in the EditBox will be replaced with the 0x0D
            string strTemp = textBoxSendRawCommand.Text.Replace("\r\n", "\r");

            if (string.IsNullOrWhiteSpace(strTemp))
            {
                MessageBox.Show("Veuillez entrer une commande brute.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes(strTemp);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSsetAcquissitionStart_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetAcquisitionStop_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonResetSettings_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetSettings");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonResetEncoder_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetEncoder");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonResetPicCnt_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetPictureCounter");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonResetBaseTime_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetBaseTimeCounter");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonStopResetStartSequence_Click(object sender, EventArgs e)
        {
            FormMain.StopResetStart();
        }

        private void SaveXML_Click(object sender, EventArgs e)
        {
            int iRes = weCat3D_SDK.EthernetScanner_GetInfo(FormMain.ScannerHandle,
                                                            FormMain.m_strScannerInfoXML,
                                                            FormMain.iETHERNETSCANNER_GETINFOSIZEMAX,
                                                            "xml");

            if (iRes <= 0)
            {
                MessageBox.Show("Error: could not receive XML from the scanner", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (iRes == FormMain.iETHERNETSCANNER_GETINFOSMALLBUFFER)
            {
                Debug.WriteLine("ETHERNETSCANNER_GETINFOSMALLBUFFER\n");
            }
            else
            {
                StringBuilder strScanDataFileName = new StringBuilder();
                String strFileName = FormMain.strModelNumber + "_" + FormMain.strIPAddress + ".xml";
                strScanDataFileName.Append(FormMain.strApplicationPath);
                strScanDataFileName.Append("\\");
                strScanDataFileName.Append(strFileName);

                System.IO.StreamWriter file = new System.IO.StreamWriter(strScanDataFileName.ToString());

                file.Write(FormMain.m_strScannerInfoXML);

                file.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;
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

            string command = "SetSignalSelection=" + valueToSend.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void btnreboot_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetReboot");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void sendtriggeramount_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxsyncmode.SelectedIndex;
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

            string command = "SetTriggerSource=" + valueToSend.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }
        private bool isButtonOn = false;

        //

        private void buttononofflaser_Click(object sender, EventArgs e)
        {
            int value = 1;

            if (isButtonOn)
            {
                isButtonOn = false;
                buttononofflaser.Text = "OFF";
                buttononofflaser.BackColor = Color.Red;
                byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + value.ToString());//ne pas utilier SetEA1FunctionLaserOff
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            }
            else
            {
                value = 0;
                isButtonOn = true;
                buttononofflaser.Text = "ON";
                buttononofflaser.BackColor = Color.Green;
                byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + value.ToString());
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            }

        }

        private void ea1envoie_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea1.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                case 3:
                    valueToSend = 4;
                    break;
                case 4:
                    valueToSend = 5;
                    break;
                default:
                    return;
            }

            string command = "SetEA1Function=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttoninputload_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea1.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA1InputLoad=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA1InputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1InputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA1InputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA1Output_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1Output.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetEA1Output=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA1OutputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1OutputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA1OutputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonLaserOff_Click(object sender, EventArgs e)
        {
            int laserOffValue = Convert.ToInt32(textBoxLaserOff.Text);
            int oppositeValue = 1 - laserOffValue;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxLaserOff.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnable_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable = Convert.ToInt32(textBoxProfileEnable.Text);
            int oppositeValue = 1 - FunctionProfileEnable;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxProfileEnable.Text = oppositeValue.ToString();
        }

        private void buttonResetCounter_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter = Convert.ToInt32(textBoxResetCounter.Text);
            int oppositeValue = 1 - FunctionResetCounter;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxResetCounter.Text = oppositeValue.ToString();
        }

        private void ea2envoie_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea2.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                case 3:
                    valueToSend = 4;
                    break;
                case 4:
                    valueToSend = 5;
                    break;
                default:
                    return;
            }

            string command = "SetEA2Function=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttoninputload2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea2.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA2InputLoad=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA2InputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA2InputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA2InputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);



        }

        private void buttonEA2Output_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA2Output.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetEA2Output=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA2OutputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA2OutputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA2OutputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonLaserOffea2_Click(object sender, EventArgs e)
        {
            int laserOffValue = Convert.ToInt32(textBoxLaserOff2.Text);
            int oppositeValue = 1 - laserOffValue;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxLaserOff2.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnableea2_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable = Convert.ToInt32(textBoxProfileEnable2.Text);
            int oppositeValue = 1 - FunctionProfileEnable;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxProfileEnable2.Text = oppositeValue.ToString();
        }

        private void buttonResetCounterea2_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter = Convert.ToInt32(textBoxResetCounter2.Text);
            int oppositeValue = 1 - FunctionResetCounter;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxResetCounter2.Text = oppositeValue.ToString();
        }

        private void ea3envoie_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea3.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                case 3:
                    valueToSend = 4;
                    break;
                case 4:
                    valueToSend = 5;
                    break;
                default:
                    return;
            }

            string command = "SetEA3Function=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttoninputload3_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea3.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA3InputLoad=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA3InputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3InputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA3InputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA3Output_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3Output.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetEA3Output=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA3OutputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3OutputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA3OutputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonLaserOff3_Click(object sender, EventArgs e)
        {
            int laserOffValue3 = Convert.ToInt32(textBoxLaserOff3.Text);
            int oppositeValue = 1 - laserOffValue3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxLaserOff3.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnable3_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable3 = Convert.ToInt32(textBoxProfileEnable3.Text);
            int oppositeValue = 1 - FunctionProfileEnable3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxProfileEnable3.Text = oppositeValue.ToString();
        }

        private void buttonResetCounter3_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter3 = Convert.ToInt32(textBoxResetCounter3.Text);
            int oppositeValue = 1 - FunctionResetCounter3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxResetCounter3.Text = oppositeValue.ToString();
        }

        private void ea4envoie_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea4.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                case 3:
                    valueToSend = 4;
                    break;
                case 4:
                    valueToSend = 5;
                    break;
                default:
                    return;
            }

            string command = "SetEA4Function=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttoninputload4_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea4.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA4InputLoad=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA4InputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4InputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA4InputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA4Output_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4Output.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner une option dans la liste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int valueToSend;
            switch (selectedIndex)
            {
                case 0:
                    valueToSend = 1;
                    break;
                case 1:
                    valueToSend = 2;
                    break;
                case 2:
                    valueToSend = 3;
                    break;
                default:
                    return;
            }

            string command = "SetEA4Output=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonEA4OutputFunction_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4OutputFunction.SelectedIndex;

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
                default:
                    return;
            }

            string command = "SetEA4OutputFunction=" + valueToSend.ToString() + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonLaserOff4_Click(object sender, EventArgs e)
        {
            int laserOffValue4 = Convert.ToInt32(textBoxLaserOff3.Text);
            int oppositeValue = 1 - laserOffValue4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxLaserOff4.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnable4_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable4 = Convert.ToInt32(textBoxProfileEnable3.Text);
            int oppositeValue = 1 - FunctionProfileEnable4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxProfileEnable4.Text = oppositeValue.ToString();
        }

        private void buttonResetCounter4_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter4 = Convert.ToInt32(textBoxResetCounter3.Text);
            int oppositeValue = 1 - FunctionResetCounter4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            textBoxResetCounter3.Text = oppositeValue.ToString();
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = settingsComboBox.SelectedIndex;
            string command = "SetSettingsSave=" + selectedIndex + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void Loadbtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = settingsComboBox.SelectedIndex;
            string command = "SetSettingsLoad=" + selectedIndex + "\r";

            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void textBoxSetExposureTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetExposureTime_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSyncOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOut_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSyncOutDelay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOutDelay_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxSetROIHeightZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIHeightZ_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetROIOffsetZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIOffsetZ_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetROIWidthX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIWidthXCurrent_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetROIStepX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIStepX_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetROIOffsetX_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSsetROIOffsetX_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSignalEnable_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSignalEnable_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSignalSelection_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSignalSelection_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSignalWidthMin_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSignalWidthMin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSignalWidthMax_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSignalWidthMax_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSignalStrengthMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSignalStrengthMin_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxsyncmode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendtriggeramount_Click(sender, e);
                e.Handled = true;
            }
        }

        private void combopinFunctionea1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea1envoie_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA1InputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1InputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA1Output_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1Output_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA1OutputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1OutputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxLaserOff_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOff_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnable_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxResetCounter_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounter_Click(sender, e);
                e.Handled = true;
            }
        }

        private void combopinFunctionea2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea2envoie_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2InputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2InputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2Output_KeyDown(object sender, KeyEventArgs e)
        {



            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2Output_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2OutputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2OutputFunction_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxLaserOff2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOffea2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnableea2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxResetCounter2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounterea2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void combopinFunctionea3_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea3envoie_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboinmputloadea3_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload3_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA3InputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3InputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA3Output_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3Output_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA3OutputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3OutputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxLaserOff3_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOff3_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnable3_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxResetCounter3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounter3_Click(sender, e);
                e.Handled = true;
            }

        }

        private void combopinFunctionea4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ea4envoie_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload4_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxEA4InputFunction_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4InputFunction_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA4Output_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4Output_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxEA4OutputFunction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4OutputFunction_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxLaserOff4_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOff4_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable4_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnable4_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxResetCounter4_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounter4_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
