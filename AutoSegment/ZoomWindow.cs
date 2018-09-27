using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSegment
{
    public partial class ZoomWindow : Form
    {
        public ZoomWindow zoomwnd2;             // the other copy of zooming window

        public ZoomWindow()
        {
            InitializeComponent();

            zoomwnd2 = null;
        }

        private void ZoomWindow_SizeChanged(object sender, EventArgs e)
        {
            if ( (this.Width == zoomwnd2.Width) && (this.Height == zoomwnd2.Height) )
                return; // avoid endless cycle

            // make both zoom window's size synchronised
            zoomwnd2.Width = this.Width;
            zoomwnd2.Height = this.Height;

        }

        private void ZoomWindow_MouseLeave(object sender, EventArgs e)
        {
            this.Owner.Activate();
        }
    }
}
