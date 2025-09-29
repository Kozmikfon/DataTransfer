using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public partial class ImagePanel : Panel
    {
        public ImagePanel()
        {
            this.BackColor=Color.Red;
            
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            
        }

    }
}
