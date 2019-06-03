using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using UnityEngine;

public static class AgentInvoker {

	private static readonly CultureInfo dotSeparatedFloat = CultureInfo.CreateSpecificCulture("en-US");

	public static IEnumerator Invoke(IntelligentAgent agent, string action, List<string> parameters) {
		MethodInfo actionMethod = agent.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
		List<object> genericParameters = ConvertParameterList(actionMethod, parameters);
		if (actionMethod != null) {
			if (actionMethod.GetCustomAttribute(typeof(AgentAction), true) != null) {
				if (actionMethod.ReturnType == typeof(void)) {
					actionMethod.Invoke(agent, genericParameters.ToArray());
					yield return new ActResponseMessage();
				} else if (actionMethod.ReturnType == typeof(ActResponseMessage)) {
					yield return (ActResponseMessage)actionMethod.Invoke(agent, genericParameters.ToArray());
				} else if (actionMethod.ReturnType == typeof(IEnumerator)) {
					CoroutineWithData<ActResponseMessage> invocationCoroutine = new CoroutineWithData<ActResponseMessage>(agent, (IEnumerator) actionMethod.Invoke(agent, genericParameters.ToArray()));
					yield return invocationCoroutine.coroutine;
					ActResponseMessage responseMessage;
					try {
						responseMessage = invocationCoroutine.GetResult();
						if (responseMessage == null) {
							responseMessage = new ActResponseMessage();
						}
					} catch {
						responseMessage = new ActResponseMessage();
					}
					yield return responseMessage;
				} else {
					Debug.LogError("Action \"" + action + "\" on agent \"" + agent.getAgentIdentifier() + "\" should return one of the following: \"ActResponseMessage\", \"IEnumerator\" or \"void\".");
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
					genericParameterList.Add(converter.ConvertFromInvariantString(parameters[i]));
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
