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
	public partial class filterPage : ContentPage
	{
        private tenderPage tenderPage;
        private List<string> selectedFilter = new List<string>();
        Dictionary<string, bool> filterList = new Dictionary<string, bool>();
        private bool isAllSelected = true;
        private bool isSourceToggled = false;
		public filterPage ()
		{
			InitializeComponent ();
		}

        public filterPage(List<string> originatingSources, tenderPage atenderPage, List<string> currentFilter)
        {
            InitializeComponent();
            tenderPage = atenderPage;
            
            //Set whether the filter options should be toggled or not toggled
            foreach (string source in originatingSources)
            {
                if (currentFilter.Contains(source))
                {
                    filterList[source] = true;
                } else
                {
                    filterList[source] = false;
                }
            }
            if (currentFilter.Count() == originatingSources.Count())
            {
                //Set "All" switch to true if all the filter is selected
                isSourceToggled = true;
                allSwitch.IsToggled = true;
                isSourceToggled = false;
            } else
            {
                isSourceToggled = true;
                allSwitch.IsToggled = false;
                isSourceToggled = false;
            }
            listView.ItemsSource = filterList;
        }

        void switcherToggled(object sender, ToggledEventArgs e)
        {
            //Console.WriteLine("Switch is now " + e.Value);
            Switch switchSender = (Switch)sender;

            if (switchSender.BindingContext != null)
            {
                KeyValuePair<string, bool> selectedOption = (KeyValuePair<string, bool>)switchSender.BindingContext;
                //Console.WriteLine("Switch is now: " + selectedOption.Value + " Key is " + selectedOption.Key);

                //Update filterList to include no toggle value
                filterList[selectedOption.Key] = e.Value;
            }
            
            if (!e.Value)
            {
                //Toggle "All" switch to off
                isSourceToggled = true;
                allSwitch.IsToggled = false;
                isSourceToggled = false;
            } else
            {
                bool isAllOptionSelected = true;
                //Check if all selection is chosen, if all is on, toggle the "All" switch to on
                foreach (KeyValuePair<string, bool> option in filterList)
                {
                    if (option.Value == false)
                    {
                        isAllOptionSelected = false;
                    }
                }

                if (isAllOptionSelected)
                {
                    isSourceToggled = true;
                    allSwitch.IsToggled = true;
                    isSourceToggled = false;
                }
            }

            

            //string sourceSelected = (string) switchSender.BindingContext;
            //Console.WriteLine("Switch selected: " + sourceSelected);
        }

        void allSwitchToggled(object sender, ToggledEventArgs e)
        {
            Switch switchSender = (Switch)sender;

            if (e.Value == true)
            {
                isAllSelected = true;
            }
            else
            {
                isAllSelected = false;
            }

            //Set all the switch to toggled
            if (!isSourceToggled)
            {
                Dictionary<string, bool> tempList = new Dictionary<string, bool>();
                foreach (var option in filterList)
                {
                    tempList[option.Key] = isAllSelected;
                }
                filterList = tempList;
                listView.ItemsSource = filterList;
            }
        }

        async void OnFilterButtonClicked(object sender, EventArgs args)
        {
            List<string> filterListStr = new List<string>();

            //Check which options are selected
            foreach (KeyValuePair<string, bool> option in filterList)
            {
                if (option.Value)
                {
                    filterListStr.Add(option.Key);
                }
            }
            //Pass selected filter options back to tender page
            Console.WriteLine("Number of filter selected: " + filterListStr.Count());
            tenderPage.filterTenders(filterListStr);
            await Navigation.PopModalAsync();
        }

        async void OnCancelButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}