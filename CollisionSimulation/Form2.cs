using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollisionSimulation
{
    public partial class Form2 : Form
    {
        public VisibleBall[] balls;
        public Double tr, infosz;
        public Ball.Position Ori;
        public Size GroundSize;
        public Boolean Coor;

        Bitmap BallPreview;
        Graphics g;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (VisibleBall b in balls)
                listBox1.Items.Add(b);
            BallPreview = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = BallPreview;
            g = Graphics.FromImage(BallPreview);
            label1.Text = String.Format("Width = {0}\nHeight = {1}", GroundSize.Width, GroundSize.Height);
            checkBox1.Checked = Coor;
            textBox1.Text = Ori.x.ToString();
            textBox2.Text = Ori.y.ToString();
            textBox3.Text = infosz.ToString();
            textBox4.Text = tr.ToString();

        }

        private void PreviewRefresh(VisibleBall BallToDisplay)
        {
            g.Clear(Color.Transparent);
            g.FillEllipse(new SolidBrush(BallToDisplay.Col),
                (float)(pictureBox1.Width / 2 - BallToDisplay.R),
                (float)(pictureBox1.Height / 2 - BallToDisplay.R),
                (float)(2 * BallToDisplay.R), (float)(2 * BallToDisplay.R));
            pictureBox1.Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                BallSelected(true);
            else
                BallSelected(false);
        }

        private void BallSelected(Boolean IsSelected)
        {
            if (IsSelected)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                groupBox1.Enabled = true;
                AttributeReload(listBox1.SelectedIndex);
            }
            else
            {
                g.Clear(Color.Transparent);
                pictureBox1.Refresh();

                button3.Enabled = false;
                button4.Enabled = false;
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                groupBox1.Enabled = false;
            }
        }

        private void AttributeReload(int BallIndex)
        {
            PreviewRefresh(balls[BallIndex]);

            textBox5.Text = balls[BallIndex].Mass.ToString();
            textBox6.Text = balls[BallIndex].R.ToString();
            textBox7.Text = balls[BallIndex].Pos.x.ToString();
            textBox8.Text = balls[BallIndex].Pos.y.ToString();
            textBox9.Text = balls[BallIndex].GetVx().ToString();
            textBox10.Text = balls[BallIndex].GetVy().ToString();
        }

        private void ListUpdate(int BallIndex)
        {
            listBox1.Items.Clear();
            foreach (VisibleBall b in balls)
                listBox1.Items.Add(b);
            listBox1.SelectedIndex = BallIndex;
        }

        private void ArrayUpdate()
        {
            balls = new VisibleBall[listBox1.Items.Count];
            listBox1.Items.CopyTo(balls,0);

            if (balls.GetLength(0) == 0)
                button1.Enabled = false;
            else
                button1.Enabled = true;
        }

        #region GlobalSettings
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Coor = checkBox1.Checked;
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Ori.x = Convert.ToDouble(textBox1.Text);
            }
            catch
            {
                textBox1.Text = Ori.x.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Ori.y = Convert.ToDouble(textBox2.Text);
            }
            catch
            {
                textBox2.Text = Ori.y.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_LostFocus(object sender, EventArgs e)
        {
            try
            {
                infosz = Convert.ToDouble(textBox3.Text);
            }
            catch
            {
                textBox3.Text = infosz.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox4_LostFocus(object sender, EventArgs e)
        {
            try
            {
                tr = Convert.ToDouble(textBox4.Text);
            }
            catch
            {
                textBox4.Text = tr.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region BallAttributesSettings
        private void button6_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog
            {
                Color = balls[listBox1.SelectedIndex].Col
            };
            if (cd.ShowDialog(this) == DialogResult.OK)
            {
                balls[listBox1.SelectedIndex].Col = cd.Color;
                ListUpdate(listBox1.SelectedIndex);
                AttributeReload(listBox1.SelectedIndex);
            }
        }

        private void textBox5_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox5.Text);
                if (val > 0)
                {
                    Ball.Vector tmp = balls[listBox1.SelectedIndex].GetVelocity();
                    balls[listBox1.SelectedIndex].Mass = val;
                    balls[listBox1.SelectedIndex].SetMomentumByVelocity(tmp);
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox5.Text = balls[listBox1.SelectedIndex].Mass.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox6.Text);
                if (val > 0)
                {
                    balls[listBox1.SelectedIndex].R = val;
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox6.Text = balls[listBox1.SelectedIndex].R.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox7_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox7.Text);
                if (val > 0)
                {
                    balls[listBox1.SelectedIndex].Pos.x = val;
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox7.Text = balls[listBox1.SelectedIndex].Pos.x.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox8_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox8.Text);
                if (val > 0)
                {
                    balls[listBox1.SelectedIndex].Pos.y = val;
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox8.Text = balls[listBox1.SelectedIndex].Pos.y.ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox9_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox9.Text);
                if (val > 0)
                {
                    balls[listBox1.SelectedIndex].SetMomentumByVelocity(val, balls[listBox1.SelectedIndex].GetVy());
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox9.Text = balls[listBox1.SelectedIndex].GetVx().ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox10_LostFocus(object sender, EventArgs e)
        {
            try
            {
                Double val = Convert.ToDouble(textBox10.Text);
                if (val > 0)
                {
                    balls[listBox1.SelectedIndex].SetMomentumByVelocity(balls[listBox1.SelectedIndex].GetVx(), val);
                    ListUpdate(listBox1.SelectedIndex);
                }
            }
            catch
            {
                textBox10.Text = balls[listBox1.SelectedIndex].GetVy().ToString();
                MessageBox.Show("Invaild input number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ListManipulating
        private void button2_Click(object sender, EventArgs e)
        {
            int NewIndex = listBox1.Items.Add(new VisibleBall(1, 10, Color.Black, 0, 0, 0, 0));
            ArrayUpdate();
            listBox1.SelectedIndex = NewIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int NewIndex = listBox1.Items.Add(balls[listBox1.SelectedIndex]);
            ArrayUpdate();
            listBox1.SelectedIndex = NewIndex;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            ArrayUpdate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult Confirmation =
                MessageBox.Show("Do you really want to remove all the balls?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (Confirmation == DialogResult.Yes)
            {
                listBox1.Items.Clear();
                ArrayUpdate();
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            String OverlappedBalls = "";
            for (int i = 0; i < balls.GetUpperBound(0); i++)
                for (int j = i + 1; j <= balls.GetUpperBound(0); j++)
                    if (VisibleBall.ChkCollision(balls[i], balls[j]) < 0)
                        OverlappedBalls += String.Format("\nBall #{0} with Ball #{1}", i + 1, j + 1);

            if (OverlappedBalls != "")
            {
                DialogResult r = MessageBox.Show(this, 
                    String.Format("Detected overlapped balls:{0}\nProceed to simulation anyway?", OverlappedBalls),
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (r == DialogResult.No)
                    return;
            }

            Form1 mainform = (Form1)this.Owner;

            mainform.balls = this.balls;
            mainform.O = this.Ori;
            mainform.tr = this.tr;
            mainform.Coor = this.Coor;
            mainform.infosz = this.infosz;

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

    }
}
