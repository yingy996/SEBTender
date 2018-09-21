using Newtonsoft.Json;
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
	public partial class surveyPage : ContentPage
	{

        Survey survey = new Survey();

        public surveyPage ()
		{
			//InitializeComponent ();

            //retrieveSurvey();
        }

        public surveyPage(Survey surveyitem)
        {
            InitializeComponent();
            retrieveSurvey();
        }

        async void retrieveSurvey()
        {


            /*activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions());
            while (httpTask == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions());
            }

            if (httpTask != null)
            {
                List<Survey> surveyList = JsonConvert.DeserializeObject<List<Survey>>(httpTask.ToString());

                //If survey is available, get the survey details, else display the error message
                if (surveyList != null)
                {
                    if (surveyList.Count > 0)
                    {
                        //Get survey details
                        survey = surveyList[0];
                        Console.WriteLine("Survey Title is: " + survey.surveyTitle);
                        string surveyID = survey.surveyID;
                        string surveydescription = survey.description;
                        List<surveyQuestion> surveyQuestions = survey.surveyQuestions;
                        

                        //Get survey questions
                        string httpOptionTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollOptions(pollID));
                        while (httpOptionTask == null)
                        {
                            httpOptionTask = await Task.Run<string>(() => HttpRequestHandler.PostGetPollOptions(pollID));
                        }

                        if (httpOptionTask != null)
                        {
                            List<pollOption> pollOptionList = JsonConvert.DeserializeObject<List<pollOption>>(httpOptionTask.ToString());
                            poll.pollOptions = pollOptionList;
                            List<string> pollOptionStrings = new List<string>();
                            foreach (pollOption option in poll.pollOptions)
                            {
                                pollOptionStrings.Add(option.optionTitle);
                            }
                            if (pollOptionList != null)
                            {
                                if (pollOptionList.Count > 0)
                                {

                                    //Display poll details and options    
                                    isPollPresent = true;
                                    Console.WriteLine("YES TESTING IF ISPRESENT " + isPollPresent);
                                    pollQuestionLbl.Text = pollQuestion;
                                    pollOptionPicker.ItemsSource = pollOptionStrings;
                                    pollQuestionLbl.IsVisible = true;
                                    optionFrame.IsVisible = true;
                                    //submitButton.IsVisible = true;
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        errorMsg.IsVisible = true;
                    }
                }
                else
                {
                    Console.WriteLine("Survey Task is null ");
                    errorMsg.IsVisible = true;
                }
            }*/
        }
	}
}