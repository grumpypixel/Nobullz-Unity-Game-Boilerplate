using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class PoolObject : MonoBehaviour
	{
		private bool m_isAvailableForReuse = true;

		public bool isAvailableForReuse
		{
			get
			{
				return m_isAvailableForReuse && this.gameObject.activeInHierarchy == false;
			}
			set
			{
				m_isAvailableForReuse = value;
				this.gameObject.SetActive(!value);
			}
		}
	}

	public class GameObjectPool : MonoBehaviour
	{
		[System.Serializable]
		public class PoolData
		{
			public string		name;
			public GameObject	prefab;
			public Transform	parent;
			public int			initialCapacity = 8;
			public int			maxObjects = 128;
		}

		public PoolData[]		poolData;

		private Dictionary<string, Pool> m_pools = new Dictionary<string, Pool>();

		public void CreatePool(string poolName, GameObject prefab, Transform parent, int initialCapacity, int maxObjects, Pool.CreateGameObjectDelegate func = null)
		{
			Pool pool = new Pool(prefab, parent, initialCapacity, maxObjects, func);
			m_pools[poolName] = pool;
		}

		public GameObject GetGameObject(string poolName)
		{
			Pool pool;
			if (m_pools.TryGetValue(poolName, out pool))
			{
				return pool.GetGameObject();
			}
			return null;
		}

		public List<PoolObject> GetPoolObjects(string poolName)
		{
			Pool pool;
			if (m_pools.TryGetValue(poolName, out pool))
			{
				return pool.objects;
			}
			else
			{
				return null;
			}
		}

		void Start()
		{
			foreach (PoolData data in poolData)
			{
				CreatePool(data.name, data.prefab, data.parent, data.initialCapacity, data.maxObjects);
			}
		}

		public class Pool
		{
			public List<PoolObject> objects { get; private set; }
			public GameObject prefab { get; private set; }
			public Transform parent { get; private set; }
			public int maxObjects { get; private set; }
			public delegate GameObject CreateGameObjectDelegate(GameObject prefab);

			private CreateGameObjectDelegate m_createGameObjectDelegate = null;

			public Pool(GameObject prefab, Transform parent, int initialCapacity, int maxObjects, CreateGameObjectDelegate func = null)
			{
				this.prefab = prefab;
				this.parent = parent;
				this.maxObjects = maxObjects;
				this.objects = new List<PoolObject>(initialCapacity);
				m_createGameObjectDelegate = func;
				CreatePoolObjects(initialCapacity);
			}

			public GameObject GetGameObject()
			{
				PoolObject poolObject = null;

				int count = this.objects.Count;
				for (int i = 0; i < count; ++i)
				{
					if (this.objects[ i ].isAvailableForReuse)
					{
						poolObject = this.objects[ i ];
						break;
					}
				}

				if (poolObject == null && (count < this.maxObjects || this.maxObjects == -1))
				{
					CreatePoolObjects(1);

					int lastIndex = this.objects.Count - 1;
					if (this.objects[ lastIndex ].isAvailableForReuse)
					{
						poolObject = this.objects[ lastIndex ];
					}
				}

				if (poolObject != null)
				{
					poolObject.isAvailableForReuse = false;
					return poolObject.gameObject;
				}
				else
				{
					return null;
				}
			}

			private void CreatePoolObjects(int count)
			{
				for (int i = 0; i < count; ++i)
				{
					if (this.objects.Count == this.maxObjects)
					{
						break;
					}

					GameObject go = CreateGameObject();
					if (go != null)
					{
						PoolObject poolObject = go.GetComponent<PoolObject>();
						if (poolObject == null)
						{
							poolObject = go.AddComponent<PoolObject>();
						}

						if (poolObject != null)
						{
							if (this.parent != null)
							{
								poolObject.transform.SetParent(this.parent);
							}

							poolObject.isAvailableForReuse = true;
							this.objects.Add(poolObject);
						}
					}
				}
			}

			private GameObject CreateGameObject()
			{
				if (this.prefab != null)
				{
					if (m_createGameObjectDelegate != null)
					{
						return m_createGameObjectDelegate(this.prefab);
					}
					else
					{
						return Object.Instantiate(this.prefab) as GameObject;
					}
				}
				else
				{
					return null;
				}
			}
		}
	}
}
