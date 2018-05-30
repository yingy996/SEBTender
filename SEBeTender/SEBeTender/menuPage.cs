using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{//This page contains the listview for menu
	public class menuPage : ContentPage
	{
        public ListView menu { get; set; }
		public menuPage ()
		{            
            Title = "Sarawak Energy e-Tender";
            Icon = "menuicon.png";
            //BackgroundColor = Color.FromHex("4295d1");
            BackgroundColor = Color.FromHex("#3C83B8");
            Padding = new Thickness(10);
            menu = new menuListView();

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 15, 0, 15),
                Content = new Label
                {
                    //TextColor = Color.FromHex("2C4D66"),
                    TextColor = Color.White,
                    
                    Text = "Sarawak Energy e-Tender",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18
                }
            };

            var separator = new BoxView
            {
                Margin = new Thickness(0, 0, 0, 10),
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = Color.White
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            layout.Children.Add(menuLabel);
            layout.Children.Add(separator);
            layout.Children.Add(menu);

            Content = layout;
		}
	}
}