using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace In_Lec
{
    class SinWave : _3D_Model
    {
        
        public void Design()
        {
            float z = 0;
            int i = 0;
            for (float x = -450; x < 450; x+=10f)
            {
                float y = 200*(float)Math.Sin(x*Math.PI / 180);
                AddPoint(new _3D_Point(x, y, z));


                if (i > 0)
                {
                    AddEdge(i, i - 1, Color.Brown);
                }
                i++;
            }
        }
    }
}
