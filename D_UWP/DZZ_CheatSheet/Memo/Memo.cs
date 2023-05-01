using System.Collections.Generic;

using Windows.Security.Credentials;

namespace Aki32Utilities.UWPAppUtilities.CheatSheet
{
    internal class Memo
    {
        // TODO 整理

        #region ★★★★★★★★★★★★★★★ 重要！！

        // UWPリリース時やること
        //
        // ★ﾏﾆﾌｪｽﾄの中の自分の名前とか証明書を，全部書き換える！
        //
        // Visual Studio閉じた状態で，「～～～.users」開く
        //
        // <Property Group> の中に以下を加える。
        //
        // <UapAppxPackageBuildMode>StoreUpload</UapAppxPackageBuildMode>

        #endregion

        #region ★★★★★★★★★★★★★★★ 試す！
        #region ★★★★★ APIキーなどをロッカーに保存

        string CredentialsName = "testing";

        private PasswordCredential GetCredentialFromLocker(string username)
        {
            PasswordCredential credential = null;

            var vault = new PasswordVault();
            IReadOnlyList<PasswordCredential> credentialList = null;
            try
            {
                credentialList = vault.FindAllByUserName(username);
            }
            catch
            {
                return credential;
            }
            if (credentialList.Count > 0)
            {
                credential = credentialList[0];
            }

            return credential;
        }

