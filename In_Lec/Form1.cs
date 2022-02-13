using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace In_Lec
{
	public partial class Form1 : Form
	{
		Bitmap off;
		_3D_Model mainCube = new _3D_Model();
		List<List<_3D_Model>> grid = new List<List<_3D_Model>>();
		List<List<Kwtsh>> circles = new List<List<Kwtsh>>();
		Camera cam = new Camera();
		Timer t = new Timer();
		Random rand = new Random();

		public Form1() {
			this.WindowState = FormWindowState.Maximized;
			this.Paint += new PaintEventHandler( Form1_Paint );
			this.Load += new EventHandler( Form1_Load );
			this.KeyDown += new KeyEventHandler( Form1_KeyDown );
			t.Start();
			t.Interval = 20;
			t.Tick += new EventHandler( t_Tick );
		}

		void Form1_Load( object sender, EventArgs e ) {
			off = new Bitmap( this.ClientSize.Width, this.ClientSize.Height );
			cam.ceneterX = this.ClientSize.Width / 2;
			cam.ceneterY = this.ClientSize.Height / 2;
			cam.BuildNewSystem();
			cam.cop.Z -= 200;
			RotateY( 30 );
			RotateX( 30 );
			cam.up = new _3D_Point( cam.cop.X, cam.cop.Y, cam.cop.Z + 100 );
			cam.BuildNewSystem();

			CreateCube( mainCube, 0, 0, 0 );
			mainCube.TransZ( -50 );

			for ( int i = 0; i < 17; i++ ) {
				grid.Add( new List<_3D_Model>() );
				circles.Add( new List<Kwtsh>() );
				for ( int j = 0; j < 7; j++ ) {
					int r = rand.Next( 0, 7 );

					_3D_Model p = new _3D_Model();
					CreatePlane( p, 0, 0, 0 );
					p.RotX( -90 );
					p.TransX( ( j - 3 ) * 100 );
					p.TransY( ( i - 2 ) * -100 );
					grid[i].Add( p );
					if ( i >= 4 && j == r ) {
						Kwtsh c = new Kwtsh();
						c.Design( Color.Red );
						c.cam = cam;
						c.TransX( ( j - 3 ) * 100 );
						c.TransY( ( i - 2 ) * -100 );
						c.AddPoint( CalcMidPoint( c.L_3D_Pts ) );
						circles[i].Add( c );
					}
				}
			}
		}

		int[] edgePat = { 2, 0, 4, 6 };
		int edgePatInd = 0;
		int rt = 0;
		int speed = 5;
		float prevY = 0;
		float prevZ = 0;
		bool gameOver = false;

		void t_Tick( object sender, EventArgs e ) {
			if ( !gameOver ) {
				if ( goRight == false && goLeft == false ) {
					var l = new List<_3D_Point>();
					l.Add( mainCube.L_3D_Pts[1] );
					l.Add( mainCube.L_3D_Pts[6] );
					_3D_Point p1 = CalcMidPoint( l );

					l = new List<_3D_Point>();
					l.Add( mainCube.L_3D_Pts[0] );
					l.Add( mainCube.L_3D_Pts[7] );
					_3D_Point p2 = CalcMidPoint( l );

					prevY = mainCube.L_3D_Pts[2].Y;
					prevZ = mainCube.L_3D_Pts[2].Z;

					Transformation.RotateArbitrary( mainCube.L_3D_Pts, p1, p2, speed );
					for ( int i = 0; i < grid.Count; i++ ) {
						for ( int j = 0; j < grid[i].Count; j++ ) {
							grid[i][j].TransY( mainCube.L_3D_Pts[2].Y - prevY );
							if ( grid[i][j].L_3D_Pts[0].Y > 400 ) {
								grid[i][j].TransY( -grid.Count * 100 );
							}
						}
					}
					for ( int i = 0; i < circles.Count; i++ ) {
						for ( int j = 0; j < circles[i].Count; j++ ) {
							circles[i][j].TransY( mainCube.L_3D_Pts[2].Y - prevY );
							if ( circles[i][j].L_3D_Pts[0].Y > 400 ) {
								circles[i][j].TransY( -circles.Count * 100 );
							}
						}
					}

					mainCube.TransZ( prevZ - mainCube.L_3D_Pts[2].Z );

					rt += speed;

					if ( rt >= 90 ) {
						Transformation.RotateArbitrary( mainCube.L_3D_Pts, p1, p2, -90 );
						mainCube.TransZ( -mainCube.L_3D_Pts[8].Z - 50 );
						mainCube.TransY( -mainCube.L_3D_Pts[8].Y - 0 );
						rt = 0;
						edgePatInd++;
						if ( startGoRight ) goRight = true;
						if ( startGoLeft ) goLeft = true;

						for ( int i = 0; i < circles.Count; i++ ) {
							for ( int j = 0; j < circles[i].Count; j++ ) {
								
								if ( mainCube.L_3D_Pts.Last().X <= circles[i][j].L_3D_Pts.Last().X + 20 &&
								mainCube.L_3D_Pts.Last().X >= circles[i][j].L_3D_Pts.Last().X - 20 ) {
									
									if ( mainCube.L_3D_Pts.Last().Y <= circles[i][j].L_3D_Pts.Last().Y + 20 &&
									mainCube.L_3D_Pts.Last().Y >= circles[i][j].L_3D_Pts.Last().Y - 20 ) {
										gameOver = true;
									}
								}
							}
						}

					}
					if ( edgePatInd >= edgePat.Length ) {
						edgePatInd = 0;
					}
				}

				if ( goRight ) {
					mainCube.RotateAroundEdge( 10, -speed );
					rt += speed;
					if ( rt >= 90 ) {
						rt = 0;

						var l = new List<_3D_Point>();
						l.Add( mainCube.L_3D_Pts[4] );
						l.Add( mainCube.L_3D_Pts[6] );
						_3D_Point p1 = CalcMidPoint( l );

						l = new List<_3D_Point>();
						l.Add( mainCube.L_3D_Pts[0] );
						l.Add( mainCube.L_3D_Pts[2] );
						_3D_Point p2 = CalcMidPoint( l );

						Transformation.RotateArbitrary( mainCube.L_3D_Pts, p1, p2, -90 );
						startGoRight = false;
						goRight = false;
					}
				}
				else if ( goLeft ) {
					mainCube.RotateAroundEdge( 11, speed );
					rt += speed;
					if ( rt >= 90 ) {
						rt = 0;
						var l = new List<_3D_Point>();
						l.Add( mainCube.L_3D_Pts[4] );
						l.Add( mainCube.L_3D_Pts[6] );
						_3D_Point p1 = CalcMidPoint( l );

						l = new List<_3D_Point>();
						l.Add( mainCube.L_3D_Pts[0] );
						l.Add( mainCube.L_3D_Pts[2] );
						_3D_Point p2 = CalcMidPoint( l );

						Transformation.RotateArbitrary( mainCube.L_3D_Pts, p1, p2, 90 );
						startGoLeft = false;
						goLeft = false;
					}
				}
			}
			else {
				mainCube.TransZ( 10 );
			}
			DrawDubble( this.CreateGraphics() );
		}

		bool startGoRight = false;
		bool goRight = false;
		bool startGoLeft = false;
		bool goLeft = false;

		void Form1_KeyDown( object sender, KeyEventArgs e ) {
			switch ( e.KeyCode ) {
				case Keys.Right:
					startGoRight = true;
					break;
				case Keys.Left:
					startGoLeft = true;
					break;
			}
			DrawDubble( this.CreateGraphics() );
		}

		_3D_Point CalcMidPoint( List<_3D_Point> l ) {
			float sumX, sumY, sumZ;
			sumX = sumY = sumZ = 0;
			for ( int i = 0; i < l.Count; i++ ) {
				sumX += l[i].X;
				sumY += l[i].Y;
				sumZ += l[i].Z;
			}

			return new _3D_Point( sumX / l.Count, sumY / l.Count, sumZ / l.Count );
		}

		void CreateCube( _3D_Model cb, float XS, float YS, float ZS ) {
			float[] vert =
							{
								-50  ,  -50   ,  -50,
								 50   , -50   ,  -50,
								 50   , -50   , 50,
								-50  ,  -50   , 50,

								-50  ,  50   ,  -50,
								 50   , 50   ,  -50,
								 50   , 50   , 50,
								-50  ,  50   , 50
							};

			_3D_Point pnn;
			int j = 0;
			for ( int i = 0; i < 8; i++ ) {
				pnn = new _3D_Point( vert[j] + XS, vert[j + 1] + YS, vert[j + 2] + ZS );
				j += 3;
				cb.AddPoint( pnn );
			}

			int[] Edges = {
							  0,1 ,
							  1,2 ,
							  2,3 ,
							  3,0 ,

							  4,5,
							  5,6,
							  6,7,
							  7,4,

							  0,4,
							  1,5,
							  2,6,
							  3,7

						  };
			j = 0;
			Color[] cl = { Color.Red, Color.HotPink, Color.Black, Color.Blue };
			for ( int i = 0; i < 12; i++ ) {
				cb.AddEdge( Edges[j], Edges[j + 1], cl[1] );

				j += 2;
			}
			cb.cam = cam;
			cb.AddPoint( CalcMidPoint( cb.L_3D_Pts ) );
		}

		void CreatePlane( _3D_Model pln, float XS, float YS, float ZS ) {
			float[] vert =
							{
								-50  ,  0  , -50,
								50   , 0   , -50,
								50   , 0   , 50,
								-50  ,  0  , 50
							};


			_3D_Point pnn;
			int j = 0;
			for ( int i = 0; i < 4; i++ ) {
				pnn = new _3D_Point( vert[j] + XS, vert[j + 1] + YS, vert[j + 2] + ZS );
				j += 3;
				pln.AddPoint( pnn );
			}


			int[] Edges = {
							  0,1 ,
							  1,2 ,
							  2,3 ,
							  3,0
						  };
			j = 0;
			Color[] cl = { Color.Red, Color.Green, Color.Black, Color.Blue };
			for ( int i = 0; i < 4; i++ ) {
				pln.AddEdge( Edges[j], Edges[j + 1], Color.DeepSkyBlue );

				j += 2;
			}
			pln.cam = cam;
		}

		void RotateY( float th2 ) {
			float th = ( float ) ( th2 * Math.PI / 180 );
			float x_ = ( float ) ( cam.cop.Z * Math.Sin( th ) + cam.cop.X * Math.Cos( th ) );
			float y_ = cam.cop.Y;
			float z_ = ( float ) ( cam.cop.Z * Math.Cos( th ) - cam.cop.X * Math.Sin( th ) );

			cam.cop.X = x_;
			cam.cop.Z = z_;
			cam.BuildNewSystem();
		}

		void RotateX( float th2 ) {
			float th = ( float ) ( th2 * Math.PI / 180 );
			float x_ = cam.cop.X;
			float y_ = ( float ) ( cam.cop.Y * Math.Cos( th ) - cam.cop.Z * Math.Sin( th ) );
			float z_ = ( float ) ( cam.cop.Y * Math.Sin( th ) + cam.cop.Z * Math.Cos( th ) );

			cam.cop.Y = y_;
			cam.cop.Z = z_;
			cam.BuildNewSystem();
		}

		void Form1_Paint( object sender, PaintEventArgs e ) {
			DrawDubble( e.Graphics );
		}

		void DrawScene( Graphics g ) {
			g.Clear( Color.Black );
			for ( int i = 0; i < grid.Count; i++ ) {
				for ( int j = 0; j < grid[i].Count; j++ ) {
					grid[i][j].DrawYourSelf( g );

				}
			}
			for ( int i = 0; i < circles.Count; i++ ) {
				for ( int j = 0; j < circles[i].Count; j++ ) {
					circles[i][j].DrawYourSelf( g );
				}
			}
			mainCube.DrawYourSelf( g );
		}

		void DrawDubble( Graphics g ) {
			Graphics g2 = Graphics.FromImage( off );
			DrawScene( g2 );
			g.DrawImage( off, 0, 0 );
		}
	}
}
