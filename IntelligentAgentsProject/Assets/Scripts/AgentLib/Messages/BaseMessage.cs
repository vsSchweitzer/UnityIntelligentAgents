
using System;

[System.Serializable]
public class BaseMessage {

	public string type;

	public MessageType GetTypeEnum() {
		return (MessageType)Enum.Parse(typeof(MessageType), type);
	}

	public void SetType(MessageType type) {
		this.type = type.ToString().ToUpper();
	}

}