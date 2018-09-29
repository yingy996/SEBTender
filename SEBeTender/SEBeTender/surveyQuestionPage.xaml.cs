using Newtonsoft.Json;
using SEBeTender.Controls;
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
	public partial class surveyQuestionPage : ContentPage
	{
        Survey survey = new Survey();
        int currentQuestionCount = 0;
        int currentQuestion = 0;
        bool isFirstLoaded = true;
        string userAnswer = "";
		public surveyQuestionPage ()
		{
			

        }

        public surveyQuestionPage(Survey surveyitem)
        {
            InitializeComponent();

            survey = surveyitem;
            
            
            getQuestionAnswers();
           
            //Display survey question
            surveyTitleLbl.Text = survey.surveyTitle;
            surveyDescLbl.Text = survey.description;
            backButton.IsVisible = false;
        }

        async void getQuestionAnswers()
        {
            //Console.WriteLine("NUMBER OF QUESTIONS" + survey.surveyQuestions.Count());
            for (int i = 0; i < survey.surveyQuestions.Count(); i++)
            {
                
                
                string currentnumber = i.ToString();
                

                List<surveyOption> surveyOptions = new List<surveyOption>();

                
                if (survey.surveyQuestions[i].questionType == "dropdown")
                {
                    
                    string httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));
                    
                    while (httpTaskanswers == null)
                    {
                        httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));
                    }
                    
                    //if survey question dropdown/radiobutton/checkbox is available, get the list of answers for this particular question
                    if (httpTaskanswers != null)
                    {
                        
                        survey.surveyQuestions[i].surveyOptions = JsonConvert.DeserializeObject<List<surveyOption>>(httpTaskanswers.ToString());
                       
                    }
                    
                    
                    
                    

                }
                else if (survey.surveyQuestions[i].questionType == "checkboxes")
                {
                    string httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));

                    while (httpTaskanswers == null)
                    {
                        httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));
                    }

                    //if survey question dropdown/radiobutton/checkbox is available, get the list of answers for this particular question
                    if (httpTaskanswers != null)
                    {

                        survey.surveyQuestions[i].surveyOptions = JsonConvert.DeserializeObject<List<surveyOption>>(httpTaskanswers.ToString());

                    }



                    

                }
                else if (survey.surveyQuestions[i].questionType == "radiobutton")
                {
                    string httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));

                    while (httpTaskanswers == null)
                    {
                        httpTaskanswers = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestionAnswers(survey.surveyQuestions[i].questionID));
                    }

                    //if survey question dropdown/radiobutton/checkbox is available, get the list of answers for this particular question
                    if (httpTaskanswers != null)
                    {

                        survey.surveyQuestions[i].surveyOptions = JsonConvert.DeserializeObject<List<surveyOption>>(httpTaskanswers.ToString());

                    }


                    

                }

            }
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
                            submitButton.IsVisible = true;


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
                surveyQuestion surveyQuestion = new surveyQuestion();
                surveyQuestion = survey.surveyQuestions[currentQuestionCount];
                
                surveyQuestionLbl.Text = surveyQuestion.questionTitle;
                if (currentQuestionCount != 0)
                {
                    //Delete the last child (answer field)
                    var lastChild = questionLayout.Children.Last();
                    questionLayout.Children.Remove(lastChild);

                    //Display the back button when it's not the first question
                    backButton.IsVisible = true;
                }

                //Create the answer field
                if (surveyQuestion.questionType == "dropdown" || surveyQuestion.questionType == "radiobutton")
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

                    if (picker.ItemsSource != null)
                    {
                        foreach (surveyOption option in picker.ItemsSource)
                        {
                            if (option.answerTitle == survey.surveyQuestions[currentQuestionCount].responseAnswer)
                            {
                                picker.SelectedItem = option;
                                break;
                            }
                        }
                    }

                    

                    frame.Content = picker;
                    stackLayout.Children.Add(frame);
                    questionLayout.Children.Add(stackLayout);
                }
                else if (surveyQuestion.questionType == "shortsentence")
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
                else if (surveyQuestion.questionType == "longsentence")
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
                else if (surveyQuestion.questionType == "checkboxes")
                {             
                    //Console.WriteLine("TESTINGGGGGGGG" + surveyQuestion.surveyOptions[0].answerTitle);
                    if (surveyQuestion.surveyOptions != null)
                    {                       
                        for (int y = 0; y < surveyQuestion.surveyOptions.Count(); y++)
                        {
                            StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal };

                            Switch switcher = new Switch();
                            switcher.StyleId = surveyQuestion.surveyOptions[y].answerID;
                            switcher.Toggled += switcher_Toggled;
                            stackLayout.Children.Add(switcher);
                            stackLayout.Children.Add(new Label()
                            {
                                Text = surveyQuestion.surveyOptions[y].answerTitle
                            });

                            questionLayout.Children.Add(stackLayout);
                        }
                    }

                    
                }
            }
            
           
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
            surveyQuestionLbl.Text = surveyQuestion.questionTitle;

            //Create the answer field
            if (surveyQuestion.questionType == "dropdown" || surveyQuestion.questionType == "radiobutton")
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
            else if (surveyQuestion.questionType == "shortsentence")
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
            else if (surveyQuestion.questionType == "longsentence")
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
            else if (surveyQuestion.questionType == "checkboxes")
            {
                if (surveyQuestion.surveyOptions != null)
                {
                    for (int y = 0; y < surveyQuestion.surveyOptions.Count(); y++)
                    {
                        StackLayout stackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal };

                        Switch switcher = new Switch();
                        switcher.StyleId = surveyQuestion.surveyOptions[y].answerID;

                        stackLayout.Children.Add(switcher);
                        stackLayout.Children.Add(new Label()
                        {
                            Text = surveyQuestion.surveyOptions[y].answerTitle
                        });

                        questionLayout.Children.Add(stackLayout);
                    }
                }
            }

           
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

        void switcher_Toggled(object sender, ToggledEventArgs e)
        {
            if(sender.GetType() == typeof(Switch))
            {
                Switch switcher = (Switch)sender;
                userAnswer = switcher.StyleId;
                survey.surveyQuestions[currentQuestionCount].responseAnswer = userAnswer;



            }
        }

        void onSubmitButtonClicked(object sender, EventArgs e)
        {
            string jsonsurvey = JsonConvert.SerializeObject(survey.surveyQuestions);
            DisplayAlert("Alert", jsonsurvey, "OK");
        }
    }
}