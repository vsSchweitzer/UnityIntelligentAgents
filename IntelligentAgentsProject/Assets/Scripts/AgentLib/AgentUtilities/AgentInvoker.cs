using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class AgentInvoker {

	public static IEnumerator Invoke(IntelligentAgent agent, string action, List<string> parameters) {
		MethodInfo actionMethod = agent.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
		List<object> genericParameters = ConvertParameterList(actionMethod, parameters);
		if (actionMethod != null) {
			if (actionMethod.GetCustomAttribute(typeof(AgentAction), true) != null) {
				if (actionMethod.ReturnType == typeof(void)) {
					actionMethod.Invoke(agent, genericParameters.ToArray());
					yield return new List<Percept>();
				} else if (actionMethod.ReturnType == typeof(List<Percept>)) {
					yield return (List<Percept>)actionMethod.Invoke(agent, genericParameters.ToArray());
				} else if (actionMethod.ReturnType == typeof(IEnumerator)) {
					CoroutineWithData<List<Percept>> invocationCoroutine = new CoroutineWithData<List<Percept>>(agent, (IEnumerator) actionMethod.Invoke(agent, genericParameters.ToArray()));
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

	private static List<object> ConvertParameterList(MethodInfo method, List<string> parameters) {
		List<object> genericParameterList = new List<object>();
		ParameterInfo[] methodParameters = method.GetParameters();
		if (methodParameters.Length == parameters.Count) {
			int i = 0;
			foreach (ParameterInfo paramInfo in methodParameters) {
				try {
					TypeConverter converter = TypeDescriptor.GetConverter(paramInfo.ParameterType);
					genericParameterList.Add(converter.ConvertFrom(parameters[i]));
				} catch {
					Debug.LogError("Method " + method.Name + " has a parameter that can't be converted from a string: " + paramInfo.ParameterType + " " + paramInfo.Name);
					throw new Exception();
				}
				i++;
			}
		} else if (methodParameters.Length > parameters.Count) {
			Debug.Log("Method " + method.Name + " was called with less parameters than it has");
			throw new Exception();
		} else if (methodParameters.Length < parameters.Count) {
			Debug.Log("Method " + method.Name + " was called with more parameters than it has");
			throw new Exception();
		}

		return genericParameterList;
	}

}
