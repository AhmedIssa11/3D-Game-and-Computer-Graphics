using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace In_Lec
{
	class _3D_Model
	{
		public List<_3D_Point> L_3D_Pts = new List<_3D_Point>();
		public List<Edge> L_Edges = new List<Edge>();
		public Camera cam;

		public void AddPoint( _3D_Point pnn ) {
			L_3D_Pts.Add( pnn );
		}

		public void AddEdge( int i, int j, Color cl ) {
			Edge pnn = new Edge( i, j );
			pnn.cl = cl;
			L_Edges.Add( pnn );
		}

		public void RotX( float th ) {
			Transformation.RotatX( L_3D_Pts, th );
		}

		public void TransX( float tx ) {
			Transformation.TranslateX( L_3D_Pts, tx );
		}

		public void TransY( float ty ) {
			Transformation.TranslateY( L_3D_Pts, ty );
		}

		public void TransZ( float tz ) {
			Transformation.TranslateZ( L_3D_Pts, tz );
		}

		public void RotY( float th ) {
			Transformation.RotatY( L_3D_Pts, th );
		}

		public void RotZ( float th ) {
			Transformation.RotatZ( L_3D_Pts, th );
		}

		public void RotateAroundEdge( int iWhichEdge, float th ) {
			_3D_Point p1 = new _3D_Point( L_3D_Pts[L_Edges[iWhichEdge].i] );
			_3D_Point p2 = new _3D_Point( L_3D_Pts[L_Edges[iWhichEdge].j] );
			Transformation.RotateArbitrary( L_3D_Pts, p1, p2, th );
		}

		public void DrawYourSelf( Graphics g ) {
			Font FF = new Font( "System", 10 );
			for ( int k = 0; k < L_Edges.Count; k++ ) {
				int i = L_Edges[k].i;
				int j = L_Edges[k].j;

				_3D_Point pi = L_3D_Pts[i];
				_3D_Point pj = L_3D_Pts[j];

				PointF pi_2D = cam.TransformToOrigin_And_Rotate_And_Project( pi );
				PointF pj_2D = cam.TransformToOrigin_And_Rotate_And_Project( pj );

				Pen Pn = new Pen( L_Edges[k].cl, 2 );
				g.DrawLine( Pn, pi_2D.X, pi_2D.Y, pj_2D.X, pj_2D.Y );

				//if ( L_Edges[k].cl == Color.Yellow ) {
				//	pi = new _3D_Point( ( pi.X + pj.X ) / 2, ( pi.Y + pj.Y ) / 2, ( pi.Z + pj.Z ) / 2 );
				//	pi_2D = cam.TransformToOrigin_And_Rotate_And_Project( pi );
				//	g.DrawString( k.ToString(), FF, Brushes.White, pi_2D );
				//}
			}
		}

		public void DrawYourSelfGradient( Graphics g, _3D_Point gp1, PointF gp2 ) {
			Font FF = new Font( "System", 10 );
			for ( int k = 0; k < L_Edges.Count; k++ ) {
				int i = L_Edges[k].i;
				int j = L_Edges[k].j;

				_3D_Point pi = L_3D_Pts[i];
				_3D_Point pj = L_3D_Pts[j];

				PointF pi_2D = cam.TransformToOrigin_And_Rotate_And_Project( pi );
				PointF pj_2D = cam.TransformToOrigin_And_Rotate_And_Project( pj );

				_3D_Point pgi = gp1;
				PointF pgj = gp2;

				PointF pgi_2D = cam.TransformToOrigin_And_Rotate_And_Project( pgi );
				PointF pgj_2D = pgj;//cam.TransformToOrigin_And_Rotate_And_Project( pgj );

				g.FillRectangle( Brushes.White, pgi_2D.X, pgi_2D.Y, 5, 5 );
				g.FillRectangle( Brushes.White, pgj_2D.X, pgj_2D.Y, 5, 5 );

				using ( Brush aGradientBrush =
					new System.Drawing.Drawing2D.LinearGradientBrush
					( pgi_2D, pgj_2D, Color.Magenta, Color.FromArgb( 0, Color.Black ) ) ) {

					using ( Pen aGradientPen = new Pen( aGradientBrush, 2 ) ) {
						g.DrawLine( aGradientPen, pi_2D.X, pi_2D.Y, pj_2D.X, pj_2D.Y );
						aGradientBrush.Dispose();
						aGradientPen.Dispose();
					}
				}

			}
		}
	}
}
