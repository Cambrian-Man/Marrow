using UnityEngine;
using System.Collections;

[NodeDescription("Broadcast", 2, 0, NodeDescription.Type.OUTPUT)]
public class BroadcastNode : OutputNode {
	[MarrowProperty]
	public string message;
	
	[MarrowProperty]
	public string text = "";

	public static string defaultMessage = "OnInteract";
	
	override public string[] InputLabels {
		get {
			return new string[]{"Input", "Text"};
		}
	}
	
	override public void OnInput(int input) {
		if (message == null)
			message = defaultMessage;
		
		if (GetInput(1) != null) {
			if (!(GetInput(1) is IStringNode))
				throw new UnityException("Input is not string node");
			else
				text = ((IStringNode) GetInput(1)).GetString();
		}
		
		gameObject.BroadcastMessage(message, text, SendMessageOptions.DontRequireReceiver); 
	}
}
