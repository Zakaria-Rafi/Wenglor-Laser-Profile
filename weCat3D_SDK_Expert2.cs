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
    public partial class weCat3D_SDK_Expert2 : Form
    {
        private weCat3D_SDK FormMain = null;
        float m_CScanView1_Z_Start = 0;
        float m_CScanView1_Z_Range = 0;
        float m_CScanView1_X_Range_At_Start = 0;
        float m_CScanView1_X_Range_At_End = 0;
        float m_CScanView1_WidthX = 0;
        float m_CScanView1_WidthZ = 0;
        private bool isButtonOn = false;
        public String strApplicationPath = "";
        public String strModelNumber = "";
        public String strProductVersion = "";
        public String strVendorName = "";
        public String strDescription = "";
        public String strHardwareVersion = "";
        public String strFirwareVersion = "";
        public String strScannerMAC = "";
        public String strSerialNummber = "";

        public weCat3D_SDK_Expert2(Form pFormMain)
        {
            FormMain = pFormMain as weCat3D_SDK;
            InitializeComponent();


            comboBoxsignalselect.Items.Insert(0, "Peak 1");
            comboBoxsignalselect.Items.Insert(1, "Intensity");
            comboBoxsignalselect.Items.Insert(2, "Width");
            comboBoxsignalselect.Items.Insert(3, "Peak 2");

            comboBoxtriggersource.Items.Insert(0, "Internal");
            comboBoxtriggersource.Items.Insert(1, "SyncIn");
            comboBoxtriggersource.Items.Insert(2, "Encoder");
            comboBoxtriggersource.Items.Insert(3, "Software");

            textBoxLinearizationTableSerialNumber1.Text = "";
            textBoxLinearizationTableSerialNumber2.Text = "";
            textBoxLinearizationTableSerialNumber3.Text = "";



            combopinFunctionea1scan2.Items.Insert(0, "sync_in");
            combopinFunctionea1scan2.Items.Insert(1, "sync_out");
            combopinFunctionea1scan2.Items.Insert(2, "input");
            combopinFunctionea1scan2.Items.Insert(3, "output");
            combopinFunctionea1scan2.Items.Insert(4, "encoder_ab");

            comboinmputloadea1scan2.Items.Insert(0, "OFF");
            comboinmputloadea1scan2.Items.Insert(1, "ON");


            comboBoxEA1InputFunctionscan2.Items.Insert(0, "Ub inactive");
            comboBoxEA1InputFunctionscan2.Items.Insert(1, "Ub active");

            comboBoxEA1Outputscan2.Items.Insert(0, "Push-Pull");
            comboBoxEA1Outputscan2.Items.Insert(1, "PNP");
            comboBoxEA1Outputscan2.Items.Insert(2, "NPN");


            comboBoxEA1OutputFunctionscan2.Items.Insert(0, "NO");
            comboBoxEA1OutputFunctionscan2.Items.Insert(1, "NC");




            combopinFunctionea2scan2.Items.Insert(0, "sync_in");
            combopinFunctionea2scan2.Items.Insert(1, "sync_out");
            combopinFunctionea2scan2.Items.Insert(2, "input");
            combopinFunctionea2scan2.Items.Insert(3, "output");
            combopinFunctionea2scan2.Items.Insert(4, "encoder_ab");

            comboinmputloadea2scan2.Items.Insert(0, "OFF");
            comboinmputloadea2scan2.Items.Insert(1, "ON");


            comboBoxEA2InputFunctionscan2.Items.Insert(0, "Ub inactive");
            comboBoxEA2InputFunctionscan2.Items.Insert(1, "Ub active");

            comboBoxEA2Outputscan2.Items.Insert(0, "Push-Pull");
            comboBoxEA2Outputscan2.Items.Insert(1, "PNP");
            comboBoxEA2Outputscan2.Items.Insert(2, "NPN");


            comboBoxEA2OutputFunctionscan2.Items.Insert(0, "NO");
            comboBoxEA2OutputFunctionscan2.Items.Insert(1, "NC");



            combopinFunctionea3scan2.Items.Insert(0, "sync_in");
            combopinFunctionea3scan2.Items.Insert(1, "sync_out");
            combopinFunctionea3scan2.Items.Insert(2, "input");
            combopinFunctionea3scan2.Items.Insert(3, "output");
            combopinFunctionea3scan2.Items.Insert(4, "encoder_ab");

            comboinmputloadea3scan2.Items.Insert(0, "OFF");
            comboinmputloadea3scan2.Items.Insert(1, "ON");


            comboBoxEA3InputFunctionscan2.Items.Insert(0, "Ub inactive");
            comboBoxEA3InputFunctionscan2.Items.Insert(1, "Ub active");

            comboBoxEA3Outputscan2.Items.Insert(0, "Push-Pull");
            comboBoxEA3Outputscan2.Items.Insert(1, "PNP");
            comboBoxEA3Outputscan2.Items.Insert(2, "NPN");


            comboBoxEA3OutputFunctionscan2.Items.Insert(0, "NO");
            comboBoxEA3OutputFunctionscan2.Items.Insert(1, "NC");



            ////
            combopinFunctionea4scan2.Items.Insert(0, "sync_in");
            combopinFunctionea4scan2.Items.Insert(1, "sync_out");
            combopinFunctionea4scan2.Items.Insert(2, "input");
            combopinFunctionea4scan2.Items.Insert(3, "output");
            combopinFunctionea4scan2.Items.Insert(4, "encoder_ab");

            comboinmputloadea4scan2.Items.Insert(0, "OFF");
            comboinmputloadea4scan2.Items.Insert(1, "ON");


            comboBoxEA4InputFunctionscan2.Items.Insert(0, "Ub inactive");
            comboBoxEA4InputFunctionscan2.Items.Insert(1, "Ub active");

            comboBoxEA4Outputscan2.Items.Insert(0, "Push-Pull");
            comboBoxEA4Outputscan2.Items.Insert(1, "PNP");
            comboBoxEA4Outputscan2.Items.Insert(2, "NPN");


            comboBoxEA4OutputFunctionscan2.Items.Insert(0, "NO");
            comboBoxEA4OutputFunctionscan2.Items.Insert(1, "NC");

            settingsComboBox2.Items.Add("0: Default");
            settingsComboBox2.Items.Add("1: Set1");
            settingsComboBox2.Items.Add("2: Set2");

            settingsComboBox2.SelectedIndex = 0;




        }

        private void pScannerSettigs2_Load(object sender, EventArgs e)
        {
            GetXMLDescriptor2_Click(null, null);
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

        private void GetXMLDescriptor2_Click(object sender, EventArgs e)
        {
            int iRes = weCat3D_SDK.EthernetScanner_GetInfo(FormMain.ScannerHandle2,
                                                            FormMain.m_strScannerInfoXML2,
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
        private void ShowExpertData()
        {
            textBoxHTLEncoder2.Text = String.Format("{0}", FormMain.m_iEncoderHTL2);
            textBoxTTLEncoder2.Text = String.Format("{0}", FormMain.m_iEncoderTTL2);

            string strTemp_1 = "";
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodes = null;
            doc.LoadXml(FormMain.m_strScannerInfoXML2.ToString());
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
                comboBoxsignalselect.SelectedIndex = currentValue;

            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);
                comboBoxtriggersource.SelectedIndex = currentValue;

            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxTriggerSourceCurrent.Text = strTemp_1;

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

            }
            textBoxExposureTimeCurrent.Text = strTemp_1;
            double exposureTime = float.Parse(strTemp_1);
            int exposuretimeinHz = (int)(1000000 / exposureTime);

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncout");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSyncOutCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncoutdelay");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSyncOutDelayCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/protocol/profile/signal/enable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSignalEnableCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalselection");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSignalSelectionCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalwidthmin");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSignalWidthMinCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalwidthmax");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSignalWidthMaxCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalstrengthmin");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxSignalStrengthCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/height");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIHeightZCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIOffsetZCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/width");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIWidthXCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIOffsetXCurrent.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/step");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIStepXCurrent.Text = strTemp_1;








            //////////// ea1 /////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea1scan2.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea1scan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea1scan2.SelectedIndex = selectedIndex;
            }
            textBoxEA1InputLoadscan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA1InputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA1InputFunctionscan2.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA1Outputscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA1Outputscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA1OutputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA1OutputFunctionscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOffscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnablescan2.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea1/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounterscan2.Text = strTemp_1;





            //////////// ea2 /////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea2scan2.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea2scan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea2scan2.SelectedIndex = selectedIndex;
            }
            textBoxEA2InputLoadscan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA2InputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA2InputFunctionscan2.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA2Outputscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA2Outputscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA2OutputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA2OutputFunctionscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff2scan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable2scan2.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea2/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter2scan2.Text = strTemp_1;




            //////////// ea3 /////////////


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea3scan2.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea3scan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea3scan2.SelectedIndex = selectedIndex;
            }
            textBoxEA3InputLoadscan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA3InputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA3InputFunctionscan2.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA3Outputscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA3Outputscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA3OutputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA3OutputFunctionscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff3scan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable3scan2.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter3scan2.Text = strTemp_1;



            //////////// ea4 /////////////


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                combopinFunctionea4scan2.SelectedIndex = selectedIndex;
            }
            textBoxpinfunctionea4scan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/inputfunctionload");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboinmputloadea4scan2.SelectedIndex = selectedIndex;
            }
            textBoxEA4InputLoadscan2.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/inputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA4InputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA4InputFunctionscan2.Text = strTemp_1;




            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/output");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1) - 1;

                comboBoxEA4Outputscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA4Outputscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/outputfunction");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

                int selectedIndex = int.Parse(strTemp_1);

                comboBoxEA4OutputFunctionscan2.SelectedIndex = selectedIndex;
            }
            textBoxEA4OutputFunctionscan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionlaseroff");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxLaserOff4scan2.Text = strTemp_1;


            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionprofileenable");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxProfileEnable4scan2.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/functionresetcounter");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxResetCounter4scan2.Text = strTemp_1;







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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetAcquisitionLineTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetAcquisitionLineTime.Text))
            {
                MessageBox.Show("Veuillez entrer un temps de ligne d'acquisition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionLineTime=" + textBoxSetAcquisitionLineTime.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetExposureTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetExposureTime.Text))
            {
                MessageBox.Show("Veuillez entrer un temps d'exposition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetExposureTime=" + textBoxSetExposureTime.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSyncOut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOut.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOut=" + textBoxSetSyncOut.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSyncOutDelay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOutDelay.Text))
            {
                MessageBox.Show("Veuillez entrer un délai de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOutDelay=" + textBoxSetSyncOutDelay.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSignalEnable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalEnable.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur d'activation du signal.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalEnable=" + textBoxSetSignalEnable.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSignalSelection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalSelection.Text))
            {
                MessageBox.Show("Veuillez entrer une sélection de signal.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalSelection=" + textBoxSetSignalSelection.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSignalWidthMin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalWidthMin.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de signal minimale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalWidthMin=" + textBoxSetSignalWidthMin.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSignalWidthMax_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalWidthMax.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de signal maximale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalWidthMax=" + textBoxSetSignalWidthMax.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetSignalStrengthMin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSignalStrengthMin.Text))
            {
                MessageBox.Show("Veuillez entrer une intensité de signal minimale.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSignalSstrengthMin=" + textBoxSetSignalStrengthMin.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetROIHeightZ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetROIHeightZ.Text))
            {
                MessageBox.Show("Veuillez entrer une hauteur de ROI Z.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1HeightZ=" + textBoxSetROIHeightZ.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetROIOffsetZ_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetZ.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI Z.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetZ=" + textBoxSetROIOffsetZ.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetROIWidthXCurrent_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIWidthX.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1WidthX=" + textBoxSetROIWidthX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }

        private void buttonSetROIStepX_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIStepX.Text))
            {
                MessageBox.Show("Veuillez entrer un pas de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1StepX=" + textBoxSetROIStepX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }

        private void buttonSsetROIOffsetX_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetX.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetX=" + textBoxSetROIOffsetX.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSsetAcquissitionStart_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonSetAcquisitionStop_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonResetSettings_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetSettings");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonResetEncoder_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetEncoder");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonResetPicCnt_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetPictureCounter");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonResetBaseTime_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetResetBaseTimeCounter");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonStopResetStartSequence_Click(object sender, EventArgs e)
        {
            FormMain.StopResetStart2();
        }

        private void SaveXML_Click(object sender, EventArgs e)
        {
            int iRes = weCat3D_SDK.EthernetScanner_GetInfo(FormMain.ScannerHandle2,
                                                           FormMain.m_strScannerInfoXML2,
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

                file.Write(FormMain.m_strScannerInfoXML2);

                file.Close();
            }
        }

        private void sendtriggersource_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxtriggersource.SelectedIndex;
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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void sendsignalselect_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxsignalselect.SelectedIndex;
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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void ea1envoiescan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea1scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttoninputloadscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea1scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA1InputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1InputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA1Outputscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1Outputscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA1OutputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA1OutputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonLaserOffscan2_Click(object sender, EventArgs e)
        {
            int laserOffValue = Convert.ToInt32(textBoxLaserOffscan2.Text);
            int oppositeValue = 1 - laserOffValue;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxLaserOffscan2.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnablescan2_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable = Convert.ToInt32(textBoxProfileEnablescan2.Text);
            int oppositeValue = 1 - FunctionProfileEnable;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxProfileEnablescan2.Text = oppositeValue.ToString();
        }

        private void buttonResetCounterscan2_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter = Convert.ToInt32(textBoxResetCounterscan2.Text);
            int oppositeValue = 1 - FunctionResetCounter;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxResetCounterscan2.Text = oppositeValue.ToString();
        }

        private void ea2envoiescan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea2scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttoninputload2scan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea2scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA2InputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA2InputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA2Outputscan2_Click(object sender, EventArgs e)
        {

            int selectedIndex = comboBoxEA2Outputscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA2OutputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA2OutputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonLaserOffea2scan2_Click(object sender, EventArgs e)
        {
            int laserOffValue = Convert.ToInt32(textBoxLaserOff2scan2.Text);
            int oppositeValue = 1 - laserOffValue;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxLaserOff2scan2.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnableea2scan2_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable = Convert.ToInt32(textBoxProfileEnable2scan2.Text);
            int oppositeValue = 1 - FunctionProfileEnable;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxProfileEnable2scan2.Text = oppositeValue.ToString();
        }

        private void buttonResetCounterea2scan2_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter = Convert.ToInt32(textBoxResetCounter2scan2.Text);
            int oppositeValue = 1 - FunctionResetCounter;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxResetCounter2scan2.Text = oppositeValue.ToString();
        }

        private void ea3envoiescan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea3scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttoninputload3scan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea3scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA3InputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3InputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA3Outputscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3Outputscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA3OutputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA3OutputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonLaserOff3scan2_Click(object sender, EventArgs e)
        {
            int laserOffValue3 = Convert.ToInt32(textBoxLaserOff3scan2.Text);
            int oppositeValue = 1 - laserOffValue3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxLaserOff3scan2.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnable3scan2_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable3 = Convert.ToInt32(textBoxProfileEnable3scan2.Text);
            int oppositeValue = 1 - FunctionProfileEnable3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxProfileEnable3scan2.Text = oppositeValue.ToString();
        }

        private void buttonResetCounter3scan2_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter3 = Convert.ToInt32(textBoxResetCounter3scan2.Text);
            int oppositeValue = 1 - FunctionResetCounter3;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA2FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxResetCounter3scan2.Text = oppositeValue.ToString();
        }

        private void ea4envoiescan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = combopinFunctionea4scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttoninputload4scan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboinmputloadea4scan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA4InputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4InputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA4Outputscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4Outputscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonEA4OutputFunctionscan2_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxEA4OutputFunctionscan2.SelectedIndex;

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
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttonLaserOff4scan2_Click(object sender, EventArgs e)
        {
            int laserOffValue4 = Convert.ToInt32(textBoxLaserOff3scan2.Text);
            int oppositeValue = 1 - laserOffValue4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionLaserOff=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxLaserOff3scan2.Text = oppositeValue.ToString();
        }

        private void buttonProfileEnable4scan2_Click(object sender, EventArgs e)
        {
            int FunctionProfileEnable4 = Convert.ToInt32(textBoxProfileEnable3scan2.Text);
            int oppositeValue = 1 - FunctionProfileEnable4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionProfileEnable=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxProfileEnable4scan2.Text = oppositeValue.ToString();
        }

        private void buttonResetCounter4scan2_Click(object sender, EventArgs e)
        {
            int FunctionResetCounter4 = Convert.ToInt32(textBoxResetCounter3scan2.Text);
            int oppositeValue = 1 - FunctionResetCounter4;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA4FunctionResetCounter=" + oppositeValue.ToString());
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            textBoxResetCounter3scan2.Text = oppositeValue.ToString();
        }

        private void Savebtn2_Click(object sender, EventArgs e)
        {
            int selectedIndex = settingsComboBox2.SelectedIndex;
            string command = "SetSettingsSave=" + selectedIndex + "\r";

            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void Loadbtn2_Click(object sender, EventArgs e)
        {
            int selectedIndex = settingsComboBox2.SelectedIndex;
            string command = "SetSettingsLoad=" + selectedIndex + "\r";
            byte[] buffer = Encoding.ASCII.GetBytes(command);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void buttononofflaser_Click(object sender, EventArgs e)
        {
            int value = 1;

            if (isButtonOn)
            {
                isButtonOn = false;
                buttononofflaser.Text = "OFF";
                buttononofflaser.BackColor = Color.Red;
                byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + value.ToString());//ne pas utilier SetEA1FunctionLaserOff
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            }
            else
            {
                value = 0;
                isButtonOn = true;
                buttononofflaser.Text = "ON";
                buttononofflaser.BackColor = Color.Green;
                byte[] buffer = Encoding.ASCII.GetBytes("SetEA1FunctionLaserOff=" + value.ToString());
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            }
        }

        private void btnreboot_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("SetReboot");
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
        }

        private void textBoxSetTriggerSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetTriggerSource_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxSetAcquisitionLineTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetAcquisitionLineTime_Click(sender, e);
                e.Handled = true;
            }

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

        private void comboBoxtriggersource_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                sendtriggersource_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxsignalselect_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                sendsignalselect_Click(sender, e);
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

        private void combopinFunctionea1scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea1envoiescan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea1scan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttoninputloadscan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxEA1InputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1InputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxEA1Outputscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1Outputscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA1OutputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEA1OutputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxLaserOffscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOffscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnablescan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnablescan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxResetCounterscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounterscan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void combopinFunctionea2scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea2envoiescan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea2scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload2scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2InputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2InputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2Outputscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2Outputscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA2OutputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEA2OutputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxLaserOff2scan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOffea2scan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxProfileEnable2scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnableea2scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxResetCounter2scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounterea2scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void combopinFunctionea3scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea3envoiescan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea3scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload3scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA3InputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3InputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void comboBoxEA3Outputscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3Outputscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA3OutputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA3OutputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxLaserOff3scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOff3scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable3scan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnable3scan2_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxResetCounter3scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounter3scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void combopinFunctionea4scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ea4envoiescan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboinmputloadea4scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttoninputload4scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA4InputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4InputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA4Outputscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4Outputscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void comboBoxEA4OutputFunctionscan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonEA4OutputFunctionscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxLaserOff4scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonLaserOff4scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxProfileEnable4scan2_KeyDown(object sender, KeyEventArgs e)
        {



            if (e.KeyCode == Keys.Enter)
            {
                buttonProfileEnable4scan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxResetCounter4scan2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonResetCounter4scan2_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
