using UnityEngine;
using System.Collections;

[NodeDescription("Split", 1, 2, NodeDescription.Type.CONTROL)]
public class SplitNode : MarrowNode, IValueNode {
	[MarrowProperty]
	public int numberOut = 2;
	
	private float value;
	
	override public void OnInput(int input) {
		if (GetInput(0) is IValueNode)
			value = ((IValueNode) GetInput(0)).GetFloat();
			
		for (int i = 0; i < GetOutputSize(); i++) {
			SendOutput(i);
		}
	}
	
	override public string[] OutputLabels {
		get {
			string[] outputLabels = new string[GetOutputSize()];
			for (int i = 0; i < GetOutputSize(); i++) {
				outputLabels[i] = "Output " + i;
			}
			return outputLabels;
		}
	}
	
	override public void OnPropertyChange() {
		if (numberOut > 0) {
			SetOutputSize(numberOut);
		}
	}
	
	public float GetFloat() {
		return value;
	}
	
	public int GetInt() {
		return Mathf.RoundToInt(value);
	}
	
	public string GetString() {
		return value.ToString();
	}
	
}
