using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AgentMessageInterpreter {

	public static BaseMessage Interpret(string message) {
		BaseMessage baseMessage = JsonUtility.FromJson<BaseMessage>(message);
		switch (baseMessage.GetTypeEnum()) {
			case MessageType.ACT:
				return JsonUtility.FromJson<ActMessage>(message);
			default:
				throw new IOException();
		}
	}

	public static string MessageAsJson(BaseMessage message) {
		return JsonUtility.ToJson(message);
	}

}
