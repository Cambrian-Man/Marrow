using UnityEngine;
using System.Collections;

public class MarrowInventoryEnumerator : IEnumerator {
	public IInventoryItem[] _items;
	
	int position = -1;
	
	int count;
	
	public MarrowInventoryEnumerator(IInventoryItem[] items) {
		_items = items;
		
		for (int i = 0; i < _items.Length; i++) {
			if (_items[i] != null)
				count++;
		}
	}
	
	public bool MoveNext() {
		position++;
		
		return (position < count);
	}
	
	public void Reset() {
		position = -1;
	}
	
	object IEnumerator.Current {
		get {
			return Current;
		}
	}
	
	public IInventoryItem Current {
		get {
			try {
				return _items[position];
			}
			catch (System.IndexOutOfRangeException) {
				throw new System.InvalidOperationException();
			}
		}
	}
}
