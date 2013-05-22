using UnityEngine;
using System.Collections;

[NodeDescription("Concatenate", 1, 1, NodeDescription.Type.STRING)]
public class ConcatNode : MarrowNode, IStringNode {
	[MarrowProperty]
	public string text;
	
	[MarrowProperty]
	public int numberIn  = 1;
	
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
	
	public string GetString() {
		string output = (string) text.Clone();
		
		for (int i = 0; i < GetInputSize(); i++) {
			IStringNode node = GetInput(i) as IStringNode;
			if (node == null)
				throw new UnityException("Node is not a string node");
			else
				output = output.Replace("{" + i + "}", node.GetString());
		}
		
		return output;
	}
}
