using System;
using System.Threading;
using MonoBrickFirmware.Movement;
using MonoBrickFirmware.Sensors;

namespace Rub1k3ks
{
	public static class MoveCube
	{

		public static readonly int RestArm = 0;
		public static readonly int GrabArm = 70;
		public static readonly int PullArm = 180;
		public static readonly int SensorRest = 0;
		public static readonly int SensorMid = 106; //107
		public static readonly int SensorSide = 87; //79
		//public static readonly int SensorCorner = 75; //68 not really necessary, faster without :)
		public static readonly int CW90 = 90;
		public static readonly int CW180 = 180;
		public static readonly int CCW90 = -90;
		public static readonly int CCW45 = -45;

		/*
		 * This method moves a motor in absolute positions.
		 * Although it can be used with any motor, only the
		 * sensor arm and the cube arm are in fact using it.
		 */ 
		public static void Move (Motor a, int abspos, sbyte speed = 30){

			int relpos;
			WaitHandle handle;

			relpos = a.GetTachoCount () - abspos;

			try {
				if (relpos < 0)
					handle = a.SpeedProfile (speed, 4, (uint)-relpos-8, 4, true);
				else 
					handle = a.SpeedProfile ((sbyte)-speed, 4, (uint) relpos-8, 4, true);
				handle.WaitOne ();
			}
			catch (WaitHandleCannotBeOpenedException e) {
				Console.WriteLine (e.ToString ());
			}
		}

		/*
		 * This method moves a motor relatively to its own position.
		 * Although it can be used with any motor, only the cube tray
		 * is in fact invoking this method to rotate in 45, 90 or 180 
		 * degrees.
		 */ 
		public static void MoveRel (Motor a, int relpos, sbyte speed = 90){

			WaitHandle handle;
			try {
				if (relpos < 0)
					handle = a.SpeedProfile (speed, 0, (uint)-relpos-40, 40, true);
				else 
					handle = a.SpeedProfile ((sbyte)-speed, 0, (uint) relpos-40, 40, true);
				handle.WaitOne ();
			}
			catch (WaitHandleCannotBeOpenedException e) {
				Console.WriteLine (e.ToString ());
			}
		}

		/*
		 * This method controls the movements of the robot to position
		 * each face of the cube to be scanned by the color sensor.
		 */ 
		public static char[] BuildCube (Cube cube, Motor motorA, Motor motorB, Motor motorD, EV3ColorSensor sensor){


			char[] faces = new char[6];
				

			// Position face in cube
			for (int j = 0; j < 6; j++) {
				
				switch (j) {
				case 0:
					BuildFace (cube.face[3], motorA, motorB, sensor);
					cube.face [3].TurnCWCore ();
					cube.face [3].TurnCWCore ();
					MoveCube.Move (motorD, MoveCube.GrabArm);
					MoveCube.Move (motorD, MoveCube.PullArm);
					MoveCube.Move (motorD, MoveCube.RestArm);
					break;
				case 1:
					BuildFace (cube.face[2], motorA, motorB, sensor);
					cube.face [2].TurnCWCore ();
					MoveCube.Move (motorD, MoveCube.GrabArm);
					MoveCube.Move (motorD, MoveCube.PullArm);
					MoveCube.Move (motorD, MoveCube.RestArm);
					break;
				case 2:
					BuildFace (cube.face[1], motorA, motorB, sensor);
					MoveCube.MoveRel (motorA, MoveCube.CCW90);
					MoveCube.Move (motorD, MoveCube.GrabArm);
					MoveCube.Move (motorD, MoveCube.PullArm);
					MoveCube.Move (motorD, MoveCube.RestArm);
					break;
				case 3:
					BuildFace (cube.face[0], motorA, motorB, sensor);
					cube.face [0].TurnCCWCore ();
					MoveCube.MoveRel (motorA, MoveCube.CW90);
					MoveCube.Move (motorD, MoveCube.GrabArm);
					MoveCube.Move (motorD, MoveCube.PullArm);
					MoveCube.Move (motorD, MoveCube.RestArm);
					break;
				case 4:
					BuildFace (cube.face [4], motorA, motorB, sensor);
					MoveCube.Move (motorD, MoveCube.GrabArm);
					MoveCube.Move (motorD, MoveCube.PullArm);
					MoveCube.Move (motorD, MoveCube.RestArm);
					break;
				case 5:
					BuildFace (cube.face [5], motorA, motorB, sensor);
					break;
				}
			}
			for (int i = 0; i < 6; i++)
				faces [i] = cube.face [i].square [1, 1];
			return faces;
		}

