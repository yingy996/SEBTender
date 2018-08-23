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
                }
                currentOptionCount = selectedOptionCount + 1;
            }

            Console.WriteLine("Selected option: " + selectedOption);
            Console.WriteLine("Layout children: " + optionListLayout.Children.Count());

            
        }

        async void OnPublishButtonClicked(object sender, EventArgs e)
        {
            bool isErrorPresent = false;
            string errorMessage = "";
            string pollQuestion = "";
            //Validate inputs
            if (pollQuestionInput.Text == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter poll question. ";
            }

            if (selectedOption == "")
            {
                isErrorPresent = true;
                errorMessage = "Please enter number of poll option. ";
            }
            //Get option inputs
            string[] options = new string[optionListLayout.Children.Count()];
            int count = 0;

            //Obtain the option inputs
            foreach (StackLayout layout in optionListLayout.Children)
            {
                foreach (View view in layout.Children)
                {
                    if (view.GetType() == typeof(Frame))
                    {
                        if (((Frame)view).Content.GetType() == typeof(Entry))
                        {
                            Entry entry = (Entry) ((Frame)view).Content;
                            options[count] = entry.Text;                           
                            count++;
                        }
                    }
                }
            }

            if (options.Count() <= 0)
            {
                isErrorPresent = true;
                errorMessage = "Please enter poll option fields. ";
            }

            if (!isErrorPresent)
            {
                if (adminAuth.Username != null && adminAuth.Password != null)
                {
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;

                    string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostAddPoll(adminAuth.Username, adminAuth.Password, pollQuestionInput.Text, selectedOption, options));
                    string httpResult = httpTask.ToString();

                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    if (httpResult == "You have succesfully published a poll!")
                    {
                        await DisplayAlert("Success", "Poll has been successfully published!", "OK");
                        var page = App.Current.MainPage as rootPage;
                        var pollPage = new pollPage();
                        page.changePage(pollPage);

                    }
                    else
                    {
                        await DisplayAlert("Failed", httpResult, "OK");
                    }
                }
                else
                {
                    Console.WriteLine("User is not logged in");
                }
            } else
            {
                await DisplayAlert("Failed", errorMessage, "OK");
            }             
        }
    }
}