using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AgentManager : MonoBehaviour {

	private Dictionary<string, IntelligentAgent> agentsInScene;
	private AgentListener agentMessageListener;

	[SerializeField]
	private int portToListen = 10000;

	void Start() {
		PopulateAgentDictionary();
		SetupListener();
	}

	void PopulateAgentDictionary() {
		agentsInScene = new Dictionary<string, IntelligentAgent>();
		IntelligentAgent[] foundAgents = FindObjectsOfType<IntelligentAgent>();
		foreach (IntelligentAgent intelligentAgent in foundAgents) {
			string identifier = intelligentAgent.getAgentIdentifier();
			try {
				agentsInScene.Add(identifier, intelligentAgent);
			} catch (System.ArgumentException) {
				Debug.LogError("There are multiple agents with the same identifier: " + identifier);
			}
		}
	}

	void SetupListener() {
		agentMessageListener = new AgentListener(portToListen);
		agentMessageListener.StartListening(MessageReceivedCallback);
	}

	IntelligentAgent AgentFromIdentifier(string identifier) {
		return agentsInScene[identifier];
	}

	public void MessageReceivedCallback(TcpClient connectedClient, string message) {
		BaseMessage baseMessage = AgentMessageInterpreter.Interpret(message);

		switch (baseMessage.type) {
			case MessageType.ACT:
				ActMessage actMessage = (ActMessage) baseMessage;
				IntelligentAgent agent = AgentFromIdentifier(actMessage.agent);
				StartCoroutine(ExecuteActionCoroutine(connectedClient, agent, actMessage.action, actMessage.parameters));
				break;
			default:
				// TODO
				break;
		}
	}

	// TODO rever o IEnumerator<List<Percept>> pra ver se tem como ter um retorno
	IEnumerator ExecuteActionCoroutine(TcpClient connectedClient, IntelligentAgent agent, string action, List<string> parameters) {

		CoroutineWithData<List<Percept>> invocationCoroutine = new CoroutineWithData<List<Percept>>(this, AgentInvoker.Invoke(agent, action, parameters));
		yield return invocationCoroutine.coroutine;
		List<Percept> percepts = invocationCoroutine.result;

		string responseJsonMessage = AgentMessageInterpreter.MessageAsJson(
				AgentMessageInterpreter.buildActResponse(ActResponseStatus.SUCCESS, percepts)
			);
		agentMessageListener.SendResponseToClient(connectedClient, responseJsonMessage);
	}

}
