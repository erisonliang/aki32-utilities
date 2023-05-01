

namespace Aki32Utilities.Uno.CheatSheet;
internal class Memo
{
    // TODO 整理

    #region ★★★★★★★★★★★★★★★いろいろ

    // ★★★★★ Lottie使いたい！
    // https://github.com/unoplatform/uno/blob/master/doc/articles/features/Lottie.md/

    //★★★★★注意
    //★絶対に「Windows.UI.Xaml」必要になる。「C#+XAML」のまとめ参照。

    //★★★★★ Json取り扱いたい（全プラットフォームに対応ならこれ！）
    //Newtonsoft.Json

    #region ★★★★★Androidの命名が長くなるの許可

    //★PC側
    //「グループポリシー」か「gerdit.msc」を検索して，
    // Computer Configuration > Administrative Templates > System > Filesystem > Enable NTFS long paths.

    //★manifestを追加
    //<application xmlns="urn:schemas-microsoft-com:asm.v3">
    //  <windowsSettings xmlns:ws2= "http://schemas.microsoft.com/SMI/2016/WindowsSettings" >
    //    < ws2:longPathAware>true</ws2:longPathAware>
    //  </windowsSettings>
    //</application>

    #endregion

    #endregion

    #region ★★★★★★★★★★★★★★★プラットフォームごとの書き分け方

    #region ★★★★★XAML側


    //★参考

    //https://platform.uno/docs/articles/platform-specific-xaml.html
    //https://www.xamarinhelp.com/entity-framework-core-xamarin-forms/


    //★定義
    //<Page x:Class="UF15.MainPage"
    //      ……省略……
    //      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"

    //      xmlns:win="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    //      xmlns:not_win="http:/uno.ui/not_win"
    //      xmlns:android="http://uno.ui/android"
    //      xmlns:ios="http://uno.ui/ios"
    //      xmlns:wasm="http://uno.ui/wasm"

    //      mc:Ignorable="d not_win android ios wasm">

    //★使い方
    //<StackPanel>
    //	<TextBlock Margin = "20" FontSize= "30" Text= "Hello, Uno !"

    //        win:Foreground= "DodgerBlue"

    //        android:Foreground= "LimeGreen"

    //        ios:Foreground= "BlueViolet"

    //        wasm:Foreground= "OrangeRed"
    //    />

    //    < win:AppBarButton Icon = "People" Label= "UWP" />

    //    < not_win:Button Margin = "20,0"

    //        android:Content= "Android"

    //        ios:Content= "iOS"

    //        wasm:Content= "WebAssembly" />
    //</ StackPanel >

    #endregion
    #region ★★★★★C#側（#ifを使用）

    //string Platform
    //#if NETFX_CORE
    //      = "UWP";
    //#elif __ANDROID__
    //      = "Android";
    //#elif __IOS__
    //      = "iOS";
    //#elif __WASM__
    //      = "WebAssembly";
    //#endif

    #endregion
    #region ★★★★★C#側（パーシャルクラスを使用）

    //★使用側
    //internal partial class SampleMessage
    //{
    //    internal static string GetString()
    //    => string.Format($"このプラットフォームは{PlatformName}です。");
    //}

    //★パーシャルクラス側（例：.iOS/SampleMessage.iOS.cs）
    //internal partial class SampleMessage
    //{
    //  const string PlatformName = "iOS";
    //}

    #endregion

    #region ★★★★★ダブルタップ

    //ダブルタップ

    //#if NETFX_CORE || __ANDROID__

    //                newimage.DoubleTapped += async (sender, e) =>
    //                {
    //                    await new ContentDialog()
    //                    {
    //                        Title = "あ",
    //                        Content = "画像ダブルタップ",
    //                        SecondaryButtonText = "OK"
    //                    }.ShowAsync();

    //                    MainPageRef.Frame.Navigate(typeof(ImagePreviewPage), newimage.Source);
    //                    e.Handled = true;
    //                };

