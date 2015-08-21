using System;
using System.Threading;
using MonoBrickFirmware.Movement;

namespace Rub1k3ks
{
	public class Cube
	{
		public Face[] face;
		Motor motorA;
		Motor motorB;
		Motor motorD;
		private bool _isArmRest;

		/* 
		 * Constructor for a virtual cube
		 * Solver.Randomizer() method works only with a virtual cube
		 */
		public Cube ()
		{
			this.face = new Face [] {
				new Face ((char)'F'),
				new Face ((char)'D'),
				new Face ((char)'L'),
				new Face ((char)'U'),
				new Face ((char)'R'),
				new Face ((char)'B')
			};
		}

		/* 
		 * Constructor for the real cube through Lego robot
		 */ 
		public Cube (Motor motorA, Motor motorB, Motor motorD)
		{
			this.motorA = motorA;
			this.motorB = motorB;
			this.motorD = motorD;
			this.motorA.ResetTacho();
			this.motorB.ResetTacho();
			this.motorD.ResetTacho();

			this._isArmRest = true;
			this.face = new Face [] {
				new Face ('O'),
				new Face ('B'),
				new Face ('W'),
				new Face ('G'),
				new Face ('Y'),
				new Face ('R')
			};
		}			

		/*
		 * This method turns the virtual cube and the real cube (if instantiated)
		 * 90 degrees clockwise.
		 */ 
		public void TurnCubeCW ()
		{
			// moves the virtual model of cube
			Face aux;
			this.face [0].TurnCWCore ();
			this.face [1].TurnCWCore ();
			this.face [2].TurnCWCore ();
			this.face [3].TurnCWCore ();
			this.face [4].TurnCWCore ();
			this.face [5].TurnCCWCore ();
			aux = this.face [1];
			this.face [1] = this.face [4];
			this.face [4] = this.face [3];
			this.face [3] = this.face [2];
			this.face [2] = aux;
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) {
				if (!this._isArmRest){
					MoveCube.Move (motorD, MoveCube.RestArm);
					this._isArmRest = true;
				}
				MoveCube.MoveRel (motorA, MoveCube.CW90);
			}
				
		}

