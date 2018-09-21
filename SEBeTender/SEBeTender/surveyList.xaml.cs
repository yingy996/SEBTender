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
	public partial class surveyList : ContentPage
	{
        Survey survey = new Survey();
        public surveyList ()
		{
            BindingContext = this;
            InitializeComponent ();

            retrieveSurvey();

        }

        async void retrieveSurvey()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions());
            while (httpTask == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions());
            }

            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            pageTitle.IsVisible = true;

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
                        listView.ItemsSource = surveyQuestions;
                    }
                }
            }
        }

        async void onItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.SelectedItem = null;
            var item = e.SelectedItem as Survey;

            if (item != null)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new surveyPage(item));
            }
        }
    }
}