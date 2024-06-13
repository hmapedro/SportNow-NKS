using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Microsoft.Maui.Controls.Shapes;

namespace SportNow.Views
{
	public class EventParticipationsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            base.OnAppearing();
            //competition_participation = App.competition_participation;
            initSpecificLayout();

		}

		protected override void OnDisappearing()
		{
            //absoluteLayout = null;
            collectionViewEventParticipations = null;	
			registerButton = null;
		}

		private CollectionView collectionViewEventParticipations;

		private Event event_;

		private List<Event_Participation> event_Participations;

        RegisterButton registerButton;
        CancelButton cancelButton;

        Label eventNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "INSCRIÇÕES";
		}

        public async void initSpecificLayout()
		{
            event_Participations = await GetEventParticipationAll();

			CreatEventParticipationColletionView();

		}


		public void CreatEventParticipationColletionView()
		{

			foreach (Event_Participation event_Participation in event_Participations)
			{
				Debug.Print("event_Participation.estado=" + event_Participation.estado);
				if (event_Participation.estado == "inscrito")
				{
                    event_Participation.estadoTextColor = Colors.Green;
                    event_Participation.estadoText = "Inscrito";
                }
                if (event_Participation.estado == "nao_inscrito")
                {
                    event_Participation.estadoTextColor = Colors.Orange;
                    event_Participation.estadoText = "Não Inscrito";
                }
			}

			eventNameLabel = new Label
			{
                FontFamily = "futuracondensedmedium",
                Text = event_.name,
				BackgroundColor = Colors.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.bigTitleFontSize,
				TextColor = App.normalTextColor
			};

			absoluteLayout.Add(eventNameLabel);
            absoluteLayout.SetLayoutBounds(eventNameLabel, new Rect(0, 0, App.screenWidth, 60 * App.screenHeightAdapter));

			if (event_Participations.Count > 0)
			{
				nameTitleLabel = new Label
				{
                    FontFamily = "futuracondensedmedium",
                    Text = "NOME",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(nameTitleLabel);
                absoluteLayout.SetLayoutBounds(nameTitleLabel, new Rect(0, 50 * App.screenHeightAdapter, App.screenWidth / 3 * 2 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				categoryTitleLabel = new Label
				{
                    FontFamily = "futuracondensedmedium",
                    Text = "ESTADO",
					BackgroundColor = Colors.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = App.titleFontSize,
                    TextColor = App.topColor,
                    LineBreakMode = LineBreakMode.WordWrap
				};

				absoluteLayout.Add(categoryTitleLabel);
                absoluteLayout.SetLayoutBounds(categoryTitleLabel, new Rect(App.screenWidth / 3 * 2, 50 * App.screenHeightAdapter, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));
			}

            collectionViewEventParticipations = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = event_Participations,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não há inscritos neste evento.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Colors.Red, FontSize = 20 },
			}
					}
				}
			};

            //collectionViewCompetitionCall.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;

            collectionViewEventParticipations.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membername");
				

                Border nameFrame = new Border
				{
					BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemabsoluteLayout.Add(nameFrame);
				itemabsoluteLayout.SetLayoutBounds(nameFrame, new Rect(0, 0, App.screenWidth / 3 * 2 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				Label estadoabel = new Label { FontFamily = "futuracondensedmedium", BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formValueFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                estadoabel.SetBinding(Label.TextProperty, "estadoText");
                estadoabel.SetBinding(Label.TextColorProperty, "estadoTextColor");

                Border categoryFrame = new Border
				{
                    BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    Padding = new Thickness(5, 0, 0, 0)
				};
				categoryFrame.Content = estadoabel;

				itemabsoluteLayout.Add(categoryFrame);
		        itemabsoluteLayout.SetLayoutBounds(categoryFrame, new Rect((App.screenWidth / 3 * 2), 0, App.screenWidth / 3 - 10 * App.screenWidthAdapter, 40 * App.screenHeightAdapter));

				return itemabsoluteLayout;
			});
			absoluteLayout.Add(collectionViewEventParticipations);
            absoluteLayout.SetLayoutBounds(collectionViewEventParticipations, new Rect(0, 100 * App.screenHeightAdapter, App.screenWidth, App.screenHeight - 100 - 120 * App.screenHeightAdapter));
		}

		public EventParticipationsPageCS(Event event_)
		{
			this.event_ = event_;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Event_Participation>> GetEventParticipationAll()
		{
			Debug.WriteLine("AQUI 1 GetEventParticipationAll");
			EventManager eventManager = new EventManager();

			List<Event_Participation> event_Participations = await eventManager.GetEventParticipationAll(event_.id);
			if (event_Participations == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return event_Participations;
		}

    }
}
