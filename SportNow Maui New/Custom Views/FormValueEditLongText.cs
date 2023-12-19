﻿using System;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui;

namespace SportNow.CustomViews
{

    public class FormValueEditLongText : Frame
    {

        public Editor entry;
        //public string Text {get; set; }

        public FormValueEditLongText(string Text)
        {
            createFormValueEdit(Text, Keyboard.Text, 80);
        }

        public FormValueEditLongText(string Text, Keyboard keyboard, int height)
        {
            createFormValueEdit(Text, keyboard, height);
        }

        public void createFormValueEdit(string Text, Keyboard keyboard, int height)
        {
            this.CornerRadius = 5 * (float)App.screenHeightAdapter;
            this.IsClippedToBounds = true;
            BorderColor = App.topColor;
            BackgroundColor = Colors.Transparent;
            this.Padding = new Thickness(1, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = height * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            this.HasShadow = false;
            
            entry = new Editor
            {
                //Padding = new Thickness(5,0,5,0),
                Text = Text,
                //VerticalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                BackgroundColor = Colors.Transparent,
                FontSize = App.formValueFontSize,
                Keyboard = keyboard,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IsTextPredictionEnabled = true,
                FontFamily = "futuracondensedmedium",

                //HeightRequest = 30
            };

            this.Content = entry; // relativeLayout_Button;

        }
    }
}