    //#elif __IOS__

    //                var doubleTap = new UIKit.UITapGestureRecognizer(async (gesture) =>
    //                {
    //                    await new ContentDialog()
    //                    {
    //                        Title = "あ",
    //                        Content = "画像ダブルタップ",
    //                        SecondaryButtonText = "OK"
    //                    }.ShowAsync();

    //                    MainPageRef.Frame.Navigate(typeof(ImagePreviewPage), newimage.Source);
    //                    //e.Handled = true;
    //                })
    //                {
    //                    NumberOfTapsRequired = 2,
    //                };
    //                newimage.AddGestureRecognizer(doubleTap);

    //#endif

    #endregion

    #endregion

    #region ★★★★★★★★★★★★★★★データベース関係

    //★★★★★参考
    //https://codezine.jp/article/detail/11840?p=3

    //★★★★★Nuget
    //Microsoft.EntityFrameworkCore.Sqlite（Core3に対応してないので，ver. 2.0.1とする！！！！！）
    //Microsoft.EntityFrameworkCore.toolsも？
    //WebASMのプロジェクトには，以下も。
    //Uno.SQLitePCLRaw.provider.wasmとUno.sqlite-wasm


    //★★★★★初期追加コード

    //★WebASMでSQLiteを使う（.WasmプロジェクトのProgram.cs）
    //static void Main(string[] args)
    //{
    //  SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_WebAssembly());
    //  Windows.UI.Xaml.Application.Start(_ => _app = new App());
    //}

    //★iOSでSQLiteを使う（SQLiteSampleプロジェクトのArticleDatabase.cs内、InitForIOSメソッド）
    //SQLitePCL.Batteries.Init();


    //★★★★★接続文字列！
    //// 接続文字列（UWPとWebASM）
    //const string DbName = "uf16.db";
    //internal static string _connectionString = $"data source={DbName}";

    //// 接続文字列（Android）
    //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DbName);
    //_connectionString = $"filename={dbPath}";

    //// 接続文字列（iOS）
    //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DbName);
    //_connectionString = $"filename={dbPath}";

    #endregion



    #region ★★★★★★★★★★★★★★★ 共通取り扱い

    #region ★★★★★ マイクからの音声抽出

    // 音のbyte[]の取り方
    // 参考： https://stackoverflow.com/questions/66110930/how-do-i-record-audio-on-multiple-platforms-with-uno-platform


    // byte[]のwavファイルの操作方法。
    // 参考： https://101010.fun/programming/wav-sound-python.html

    // サンプリング周波数 fs = 44100[Hz]
    // ナイキスト周波数 = fs/2 = 22050[Hz]

    // 44byte目からが音データ


    #region ★サンプルを配列に変換

    //var recordedVoice = await GetPCMData();

    //async Task<List<short>> GetPCMData()
    //{
    //    byte[] bytedata = new byte[buffer.Size];
    //    DataReader dataReader = new DataReader(buffer.GetInputStreamAt(0));

    //    await dataReader.LoadAsync((uint)buffer.Size);
    //    dataReader.ReadBytes(bytedata);

    //    // 44byte目からが音データ。
    //    bytedata = bytedata.Skip(44).ToArray();

    //    //shortに直す。
    //    var shortdata = new List<short>();

    //    for (int i = 0; i < bytedata.Length; i += 2)
    //        shortdata.Add(BitConverter.ToInt16(bytedata, i));

    //    return shortdata;
    //}

    #endregion


    #endregion



    #endregion


    #region ★★★★★★★★★★★★★★★iOS取り扱い

    #region 実機デバッグが上手くいかない時

    // 解決策１， https://qiita.com/toshi0607/items/e05ff8c6ddb6ebc3af93

    // 解決策２， https://itblogdsi.blog.fc2.com/blog-entry-435.html
    //
    // <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|iPhone' ">
    // </PropertyGroup>
    //
    // の中の一番下に，
    //
    // <CodesignExtraArgs />
    // <CodesignResourceRules />

