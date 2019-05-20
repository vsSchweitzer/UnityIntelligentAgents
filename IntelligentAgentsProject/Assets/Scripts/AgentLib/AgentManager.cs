using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

	private Dictionary<string, IntelligentAgent> agentsInScene;
	private AgentListener myAgentListener;

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
		myAgentListener = FindObjectOfType<AgentListener>();
		myAgentListener.SetMessageReceivedAction(MessageReceivedCallback);
	}

	IntelligentAgent AgentFromIdentifier(string identifier) {
		return agentsInScene[identifier];
	}

	public IEnumerator MessageReceivedCallback(string message) {
		BaseMessage baseMessage = AgentMessageInterpreter.Interpret(message);
		switch (baseMessage.GetTypeEnum()) {
			case MessageType.ACT:
				ActMessage actMessage = (ActMessage) baseMessage;
				IntelligentAgent agent = AgentFromIdentifier(actMessage.agent);
				CoroutineWithData<string> methodCoroutine = new CoroutineWithData<string>(this, ExecuteAction(agent, actMessage.action, actMessage.parameters));
				yield return methodCoroutine.coroutine;
				yield return methodCoroutine.GetResult();
				break;
			default:
				// TODO
				yield return null;
				break;
		}
	}
	
	IEnumerator ExecuteAction(IntelligentAgent agent, string action, List<string> parameters) {
		CoroutineWithData<List<Percept>> invokerCoroutine = new CoroutineWithData<List<Percept>>(this, AgentInvoker.Invoke(agent, action, parameters));
		yield return invokerCoroutine.coroutine;
		List<Percept> percepts = invokerCoroutine.GetResult();

		string responseJsonMessage = AgentMessageInterpreter.MessageAsJson(
				AgentMessageInterpreter.BuildActResponse(ActResponseStatus.SUCCESS, percepts)
			);
		yield return responseJsonMessage;
	}

}
