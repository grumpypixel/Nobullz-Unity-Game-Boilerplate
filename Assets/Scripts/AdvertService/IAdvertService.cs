namespace game
{
	interface IAdvertService
	{
		float bannerHeight { get; }

		void SetTagForChildDirectedTreatment(bool tagEnabled);

		void ShowBanner();
		void HideBanner();
		void DestroyBanner();

		void LoadInterstitial();
		void ShowInterstitial();
		bool IsInterstitialReady();
	}
}
