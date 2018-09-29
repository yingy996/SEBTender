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

            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += onItemSelected;

        }

        async void retrieveSurvey()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            //Retrieve list of survey from server
            string httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveys());
            while (httpTask == null)
            {
                httpTask = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveys());
            }

            

            if (httpTask != null)
            {
                List<Survey> surveyList = JsonConvert.DeserializeObject<List<Survey>>(httpTask.ToString());

                //If surveys is available, get the survey details, else display the error message
                if (surveyList != null)
                {
                    for(int i=0; i<surveyList.Count(); i++) 
                    {
                        //Get survey details
                        
                        string surveyID = surveyList[i].surveyID;
                        string surveydescription = surveyList[i].description;

                        string httpTaskquestions = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions(surveyList[i].surveyID));
                        while (httpTaskquestions == null)
                        {
                            httpTaskquestions = await Task.Run<string>(() => HttpRequestHandler.PostGetSurveyQuestions(surveyList[i].surveyID));
                        }

                        //if survey question is available, get the list of questions for this particular survey
                        if (httpTaskquestions != null)
                        {
                            surveyList[i].surveyQuestions = JsonConvert.DeserializeObject<List<surveyQuestion>>(httpTaskquestions.ToString());
                        }


                        
                    }
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    pageTitle.IsVisible = true;
                    listView.ItemsSource = surveyList;
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
                await Navigation.PushAsync(new surveyQuestionPage(item));
            }
        }
    }
}