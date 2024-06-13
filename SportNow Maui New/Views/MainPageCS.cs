using System;
using System.Collections.Generic;
using Microsoft.Maui;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using SportNow.ViewModel;
using Microsoft.Maui;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using SportNow.Views.Profile;
using SportNow.CustomViews;
using SportNow.Views.Personal;
using System.Xml;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Plugin.BetterFirebasePushNotification;

namespace SportNow.Views
{
	public class MainPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
            base.OnAppearing();
            if (gridMain == null)
			{
                initSpecificLayout();
            }
        }

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		public List<MainMenuItem> MainMenuItems { get; set; }

		Label msg;

		Grid gridMain, gridPersonal, gridTeacherClasses, gridClasses, gridEvents;

		private ObservableCollection<Class_Schedule> cleanClass_Schedule, importantClass_Schedule;

		private List<Class_Schedule> teacherClass_Schedules;

		private CollectionView importantClassesCollectionView;
		private CollectionView importantEventsCollectionView;
		private CollectionView teacherClassesCollectionView;

		ScheduleCollection scheduleCollection;

		Label usernameLabel, attendanceLabel, eventsLabel, teacherClassesLabel;
		Label currentFeeLabel;
		Label famousQuoteLabel;
		Label currentVersionLabel;

		private List<Event> importantEvents;
		private List<Competition> importantCompetitions;
		private List<Examination_Session> importantExaminationSessions;


		int classesY = 0;
		int eventsY = 0;
		int teacherClassesY = 0;
		int eventsHeight = 0;
        int personalClassesY = 0;
        int feesOrQuoteY = 0;

		RoundButton personalClassesButton;
		RoundButton technicalButton;

		Image websiteImage, grupoFacebookImage, whatsappImage, documentosImage, manualNKSKidsImage;
		Label websiteLabel, grupoFacebookLabel, whatsappLabel, documentosLabel, manualNKSKidsLabel;
		Grid gridLinks;

        public void CleanScreen()
		{
			//valida se os objetos já foram criados antes de os remover
			if (gridMain != null)
			{
				gridMain = null;
			}

            if (usernameLabel != null)
			{
				//absoluteLayout.Remove(usernameLabel);
				usernameLabel = null;
			}

			if (importantClassesCollectionView != null)
			{
				//absoluteLayout.Clear();
				

				importantClassesCollectionView = null;
				importantEventsCollectionView = null;
				attendanceLabel = null;
				eventsLabel = null;
			}
			if (teacherClassesCollectionView != null) {

				teacherClassesCollectionView = null;
				teacherClassesLabel = null;
			}
			if (currentFeeLabel != null)
			{
				currentFeeLabel = null;
			}
			if (currentVersionLabel != null)
			{
				currentVersionLabel = null;
			}

			if (famousQuoteLabel != null)
			{
				famousQuoteLabel = null;
			}

            if (gridLinks != null)
            {
                gridLinks = null;
            }
        }

        public void initLayout()
		{
			Title = "PRINCIPAL";

			ToolbarItem toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "perfil.png";

            /*if (App.member.members_to_approve.Count != 0)
			{
				toolbarItem.IconImageSource = "perfil.png";
            }
			else
            {
                toolbarItem.IconImageSource = "perfil.png";
            }*/

            toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);

        }


		public async void initSpecificLayout()
		{
			gridMain = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Colors.Transparent, WidthRequest = App.screenWidth, RowSpacing = 10 * App.screenHeightAdapter };
			gridMain.RowDefinitions.Add(new RowDefinition { Height = 50 * App.screenHeightAdapter });
			gridMain.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			var textWelcome = "";

			textWelcome = "Olá " + App.member.nickname + " ";

			//USERNAME LABEL
			usernameLabel = new Label
			{
				Text = textWelcome,
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.End,
				FontSize = App.titleFontSize,
				FontFamily = "futuracondensedmedium",
				WidthRequest = App.screenWidth,
			};
            //absoluteLayout.Add(usernameLabel);
            //absoluteLayout.SetLayoutBounds(usernameLabel, new Rect(App.screenWidth - 200 * App.screenWidthAdapter, 2 * App.screenHeightAdapter, 190 * App.screenWidthAdapter, 30 * App.screenHeightAdapter));

            gridMain.Add(usernameLabel, 0, 0);

            if (App.member.students_count > 0)
			{
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });


                /*                teacherClassesY = (int) (40 * App.screenHeightAdapter);
								classesY = (int) ((teacherClassesY + App.ItemHeight ) + (50 * App.screenHeightAdapter));
								eventsY = (int) ((classesY + App.ItemHeight) + (50 * App.screenHeightAdapter));
								eventsHeight = (int)(App.ItemHeight  + 10);
								feesOrQuoteY = (int)((eventsY + eventsHeight) + (50 * App.screenHeightAdapter));*/
            }
			else if (App.member.students_count == 0)
			{
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 150 * App.screenHeightAdapter });
                gridMain.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
                /*classesY = (int) (40 * App.screenHeightAdapter);
				eventsY = (int) ((classesY + App.ItemHeight) + (50 * App.screenHeightAdapter));
				//eventsHeight = (int)(2 * (App.ItemHeight  + 10));
                eventsHeight = (int) (App.ItemHeight + 10);
                personalClassesY = (int)((eventsY + eventsHeight) + (50 * App.screenHeightAdapter));
                feesOrQuoteY = (int) ((personalClassesY + 150) + (50 * App.screenHeightAdapter));*/
            }

            showActivityIndicator();

			int result = await getClass_DetailData();
			importantEvents = await GetImportantEvents();

			DateTime currentTime = DateTime.Now.Date;
			DateTime currentTime_add7 = DateTime.Now.AddDays(7).Date;

			string firstDay = currentTime.ToString("yyyy-MM-dd");
			string lastday = currentTime_add7.AddDays(6).ToString("yyyy-MM-dd");

			teacherClass_Schedules = await GetAllClass_Schedules(firstDay, lastday);
			CompleteTeacherClass_Schedules();

			if (App.member.currentFee == null)
			{
				Debug.Print("Current Fee NULL não devia acontecer!");
				if (App.member.currentFee == null)
				{
					Debug.Print("Current Fee NULL não devia acontecer!");
					int result1 = await GetCurrentFees(App.member);
				}
				await GetCurrentFees(App.member);
			}


			hideActivityIndicator();

			createImportantClasses();
			Debug.Print("App.member.students_count = " + App.member.students_count);
			if (App.member.students_count > 0)
			{
				createImportantTeacherClasses();
				createImportantEvents();
                gridMain.Add(gridTeacherClasses, 0, 1);
                gridMain.Add(gridClasses, 0, 2);
                gridMain.Add(gridEvents, 0, 3);

            }
			else
			{
                createImportantEvents();
                createPersonalClasses();
                gridMain.Add(gridClasses, 0, 1);
                gridMain.Add(gridEvents, 0, 2);
                gridMain.Add(gridPersonal, 0, 3);
            }
            


            /*technicalButton = new RoundButton("Tecnico", 100, 40);
            technicalButton.button.BackgroundColor = App.topColor;
            technicalButton.button.Clicked += OnTechnicalButtonClicked;
			*/

            createCurrentFee();

            createDelayedMonthFee();

            //createVersion();


			
            //gridMain.Add(usernameLabel, 0, 0);


            absoluteLayout.Add(gridMain);
            absoluteLayout.SetLayoutBounds(gridMain, new Rect(0, 0, App.screenWidth, App.screenHeight - 132 * App.screenHeightAdapter));

        }

        public async void createPersonalClasses()
		{

            gridPersonal = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
            gridPersonal.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPersonal.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridPersonal.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });


            Label personalClassesLabel = new Label
			{
				Text = "SABIAS QUE AGORA JÁ PODES MARCAR AULAS INDIVIDUAIS COM OS TEUS TREINADORES FAVORITOS!",
				TextColor = Color.FromRgb(96, 182, 89),
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };



  //          absoluteLayout.Add(personalClassesLabel);
  //          absoluteLayout.SetLayoutBounds(personalClassesLabel, new Rect(0, personalClassesY, App.screenWidth, 60 * App.screenHeightAdapter));

            personalClassesButton = new RoundButton("SABER MAIS!", App.screenWidth, 50 * App.screenHeightAdapter);
			personalClassesButton.button.BackgroundColor = App.topColor;
            personalClassesButton.button.Clicked += OnPersonalClassesButtonClicked;

            gridPersonal.Add(personalClassesLabel, 0, 0);
            gridPersonal.Add(personalClassesButton, 0, 1);

