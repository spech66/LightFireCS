//-----------------------------------------------------------------------------
//  Result.cs
//  Copyright (C) 2005 by Sebastian Pech
//  This file is part of the "LightFire# Engine".
// 	For conditions of distribution and use, see copyright notice in Main.cs
//  - Result for error handling -
//-----------------------------------------------------------------------------

namespace LightFireCS
{
	public enum ResultCodes
	{
		OK = 0,
		ERROR
	}

	public class Result
	{
		private ResultCodes resultCode;
		
		public ResultCodes Code { get { return resultCode; } }
		
		public Result(ResultCodes code)
		{
			resultCode = code;
		}
	
		public static string GetMessage(ResultCodes code)
		{
			switch(code)
			{
				case ResultCodes.OK:
				{
					return "Everything went fine.";
				}
				case ResultCodes.ERROR:
				{
					return "There was an not specified error.";
				}
			}
			
			return "Unknown Result code!";
		}
	}
}