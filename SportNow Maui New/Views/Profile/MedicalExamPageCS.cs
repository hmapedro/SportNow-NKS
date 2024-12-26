using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Model;
using Syncfusion.Maui.PdfViewer;

namespace SportNow.Views.Profile
{
    public class MedicalExamPageCS : DefaultPage
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
            Title = "EXAME MÉDICO";
        }


        public void CleanScreen()
        {
            Debug.Print("CleanScreen");
        }

        public async void initSpecificLayout()
        {

            Image ipdjImage = new Image
            {
                Source = "ipdj.png",
                WidthRequest = 80,
            };


            bool isExpired = true;
            bool isAlmostExpired = false;
            string expireDate = "";
            string documentID = "";
            string mimeType = "";
            foreach (MedicalExam medicalExam in App.member.medicalExams)
            {
                if (medicalExam.status == "Aprovado")
                {
                    isExpired = false;
                    expireDate = medicalExam.expireDate;
                    documentID = medicalExam.id;
                    mimeType = medicalExam.mimeType;
                    DateTime currentTime = DateTime.Now.Date;
                    DateTime expireDate_datetime = DateTime.Parse(medicalExam.expireDate).Date;
                    Debug.Print("(expireDate_datetime - currentTime).Days = " + (expireDate_datetime - currentTime).Days);
                    if ((expireDate_datetime - currentTime).Days < 30)
                    {
                        isAlmostExpired = true;
                    }
                }
            }

            string exameMedicoLabelText = "";


            if (isExpired)
            {
                exameMedicoLabelText = "Informamos que tens o Exame Médico Desportivo em falta ou expirado.";
            }
            else if (isAlmostExpired)
            {
                exameMedicoLabelText = "Informamos que tens o Exame Médico Desportivo quase a expirar.";
            }
            else
            {
                exameMedicoLabelText = "Informamos que tens o Exame Médico Desportivo válido.";
            }

            Debug.Print("exameMedicoLabelText = " + exameMedicoLabelText);

            Label exameMedicoLabel = new Label
            {
                Text = exameMedicoLabelText,
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.bigTitleFontSize,
                FontFamily = "futuracondensedmedium",
            };
            //absoluteLayout.Add(objetivosLabel);
            //absoluteLayout.SetLayoutBounds(objetivosLabel, new Rect(10 * App.screenWidthAdapter, 30 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

            Label expireDateLabel = new Label
            {
                Text = "Data de Expiração: " + expireDate,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.formLabelFontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                FontFamily = "futuracondensedmedium",
            };

            StackLayout medicalExamStackLayout = new StackLayout();
            Image browser2 = new Image();
            SfPdfViewer browser1 = new SfPdfViewer();

            if (documentID != "")
            {
                Debug.Print("mimeType = " + mimeType);
                if (mimeType == "application/pdf")
                {
                    HttpClient httpClient = new HttpClient();

                    var docUrl = Constants.images_URL + documentID;

                    showActivityIndicator();
                    HttpResponseMessage response = await httpClient.GetAsync(docUrl);
                    Stream PdfDocumentStream = await response.Content.ReadAsStreamAsync();
                    hideActivityIndicator();

                    browser1.DocumentSource = PdfDocumentStream;
                    browser1.WidthRequest = App.screenWidth;
                    browser1.HeightRequest = 400 * App.screenHeightAdapter;
                }
                else
                {
                    browser2 = new Image
                    {
                        HeightRequest = 400 * App.screenHeightAdapter,
                        WidthRequest = App.screenWidth,
                        VerticalOptions = LayoutOptions.Fill,
                        Source = Constants.images_URL + documentID,
                    };
                }
                
            }

            Label expiredLabel = new Label
            {
                Text = "Com o Exame Médico Desportivo (EMD) inválido não poderás participar nas atividades.\n\n\nApós consulta médica, deverás entregar o destacado do EMD ao treinador responsável.",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.formLabelFontSize,
                LineBreakMode = LineBreakMode.WordWrap,
                FontFamily = "futuracondensedmedium",
            };

            Label MedicalExamTemplateLinkLabel = new Label { FontFamily = "futuracondensedmedium", Text = "Carrega aqui para fazeres download do documento do Exame Médico Desportivo.", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 13 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

            var MedicalExamTemplateLinkLabel_tap = new TapGestureRecognizer();
            MedicalExamTemplateLinkLabel_tap.Tapped += async (s, e) =>
            {
                try
                {
                    await Browser.OpenAsync("https://nks-server.synology.me:5011/d/s/xzxm7stPNMRWraIihOgYq8Mo7ZlM6Y7P/J6Nyt-8b5hpaBnaSsMh2sGMtZ1nxXGGM-37KgzgPynws", BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                }
            };
            MedicalExamTemplateLinkLabel.GestureRecognizers.Add(MedicalExamTemplateLinkLabel_tap);


            Label ClinicNameLabel = new Label {
                FontFamily = "futuracondensedmedium",
                Text = "\nCLÍNICA IBERVITA (ANADIA)",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 15 * App.screenWidthAdapter,
                FontAttributes = FontAttributes.Bold,
                TextColor = App.normalTextColor
            };

            Label ClinicContactsLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Contactos:",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 13 * App.screenWidthAdapter,
                TextColor = App.normalTextColor,
            };

            Label ClinicPhoneLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "+351 231 525 767",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 13 * App.screenWidthAdapter,
                TextColor = App.normalTextColor,
            };

            var ClinicPhoneLabel_tap = new TapGestureRecognizer();
            ClinicPhoneLabel_tap.Tapped += async (s, e) =>
            {
                PhoneDialer.Open("+351 231 525 767");
            };
            ClinicPhoneLabel.GestureRecognizers.Add(ClinicPhoneLabel_tap);

            Label ClinicEmailLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "hello@ibervita.com",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 13 * App.screenWidthAdapter,
                TextColor = App.normalTextColor,
            };

            var ClinicEmailLabel_tap = new TapGestureRecognizer();
            ClinicEmailLabel_tap.Tapped += async (s, e) =>
            {
                if (Email.Default.IsComposeSupported)
                {
                    var message = new EmailMessage
                    {

                        To = new List<string>(new[] { "hello@ibervita.com" })
                    };
                    await Email.Default.ComposeAsync(message);
                }
            };
            ClinicEmailLabel.GestureRecognizers.Add(ClinicEmailLabel_tap);

            Label ClinicPriceLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Valor: 9,75€",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 13 * App.screenWidthAdapter,
                TextColor = App.normalTextColor,
            };
            Label ClinicOtherLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Outros descontos podem ser encontrados em documentos,na pasta protocolos, na homepage da app.",
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 10 * App.screenWidthAdapter,
                TextColor = App.normalTextColor,
            };

            StackLayout ClinicContactStackLayout = new StackLayout
            {
                //WidthRequest = 370,
                Margin = new Thickness(0),
                Spacing = 10 * App.screenWidthAdapter,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                        {
                            ClinicPhoneLabel,
                            ClinicEmailLabel,
                        }
            };


            StackLayout ClinicStackLayout = new StackLayout
            {
                //WidthRequest = 370,
                Margin = new Thickness(0),
                Spacing = 5 * App.screenWidthAdapter,
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                        {
                            ClinicNameLabel,
                            ClinicContactStackLayout,
                            ClinicPriceLabel,
                            ClinicOtherLabel
                        }
            };


            if (isExpired)
            {
                medicalExamStackLayout = new StackLayout
                {
                    //WidthRequest = 370,
                    Margin = new Thickness(0),
                    Spacing = 25 * App.screenHeightAdapter,
                    Orientation = StackOrientation.Vertical,
                    Children =
                        {
                            ipdjImage,
                            exameMedicoLabel,
                            expiredLabel,
                            MedicalExamTemplateLinkLabel,
                            ClinicStackLayout
                        }
                };
            }
            else if (isAlmostExpired)
            {
                medicalExamStackLayout = new StackLayout
                {
                    //WidthRequest = 370,
                    Margin = new Thickness(0),
                    Spacing = 15 * App.screenHeightAdapter,
                    Orientation = StackOrientation.Vertical,
                    Children =
                        {
                            ipdjImage,
                            exameMedicoLabel,
                            expireDateLabel,
                            MedicalExamTemplateLinkLabel,
                            ClinicStackLayout
                        }
                };

                if (mimeType == "application/pdf")
                {
                    medicalExamStackLayout.Add(browser1);
                }
                else
                {
                    medicalExamStackLayout.Add(browser2);
                }
            }
            else
            {
                medicalExamStackLayout = new StackLayout
                {
                    //WidthRequest = 370,
                    Margin = new Thickness(0),
                    Spacing = 20 * App.screenHeightAdapter,
                    Orientation = StackOrientation.Vertical,
                    Children =
                        {
                            ipdjImage,
                            exameMedicoLabel,
                            expireDateLabel,
                        }
                };
                if (mimeType == "application/pdf")
                {
                    medicalExamStackLayout.Add(browser1);
                }
                else
                {
                    medicalExamStackLayout.Add(browser2);
                }
            }

            absoluteLayout.Add(medicalExamStackLayout);
            absoluteLayout.SetLayoutBounds(medicalExamStackLayout, new Rect(10 * App.screenWidthAdapter, 10 * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, App.screenHeight - 110 ));


            //hideActivityIndicator();
        }



        public MedicalExamPageCS()
        {
            this.initLayout();
        }
        
    }
}