//			absoluteLayout.Add(personalClassesButton);
//            absoluteLayout.SetLayoutBounds(personalClassesButton, new Rect(0, personalClassesY + (60 * App.screenHeightAdapter), App.screenWidth, 60 * App.screenHeightAdapter));
            
        }
            

        public void createImportantTeacherClasses()
		{
            gridTeacherClasses = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
            gridTeacherClasses.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridTeacherClasses.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridTeacherClasses.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            //AULAS LABEL
            teacherClassesLabel = new Label
			{
				Text = "PRÓXIMAS AULAS COMO INSTRUTOR/MONITOR",
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Start,
				FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            gridTeacherClasses.Add(teacherClassesLabel, 0, 0);

            //absoluteLayout.Add(teacherClassesLabel);
            //absoluteLayout.SetLayoutBounds(teacherClassesLabel, new Rect(0, teacherClassesY, App.screenWidth, 30 * App.screenHeightAdapter));

			CreateTeacherClassesColletion();
		}

		public void CompleteTeacherClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in teacherClass_Schedules)
			{
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;

				if (class_schedule.imagesource == null)
				{
					class_schedule.imagesourceObject = "company_logo_square.png";
				}
				else
				{
					class_schedule.imagesourceObject = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + class_schedule.classid + "_imagem_c"),
						CachingEnabled = false,
						CacheValidity = new TimeSpan(0, 0, 0, 0)
					};
				}
			}
		}


		public void CreateTeacherClassesColletion()
		{
			//COLLECTION TEACHER CLASSES
			teacherClassesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = teacherClass_Schedules,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			teacherClassesCollectionView.SelectionChanged += OnTeacherClassesCollectionViewSelectionChanged;

			teacherClassesCollectionView.ItemTemplate = new DataTemplate(() =>
			{

				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = App.ItemHeight ,
					WidthRequest = App.ItemWidth
				};

				Debug.Print("App.ItemHeight  = " + (App.ItemHeight  - 10) * App.screenHeightAdapter);

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
                    WidthRequest = App.ItemWidth,
                    VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

                /*var itemFrame_tap = new TapGestureRecognizer();
				itemFrame_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new EquipamentsPageCS("protecoescintos"));
				};
				itemFrame.GestureRecognizers.Add(itemFrame_tap);*/

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
				dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");


                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");


                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

				return itemabsoluteLayout;
			});

            gridTeacherClasses.Add(teacherClassesCollectionView, 0, 1);

