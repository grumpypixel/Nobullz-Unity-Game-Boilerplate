using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace game
{
	public class SavegameManager : MonoBehaviour
	{
		public string filename = "savegame.json";
		public string key = "super.secret.key";

		private Savegame m_savegame;

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
			string pathname = MakePath(this.filename);
		#if UNITY_DEBUG
			Debug.Log("Loading savegame: " + pathname);
		#endif

			if (File.Exists(pathname) == false)
			{
		#if UNITY_DEBUG
				Debug.Log("Savegame does not exist. Resetting.");
		#endif
				ResetSavegame(gameSettings);
				return;
			}

			// Load existing savegame:
			try
			{
				string content = File.ReadAllText(pathname);
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

		public void Save(GameSettings gameSettings)
		{
			string pathname = MakePath(this.filename);

			m_savegame.data = gameSettings.ToJson();
			m_savegame.hmac = CalculateHMAC();

			try
			{
				string contents = JsonUtility.ToJson(m_savegame);
				File.WriteAllText(pathname, contents);
		#if UNITY_DEBUG
				Debug.Log("Successfully saved savegame: " + pathname);
		#endif
			}
			catch (Exception e)
			{
		#if UNITY_DEBUG
				Debug.LogWarning(string.Format("An error occurred while trying to save the savegame: {0}\n{1}", pathname, e));
		#else
				Debug.LogWarning(e);
		#endif
			}
		}

		public void DeleteSavegame()
		{
			try
			{
				string pathname = MakePath(this.filename);
				File.Delete(pathname);
		#if UNITY_DEBUG
				Debug.Log("Deleted savegame: " + pathname);
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
			using (HMACSHA256 hash = new HMACSHA256(Encoding.UTF8.GetBytes(this.key)))
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
			Save(gameSettings);
		}

		private string MakePath(string filename)
		{
			return Path.Combine(Application.persistentDataPath, filename);
		}
	}
}
