using UnityEngine;
using System.Collections;

[NodeDescription("Increment", 3, 1, NodeDescription.Type.MATH)]
public class IncrementNode : MarrowNode, IValueNode {
	[MarrowProperty]
	public bool repeat = true;
		
	[MarrowProperty]
	public int start;
	
	[MarrowProperty]
	public int end;
	
	protected int value;
	
	protected bool valuesSet;
	
	void OnEnable () {
		inputLabels = new string[] {"Input", "Start", "End"};
	}
	
	protected void SetValues() {
		if (GetInput(1) is IValueNode) {
			start = ((IValueNode) GetInput(1)).GetInt();
		}

		value = start;
		
		if (GetInput(2) is IValueNode) {
			end = ((IValueNode) GetInput (2)).GetInt();	
		}
		
		valuesSet = true;
	}
	
	public override void OnInput (int input) {
		if (!valuesSet)
			SetValues();
		
		if (value < end) {
			value++;
		}
		else if (value == end && repeat) {
			value = 0;
		}
		
		base.OnInput (0);
	}
	
 	public float GetFloat() {
		return (float) value;
	}
	
	public int GetInt() {
		return value;
	}
	
	public string GetString() {
		return value.ToString();
	}
}
