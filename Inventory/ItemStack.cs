using UnityEngine;
using System.Collections;

public class ItemStack : IInventoryItem {
	System.Type itemType;

	int count;
	
	int stackSize;
	
	public ItemStack (System.Type type) {
		itemType = type;
		
		if (itemType == null) {
			if (!itemType.IsAssignableFrom(typeof(IInventoryItem)))
				throw new UnityException("Type is not an item");
			else	
				itemType = type;
		}
	}
	
	public int StackSize {
		get {
			return stackSize;
		}
		set {
			if (count > stackSize)
				count = stackSize;
			stackSize = value;
		}
	}
	
	public int Count {
		get {
			return count;
		}
		set {
			count = value;
		}
	}
	
	public System.Type Item {
		get {
			return itemType;
		}
	}
	
	public ItemStack Split (int amount) {
		ItemStack newStack = new ItemStack(Item);
		newStack.StackSize = StackSize;
		newStack.Count = amount;
		Count -= amount;
		return newStack;
	}
}
