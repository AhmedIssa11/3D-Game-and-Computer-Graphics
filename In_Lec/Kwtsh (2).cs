using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace In_Lec
{
	class Kwtsh : _3D_Model
	{
		public float Rad = 50/2;

		public void Design( Color c ) {
			float xx, yy, zz = 0;
			int i = 0;
			float inc = 20;
			int steps = ( int ) ( 360 / inc );
			for ( float th = 0; th < 360; th += inc ) {
				xx = ( float ) ( Math.Cos( th * Math.PI / 180 ) * Rad );
				yy = ( float ) ( Math.Sin( th * Math.PI / 180 ) * Rad );

				L_3D_Pts.Add( new _3D_Point( xx, yy, zz ) );

				if ( th > 0 ) {
					AddEdge( i, i - 1, c );
				}
				i++;
			}
			AddEdge( i - 1, 0, c );
		}
	}
}
