using System;
using System.Collections.Generic;

[System.Serializable]
public class ActResponseMessage : BaseMessage {

	public string status;
	public List<Percept> percepts;

	public ActResponseMessage() {
		SetType(MessageType.ACT_RESPONSE);
	}

	public ActResponseStatus GetStatusEnum() {
		return (ActResponseStatus)Enum.Parse(typeof(ActResponseStatus), status);
	}

	public void SetStatus(ActResponseStatus status) {
		this.status = status.ToString().ToUpper();
	}

}
