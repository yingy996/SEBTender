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
	public partial class editPoll : ContentPage
	{
        string selectedOption = "";
        int currentOptionCount = 1;
        Poll currentPoll = new Poll();
        bool isPageFirstLoaded = true;

        public editPoll ()
		{
			InitializeComponent ();
		}

        public editPoll(Poll poll)
        {
            InitializeComponent();
            currentPoll = poll;
            displayCurrentPoll(poll);          
        }

        void displayCurrentPoll(Poll poll)
        {
            //Displaying the current details of the poll 
            pollQuestionInput.Text = poll.pollQuestion;

            var optionNumberList = new List<string>();
            for (var i = 2; i < 21; i++)
            {
                optionNumberList.Add(i.ToString());
            }

            //Display number of option
            pollOptionNoPicker.ItemsSource = optionNumberList;

            int numberOfOption = poll.pollOptions.Count;
            currentOptionCount = numberOfOption;
            
            //Display all current options
            for (var i = 1; i <= numberOfOption; i++) {
                StackLayout stackLayout = new StackLayout();

                //Create label
                Label label = new Label();
                label.Text = "Option " + i.ToString() + ": ";
                label.FontAttributes = FontAttributes.Bold;

                //Create input field
                Frame frame = new Frame();
                frame.CornerRadius = 5;
                frame.BackgroundColor = Color.FromHex("#E5E7E8");
                frame.Padding = 2;
                frame.HasShadow = false;

                Entry entry = new Entry();
                entry.Text = poll.pollOptions[i-1].optionTitle;

                frame.Content = entry;

                Button deleteButton = new Button();
                deleteButton.Text = "Delete";
                deleteButton.BackgroundColor = Color.FromHex("#EC7063");
                deleteButton.TextColor = Color.White;
                deleteButton.StyleId = poll.pollOptions[i - 1].optionID;
                deleteButton.Clicked += OnDeleteButtonClicked;

                stackLayout.Children.Add(label);
                stackLayout.Children.Add(frame);
                stackLayout.Children.Add(deleteButton);
                optionListLayout.Children.Add(stackLayout);
            }

            foreach (string optionNo in pollOptionNoPicker.ItemsSource)
            {
                if (optionNo == numberOfOption.ToString())
                {
                    pollOptionNoPicker.SelectedItem = optionNo;
                    break;
                }
            }
            currentOptionCount++;
        }

        async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            bool isErrorPresent = false;
            string errorMessage = "";
            string pollQuestion = "";

            //Validate user inputs
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

            //Obtain the inputs for options
            string[] options = new string[optionListLayout.Children.Count()];
            int count = 0;

            foreach (StackLayout layout in optionListLayout.Children)
            {
                foreach (View view in layout.Children)
                {
                    if (view.GetType() == typeof(Frame))
                    {
                        if (((Frame)view).Content.GetType() == typeof(Entry))
                        {
                            Entry entry = (Entry)((Frame)view).Content;
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
                //If inputs are valid, proceed to update the poll
                if (adminAuth.Username != null && adminAuth.Password != null)
                {
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;

                    string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostUpdatePoll(adminAuth.Username, adminAuth.Password, pollQuestionInput.Text, selectedOption, options, currentPoll));
                    string httpResult = httpTask.ToString();

                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    if (httpResult == "Successfully updated the poll!")
                    {
                        await DisplayAlert("Success", "Poll has been successfully updated!", "OK");
                        var page = App.Current.MainPage as rootPage;
                        var pollPage = new pollPage();
                        page.changePage(pollPage);

                    }
                    else
                    {
                        await DisplayAlert("Failed", httpResult, "OK");
                    }
                } else
                {
                    Console.WriteLine("User is not logged in");
                }
            } else {
                await DisplayAlert("Failed", errorMessage, "OK");
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (adminAuth.Username != null && adminAuth.Password != null)
            {
                var answer = await DisplayAlert("Delete", "Are you sure you want to delete the option?", "Yes", "No");
                Button deleteButton = (Button)sender;
                string optionID = deleteButton.StyleId;
                if (answer)
                {
                    //If user confirm to delete the option, send the HTTP request
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;

                    string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostDeleteOption(adminAuth.Username, adminAuth.Password, optionID));
                    string httpResult = httpTask.ToString();

                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;

                    if (httpResult == "Poll option successfully deleted!")
                    {
                        await DisplayAlert("Success", "Poll option has been successfully deleted!", "OK");
                        var page = App.Current.MainPage as rootPage;
                        var pollPage = new pollPage();
                        page.changePage(pollPage);

                    }
                    else
                    {
                        await DisplayAlert("Failed", httpResult, "OK");
                    }
                }
            }           
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPageFirstLoaded)
            {
                isPageFirstLoaded = false;
                Picker picker = (Picker)sender;
                selectedOption = pollOptionNoPicker.Items[pollOptionNoPicker.SelectedIndex];
            } else
            {
                Console.WriteLine("Index changed for picker" + currentOptionCount);

                Picker picker = (Picker)sender;
                selectedOption = pollOptionNoPicker.Items[pollOptionNoPicker.SelectedIndex];
                int selectedOptionCount = Convert.ToInt32(selectedOption);

                if (selectedOptionCount < currentOptionCount)
                {
                    Console.WriteLine("less than runned!");
                    if (selectedOptionCount < currentPoll.pollOptions.Count)
                    {
                        Console.WriteLine("Selected is less than poll count: selected " + selectedOptionCount + " poll: " + currentPoll.pollOptions.Count);
                        //When selected number of option is smaller, display alert to disallow action
                        DisplayAlert("Action failed", "You can only select a greater number of option than the current number. To delete current option, use the 'Delete' button", "Okay");

                        //Restore to previous option number
                        foreach (string optionNo in pollOptionNoPicker.ItemsSource)
                        {
                            if (optionNo == (currentOptionCount-1).ToString())
                            {
                                pollOptionNoPicker.SelectedItem = optionNo;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Selected is more than or equal poll count: selected " + selectedOptionCount + " poll: " + currentPoll.pollOptions.Count);
                        Console.WriteLine("This is followed: currentoptioncount " + currentOptionCount);
                        int numberDifference = currentOptionCount - (selectedOptionCount + 1);

                        for (var i = 0; i < numberDifference; i++)
                        {
                            var lastChild = optionListLayout.Children.Last();
                            optionListLayout.Children.Remove(lastChild);
                        }
                  
                        currentOptionCount = selectedOptionCount + 1;                                                
                    }
                }
                else //if (selectedOptionCount > currentOptionCount)
                {
                    Console.WriteLine("greater runned!");
                    Console.WriteLine("Selected: " + selectedOptionCount);
                    Console.WriteLine("Current: " + currentOptionCount);
                    //When selected option number is greater than current option number, generate more option fields
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
            }
            /*Console.WriteLine("Index changed for picker" + currentOptionCount);

            Picker picker = (Picker)sender;
            selectedOption = pollOptionNoPicker.Items[pollOptionNoPicker.SelectedIndex];
            int selectedOptionCount = Convert.ToInt32(selectedOption);

            if (selectedOptionCount < currentOptionCount)
            {
                Console.WriteLine("less than runned!");
                if (selectedOptionCount < currentPoll.pollOptions.Count)
                {
                    //When selected number of option is smaller, display alert to disallow action
                    DisplayAlert("Action failed", "You can only select a greater number of option than the current number. To delete current option, use the 'Delete' button", "Okay");

                    //Restore to previous option number
                    foreach (string optionNo in pollOptionNoPicker.ItemsSource)
                    {
                        if (optionNo == currentOptionCount.ToString())
                        {
                            pollOptionNoPicker.SelectedItem = optionNo;
                            break;
                        }
                    }
                } else
                {
                    int numberDifference = currentOptionCount - (selectedOptionCount + 1);

                    for (var i = 0; i < numberDifference; i++)
                    {
                        var lastChild = optionListLayout.Children.Last();
                        optionListLayout.Children.Remove(lastChild);
                    }
                    currentOptionCount = selectedOptionCount + 1;
                }
            } else //if (selectedOptionCount > currentOptionCount)
            {
                Console.WriteLine("greater runned!");
                Console.WriteLine("Selected: " + selectedOptionCount);
                Console.WriteLine("Current: " + currentOptionCount);
                //When selected option number is greater than current option number, generate more option fields
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
            }*/ /* else
            {
                /*Console.Write("ELSE RUNNED!");
                //Restore to previous option number
                foreach (string optionNo in pollOptionNoPicker.ItemsSource)
                {
                    if (optionNo == currentOptionCount.ToString())
                    {
                        pollOptionNoPicker.SelectedItem = optionNo;
                        break;
                    }
                }*/
            //}
            //Console.WriteLine("the end option count: " + currentOptionCount);
            /*

            if (selectedOptionCount < currentOptionCount)
            {
                int numberDifference = currentOptionCount - (selectedOptionCount + 1);

                for (var i = 0; i < numberDifference; i++)
                {
                    var lastChild = optionListLayout.Children.Last();
                    optionListLayout.Children.Remove(lastChild);
                }
                currentOptionCount = selectedOptionCount + 1;
            }
            else
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
            Console.WriteLine("Layout children: " + optionListLayout.Children.Count());*/
        }
    }
}