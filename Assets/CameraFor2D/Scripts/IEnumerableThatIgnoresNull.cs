using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class that allows iteration over a collection of components without needing to
/// null check each element.  This allows things such as Linq methods to be applied to the
/// collection when the Unity GameObject the Component is attached to may have been destroyed.
/// </summary>
public class IEnumerableThatIgnoresNull<T> : IEnumerable<T>, IEnumerable where T : Component {
	IEnumerable<T> wrappedEnumerable;

	public IEnumerableThatIgnoresNull(IEnumerable<T> wrappedEnumerable) {
		this.wrappedEnumerable = wrappedEnumerable;
	}
	
	public IEnumerator<T> GetEnumerator() {
		foreach(T element in wrappedEnumerable) {
			if(element != null) yield return element;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		foreach(T element in wrappedEnumerable) {
			if(element != null) yield return element;
		}
	}
}