using System;
using System.Collections.Generic;
using System.Linq;
using Forms = AppName.Droid.Models.Forms;
using Xamarin.Forms.Internals;
using Android.App;
using Android.BillingClient.Api;

namespace Aki32Utilities.Uno.CheatSheet;
internal class Android課金
{
    // TODO 整理

    /// <summary>
    /// InBillingServiceV3
    /// Google Play Billing Library v3 対応
    /// </summary>
    public class InBillingServiceV3
    {
        private BillingClient _billingClient = null;
        private BillingClientStateListener _stateListener = null;
        private SkuDetailsResponseListener _skuDetailsListener = null;
        private PurchaseHistoryResponseListener _purchaseHistoryListener = null;
        private bool _connected = false;
        private string _productId = String.Empty;
        private string _itemType = String.Empty;

        /// <summary>
        /// アカウント情報（アプリ起動中に永続的に記憶させたいためstaticに設定）
        /// </summary>
        public static AccountIdentifiers AccountIdentifiers { get; set; }

        /// <summary>
        /// 購入結果をハンドリングするイベント
        /// </summary>
        public event OnPurchaseProductDelegate OnPurchaseProduct;
        public event OnUserCanceledDelegate OnUserCanceled;
        public event OnPurchaseProductErrorDelegate OnPurchaseProductError;

        public void SetConnected(bool isConnected)
        {
            _connected = isConnected;
        }

        //DependencyServiceから呼び出せるようにプロパティでなくメソッドで作成しています。
        public bool IsConnected()
        {
            if (_billingClient == null)
            {
                return false;
            }
            return _billingClient.IsReady && _connected;
        }

        /// <summary>
        /// BillingClient を初期化する
        /// </summary>
        /// <param name="productId">Google Play ConsoleのアイテムID</param>
        /// <param name="itemType">BillingClient.SkuType.Inapp or BillingClient.SkuType.Subs</param>
        public void Initialize(string productId, string itemType)
        {
            _productId = productId;
            _itemType = itemType;

            Activity activity = Forms.MainActivity;

            var listener = new PurchasesUpdatedListener(this, _itemType);
            listener.OnUserCanceled += () =>
            {
                if (this.OnUserCanceled != null)
                {
                    this.OnUserCanceled();
                }
            };
            listener.OnPurchaseProductError += (responseCode, sku) =>
            {
                if (this.OnPurchaseProductError != null)
                {
                    this.OnPurchaseProductError(responseCode, sku);
                }
            };

            //Google Play Billing Library 
            _billingClient = BillingClient.NewBuilder(activity)
                                .EnablePendingPurchases()
                                .SetListener(listener)
                                .Build();
        }

        /// <summary>
        /// Google Play との接続を確立する
        /// </summary>
        public void StartConnection()
        {
            if (_billingClient == null)
            {
                if (!String.IsNullOrEmpty(_productId) &&
                    !String.IsNullOrEmpty(_itemType))
                {
                    //一度でも接続した場合、再初期化する
                    this.Initialize(_productId, _itemType);
                }
                else
                {
                    return;
                }
            }
            _stateListener = new BillingClientStateListener(this);
            _billingClient.StartConnection(_stateListener);
        }

        /// <summary>
        /// 購入可能なアイテムを表示する
        /// </summary>
        public void QuerySkuDetails()
        {
            if (!this.IsConnected())
            {
                return;
            }

            var skuList = new List<string>();
            //skuList.Add("premium_upgrade");
            //skuList.Add("gas");
            skuList.Add(_productId);
            SkuDetailsParams param = SkuDetailsParams.NewBuilder()
                                                .SetSkusList(skuList)
                                                .SetType(_itemType)
                                                .Build();
            _skuDetailsListener = new SkuDetailsResponseListener();
            _billingClient.QuerySkuDetails(param, _skuDetailsListener);
        }

        /// <summary>
        /// 購入フローを起動する
        /// </summary>
        public bool CanPurchase()
        {
            if (!this.IsConnected())
            {
                return false;
            }

            //var detail = new SkuDetails(_productId);
            var detail = _skuDetailsListener.SkuDetailsList.FirstOrDefault();
            return this.CanPurchase(detail);
        }

        /// <summary>
        /// 購入フローを起動する
        /// </summary>
        public bool CanPurchase(SkuDetails detail)
        {
            if (!this.IsConnected())
            {
                return false;
            }

            // An activity reference from which the billing flow will be launched.
            Activity activity = Forms.MainActivity;

            // Retrieve a value for "skuDetails" by calling querySkuDetailsAsync().
            BillingFlowParams.Builder builder = BillingFlowParams.NewBuilder()
                                                    .SetSkuDetails(detail);
            if (InBillingServiceV3.AccountIdentifiers != null)
            {
                //不正行為が行われる前に Google が検出できるようにする
                builder = builder.SetObfuscatedAccountId(InBillingServiceV3.AccountIdentifiers.ObfuscatedAccountId)
                                 .SetObfuscatedProfileId(InBillingServiceV3.AccountIdentifiers.ObfuscatedProfileId);
                System.Console.WriteLine(" ** CanPurchase Set AccountIdentifiers.");
            }
            BillingFlowParams billingFlowParams = builder.Build();
            BillingResult result = _billingClient.LaunchBillingFlow(activity, billingFlowParams);
            BillingResponseCode responseCode = result.ResponseCode;

            // Handle the result.
            return responseCode == BillingResponseCode.Ok;
        }

