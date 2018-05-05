using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SEBeTender
{
	public class menuListView : ListView
	{
		public menuListView ()
		{
            List<menuItem> data = new menuData();
            ItemsSource = data;
            
            BackgroundColor = Color.FromHex("#3C83B8");
            VerticalOptions = LayoutOptions.FillAndExpand;
            this.SeparatorVisibility = SeparatorVisibility.None;
            ItemTemplate = new DataTemplate(() => {
                var label = new Label { VerticalOptions = LayoutOptions.Center, TextColor = Color.White, Margin = new Thickness(5) };
                label.SetBinding(Label.TextProperty, "Title");
                //717D7E
                return new ViewCell { View = label };
            });
            
		}
	}
}