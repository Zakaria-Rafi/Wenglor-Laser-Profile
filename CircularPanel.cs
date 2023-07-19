using System;
using System.Drawing;
using System.Windows.Forms;

namespace EthernetScannnerDemo
{
    public class CircularPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw the circular shape
            int diameter = Math.Min(Width, Height);
            Rectangle circleRect = new Rectangle(Width / 2 - diameter / 2, Height / 2 - diameter / 2, diameter, diameter);
            g.FillEllipse(new SolidBrush(BackColor), circleRect);
        }
    }
}
