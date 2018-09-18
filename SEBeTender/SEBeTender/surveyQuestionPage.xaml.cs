﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEBeTender
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class surveyQuestionPage : ContentPage
	{
        Survey survey = new Survey();
        int currentQuestionCount = 0;
        int currentQuestion = 0;
        bool isFirstLoaded = true;
        string userAnswer = "";
		public surveyQuestionPage ()
		{
			InitializeComponent ();

            //Create survey item for demo
            surveyQuestion surveyQuestion1 = new surveyQuestion();
            surveyQuestion1.questionID = "11111111";
            surveyQuestion1.questionTitle = "What is your favourite food?";
            surveyQuestion1.questionType = "dropdown";
            surveyQuestion1.surveyID = "1234567";

            surveyOption option1 = new surveyOption();
            option1.answerID = "1111112";
            option1.answerTitle = "Pizza";
            option1.questionID = "11111111";
            option1.surveyID = "1234567";

            surveyOption option2 = new surveyOption();
            option2.answerID = "1111113";
            option2.answerTitle = "Cabornara Spaghetti";
            option2.questionID = "11111111";
            option2.surveyID = "1234567";

            List<surveyOption> surveyOptions = new List<surveyOption>();
            surveyOptions.Add(option1);
            surveyOptions.Add(option2);
            surveyQuestion1.surveyOptions = surveyOptions;

            surveyQuestion surveyQuestion2 = new surveyQuestion();
            surveyQuestion2.questionID = "2222222";
            surveyQuestion2.questionTitle = "What is the way you use to pay your bill?";
            surveyQuestion2.questionType = "shortAnswer";
            surveyQuestion2.surveyID = "1234567";

            surveyQuestion surveyQuestion3 = new surveyQuestion();
            surveyQuestion3.questionID = "3333333";
            surveyQuestion3.questionTitle = "What is your feedback for this survey?";
            surveyQuestion3.questionType = "paragraph";
            surveyQuestion3.surveyID = "1234567";

            survey.surveyID = "1234567";
            survey.surveyTitle = "User demographic survey";
            survey.description = "This survey is to collect demographic information of app users.";
            survey.surveyQuestions = new List<surveyQuestion>();
            survey.surveyQuestions.Add(surveyQuestion1);
            survey.surveyQuestions.Add(surveyQuestion2);
            survey.surveyQuestions.Add(surveyQuestion3);

            //Display survey question
            surveyTitleLbl.Text = survey.surveyTitle;
            surveyDescLbl.Text = survey.description;
            backButton.IsVisible = false;

        }

        void onNextButtonClicked(object sender, EventArgs e)
        {
            bool isErrorPresent = false;
            if (isFirstLoaded)
            {
                isFirstLoaded = false;
                surveyTitleLbl.IsVisible = false;
                surveyDescLbl.IsVisible = false;
                firstPageBoxView.IsVisible = false;

                questionLayout.IsVisible = true;

                nextButton.Text = "Next";                
            } else
            {
                              
                if(userAnswer == "")
                {
                    //Error is present when input field is empty, user cannot proceed to next question when error is present
                    DisplayAlert("Answer field must not be empty", "Please fill in the answer field.", "OK");
                    isErrorPresent = true;
                } else
                {
                    //Reset user answer variable 
                    userAnswer = "";
                    //Increase the current count and display the next questions 
                    if (currentQuestionCount < survey.surveyQuestions.Count)
                    {
                        currentQuestionCount++;
                        if (currentQuestionCount == (survey.surveyQuestions.Count - 1))
                        {
                            nextButton.IsVisible = false;
                        } else
                        {
                            nextButton.IsVisible = true;
                        }
                    }
                    
                }
            }

            if (!isErrorPresent)
            {
                //Display the question when no error present
                surveyQuestion surveyQuestion = survey.surveyQuestions[currentQuestionCount];
                pollQuestionLbl.Text = surveyQuestion.questionTitle;
                if (currentQuestionCount != 0)
                {
                    //Delete the last child (answer field)
                    var lastChild = questionLayout.Children.Last();
                    questionLayout.Children.Remove(lastChild);

                    //Display the back button when it's not the first question
                    backButton.IsVisible = true;
                }

                //Create the answer field
                if (surveyQuestion.questionType == "dropdown")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    //Create picker to store dropdown options
                    Picker picker = new Picker();
                    picker.Title = "- Select your answer -";
                    picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
                    picker.ItemsSource = surveyQuestion.surveyOptions;
                    picker.ItemDisplayBinding = new Binding("answerTitle");

                    foreach (surveyOption option in picker.ItemsSource)
                    {
                        if (option.answerTitle == survey.surveyQuestions[currentQuestionCount].responseAnswer)
                        {
                            picker.SelectedItem = option;
                            break;
                        }
                    }

                    frame.Content = picker;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "shortAnswer")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    Entry entry = new Entry();
                    entry.TextChanged += OnTextChanged;
                    if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                    {
                        entry.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                    }

                    frame.Content = entry;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "paragraph")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    Editor editor = new Editor();
                    editor.HeightRequest = 200;
                    editor.TextChanged += OnTextChanged;
                    if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                    {
                        editor.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                    }

                    frame.Content = editor;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "checkbox")
                {

                }
                else
                {

                }
            }
            
            /*
            if (userAnswer == "" && surveyTitleLbl.IsVisible == false)
            {
                DisplayAlert("Answer field must not be empty", "Please fill in the answer field.", "OK");
            } else
            {
                //Set the answer of the survey question to user's answer and reset the userAnswer variable
                if (currentQuestionCount > 0)
                {
                    survey.surveyQuestions[currentQuestionCount - 1].responseAnswer = userAnswer;
                    userAnswer = "";
                }                
                //Set the labels displaying the survey details to invisible and set the Back button to visible after the user starts the survey
                if (surveyTitleLbl.IsVisible == true)
                {
                    surveyTitleLbl.IsVisible = false;
                    surveyDescLbl.IsVisible = false;
                    firstPageBoxView.IsVisible = false;

                    questionLayout.IsVisible = true;

                    nextButton.Text = "Next";
                }

                //Display question
                if (currentQuestionCount < survey.surveyQuestions.Count)
                {
                    if (pollQuestionLbl.Text == survey.surveyQuestions[currentQuestionCount].questionTitle && (currentQuestionCount+1) < survey.surveyQuestions.Count)
                    {
                        currentQuestionCount++;
                    }
                    //Console.WriteLine("Current question count is: " + currentQuestionCount);
                    surveyQuestion surveyQuestion = survey.surveyQuestions[currentQuestionCount];
                    pollQuestionLbl.Text = surveyQuestion.questionTitle;
                    if (currentQuestionCount != 0)
                    {
                        //Delete the last child (answer field)
                        var lastChild = questionLayout.Children.Last();
                        questionLayout.Children.Remove(lastChild);

                        //Display the back button when it's not the first question
                        backButton.IsVisible = true;
                    }

                    //Create the answer field
                    if (surveyQuestion.questionType == "dropdown")
                    {
                        StackLayout stackLayout = new StackLayout();

                        Frame frame = new Frame();
                        frame.CornerRadius = 5;
                        frame.BackgroundColor = Color.FromHex("#E5E7E8");
                        frame.Padding = 2;
                        frame.HasShadow = false;

                        //Create picker to store dropdown options
                        Picker picker = new Picker();
                        picker.Title = "- Select your answer -";
                        picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
                        picker.ItemsSource = surveyQuestion.surveyOptions;
                        picker.ItemDisplayBinding = new Binding("answerTitle");

                        frame.Content = picker;
                        stackLayout.Children.Add(frame);
                        questionLayout.Children.Add(stackLayout);
                    }
                    else if (surveyQuestion.questionType == "shortAnswer")
                    {
                        StackLayout stackLayout = new StackLayout();

                        Frame frame = new Frame();
                        frame.CornerRadius = 5;
                        frame.BackgroundColor = Color.FromHex("#E5E7E8");
                        frame.Padding = 2;
                        frame.HasShadow = false;

                        Entry entry = new Entry();
                        entry.TextChanged += OnTextChanged;
                        if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                        {
                            entry.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                        }

                        frame.Content = entry;
                        stackLayout.Children.Add(frame);
                        questionLayout.Children.Add(stackLayout);
                    }
                    else if (surveyQuestion.questionType == "paragraph")
                    {
                        StackLayout stackLayout = new StackLayout();

                        Frame frame = new Frame();
                        frame.CornerRadius = 5;
                        frame.BackgroundColor = Color.FromHex("#E5E7E8");
                        frame.Padding = 2;
                        frame.HasShadow = false;

                        Editor editor = new Editor();
                        editor.HeightRequest = 200;
                        editor.TextChanged += OnTextChanged;
                        if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                        {
                            editor.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                        }

                        frame.Content = editor;
                        stackLayout.Children.Add(frame);
                        questionLayout.Children.Add(stackLayout);
                    }
                    else if (surveyQuestion.questionType == "checkbox")
                    {

                    }
                    else
                    {

                    }

                    currentQuestionCount++;

                    if (currentQuestionCount >= survey.surveyQuestions.Count)
                    {
                        nextButton.IsVisible = false;
                    }
                    else
                    {
                        nextButton.IsVisible = true;
                    }
                }
            }
            Console.WriteLine("Current question count is: " + currentQuestionCount);
            */
        }

        void onBackButtonClicked(object sender, EventArgs e)
        {
            if (currentQuestionCount > 0)
            {
                //Decrease current count to go back to previous question
                currentQuestionCount--;

                //Set user answer variable to equal to user's answer
                userAnswer = survey.surveyQuestions[currentQuestionCount].responseAnswer;

                //Set the visibility of the 'Back' and 'Next' buttons
                if (currentQuestionCount == 0)
                {
                    backButton.IsVisible = false;
                    nextButton.IsVisible = true;
                } else
                {
                    backButton.IsVisible = true;
                    if (currentQuestionCount < (survey.surveyQuestions.Count-1))
                    {
                        nextButton.IsVisible = true;
                    } else
                    {
                        nextButton.IsVisible = false;
                    }
                }
            }
            
            //Delete the last child (answer field)
            var lastChild = questionLayout.Children.Last();
            questionLayout.Children.Remove(lastChild);

            //Display previous question
            surveyQuestion surveyQuestion = survey.surveyQuestions[currentQuestionCount];
            pollQuestionLbl.Text = surveyQuestion.questionTitle;

            //Create the answer field
            if (surveyQuestion.questionType == "dropdown")
            {
                StackLayout stackLayout = new StackLayout();

                Frame frame = new Frame();
                frame.CornerRadius = 5;
                frame.BackgroundColor = Color.FromHex("#E5E7E8");
                frame.Padding = 2;
                frame.HasShadow = false;

                //Create picker to store dropdown options
                Picker picker = new Picker();
                picker.Title = "- Select your answer -";
                picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
                picker.ItemsSource = surveyQuestion.surveyOptions;
                picker.ItemDisplayBinding = new Binding("answerTitle");

                foreach (surveyOption option in picker.ItemsSource)
                {
                    if (option.answerTitle == survey.surveyQuestions[currentQuestionCount].responseAnswer)
                    {
                        picker.SelectedItem = option;
                        break;
                    }
                }

                frame.Content = picker;
                stackLayout.Children.Add(frame);
                questionLayout.Children.Add(stackLayout);
            }
            else if (surveyQuestion.questionType == "shortAnswer")
            {
                StackLayout stackLayout = new StackLayout();

                Frame frame = new Frame();
                frame.CornerRadius = 5;
                frame.BackgroundColor = Color.FromHex("#E5E7E8");
                frame.Padding = 2;
                frame.HasShadow = false;

                Entry entry = new Entry();
                entry.TextChanged += OnTextChanged;
                if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                {
                    entry.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                }

                frame.Content = entry;
                stackLayout.Children.Add(frame);
                questionLayout.Children.Add(stackLayout);
            }
            else if (surveyQuestion.questionType == "paragraph")
            {
                StackLayout stackLayout = new StackLayout();

                Frame frame = new Frame();
                frame.CornerRadius = 5;
                frame.BackgroundColor = Color.FromHex("#E5E7E8");
                frame.Padding = 2;
                frame.HasShadow = false;

                Editor editor = new Editor();
                editor.HeightRequest = 200;
                editor.TextChanged += OnTextChanged;
                if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                {
                    editor.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                }

                frame.Content = editor;
                stackLayout.Children.Add(frame);
                questionLayout.Children.Add(stackLayout);
            }
            else if (surveyQuestion.questionType == "checkbox")
            {

            }
            else
            {

            }

            /*if (currentQuestionCount > 0)
            {
                currentQuestionCount--;

                if (pollQuestionLbl.Text == survey.surveyQuestions[currentQuestionCount].questionTitle)
                {
                    currentQuestionCount--;
                }
            }

            if (currentQuestionCount <= 0)
            {
                backButton.IsVisible = false;
            } else
            {
                backButton.IsVisible = true;
                nextButton.IsVisible = true;
            }
           
            if (currentQuestionCount >= 0)
            {
                //Delete the last child (answer field)
                var lastChild = questionLayout.Children.Last();
                questionLayout.Children.Remove(lastChild);

                //Display previous question
                
                surveyQuestion surveyQuestion = survey.surveyQuestions[currentQuestionCount];
                pollQuestionLbl.Text = surveyQuestion.questionTitle;

                //Create the answer field
                if (surveyQuestion.questionType == "dropdown")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    //Create picker to store dropdown options
                    Picker picker = new Picker();
                    picker.Title = "- Select your answer -";
                    picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
                    picker.ItemsSource = surveyQuestion.surveyOptions;
                    picker.ItemDisplayBinding = new Binding("answerTitle");

                    frame.Content = picker;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "shortAnswer")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    Entry entry = new Entry();
                    entry.TextChanged += OnTextChanged;
                    if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                    {
                        entry.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                    }

                    frame.Content = entry;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "paragraph")
                {
                    StackLayout stackLayout = new StackLayout();

                    Frame frame = new Frame();
                    frame.CornerRadius = 5;
                    frame.BackgroundColor = Color.FromHex("#E5E7E8");
                    frame.Padding = 2;
                    frame.HasShadow = false;

                    Editor editor = new Editor();
                    editor.HeightRequest = 200;
                    editor.TextChanged += OnTextChanged;
                    if (survey.surveyQuestions[currentQuestionCount].responseAnswer != "")
                    {
                        editor.Text = survey.surveyQuestions[currentQuestionCount].responseAnswer;
                    }

                    frame.Content = editor;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "checkbox")
                {

                }
                else
                {

                }
            }
            if (currentQuestionCount == 0)
            {
                currentQuestionCount++;
            }
            Console.WriteLine("Current question count is: " + currentQuestionCount);*/
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            string selectedOption = picker.Items[picker.SelectedIndex];
            userAnswer = selectedOption;
            survey.surveyQuestions[currentQuestionCount].responseAnswer = userAnswer;
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            if(sender.GetType() == typeof(Entry))
            {
                Entry entry = (Entry)sender;
                userAnswer = entry.Text;
                survey.surveyQuestions[currentQuestionCount].responseAnswer = userAnswer;
            } else if (sender.GetType() == typeof(Editor))
            {
                Editor editor = (Editor)sender;
                userAnswer = editor.Text;
                survey.surveyQuestions[currentQuestionCount].responseAnswer = userAnswer;
            }
        }
    }
}