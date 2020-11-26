using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerwerTCPAsynch
{
    public abstract class ProtokolKomunikacyjny
    {
        public ProtokolKomunikacyjny()
        {

        }
        public abstract string utworzOdpowiedz(String wiadomosc);

    }
}