        public void CreatePassword(string password, string username)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(CredentialsName, username, password));
        }

        #endregion
        #endregion

        #region ★★★★★★★★★★★★★★★ 無駄に時間食った
        #region ★★★★★ ListBoxのEnter入力を検知する

        // ★ KeyDownとかじゃなくてPewviweKeyDown使う！！！！！

        //ListBox1.PreviewKeyDown += ListBox_PreviewKeyDown;
        //private void ListBox_PreviewKeyDown(object sender, KeyRoutedEventArgs args)
        //    { if (args.Key == VirtualKey.Enter) { } }

        #endregion
        #region ★★★★★ コントロールでタップが止まる！（特にGrid）

        // ★ 背景を透過する！

        //var bullet_grid = new Grid()
        //{
        //    Background = new SolidColorBrush(Colors.Transparent),

        //};

        #endregion
        #endregion

        #region ★★★★★★★★★★★★★★★ NuGetおすすめ

        // ★★★★★ 基本的な基本的なUIコントロール（UNOとかUWPのUIに必須！！！）
        // Nuget : Windows.UI.Xaml
        #region 使用例

        //★App.xamlに追加！（xmlnsっていう名称はいろんなライブラリで共有できるらしい）
        //<Application.Resources>
        //   <ResourceDictionary>
        //      <ResourceDictionary.MergedDictionaries>
        //         <XamlControlsResources xmlns = "using:Microsoft.UI.Xaml.Controls" />
        //         < XamlControlsResources xmlns= "using:Microsoft.UI.Xaml.Controls.Primitives" />
        //         < !--Other merged dictionaries here -->
        //      </ResourceDictionary.MergedDictionaries>
        //      <!-- Other app resources here -->
        //   </ResourceDictionary>
        //</Application.Resources>


        //★Pageに追加！（好きに略称決めてOK）

        //xmlns:muxc= "using:Microsoft.UI.Xaml.Controls"


        //★C#に追加

        //using muxc = Microsoft.UI.Xaml.Controls;


        //★XAML 一例

        //<muxc:ProgressBar Width = "130" IsIndeterminate="True" ShowPaused="False" ShowError="False" />


        //★使える発展コントロール一覧！

        //https://platform.uno/docs/articles/implemented-views.html

        #endregion


        // ★★★★★ 描画したい際にいる！（UWP？）
        // Nuget : System.Drawing.Common


        // ★★★★★ Graphics（UWPで描画用のCanvas使いたい時！）
        // Nuget : Win2D
        #region 使用例
        //★Pageに追加。

        //xmlns:canvas="using:Microsoft.Graphics.Canvas.UI.Xaml"

        //★XAML
        //<canvas:CanvasControl Draw="CanvasControl_Draw" ClearColor="White" />

        //★C#に追加

        //using muxc = Microsoft.UI.Xaml.Controls;

        //★C#で描画

        //private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        //{
        //     CanvasDrawingSession d = args.DrawingSession;
        //     d.FillRectangle(250, 250, 700, 400,Windows.UI. Color.FromArgb(50, 100, 50, 250));
        //     d.DrawEllipse(450, 450, 200, 200, Windows.UI.Color.FromArgb(100, 100, 100, 255));
        //     d.FillEllipse(750, 450, 200, 200, Windows.UI.Color.FromArgb(100, 100, 100, 255));
        //     CanvasTextFormat f = new CanvasTextFormat();
        //     f.FontSize = 30;
        //     d.DrawText("Hello World!", 370, 430, Windows.UI.Color.FromArgb(200, 255, 255, 255), f);
        //}

        #endregion


        // ★★★★★ Microsoft Toolkit（UWPのみ）
        // Nuget : 
        // Microsoft.Toolkit.Uwp.UI.Controls
        // Microsoft.Toolkit.Uwp.UI.Controls.DataGrid
        #region 使用例

        //★参照
        //xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"
        //using Microsoft.Toolkit.Uwp.UI.Controls;

        #endregion


        #endregion


        #region ★★★★★★★★★★★★★★★ C#,XAML混合
        #region ★★★★★ MainPageのClose検知


        //★ﾏﾆﾌｪｽﾄ

        //xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
        //...
        //<Capabilities> 
        //  <Capability Name="internetClient" /> 
        //  <rescap:Capability Name="confirmAppClose"/> 
        //</Capabilities> 


        //★C#

        //public MainPage()
        //{
        //    this.InitializeComponent();
        //    SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnMainPageCloseRequest;
        //}

        //private void OnMainPageCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        //{
        //    var deferral = e.GetDeferral();

        //    if (!saved) 
        //    {
        //        e.Handled = true; //これをすると閉じなくなる 
        //        SomePromptFunction(); 
        //    }

        //    deferral.Complete();
        //}

        #endregion
        #region ★★★★★ ComboBoxの選択肢のバインディング

        //★C#
        //public IssueType[] IssueTypeList => new[]
        //        {
        //            IssueType.Bug,
        //            IssueType.Feature,
        //            IssueType.Issue,
        //            IssueType.Task
        //        };

        //★XAML
        //<ComboBox x:Name="IssueTypeBox"
        //     ItemsSource="{x:Bind IssueTypeList}"
        //     SelectedItem="{x:Bind Item.Type,Mode=TwoWay}"
        //     SelectionChanged="IssueType_SelectionChanged"
        //     PlaceholderText="Enter the Issue Type"
        //     HorizontalAlignment="Stretch"
        //     Margin="10,0,0,0"/>

        #endregion
        #endregion

        #region ★★★★★★★★★★★★★★★ C#

        #region ★★★★★ Grid追加

        //★Grid自体定義
        //var newgrid = new Grid()
        //{
        //  ColumnDefinitions =
        //  {
        //    new ColumnDefinition(){Width=new GridLength(100, GridUnitType.Pixel) },
        //    new ColumnDefinition(){Width=new GridLength(1, GridUnitType.Star) },
        //  }
        //};

        //★Gridに要素追加
        //var XXX = new TextBlock();
        //Grid.SetColumn(XXX, 0);
        //newgrid.Children.Add(XXX);

        #endregion
        #region ★★★★★ 初期化時！

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //   //内容。例えば↓で，Navigate元からのデータを受け取ってDataContextの中に収めておける。
        //   //this.DataContext = e.Parameter;
        //   base.OnNavigatedTo(e);
        //}

        #endregion
        #region ★★★★★ ページ遷移関係

        //★ページ送り
        //this.Frame.Navigate(typeof(SecondPage), "伝達内容");

        //★ページ戻し
        //this.Frame.GoBack();

        //★もしキャッシュを残したいなら…，コンストラクタにこれ入れる。
        //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

        //★遷移元からのデータを受け取る

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
        //    {
        //	//内容
        //    }
        //    base.OnNavigatedTo(e);
        //}


        #endregion
        #region ★★★★★ 既にあるデータの保存

        //StorageFile currentImage = await StorageFile.GetFileFromPathAsync(LocalData.output_zip);
        //var savePicker = new FileSavePicker();
        //savePicker.FileTypeChoices.Add("SolHubデータパッケージ", new List<string>() { ".solhub.pkg" });
        //savePicker.SuggestedSaveFile = currentImage;
        //savePicker.SuggestedFileName = currentImage.Name;
        //await savePicker.PickSaveFileAsync();

        //if (file != null)
        //  await file.CopyAndReplaceAsync(localfile); 

        #endregion
        #region ★★★★★ データの保存

        //var picker = new FileSavePicker();
        //picker.SuggestedStartLocation = PickerLocationId.Desktop;
        //picker.DefaultFileExtension = ".txt";
        //picker.FileTypeChoices.Add("テキスト", new List<string>() { ".txt" });
        //picker.SuggestedFileName = "新規メモ";
        //var file = await picker.PickSaveFileAsync();

        //if (file != null)
        //{
        //     try
        //     {
        //          await FileIO.WriteTextAsync(file, TextBox_memo.Text);

        //          ////ストリーム利用系
        //          //using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //          //    await InkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
        //     }
        //     catch (Exception ex)
        //     {
        //          await new MessageDialog(ex.Message, "エラー").ShowAsync();
        //     }
        //}


        #endregion
        #region ★★★★★ データ開く

        //var picker = new FileOpenPicker();
        //picker.SuggestedStartLocation = PickerLocationId.Desktop;
        //picker.FileTypeFilter.Add(".txt");
        //var file = await picker.PickSingleFileAsync();

        //if (file != null)
        //{
        //     try
        //     {
        //          var txt = await FileIO.ReadTextAsync(file);
        //          TextBox_memo.Text = txt;

        //          ////画像
        //          //var bmp = new BitmapImage();
        //          //using (var stream = await file.OpenReadAsync())
        //          //    bmp.SetSource(stream);

        //          ////ストリーム系
        //          //using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //          //    await InkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);

        //     }
        //     catch (Exception ex)
        //     {
        //          await new MessageDialog(ex.Message, "エラー").ShowAsync();
        //     }
        //}

        #endregion
        #region ★★★★★ MostRecentlyUsedList

        //★ディレクティブ

        //using Windows.Storage.AccessCache;

        //★データを追加

        //var mru = StorageApplicationPermissions.MostRecentlyUsedList;
        //mru.Add(file, file.Name);

        //★アクセス



        #endregion
        #region ★★★★★ クリップボード操作

        //★コピー

        //var data_pkg = new DataPackage();
        //data_pkg.RequestedOperation = DataPackageOperation.Copy;
        //data_pkg.SetText(TextBox_memo.SelectedText);
        //Clipboard.SetContent(data_pkg);

        //TextBox_memo.Focus(FocusState.Programmatic);

        #endregion
        #region ★★★★★ 単純なメッセージを表示

        //MessageDialog
        //await new MessageDialog(Label.Text, "お知らせ").ShowAsync();
        //await new MessageDialog($"データの出力に失敗しました。\r\nコード：{ex.Message}", "失敗").ShowAsync();


        //ContentDialog
        //await new ContentDialog()
        //{
        //    Title = "送信完了",
        //    Content = "送信が完了しました。",
        //    SecondaryButtonText = "OK"
        //}.ShowAsync();

        #endregion
        #region ★★★★★ 選択肢

        //var result = await new ContentDialog
        //{
        //     Title = "タイトル",
        //     Content = "どれを選びますか？",

        //     CloseButtonText = "閉", //安全のため必須
        //     PrimaryButtonText = "1",
        //     SecondaryButtonText = "2", //二択の場合，これを消す
        //}.ShowAsync();


        //switch (result)
        //{
        //     case ContentDialogResult.None:
        //     break;

        //     case ContentDialogResult.Primary:
        //     break;

        //     case ContentDialogResult.Secondary:
        //     break;

        //     default:
        //     break;
        //}

        #endregion
        #region ★★★★★ Jsonの復号

        //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Prefecture.json"));
        //var json = await FileIO.ReadTextAsync(file);

        //List<Prefecture> pref;

        //using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        //{
        //     var deserializer = new DataContractJsonSerializer(typeof(List<Prefecture>));
        //     pref = (List<Prefecture>)deserializer.ReadObject(stream);
        //}


        #endregion
        #region ★★★★★ ComboBoxにコードでデータ追加

        //ComboBox_Prefecture.ItemsSource = pref;
        //ComboBox_Prefecture.DisplayMemberPath = "Name";
        //ComboBox_Prefecture.SelectedValuePath = "Item";
        //ComboBox_Prefecture.SelectedIndex = 0;


        #endregion
        #region ★★★★★ DragDrop

        //CanDrag = true;
        //DragStarting += (sender, e) =>
        //{
        //    e.Data.SetText("a");
        //};

        //AllowDrop = true;
        //DragOver += async (sender, e) =>
        //{
        //    e.AcceptedOperation = DataPackageOperation.Move;
        //    e.DragUIOverride.Caption = $"Sol={  Solution.LocalSolutionId  } の内部に，このSolを移動します。";
        //    e.DragUIOverride.IsCaptionVisible = true; // Sets if the caption is visible
        //    e.DragUIOverride.IsContentVisible = true; // Sets if the dragged content is visible
        //    e.DragUIOverride.IsGlyphVisible = true; // Sets if the glyph is visibile
        //    e.Handled = true;
        //};

        //Drop += async (sender, e) =>
        //{
        //    if (e.DataView.Contains(StandardDataFormats.Text))
        //    {
        //        var item = await e.DataView.GetTextAsync();
        //    }
        //    e.Handled = true;
        //};


        #endregion
        #region ★★★★★ タイマー

        //★★★同期的（DispatcherTimer）（重い処理をしない）

        //★プロパティ

        //public DispatcherTimer timer { get; set; }


        //★初期化

        //timer = new DispatcherTimer();
        //timer.Interval = TimeSpan.FromMilliseconds(1);
        //timer.Tick += timer_Tick;


        //★イベントハンドラー

        //private async void timer_Tick(object sender, object e)     
        //{
        //     //重くない処理
        //}

        //★★★非同期的（ThreadingPoolTimer）

        //★using

        //using Windows.System.Threading;

        //★初期化

        //timer = ThreadPoolTimer.CreatePeriodicTimer(timer_Tick, TimeSpan.FromMilliseconds(1));

        //★プロパティ

        //public ThreadPoolTimer timer { get; set; }

        //★イベントハンドラー（非同期ながらコントロールをいじれる機能付き）

        //private async void timer_Tick(ThreadPoolTimer timer)
        //{
        //     bool IsBoost = BOOST.Checked;
        //     if (RadiationCoefficient != 1)
        //          picBox.Image = im = RandomInsert.Radiation(im, DotRadius, RadiationCoefficient, Transparent, IsBoost);
        //     else
        //                    picBox.Image = im = RandomInsert.Nomal(im, DotRadius, Transparent, IsBoost);
        //     // UIスレッド以外のスレッドから画面を更新する場合はDispatcher.RunAsyncを利用する     
        //     // 非同期処理なのでawait/asyncキーワードが必要になる
        //     await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //     {
        //          this.textBlock.Text = this._count.ToString();
        //     });
        //}

        #endregion
        #region ★★★★★ Web APIからJsonデータ取得



        #endregion
        #region ★★★★★ ネット上から文字列取得（例：Jsonデータ）

        //string json = await (new Windows.Web.Http.HttpClient()).GetStringAsync(new Uri("http://■■■"));

        //// JSONデータから必要なデータを取り出して、UIのデータコンテキストにセットする
        //rootGrid.DataContext = (new WeatherData(json)).Data;

        #endregion

        //URL系
        #region ★★★★★ データを保存
        // https://docs.microsoft.com/ja-jp/windows/uwp/launch-resume/suspend-an-app


        //Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);


        //async void App_Suspending(Object sender,Windows.ApplicationModel.SuspendingEventArgs e)
        //{
        //    // TODO: This is the time to save app data in case the process is terminated.
        //}

        #endregion
        #region ★★★★★ データをローカルに簡単にキープ
        // https://docs.microsoft.com/ja-jp/uwp/api/windows.storage.applicationdata.localsettings?view=winrt-20348

        //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        //// Create a simple setting.
        //localSettings.Values["exampleSetting"] = "Hello Windows";

        //// Read data from a simple setting.
        //Object value = localSettings.Values["exampleSetting"];

        //if (value == null)
        //{
        //// No data.
        //}
        //else
        //{
        //// Access data in value.
        //}

        //// Delete a simple setting.
        //localSettings.Values.Remove("exampleSetting");


        #endregion

        #endregion

        #region ★★★★★★★★★★★★★★★★ XAML側

        #region ★★★★★ 行列の定義

        //★行

        //<Grid.RowDefinitions>
        //     <RowDefinition  Height="auto"/>
        //     <RowDefinition  Height="*"/>
        //</Grid.RowDefinitions>

        //★列

        //<Grid.ColumnDefinitions>
        //     <ColumnDefinition  Width="auto"/>
        //     <ColumnDefinition  Width="*"/>
        //</Grid.ColumnDefinitions>


        #endregion
        #region ★★★★★ ComboBox

        //<ComboBox x:Name="ComboBox_PenStyle" Width="80" Margin="10,0,0,0">
        //     <ComboBoxItem Content="●"/>
        //     <ComboBoxItem Content="■"/>
        //</ComboBox>


        #endregion
        #region ★★★★★ アイコン付きボタン

        //<Button x:Name="Button_Save"  ToolTipService.ToolTip="保存" Margin="10,0,0,0">
        //     <FontIcon  FontFamily="Segoe MDL2 Assets" Glyph="&#xE105;"/> 
        //</Button>

        //★Glyphよく使うやつ

        //ハンバーガーE700，開くE197，保存E105，削除E107
        //消しゴムED60，

        //★C#で実現する際
        //new FontIcon(){ FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xF1CB" };


        #endregion
        #region ★★★★★ DataTemplate（バインディング多め）

        //     <GridView ItemsSource="{x:Bind Summary}">
        //          <GridView.ItemTemplate>
        //               <DataTemplate x:DataType="data:WeatherSummary">
        //                    <StackPanel>

        //                         <TextBlock Text="{Binding DateLabel}" Width="100" HorizontalAlignment="Center" VerticalAlignment="Center"/>     
        //                         <TextBlock Text="{Binding Date}" Width="100" HorizontalAlignment="Center" VerticalAlignment="Center" FontSize="10"/>
        //                              <Image Source="{Binding Url}" Width="50" Height="31"/>
        //                         <TextBlock Text="{Binding Telop}"  HorizontalAlignment="Center" VerticalAlignment="Center"/>

        //                    </StackPanel>
        //               </DataTemplate>
        //          </GridView.ItemTemplate>
        //     </GridView>

        #endregion
        #region ★★★★★ StackPanel例

        //<StackPanel VerticalAlignment="Center" 
        //         HorizontalAlignment="Stretch"
        //         Margin="10,20,30,40">
        //     <Label Content="文字が書ける"/>
        //     <TextBox Text="0000"/>
        //     <Button Content="ボタンです"/>
        //</StackPanel>


        #endregion
        #region ★★★★★ Flyout例

        //★AppBarButtonに普通のやつ

        //<AppBarButton x:Name="Button_Setting" Icon="Setting" Label="設定" Click="Button_Setting_Click">
        //     <AppBarButton.Flyout>
        //          <MenuFlyout>

        //               <MenuFlyoutItem Text="Share" >
        //                    <MenuFlyoutItem.Icon>
        //                         <FontIcon Glyph="&#xE72D;" />
        //                    </MenuFlyoutItem.Icon>
        //               </MenuFlyoutItem>
        //               <MenuFlyoutItem Text="Copy" Icon="Copy"></MenuFlyoutItem>
        //               <MenuFlyoutItem Text="Delete" Icon="Delete" />

        //               <MenuFlyoutSeparator />

        //               <MenuFlyoutItem Text="Rename" />
        //               <MenuFlyoutItem Text="Select" />

        //          </MenuFlyout>
        //     </AppBarButton.Flyout>
        //</AppBarButton>


        //★AppBarButtonから，書き込み可能なFlyout出すやつ

        //<AppBarButton Icon="Setting" Label="設定" AllowFocusOnInteraction="True">
        // <AppBarButton.Flyout>
        //  <Flyout>
        //   <StackPanel Width="300">
        //    <TextBlock>情報書き込み</TextBlock>
        //    <TextBox Name="SecretKey" Text="Test" />
        //   </StackPanel>
        //  </Flyout>
        // </AppBarButton.Flyout>
        //</AppBarButton>


        #endregion
        #region ★★★★★ リソースの追加（App全体に！）（スタイル名つけてる）

        //★App.xaml

        //<Application>
        //    <Application.Resources>
        //        <Style TargetType="Button">
        //            <Setter Property="Background" Value="DarkGray"/>
        //            <Setter Property="Foreground" Value="Black"/>
        //            <Setter Property="FontSize" Value="25"/>
        //            <Setter Property="Margin" Value="5"/>
        //        </Style>

        //        <Style TargetType="Button" x:Key="AButtonStyle">
        //            <Setter Property="Background" Value="Green"/>
        //            <Setter Property="Foreground" Value="White"/>
        //            <Setter Property="FontSize" Value="25"/>
        //            <Setter Property="Margin" Value="5"/>
        //        </Style>

        //        <Style TargetType="Button" x:Key="BButtonStyle" BasedOn="{StaticResource AButtonStyle}">
        //            <Setter Property="Background" Value="Red"/>
        //        </Style>
        //    </Application.Resources>
        //</Application>

        //★MainWindow.xaml

        //<Window x:Class="WPF008.MainWindow"
        //        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        //        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        //        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        //        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        //        xmlns:local="clr-namespace:WPF008"
        //        mc:Ignorable="d"
        //        Title="MainWindow" Height="500" Width="300">
        //    <Grid>
        //        <StackPanel>
        //            <Button Content="A1"
        //                    Style="{StaticResource AButtonStyle}"/>
        //            <Button Content="A2"
        //                    Style="{StaticResource AButtonStyle}"/>
        //            <Button Content="A3"
        //                    Style="{StaticResource AButtonStyle}"/>
        //            <Button Content="B1"
        //                     Style="{StaticResource BButtonStyle}"/>
        //            <Button Content="B2"
        //                    Style="{StaticResource BButtonStyle}"/>
        //            <Button Content="B3"/>
        //        </StackPanel>
        //    </Grid>
        //</Window>

        #endregion
        #region ★★★★★ リソースの追加（Pageのみに！）

        //<Page>
        //    <Page.Resources>
        //        <SolidColorBrush x:Key="BrandBrush" Color="#FFFFA500"/>
        //    </Page.Resources>

        //    <StackPanel>
        //        <TextBlock Foreground="{StaticResource BrandBrush}" />
        //        <Button Content="Submit" Foreground="{StaticResource BrandBrush}" />
        //    </StackPanel>
        //</Page>


        #endregion

        //URL系
        #region ★★★★★ StyleとかのリソースをApplicationのところに一括作成！
        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.resources?view=net-5.0
        #endregion

        #endregion

    }
}
