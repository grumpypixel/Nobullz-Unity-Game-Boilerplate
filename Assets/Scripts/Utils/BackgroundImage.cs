using System.Collections.Generic;
using UnityEngine;

namespace game
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class BackgroundImage : MonoBehaviour
	{
		public Camera optCameraReference;

		void Start()
		{
			ScaleBackground();
		}

		private void ScaleBackground()
		{
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			if (spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}

			Vector2 size = spriteRenderer.sprite.bounds.size;
			Camera camera = this.optCameraReference != null ? this.optCameraReference : Camera.main;
			Bounds bounds = new Bounds(Vector3.zero, new Vector3(camera.orthographicSize * camera.aspect * 2, camera.orthographicSize * 2, 0));

			Vector3 localScale = Vector3.one;
			localScale.x = bounds.size.x / size.x;
			localScale.y = bounds.size.y / size.y;
			this.transform.localScale = localScale;
		}
	}
}
