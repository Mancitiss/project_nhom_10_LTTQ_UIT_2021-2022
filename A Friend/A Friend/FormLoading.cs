using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Friend
{
    public partial class FormLoading : Form
    {
        private Pen curvePen;
        private Pen circlePen;
        private Rectangle rect;
        private int startAngle = 0;
        private int curveAnge = 120;
        public FormLoading()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            curvePen = new Pen(Color.Black, 10);
            curvePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            curvePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            circlePen = new Pen(Color.LightGray, 10);
            rect = new Rectangle();
            rect.Width = 100;
            rect.Height = 100;
            rect.Location = new Point((int)(this.Width / 2- rect.Width / 2), (int)(this.Height / 2 - rect.Height / 2));
            labelTittle.Location = new Point((int)(this.Width / 2 - labelTittle.Width / 2), rect.Bottom + 10);
        }

        private void FormLoading_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void FormLoading_Resize(object sender, EventArgs e)
        {
            rect.Location = new Point((int)(this.Width / 2- rect.Width / 2), (int)(this.Height / 2 - rect.Height / 2));
            labelTittle.Location = new Point((int)(this.Width / 2 - labelTittle.Width / 2), rect.Bottom + 20);
            this.Invalidate();
        }

        private void FormLoading_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawArc(circlePen, rect, 0, 360);
            e.Graphics.DrawArc(curvePen, rect, startAngle, curveAnge);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            startAngle = (startAngle + 5) % 360;
            this.Invalidate();
        }

        public void StartSpinning()
        {
            timer.Start();
        }

        public void StopSpinning()
        {
            timer.Stop();
        }
    }
}
