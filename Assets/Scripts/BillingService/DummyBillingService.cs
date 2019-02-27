using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class DummyBillingService : IIABService
	{
		private List<IABProduct>    m_products;
		private List<IABPurchase>   m_purchases;

		public delegate void BillingSupportedEvent();
		public delegate void RequestSucceededEvent();
		public delegate void PurchaseSucceededEvent(string productId, IABTransactionState transactionState);

		public event BillingSupportedEvent  billingSupportedEvent;
		public event RequestSucceededEvent  requestSucceededEvent;
		public event PurchaseSucceededEvent purchaseSucceededEvent;

		public IABBillingState billingState { get; private set; }
		public IABRequestState requestState { get; private set; }
		public IABProduct[] products { get { return m_products.ToArray(); } }
		public IABPurchase[] purchases { get { return m_purchases.ToArray(); } }

		public DummyBillingService()
		{
			this.billingState = IABBillingState.BillingNotSupported;
			m_products = new List<IABProduct>();
			m_purchases = new List<IABPurchase>();
		}

		public void Initialize(string key)
		{
			this.billingState = IABBillingState.BillingSupported;
			if (this.billingSupportedEvent != null)
			{
				billingSupportedEvent();
			}
		}

		public void ReInit()
		{
			this.billingState = IABBillingState.BillingSupported;
			if (this.billingSupportedEvent != null)
			{
				billingSupportedEvent();
			}
		}

		public void Shutdown()
		{
			this.billingState = IABBillingState.BillingNotSupported;
		}

		public void RequestProducts(string[] productIds)
		{
#if UNITY_DEBUG
			Debug.Log("Request products for Dummy billings service.");
#endif
			this.requestState = IABRequestState.Succeeded;
			if (this.requestSucceededEvent != null)
			{
				// requestSucceededEvent();
			}
		}

		public void PurchaseProduct(string productId)
		{
			foreach (IABPurchase iabPurchase in m_purchases)
			{
				if (iabPurchase.productId == productId)
				{
					return;
				}
			}

			m_purchases.Add(new IABPurchase(productId));

			if (this.purchaseSucceededEvent != null)
			{
				purchaseSucceededEvent(productId, IABTransactionState.Purchased);
			}
		}

		public void RestorePurchases()
		{
			// TODO
		}
	}
}
