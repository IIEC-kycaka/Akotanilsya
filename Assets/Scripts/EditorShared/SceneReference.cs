//
// Author: Jonatan Johansson
// Created: 2018/19-XX-XX
// Updated: 2021-01-26
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A reference to a scene selectable from the inspector.
/// </summary>
[Serializable]
public struct SceneReference : ISerializationCallbackReceiver {
	// This field is only used in builds as the editor can just query the scene asset.
	[SerializeField, HideInInspector]
	private int buildIndex;

#if UNITY_EDITOR
	// The SceneAsset type is only available in the editor, so this field is excluded from builds.
	[SerializeField] private SceneAsset sceneAsset;
#endif

	/// <summary>
	/// The build index of the scene. Returns -1 if no scene is assigned.
	/// </summary>
	public int BuildIndex {
		get {
		#if UNITY_EDITOR
			// GetBuildIndexByScenePath returns -1 if the given argument does not refer to a valid scene.
			return SceneUtility.GetBuildIndexByScenePath(ScenePath);
		#else
			return buildIndex;
		#endif
		}
	}

	/// <summary>
	/// The path to the scene. Returns null if no scene is assigned.
	/// </summary>
	public string ScenePath {
		get {
		#if UNITY_EDITOR
			if (sceneAsset != null)
				return AssetDatabase.GetAssetPath(sceneAsset);
			return null;
		#else
			return SceneUtility.GetScenePathByBuildIndex(buildIndex);
		#endif
		}
	}

	// Serialization will always happen when building the player. Thus, we can use this callback to ensure that the
	// `buildIndex` field gets populated with the right value in builds.
	void ISerializationCallbackReceiver.OnBeforeSerialize() {
	#if UNITY_EDITOR
		buildIndex = BuildIndex;
	#endif
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize() { }

	// Implicit conversion to int so that it can be used directly as an argument to SceneManager.LoadScene etc.
	public static implicit operator int(SceneReference sceneReference) => sceneReference.BuildIndex;
}


#if UNITY_EDITOR
namespace PropertyDrawers {
	[CustomPropertyDrawer(typeof(SceneReference))]
	public class SceneReferencePropertyDrawer : PropertyDrawer {
		private SerializedProperty sceneAssetProperty;
		private SceneInfo sceneInfo;
		private bool hasScene;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			sceneAssetProperty = property.FindPropertyRelative("sceneAsset");

			// Get the selected scene.
			SceneAsset sceneAsset = sceneAssetProperty.objectReferenceValue as SceneAsset;
			hasScene = sceneAsset != null;
			if (hasScene)
				sceneInfo = GetSceneInfo(sceneAsset);

			// Handle context menu.
			Event e = Event.current;
			if (e.type == EventType.MouseDown && e.button == 1 && position.Contains(e.mousePosition))
				DoContextMenu();

			// Set color based on state in build settings.
			Color oldColor = GUI.backgroundColor;
			if (hasScene) {
				if (!sceneInfo.InBuildSettings) {
					GUI.backgroundColor = Color.red;
					label.tooltip = "*Not in build* " + label.tooltip;
				}
				else if (!sceneInfo.enabled) {
					GUI.backgroundColor = Color.yellow;
					label.tooltip = "*Disabled in build* " + label.tooltip;
				}
				else {
					GUI.backgroundColor = Color.green;
					label.tooltip = $"*Enabled at build index: {sceneInfo.indexInBuildSettings}* {label.tooltip}";
				}
			}

			EditorGUI.PropertyField(position, sceneAssetProperty, label);

			GUI.backgroundColor = oldColor;
		}

		private void DoContextMenu() {
			if (!hasScene) return;
			GenericMenu menu = new GenericMenu();

			if (hasScene) {
				if (sceneInfo.InBuildSettings)
					menu.AddItem(new GUIContent("Remove scene from build settings"), false, RemoveFromBuild, sceneAssetProperty);
				else
					menu.AddItem(new GUIContent("Add scene to build settings"), false, AddToBuild, sceneAssetProperty);

				if (sceneInfo.InBuildSettings)
					menu.AddItem(new GUIContent("Enabled in build"), sceneInfo.enabled, ToggleEnabledInBuild, sceneAssetProperty);
			}

			menu.ShowAsContext();
		}


		//
		// Context actions
		//

		private static void AddToBuild(object arg) {
			SerializedProperty property = (SerializedProperty)arg;
			property.serializedObject.Update();

			if (!TryGetSceneFromProperty(property, out SceneInfo sceneInfo)) return;
			if (sceneInfo.InBuildSettings) return;

			EditorBuildSettings.scenes =
				EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(sceneInfo.path, true)).ToArray();
		}

		private static void RemoveFromBuild(object arg) {
			SerializedProperty property = (SerializedProperty)arg;
			property.serializedObject.Update();

			if (!TryGetSceneFromProperty(property, out SceneInfo scene)) return;
			if (!scene.InBuildSettings) return;

			List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();
			int index = scene.indexInBuildSettings;
			if (index < buildScenes.Count && buildScenes[index].path == scene.path) {
				buildScenes.RemoveAt(index);
				EditorBuildSettings.scenes = buildScenes.ToArray();
			}
		}

		private static void ToggleEnabledInBuild(object arg) {
			SerializedProperty property = (SerializedProperty)arg;
			property.serializedObject.Update();

			if (!TryGetSceneFromProperty(property, out SceneInfo scene)) return;
			if (!scene.InBuildSettings) return;

			EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
			int index = scene.indexInBuildSettings;
			if (index < buildScenes.Length && buildScenes[index].path == scene.path) {
				buildScenes[index].enabled = !scene.enabled;
				EditorBuildSettings.scenes = buildScenes;
			}
		}

		//
		// Helpers
		//

		private struct SceneInfo {
			public SceneAsset asset;
			public string path;
			public bool enabled;
			public int indexInBuildSettings;

			public bool InBuildSettings => indexInBuildSettings != -1;
			public bool IsValid => path != null;
		}

		private static SceneInfo GetSceneInfo(SceneAsset sceneAsset) {
			string assetPath = AssetDatabase.GetAssetPath(sceneAsset);
			if (assetPath == null) return new SceneInfo {asset = sceneAsset};

			EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
			int index = Array.FindIndex(buildScenes, buildScene => buildScene.path == assetPath);

			return new SceneInfo {
				asset = sceneAsset,
				path = assetPath,
				indexInBuildSettings = index,
				enabled = index != -1 && buildScenes[index].enabled
			};
		}

		private static bool TryGetSceneFromProperty(SerializedProperty property, out SceneInfo sceneInfo) {
			SceneAsset sceneAsset = property.objectReferenceValue as SceneAsset;
			if (sceneAsset != null) {
				sceneInfo = GetSceneInfo(sceneAsset);
				return sceneInfo.IsValid;
			}

			sceneInfo = default;
			return false;
		}
	}
}
#endif