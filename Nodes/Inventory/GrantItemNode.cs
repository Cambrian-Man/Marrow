using UnityEngine;
using System.Collections;

[NodeDescription("Grant Item", 1, 0, NodeDescription.Type.INVENTORY)]
public class GrantItemNode : MarrowNode {
	[MarrowProperty]
	public GameObject objectWithInventory;
	
	[MarrowProperty]
	public string itemClassName;
	
	[MarrowProperty]
	public int amount = 1;
	
	[MarrowProperty]
	public bool isStack;
	
	[MarrowProperty]
	public bool mergeStacks;
	
	MarrowInventory inventory;
		
	void OnEnable() {
		if (objectWithInventory != null) {
			inventory = objectWithInventory.GetComponent<MarrowInventory>();
			if (inventory == null)
				throw new UnityException("Object does not have an inventory.");
		}
	}
	
	public override void OnInput (int input) {
		System.Type itemType = System.Type.GetType(itemClassName);
		
		if (isStack) {
			ItemStack stack = new ItemStack(itemType);
			if (stack != null) {
				stack.StackSize = 64;
				stack.Count = amount;
				addStack(stack);
			}
		}
		else {
			IInventoryItem item = System.Activator.CreateInstance(itemType) as IInventoryItem;
			if (item != null) {
				addItem(item);
			}
		}
	}
	
	void addItem(IInventoryItem item) {
		for (int i = 0; i < amount; i++) {
			try {
				inventory.Add(item);
			}
			catch (InventoryFullException ex) {
				Debug.Log(ex);
				objectWithInventory.SendMessage("OnFullInventory");
			}
		}
	}
	
	void addStack(ItemStack stack) {
		try {
			inventory.AddStack(stack, mergeStacks);
		}
		catch (InventoryFullException ex) {
			Debug.Log(ex);
			objectWithInventory.SendMessage("OnFullInventory");
		}
	}
}
