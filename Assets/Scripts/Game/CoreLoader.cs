using UnityEngine;

namespace game
{
	public class CoreLoader : MonoBehaviour
	{
		public GameObject corePrefab;
		public string     coreTag;

	#if UNITY_EDITOR
		void Awake()
		{
			if (GameObject.FindWithTag(coreTag) == null)
			{
				GameObject.Instantiate(corePrefab);
			}
		}
	#endif
	}
}
