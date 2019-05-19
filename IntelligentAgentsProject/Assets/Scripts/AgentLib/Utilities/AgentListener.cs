using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class AgentListener {

	private int port;
	private string ipAddress = "127.0.0.1";

	private TcpListener tcpListener;
	private Thread tcpListenerThread;

	Action<TcpClient, string> messageReceivedCallback;

	public AgentListener (int port) {
		this.port = port;
		tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread.IsBackground = true;
	}

	public void StartListening(Action<TcpClient, string> messageReceivedCallback) {
		this.messageReceivedCallback = messageReceivedCallback;
		tcpListenerThread.Start();
	}

	private void ListenForIncommingRequests() {
		try {
			tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
			tcpListener.Start();
			Byte[] buffer = new Byte[2048];
			while (true) {
				TcpClient connectedTcpClient;
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) {				
					using (NetworkStream stream = connectedTcpClient.GetStream()) {
						int length;					
						while ((length = stream.Read(buffer, 0, buffer.Length)) != 0) {
							var incommingData = new byte[length];
							Array.Copy(buffer, 0, incommingData, 0, length);				
							string clientMessage = Encoding.ASCII.GetString(incommingData);

							Thread processingThread = new Thread(() => messageReceivedCallback(connectedTcpClient, clientMessage));
							processingThread.IsBackground = true;
							processingThread.Start();
						}
					}
				}
				connectedTcpClient = null;
			}
		} catch (SocketException socketException) {
			Debug.Log("SocketException " + socketException.ToString());
		}
	}

	public void SendResponseToClient(TcpClient client, string message) {
		byte[] data = Encoding.ASCII.GetBytes(message);
		using (NetworkStream stream = client.GetStream()) {
			stream.Write(data, 0, data.Length);
		}
		client.Dispose();
	}
}
