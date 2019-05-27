using System;
using System.Collections.Generic;

[System.Serializable]
public class ActResponseMessage : BaseMessage {

	public string status;
	public List<Percept> percepts;

	public ActResponseMessage() {
		SetType(MessageType.ACT_RESPONSE);
		SetStatus(ActResponseStatus.SUCCESS);
		percepts = new List<Percept>();
	}

	public ActResponseMessage(List<Percept> percepts) {
		SetType(MessageType.ACT_RESPONSE);
		SetStatus(ActResponseStatus.SUCCESS);
		this.percepts = percepts;
	}

	public ActResponseMessage(ActResponseStatus status) {
		SetType(MessageType.ACT_RESPONSE);
		SetStatus(status);
		percepts = new List<Percept>();
	}

	public ActResponseStatus GetStatusEnum() {
		return (ActResponseStatus)Enum.Parse(typeof(ActResponseStatus), status);
	}

	public void SetStatus(ActResponseStatus status) {
		this.status = status.ToString().ToUpper();
	}

}