        /// <summary>
        /// SKU商品を消費する
        /// </summary>
        /// <param name="purchaseToken"></param>
        public void Consume(string purchaseToken)
        {
            if (!this.IsConnected())
            {
                return;
            }

            ConsumeParams consumeParams =
                        ConsumeParams.NewBuilder()
                            .SetPurchaseToken(purchaseToken)
                            .Build();

            ConsumeResponseListener listener = new ConsumeResponseListener();
            listener.OnPurchaseProduct += () =>
            {
                if (this.OnPurchaseProduct != null)
                {
                    this.OnPurchaseProduct();
                }
            };

            _billingClient.Consume(consumeParams, listener);
        }

        /// <summary>
        /// 定期購読を消費する
        /// </summary>
        /// <param name="purchaseToken"></param>
        public void AcknowledgePurchase(string purchaseToken)
        {
            if (!this.IsConnected())
            {
                return;
            }

            AcknowledgePurchaseParams acknowledgeParams =
                        AcknowledgePurchaseParams.NewBuilder()
                            .SetPurchaseToken(purchaseToken)
                            .Build();

            AcknowledgePurchaseResponseListener listener = new AcknowledgePurchaseResponseListener();
            listener.OnPurchaseProduct += () =>
            {
                if (this.OnPurchaseProduct != null)
                {
                    this.OnPurchaseProduct();
                }
            };

            _billingClient.AcknowledgePurchase(acknowledgeParams, listener);
        }

        /// <summary>
        /// 購入結果の確認クエリを投げる
        /// </summary>
        public Purchase.PurchasesResult GetQueryPurchases(string itemType)
        {
            if (!this.IsConnected())
            {
                return null;
            }

            // https://developer.android.com/reference/com/android/billingclient/api/BillingClient
            // queryPurchases(String skuType)
            return _billingClient.QueryPurchases(itemType);
        }

        /// <summary>
        /// 購入履歴の確認クエリを投げる
        /// </summary>
        public void QueryPurchaseHistory()
        {
            if (!this.IsConnected())
            {
                return;
            }

            _purchaseHistoryListener = new PurchaseHistoryResponseListener();
            _billingClient.QueryPurchaseHistory(_itemType, _purchaseHistoryListener);
        }

        /// <summary>
        /// 購入履歴を取得する
        /// </summary>
        /// <returns>購入履歴のリスト</returns>
        public IList<Android.BillingClient.Api.PurchaseHistoryRecord> GetPurchaseHistoryRecords()
        {
            if (!this.IsConnected())
            {
                return null;
            }
            return _purchaseHistoryListener.PurchaseHistoryList;
        }

        /// <summary>
        /// Google Play との接続を切断する
        /// </summary>
        public void Disconnect()
        {
            if (this.IsConnected() &&
                _billingClient != null)
            {
                _billingClient.EndConnection();
                _billingClient.Dispose();
                _billingClient = null;    //インスタンスの初期化時の判断で必要です。
            }
        }
    }

    /// <summary>
    /// Google Play との接続状態を管理する
    /// </summary>
    public class BillingClientStateListener : Java.Lang.Object, IBillingClientStateListener
    {
        private InBillingServiceV3 _inBillingService = null;

        public BillingClientStateListener()
        {
        }

        public BillingClientStateListener(InBillingServiceV3 instance)
        {
            _inBillingService = instance;
        }

        public void OnBillingSetupFinished(Android.BillingClient.Api.BillingResult result)
        {
            System.Console.WriteLine("OnBillingSetupFinished ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok)
            {
                // The BillingClient is ready. You can query purchases here.
                if (_inBillingService != null)
                {
                    _inBillingService.SetConnected(true);
                    _inBillingService.QuerySkuDetails();        //購入商品リストの確認
                    _inBillingService.QueryPurchaseHistory();   //購入履歴の確認
                }
            }
        }
        public void OnBillingServiceDisconnected()
        {
            // Try to restart the connection on the next request to
            // Google Play by calling the startConnection() method.
            System.Console.WriteLine("OnBillingServiceDisconnected");
            if (_inBillingService != null)
            {
                _inBillingService.SetConnected(false);
            }
        }
    }

    /// <summary>
    /// 購入可能なアイテムの結果を取得する
    /// </summary>
    public class SkuDetailsResponseListener : Java.Lang.Object, ISkuDetailsResponseListener
    {
        public IList<SkuDetails> SkuDetailsList = null;

        public void OnSkuDetailsResponse(BillingResult result, IList<SkuDetails> list)
        {
            System.Console.WriteLine("OnSkuDetailsResponse ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok)
            {
                list.ForEach(r =>
                {
                    System.Console.WriteLine("OnSkuDetailsResponse SkuDetails : " + r);
                });
                this.SkuDetailsList = list;
            }
        }
    }

