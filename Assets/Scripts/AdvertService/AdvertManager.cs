using UnityEngine;

namespace game
{
	public class AdvertManager : MonoBehaviour
	{
		public bool tagForChildDirectedTreatment = false;

		private IAdvertService m_service = null;

		void Start()
		{
		#if UNITY_EDITOR
			DummyAdvertService service = new DummyAdvertService();
			service.SetTagForChildDirectedTreatment(this.tagForChildDirectedTreatment);
			service.interstitalLoadedEvent += OnInterstitialLoaded;
			service.interstitalFailedEvent += OnInterstitialFailed;
			m_service = service;

		#elif UNITY_ANDROID && ENABLE_ADS
			// pass

		#elif UNITY_IOS && ENABLE_ADS
			// pass

		#else
			DummyAdvertService service = new DummyAdvertService();
			m_service = service;
		#endif
		}

		void OnDestroy()
		{
			m_service.DestroyBanner();
		}

		public void ShowBanner()
		{
			m_service.ShowBanner();
		}

		public void HideBanner()
		{
			m_service.HideBanner();
		}

		public void LoadInterstitial()
		{
			m_service.LoadInterstitial();
		}

		public void ShowInterstitial()
		{
			m_service.ShowInterstitial();
		}

		private void OnInterstitialLoaded()
		{
		}

		private void OnInterstitialFailed()
		{
		}
	}
}
