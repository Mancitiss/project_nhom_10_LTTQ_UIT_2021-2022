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
        public delegate void ChangeIncognitoModeDelegate(bool value);
        public ChangeIncognitoModeDelegate changeIncognitoMode;


        public FormSettings()
        {
            InitializeComponent();
            //this.circlePictureBox1.Image = AFriendClient.user.avatar;
            /*
            if (AFriendClient.user.avatar != null)
            {
                this.circlePictureBox1.Image = AFriendClient.user.avatar; // can fix dong nay
            }*/
            //labelUsername.Location = new Point((this.Width - labelUsername.Width) / 2 - 5, labelUsername.Top);
            //customButtonUsername.Location = new Point(labelUsername.Left - 20, customButtonUsername.Top);
            labelWarning.Text = "";
            changeSettingsWarning = new ChangeSettingsWarning(ChangeLabel);
            changeIncognitoMode = new ChangeIncognitoModeDelegate(ChangeIncognitoMode);
        }

        public void ChangeLabel(string text, Color color)
        {
            labelWarning.Text = text;
            labelWarning.ForeColor = color;
            //labelWarning.Location = new Point((this.Width - labelWarning.Width) / 2 - 5, labelWarning.Top);
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AFriendClient.img_string))
            {
                circlePictureBox1.Image = StringToImage(AFriendClient.img_string);
            }
            this.labelUsername.Text = AFriendClient.user.name;
            ChangeIncognitoMode(AFriendClient.user.priv);
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
        private void buttonSaveUsername_Click_1(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            if (String.IsNullOrEmpty(customTextBoxUsername.Texts.Trim()))
                //MessageBox.Show("Please enter new username!", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter new name!", Color.FromArgb(213, 54, 41));
            else
            {
                if (!checkBytesN())
                {
                    ChangeLabel("Username has over the limit of characters", Color.FromArgb(213, 54, 41));
                }
                else
                {
                    AFriendClient.stream.Write(Encoding.Unicode.GetBytes("1012" + AFriendClient.data_with_byte(customTextBoxUsername.Texts.Trim())));
                    AFriendClient.temp_name = customTextBoxUsername.Texts.Trim();
                    panelUsername.Hide();
                    customTextBoxUsername.Texts = "";
                    //this.Close();
                }
            }
        }
       
        private void customButtonPassword_Click_1(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            panelUsername.Hide();
            panelPassword.Show();
        }
       
        private void buttonSavePassword_Click_1(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            if (string.IsNullOrEmpty(textBoxCurrentPassword.Texts) || string.IsNullOrEmpty(textBoxConfirmPassword.Texts) || string.IsNullOrEmpty(textBoxNewPassword.Texts))
                //MessageBox.Show("Please enter your password!", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter your password!", Color.FromArgb(213, 54, 41));
            else
            {
                if (!checkBytesP())
                {
                    ChangeLabel("Password has over the limit of characters", Color.FromArgb(213, 54, 41));
                }
                else
                {
                    if (!textBoxConfirmPassword.Texts.Equals(textBoxNewPassword.Texts))
                    {
                        ChangeLabel("Those passwords doesn't match", Color.FromArgb(213, 54, 41));
                    }
                    else
                    {
                        if (textBoxConfirmPassword.Texts.Equals(textBoxNewPassword.Texts))
                        {
                            AFriendClient.stream.Write(Encoding.Unicode.GetBytes("4269" + AFriendClient.data_with_byte(textBoxCurrentPassword.Texts) + AFriendClient.data_with_byte(textBoxConfirmPassword.Texts)));
                        }
                        panelPassword.Hide();
                        textBoxNewPassword.Texts = "";
                        textBoxCurrentPassword.Texts = "";
                        textBoxConfirmPassword.Texts = "";
                    }
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
        static public class TopMostMessageBox
        {
            static public DialogResult Show(string message)
            {
                return Show(message, string.Empty, MessageBoxButtons.OK);
            }

            static public DialogResult Show(string message, string title)
            {
                return Show(message, title, MessageBoxButtons.OK);
            }

            static public DialogResult Show(string message, string title,
                MessageBoxButtons buttons)
            {
                // Create a host form that is a TopMost window which will be the 
                // parent of the MessageBox.
                Form topmostForm = new Form();
                // We do not want anyone to see this window so position it off the 
                // visible screen and make it as small as possible
                topmostForm.Size = new System.Drawing.Size(1, 1);
                topmostForm.StartPosition = FormStartPosition.Manual;
                System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
                topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10,
                    rect.Right + 10);
                topmostForm.Show();
                // Make this form the active form and make it TopMost
                topmostForm.Focus();
                topmostForm.BringToFront();
                topmostForm.TopMost = true;
                // Finally show the MessageBox with the form just created as its owner
                DialogResult result = MessageBox.Show(topmostForm, message);
                topmostForm.Dispose(); // clean it up all the way

                return result;
            }
        }


        private void customButtonAvatar_Click_1(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            Thread thread = new Thread((ThreadStart)(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Images|*.pjp;*.jpg;*.pjpeg;*.jpeg;*.jfif;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string imageAsString = ImageToString(ofd.FileName);
                    int length = imageAsString.Length;
                    if (length < 2800000)
                    {
                        AFriendClient.stream.Write(AFriendClient.Combine(Encoding.Unicode.GetBytes("0601"), Encoding.ASCII.GetBytes(AFriendClient.data_with_ASCII_byte(imageAsString.Trim()))));
                        AFriendClient.img_string = imageAsString.Trim();
                        circlePictureBox1.Image = StringToImage(AFriendClient.img_string.ToString());
                    }
                    else
                    {
                        //ChangeLabel("Please choose an image that is less than 2Mb", Color.Red);
                        //this.Invoke(this.changeSettingsWarning, new object[] { "Please choose an image that is less than 2Mb", Color.Red });
                        TopMostMessageBox.Show("Please choose an image that is less than 2Mb"); 
                    }
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
        private void customButtonExit_Click_1(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            this.Close();
        }
        
        private void textBoxCurrentPassword_Enter(object sender, EventArgs e)
        {
            labelWarning.Text = "";
        }
        private bool checkBytesP()
        {
            if (Encoding.Unicode.GetByteCount(textBoxNewPassword.Texts) < 128)
                return true;
            return false;
        }

        private bool checkBytesN()
        {
            if (Encoding.Unicode.GetByteCount(customTextBoxUsername.Texts) < 64)
                return true;
            return false;
        }

        public void ChangeIncognitoMode(bool value)
        {
            toggleButton1.Checked = value;
        }

        private void toggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Code to change private mode
            if (toggleButton1.Checked)
            {
                AFriendClient.stream.Write(Encoding.Unicode.GetBytes("1508"));
            } else
            {
                AFriendClient.stream.Write(Encoding.Unicode.GetBytes("0508"));
            }
        }
    }
}
