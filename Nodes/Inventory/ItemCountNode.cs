using UnityEngine;
using System.Collections;

[NodeDescription("Item Count", 0, 1, NodeDescription.Type.INVENTORY)]
public class ItemCountNode : MarrowNode, IValueNode {
	[MarrowProperty]
	public GameObject objectWithInventory;
	
	[MarrowProperty]
	public string itemName;
	
	[MarrowProperty]
	public bool countStacks = false;
	
	MarrowInventory inventory;
	
	void OnEnable() {
		if (objectWithInventory != null) {
			inventory = objectWithInventory.GetComponent<MarrowInventory>();
			if (inventory == null)
				throw new UnityException("Object does not have an inventory.");
		}
	}
	
	public int GetInt() {
		System.Type itemType = System.Type.GetType(itemName);
		
		if (itemType != null)
			return inventory.GetNumberOfType(itemType, countStacks);
		else
			return 0;
	}
	
	public float GetFloat() {
		return (float) GetInt();
	}
	
	public string GetString() {
		return GetInt().ToString();
	}
}