    /// <summary>
    /// 購入結果の確認
    /// </summary>
    public class PurchasesUpdatedListener : Java.Lang.Object, IPurchasesUpdatedListener
    {
        private InBillingServiceV3 _instance = null;
        private string _itemType = String.Empty;
        public event OnUserCanceledDelegate OnUserCanceled;
        public event OnPurchaseProductErrorDelegate OnPurchaseProductError;

        public PurchasesUpdatedListener(InBillingServiceV3 instance, string itemType)
        {
            _instance = instance;
            _itemType = itemType;
        }

        public void OnPurchasesUpdated(Android.BillingClient.Api.BillingResult result, IList<Android.BillingClient.Api.Purchase> list)
        {
            System.Console.WriteLine("OnPurchasesUpdated ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok &&
                list != null)
            {
                list.ForEach(r =>
                {
                    System.Console.WriteLine("OnPurchasesUpdated Purchase : " + r.OriginalJson);
                    InBillingServiceV3.AccountIdentifiers = r.AccountIdentifiers; //不正防止
                    this.HandlePurchase(r);
                });
            }
            else if (result.ResponseCode == BillingResponseCode.UserCancelled)
            {
                // Handle an error caused by a user cancelling the purchase flow.
                System.Console.WriteLine("OnPurchasesUpdated UserCancelled");
                if (this.OnUserCanceled != null)
                {
                    this.OnUserCanceled();
                }
            }
            else
            {
                if (this.OnPurchaseProductError != null)
                {
                    this.OnPurchaseProductError((int)result.ResponseCode, "");
                }
                // Handle any other error codes.
                System.Console.WriteLine("OnPurchasesUpdated Fail");
            }
        }

        /// <summary>
        /// 購入を処理する
        /// </summary>
        /// <param name="purchase"></param>
        public void HandlePurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                System.Console.WriteLine("HandlePurchase purchase is null.");
                return;
            }
            if (_instance == null)
            {
                System.Console.WriteLine("HandlePurchase _instance is null.");
                return;
            }

            // Purchase retrieved from BillingClient#queryPurchases or your PurchasesUpdatedListener.
            //Purchase purchase = ...;

            // Verify the purchase.
            // Ensure entitlement was not already granted for this purchaseToken.
            // Grant entitlement to the user.

            if (_itemType == BillingClient.SkuType.Subs)
            {
                _instance.AcknowledgePurchase(purchase.PurchaseToken);
            }
            else
            {
                _instance.Consume(purchase.PurchaseToken);
            }

            System.Console.WriteLine("HandlePurchase Consume Success.");
        }
    }

    /// <summary>
    /// 購入の処理結果を取得する
    /// </summary>
    public class ConsumeResponseListener : Java.Lang.Object, IConsumeResponseListener
    {
        public event OnPurchaseProductDelegate OnPurchaseProduct;
        public event OnPurchaseProductErrorDelegate OnPurchaseProductError;

        public void OnConsumeResponse(BillingResult result, string purchaseToken)
        {
            System.Console.WriteLine("OnConsumeResponse ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok)
            {
                // Handle the success of the consume operation.
                System.Console.WriteLine("OnConsumeResponse purchaseToken : " + purchaseToken);
                if (this.OnPurchaseProduct != null)
                {
                    this.OnPurchaseProduct();
                }
            }
            else
            {
                if (this.OnPurchaseProductError != null)
                {
                    this.OnPurchaseProductError((int)result.ResponseCode, "");
                }
            }
        }
    }

    /// <summary>
    /// 定期購読の処理結果を取得する
    /// </summary>
    public class AcknowledgePurchaseResponseListener : Java.Lang.Object, IAcknowledgePurchaseResponseListener
    {
        public event OnPurchaseProductDelegate OnPurchaseProduct;
        public event OnPurchaseProductErrorDelegate OnPurchaseProductError;

        public void OnAcknowledgePurchaseResponse(BillingResult result)
        {
            System.Console.WriteLine("OnAcknowledgePurchaseResponse ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok)
            {
                // Handle the success of the consume operation.
                if (this.OnPurchaseProduct != null)
                {
                    this.OnPurchaseProduct();
                }
            }
            else
            {
                if (this.OnPurchaseProductError != null)
                {
                    this.OnPurchaseProductError((int)result.ResponseCode, "");
                }
            }
        }
    }

    /// <summary>
    /// 購入履歴を取得する
    /// </summary>
    public class PurchaseHistoryResponseListener : Java.Lang.Object, IPurchaseHistoryResponseListener
    {
        public IList<PurchaseHistoryRecord> PurchaseHistoryList = null;

        public void OnPurchaseHistoryResponse(BillingResult result, IList<PurchaseHistoryRecord> list)
        {
            System.Console.WriteLine("OnPurchaseHistoryResponse ResponseCode : " + result.ResponseCode.ToString());
            if (result.ResponseCode == BillingResponseCode.Ok &&
                list != null)
            {
                list.ForEach(r =>
                {
                    System.Console.WriteLine("OnPurchaseHistoryResponse SkuDetails : " + r.OriginalJson);
                });
                this.PurchaseHistoryList = list;
            }
        }
    }
}
