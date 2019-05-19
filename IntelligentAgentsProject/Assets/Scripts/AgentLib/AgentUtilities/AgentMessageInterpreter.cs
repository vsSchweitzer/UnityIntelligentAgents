using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AgentMessageInterpreter {

	public static BaseMessage Interpret(string message) {
		BaseMessage baseMessage = JsonUtility.FromJson<BaseMessage>(message);
		switch (baseMessage.type) {
			case MessageType.ACT:
				return JsonUtility.FromJson<ActMessage>(message);
			default:
				throw new IOException();
		}
	}

	public static string MessageAsJson(BaseMessage message) {
		return JsonUtility.ToJson(message);
	}

	public static ActResponseMessage buildActResponse(ActResponseStatus status, List<Percept> percepts) {
		ActResponseMessage message = new ActResponseMessage();
		message.percepts = percepts;
		message.status = status;
		return message;
	}

}
