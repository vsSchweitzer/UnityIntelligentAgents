using System.Collections.Generic;

[System.Serializable]
public class ActResponseMessage : BaseMessage {

	public ActResponseStatus status;
	public List<Percept> percepts;
	
}
