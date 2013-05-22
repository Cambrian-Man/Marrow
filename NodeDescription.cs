using UnityEngine;
using System.Collections;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class NodeDescription : System.Attribute {
	private string cleanName;
	private int inputs;
	private int outputs;
	private Type type;
	
	public enum Type {
		INPUT,
		OUTPUT,
		BOOLEAN,
		CONTROL,
		GENERATOR,
		MATH,
		STRING,
		INVENTORY,
		CUSTOM
	}
	
	public NodeDescription(string name, int numIn, int numOut, Type type = Type.CUSTOM) {
		cleanName = name;
		inputs = numIn;
		outputs = numOut;
		this.type = type;
	}
	
	public string GetName() {
		return cleanName;
	}
	
	public int GetInputs() {
		return inputs;
	}
	
	public int GetOutputs() {
		return outputs;
	}
	
	public NodeDescription.Type GetNodeType() {
		return type;
	}
}
