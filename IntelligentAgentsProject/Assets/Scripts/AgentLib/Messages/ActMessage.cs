using System.Collections.Generic;

[System.Serializable]
public class ActMessage : BaseMessage {

	public string agent;
	public string action;
	public List<string> parameters;

}
