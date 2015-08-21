using System;
using KociembaTwoPhase;

namespace Rub1k3ks
{
	public class Solver
	{
		public Solver (){
		}

		/* 
		 * This method just delegates the solving of the cube
		 * to Kociemba's algorithm.
		 */ 
		public static String Solve (Cube a){

			return Search.solution (a.ToString (), 20, true);

		}

		/* 
		 * This method scrambles a virtual cube, as such
		 * it cannot be used with a real cube.
		 */ 
		public static void Randomizer(Cube a, int count)
		{
			Random rand = new Random();
			int aux;
			for (int i = 0; i < count; i++) {
				aux = rand.Next () % 5;
				switch (aux) {
				case 0:
					a.TurnRowCW ();
					break;
				case 1:
					a.TurnRowCCW ();
					break;
				case 2:
					a.Roll ();
					break;
				case 3:
					a.TurnCubeCW ();
					break;
				case 4:
					a.TurnCubeCCW ();
					break;
				}
			}
			GoToInitial (a);
		}

		/*
		 * This method positions any specified face in the bottom
		 * of the cube (a face is specified by its central facelet).
		 */ 
		public static void GoToFace (string face, Cube a){

			if (a.face [5].square [1, 1].ToString() == face) {
				a.Roll ();
				a.Roll ();
			} else if (a.face [4].square [1, 1].ToString () == face) {
				a.Roll ();
			} else if (a.face [3].square [1, 1].ToString () == face) {
				a.TurnCubeCW ();
				a.Roll ();
			} else if (a.face [2].square [1, 1].ToString () == face) {
				a.TurnCube180 ();
				a.Roll ();
			} else if (a.face [1].square [1, 1].ToString () == face) {
				a.TurnCubeCCW ();
				a.Roll ();
			} 

		}

		/*
		 * Used only by the Randomizer function, to return the cube to the original position
		 * Not necessary if MoveCube is building the cube through color scan
		 */ 
		private static void GoToInitial (Cube a){
			GoToFace ("F", a);
			switch (a.face [1].square[1,1].ToString()) {
			case "L":
				a.TurnCubeCW ();
				break;
			case "R":
				a.TurnCubeCCW ();
				break;
			case "U":
				a.TurnCube180();
				break;
			}
		}

		/*
		 * This method translates the string retrieved from Kociemba's two-phase
		 * algorithm into movements of the virtual and real cube (if instantiated).
		 */ 
		public static void TranslateMove (String move, Cube a) {

			string aux;
			bool turn = false;

			for (int i = 0; i < move.Length; i++){
				aux = move.Substring (i, 1);
				switch (aux) {
				case "U":
				case "D":
				case "L":
				case "R":
				case "F":
				case "B":
					turn = true;
					GoToFace (aux, a);
					break;
				default:
					continue;
				}
				if (turn){
					turn = false;
					aux = move.Substring (i + 1, 1);
					switch (aux) {
					case "\'":
						a.TurnRowCCW ();
						break;
					case "2":
						a.TurnRow180 ();
						break;
					default:
						a.TurnRowCW ();
						break;
					}
				}
			}
			a.RestArm ();
		}
	}
}

