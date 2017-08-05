using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle.Command
{
    //use for drive role action or ui action
    //independ what action source is
    class SosCmd
    {
        int cmd_id;
        byte[] data;
        int len;//data length
    }
}
