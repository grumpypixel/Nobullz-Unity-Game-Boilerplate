namespace game
{
	public enum IABBillingState
	{
		BillingNotInitialized,
		BillingNotSupported,
		BillingSupported
	}

	public enum IABRequestState
	{
		Unknown,
		NotRequested,
		Requesting,
		Succeeded,
		Failed
	}

	public enum IABTransactionState
	{
		Failed,
		Purchased,
		Restored
	}

	public class IABProduct
	{
		public string productId { get; set; }
		public string title { get; set; }
		public string price { get; set; }
		public string description { get; set; }
	}

	public class IABPurchase
	{
		public string productId { get; set; }

		public IABPurchase(string productId)
		{
			this.productId = productId;
		}
	}

	interface IIABService
	{
		IABBillingState billingState { get; }
		IABRequestState requestState { get; }
		IABProduct[] products { get; }
		IABPurchase[] purchases { get; }

		void Initialize(string key);
		void Shutdown();
		void ReInit();
		void RequestProducts(string[] productIds);
		void PurchaseProduct(string productId);
		void RestorePurchases();
	}
}
