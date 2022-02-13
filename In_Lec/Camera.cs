using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace In_Lec
{
    class Camera
    {
        public _3D_Point cop;
        public _3D_Point lookAt;
        public _3D_Point up;
        public float focal = 500;


        public _3D_Point basisa, lookDir, basisc;



        public int ceneterX, ceneterY;
        public int cxScreen, cyScreen;

        public Camera()
        {
            cop = new _3D_Point(0, 0, -350);        // new Point3D(0, -50, 0);
            lookAt = new _3D_Point(0, 0, 0);       //new Point3D(0, 50, 0);
            up = new _3D_Point(0, 1, 0);
        }

        public void BuildNewSystem()
        {
            lookDir = new _3D_Point(0, 0, 0);
            basisa = new _3D_Point(0, 0, 0);
            basisc = new _3D_Point(0, 0, 0);

            lookDir.X = lookAt.X - cop.X;
            lookDir.Y = lookAt.Y - cop.Y;
            lookDir.Z = lookAt.Z - cop.Z;
            Matrix.Normalise(lookDir);

            basisa = Matrix.CrossProduct(up, lookDir);
            Matrix.Normalise(basisa);

            basisc = Matrix.CrossProduct(lookDir, basisa);
            Matrix.Normalise(basisc);
        }

        public void TransformToOrigin_And_Rotate(_3D_Point a, _3D_Point e)
        {
            _3D_Point w = new _3D_Point(a.X, a.Y, a.Z);
            w.X -= cop.X;
            w.Y -= cop.Y;
            w.Z -= cop.Z;

            e.X = w.X * basisa.X + w.Y * basisa.Y + w.Z * basisa.Z;
            e.Y = w.X * basisc.X + w.Y * basisc.Y + w.Z * basisc.Z;
            e.Z = w.X * lookDir.X + w.Y * lookDir.Y + w.Z * lookDir.Z;            
        }

        public PointF TransformToOrigin_And_Rotate_And_Project(_3D_Point w1)
        {
            _3D_Point  e1 = new _3D_Point(0, 0, 0);            
            TransformToOrigin_And_Rotate(w1, e1);


            _3D_Point n1 = new _3D_Point(0, 0, 0);
            n1.X = focal * e1.X / e1.Z;
            n1.Y = focal * e1.Y / e1.Z;
            n1.Z = focal;

            

            // view mapping
            n1.X = (int)(ceneterX +  n1.X);
            n1.Y = (int)(ceneterY +  n1.Y);

            //n1.X = (int)(ceneterX + cxScreen * n1.X);
            //n1.Y = (int)(ceneterY + cyScreen * n1.Y);

            //n1.X = (int)(ceneterX + cxScreen * n1.X / 2);
            //n1.Y = (int)(ceneterY - cyScreen * n1.Y / 2);

            return new PointF(n1.X, n1.Y);
        }
    }
}
