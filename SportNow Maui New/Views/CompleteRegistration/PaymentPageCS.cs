﻿using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Services.Data.JSON;
//Ausing Acr.UserDialogs;
using System.Globalization;

namespace SportNow.Views.CompleteRegistration
{
	public class PaymentPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{

		}


		private ScrollView scrollView;

		//private Member member;


		FormValue valueQuotaNKS, valueMensalidadeNKS, valueTotal;
		Picker familiaresPicker;
		public double valorQuotaNKS;
		string paymentID;
		Payment payment;

		bool paymentDetected;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{
			showActivityIndicator();
			MemberManager memberManager = new MemberManager();
			string season = DateTime.Now.ToString("yyyy");

			//TENHO DE METER AQUI VALIDAÇÕES!!!
			//
			string feeID = await memberManager.CreateFee(App.original_member.id, App.member.member_type, season);

            List<Payment> payments = await memberManager.GetFeePayment(feeID);
			Payment payment = payments[0];
            paymentID = payment.id;

            PaymentManager paymentManager = new PaymentManager();
            _ = await paymentManager.Update_Payment_Name(paymentID, App.member.id, "Inscrição - "+App.member.name);


            string year = DateTime.Now.Year.ToString();
			Debug.Print("year = " + year);
			string month = "";
			if (DateTime.Now.Month == 8)
			{
				month = "9";
			}
			else
			{
				month = DateTime.Now.Month.ToString();
			}

			MonthFeeManager monthFeeManager = new MonthFeeManager();
			string monthFeeID = "";
			string valor_mensalidade = "";
			if (App.member.member_type == "praticante")
			{
				monthFeeID = await monthFeeManager.CreateMonthFee(App.original_member.id, App.member.id, App.member.name, valor_mensalidade, year, month, "emitida", paymentID, "0");
				valor_mensalidade = calculateMensalidade(0).ToString("0.00").Replace(",", ".");
				await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, valor_mensalidade);
			}
			hideActivityIndicator();

			List<Fee> allFees = await memberManager.GetFees(App.member.id, season);
			Fee fee = allFees[0];
			valorQuotaNKS = fee.valor;

			int y_index = CreateHeader();
			y_index = y_index + 10;

