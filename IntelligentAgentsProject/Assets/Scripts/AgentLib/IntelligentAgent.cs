using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelligentAgent : MonoBehaviour {

	[SerializeField]
	string agentIdentifier = "Agent-0";

	public string getAgentIdentifier() {
		return agentIdentifier;
	}

}
