using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuncMin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double x_1, x_2;

        double intervalTop = 0;
        double intervalBottom = 0;

        double accuracy = 1e-10;

        int iterations = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            double calc;

            Graphics graph = pictureBox1.CreateGraphics();

            Pen pen_black = new Pen(Brushes.Black, 2);
            Pen pen_gray = new Pen(Brushes.Gray, 1);
            Pen pen_red = new Pen(Brushes.Red, 2);

            graph.DrawLine(pen_red, 0, 350, 750, 350);
            graph.DrawLine(pen_red, 250, 0, 250, 500);

            for (double i = -10; i <= 10; i++)
            {
                graph.DrawLine(pen_gray, (float)(250 + i * 30), 0, (float)(250 + i * 30), 500);
            }

            for (double i = -10; i <= 10; i = i + 0.001)
            {
                calc = function(i);
                graph.DrawEllipse(pen_black, (float)(i * 30 + 250), (float)(350 - calc * 10), 1, 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iterations = 0;

            intervalTop = Convert.ToDouble(textBox1.Text);
            intervalBottom = Convert.ToDouble(textBox2.Text);

            if (radioButton1.Checked == true)
                method1();
            if (radioButton2.Checked == true)
                method2();
            if (radioButton3.Checked == true)
                method3();
        }

        double function(double currentX)
        {
            return 2 * Math.Pow(currentX, 3) + 5 * Math.Pow(currentX, 2) - 12 * currentX;
        }

        double ab(double i)
        {
            if (i < 0)
                return -i;
            else
                return i;
        }

        void method1()
        {
            double currentX = 0;

            while ((intervalTop - intervalBottom) >= accuracy)
            {
                iterations++;

                currentX = (intervalTop + intervalBottom) / 2;

                x_1 = currentX - accuracy / 2;
                x_2 = currentX + accuracy / 2;

                if (function(x_1) >= function(x_2))
                    intervalBottom = currentX;
                else
                    intervalTop = currentX;

                currentX = intervalTop - intervalBottom;

                label2.Text = iterations.ToString();
            }

            label1.Text = ((intervalTop + intervalBottom) / 2).ToString();
        }

        void method2()
        {
            double gr = (Math.Sqrt(5) - 1) / 2;

            double c = intervalTop - gr * (intervalTop - intervalBottom);
            double d = intervalBottom + gr * (intervalTop - intervalBottom);

            while (Math.Abs(c - d) > accuracy)
            {
                iterations++;
                label2.Text = iterations.ToString();

                double fc = function(c);
                double fd = function(d);

                if (fc < fd)
                {
                    intervalTop = d;
                    d = c;
                    c = intervalTop - gr * (intervalTop - intervalBottom);
                }
                else
                {
                    intervalBottom = c;
                    c = d;
                    d = intervalBottom + gr * (intervalTop - intervalBottom);
                }
            }

            label1.Text = ((intervalTop + intervalBottom) / 2).ToString();
        }

        void method3()
        {
            double[] fibonachchi = new double[] { 0, 1, 1 };

            double last_3_nums;

            double y_1, y_2;

            int n;

            last_3_nums = (intervalTop - intervalBottom) / (2 * accuracy);

            n = 3;
            while (fibonachchi[2] < last_3_nums)
            {
                n++;
                fibonachchi[0] = fibonachchi[1];
                fibonachchi[1] = fibonachchi[2];
                fibonachchi[2] = fibonachchi[1] + fibonachchi[0];
            }

            x_1 = (intervalTop - intervalBottom) * fibonachchi[0] / fibonachchi[2] + intervalBottom;
            x_2 = (intervalTop - intervalBottom) * fibonachchi[1] / fibonachchi[2] + intervalBottom;

            while (n > 1)
            {
                iterations++;
                label2.Text = iterations.ToString();

                y_1 = function(x_1);
                y_2 = function(x_2);

                if (y_1 <= y_2)
                {
                    intervalTop = x_2;
                    x_2 = x_1;
                    x_1 = intervalBottom + fibonachchi[0] * (intervalTop - intervalBottom) / fibonachchi[2];
                }
                else
                {
                    intervalBottom = x_1;
                    x_1 = x_2;
                    x_2 = intervalBottom + fibonachchi[1] * (intervalTop - intervalBottom) / fibonachchi[2];
                }

                fibonachchi[2] = fibonachchi[1];
                fibonachchi[1] = fibonachchi[0];
                fibonachchi[0] = fibonachchi[2] - fibonachchi[1];

                n--;
            }

            label1.Text = ((intervalTop + intervalBottom) / 2).ToString();
        }
    }
}
