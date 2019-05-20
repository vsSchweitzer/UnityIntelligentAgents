using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class AgentInvoker {

	public static IEnumerator Invoke(IntelligentAgent agent, string action, List<string> parameters) {
		MethodInfo actionMethod = agent.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if (actionMethod != null) {
			if (actionMethod.GetCustomAttribute(typeof(AgentAction), true) != null) {
				if (actionMethod.ReturnType == typeof(void)) {
					actionMethod.Invoke(agent, parameters.ToArray());
					yield return new List<Percept>();
				} else if (actionMethod.ReturnType == typeof(List<Percept>)) {
					yield return (List<Percept>)actionMethod.Invoke(agent, parameters.ToArray());
				} else if (actionMethod.ReturnType == typeof(IEnumerator)) {
					CoroutineWithData<List<Percept>> invocationCoroutine = new CoroutineWithData<List<Percept>>(agent, (IEnumerator) actionMethod.Invoke(agent, parameters.ToArray()));
					yield return invocationCoroutine.coroutine;
					List<Percept> percepts;
					try {
						percepts = invocationCoroutine.GetResult();
					} catch {
						percepts = new List<Percept>();
					}
					yield return percepts;
				} else {
					Debug.LogError("Action \"" + action + "\" on agent \"" + agent.getAgentIdentifier() + "\" should return one of the following: \"List<Percept>\", \"IEnumerator\" or \"void\".");
					throw new Exception();
				}
			} else {
				Debug.LogError("Action \"" + action + "\" on agent \"" + agent.getAgentIdentifier() + "\" is not tagged with \"[AgentAction]\" attribute.");
				throw new Exception();
			}
		} else {
			Debug.LogError("Action \"" + action + "\" on agent \"" + agent.getAgentIdentifier() + "\" was not found.");
			throw new Exception();
		}
	}

}
