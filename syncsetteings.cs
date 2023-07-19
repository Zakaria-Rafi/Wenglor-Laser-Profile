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
    public partial class syncsetteings : Form
    {
        private weCat3D_SDK FormMain = null;
        public bool maitrescan1;
        public bool maitrescan2;

        public syncsetteings(Form pFormMain)
        {

            FormMain = pFormMain as weCat3D_SDK;
            InitializeComponent();

            ////////////////////////////////////////////////////

            /////////////////////////////////////////

            toolTip1.Active = true;
            toolTip1.AutoPopDelay = 20000;
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
        

            

          

            toolTip1.SetToolTip(labelSetExposureTime, "Définir le temps d'exposition permet de définir la durée d'exposition pour la capture d'images lors de l'utilisation d'un laser, \r" +
                "en réglant le temps pendant lequel le capteur de l'appareil photo est exposé à la lumière.");

            toolTip1.SetToolTip(label20, "Définir le temps d'exposition permet de définir la durée d'exposition pour la capture d'images lors de l'utilisation d'un laser, \r" +
                "en réglant le temps pendant lequel le capteur de l'appareil photo est exposé à la lumière.");
            toolTip1.SetToolTip(label9, "DéfinirSyncOut définit la durée du signal SyncOut pour l'E/A SynccOut en microsecondes,\r " +
                "avec des valeurs de paramètre comprises entre 10 et 100 000 et une valeur par défaut de 1 000 microsecondes.");
            toolTip1.SetToolTip(label18, "DéfinirSyncOut définit la durée du signal SyncOut pour l'E/A SynccOut en microsecondes,\r " +
                "avec des valeurs de paramètre comprises entre 10 et 100 000 et une valeur par défaut de 1 000 microsecondes.");


            toolTip1.SetToolTip(label8, "permet de définir la temporisation du signal de déclenchement SyncOut (haute) en microsecondes pour l'E/A SynccOut,\r" +
                " avec des valeurs de paramètre comprises entre 0 et 100 000 et une valeur par défaut de 0 microseconde.");

            toolTip1.SetToolTip(label10, "permet de définir la temporisation du signal de déclenchement SyncOut (haute) en microsecondes pour l'E/A SynccOut,\r" +
                " avec des valeurs de paramètre comprises entre 0 et 100 000 et une valeur par défaut de 0 microseconde.");

            toolTip1.SetToolTip(label11, "SetROIHeightZ définit la hauteur de la région d'intérêt (ROI) pour la caméra en nombre de lignes \r" +
                "Attention ! \r" +
                "Le nombre de lignes de la caméra à lire a un effet sur la bande passante Ethernet et la fréquence d'acquisition des mesures.");

           

            toolTip1.SetToolTip(label14, "SetROIOffsetZ permet de définir le décalage de la région d'intérêt (ROI) dans la direction Z par rapport à la première ligne. \r" +
                "Autrement dit, cette commande permet de spécifier la position verticale de la ROI par rapport au début de l'image acquise.");



            toolTip1.SetToolTip(label16, "SetROIWidthX permet de définir la largeur de la région d'intérêt (ROI) dans la direction X. \r" +
                " Autrement dit, cette commande permet de spécifier la largeur horizontale de la zone de l'image acquise.");

            toolTip1.SetToolTip(label3, "SetROIWidthX permet de définir la largeur de la région d'intérêt (ROI) dans la direction X. \r" +
                " Autrement dit, cette commande permet de spécifier la largeur horizontale de la zone de l'image acquise.");

            toolTip1.SetToolTip(label15, "SetROIStepX permet de définir la taille de l'étape (pas) pour le déplacement de la région d'intérêt (ROI) dans la direction X. \r" +
                " Cette commande est utilisée pour déplacer la ROI horizontalement avec une certaine distance spécifiée.");

            toolTip1.SetToolTip(label2, "SetROIStepX permet de définir la taille de l'étape (pas) pour le déplacement de la région d'intérêt (ROI) dans la direction X. \r" +
                " Cette commande est utilisée pour déplacer la ROI horizontalement avec une certaine distance spécifiée.");

            toolTip1.SetToolTip(label17, "SetROIOffsetX permet de définir le décalage de la région d'intérêt (ROI) dans la direction X par rapport à la première colonne. \r" +
                " Autrement dit, cette commande permet de spécifier la position horizontale de la ROI par rapport au début de l'image acquise.");

            toolTip1.SetToolTip(label1, "SetROIOffsetX permet de définir le décalage de la région d'intérêt (ROI) dans la direction X par rapport à la première colonne. \r" +
                " Autrement dit, cette commande permet de spécifier la position horizontale de la ROI par rapport au début de l'image acquise.");


        }

        private void pScannerSettigs3_Load(object sender, EventArgs e)
        {
            Updatebtn_Click(null, null);
        }
        string strTemp_1_ea3_scan1;
        string strTemp_1_ea4_scan1;
        string strTemp_1_triggersource_scan1;

        string strTemp_1_ea3_scan2;
        string strTemp_1_ea4_scan2;
        string strTemp_1_triggersource_scan2;

        double syncOutDelay;

        double exposureTimecalc;
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


        double microseconds;
        double microseconds2;


        private void Updatebtn_Click(object sender, EventArgs e)
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



            int iRes2 = weCat3D_SDK.EthernetScanner_GetInfo(FormMain.ScannerHandle2,
                                                FormMain.m_strScannerInfoXML2,
                                                FormMain.iETHERNETSCANNER_GETINFOSIZEMAX,
                                                "xml");

            if (iRes2 == FormMain.iETHERNETSCANNER_GETINFONOVALIDINFO)
            {
                Debug.WriteLine("ETHERNETSCANNER_GETINFONOVALIDINFO\n");
            }
            else if (iRes2 == FormMain.iETHERNETSCANNER_GETINFOSMALLBUFFER)
            {
                Debug.WriteLine("ETHERNETSCANNER_GETINFOSMALLBUFFER\n");
            }
            else
            {
                ShowExpertData1();
            }
            

            if (strTemp_1_ea3_scan1 == "2" && strTemp_1_triggersource_scan1 == "0")
            {
                checkBoxMaitrescan1.Checked = true;
                maitrescan1 = true;
                maitrescan2 = false;
                checkBoxMaitrescan1_CheckedChanged( sender,  e);
            }


            if (strTemp_1_ea4_scan1 == "1" && strTemp_1_triggersource_scan1 == "1")
            {
                checkBoxesclavescan1.Checked = true;
                checkBoxesclavescan1_CheckedChanged(sender, e);
            }




            if (strTemp_1_ea3_scan2 == "2" && strTemp_1_triggersource_scan2 == "0")
            {
                checkBoxMaitrescan2.Checked = true;
                maitrescan1 = false;
                maitrescan2 = true;
                checkBoxMaitrescan2_CheckedChanged(sender, e);
            }
            if (strTemp_1_ea4_scan2 == "1" && strTemp_1_triggersource_scan2 == "1")
            {
                checkBoxesclavescan2.Checked = true;
                checkBoxesclavescan2_CheckedChanged(sender, e);
            }




            labelCalculation.Text = syncOutDelay.ToString() +" "+">=" + " " +exposureTimecalc.ToString();
            if (syncOutDelay < exposureTimecalc)
            {
                textBoxav.Text = "Le délai de sortie de synchronisation doit être supérieur au temps d'exposition.";
                labelCalculation.BackColor = Color.Red;
            }else if (syncOutDelay >= exposureTimecalc)
            {
                textBoxav.Text = "Le délai de sortie de synchronisation est supérieur au temps d'exposition.";
                labelCalculation.BackColor = Color.Green;
            }
            if (string.IsNullOrEmpty(textBoxfreq.Text))
            {
                ShowExpertData();
                ShowExpertData();

            }


        }







        private void ShowExpertData()
        {
            string strTemp_1 = "";

            XmlDocument doc = new XmlDocument();
            XmlNodeList nodes = null;
            doc.LoadXml(FormMain.m_strScannerInfoXML.ToString());



            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/signalselection");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);
               


            }
            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);

            }
            //////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/acquisitionlinetime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            microseconds = double.Parse(strTemp_1);
            double hertz = 1.0 / (microseconds / 1000000.0);

            if (maitrescan1 == true && maitrescan2 == false)
            {
                textBoxfreq.Text = hertz.ToString("F1");

            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/exposuretime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxExposureTimeCurrent1.Text = strTemp_1;
            exposureTimecalc = float.Parse(strTemp_1);
            int exposuretimeinHz = (int)(1000000 / exposureTimecalc);

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncout");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxSyncOutCurrent1.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/syncoutdelay");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxSyncOutDelayCurrent1.Text = strTemp_1;
            syncOutDelay = double.Parse(strTemp_1);


            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/height");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIHeightZCurrent1.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/z1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIOffsetZCurrent1.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/width");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIWidthXCurrent1.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIOffsetXCurrent1.Text = strTemp_1;

            nodes = doc.DocumentElement.SelectNodes("/device/settings/roi/x1/step");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIStepXCurrent1.Text = strTemp_1;



            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea3/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1_ea3_scan1 = node.SelectSingleNode("current").InnerText;
            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/usrio/ea4/function");
            foreach (XmlNode node in nodes)
            {
                 strTemp_1_ea4_scan1 = node.SelectSingleNode("current").InnerText;
            }

            nodes = doc.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                strTemp_1_triggersource_scan1 = node.SelectSingleNode("current").InnerText;
            }



            ///////////////////////////////////////////////////////////////////////////
            double heightZ = float.Parse(textBoxROIHeightZCurrent1.Text);
            double widthX = float.Parse(textBoxROIWidthXCurrent1.Text);

            String m_sModelNumber = "";
            nodes = doc.DocumentElement.SelectNodes("/device/general");
            foreach (XmlNode node in nodes)
            {
                m_sModelNumber = node.SelectSingleNode("ordernumber").InnerText;
            }

            if (m_sModelNumber.StartsWith("MLWL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor(149359.5 * Math.Pow(heightZ, -0.8678007147) - 20.0), exposuretimeinHz);
                textBoxMaxMeasurementRate1.Text = String.Format("{0}", iMaxMeasureRate);
            }
            else if (m_sModelNumber.StartsWith("MLSL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor((1000000.0 / ((0.003458273 * widthX + 0.073443424) * heightZ + 56.0)) * 0.95), exposuretimeinHz);
                textBoxMaxMeasurementRate1.Text = String.Format("{0}", iMaxMeasureRate);
            }
            //////////////////////////////////////////////////////////////////////////
        }

        private void ShowExpertData1()
        {
            string strTemp_1 = "";

            XmlDocument doc2 = new XmlDocument();
            XmlNodeList nodes = null;
            doc2.LoadXml(FormMain.m_strScannerInfoXML2.ToString());



            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc2.DocumentElement.SelectNodes("/device/settings/signalselection");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);
              
            }

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                int currentValue = Int32.Parse(node.SelectSingleNode("current").InnerText);


            }
            //////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc2.DocumentElement.SelectNodes("/device/settings/acquisitionlinetime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            microseconds2 = double.Parse(strTemp_1);
            double hertz2 = 1.0 / (microseconds2 / 1000000.0);

            if (maitrescan2 == true && maitrescan1 == false)
            {
                textBoxfreq.Text = hertz2.ToString("F1");

            }

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/exposuretime");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxExposureTimeCurrent2.Text = strTemp_1;
            double exposureTime2 = float.Parse(strTemp_1);
            int exposuretimeinHz = (int)(1000000 / exposureTime2);

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/syncout");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxSyncOutCurrent2.Text = strTemp_1;

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/syncoutdelay");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxSyncOutDelayCurrent2.Text = strTemp_1;
            //////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////
            nodes = doc2.DocumentElement.SelectNodes("/device/settings/roi/z1/height");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIHeightZCurrent2.Text = strTemp_1;

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/roi/z1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIOffsetZCurrent2.Text = strTemp_1;

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/roi/x1/width");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIWidthXCurrent2.Text = strTemp_1;

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/roi/x1/offset");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;
            }
            textBoxROIOffsetXCurrent2.Text = strTemp_1;

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/roi/x1/step");
            foreach (XmlNode node in nodes)
            {
                strTemp_1 = node.SelectSingleNode("current").InnerText;

            }
            textBoxROIStepXCurrent2.Text = strTemp_1;





            nodes = doc2.DocumentElement.SelectNodes("/device/settings/usrio/ea3/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1_ea3_scan2 = node.SelectSingleNode("current").InnerText;
            }

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/usrio/ea4/function");
            foreach (XmlNode node in nodes)
            {
                strTemp_1_ea4_scan2 = node.SelectSingleNode("current").InnerText;
            }

            nodes = doc2.DocumentElement.SelectNodes("/device/settings/triggersource");
            foreach (XmlNode node in nodes)
            {
                strTemp_1_triggersource_scan2 = node.SelectSingleNode("current").InnerText;
            }









            ///////////////////////////////////////////////////////////////////////////
            /*
            double heightZ = float.Parse(textBoxROIHeightZCurrent2.Text);
            double widthX = float.Parse(textBoxROIWidthXCurrent2.Text);

            String m_sModelNumber = "";
            nodes = doc.DocumentElement.SelectNodes("/device/general");
            foreach (XmlNode node in nodes)
            {
                m_sModelNumber = node.SelectSingleNode("ordernumber").InnerText;
            }

            if (m_sModelNumber.StartsWith("MLWL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor(149359.5 * Math.Pow(heightZ, -0.8678007147) - 20.0), exposuretimeinHz);
                textBoxMaxMeasurementRate2.Text = String.Format("{0}", iMaxMeasureRate);
            }
            else if (m_sModelNumber.StartsWith("MLSL"))
            {
                int iMaxMeasureRate = Math.Min((int)Math.Floor((1000000.0 / ((0.003458273 * widthX + 0.073443424) * heightZ + 56.0)) * 0.95), exposuretimeinHz);
                textBoxMaxMeasurementRate2.Text = String.Format("{0}", iMaxMeasureRate);
            }*/
            //////////////////////////////////////////////////////////////////////////

        }
        ///ligne 1 send and combo box 






        ///////////////////fin //////////


        //ligne 2 send buttons


        private void buttonSetExposureTime1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetExposureTime1.Text))
            {
                MessageBox.Show("Veuillez entrer un temps d'exposition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(textBoxSetExposureTime1.Text, out int exposureTime1))
            {
                MessageBox.Show("Le temps d'exposition doit être un entier valide.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetExposureTime=" + textBoxSetExposureTime1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Updatebtn_Click(sender, e);

            if (maitrescan1 == true && maitrescan2 == false)
            {
                calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (exposureTime1 + int.Parse(textBoxSetExposureTime2.Text)).ToString() + "<" + microseconds.ToString() + " us"; ;

            }
            else if (maitrescan1 == false && maitrescan2 == true)
            {
                calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (exposureTime1 + int.Parse(textBoxSetExposureTime2.Text)).ToString() + "<" + microseconds2.ToString("F1") + " us"; ;

            }
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);


        }

        private void buttonSetSyncOut1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOut1.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOut=" + textBoxSetSyncOut1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
        }

        private void buttonSetSyncOutDelay1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOutDelay1.Text))
            {
                MessageBox.Show("Veuillez entrer un délai de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOutDelay=" + textBoxSetSyncOutDelay1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);

        }



        private void buttonSetExposureTime2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetExposureTime2.Text))
            {
                MessageBox.Show("Veuillez entrer un temps d'exposition.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(textBoxSetExposureTime2.Text, out int exposureTime2))
            {
                MessageBox.Show("Le temps d'exposition doit être un entier valide.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] buffer = Encoding.ASCII.GetBytes("SetExposureTime=" + textBoxSetExposureTime2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            if (maitrescan1 == true && maitrescan2 == false)
            {
                calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (int.Parse(textBoxSetExposureTime1.Text) + exposureTime2).ToString() + "<" + microseconds.ToString("F1") + " us";

            }else if (maitrescan1 == false && maitrescan2 == true)
            {

                calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (int.Parse(textBoxSetExposureTime1.Text) + exposureTime2).ToString() + "<" + microseconds2.ToString("F1") + " us";

            }
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);

        }

        private void buttonSetSyncOut2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOut2.Text))
            {
                MessageBox.Show("Veuillez entrer une valeur de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOut=" + textBoxSetSyncOut2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);
        }

        private void buttonSetSyncOutDelay2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSetSyncOutDelay2.Text))
            {
                MessageBox.Show("Veuillez entrer un délai de synchronisation de sortie.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetSyncOutDelay=" + textBoxSetSyncOutDelay2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);
        }



        private void buttonSetROIWidthXCurrent1_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIWidthX1.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1WidthX=" + textBoxSetROIWidthX1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }

        private void buttonSetROIStepX1_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIStepX1.Text))
            {
                MessageBox.Show("Veuillez entrer un pas de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1StepX=" + textBoxSetROIStepX1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }

        private void buttonSsetROIOffsetX1_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetX1.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetX=" + textBoxSetROIOffsetX1.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
        }


        private void buttonSetROIWidthXCurrent2_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIWidthX2.Text))
            {
                MessageBox.Show("Veuillez entrer une largeur de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1WidthX=" + textBoxSetROIWidthX2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }

        private void buttonSetROIStepX2_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIStepX2.Text))
            {
                MessageBox.Show("Veuillez entrer un pas de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1StepX=" + textBoxSetROIStepX2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }

        private void buttonSsetROIOffsetX2_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = Encoding.ASCII.GetBytes("SetAcquisitionStop");
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(500);

            if (string.IsNullOrWhiteSpace(textBoxSetROIOffsetX2.Text))
            {
                MessageBox.Show("Veuillez entrer un décalage de ROI X.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] buffer = Encoding.ASCII.GetBytes("SetROI1OffsetX=" + textBoxSetROIOffsetX2.Text);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(500);

            byte[] buffer3 = Encoding.ASCII.GetBytes("SetAcquisitionStart");
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }
        ///////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            float frequency;
            if (!float.TryParse(textBoxfreq.Text, out frequency))
            {
                return;
            }

            if (!int.TryParse(textBoxSetExposureTime1.Text, out int exposureTime1))
            {
                MessageBox.Show("Le temps d'exposition doit être un entier valide.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (maitrescan1 == true)
            {
                double microseconds = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
                Thread.Sleep(800);
                frequency = FormMain.getfreq();

     

                if (maitrescan1 == true && maitrescan2 == false)
                {
                    calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (exposureTime1 + int.Parse(textBoxSetExposureTime2.Text)).ToString() + "<" + microseconds.ToString() + " us"; ;

                }
             
            }
            else if (maitrescan2 == true)
            {
                double microseconds2 = 1000000 / frequency;
                int microsecondsRounded = (int)Math.Round(microseconds2);
                string command = "SetAcquisitionLineTime=" + microsecondsRounded.ToString();
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
                Thread.Sleep(800);
                frequency = FormMain.getfreq();

                if (maitrescan1 == false && maitrescan2 == true)
                {
                    calcexpo.Text = textBoxSetExposureTime1.Text + " + " + textBoxSetExposureTime2.Text + " = " + (exposureTime1 + int.Parse(textBoxSetExposureTime2.Text)).ToString() + "<" + microseconds2.ToString("F1") + " us"; ;

                }
            }



        }



     

        private void textBoxSetExposureTime1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetExposureTime1_Click(sender, e);
                e.Handled = true;
            }

        }

        private void textBoxSetSyncOut1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOut1_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetSyncOutDelay1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOutDelay1_Click(sender, e);
                e.Handled = true;
            }
            
        }



        private void textBoxSetROIWidthX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIWidthXCurrent1_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxSetROIStepX1_KeyDown(object sender, KeyEventArgs e)
        {
            
                            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIStepX1_Click(sender, e);
                e.Handled = true;
            }
        }

        private void textBoxSetROIOffsetX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSsetROIOffsetX1_Click(sender, e);
                e.Handled = true;
            }
            
        }

   

        private void textBoxSetExposureTime2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetExposureTime2_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxSetSyncOut2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOut2_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxSetSyncOutDelay2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetSyncOutDelay2_Click(sender, e);
                e.Handled = true;
            }
            
        }


        private void textBoxSetROIWidthX2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIWidthXCurrent2_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxSetROIStepX2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSetROIStepX2_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxSetROIOffsetX2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSsetROIOffsetX2_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void textBoxfreq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
            }
            
        }

        private void checkBoxMaitrescan1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMaitrescan1.Checked)
            {
                checkBoxesclavescan1.Checked = false;

            }
            maitrescan1 = true;
            maitrescan2 = false;

            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3Function=" + 2);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            Thread.Sleep(50);
            byte[] buffer3 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 0);
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer3, buffer3.Length);
            Thread.Sleep(50);
            byte[] buffer5 = Encoding.ASCII.GetBytes("SetEA1Function=" + 3);
            int iRes5 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer5, buffer5.Length);
            Thread.Sleep(50);

            byte[] buffer6 = Encoding.ASCII.GetBytes("SetEA1InputLoad=" + 1);
            int iRes6 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer6, buffer6.Length);
            Thread.Sleep(50);
        }

        private void checkBoxesclavescan1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxesclavescan1.Checked)
            {
                checkBoxMaitrescan1.Checked = false;

            }

            byte[] buffer2 = Encoding.ASCII.GetBytes("SetEA4Function=" + 1);
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer2, buffer2.Length);
            Thread.Sleep(50);

            byte[] buffer4 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 1);
            int iRes4 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer4, buffer4.Length);
        }

        private void checkBoxMaitrescan2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMaitrescan2.Checked)
            {
                checkBoxesclavescan2.Checked = false;

            }
            maitrescan1 = false;
            maitrescan2 = true;
            byte[] buffer = Encoding.ASCII.GetBytes("SetEA3Function=" + 2);
            int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            Thread.Sleep(50);
            byte[] buffer3 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 0);
            int iRes3 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer3, buffer3.Length);
        }

        private void checkBoxesclavescan2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxesclavescan2.Checked)
            {
                checkBoxMaitrescan2.Checked = false;
            }

            byte[] buffer2 = Encoding.ASCII.GetBytes("SetEA4Function=" + 1);
            int iRes2 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer2, buffer2.Length);
            Thread.Sleep(50);

            byte[] buffer4 = Encoding.ASCII.GetBytes("SetTriggerSource=" + 1);
            int iRes4 = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer4, buffer4.Length);
        }

        private void sendroizmmscan1_Click(object sender, EventArgs e)
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

                FormMain.startrangezmm = startValue;
                FormMain.endrangezmm = endValue;

                byte[] buffer = Encoding.ASCII.GetBytes(command);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle, buffer, buffer.Length);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }

            Thread.Sleep(500);
            Updatebtn_Click(sender, e);
        }

        private void textBoxstartmmscan1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendroizmmscan1_Click(sender, e);
                e.Handled = true;
            }

        }

        private void sendroizmmscan2_Click(object sender, EventArgs e)
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

                FormMain.startrangezmm2 = startValue2;
                FormMain.endrangezmm2 = endValue2;

                byte[] buffer = Encoding.ASCII.GetBytes(command2);
                int iRes = weCat3D_SDK.EthernetScanner_WriteData(FormMain.ScannerHandle2, buffer, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
            Thread.Sleep(500);
            Updatebtn_Click(sender, e);
        }

        private void textBoxstartmmscan2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendroizmmscan2_Click(sender, e);
                e.Handled = true;
            }
        }

        private void label25_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            Color borderColor = Color.White; 
            int borderWidth = 4; 

            ControlPaint.DrawBorder(e.Graphics, label.ClientRectangle, borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private void label26_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            Color borderColor = Color.DeepSkyBlue; 
            int borderWidth = 4;

            ControlPaint.DrawBorder(e.Graphics, label.ClientRectangle, borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

     

    }
}
