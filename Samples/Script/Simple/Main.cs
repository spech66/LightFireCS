using System;
using LightFireCS.Script;

public class Simple
{
	public static void Main(string[] args)
	{
		string exClass = "Script";
		string exFunc = "Main";
	
		Console.WriteLine("LightFire# Simple Script Execute");
		Console.WriteLine();
			
		if(args.Length < 1)
		{
			Console.WriteLine("Using:");
			Console.WriteLine("    Simple.exe SCRIPT [CLASS [FUNCTION]]");
			Console.WriteLine();
			Console.WriteLine("    SCRIPT		C# Script File");
			Console.WriteLine("    CLASS		Execute a function in CLASS. Default: {0}", exClass);
			Console.WriteLine("    FUNCTION		Execute FUNCTION in CLASS. Default: {0}", exFunc);
			return;
		}
		
		if(args.Length >= 2)
			exClass = args[1];

		if(args.Length >= 3)
			exClass = args[2];
		
		ScriptObject sobject = new ScriptObject();
		
		Console.WriteLine("Compiling: {0}", args[0]);
		sobject.CompileFromFile(args[0]);
		Console.WriteLine("Errors: {0}", sobject.errors.Count);
		if(sobject.errors.Count != 0)
		{
			foreach(string s in sobject.errors)
				Console.WriteLine(s);
			return;
		}
		
		Console.WriteLine("Execute '{0}' in class '{1}'", exFunc, exClass);
		sobject.ExecuteFunction(exClass, exFunc);
	}
}