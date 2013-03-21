using UnityEngine;
using UnityEditor;
using System.Collections;

public class tk2dSpriteAnimationPreview 
{
	tk2dSpriteThumbnailCache spriteThumbnailRenderer = new tk2dSpriteThumbnailCache();

	private void Init()
	{
	}

	public void Destroy()
	{
		spriteThumbnailRenderer.Destroy();
		tk2dGrid.Done();
	}

	void Repaint() { HandleUtility.Repaint(); }

	public int Frame { get; set; }
	Vector2 translate = Vector2.zero;
	float scale = 1.0f;
	bool dragging = false;

	public void ResetTransform()
	{
		scale = 1.0f;
		translate.Set(0, 0);
		Repaint();
	}

	public void Draw(Rect r, tk2dSpriteDefinition sprite)
	{
		Init();

		Event ev = Event.current;
		switch (ev.type)
		{
			case EventType.MouseDown:
				if (r.Contains(ev.mousePosition))
				{
					dragging = true;
					ev.Use();
				}
				break;
			case EventType.MouseDrag:
				if (dragging && r.Contains(ev.mousePosition)) 
				{
					translate += ev.delta;
					ev.Use();
					Repaint();
				}
				break;
			case EventType.MouseUp:
				dragging = false;
				break;
			case EventType.ScrollWheel:
				if (r.Contains(ev.mousePosition)) 
				{
					scale = Mathf.Clamp(scale + ev.delta.y * 0.1f, 0.1f, 10.0f);
					ev.Use();
					Repaint();
				}
				break;
		}

		tk2dGrid.Draw(r, translate);

		// Draw sprite
		if (sprite != null)
		{
			spriteThumbnailRenderer.DrawSpriteTextureCentered(r, sprite, translate, scale, Color.white);
		}
	}
}
