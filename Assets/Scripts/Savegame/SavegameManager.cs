using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace game
{
	public class SavegameManager : MonoBehaviour
	{
		private Savegame m_savegame;

		private const string Filename = "savegame.json";
		private const string Key = "super.secret.key";

		public bool savegameLoaded
		{
			get { return m_savegame != null; }
		}

		void Awake()
		{
			CreateSavegame();
		}

		public void Load(GameSettings gameSettings)
		{
			string path = MakePath(Filename);
		#if UNITY_DEBUG
			Debug.Log("Loading savegame: " + path);
		#endif
			if (File.Exists(path) == false)
			{
		#if UNITY_DEBUG
				Debug.Log("Savegame does not exist. Resetting.");
		#endif
				ResetSavegame(gameSettings);
			}
			else
			{
				try
				{
					string content = File.ReadAllText(path);
					JsonUtility.FromJsonOverwrite(content, m_savegame);

					string fileHmac = m_savegame.hmac;
					string hmac = CalculateHMAC();

					if (fileHmac == hmac)
					{
						gameSettings.FromJson(m_savegame.data);
					}
					else
					{
		#if UNITY_DEBUG
						Debug.LogWarning("Savegame looks corrupted. Resetting.");
		#endif
						ResetSavegame(gameSettings);
					}
				}
				catch (Exception e)
				{
		#if UNITY_DEBUG
					Debug.LogWarning("Failed to load savegame. Resetting.\n" + e);
		#else
					Debug.LogWarning(e);
		#endif
					ResetSavegame(gameSettings);
				}
			}
		}

		public void Save(GameSettings gameSettings)
		{
			string path = MakePath(Filename);

			m_savegame.data = gameSettings.ToJson();
			m_savegame.hmac = CalculateHMAC();

			try
			{
				string contents = JsonUtility.ToJson(m_savegame);
				File.WriteAllText(path, contents);
		#if UNITY_DEBUG
				Debug.Log("Successfully saved savegame: " + path);
		#endif
			}
			catch (Exception e)
			{
		#if UNITY_DEBUG
				Debug.LogWarning(string.Format("An error occurred while trying to save the savegame: {0}\n{1}", path, e));
		#else
				Debug.LogWarning(e);
		#endif
			}
		}

		public void DeleteSavegame()
		{
			try
			{
				string path = MakePath(Filename);
				File.Delete(path);
		#if UNITY_DEBUG
				Debug.Log("Deleted savegame: " + path);
		#endif
			}
			catch (Exception e)
			{
				Debug.LogWarning(e);
			}
		}

		private string CalculateHMAC()
		{
			m_savegame.hmac = Savegame.DefaultHmac;
			string json = JsonUtility.ToJson(m_savegame);

			byte[] hashValue;
			using (HMACSHA256 hash = new HMACSHA256(Encoding.UTF8.GetBytes(Key)))
			{
				hashValue = hash.ComputeHash(Encoding.UTF8.GetBytes(json));
			}

			string hmac = string.Empty;
			int count = hashValue.Length;
			for (int i = 0; i < count; ++i)
			{
				hmac += string.Format("{0:X2}", hashValue[i]);
			}
			return hmac;
		}

		private void CreateSavegame()
		{
			m_savegame = ScriptableObject.CreateInstance<Savegame>();
			m_savegame.Initialize();
		}

		private void ResetSavegame(GameSettings gameSettings)
		{
			CreateSavegame();
			gameSettings.Reset();
			Save(gameSettings);
		}

		private string MakePath(string filename)
		{
			return Path.Combine(Application.persistentDataPath, filename);
		}
	}
}
