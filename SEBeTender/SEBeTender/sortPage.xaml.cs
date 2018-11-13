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
	public partial class sortPage : ContentPage
	{
        private tenderPage tenderPage;
        private string currentOrderToggled = "ascending";
        private string selectedSort = "";
        public sortPage ()
		{
			InitializeComponent ();
        }

        public sortPage(tenderPage atenderPage)
        {
            InitializeComponent();
            List<string> sortList = new List<string>();
            sortList.Add("Closing date");
            sortList.Add("Originating source");
            sortPicker.ItemsSource = sortList;

            tenderPage = atenderPage;
        }

        async void OnSortButtonClicked(object sender, EventArgs args)
        {
            //Send the sorting info back to tender page to do sorting
            //Give back which field to sort, and in what order
            if (selectedSort != "" && currentOrderToggled != "")
            {
                tenderPage.sortTenders(selectedSort, currentOrderToggled);
                await Navigation.PopModalAsync();
            } else
            {
                DisplayAlert("Invalid inputs", "Please ensure the sort field and sort order are selected", "OK");
            }
        }

        async void OnCancelButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        void switcherToggled(object sender, ToggledEventArgs e)
        {
            Switch switchSender = (Switch)sender;

            if (switchSender.StyleId == "ascending")
            {
                if (e.Value)
                {
                    if (currentOrderToggled != "ascending")
                    {
                        currentOrderToggled = "ascending";
                        descendingSwitch.IsToggled = false;
                    }
                }
                else
                {
                    if (currentOrderToggled == "ascending")
                    {
                        ascendingSwitch.IsToggled = true;
                    }
                }
                

                /*if (e.Value)
                {
                    descendingSwitch.IsToggled = false;
                } else
                {
                    descendingSwitch.IsToggled = true;
                }*/
            }
            else
            {
                if (e.Value)
                {
                    if (currentOrderToggled != "descending")
                    {
                        currentOrderToggled = "descending";
                        ascendingSwitch.IsToggled = false;
                    }
                } else
                {
                    if (currentOrderToggled == "descending")
                    {
                        descendingSwitch.IsToggled = true;
                    }
                            
                }
                
                    

                /*if (e.Value)
                {
                    ascendingSwitch.IsToggled = false;
                }
                else
                {
                    ascendingSwitch.IsToggled = true;
                }*/
            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            selectedSort = sortPicker.Items[sortPicker.SelectedIndex];
            Console.WriteLine("Selected sort : " + selectedSort);
        }
    }
}