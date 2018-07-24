using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randoms
{
    public partial class Form1 : Form
    {
        public static Form f2;
        public static Form f3;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            f2 = new Form2();
            f3 = new Form3();
            f3.Show();
        }
    }
}
