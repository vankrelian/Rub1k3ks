using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace Rub1k3ks
{
	public static class Communication
	{
		// Specifies the file path containing the Cube.ToString().
		public static string CubePath
		{
			get
			{
				return "./cube.txt";
			}
		}

		// Specifies the file path of the solution found by Kociemba's algorithm.
		public static string SolutionPath
		{
			get
			{
				return "./solution.txt";
			}
		}

		/*
		 * Just make sure to clean up old entries.
		 */ 
		public static void ClearCube(string filename){

			if (File.Exists (filename))
				File.Delete (filename);
		
		}

		/*
		 * Writes the Cube.ToString() into cube.txt.
		 */ 
		public static void SaveCube (string filename, string cube){

			using (Stream stream = File.Open(filename, FileMode.Create))
			{
				stream.Write(Encoding.ASCII.GetBytes(cube),0,cube.Length);
			}

		}

		/*
		 * Reads the solution string of the cube from solution.txt.
		 */ 
		public static string ReceiveSolution(){
			while (!File.Exists (SolutionPath));
			using (Stream stream = File.Open(SolutionPath, FileMode.Open))
			{
				byte[] cube = new byte[(int)stream.Length];
				stream.Read(cube,0,(int)stream.Length);
				return Encoding.ASCII.GetString(cube);
			}
		}
	}
}

