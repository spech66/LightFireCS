using System;

public class ChessServer
{
	static void Main()
	{
		Server server = new Server(13876, 64);
		while(true)
			server.Process();
	}
}
