using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.UI.Popups;
using Microsoft.Identity.Client;
using System.Net.Http;
using Windows.UI.Xaml;
using System.Net.Http.Headers;
using Aki32Utilities.Azure.B2CAuth.FromApps.LogOn;

namespace Aki32Utilities.Azure.B2CAuth.FromApps
{
    public sealed partial class MainPage : Page
    {
        private bool LoggedIn = false;
        private string DisplayName;

        async void Button_LogOn_InOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedIn)
                {
                    var userContext = await B2CAuthenticationService.Instance.SignOutAsync();
                    UpdateSignInState(userContext);
                    PostInfo(InfoIcons.LogOn, $"ログアウト");
                }
                else
                {
                    var userContext = await B2CAuthenticationService.Instance.SignInAsync();
                    UpdateSignInState(userContext);
                    PostInfo(InfoIcons.LogOn, $"{DisplayName}としてログイン");
                }
            }
            catch (Exception ex)
            {
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                    OnPasswordReset();
                // Alert if any exception excluding user canceling sign-in dialog
                else if ((ex as MsalException)?.ErrorCode != "authentication_canceled")
                    await new MessageDialog("ログインに失敗しました。", "失敗").ShowAsync();

            }
        }

        async Task OnCallApi(object sender, EventArgs e)
        {
            try
            {
                PostInfo(InfoIcons.LogOn, $"API Endpoint：{B2CConstants.ApiEndpoint}");

                var userContext = await B2CAuthenticationService.Instance.SignInSilentAsync();

                if (userContext == null)
                    throw new MsalUiRequiredException("", "");

                UpdateSignInState(userContext);

                var token = userContext.AccessToken;//AccessToken
                PostInfo(InfoIcons.LogOn, $"Access Token：{token}");

                //Get data from API
                using (HttpClient client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Get, B2CConstants.ApiEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    using (var response = await client.SendAsync(request))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        //成功
                        if (response.IsSuccessStatusCode)
                            PostInfo(InfoIcons.LogOn, $"成功：{responseString}");

                        //失敗
                        else
                            PostInfo(InfoIcons.LogOn, $"失敗：{response.StatusCode.GetHashCode()} - {response.ReasonPhrase} - {responseString}");

                    }

                }


            }
            catch (MsalUiRequiredException ex)
            {
                await new MessageDialog("ログインセッションが切れています。改めてログインしてから再度お試しください。", "セッション切れ").ShowAsync();
            }
            catch (Exception ex)
            {
                PostInfo(InfoIcons.LogOn, $"大失敗：{ex.Message}");
            }
        }

        async void Button_LogOn_EditInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userContext = await B2CAuthenticationService.Instance.EditProfileAsync();
                UpdateSignInState(userContext);
                PostInfo(InfoIcons.LogOn, $"登録情報を編集");
            }
            catch (Exception ex)
            {
                // Alert if any exception excluding user canceling sign-in dialog
                if ((ex as MsalException)?.ErrorCode != "authentication_canceled")
                    await new MessageDialog("ログインに失敗しました。", "失敗").ShowAsync();

            }
        }

        async void OnResetPassword(object sender, EventArgs e)
        {
            try
            {
                var userContext = await B2CAuthenticationService.Instance.ResetPasswordAsync();
                UpdateSignInState(userContext);
                PostInfo(InfoIcons.LogOn, $"パスワードをリセット");
            }
            catch (Exception ex)
            {
                // Alert if any exception excluding user canceling sign-in dialog
                if ((ex as MsalException)?.ErrorCode != "authentication_canceled")
                    await new MessageDialog("ログインに失敗しました。", "失敗").ShowAsync();

            }
        }

        async void OnPasswordReset()
        {
            try
            {
                var userContext = await B2CAuthenticationService.Instance.ResetPasswordAsync();
                UpdateSignInState(userContext);
                PostInfo(InfoIcons.LogOn, $"パスワードをリセット");
            }
            catch (Exception ex)
            {
                // Alert if any exception excluding user canceling sign-in dialog
                if ((ex as MsalException)?.ErrorCode != "authentication_canceled")
                    await new MessageDialog("ログインに失敗しました。", "失敗").ShowAsync();

            }
        }

        async Task TrySignInSilentAsync()
        {
            if (LoggedIn)
                return;

            try
            {
                var userContext = await B2CAuthenticationService.Instance.SignInSilentAsync();

                if (userContext == null)
                {
                    PostInfo(InfoIcons.LogOn, $"ログインしていません");
                    return;
                }

                UpdateSignInState(userContext);
                PostInfo(InfoIcons.LogOn, $"{DisplayName}としてログイン");
            }
            catch (Exception ex)
            {
                PostInfo(InfoIcons.LogOn, $"ログインしていません");
            }
        }

        private void UpdateSignInState(UserContext userContext)
        {
            LoggedIn = userContext.IsLoggedOn;
            DisplayName = userContext.Name;

            //TextBlock_LogOn_InOutState.Text = LoggedIn ? $"{DisplayName}としてログインしています。" : "ログインしていません。";
            //Textblock_In_Button_LogOn_InOut.Text = LoggedIn ? "ログアウト" : "ログイン／登録";
            //Button_LogOn_EditInfo.IsEnabled = LoggedIn;

            //TextBlock_PackageLogOnWarning.Visibility = LoggedIn ? Visibility.Collapsed : Visibility.Visible;
            //Button_SavePackageToLocal.IsEnabled = LoggedIn;
            //Button_SavePackageToTestServer.IsEnabled = LoggedIn;
            //Button_SavePackageToRealServer.IsEnabled = LoggedIn;

        }

        /// <summary>
        /// 編集画面の左下に情報を追加します。
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="comment"></param>
        internal void PostInfo(InfoIcons icon, string comment)
        {
            string glyph;

            switch (icon)
            {
                case InfoIcons.Save:
                    glyph = "\xE74E";
                    break;
                case InfoIcons.Info:
                    glyph = "\xE8BD";
                    break;
                case InfoIcons.Delete:
                    glyph = "\xE74D";
                    break;
                case InfoIcons.Change:
                    glyph = "\xE70F";
                    break;
                case InfoIcons.Add:
                    glyph = "\xE710";
                    break;
                case InfoIcons.Reset:
                    glyph = "\xE895";
                    break;
                case InfoIcons.Important:
                    glyph = "\xE8C9";
                    break;

                case InfoIcons.LogOn:
                    glyph = "\xE76E";
                    break;

                case InfoIcons.Undo:
                    glyph = "\xE7A7";
                    break;
                case InfoIcons.Redo:
                    glyph = "\xE7A6";
                    break;

                case InfoIcons.Undef:
                    glyph = "\xE76E";
                    break;

                default:
                    glyph = "\xE76E";
                    break;
            }

            var adding_stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10,
            };

            adding_stack.Children.Add(new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = glyph });
            adding_stack.Children.Add(new TextBlock() { Text = comment });

            //ListView_Info.Items.Add(adding_stack);

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(10000);
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                    {
                        //ListView_Info.Items.Remove(adding_stack);
                    });
                }
                catch (Exception)
                {
                }
            });

        }

        /// <summary>
        /// 左下の情報のアイコンリスト
        /// </summary>
        internal enum InfoIcons
        {
            Save,
            Info,
            Delete,
            Change,
            Add,
            Reset,
            Important,

            LogOn,

            Undo,
            Redo,

            Undef,
        }

    }
}
