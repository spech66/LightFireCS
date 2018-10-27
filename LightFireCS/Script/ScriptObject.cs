//-----------------------------------------------------------------------------
//  ScriptObject.cs
//  Copyright (C) 2004-2009 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Compile and run Scripts -
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;

namespace LightFireCS.Script
{
	public class ScriptObject
	{
		private ArrayList references = new ArrayList();
		private Assembly compiledAssembly;
		
		public ArrayList errors = new ArrayList();
	
		/// <summary>Add reference to assembly.</summary>
		/// <param name="reference">Referenced assembly name</param>
		public void AddReference(string reference)
		{
			references.Add(reference);
		}
		
		/// <summary>Compile script from file.</summary>
		/// <param name="file">Script file</param>
		/// <returns>Error count in script</returns>
		public int CompileFromFile(string file)
		{
			errors.Clear();
		
			CSharpCodeProvider codeProvider = new CSharpCodeProvider();
			//ICodeCompiler compiler = codeProvider.CreateCompiler();
		
			CompilerParameters compParams = new CompilerParameters();
			compParams.CompilerOptions = "/target:library /optimize";
			compParams.GenerateExecutable = false;
			compParams.GenerateInMemory = true;
			compParams.IncludeDebugInformation = false;
			compParams.ReferencedAssemblies.Add("System.dll");
			foreach(string reference in references)
				compParams.ReferencedAssemblies.Add(reference);
				
			FileStream fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
			StreamReader fsReader = new StreamReader(fStream);
			string script = fsReader.ReadToEnd();
			fsReader.Close();
			fStream.Close();
			
			//CompilerResults results = compiler.CompileAssemblyFromSource(compParams, script);
			CompilerResults results = codeProvider.CompileAssemblyFromSource(compParams, script);

			if(results.Errors.Count > 0)
			{
				foreach(CompilerError error in results.Errors)
					errors.Add(error.ErrorText);
					
				return results.Errors.Count;
			}
			
			compiledAssembly = results.CompiledAssembly;
			
			return 0;
		}
		
		/// <summary>Execute function in assembly.</summary>
		/// <param name="classPath">Path to class including namespaces</param>
		/// <param name="function">Function to execute</param>
		/// <param name="args">Function arguments</param>
		/// <returns>Function return value</returns>
		public object ExecuteFunction(string classPath, string function, params object[] args)
		{
			Object obj = compiledAssembly.CreateInstance(classPath);
			Type type = obj.GetType();
			MethodInfo method = type.GetMethod(function);
			object retval = method.Invoke(obj, args);
			return retval;
		}
	}
}