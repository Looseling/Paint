using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintFinal
{
    public partial class Form1 : Form
    {


        private class ArrayPoints
        {
            private Point[] points;
            private int index;

            public ArrayPoints(int size)
            {
                if (size <= 0) { size = 2; }
                points = new Point[size];
            }

            public void SetPoints(int x, int y)
            {
                if (index >= points.Length)
                {
                    index = 0;
                }
                points[index] = new Point(x, y);
                index++;
            }

            public void ResetPoints()
            {
                index = 0;
            }

            public int GetCounterPoints()
            {
                return index;
            }

            public Point[] GetPoints()
            {
                return points;
            }
        }
        private bool IsMousePressed = false;
        private ArrayPoints arraypoints = new ArrayPoints(2);

        Bitmap map = new Bitmap(100, 100);
        Graphics graphics;
        Pen pen = new Pen(Color.Black, 3f);
        Pen Eraser;

        int index;
        int x, y, sX, sY, cX, cY;

        private void preCompile()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);

            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            Eraser = new Pen(pictureBox1.BackColor, 3f);


        }








        public Form1()
        {
            InitializeComponent();
            preCompile();
        }




        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMousePressed = true;

            //Shapes
            cX = e.X;
            cY = e.Y;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMousePressed = false;
            arraypoints.ResetPoints();

            sX = e.X - cX;
            sY = e.Y - cY;


            switch (index)
            {
                case 3:
                    Rectangle Ellipse = new Rectangle(cX, cY, sX, sY);
                    graphics.DrawEllipse(pen, Ellipse);
                        pictureBox1.Image = map;

                    break;
                case 4:
                    Rectangle RcDraw = new Rectangle(cX, cY, sX, sY);
                    graphics.DrawRectangle(pen, RcDraw);
                        pictureBox1.Image = map;

                    break;
                case 5:
                    graphics.DrawLine(pen, cX, cY, e.X, e.Y);
                    pictureBox1.Image = map;
                    break;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMousePressed)
                return;

            switch (index)
            {
                case 1:
                    arraypoints.SetPoints(e.X, e.Y);
                    if (arraypoints.GetCounterPoints() >= 2)
                    {
                        graphics.DrawLines(pen, arraypoints.GetPoints());
                        pictureBox1.Image = map;
                        arraypoints.SetPoints(e.X, e.Y);
                    }
                    break;
                case 2:
                    arraypoints.SetPoints(e.X, e.Y);
                    if (arraypoints.GetCounterPoints() >= 2)
                    {
                        graphics.DrawLines(Eraser, arraypoints.GetPoints());
                        pictureBox1.Image = map;
                        arraypoints.SetPoints(e.X, e.Y);
                    }
                    break;
                case 3:

                    break;

            }





        }

        private void button_color_change(object sender, EventArgs e)
        {
            pen.Color = ((Button)sender).BackColor;
            label6.BackColor = ((Button)sender).BackColor;
        }

        private void Edit_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog1.Color;
                button_color_edit.BackColor = colorDialog1.Color;
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            pen.Color = ((Button)sender).BackColor;
        }

        private void button34_Click_1(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            pictureBox1.Image = map;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;
            Eraser.Width = trackBar1.Value;

        }

        private void button35_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG(*.JPG)|*.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName);
                }

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            panel9.Height = 250;
            //190, 25
        }



        private void shpes_button_Click(object sender, EventArgs e)
        {
            if (panel9.Height == 29)
            {
                panel9.Height = 250;
            }
            else
                panel9.Height = 29;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog filed = new OpenFileDialog()
            {
                Filter = "JPEG|*jpg|png|*png",
                ValidateNames = true
            })
                if (filed.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.BackgroundImage = Image.FromFile(filed.FileName);
                }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += myPrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
        }

        private void myPrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bm, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }

        //Tools indexes
        #region
        private void Ellipse_button_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void Line_button_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void pen_button_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void Eraser_button_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void Rectangle_button_Click(object sender, EventArgs e)
        {
            index = 4;
        }
        #endregion
    }
}
