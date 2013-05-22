using UnityEngine;
using System.Collections;

[NodeDescription("Not", 1, 1, NodeDescription.Type.BOOLEAN)]
public class NotNode : MarrowNode {
	override public bool IsTrue() {
		return !GetInput(0).IsTrue();
	}
}
