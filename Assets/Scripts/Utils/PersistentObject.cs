using UnityEngine;

namespace game
{
	public class PersistentObject : MonoBehaviour
	{
		void Start()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
