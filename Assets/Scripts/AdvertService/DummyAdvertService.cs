using UnityEngine;

#pragma warning disable 0414 // field ... is assigned but its value is never used

namespace game
{
	public class DummyAdvertService : IAdvertService
	{
		public delegate void InterstitialLoadedEvent();
		public delegate void InterstitialFailedEvent();

		public event InterstitialLoadedEvent interstitalLoadedEvent;
		public event InterstitialFailedEvent interstitalFailedEvent;

		public float bannerHeight
		{
			get { return 100; }
		}

		public DummyAdvertService()
		{
			interstitalLoadedEvent = null;
			interstitalFailedEvent = null;
		}

		public void SetTagForChildDirectedTreatment(bool tagEnabled)
		{
			// pass
		}

		public void ShowBanner()
		{
			// pass
		}

		public void HideBanner()
		{
			// pass
		}

		public void DestroyBanner()
		{
			// pass
		}

		public void LoadInterstitial()
		{
			// pass
		}

		public void ShowInterstitial()
		{
			// pass
		}

		public bool IsInterstitialReady()
		{
			return false;
		}
	}
}

 #pragma warning restore 0414