		/* 
		 * Converts facelets' colors into a metaformat used by Kociemba's algorithm.
		 * Kociemba's algorithm always uses an immutable metaformat. 
		 * This means that the front of the cube will always stay in
		 * in front, top on top, back in back, etc.
		 * By calling this method, whichever color of the central facelet that 
		 * happens to be on top will become the top in the metaformat, the color on
		 * on the frontal central facelet will become the new front, and so on. This saves 
		 * the trouble of repositioning the cube before solving it instead of when the colors 
		 * are fixed to each of the faces in the metaformat.
		 */
		public static void BuildCube2P(Cube a, char[] faces){

			for (int k = 0; k < 6; k++)
				for (int i = 0; i < 3; i++)
					for (int j = 0; j < 3; j++){
						if (faces [0] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'F';
						else if (faces [1] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'D';
						else if (faces [2] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'L';
						else if (faces [3] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'U';
						else if (faces [4] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'R';
						else if (faces [5] == a.face [k].square [i,j])
							a.face [k].square [i,j] = 'B';
					}

		}

		/* 
		 * Method to retrieve the colors of the facelets,
		 * you're welcome to play with it!
		 * Most likely you'll have to find your own values :)
		 */
		public static char getColorValue (RGBColor color){
			char colorvalue;
			//if (color.Red > (byte)60) {
			if (color.Red >= (byte)50) {
				//if (color.Blue >= (byte)90 || (color.Red >= (byte)160 &&  color.Blue <= (byte)20))
				if (color.Blue >= (byte)90 || (color.Red >= (byte)160))
					colorvalue = 'W'; // "white";
				else if (color.Green > (byte)70) {
					colorvalue = 'Y'; // "yellow";
				} else {
					//if ((float)color.Green/(float)color.Red >= .47 || color.Red >= (byte)75)
					if (color.Red > (byte)70 || (float)color.Green/(float)color.Red > .6 )
						colorvalue = 'O'; // "orange";
					else
						colorvalue = 'R'; // "red";
				}
			} else {
				if (color.Green >= (byte)240)
					colorvalue = 'Y'; // "yellow";
				else if (color.Blue > color.Green) {
					if (color.Green >= 30)
						colorvalue = 'B'; // "blue";
					else
						colorvalue = 'Y'; // "yellow";
				} else if (color.Green >= (byte)30) {
					//if ((float)color.Blue / (float)color.Green >= .34)
					if (color.Green >= (byte)65)
						colorvalue = 'G'; // "green";
					else if ((int)color.Green + (int)color.Blue + (int)color.Red <= (int)300)
						colorvalue = 'B'; // "blue";
					else
						colorvalue = 'W'; // "white";
				} else
					colorvalue = 'R'; // "red";
			}
			return colorvalue;
		}

		/* 
		 * This method controls the movements of the robot to scan the facelets
		 * of any single face of the cube.
		 */
		public static void BuildFace (Face face, Motor motorA, Motor motorB, EV3ColorSensor sensor){
		
			RGBColor[] colors = null;
			RGBColor color = null;
			Thread count = new Thread (() => colors = CountFacelets (motorA, sensor));

			MoveCube.Move (motorB, MoveCube.SensorMid, 15);
			color = sensor.ReadRGB ();
			MoveCube.Move (motorB, MoveCube.SensorSide, 15);

			count.Start ();
			while (!count.IsAlive);
			WaitHandle handle = motorA.SpeedProfile ((sbyte)100, 0, (uint) 360, 0, true);
			handle.WaitOne ();
			count.Join ();
			MoveCube.Move (motorB, MoveCube.SensorRest, 15);

			char colorvalue = ' ';
			for (int i = 0; i < 9; i++) {
				switch (i) {
				case 0:
					colorvalue = getColorValue (color);
					face.square[1,1] = colorvalue;
					break;
				case 1:
					color = colors[0];
					colorvalue = getColorValue (color);
					face.square[2,1] = colorvalue;
					break;
				case 3:
					color = colors[2];
					colorvalue = getColorValue (color);
					face.square[1,2] = colorvalue;
					break;
				case 5:
					color = colors[4];
					colorvalue = getColorValue (color);
					face.square[0,1] = colorvalue;
					break;
				case 7:
					color = colors[6];
					colorvalue = getColorValue (color);
					face.square[1,0] = colorvalue;
					break;
				case 2:
					color = colors[1];
					colorvalue = getColorValue (color);
					face.square[2,2] = colorvalue;
					break;
				case 4:
					color = colors[3];
					colorvalue = getColorValue (color);
					face.square[0,2] = colorvalue;
					break;
				case 6:
					color = colors[5];
					colorvalue = getColorValue (color);
					face.square[0,0] = colorvalue;
					break;
				case 8:
					color = colors[7];
					colorvalue = getColorValue (color);
					face.square[2,0] = colorvalue;
					break;
				}
				Console.WriteLine ("Line: {0} Color: {1} RGB code: {2}", i, colorvalue, color);
			}
		}

		public static RGBColor[] CountFacelets (Motor a, EV3ColorSensor sensor){
			RGBColor[] colors = new RGBColor[8];
			a.ResetTacho();
			for (int count = 0; count < 8;) 
				if (count == a.GetTachoCount () / 45 % 8) 
					colors [count++] = sensor.ReadRGB ();
			return colors;
		}
	}
}

