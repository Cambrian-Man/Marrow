using UnityEngine;
using System.Collections;

public class MarrowInventory : MonoBehaviour, IEnumerable {
	private IInventoryItem[] items;
	
	public int Size;
	
	// Use this for initialization
	void Start () {
		items = new IInventoryItem[Size];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public IInventoryItem this[int index] {
		get {
			return items[index];
		}
		set {
			items[index] = value;
			SendMessage("OnInventoryUpdate", index);
		}
	}
	
	public int Add(IInventoryItem item) {
		if (SpaceLeft > 0) {
			for (int i = 0; i < items.Length; i++) {
				if (items[i] == null) {
					items[i] = item;
					return i;
				}
			}
		}
		else {
			throw new InventoryFullException();
		}
		
		return -1;
	}
	
	public void Remove(IInventoryItem item) {
		for (int i = 0; i < items.Length; i++) {
			items[i] = null;
		}
	}
	
	public void Swap(int i, int j) {
		IInventoryItem first = items[i];
		IInventoryItem second = items[j];
		
		items[i] = second;
		items[j] = first;
	}
	
	public bool Contains(string name) {
		System.Type type = System.Type.GetType(name);
		
		return Contains(type);
	}
	
	public bool Contains<T>() where T : IInventoryItem {
		return Contains(typeof(T));
	}
	
	public bool Contains(System.Type type) {
		for (int i = 0; i < items.Length; i++) {
			ItemStack stack = items[i] as ItemStack;
			
			if (stack != null && stack.Item == type)
				return true;
			else if (items[i] != null && items[i].GetType() == type)
				return true;
		}
		
		return false;
		
	}
	
	public bool Contains(IInventoryItem item) {
		for (int i = 0; i < items.Length; i++) {
			if (items[i] == item)
				return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// Gets the number of items of a certain type.
	/// </summary>
	/// <returns>
	/// The number of items.
	/// </returns>
	/// <param name='itemType'>
	/// Item type.
	/// </param>
	/// <param name='countStacks'>
	/// If true, counts stacks as individual items,
	/// otherwise, return the total number of items.
	/// </param>
	public int GetNumberOfType(System.Type itemType, bool countStacks = false) {
		int total = 0;
		
		foreach (IInventoryItem item in this) {
			ItemStack stack = item as ItemStack;
			
			if (!countStacks && stack != null)
				total += stack.Count;
			else
				total++;
		}
		
		return total;
	}
	
	public int Count {
		get {
			int count = 0;
			for (int i = 0; i < items.Length; i++) {
				if (items[i] != null)
					count++;
			}
			
			return count;
		}
	}
	
	public int SpaceLeft {
		get {
			int space = 0;
			for (int i = 0; i < items.Length; i++) {
				if (items[i] == null)
					space++;
			}
			
			return space;
		}
	}
	
	public void AddStack (ItemStack stack, bool merge) {
		if (!merge)
			Add(stack);
		else if (!Contains(stack.Item))
			Add(stack);
		else
			Merge(stack);
	}
	
	public void Merge (ItemStack stack) {
		for (int i = 0; i < items.Length; i++) {
			ItemStack s = items[i] as ItemStack;
			if (s != null && s.Item == stack.Item) {
				int amountFree = s.StackSize - s.Count;
				
				if (amountFree > stack.Count) {
					s.Count += stack.Count;
					stack.Count = 0;
					break;
				}
				else {
					stack.Count = (s.Count + stack.Count) % stack.StackSize;
					s.Count = s.StackSize;
				}
			}
		}
		
		if (stack.Count > 0)
			Add(stack);
	}
	
	IEnumerator IEnumerable.GetEnumerator() {
		return (IEnumerator) GetEnumerator();
	}
	
	public MarrowInventoryEnumerator GetEnumerator () {
		return new MarrowInventoryEnumerator(items);
	}
}
