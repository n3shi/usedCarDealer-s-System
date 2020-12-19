using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace BD2_Komisy_samochodowe_DesktopApp
{
    public partial class FormAuthenticator : Form
    {
        string hostName;
        Form1 form1 = new Form1();
        public FormAuthenticator()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Text = "LogTest";
            labelLoginError.Hide();
            tbIpAddress.Text = "0.0.0.0";

            //hostName = Dns.GetHostName();
            //tbIpAddress.Text = Dns.GetHostByName(hostName).AddressList[0].ToString();
            
            

        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        //Form1 form = new Form1();
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
           form1.Enabled = true;
            try
            {
                if(textBoxLogin.Text == "" || textBoxPassword.Text == "" || tbIpAddress.Text == "")
                {
                    labelLoginError.Show();
                }
               else
                {

                form1.getId(textBoxLogin.Text, textBoxPassword.Text, tbIpAddress.Text);
                form1.StartConnection();
                form1.Show();
                this.Hide();
                form1.setLoggedUserInfo(textBoxLogin.Text);
                }
            }
            catch
            {
                labelLoginError.Show();
            }
        }

    }
}
