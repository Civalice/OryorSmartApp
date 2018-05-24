using UnityEngine;
using System.Collections;

public class StateButtonObject<T> : ButtonObject
where T : new()
{
	public T WorkingState;
}
