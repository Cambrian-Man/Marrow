using UnityEngine;
using System.Collections;

[NodeDescription("Collect", 2, 1, NodeDescription.Type.CONTROL)]
public class CollectNode : MarrowNode, IValueNode {
	[MarrowProperty]
	public int numberIn = 2;
	
	private float value;
	
	public override void OnInput (int input) {
		IValueNode node = GetInput(input) as IValueNode;
		if (node != null)
			value = node.GetFloat();
		
		SendOutput(0);
	}
	
	override public string[] InputLabels {
		get {
			string[] inputLabels = new string[GetInputSize()];
			for (int i = 0; i < GetInputSize(); i++) {
				inputLabels[i] = "Input " + i;
			}
			return inputLabels;
		}
	}
	
	override public void OnPropertyChange() {
		if (numberIn > 0) {
			SetInputSize(numberIn);
		}
	}
	
	public float GetFloat() {
		return value;
	}
	
	public float GetFloat(int input) {
		IValueNode node = GetInput(input) as IValueNode;
		if (node != null)
			return node.GetFloat();
		else
			return 0;
	}
	
	public int GetInt() {
		return Mathf.RoundToInt(value);
	}
	
	public int GetInt(int input) {
		if (GetInput(input) is IValueNode)
			return ((IValueNode) GetInput(input)).GetInt();
		else
			return 0;
	}
	
	public string GetString() {
		return GetFloat().ToString();
	}
}
