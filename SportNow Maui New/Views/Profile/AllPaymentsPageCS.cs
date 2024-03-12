using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;

namespace SportNow.Views.Profile
{
	public class AllPaymentsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private Microsoft.Maui.Controls.StackLayout stackButtons;

		private CollectionView collectionViewPayments;
		ObservableCollection<Payment> payments_filtered;
		FormValueEdit searchEntry;

        //private List<Member> members;

        public void initLayout()
		{
			Debug.Print("AllPaymentsPageCS.initLayout");
			Title = "PAGAMENTOS";
        }


        public void CleanScreen()
		{
			Debug.Print("AllPaymentsPageCS.CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
            {
				absoluteLayout.Remove(stackButtons);
				absoluteLayout.Remove(collectionViewPayments);

				stackButtons = null;
                collectionViewPayments = null;
			}

		}

		public async void initSpecificLayout()
		{
			Label titleLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "LISTAGEM DE PAGAMENTOS:";

			absoluteLayout.Add(titleLabel);
			absoluteLayout.SetLayoutBounds(titleLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			CompletePayments();
            CreateSearchEntry();
            CreatePaymentsColletion();
		}

		public void CreateSearchEntry()
		{
            searchEntry = new FormValueEdit("",Keyboard.Text, 45);
			searchEntry.entry.Placeholder = "Pesquisa...";
            searchEntry.entry.TextChanged += onSearchTextChange;
            absoluteLayout.Add(searchEntry);
            absoluteLayout.SetLayoutBounds(searchEntry, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth, 50 * App.screenHeightAdapter));

        }

        async void onSearchTextChange(object sender, EventArgs e)
        {
            Debug.WriteLine("AllPaymentsPageCS.onSearchTextChange");
			if (searchEntry.entry.Text == "")
			{
                payments_filtered = new ObservableCollection<Payment>(App.member.payments);

            }
			else
			{
                payments_filtered = new ObservableCollection<Payment>(App.member.payments.Where(i => i.name.ToLower().Contains(searchEntry.entry.Text.ToLower())));
            }

            collectionViewPayments.ItemsSource = null;
            collectionViewPayments.ItemsSource = payments_filtered;
            Debug.Print("AllPaymentsPageCS.onSearchTextChange " + payments_filtered.Count());

        }

        public void CompletePayments()
        {
			foreach (Payment payment in App.member.payments)
			{
				if (payment.status == "aberto") 
				{
					payment.statusText = "Por pagar";

                }
				else if (payment.status == "anulado")
				{
                    payment.statusText = "Anulado";
                }
                else
                {
                    payment.statusText = "Pago";
                }
            }
        }

        public void CreatePaymentsColletion()
		{

            payments_filtered = new ObservableCollection<Payment>(App.member.payments);

            Debug.Print("AllPaymentsPageCS.CreatePaymentsColletion " + payments_filtered.Count());
			//COLLECTION GRADUACOES
			collectionViewPayments = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = payments_filtered,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { FontFamily = "futuracondensedmedium", Text = "Não tem pagamentos associados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

            collectionViewPayments.SelectionChanged += OnCollectionViewPaymentsSelectionChanged;

            collectionViewPayments.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = 35 * App.screenHeightAdapter
				};

				FormValue nameLabel = new FormValue("", 35 * App.screenHeightAdapter);
				nameLabel.label.FontSize = App.formValueSmallFontSize;
                nameLabel.label.SetBinding(Label.TextProperty, "name");


				itemabsoluteLayout.Add(nameLabel);
				itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(0,0, App.screenWidth - 110 * App.screenWidthAdapter, 35 * App.screenHeightAdapter));

				FormValue estadoLabel = new FormValue("", 35 * App.screenHeightAdapter);
                estadoLabel.label.FontSize = App.formValueSmallFontSize;
                estadoLabel.label.SetBinding(Label.TextProperty, "statusText");


				itemabsoluteLayout.Add(estadoLabel);
				itemabsoluteLayout.SetLayoutBounds(estadoLabel, new Rect(App.screenWidth - (105 * App.screenWidthAdapter), 0, 100 * App.screenWidthAdapter, 35 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});

            absoluteLayout.Add(collectionViewPayments);
            absoluteLayout.SetLayoutBounds(collectionViewPayments, new Rect(0, 110 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 220 * App.screenHeightAdapter));

        }

        public AllPaymentsPageCS()
		{
			Debug.WriteLine("AllPaymentsPageCS");
			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnCollectionViewPaymentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("AllPaymentsPageCS.OnCollectionViewPaymentsSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Payment payment = (sender as CollectionView).SelectedItem as Payment;

                Debug.WriteLine("AllPaymentsPageCS.OnCollectionViewPaymentsSelectionChanged payment.id="+payment.invoiceid);
                if ((payment.invoiceid != "") & (payment.invoiceid != null))
				{
                    await Navigation.PushAsync(new InvoiceDocumentPageCS(payment.invoiceid));
                }
                
			}

		}
    }
}
