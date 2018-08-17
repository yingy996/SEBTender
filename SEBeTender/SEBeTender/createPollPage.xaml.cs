using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class createPollPage : ContentPage
	{
        string selectedOption = "";
        int currentOptionCount = 1;
        List<object> optionList = new List<object>();

        public createPollPage ()
		{
			InitializeComponent ();
            var optionNumberList = new List<string>();
            for(var i = 2; i < 21; i++)
            {
                optionNumberList.Add(i.ToString());
            }

            pollOptionNoPicker.ItemsSource = optionNumberList;
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            selectedOption = pollOptionNoPicker.Items[pollOptionNoPicker.SelectedIndex];
            int selectedOptionCount = Convert.ToInt32(selectedOption);

            if (selectedOptionCount < currentOptionCount)
            {
                int numberDifference = currentOptionCount - (selectedOptionCount + 1);

                for(var i = 0; i < numberDifference; i++)
                {
                    var lastChild = optionListLayout.Children.Last();
                    optionListLayout.Children.Remove(lastChild);
                }
                currentOptionCount = selectedOptionCount + 1;
            } else
            {
                //if it is the first time user select the number of option or when the new selected option is bigger than the previous selection
                for (var i = currentOptionCount; i <= selectedOptionCount; i++)
                {
                    StackLayout stackLayout = new StackLayout();
                    Label label = new Label();
                    label.Text = "Option " + i.ToString() + ": ";
                    label.FontAttributes = FontAttributes.Bold;

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    Entry entry = new Entry();

                    frame.Content = entry;
                    stackLayout.Children.Add(label);
                    stackLayout.Children.Add(frame);
                    optionListLayout.Children.Add(stackLayout);
                    optionList.Add(stackLayout);
                }
                currentOptionCount = selectedOptionCount + 1;
            }

            Console.WriteLine("Selected option: " + selectedOption);
        }

        async void OnCreateButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Created");
            //await Navigation.PushAsync(new createPollPage());
        }
    }
}