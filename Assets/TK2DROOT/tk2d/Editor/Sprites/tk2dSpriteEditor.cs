using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(tk2dSprite))]
class tk2dSpriteEditor : Editor
{
	tk2dSpriteThumbnailCache thumbnailCache;

    public override void OnInspectorGUI()
    {
        tk2dBaseSprite sprite = (tk2dBaseSprite)target;
		DrawSpriteEditorGUI(sprite);
    }

    void OnEnable()
    {
    	thumbnailCache = new tk2dSpriteThumbnailCache();
    }
	
	void OnDestroy()
	{
		thumbnailCache.Destroy();
		tk2dGrid.Done();
	}
	
	// Callback and delegate
	void SpriteChangedCallbackImpl(tk2dSpriteCollectionData spriteCollection, int spriteId, object data)
	{
		tk2dBaseSprite s = target as tk2dBaseSprite;
		if (s != null)
		{
			s.SwitchCollectionAndSprite(spriteCollection, spriteId);
			s.EditMode__CreateCollider();
			EditorUtility.SetDirty(target);
		}
	}
	tk2dSpriteGuiUtility.SpriteChangedCallback _spriteChangedCallbackInstance = null;
	tk2dSpriteGuiUtility.SpriteChangedCallback spriteChangedCallbackInstance {
		get {
			if (_spriteChangedCallbackInstance == null) {
				_spriteChangedCallbackInstance = new tk2dSpriteGuiUtility.SpriteChangedCallback( SpriteChangedCallbackImpl );
			}
			return _spriteChangedCallbackInstance;
		}
	}

	protected void DrawSpriteEditorGUI(tk2dBaseSprite sprite)
	{
		Event ev = Event.current;
		tk2dSpriteGuiUtility.SpriteSelector( sprite.Collection, sprite.spriteId, spriteChangedCallbackInstance, null );

        if (sprite.Collection != null)
        {
        	if (tk2dPreferences.inst.displayTextureThumbs) {
				tk2dSpriteDefinition def = sprite.GetCurrentSpriteDef();
				if (sprite.Collection.version < 1 || def.texelSize == Vector2.zero)
				{
					string message = "";
					
					message = "No thumbnail data.";
					if (sprite.Collection.version < 1)
						message += "\nPlease rebuild Sprite Collection.";
					
					tk2dGuiUtility.InfoBox(message, tk2dGuiUtility.WarningLevel.Info);
				}
				else
				{
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel(" ");

					int tileSize = 128;
					Rect r = GUILayoutUtility.GetRect(tileSize, tileSize, GUILayout.ExpandWidth(false));
					tk2dGrid.Draw(r);
					thumbnailCache.DrawSpriteTextureInRect(r, def, Color.white);

					GUILayout.EndHorizontal();

					r = GUILayoutUtility.GetLastRect();
					if (ev.type == EventType.MouseDown && ev.button == 0 && r.Contains(ev.mousePosition)) {
						tk2dSpriteGuiUtility.SpriteSelectorPopup( sprite.Collection, sprite.spriteId, spriteChangedCallbackInstance, null );
					}
				}
			}

            sprite.color = EditorGUILayout.ColorField("Color", sprite.color);
			Vector3 newScale = EditorGUILayout.Vector3Field("Scale", sprite.scale);
			if (newScale != sprite.scale)
			{
				sprite.scale = newScale;
				sprite.EditMode__CreateCollider();
			}
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("HFlip"))
			{
				Vector3 s = sprite.scale;
				s.x *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			if (GUILayout.Button("VFlip"))
			{
				Vector3 s = sprite.scale;
				s.y *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button(new GUIContent("Reset Scale", "Set scale to 1")))
			{
				Vector3 s = sprite.scale;
				s.x = Mathf.Sign(s.x);
				s.y = Mathf.Sign(s.y);
				s.z = Mathf.Sign(s.z);
				sprite.scale = s;
				GUI.changed = true;
			}
			
			if (GUILayout.Button(new GUIContent("Bake Scale", "Transfer scale from transform.scale -> sprite")))
			{
				tk2dScaleUtility.Bake(sprite.transform);
				GUI.changed = true;
			}
			
			GUIContent pixelPerfectButton = new GUIContent("1:1", "Make Pixel Perfect");
			if ( GUILayout.Button(pixelPerfectButton ))
			{
				if (tk2dPixelPerfectHelper.inst) tk2dPixelPerfectHelper.inst.Setup();
				sprite.MakePixelPerfect();
				GUI.changed = true;
			}
			
			sprite.pixelPerfect = GUILayout.Toggle(sprite.pixelPerfect, new GUIContent("Always", "Always keep pixel perfect"), GUILayout.Width(60.0f));
			EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.IntSlider("Need a collection bound", 0, 0, 1);
        }
		
		bool needUpdatePrefabs = false;
		if (GUI.changed)
		{
			EditorUtility.SetDirty(sprite);
#if !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			if (PrefabUtility.GetPrefabType(sprite) == PrefabType.Prefab)
				needUpdatePrefabs = true;
#endif
		}
		
		// This is a prefab, and changes need to be propagated. This isn't supported in Unity 3.4
		if (needUpdatePrefabs)
		{
#if !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			// Rebuild prefab instances
			tk2dBaseSprite[] allSprites = Resources.FindObjectsOfTypeAll(sprite.GetType()) as tk2dBaseSprite[];
			foreach (var spr in allSprites)
			{
				if (PrefabUtility.GetPrefabType(spr) == PrefabType.PrefabInstance &&
					PrefabUtility.GetPrefabParent(spr.gameObject) == sprite.gameObject)
				{
					// Reset all prefab states
					var propMod = PrefabUtility.GetPropertyModifications(spr);
					PrefabUtility.ResetToPrefabState(spr);
					PrefabUtility.SetPropertyModifications(spr, propMod);
					
					spr.ForceBuild();
				}
			}
#endif
		}
	}
	
    [MenuItem("GameObject/Create Other/tk2d/Sprite", false, 12900)]
    static void DoCreateSpriteObject()
    {
		tk2dSpriteCollectionData sprColl = null;
		if (sprColl == null)
		{
			// try to inherit from other Sprites in scene
			tk2dSprite spr = GameObject.FindObjectOfType(typeof(tk2dSprite)) as tk2dSprite;
			if (spr)
			{
				sprColl = spr.Collection;
			}
		}

		if (sprColl == null)
		{
			tk2dSpriteCollectionIndex[] spriteCollections = tk2dEditorUtility.GetOrCreateIndex().GetSpriteCollectionIndex();
			foreach (var v in spriteCollections)
			{
				GameObject scgo = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(v.spriteCollectionDataGUID), typeof(GameObject)) as GameObject;
				var sc = scgo.GetComponent<tk2dSpriteCollectionData>();
				if (sc != null && sc.spriteDefinitions != null && sc.spriteDefinitions.Length > 0 && !sc.managedSpriteCollection)
				{
					sprColl = sc;
					break;
				}
			}

			if (sprColl == null)
			{
				EditorUtility.DisplayDialog("Create Sprite", "Unable to create sprite as no SpriteCollections have been found.", "Ok");
				return;
			}
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Sprite");
		tk2dSprite sprite = go.AddComponent<tk2dSprite>();
		sprite.SwitchCollectionAndSprite(sprColl, sprColl.FirstValidDefinitionIndex);
		sprite.renderer.material = sprColl.FirstValidDefinition.material;
		sprite.Build();
		
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create Sprite");
    }
}

