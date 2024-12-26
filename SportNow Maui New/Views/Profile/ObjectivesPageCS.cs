using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Model;


namespace SportNow.Views.Profile
{
    public class ObjectivesPageCS : DefaultPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AdaptScreen();
            initSpecificLayout();
        }

        protected override void OnDisappearing()
        {
            this.CleanScreen();
        }

        FormValueEditLongText objetivosEntry, disponibilidadeEntry;
        bool alreadyMember;

        public void initLayout()
        {
            Title = "NOVA ÉPOCA";

            var toolbarItem = new ToolbarItem
            {
                Text = "Fechar",
            };
            toolbarItem.Clicked += OnCloseButtonClicked;
            ToolbarItems.Add(toolbarItem);
        }


        public void CleanScreen()
        {
            Debug.Print("CleanScreen");
        }

        public async void initSpecificLayout()
        {
            showActivityIndicator();

            MemberManager memberManager = new MemberManager();

            await memberManager.GetPastFees(App.member);

            alreadyMember = false;
            foreach (Fee fee in App.member.pastFees)
            {
                Debug.Print("fee.estado = "+fee.estado_quota);

                if (fee.estado_quota == "ativa")
                {
                    alreadyMember = true;
                    break;
                }
            }

            string objetivosLabelText, objetivosExplicacaoLabelText;

            if (alreadyMember)
            {
                objetivosLabelText = "REVALIDAÇÃO\n"+App.member.nickname; //2024 - 2025;
                objetivosExplicacaoLabelText = "Desde de já obrigado por teres estado mais um época connosco. Queremos que continues a evoluir connosco, mas para isso, precisamos que submetas este questionário, sobre os objetivos que pretendes para a próxima época.";
            }
            else
            {
                objetivosLabelText = "BEM-VINDO!\n"+App.member.nickname; //2024 - 2025;
                objetivosExplicacaoLabelText = "Obrigado por te juntares à família NKS. Queremos que evoluas connosco, mas para isso, precisamos que submetas este questionário, sobre os objetivos que pretendes para esta época.";
            }

            Label objetivosLabel = new Label
            {
                Text = objetivosLabelText,
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };
            //absoluteLayout.Add(objetivosLabel);
            //absoluteLayout.SetLayoutBounds(objetivosLabel, new Rect(10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

            Label objetivosExplicacaoLabel = new Label
            {
                Text = objetivosExplicacaoLabelText,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.formLabelFontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                FontFamily = "futuracondensedmedium",
            };


            Label objetivosDisclaimerLabel = new Label
            {
                Text = "Notas (Conforme Regulamento Interno):\n"+
                    "- Caso não pretenda renovar a inscrição, comunique diretamente com o treinador responsável.\n"+
                    "- A desistência não comunicada aos Treinadores, impossibilita renovação ou nova inscrição no NKS.\n"+
                    "- No inicio da época desportiva, o atleta só poderá retomar os treinos não tendo dívidas para com o clube.",
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.formValueSmallFontSize,
                FontFamily = "futuracondensedmedium",
            };

            //absoluteLayout.Add(objetivosExplicacaoLabel);
            //absoluteLayout.SetLayoutBounds(objetivosExplicacaoLabel, new Rect(10 * App.screenWidthAdapter, 70 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 130 * App.screenHeightAdapter));

            if ((App.member.objectives != null) & (App.member.objectives.Count > 0))
            {
                objetivosEntry = new FormValueEditLongText(App.member.objectives[0].objectivos, Keyboard.Chat, Convert.ToInt16(200 * App.screenHeightAdapter));
            }
            else
            {
                objetivosEntry = new FormValueEditLongText("", Keyboard.Chat, Convert.ToInt16(200 * App.screenHeightAdapter));
            }

            //absoluteLayout.Add(objetivosEntry);
            //absoluteLayout.SetLayoutBounds(objetivosEntry, new Rect(10 * App.screenWidthAdapter, 200 * App.screenHeightAdapter, App.screenWidth - (20 * App.screenHeightAdapter), 400 * App.screenHeightAdapter));
           
            RegisterButton confirmButton = new RegisterButton("ENVIAR", App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter);
            //personalClassesButton.button.BackgroundColor = App.topColor;
            confirmButton.button.Clicked += OnConfirmButtonClicked;

            //absoluteLayout.Add(confirmButton);
            //absoluteLayout.SetLayoutBounds(confirmButton, new Rect(10 * App.screenWidthAdapter, App.screenHeight - 100 - 60 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 50 * App.screenHeightAdapter));

            StackLayout objectivesStackLayout = new StackLayout();


            if (alreadyMember)
            {
                objectivesStackLayout = new StackLayout
                {
                    //WidthRequest = 370,
                    Margin = new Thickness(0),
                    Spacing = 20 * App.screenHeightAdapter,
                    Orientation = StackOrientation.Vertical,
                    Children =
                        {
                            objetivosLabel,
                            objetivosExplicacaoLabel,
                            objetivosEntry,
                            objetivosDisclaimerLabel,
                            confirmButton
                        }
                };
            }
            else
            {
                objectivesStackLayout = new StackLayout
                {
                    //WidthRequest = 370,
                    Margin = new Thickness(0),
                    Spacing = 20 * App.screenHeightAdapter,
                    Orientation = StackOrientation.Vertical,
                    Children =
                        {
                            objetivosLabel,
                            objetivosExplicacaoLabel,
                            objetivosEntry,
                            confirmButton
                        }
                };
            }

            

            absoluteLayout.Add(objectivesStackLayout);
            absoluteLayout.SetLayoutBounds(objectivesStackLayout, new Rect(10 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 110 ));


            hideActivityIndicator();
        }



        public ObjectivesPageCS()
        {
            this.initLayout();
        }


        async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", "", false))
            {
                BarBackgroundColor = App.backgroundColor,
                BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
            };
        }

        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("ObjectivesPageCS.OnConfirmButtonClicked");

            if (objetivosEntry.entry.Text != "")
            {
                showActivityIndicator();
                MemberManager memberManager = new MemberManager();
                await memberManager.CreateObjective(App.member.id, "Objetivos - " + App.member.nickname + " - " + App.getSeasonString(), App.getSeason(), objetivosEntry.entry.Text);
                if (alreadyMember)
                {
                    await memberManager.sendMailSeason(App.member.name, App.member.email, "1");
                }
                else
                {
                    await memberManager.sendMailSeason(App.member.name, App.member.email, "0");
                }

                hideActivityIndicator();

                await DisplayAlert("OBRIGADO", "Resultados extraordinários vêm de passos comuns todos os dias, na direção certa.", "OK");
                App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };
            }
            else
            {
                bool res = await DisplayAlert("Informação em falta", "Tens a certeza que não queres dizer-nos os teus objetivos para esta época?", "Agora Não", "Quero");
                if (res == true)
                {
                    showActivityIndicator();
                    MemberManager memberManager = new MemberManager();
                    await memberManager.CreateObjective(App.member.id, "Objetivos - " + App.member.nickname + " - " + App.getSeasonString(), App.getSeason(), "");
                    if (alreadyMember)
                    {
                        await memberManager.sendMailSeason(App.member.name, App.member.email, "1");
                    }
                    else
                    {
                        await memberManager.sendMailSeason(App.member.name, App.member.email, "0");
                    }
                    hideActivityIndicator();
                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                    };
                }
            }
        }

        
    }
}
