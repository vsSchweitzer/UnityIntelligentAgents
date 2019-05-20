using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using System;

public class AgentListener : MonoBehaviour {

	#region Singleton
	private static AgentListener instance;

	private void SingletonCheck() {
		if (instance != null && instance != this) {
			Destroy(this);
			Debug.LogError("There were multiple AgentListeners in the scene.");
		} else {
			instance = this;
		}
	}

	public static AgentListener Instance() {
		return instance;
	}
	#endregion

	void Awake() {
		SingletonCheck();
	}

	private string ipAddress = "127.0.0.1";
	public int port = 10000;

	private TcpListener tcpListener;
	private byte[] buffer = new byte[2048];

	Func<string, IEnumerator> messageReceivedAction;

	void Start() {
		tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
		tcpListener.Start();
	}

	void Update() {
		if (tcpListener.Pending()) {
			StartCoroutine(ProcessMessage());
		}
	}

	IEnumerator ProcessMessage() {
		using (TcpClient connectedTcpClient = tcpListener.AcceptTcpClient()) {
			using (NetworkStream stream = connectedTcpClient.GetStream()) {
				while (!stream.DataAvailable) {
					yield return null;
				}

				string clientMessage = "";
				while (stream.DataAvailable) {
					int length = stream.Read(buffer, 0, buffer.Length);
					var incommingData = new byte[length];
					Array.Copy(buffer, 0, incommingData, 0, length);
					clientMessage += Encoding.ASCII.GetString(incommingData);
				}

				if (messageReceivedAction != null) {
					CoroutineWithData<string> messageProcessorCoroutine = new CoroutineWithData<string>(this, messageReceivedAction(clientMessage));
					yield return messageProcessorCoroutine.coroutine;

					string response = messageProcessorCoroutine.GetResult();
					byte[] data = Encoding.ASCII.GetBytes(response);
					stream.Write(data, 0, data.Length);
				} else {
					Debug.LogError("A message was received but there wasn't an action assigned to process it.");
				}
			}
		}
		yield return null;
	}

	public void SetMessageReceivedAction(Func<string, IEnumerator> actionToExecute) {
		messageReceivedAction = actionToExecute;
	}

}
