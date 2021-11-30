using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jil;

namespace A_Friend
{
    public partial class FormSettings : Form
    {
        public delegate void ChangeSettingsWarning(string text, Color color);
        public ChangeSettingsWarning changeSettingsWarning;

  
        public FormSettings()
        {
            InitializeComponent();
            this.circlePictureBox1.Image = AFriendClient.user.avatar;
            if (AFriendClient.user.avatar != null)
            {
                this.circlePictureBox1.Image = AFriendClient.user.avatar; // can fix dong nay
            }
            //labelUsername.Location = new Point((this.Width - labelUsername.Width) / 2 - 5, labelUsername.Top);
            //customButtonUsername.Location = new Point(labelUsername.Left - 20, customButtonUsername.Top);
            labelWarning.Text = "";
            changeSettingsWarning = new ChangeSettingsWarning(ChangeLabel);
        }

        public void ChangeLabel(string text, Color color)
        {
            labelWarning.Text = text;
            labelWarning.ForeColor = color;
            //labelWarning.Location = new Point((this.Width - labelWarning.Width) / 2 - 5, labelWarning.Top);
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            this.labelUsername.Text = AFriendClient.user.name;
            panelPassword.Hide();
            panelUsername.Hide();
            this.ControlBox = false;
            this.Text = " ";
        }

        private void customButtonUsername_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            panelPassword.Hide();
            panelUsername.Show();
        }

        private void buttonSaveUsername_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            if (String.IsNullOrEmpty(customTextBoxUsername.Texts.Trim()))
                //MessageBox.Show("Please enter new username!", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter new name!", Color.FromArgb(213, 54, 41));
            else
            {
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("1012" + AFriendClient.data_with_byte(customTextBoxUsername.Texts.Trim())));
                AFriendClient.temp_name = customTextBoxUsername.Texts.Trim();
                panelUsername.Hide();
                customTextBoxUsername.Texts = "";
                //this.Close();
            }
        }

        private void customButtonPassword_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            panelUsername.Hide();
            panelPassword.Show();
        }

        private void buttonSavePassword_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            if (string.IsNullOrEmpty(textBoxCurrentPassword.Texts) || string.IsNullOrEmpty(textBoxConfirmPassword.Texts) || string.IsNullOrEmpty(textBoxNewPassword.Texts))
                //MessageBox.Show("Please enter your password!", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter your password!", Color.FromArgb(213, 54, 41));
            else
            {
                if (!checkBytes())
                {
                    ChangeLabel("Username or Password has over the limit of characters", Color.FromArgb(213, 54, 41));
                }
                else
                {
                    if (textBoxConfirmPassword.Texts.Equals(textBoxNewPassword.Texts))
                    {
                        AFriendClient.client.Send(Encoding.Unicode.GetBytes("4269" + AFriendClient.data_with_byte(textBoxCurrentPassword.Texts) + AFriendClient.data_with_byte(textBoxConfirmPassword.Texts)));
                    }
                    panelPassword.Hide();
                    textBoxNewPassword.Texts = "";
                    textBoxCurrentPassword.Texts = "";
                    textBoxConfirmPassword.Texts = "";
                }
            }
        }
        public string ImageToString(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            Image im = Image.FromFile(path);
            MemoryStream ms = new MemoryStream();
            im.Save(ms, im.RawFormat);
            byte[] array = ms.ToArray();
            return Convert.ToBase64String(array);
        }
        public Image StringToImage(string imageString)
        {

            if (imageString == null)
                throw new ArgumentNullException("imageString");
            byte[] array = Convert.FromBase64String(imageString);
            Image image = Image.FromStream(new MemoryStream(array));
            return image;
        }
        private void customButtonAvatar_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            Thread thread = new Thread((ThreadStart)(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Images|*.pjp;*.jpg;*.pjpeg;*.jpeg;*.jfif;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    circlePictureBox1.Crop(Image.FromFile(ofd.FileName));
                    string imageAsString = ImageToString(ofd.FileName);
                    AFriendClient.client.Send(AFriendClient.Combine(Encoding.Unicode.GetBytes("0601"), Encoding.ASCII.GetBytes(AFriendClient.data_with_ASCII_byte(imageAsString.Trim()))));
                    AFriendClient.temp_image = imageAsString.Trim();
                    /*
                    Console.WriteLine(imageAsString);
                    Console.WriteLine(Encoding.ASCII.GetByteCount(imageAsString));
                    */
                }
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void customButtonExit_Click(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            this.Close();
        }

        private void textBoxCurrentPassword_Enter(object sender, EventArgs e)
        {
            labelWarning.Text = "";
        }

        private bool checkBytes()
        {
            if (Encoding.Unicode.GetByteCount(textBoxNewPassword.Texts) < 128)
                return true;
            return false;
        }
    }
}
