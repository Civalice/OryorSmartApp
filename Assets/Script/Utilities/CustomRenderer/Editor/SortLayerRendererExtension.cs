using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CanEditMultipleObjects()]
[CustomEditor(typeof(MeshRenderer),true)]
public class SortLayerRendererExtension : Editor {
	Renderer renderer;
	string[] sortingLayerNames;

	int selectedOption;
	bool applyToChild = false;
	bool applyToChildOldValue = false;

	void OnEnable()
	{
				sortingLayerNames = GetSortingLayerNames ();
				renderer = (target as Renderer).gameObject.GetComponent<Renderer>();

				for (int i = 0; i < sortingLayerNames.Length; i++) {
						if (sortingLayerNames [i] == renderer.sortingLayerName)
								selectedOption = i;

				}
	}

	public override void OnInspectorGUI()
	{
				DrawDefaultInspector ();
				if (!renderer)
						return;
				EditorGUILayout.LabelField ("\n");

				selectedOption = EditorGUILayout.Popup ("Sorting Layer", selectedOption, sortingLayerNames);
				if (sortingLayerNames [selectedOption] != renderer.sortingLayerName) {
						Undo.RecordObject (renderer, "Sorting Layer");
						if (!applyToChild)
								renderer.sortingLayerName = sortingLayerNames [selectedOption];
				
						EditorUtility.SetDirty (renderer);
				}
		int newSortingLayerOrder = EditorGUILayout.IntField ("Order in Layer", renderer.sortingOrder);
		if (newSortingLayerOrder != renderer.sortingOrder) {
			Undo.RecordObject(renderer, "Edit Sorting Order");
			renderer.sortingOrder = newSortingLayerOrder;
			EditorUtility.SetDirty(renderer);
				}
		applyToChild = EditorGUILayout.ToggleLeft ("Apply to Childs", applyToChild);
		if (applyToChild != applyToChildOldValue) {
			Undo.RecordObject(renderer, "Apply Sort Mode To child");
			applyToChildOldValue = applyToChild;
			EditorUtility.SetDirty(renderer);

				}

				
		}
	public string[] GetSortingLayerNames()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty ("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