		/*
		 * This method turns the virtual cube and the real cube (if instantiated)
		 * 90 degrees counterclockwise.
		 */ 
		public void TurnCubeCCW ()
		{
			// moves the virtual model of the cube
			Face aux;
			this.face [0].TurnCCWCore ();
			this.face [1].TurnCCWCore ();
			this.face [2].TurnCCWCore ();
			this.face [3].TurnCCWCore ();
			this.face [4].TurnCCWCore ();
			this.face [5].TurnCWCore ();
			aux = this.face [1];
			this.face [1] = this.face [2];
			this.face [2] = this.face [3];
			this.face [3] = this.face [4];
			this.face [4] = aux;
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) {
				if (!this._isArmRest){
					MoveCube.Move (motorD, MoveCube.RestArm);
					this._isArmRest = true;
				}
				MoveCube.MoveRel (motorA, MoveCube.CCW90);
			}
		}

		/*
		 * This method turns the bottom face of the virtual cube and the real cube 
		 * (if instantiated) 90 degrees clockwise.
		 */ 
		public void TurnRowCW ()
		{
			// moves the virtual model of the cube
			char aux;
			this.face [0].TurnCWCore ();
			aux = this.face [1].square [2, 2];
			this.face [1].square [2, 2] = this.face [4].square [0, 2];
			this.face [4].square [0, 2] = this.face [3].square [0, 0];
			this.face [3].square [0, 0] = this.face [2].square [2, 0];
			this.face [2].square [2, 0] = aux;
			aux = this.face [1].square [1, 2];
			this.face [1].square [1, 2] = this.face [4].square [0, 1];
			this.face [4].square [0, 1] = this.face [3].square [1, 0];
			this.face [3].square [1, 0] = this.face [2].square [2, 1];
			this.face [2].square [2, 1] = aux;
			aux = this.face [1].square [0, 2];
			this.face [1].square [0, 2] = this.face [4].square [0, 0];
			this.face [4].square [0, 0] = this.face [3].square [2, 0];
			this.face [3].square [2, 0] = this.face [2].square [2, 2];
			this.face [2].square [2, 2] = aux;
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) { 
				MoveCube.Move (motorD, MoveCube.GrabArm);
				MoveCube.MoveRel (motorA, MoveCube.CW90);
				this._isArmRest = false;
			}
		}

		/*
		 * This method turns the bottom face of the virtual cube and the real cube 
		 * (if instantiated) 90 degrees counterclockwise.
		 */ 
		public void TurnRowCCW ()
		{
			// moves the virtual model of the cube
			char aux;
			this.face [0].TurnCCWCore ();
			aux = this.face [1].square [2, 2];
			this.face [1].square [2, 2] = this.face [2].square [2, 0];
			this.face [2].square [2, 0] = this.face [3].square [0, 0];
			this.face [3].square [0, 0] = this.face [4].square [0, 2];
			this.face [4].square [0, 2] = aux;
			aux = this.face [1].square [1, 2];
			this.face [1].square [1, 2] = this.face [2].square [2, 1];
			this.face [2].square [2, 1] = this.face [3].square [1, 0];
			this.face [3].square [1, 0] = this.face [4].square [0, 1];
			this.face [4].square [0, 1] = aux;
			aux = this.face [1].square [0, 2];
			this.face [1].square [0, 2] = this.face [2].square [2, 2];
			this.face [2].square [2, 2] = this.face [3].square [2, 0];
			this.face [3].square [2, 0] = this.face [4].square [0, 0];
			this.face [4].square [0, 0] = aux;
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) { 
				MoveCube.Move (motorD, MoveCube.GrabArm);
				MoveCube.MoveRel (motorA, MoveCube.CCW90);
				this._isArmRest = false;
			}
		}

		/*
		 * This method turns the bottom face of the virtual cube and the real cube 
		 * (if instantiated) 180 degrees.
		 */ 
		public void TurnRow180 (){
			// moves the virtual model of the cube
			char aux;
			for (int i = 0; i < 2; i++) {
				this.face [0].TurnCWCore ();
				aux = this.face [1].square [2, 2];
				this.face [1].square [2, 2] = this.face [4].square [0, 2];
				this.face [4].square [0, 2] = this.face [3].square [0, 0];
				this.face [3].square [0, 0] = this.face [2].square [2, 0];
				this.face [2].square [2, 0] = aux;
				aux = this.face [1].square [1, 2];
				this.face [1].square [1, 2] = this.face [4].square [0, 1];
				this.face [4].square [0, 1] = this.face [3].square [1, 0];
				this.face [3].square [1, 0] = this.face [2].square [2, 1];
				this.face [2].square [2, 1] = aux;
				aux = this.face [1].square [0, 2];
				this.face [1].square [0, 2] = this.face [4].square [0, 0];
				this.face [4].square [0, 0] = this.face [3].square [2, 0];
				this.face [3].square [2, 0] = this.face [2].square [2, 2];
				this.face [2].square [2, 2] = aux;
			}
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) { 
				MoveCube.Move (motorD, MoveCube.GrabArm);
				MoveCube.MoveRel (motorA, MoveCube.CW180);
				this._isArmRest = false;
			}
		}

		/*
		 * This method turns the virtual cube and the real cube 
		 * (if instantiated) 180 degrees.
		 */ 
		public void TurnCube180 (){
			// moves the virtual model of the cube
			Face aux;
			for (int i = 0; i < 2; i++) {
				this.face [0].TurnCWCore ();
				this.face [1].TurnCWCore ();
				this.face [2].TurnCWCore ();
				this.face [3].TurnCWCore ();
				this.face [4].TurnCWCore ();
				this.face [5].TurnCCWCore ();
				aux = this.face [1];
				this.face [1] = this.face [4];
				this.face [4] = this.face [3];
				this.face [3] = this.face [2];
				this.face [2] = aux;
			}
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) {
				if (!this._isArmRest){
					MoveCube.Move (motorD, MoveCube.RestArm);
					this._isArmRest = true;
				}
				MoveCube.MoveRel (motorA, MoveCube.CW180);
			}
		}

		/*
		 * This method rolls the virtual and real cube (if instantiated).
		 */ 
		public void Roll()
		{
			// moves the virtual model of the cube
			Face aux;
			this.face [1].TurnCCWCore ();
			this.face [3].TurnCWCore ();
			aux = this.face [2];
			this.face [2] = this.face [0];
			this.face [0] = this.face [4];
			this.face [4] = this.face [5];
			this.face [5] = aux;
			// actually moves the physical cube
			if (motorA != null && motorB != null && motorD != null) { 
				MoveCube.Move (motorD, MoveCube.GrabArm);
				MoveCube.Move (motorD, MoveCube.PullArm);
				MoveCube.Move (motorD, MoveCube.GrabArm, 50);
				this._isArmRest = false;
			}
		}

		/*
		 * This method is used in conjunction with Face.PositionFace()
		 * to generate the cube map string.
		 */ 
		public string GetCubeMap ()
		{
			char[,] cubemap = new char[12, 17];

			for (int i = 0; i < 12; i++)
				for (int j = 0; j < 17; j++)
					cubemap[i,j] = (char)0x20;

			this.face [0].PositionFace (cubemap, 3, 6);
			this.face [1].PositionFace (cubemap, 3, 0);
			this.face [2].PositionFace (cubemap, 0, 6);
			this.face [3].PositionFace (cubemap, 3, 12);
			this.face [4].PositionFace (cubemap, 6, 6);
			this.face [5].PositionFace (cubemap, 9, 6);

			string cubemapstr = string.Empty;
			for (int i = 0; i < 12; i++) {
				for (int j = 0; j < 17; j++)
					cubemapstr += cubemap [i, j].ToString();
				cubemapstr += "\n";
			}

			return cubemapstr;
		}

		/*
		 * This method puts the cube arm in rest position.
		 */ 
		public void RestArm (){

			if (motorA != null && motorB != null && motorD != null) { 
				MoveCube.Move (motorD, MoveCube.RestArm);
				this._isArmRest = true;
			}
		}

		/*
		 * This method overrides the Cube.ToString() by returning the
		 * cube's facelet string in Kociemba's notation.
		 */
		public override string ToString ()
		{
			string cube = "123456789123456789123456789123456789123456789123456789";
			for (int k = 0; k < 6; k++) {
				for (int i = 0; i < 3; i++)
					for (int j = 0; j < 3; j++) {
						switch (k)
						{
						case 0:
							cube = cube.Substring (0, (2 - j) * 3 + i) + this.face [3].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 1);
							break;
						case 1:
							cube = cube.Substring (0, 9 + (2 - j) * 3 + i) + this.face [4].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 10);
							break;
						case 2:
							cube = cube.Substring (0, 18 + (2 - j) * 3 + i) + this.face [0].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 19);
							break;
						case 3:
							cube = cube.Substring (0, 27 + (2 - j) * 3 + i) + this.face [1].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 28);
							break;
						case 4:
							cube = cube.Substring (0, 36 + (2 - j) * 3 + i) + this.face [2].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 37);
							break;
						case 5:
							cube = cube.Substring (0, 45 + (2 - j) * 3 + i) + this.face [5].square [i, j].ToString () + cube.Substring ((2 - j) * 3 + i + 46);
							break;
						}
					}
			}
			return cube;
		}
	}
}

