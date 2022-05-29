using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    public enum DoroonCAMEnum
    {
        SET_CAM_CLOSE = 0,
        SET_CAM_OPEN = 1
    }

    public enum DoroonFilghtControlEnum
    {
        SET_ENG_SHUTDOWN = 0,
        SET_ENG_START = 1,
        SET_TAKEOFF = 2,
        SET_ALLTAKEOFF = 3
    }
}
