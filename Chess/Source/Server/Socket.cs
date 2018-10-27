using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TcpSocket
{
	public Socket tcpSock;
	byte[] data = new byte[1024];

	public int Receive(ref string msg)
	{
		int avail = tcpSock.Available;
		int index = 0;
		if(avail > data.Length - index)
			avail = data.Length - index;

		avail = tcpSock.Receive(data, index, avail, SocketFlags.Partial);
		msg = Encoding.ASCII.GetString(data, index, avail);
		index += avail;
		return msg.Length;
	}

	public int Send(string msg)
	{
		return tcpSock.Send(Encoding.ASCII.GetBytes(msg + "\r\n"));
	}
}

public class Server
{
	private ArrayList clients = new ArrayList();
	private Socket srvSock;
	
	public Server(int port, int maxClients)
	{
		IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
		IPEndPoint ipEndPoint = new IPEndPoint(ipHostEntry.AddressList[0], port);
		srvSock = new Socket(ipEndPoint.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);
				
		srvSock.Blocking = false;
		srvSock.Bind(ipEndPoint);
		srvSock.Listen(16);
				
		Console.WriteLine("{0}: listening to port {1}", Dns.GetHostName(), ipEndPoint.Port);
        
		clients.Capacity = maxClients;
	}
	
	public void Process()
	{
		if(srvSock.Poll(0, SelectMode.SelectRead))
		{
			int i = clients.Add(new TcpSocket());
			((TcpSocket)clients[i]).tcpSock = srvSock.Accept();
			((TcpSocket)clients[i]).Send("Welcome.");
			Console.WriteLine("Client {0} connected.", i);
		}

		string rln = null;
		for (int i = 0; i < clients.Count; i++)
		{
			if (((TcpSocket)clients[i]).tcpSock.Poll(0, SelectMode.SelectRead))
			{
				if (((TcpSocket)clients[i]).Receive(ref rln) > 0)
				{
					((TcpSocket)clients[i]).Send(rln);
					Console.Write("{0}: {1}", i, rln);
				}
				else
				{
					((TcpSocket)clients[i]).tcpSock.Shutdown(SocketShutdown.Both);
					((TcpSocket)clients[i]).tcpSock.Close();
					clients.RemoveAt(i);
					Console.WriteLine("Client {0} disconnected.", i);
				}
			}
		}		
		
	}
}