			Label labelQuotaNKS = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };

            absoluteLayout.Add(labelQuotaNKS);
            absoluteLayout.SetLayoutBounds(labelQuotaNKS, new Rect(20 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

            labelQuotaNKS.Text = "Quota Sócio";

            valueQuotaNKS = new FormValue(valorQuotaNKS.ToString("0.00") + "€", App.titleFontSize, Colors.White, App.normalTextColor, TextAlignment.End);
            //valueQuotaADCPN.Text = calculateQuotaADCPN();
            absoluteLayout.Add(valueQuotaNKS);
            absoluteLayout.SetLayoutBounds(valueQuotaNKS, new Rect(App.screenWidth - 80 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, 70 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

			y_index = y_index + 35;

			if (App.member.member_type == "praticante")
			{

				valueMensalidadeNKS = new FormValue(calculateMensalidade(0).ToString("0.00") + "€", App.titleFontSize, App.backgroundColor, App.normalTextColor, TextAlignment.End);

				valor_mensalidade = calculateMensalidade(0).ToString("0.00").Replace(",", ".");
				await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, valor_mensalidade);
				hideActivityIndicator();

				Label labelMensalidadeNKS = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				labelMensalidadeNKS.Text = "Mensalidade";
				absoluteLayout.Add(labelMensalidadeNKS);
				absoluteLayout.SetLayoutBounds(labelMensalidadeNKS, new Rect(20 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

				absoluteLayout.Add(valueMensalidadeNKS);
				absoluteLayout.SetLayoutBounds(valueMensalidadeNKS, new Rect(App.screenWidth - 80 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, 70 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));
			}
			y_index = y_index + 45;

			Label labelTotal = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			labelTotal.Text = "Total";
            absoluteLayout.Add(labelTotal);
            absoluteLayout.SetLayoutBounds(labelTotal, new Rect(20 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 120 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));


			valueTotal = new FormValue(calculateTotal(0).ToString("0.00") + "€", App.titleFontSize, App.topColor, App.backgroundColor, TextAlignment.End);

            absoluteLayout.Add(valueTotal);
            absoluteLayout.SetLayoutBounds(valueTotal, new Rect(App.screenWidth - 80 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, 70 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));


			Label selectPaymentModeLabel = new Label
			{
				Text = "Escolha o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.titleFontSize
			};

            absoluteLayout.Add(selectPaymentModeLabel);
            absoluteLayout.SetLayoutBounds(selectPaymentModeLabel, new Rect(20 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

			y_index = y_index + 30;

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				MinimumHeightRequest = 120 * App.screenHeightAdapter,
				HeightRequest = 120 * App.screenHeightAdapter,
			};

			var tapGestureRecognizerMB = new TapGestureRecognizer();
			tapGestureRecognizerMB.Tapped += OnMBButtonClicked;
			MBLogoImage.GestureRecognizers.Add(tapGestureRecognizerMB);

            absoluteLayout.Add(MBLogoImage);
            absoluteLayout.SetLayoutBounds(MBLogoImage, new Rect(40 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, 102 * App.screenHeightAdapter, 120 * App.screenHeightAdapter));

			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				MinimumHeightRequest = 120 * App.screenHeightAdapter,
				HeightRequest = 120 * App.screenHeightAdapter
			};

			var tapGestureRecognizerMBWay = new TapGestureRecognizer();
			tapGestureRecognizerMBWay.Tapped += OnMBWayButtonClicked;
			MBWayLogoImage.GestureRecognizers.Add(tapGestureRecognizerMBWay);

            absoluteLayout.Add(MBWayLogoImage);
            absoluteLayout.SetLayoutBounds(MBWayLogoImage, new Rect(App.screenWidth - 142 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, 102 * App.screenHeightAdapter, 120 * App.screenHeightAdapter));

		}



		public double getValorQuota(List<Fee> allFees, string tipoQuota)
		{
			foreach (Fee fee in allFees)
			{
				if (fee.tipo == tipoQuota)
				{
					return Convert.ToDouble(fee.valor);
				}
			}
			return 0;
		}

		public int CreateHeader()
		{
			int y_index = 10;

			Label nameLabel = new Label
			{
				Text = App.member.nickname,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Colors.Gray,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

            absoluteLayout.Add(nameLabel);
            absoluteLayout.SetLayoutBounds(nameLabel, new Rect(10 * App.screenWidthAdapter, y_index * App.screenHeightAdapter, App.screenWidth - 20 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

			return y_index + 30;
		}

		public PaymentPageCS()
		{

			this.initLayout();
			this.initSpecificLayout();

			paymentDetected = false;

			int sleepTime = 5;
			/*Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
			{
				if ((paymentID != null) & (paymentID != ""))
				{
					this.checkPaymentStatus(paymentID);
					if (paymentDetected == false)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			});*/
		}

		async void checkPaymentStatus(string paymentID)
		{
			Debug.Print("checkPaymentStatus");
			this.payment = await GetPayment(paymentID);
			if ((payment.status == "confirmado") | (payment.status == "fechado"))
			{
				App.member.estado = "activo";
				App.original_member.estado = "activo";

				if (paymentDetected == false)
				{
					paymentDetected = true;

					await DisplayAlert("Pagamento Confirmado", "O seu pagamento foi recebido com sucesso. Já pode aceder à nossa App!", "Ok");
					App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = App.backgroundColor,
                        BarTextColor = App.normalTextColor
                    };

                }
			}
		}

		async Task<Payment> GetPayment(string paymentID)
		{
			Debug.WriteLine("GetPayment");
			PaymentManager paymentManager = new PaymentManager();

			Payment payment = await paymentManager.GetPayment(this.paymentID);

			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor//FromRgb(75, 75, 75)
                };
                return null;
			}
			return payment;
		}

		public double calculateMensalidade(double desconto)
		{
			//Debug.Print("calculateMensalidade App.member.aulavalor = " + App.member.aulavalor);
			Debug.Print("calculateMensalidade desconto = " + desconto);
			Debug.Print("calculateMensalidade desconto = " + desconto);

			//Debug.Print("App.member.aulavalor = " + String.Format("{0:0}", App.member.aulavalor) + "€");
			//return String.Format("{0:0}", App.member.aulavalor) + ";
			double aulavalor = 0;// double.Parse(App.member.aulavalor, CultureInfo.InvariantCulture);
			return aulavalor * (1 - desconto);

		}

		public double calculateTotal(double desconto)
		{
			return valorQuotaNKS + calculateMensalidade(0);
			//return calculateQuotaADCPN() + calculateFiliacaoFPG() + calculateSeguroFPG() + calculateMensalidade();
		}

		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("member.member_type = " + App.member.member_type);

			if (App.member.member_type == "praticante")
			{
				if (familiaresPicker.SelectedIndex == 0) //2
				{
					await DisplayAlert("Escolha uma Opção", "Para poder prosseguir tem de escolher uma opção de Número de familiares inscritos praticantes na Associação.", "Ok");
				}
				else
				{
					//await Navigation.PushAsync(new CompleteRegistration_PaymentMB_PageCS(paymentID));
				}
			}
			else
			{
				//await Navigation.PushAsync(new CompleteRegistration_PaymentMB_PageCS(paymentID));
			}
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			if (App.member.member_type == "praticante")
			{
				if (familiaresPicker.SelectedIndex == 0) //2
				{
					await DisplayAlert("Escolha uma Opção", "Para poder prosseguir tem de escolher uma opção de Número de familiares inscritos praticantes na Associação.", "Ok");
				}
				else
				{
					//await Navigation.PushAsync(new CompleteRegistration_PaymentMBWay_PageCS(paymentID));
				}
			}
			else
			{
				//await Navigation.PushAsync(new CompleteRegistration_PaymentMBWay_PageCS(paymentID));
			}

		}

	}

}