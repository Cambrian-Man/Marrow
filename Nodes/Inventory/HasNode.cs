using UnityEngine;
using System.Collections;

[NodeDescription("Has Item", 1, 2, NodeDescription.Type.INVENTORY)]
public class HasNode : MarrowNode {
	[MarrowProperty]
	public GameObject objectWithInventory;
	
	[MarrowProperty]
	public string itemName;
	
	MarrowInventory inventory;
	
	void OnEnable() {
		if (objectWithInventory != null) {
			inventory = objectWithInventory.GetComponent<MarrowInventory>();
			if (inventory == null)
				throw new UnityException("Object does not have an inventory.");
		}
	}
	
	public override string[] OutputLabels {
		get {
			return new string[] {"True", "False"};
		}
	}
	
	public override void OnInput (int input) {
		if (IsTrue()) {
			SendOutput(0);
		}
		else {
			SendOutput(1);
		}
	}
	
	public override bool IsTrue () {
		return inventory.Contains(itemName);
	}
}