    #endregion

    #region 各種情報

    //証明書の場所！！！
    //C:\Users\zilli\AppData\Local\Xamarin\iOS\Provisioning\Certificates

    #endregion

    #region ★★★★★ノッチ付ける

    //参考： https://blog.okazuki.jp/entry/2019/12/26/235536

    //xmlns:android="http://uno.ui/android"
    //xmlns:ios="http://uno.ui/ios"
    //xmlns:wasm="http://uno.ui/wasm"
    //xmlns:win="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    //mc:Ignorable="d ios android wasm"

    //<Page.Resources>
    //    <android:Style x:Key="PageStyle" TargetType="Grid">
    //        <Setter Property = "toolkit:VisibleBoundsPadding.PaddingMask" Value="All" />
    //    </android:Style>
    //    <ios:Style x:Key="PageStyle" TargetType="Grid">
    //        <Setter Property = "toolkit:VisibleBoundsPadding.PaddingMask" Value="All" />
    //    </ios:Style>
    //    <win:Style x:Key="PageStyle" TargetType="Grid" />
    //    <wasm:Style x:Key="PageStyle" TargetType="Grid" />
    //</Page.Resources>

    //<Grid Style = "{StaticResource PageStyle}" >

    #endregion

    #region ★★★★★ 実機デバッグなど


    // 参考： https://qiita.com/KakeruFukuda/items/176c0aef5db0e04d6859
    // ポータル取り扱いはこの手順通り： https://zenn.dev/moutend/articles/feebf0120dce6e6426fa

    // ０，開く
    // https://developer.apple.com/account/resources/certificates/list

    // １，Bandle ID(=App ID)作る，
    // com.app-kobo.SolHubViewerX

    // ２，Mac の Xcode から，適当なアプリ作る。

    // ３，その新規アプリの「Xcode > Preference > Account」から Certificate 作る，
    // 最終的には App Distribution だけど，テスト段階では App Developement にする。

    // ４，Provisioning Profile 作る。
    // 名前は「 Develope Provisioning Profile for SolHubViewerX 」とかで良い。

    // ５，「uno.iOS」の「info.plist」とかをいい感じにいじる。

    // ６，実行。



    // ★ SKUは，適当でいい。
    // ComAppKoboSolHubViewerX








    #endregion

    #region ★★★★★ 課金関連



    #endregion

    #endregion

    #region ★★★★★★★★★★★★★★★Android取り扱い

    //証明書の場所！！！
    //C:\Users\zilli\AppData\Local\Xamarin\iOS\Provisioning\Certificates


    #region ★★★★★ 課金関連

    // Google Play Billing Library V3 の対応方法 : https://itblogdsi.blog.fc2.com/blog-entry-454.html

    // こっち使う！ : https://github.com/jamesmontemagno/InAppBillingPlugin
    // ドキュメンテーション : https://jamesmontemagno.github.io/InAppBillingPlugin/

    // リリースに向けての準備 : https://docs.microsoft.com/en-us/xamarin/android/deploy-test/release-prep/?tabs=windows
    //
    // 署名されたapkの取り出し方
    //
    // ０，パッケージ名は com. のやつ
    // １，Releaseする。
    // ２，アーカイブする。
    // ３，配布を押して，署名する。
    // ４，「com.app_kobo.SolHubViewer1-arm64-v8a.apk」が通るやつ。


    // キーストア : https://docs.microsoft.com/ja-jp/xamarin/android/deploy-test/signing/?tabs=windows

    // ★ MainActivity.cs にこれを追加！！！

    //protected async override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);
    //    Platform.Init(this, savedInstanceState);
    //}





    #endregion


    #endregion

    #region ★★★★★★★★★★★★★★★Android取り扱い


    // JavaScript を実行！
    // 参考： https://platform.uno/docs/articles/interop/wasm-javascript-1.html


    #endregion
}
