using System;
using System.Threading;
using MonoBrickFirmware;
using MonoBrickFirmware.Movement;
using MonoBrickFirmware.Sensors;

namespace Rub1k3ks
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			var sensor = new EV3ColorSensor(SensorPort.In1);
			sensor.Mode = ColorMode.RGB;

			Motor motorA = new Motor(MotorPort.OutA);
			Motor motorB = new Motor(MotorPort.OutB);
			Motor motorD = new Motor(MotorPort.OutD);

			string solution;

			Cube cube = new Cube (motorA, motorB, motorD); // instantiate a real cube

			// Cube cube = new Cube (); // virtual cube used for tests
			// Solver.Randomizer (cube, 5000); // scramble the virtual cube

			DateTime starttime = DateTime.Now;
			TimeSpan elapsedtime = new TimeSpan ();

			MoveCube.BuildCube2P(cube, MoveCube.BuildCube (cube, motorA, motorB, motorD, sensor));

			elapsedtime = DateTime.Now - starttime;

			Console.WriteLine (cube.GetCubeMap());
			Console.WriteLine (cube.ToString ());
			Console.WriteLine ("Elapsed time scanning cube: {0}", elapsedtime);


			starttime = DateTime.Now;

			// uncomment line below if you want to use the internal algorithm to solve the cube.
			// It's the easiest way, but also the slowest.
			// solution = Solver.Solve (cube);

			// comment the following block if you intend to use the internal algorithm to solve
			// the cube. By default the main program tries to read the solution from a solution
			// file. Just run the Kociemba algorithm on a pc and use scp, putty or any other ssh
			// tool to send the file to the intelligent Lego brick.
			Communication.ClearCube (Communication.CubePath);
			Communication.ClearCube (Communication.SolutionPath);
			Communication.SaveCube (Communication.CubePath, cube.ToString ());
			solution = Communication.ReceiveSolution ();

			elapsedtime = DateTime.Now - starttime;

			Console.WriteLine ("Solution: {0}", solution);
			Console.WriteLine ("Elapsed time receiving solution: {0}", elapsedtime);

			starttime = DateTime.Now;

			Solver.TranslateMove (solution, cube);
			Console.WriteLine (cube.GetCubeMap ());

			elapsedtime = DateTime.Now - starttime;
			Console.WriteLine ("Time elapsed moving cube: {0}", elapsedtime);

			Communication.ClearCube (Communication.CubePath);
			Communication.ClearCube (Communication.SolutionPath);


		}
	}
}