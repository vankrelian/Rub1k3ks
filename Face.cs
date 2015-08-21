using System;

namespace Rub1k3ks
{
	public class Face
	{
		public char[,] square;

		/*
		 * Constructor of a face of the cube, it receives any color
		 * and replicates throughout its facelets.
		 */ 
		public Face (char color)
		{
			square = new char[,] {{color,color,color},{color,color,color},{color,color,color}};
		}

		/*
		 * This method rotates any single face clockwise.
		 */ 
		public void TurnCWCore ()
		{
			char aux;
			aux = square[0,0];
			square[0,0] = square[2,0];
			square[2,0] = square[2,2];
			square[2,2] = square[0,2];
			square[0,2] = aux;
			aux = square[0,1];
			square[0,1] = square[1,0];
			square[1,0] = square[2,1];
			square[2,1] = square[1,2];
			square[1,2] = aux;
		}

		/*
		 * This method rotates any single face counterclockwise.
		 */ 
		public void TurnCCWCore ()
		{
			char aux;
			aux = square[0,0];
			square[0,0] = square[0,2];
			square[0,2] = square[2,2];
			square[2,2] = square[2,0];
			square[2,0] = aux;
			aux = square[0,1];
			square[0,1] = square[1,2];
			square[1,2] = square[2,1];
			square[2,1] = square[1,0];
			square[1,0] = aux;
		}

		/*
		 * This method positions any single face in a sheet.
		 * It is used in conjunction with Cube.GetCubeMap()
		 * to generate the string representing the cube map.
		 */ 
		public void PositionFace (char [,] sheet, int i, int j)
		{
			for (int ii = 0; ii < 3; ii ++)
				for (int jj = 0; jj < 6; jj += 2)
					sheet [i + ii, j + jj] = square [ii, jj/2];
		}
	}
}

