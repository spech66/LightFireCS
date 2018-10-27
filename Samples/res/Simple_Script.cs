using System;

public class Script
{
	public void Main()
	{
		Console.WriteLine("Hello Script!");
		Console.Write("Hi! What's your name? ");
		string name = Console.ReadLine();
		Console.WriteLine("Hey {0}! Isn't this cool?", name);
	}
}
