﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui;

namespace SportNow.CustomViews
{
    
     public class FormValueEditPicker : Frame
     {

         public Picker picker;
         //public string Text {get; set; }

         public FormValueEditPicker(string selectedValue, List<string> valueList) {

            this.CornerRadius = 5 * (float) App.screenHeightAdapter;
            this.IsClippedToBounds = true;
            BorderColor = App.bottomColor;
            BackgroundColor = Colors.Transparent;
            this.Padding = new Thickness(1, 2, 2, 2);
            //this.MinimumHeightRequest = 50;
            this.HeightRequest = 45 * App.screenHeightAdapter;
            this.VerticalOptions = LayoutOptions.Center;
            this.HasShadow = false;

            int selectedIndex_temp = 0;
            int selectedIndex = 0;
            foreach (string value in valueList)
            {
                Debug.Print("selectedValue = " + selectedValue + " value = " + value);
                if (value == selectedValue)
                {
                    selectedIndex = selectedIndex_temp;
                }
                selectedIndex_temp++;
            }

            picker = new Picker
            {
                Title = "",
                TitleColor = Colors.White,
                BackgroundColor = Colors.Transparent,
                TextColor = App.normalTextColor,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = App.formValueFontSize,
                FontFamily = "futuracondensedmedium",
            };
            picker.ItemsSource = valueList;
            picker.SelectedIndex = selectedIndex;
            
            this.Content = picker; // relativeLayout_Button;
        }
     }
}
