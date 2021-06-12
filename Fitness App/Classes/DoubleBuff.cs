using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_App.Classes
{
    class DoubleBuff
    {
        //allow a screen to user double buff (reduce flickering on load)
        //don't allow this process when RDP'd as this will affect preformance heavily
        public bool doubleBuffer()
        {
            bool buffer = false;

            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                buffer = false;
            }

            return buffer;
        }
    }
}
