using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randoms
{
    public partial class Form2 : Form, ITrackable
    {
        public Form2()
        {
            InitializeComponent();
            TrackableController.Track(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.f3.Close();
        }
    }
}
