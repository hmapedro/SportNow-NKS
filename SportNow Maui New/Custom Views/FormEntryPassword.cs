using System;
using Microsoft.Maui;

namespace SportNow.CustomViews
{
    public class FormEntryPassword: Frame
    {

        public Entry entry;
        //public string Text {get; set; }


        public FormEntryPassword(string Text, string placeholder, double width)
        {
            createFormEntry(Text, placeholder, width);
            this.WidthRequest = width;

        }

        public FormEntryPassword(string Text, string placeholder)
        {
            createFormEntry(Text, placeholder, 0);

        }

        public void createFormEntry(string Text, string placeholder, double width)
        {

            this.BackgroundColor = App.backgroundColor;
            this.BorderColor = App.bottomColor;

            this.CornerRadius = 10;
            this.IsClippedToBounds = true;
            this.Padding = new Thickness(2, 2, 2, 2);
            //this.WidthRequest = 300;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.HasShadow = false;

            //USERNAME ENTRY
            entry = new Entry
            {
                //Text = "tete@hotmail.com",
                Text = Text,
                IsPassword = true,
                TextColor = App.normalTextColor,
                BackgroundColor = App.backgroundColor,
                Placeholder = placeholder,
                PlaceholderColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Start,
                //WidthRequest = 300 * App.screenWidthAdapter,
                FontSize = App.formValueFontSize,
                FontFamily = "futuracondensedmedium",
                
            };
            if (width != 0)
            {
                entry.WidthRequest = width;
            }


            this.Content = entry;

        }
    }
}
