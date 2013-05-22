using UnityEngine;
using System.Collections;

[NodeDescription("Number", 0, 1, NodeDescription.Type.GENERATOR)]
public class NumberNode : GeneratorNode {
	[MarrowProperty]
	public float number;
	
	override public float GetFloat() {
		return number;
	}
	
	override public int GetInt() {
		return Mathf.RoundToInt(number);
	}
}
