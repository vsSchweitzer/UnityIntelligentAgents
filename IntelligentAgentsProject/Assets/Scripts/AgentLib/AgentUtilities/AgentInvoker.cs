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
					yield return actionMethod.Invoke(agent, parameters.ToArray());
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
