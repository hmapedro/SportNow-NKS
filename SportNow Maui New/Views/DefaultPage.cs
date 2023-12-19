using System;
using System.Diagnostics;
using Microsoft.Maui;

namespace SportNow.Views
{
    public class DefaultPage : ContentPage
	{
        Microsoft.Maui.Controls.StackLayout stack;
        ActivityIndicator indicator;
        Image loading;

        public DefaultPage()
        {
			this.initBaseLayout();
		}

		public AbsoluteLayout absoluteLayout;

		public void initBaseLayout()
		{
            
			this.BackgroundColor = App.backgroundColor;

            absoluteLayout = new AbsoluteLayout
            {
				Margin = new Thickness(5 * App.screenWidthAdapter)
			};
			Content = absoluteLayout;

			NavigationPage.SetBackButtonTitle(this, "");


            if (Application.Current.MainPage != null)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = App.backgroundColor;
                navigationPage.BarTextColor = App.normalTextColor;
            }

            stack = new Microsoft.Maui.Controls.StackLayout() { BackgroundColor = App.backgroundColor, Opacity = 0.6 };
            loading = new Image() { Source = "loading.gif", IsAnimationPlaying = true }; 

            //indicator = new ActivityIndicator() { Color = App.topColor, HeightRequest = 100, WidthRequest = 100, MinimumHeightRequest = 100, MinimumWidthRequest = 100};
        }

        public void showActivityIndicator()
        {
            //indicator.IsRunning = true;

            /*if (absoluteLayout == null)
            {
                initBaseLayout();
            }*/

            absoluteLayout.Add(stack);
            absoluteLayout.SetLayoutBounds(stack, new Rect(0, 0, App.screenWidth, App.screenHeight));

            absoluteLayout.Add(loading);
            absoluteLayout.SetLayoutBounds(loading, new Rect((App.screenWidth / 2) - 50, (App.screenHeight / 2) - 100, 100, 100));
        }

        public void hideActivityIndicator()
        {
            absoluteLayout.Remove(stack);
            absoluteLayout.Remove(loading);
            //indicator.IsRunning = false;
        }
    }
}
