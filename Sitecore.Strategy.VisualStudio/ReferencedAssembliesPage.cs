using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;

namespace Sitecore.Strategy.VisualStudio
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    [Guid("D1E0F95C-EEFA-4015-8C23-B19BD4395382")]
    public class ReferencedAssembliesPage : DialogPage
    {
        public string ReferencedAssemblies { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                var control = new ReferencedAssembliesControl(this);
                control.Initialize();
                return control;
            }
        }
    }
}