//            absoluteLayout.Add(teacherClassesCollectionView);
//            absoluteLayout.SetLayoutBounds(teacherClassesCollectionView, new Rect(0, teacherClassesY + (30 * App.screenHeightAdapter), App.screenWidth, App.ItemHeight + (10 * App.screenHeightAdapter)));
		}

		public void createImportantClasses()
		{

            gridClasses = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
            gridClasses.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridClasses.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridClasses.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star});

            //AULAS LABEL
            attendanceLabel = new Label
			{
				Text = "PRÓXIMAS AULAS COMO ALUNO(A)",
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            gridClasses.Add(attendanceLabel, 0, 0);

            //absoluteLayout.Add(attendanceLabel);
			//absoluteLayout.SetLayoutBounds(attendanceLabel, new Rect(0, classesY, App.screenWidth, 30 * App.screenHeightAdapter));

			scheduleCollection = new ScheduleCollection();
			scheduleCollection.Items = importantClass_Schedule;
			createClassesCollection();
		}

		public async Task<int> getClass_DetailData()
		{
			DateTime currentTime = DateTime.Now.Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

			importantClass_Schedule = await GetStudentClass_Schedules(currentTime.ToString("yyyy-MM-dd"), currentTime.AddDays(7).ToString("yyyy-MM-dd"));//  new List<Class_Schedule>();
			cleanClass_Schedule = new ObservableCollection<Class_Schedule>();

			CompleteClass_Schedules();

			return 1;
		}


		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in importantClass_Schedule)
			{
				/*if (class_schedule.classattendancestatus == "confirmada")
				{
					class_schedule.participationimage = "iconcheck.png";
				}*/
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;
				if (class_schedule.imagesource == null)
				{
					class_schedule.imagesourceObject = "company_logo_square.png";
				}
				else
				{
					class_schedule.imagesourceObject = new UriImageSource
					{
							Uri = new Uri(Constants.images_URL + class_schedule.classid + "_imagem_c"),
							CachingEnabled = false,
							CacheValidity = new TimeSpan(0, 0, 0, 0)
					};
				}
				

				if ((class_schedule.classattendancestatus == "confirmada") | (class_schedule.classattendancestatus == "fechada"))
				{
					class_schedule.participationimage = "iconcheck.png";
				}
				else
				{
					class_schedule.participationimage = "iconinativo.png";
				}

			}

		}

		public void createClassesCollection()
		{
			importantClassesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				//ItemsSource = importantClass_Schedule,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem aulas agendadas esta semana.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium",FontSize = App.itemTitleFontSize },
							}
					}
				}
			};
			this.BindingContext = scheduleCollection;
			importantClassesCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Items");


			importantClassesCollectionView.SelectionChanged += OnClassScheduleCollectionViewSelectionChanged;

			importantClassesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth,
				};

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,// -(10 * App.screenHeightAdapter),
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image {
					Aspect = Aspect.AspectFill,
					Opacity = 0.40,
				};
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

				itemabsoluteLayout.Add(itemFrame);
				itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

				Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "datestring");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(25 * App.screenWidthAdapter, App.ItemHeight - (45 * App.screenHeightAdapter), App.ItemWidth - (50 * App.screenWidthAdapter), 40 * App.screenHeightAdapter));


                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));

				return itemabsoluteLayout;
			});

            gridClasses.Add(importantClassesCollectionView, 0, 1);

            //absoluteLayout.Add(importantClassesCollectionView);
            //absoluteLayout.SetLayoutBounds(importantClassesCollectionView, new Rect(0, classesY + (30 * App.screenHeightAdapter), App.screenWidth, App.ItemHeight + (10 * App.screenHeightAdapter)));
		}

		public void createImportantEvents()
		{

            gridEvents = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
            gridEvents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridEvents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridEvents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });


            foreach (Event event_i in importantEvents)
			{
				if ((event_i.imagemNome == "") | (event_i.imagemNome is null))
				{
					event_i.imagemSource = "company_logo_square.png";
				}
				else
				{
					event_i.imagemSource = Constants.images_URL + event_i.id + "_imagem_c";

				}

				if ((event_i.participationconfirmed == "inscrito") | (event_i.participationconfirmed == "confirmado"))
				{
					event_i.participationimage = "iconcheck.png";
				}
                else if (event_i.participationconfirmed == "cancelado")
                {
                    event_i.participationimage = "iconinativo.png";
                }
            }

			//AULAS LABEL
			eventsLabel = new Label
			{
				Text = "PRÓXIMOS EVENTOS",
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.End,
				FontSize = App.titleFontSize,
                FontFamily = "futuracondensedmedium",
            };

            gridEvents.Add(eventsLabel, 0, 0);

			CreateProximosEventosColletion();
		}

		public void CreateProximosEventosColletion()
		{
			int gridLines = 1; //estava 2


            //COLLECTION EVENTOS IMPORTANTES
            importantEventsCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = importantEvents,
				ItemsLayout = new GridItemsLayout(gridLines, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new Microsoft.Maui.Controls.StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Eventos agendados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = App.normalTextColor, FontFamily = "futuracondensedmedium", FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			importantEventsCollectionView.SelectionChanged += OnProximosEventosCollectionViewSelectionChanged;

			importantEventsCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				AbsoluteLayout itemabsoluteLayout = new AbsoluteLayout
				{
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth
				};

                Border itemFrame = new Border
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 5 * (float)App.screenHeightAdapter,
                    },
                    Stroke = App.topColor,
                    BackgroundColor = App.backgroundOppositeColor,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = (App.ItemHeight),
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.40 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemFrame.Content = eventoImage;

                itemabsoluteLayout.Add(itemFrame);
                itemabsoluteLayout.SetLayoutBounds(itemFrame, new Rect(0, 0, App.ItemWidth, App.ItemHeight));

                Label nameLabel = new Label { BackgroundColor = Colors.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "name");

                itemabsoluteLayout.Add(nameLabel);
                itemabsoluteLayout.SetLayoutBounds(nameLabel, new Rect(3 * App.screenWidthAdapter, 25 * App.screenHeightAdapter, App.ItemWidth - (6 * App.screenWidthAdapter), 50 * App.screenHeightAdapter));

                Label categoryLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                categoryLabel.SetBinding(Label.TextProperty, "category");

                itemabsoluteLayout.Add(categoryLabel);
                itemabsoluteLayout.SetLayoutBounds(categoryLabel, new Rect(3 * App.screenWidthAdapter, ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2), App.ItemWidth - (6 * App.screenWidthAdapter), (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));

                Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.oppositeTextColor, FontFamily = "futuracondensedmedium", LineBreakMode = LineBreakMode.WordWrap };
                dateLabel.SetBinding(Label.TextProperty, "detailed_date");

                itemabsoluteLayout.Add(dateLabel);
                itemabsoluteLayout.SetLayoutBounds(dateLabel, new Rect(3 * App.screenWidthAdapter, (App.ItemHeight - 15) - ((App.ItemHeight - 15) / 4), App.ItemWidth, (App.ItemHeight - (15 * App.screenHeightAdapter)) / 4));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

                itemabsoluteLayout.Add(participationImagem);
                itemabsoluteLayout.SetLayoutBounds(participationImagem, new Rect((App.ItemWidth - 25 * App.screenWidthAdapter), 5 * App.screenWidthAdapter, 20 * App.screenWidthAdapter, 20 * App.screenWidthAdapter));


                return itemabsoluteLayout;
			});

            gridEvents.Add(importantEventsCollectionView, 0, 1);

            //absoluteLayout.Add(importantEventsCollectionView);
            //absoluteLayout.SetLayoutBounds(importantEventsCollectionView, new Rect(0, eventsY + (35 * App.screenHeightAdapter), App.screenWidth, eventsHeight));
		}




		public void createLinks()
		{
            gridLinks = new Microsoft.Maui.Controls.Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand, RowSpacing = 5 * App.screenHeightAdapter, WidthRequest = App.screenWidth};
            gridLinks.RowDefinitions.Add(new RowDefinition { Height = 35 * App.screenHeightAdapter });
            gridLinks.RowDefinitions.Add(new RowDefinition { Height = 35 * App.screenHeightAdapter });
            //gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
            gridLinks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
            gridLinks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
            gridLinks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
            gridLinks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
            gridLinks.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            grupoFacebookImage = new Image
            {
                Source = "facebook.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 35 * App.screenHeightAdapter,
                WidthRequest = 35 * App.screenHeightAdapter
            };

            TapGestureRecognizer grupoFacebook_tapEvent = new TapGestureRecognizer();
            grupoFacebook_tapEvent.Tapped += grupoFacebookImage_Clicked;
            grupoFacebookImage.GestureRecognizers.Add(grupoFacebook_tapEvent);

            grupoFacebookLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Grupo Facebook NKS",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };
            grupoFacebookLabel.GestureRecognizers.Add(grupoFacebook_tapEvent);


            gridLinks.Add(grupoFacebookImage, 0, 0);
            gridLinks.Add(grupoFacebookLabel, 0, 1);

            websiteImage = new Image
            {
                Source = "www.png",
                Aspect = Aspect.AspectFit,
				HeightRequest = 35 *App.screenHeightAdapter,
                WidthRequest = 35 * App.screenHeightAdapter
            };

            TapGestureRecognizer website_tapEvent = new TapGestureRecognizer();
            website_tapEvent.Tapped += websiteImage_Clicked;
            websiteImage.GestureRecognizers.Add(website_tapEvent);



            websiteLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Website NKS",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };
            websiteLabel.GestureRecognizers.Add(website_tapEvent);

            gridLinks.Add(websiteImage, 1, 0);
            gridLinks.Add(websiteLabel, 1, 1);

            whatsappImage = new Image
            {
                Source = "whatapp.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 35 * App.screenHeightAdapter,
                WidthRequest = 35 * App.screenHeightAdapter
            };

            TapGestureRecognizer whatsapp_tapEvent = new TapGestureRecognizer();
            whatsapp_tapEvent.Tapped += WhatsAppImage_Clicked;
            whatsappImage.GestureRecognizers.Add(whatsapp_tapEvent);

            whatsappLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "WhatsApp NKS",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };
            whatsappLabel.GestureRecognizers.Add(whatsapp_tapEvent);


            gridLinks.Add(whatsappImage, 2, 0);
            gridLinks.Add(whatsappLabel, 2, 1);

            documentosImage = new Image
            {
                Source = "documentos.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 35 * App.screenHeightAdapter,
                WidthRequest = 35 * App.screenHeightAdapter
            };

            TapGestureRecognizer documentos_tapEvent = new TapGestureRecognizer();
            documentos_tapEvent.Tapped += documentosImage_Clicked;
            documentosImage.GestureRecognizers.Add(documentos_tapEvent);

            documentosLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Documentos NKS",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };
            documentosLabel.GestureRecognizers.Add(documentos_tapEvent);


            gridLinks.Add(documentosImage, 3, 0);
            gridLinks.Add(documentosLabel, 3, 1);

		 	manualNKSKidsImage= new Image
            {
                Source = "manual_nks_kids.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 35 * App.screenHeightAdapter,
                WidthRequest = 35 * App.screenHeightAdapter
            };

            TapGestureRecognizer manualNKSKids_tapEvent = new TapGestureRecognizer();
            manualNKSKids_tapEvent.Tapped += manualNKSKidsImage_Clicked;
            manualNKSKidsImage.GestureRecognizers.Add(manualNKSKids_tapEvent);

            manualNKSKidsLabel = new Label
            {
                FontFamily = "futuracondensedmedium",
                Text = "Manual NKS Kids",
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = App.itemTextFontSize
            };
            manualNKSKidsLabel.GestureRecognizers.Add(manualNKSKids_tapEvent);


            gridLinks.Add(manualNKSKidsImage, 4, 0);
            gridLinks.Add(manualNKSKidsLabel, 4, 1);

            //absoluteLayout.Add(gridLinks);
            //absoluteLayout.SetLayoutBounds(gridLinks, new Rect(0, App.screenHeight - 110 - (125 * App.screenHeightAdapter), App.screenWidth, 75 * App.screenHeightAdapter));
            //absoluteLayout.SetLayoutBounds(gridLinks, new Rect(0, feesOrQuoteY, App.screenWidth, 75 * App.screenHeightAdapter));
            

            /*absoluteLayout.Add(websiteImage);
            absoluteLayout.SetLayoutBounds(websiteImage, new Rect(App.screenWidth / 2 + 47.5 * App.screenWidthAdapter, feesOrQuoteY, 35 * App.screenHeightAdapter, 35 * App.screenHeightAdapter));

            absoluteLayout.Add(websiteLabel);
            absoluteLayout.SetLayoutBounds(websiteLabel, new Rect(App.screenWidth / 2 + 40 * App.screenWidthAdapter, feesOrQuoteY + 35 * App.screenHeightAdapter, 60 * App.screenWidthAdapter, 20 * App.screenHeightAdapter));
			*/
        }

        async void grupoFacebookImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://www.facebook.com/groups/karatesangalhos", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
        }

        async void websiteImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://karatesangalhos.pt/", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
        }

        async void WhatsAppImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://chat.whatsapp.com/LQa034akZ2p1WIfcWNnR7n", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
        }

        async void documentosImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://nks-server.synology.me:5011/d/s/xzxkmvCFKAV4v9B4829wBMzIYg28Crdd/3L8uf_sjAZuCFk2dH90A5ofBL25sRKyF-qLdA0X6TRQs", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
        }

        async void manualNKSKidsImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("https://karatesangalhos.pt/files/MANUAL_NKS_KIDS%20_21_Nov_2023.pdf", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
            }
        }

        public async void createCurrentFee()
		{

			bool hasQuotaPayed = false;

			if (App.member.currentFee != null)
			{
				if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
				{
					hasQuotaPayed = true;
					createLinks();
                    gridMain.Add(gridLinks, 0, 4);
                    return;
				}
			}

			if (hasQuotaPayed == false)
            {

                bool answer = await DisplayAlert("A TUA QUOTA NÃO ESTÁ ATIVA.", "A tua quota para este ano não está ativa. Queres efetuar o pagamento?", "Sim", "Não");
                Debug.WriteLine("Answer: " + answer);
				if (answer == true)
				{
                    await Navigation.PushAsync(new QuotasPageCS());
                }
				else
				{
                    currentFeeLabel = new Label
                    {
                        Text = "A TUA QUOTA PARA ESTE ANO NÃO ESTÁ ATIVA. \n DESTA FORMA NÃO PODERÁS PARTICIPAR NOS NOSSOS EVENTOS :(. \n ATIVA AQUI A TUA QUOTA.",
                        TextColor = Colors.Red,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = App.itemTextFontSize,
                        FontFamily = "futuracondensedmedium",
                    };

                    var currentFeeLabel_tap = new TapGestureRecognizer();
                    currentFeeLabel_tap.Tapped += async (s, e) =>
                    {
                        await Navigation.PushAsync(new QuotasPageCS());
                    };
                    currentFeeLabel.GestureRecognizers.Add(currentFeeLabel_tap);

                    gridMain.Add(currentFeeLabel, 0, 4);
                }
                

			}
		}

        public async void createDelayedMonthFee()
        {

            string delayedMonthFeeCount = await Get_Has_DelayedMonthFees();
            if (delayedMonthFeeCount == "1")
            {
                bool answer = await DisplayAlert("MENSALIDADE A PAGAMENTO", "Tem uma mensalidade em pagamento. Para proceder ao pagamento, clique em 'Pagar'", "Pagar", "Mais tarde");
                if (answer == true)
                {
                    await Navigation.PushAsync(new MonthFeeStudentListPageCS());
                }

            }
            else if (delayedMonthFeeCount != "0")
            {
                bool answer = await DisplayAlert("MENSALIDADES A PAGAMENTO", "Tem mensalidades em pagamento. Para proceder ao pagamento, clique em 'Pagar'", "Pagar", "Mais tarde");
                if (answer == true)
                {
                    await Navigation.PushAsync(new MonthFeeStudentListPageCS());
                }
            }
        }

        public async void createVersion()
		{
			currentVersionLabel = new Label
			{
				Text = "Version 1.2(23)",
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.End,
				FontSize = 10
			};

			absoluteLayout.Add(currentVersionLabel);
			absoluteLayout.SetLayoutBounds(currentVersionLabel, new Rect(0, feesOrQuoteY + 90 * App.screenHeightAdapter, App.screenWidth, 30 * App.screenHeightAdapter));
		}
		

		public MainPageCS ()
		{

			this.initLayout();
			//this.initSpecificLayout(App.members);

		}

		void OnSendClick(object sender, EventArgs e)
		{
			/*notificationNumber++;
			string title = $"Local Notification #{notificationNumber}";
			string $"You have now received {notificationNumber} notifications!";
			notificationManager.SendNotification(title, message);*/
		}

		void OnScheduleClick(object sender, EventArgs e)
		{
			/*notificationNumber++;
			string title = $"Local Notification #{notificationNumber}";
			string $"You have now received {notificationNumber} notifications!";
			notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(10));*/
		}

		void ShowNotification(string title, string message)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				msg.Text = $"Notification Received:\nTitle: {title}\nMessage: {message}";
			});
		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfileCS());
		}

		async Task<ObservableCollection<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
		{
			Debug.WriteLine("GetStudentClass_Schedules");
			ClassManager classManager = new ClassManager();
			ObservableCollection<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules_obs(App.member.id, begindate, enddate);
			if (class_schedules_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return class_schedules_i;
		}

		async Task<List<Event>> GetImportantEvents()
		{
			Debug.WriteLine("GetImportantEvents");
			EventManager eventManager = new EventManager();
			List<Event> events = await eventManager.GetImportantEvents(App.member.id);
			if (events == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return events;
		}

		async Task<int> GetCurrentFees(Member member)
		{
			Debug.WriteLine("MainTabbedPageCS.GetCurrentFees");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetCurrentFees(member);
			if (result == -1)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return result;
			}

			return result;
		}

		async Task<List<Class_Schedule>> GetAllClass_Schedules(string begindate, string enddate)
		{
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetAllClass_Schedules(App.member.id, begindate, enddate);

			if (class_schedules_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = App.backgroundColor,
					BarTextColor = App.normalTextColor
				};
				return null;
			}
			return class_schedules_i;
		}


        async Task<string> Get_Has_DelayedMonthFees()
        {
            Debug.WriteLine("Get_Has_DelayedMonthFees");
            MonthFeeManager monthFeeManager = new MonthFeeManager();
            string count = await monthFeeManager.Get_Has_DelayedMonthFees(App.member.id);
            if (count == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = App.backgroundColor,
                    BarTextColor = App.normalTextColor
                };
                return null;
            }
            return count;
        }

        async void OnClassScheduleCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();

			Debug.WriteLine("MainPageCS.OnClassScheduleCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItems.Count != 0)
			{
				ClassManager classmanager = new ClassManager();

				Class_Schedule class_schedule = (sender as CollectionView).SelectedItems[0] as Class_Schedule;
				if (class_schedule.classattendanceid == null)
				{
                    string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                    class_schedule.classattendanceid = class_attendance_id;
					/*
                    Task.Run(async () =>
                    {
                        string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                        class_schedule.classattendanceid = class_attendance_id;
                        return true;
                    });*/
                    //string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                    /*                    string class_attendance_id =  classmanager.CreateClass_Attendance_sync(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
                                        */
                    class_schedule.classattendancestatus = "confirmada";
                    class_schedule.participationimage = "iconcheck.png";


                }
                else
				{
					if (class_schedule.classattendancestatus == "anulada")
					{
						class_schedule.classattendancestatus = "confirmada";
						class_schedule.participationimage = "iconcheck.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "confirmada")
					{
						class_schedule.classattendancestatus = "anulada";
						class_schedule.participationimage = "iconinativo.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "fechada")
					{
						await DisplayAlert("PRESENÇA EM AULA", "A tua presença nesta aula já foi validada pelo instrutor pelo que não é possível alterar o seu estado.", "Ok" );
					}
					//int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
				}

				((CollectionView)sender).SelectedItems.Clear();
				/*importantClassesCollectionView.ItemsSource = cleanClass_Schedule;
				importantClassesCollectionView.ItemsSource = importantClass_Schedule;*/

				hideActivityIndicator();
			}
		}

		async void OnProximosEventosCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewProximosEstagiosSelectionChanged " + (sender as CollectionView).SelectedItem.GetType().ToString());

			if ((sender as CollectionView).SelectedItem != null)
			{
				Event event_v = (sender as CollectionView).SelectedItem as Event;

				if (event_v.type == "estagio")
				{
					await Navigation.PushAsync(new DetailEventPageCS(event_v));
				}
				else if (event_v.type == "competicao")
				{

					if (event_v.participationid == null)
					{
                        await Navigation.PushAsync(new DetailCompetitionPageCS(event_v.id, event_v.name));
                    }
					else
					{
                        await Navigation.PushAsync(new DetailCompetitionPageCS(event_v.id, event_v.name, event_v.participationid));
                    }
					
				}
				else if (event_v.type == "sessaoexame")
				{
					await Navigation.PushAsync(new ExaminationSessionPageCS(event_v.id));
				}

			}
		}

		async void OnTeacherClassesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("MainPageCS.OnClassAttendanceCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Class_Schedule class_schedule = (sender as CollectionView).SelectedItem as Class_Schedule;
				(sender as CollectionView).SelectedItem = null;
				await Navigation.PushAsync(new AttendanceClassPageCS(class_schedule));

			}
		}

        async void OnPersonalClassesButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MainPageCS.OnPersonalClassesButtonClicked");
            await Navigation.PushAsync(new PersonalInfoPageCS());
        }

        async void OnTechnicalButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("MainPageCS.OnTechnicalButtonClicked");
            await Navigation.PushAsync(new AttendanteEvaluationPageCS("6c36406a-7ab5-fcb0-b186-64d243523b11"));
        }
    }
}
