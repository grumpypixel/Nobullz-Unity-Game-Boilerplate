using System;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class PurchaseResultMessage : Message
	{
		public GameProduct product;
		public bool completed;

		public override void Reset()
		{
			product = GameProduct.None;
			completed = false;
		}
	}

	public class RequestProductsSucceededMessage : Message
	{
	}

	public class ProductManager : MonoBehaviour
	{
		[System.Serializable]
		public class ProductMapping
		{
			public GameProduct      productType;
			public string           productId;
			public string           fallbackTitle;
			public string           fallbackDescription;
		}

		public ProductMapping[]     androidProductMap;
		public ProductMapping[]     iOSProductMap;

		private ProductMapping[]    m_productMap;
		private IIABService         m_service;
		private List<string>        m_productIds;
		private DateTime            m_lastRequestTime;
		private bool                m_initialized = false;

		private const string        NotAvailable = "n/a";

	#if UNITY_EDITOR
		// Pass
	#elif UNITY_ANDROID
		private readonly string     LicenceKey = "insert.loooooong.license.key.here";
	#elif UNITY_IOS
		// TODO
	#endif

		public IABBillingState billingState
		{
			get { return m_service != null ? m_service.billingState : IABBillingState.BillingNotInitialized; }
		}

		public IABRequestState requestState
		{
			get { return m_service != null ? m_service.requestState : IABRequestState.Unknown; }
		}

		public IABPurchase[] purchases
		{
			get { return m_service != null ? m_service.purchases : null; }
		}

		public string lastRequestTime
		{
			get { return (this.requestState == IABRequestState.Succeeded ? m_lastRequestTime.ToString() : string.Empty); }
		}

		public bool initialized
		{
			get { return m_initialized; }
		}

		public void Initialize()
		{
			if (m_service != null)
			{
				return;
			}

		#if UNITY_EDITOR
			DummyBillingService service = new DummyBillingService();
			service.billingSupportedEvent += OnBillingSupportedEvent;
			service.requestSucceededEvent += OnRequestSucceededEvent;
			service.purchaseSucceededEvent += OnPurchaseSucceededEvent;
			m_service = service;
			m_service.Initialize(null);

		// #elif UNITY_ANDROID
		// 	GoogleBillingService service = new GoogleBillingService();
		// 	service.billingSupportedEvent += OnBillingSupportedEvent;
		// 	service.requestSucceededEvent += OnRequestSucceededEvent;
		// 	service.purchaseSucceededEvent += OnPurchaseSucceededEvent;
		// 	m_service = service;
		// 	m_service.Initialize(LicenceKey);

		// #elif UNITY_IOS
		// 	AppleBillingService service = new AppleBillingService();
		// 	service.billingSupportedEvent += OnBillingSupportedEvent;
		// 	service.requestSucceededEvent += OnRequestSucceededEvent;
		// 	service.purchaseSucceededEvent += OnPurchaseSucceededEvent;
		// 	m_service = service;
		// 	m_service.Initialize(null);

		#else
			m_service = new DummyBillingService();
			m_service.Initialize(null);
		#endif
			m_initialized = true;
		}

		public void ReInit()
		{
			if (m_service == null)
			{
				Initialize();
			}
			else
			{
				m_service.ReInit();
			}
		}

		public void PurchaseProduct(GameProduct productType)
		{
			if (m_service == null)
			{
				return;
			}

			string productId = GetProductId(productType);
			if (productId != null )
			{
				m_service.PurchaseProduct(productId);
			}
		}

		public void RestorePurchases()
		{
			if (m_service == null)
			{
				return;
			}

			if (m_service.billingState == IABBillingState.BillingNotSupported)
			{
				m_service.ReInit();
			}
			else
			{
		#if UNITY_ANDROID
				RequestProducts();
		#elif UNITY_IOS
				m_service.RestorePurchases();
		#endif
			}
		}

		public string GetProductTitle(GameProduct productType)
		{
			string productTitle = null;
			string productId = GetProductId(productType);
			if (productId != null)
			{
				IABProduct product = FindProduct(productId);
				if (product != null)
				{
					productTitle = product.title;
				}
			}

			if (productTitle == null)
			{
				productTitle = GetFallbackProductName(productType);
		#if UNITY_DEBUG
				productTitle += " *";
		#endif
			}
			return productTitle;
		}

		public string GetProductDescription(GameProduct productType)
		{
			string productDescription = null;
			string productId = GetProductId(productType);
			if (productId != null)
			{
				IABProduct product = FindProduct(productId);
				if (product != null)
				{
					productDescription = product.description;
				}
			}

			if (productDescription == null)
			{
				productDescription = GetFallbackProductDescription(productType);
		#if UNITY_DEBUG
				productDescription += " *";
		#endif
			}

			return (productDescription != null ? productDescription : string.Empty);
		}

		public string GetProductPrice(GameProduct productType)
		{
			string productPrice = null;
			string productId = GetProductId(productType);
			if (productId != null)
			{
				IABProduct product = FindProduct(productId);
				if (product != null)
				{
					productPrice = product.price;
				}
			}
		#if UNITY_DEBUG
			if (productPrice == null)
			{
				productPrice += "$1.99 *";
			}
		#endif
			return (productPrice != null ? productPrice : string.Empty);
		}


		public string[] GetProductIds()
		{
			return m_productIds.ToArray();
		}

		public GameProduct GetProductType(string productId)
		{
			foreach (ProductMapping mapping in m_productMap)
			{
				if (mapping.productId == productId)
				{
					return mapping.productType;
				}
			}
			return GameProduct.None;
		}

		private string GetProductId(GameProduct productType)
		{
			foreach (ProductMapping mapping in m_productMap)
			{
				if (mapping.productType == productType)
				{
					return mapping.productId;
				}
			}
			return null;
		}

		private string GetFallbackProductName(GameProduct productType)
		{
			foreach (ProductMapping mapping in m_productMap)
			{
				if (mapping.productType == productType)
				{
					return mapping.fallbackTitle;
				}
			}
			return NotAvailable;
		}

		private string GetFallbackProductDescription(GameProduct productType)
		{
			foreach (ProductMapping mapping in m_productMap)
			{
				if (mapping.productType == productType)
				{
					return mapping.fallbackDescription;
				}
			}
			return NotAvailable;
		}

		private void RequestProducts()
		{
			if (m_service == null)
			{
				return;
			}
			if (m_service.billingState == IABBillingState.BillingNotSupported)
			{
				m_service.ReInit();
			}
			else if (m_service.requestState != IABRequestState.Requesting)
			{
				m_service.RequestProducts(GetProductIds());
			}
		}

		private IABProduct FindProduct(string productId)
		{
			if (m_service != null)
			{
				IABProduct[] products = m_service.products;
				if (products != null)
				{
					foreach (IABProduct product in products)
					{
						if (product.productId == productId)
						{
							return product;
						}
					}
				}
			}
			return null;
		}

		private IABPurchase FindPurchase(string productId)
		{
			IABPurchase[] purchases = m_service.purchases;
			if (purchases != null)
			{
				foreach (IABPurchase purchase in purchases)
				{
					if (purchase.productId == productId)
					{
						return purchase;
					}
				}
			}
			return null;
		}

		private void OnBillingSupportedEvent()
		{
		#if UNITY_EDITOR
			// pass
		#else
			RequestProducts();
		#endif
		}

		private void OnRequestSucceededEvent()
		{
			GameContext.messageDispatcher.AddMessage<RequestProductsSucceededMessage>();
			m_lastRequestTime = DateTime.Now;
		}

		private void OnPurchaseSucceededEvent(string productId, IABTransactionState transactionState)
		{
			PurchaseResultMessage message = GameContext.messageDispatcher.AddMessage<PurchaseResultMessage>();
			message.product = GetProductType(productId);
			message.completed = transactionState == IABTransactionState.Purchased;
		}

		void Awake()
		{
		#if UNITY_ANDROID
			m_productMap = androidProductMap;
		#elif UNITY_IOS
			m_productMap = iOSProductMap;
		#else
			m_productMap = null;
		#endif
			m_productIds = new List<string>(m_productMap.Length);
			foreach (ProductMapping mapping in m_productMap)
			{
				m_productIds.Add(mapping.productId);
			}
		}

		void Destroy()
		{
			if (m_service != null)
			{
				m_service.Shutdown();
			}
		}
	}
}
