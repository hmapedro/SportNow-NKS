using System.Diagnostics;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using SportNow.CustomViews;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using SportNow.Views.Profile;

using Microsoft.Maui.Controls;
using SportNow.Views.CompleteRegistration;

namespace SportNow.Views
{
	public class LoginPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			App.AdaptScreen();
			this.initSpecificLayout();
		}

		Label welcomeLabel;
        Label currentVersionLabel;
        RoundButton loginButton;
		FormEntry usernameEntry;
		FormEntryPassword passwordEntry;
		Label messageLabel;

		string password = "";
		string username = "";
		string message = "";

		public void initSpecificLayout()
		{
            NavigationPage.SetHasNavigationBar(this, false);

            createVersion();

            Microsoft.Maui.Controls.Grid gridLogin = new Microsoft.Maui.Controls.Grid {  Padding = 0, RowSpacing = 5 * App.screenHeightAdapter, BackgroundColor = Colors.Transparent  };
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = 40 * App.screenHeightAdapter });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = 50 * App.entryHeightAdapter });
            gridLogin.RowDefinitions.Add(new RowDefinition { Height = 50 * App.entryHeightAdapter });
            gridLogin.RowDefinitions.Add(new RowDefinition { Height = 70 * App.screenHeightAdapter });
            gridLogin.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
            gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); //GridLength.Star });
            gridLogin.RowDefinitions.Add(new RowDefinition { Height = 60 * App.screenHeightAdapter });
            //gridLogin.RowDefinitions.Add(new RowDefinition { Height = 60 });
            //gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gridLogin.ColumnDefinitions.Add(new ColumnDefinition { Width = 80 * App.screenWidthAdapter});
            gridLogin.ColumnDefinitions.Add(new ColumnDefinition { Width = App.screenWidth - 85 * App.screenWidthAdapter });

            welcomeLabel = new Label
			{
				Text = "BEM VINDO",
				TextColor = App.normalTextColor,
				FontSize = 30 * App.screenHeightAdapter,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = App.screenWidth,
				HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = "futuracondensedmedium",
                //BackgroundColor = Color.FromRgb(255, 0, 0)
            };

			Image logo_aksl = new Image
			{
				Source = "company_logo.png",
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 224 * App.screenHeightAdapter
			};
            
            if (Preferences.Default.Get("EMAIL", "") != "")
			{
				username = Preferences.Default.Get("EMAIL", "");
            }

			//USERNAME ENTRY
			usernameEntry = new FormEntry(username, "EMAIL", Keyboard.Email, App.screenWidth - 20 * App.screenWidthAdapter );

            if (Preferences.Default.Get("PASSWORD", "") != "")
            {
                password = Preferences.Default.Get("PASSWORD", "");
            }


            //PASSWORD ENTRY
            passwordEntry = new FormEntryPassword(password, "PASSWORD", App.screenWidth - 20 * App.screenWidthAdapter);

            //LOGIN BUTTON

            loginButton = new RoundButton("LOGIN", App.screenWidth - 20 * App.screenWidthAdapter, 50);
            loginButton.button.Clicked += OnLoginButtonClicked;


            /*            loginButton = new Button
                        {
                            Text = "LOGIN",
                            BackgroundColor = App.topColor,
                            TextColor = App.oppositeTextColor,
                            HorizontalOptions = LayoutOptions.Center,
                            WidthRequest = App.screenWidth - 20 * App.screenWidthAdapter,
                            HeightRequest = 45 * App.screenHeightAdapter,
                            FontFamily = "futuracondensedmedium",
                            FontSize = App.titleFontSize
                        };


                        Frame frame_loginButton = new Frame {
                            BackgroundColor = App.backgroundColor,
                            BorderColor = Colors.LightGray,
                            CornerRadius = 10 * (float) App.screenWidthAdapter,
                            IsClippedToBounds = true,
                            Padding = 0,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            WidthRequest = App.screenWidth - 20 * App.screenWidthAdapter,
                            HeightRequest = 45 * App.screenWidthAdapter
                        };

                        frame_loginButton.Content = loginButton;
			*/
            messageLabel = new Label {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                TextColor = Colors.Red,
                FontSize = App.itemTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            messageLabel.Text = this.message;

            //RECOVER PASSWORD LABEL
            Label recoverPasswordLabel = new Label
            {
                Text = "Recuperar palavra-passe",
                TextColor = App.normalTextColor,
                FontSize = App.formLabelFontSize,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = "futuracondensedmedium"
            };
            var recoverPasswordLabel_tap = new TapGestureRecognizer();
            recoverPasswordLabel_tap.Tapped += (s, e) =>
            {
                /*Navigation.InsertPageBefore(new RecoverPasswordPageCS(), this);
                Navigation.PopAsync();*/

            Navigation.PushAsync(new RecoverPasswordPageCS());

			};
			recoverPasswordLabel.GestureRecognizers.Add(recoverPasswordLabel_tap);


            //RECOVER PASSWORD LABEL
            Label newMemberLabel = new Label
            {
                Text = "Novo Sócio",
                TextColor = App.normalTextColor,
                FontSize = App.formLabelFontSize,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = "futuracondensedmedium"
            };
            var newMemberLabel_tap = new TapGestureRecognizer();
            newMemberLabel_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ConsentPageCS());
            };
            newMemberLabel.GestureRecognizers.Add(newMemberLabel_tap);

            Image anadiaImage = new Image
            {
                Source = "anadia.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 50 * App.screenHeightAdapter
            };

            //RECOVER PASSWORD LABEL
            Label anadiaLabel = new Label
            {
                Text = "Aplicação apoiada no âmbito do incentivo à digitalização pelo Município de Anadia.",
                TextColor = App.normalTextColor,
                FontSize = App.formLabelFontSize,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = "futuracondensedmedium"
            };
            

            gridLogin.Add(welcomeLabel, 0, 0);
            Grid.SetColumnSpan(welcomeLabel, 2);
            gridLogin.Add(logo_aksl, 0, 1);
            Grid.SetColumnSpan(logo_aksl, 2);
            gridLogin.Add(messageLabel, 0, 2);
            Grid.SetColumnSpan(messageLabel, 2);
            gridLogin.Add(usernameEntry, 0, 3);
            Grid.SetColumnSpan(usernameEntry, 2);
            gridLogin.Add(passwordEntry, 0, 4);
            Grid.SetColumnSpan(passwordEntry, 2);
            gridLogin.Add(loginButton, 0, 5);
            Grid.SetColumnSpan(loginButton, 2);
            gridLogin.Add(recoverPasswordLabel, 0, 6);
            Grid.SetColumnSpan(recoverPasswordLabel, 2);
            //gridLogin.Add(newMemberLabel, 0, 7);
            gridLogin.Add(anadiaImage, 0, 7);
            gridLogin.Add(anadiaLabel, 1, 7);
            gridLogin.Add(currentVersionLabel, 0, 8);
            Grid.SetColumnSpan(currentVersionLabel, 2);

            absoluteLayout.Add(gridLogin);
            absoluteLayout.SetLayoutBounds(gridLogin, new Rect(0, 0, App.screenWidth, App.screenHeight - 30));
        }


		public LoginPageCS (string message)
		{
			if (message != "")
			{
				this.message = message;
				//UserDialogs.Instance.Alert(new AlertConfig() { Title = "Erro", message, OkText = "Ok" });
			}
			
			//this.initBaseLayout();
			

		}

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			//await Navigation.PushAsync (new SignUpPageCS ());
		}

		async void OnLoginButtonClicked (object sender, EventArgs e)
		{
			Debug.WriteLine("OnLoginButtonClicked");

			loginButton.IsEnabled = false;

			var user = new User {
				Username = usernameEntry.entry.Text,
				Password = passwordEntry.entry.Text
			};


			MemberManager memberManager = new MemberManager();

			showActivityIndicator();

			var loginResult = await memberManager.Login(user);

			if (loginResult == "1")
			{
				Debug.WriteLine("login ok");

				App.members = await GetMembers(user);

				this.saveUserPassword(user.Username, user.Password);

				if (App.members.Count == 1)
                {
					App.original_member = App.members[0];
					App.member = App.original_member;

					//Navigation.InsertPageBefore(new MainTabbedPageCS("",""), this);
                    //await Navigation.PopAsync();

                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                    };

                    
				}
				else if (App.members.Count > 1)
				{
					Navigation.InsertPageBefore(new SelectMemberPageCS(), this);
					await Navigation.PopAsync();
				}
			}
			else {
				Debug.WriteLine("login nok");
				loginButton.IsEnabled = true;
				passwordEntry.entry.Text = string.Empty;

				if (loginResult == "0")
				{
					Debug.WriteLine("Login falhou. O Utilizador não existe");
					messageLabel.Text = "Login falhou. O Utilizador não existe.";
				}
				else if (loginResult == "-1")
				{
					Debug.WriteLine("Login falhou. A Password está incorreta");
					messageLabel.Text = "Login falhou. A Password está incorreta.";
				}
				else if (loginResult == "-2")
				{
					Debug.WriteLine("Ocorreu um erro. Volte a tentar mais tarde.");
					messageLabel.Text = "Ocorreu um erro. Volte a tentar mais tarde.";
				}
				else
				{
					Debug.WriteLine("Ocorreu um erro. Volte a tentar mais tarde.");
					messageLabel.Text = "Ocorreu um erro. Volte a tentar mais tarde.";
                    //await DisplayAlert("Erro Login", loginResult, "Ok" );
                    await DisplayAlert("Erro Login", "Ocorreu um erro. Volte a tentar mais tarde.", "Ok");
                }

				this.saveUserPassword(user.Username, user.Password);
			}
			hideActivityIndicator();
		}


		async Task<List<Member>> GetMembers(User user)
		{
			Debug.WriteLine("GetMembers");
			MemberManager memberManager = new MemberManager();

			List<Member> members;

			members = await memberManager.GetMembers(user);

			return members;
			
		}

		protected void saveUserPassword(string email, string password)
		{
            Preferences.Default.Remove("EMAIL");
            Preferences.Default.Remove("PASSWORD");

            Preferences.Default.Set("EMAIL", email);
            Preferences.Default.Set("PASSWORD", password);

			//Application.Current.SavePropertiesAsync();

			username = Preferences.Default.Get("EMAIL", "");
        }

        public async void createVersion()
        {
			currentVersionLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = "Version " + App.VersionNumber + " - " + App.BuildNumber,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };

        }
    }
}


