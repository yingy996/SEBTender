using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
namespace SEBeTender
{
    public class registrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Dictionary storing countries list in picker
        Dictionary<string, Color> nameToColor = new Dictionary<string, Color>
        {
            { "Aqua", Color.Aqua }, { "Black", Color.Black },
            { "Blue", Color.Blue }, { "Light Gray", Color.FromHex("#E5E7E8") },
            { "Gray", Color.Gray }, { "Green", Color.Green },
            { "Lime", Color.Lime }, { "Maroon", Color.Maroon },
            { "Navy", Color.Navy }, { "Olive", Color.Olive },
            { "Purple", Color.Purple }, { "Red", Color.Red },
            { "Silver", Color.Silver }, { "Teal", Color.Teal },
            { "White", Color.White }, { "Yellow", Color.Yellow }
        };


        public registrationViewModel()
        {
            var registrationPage1 = new ContentView
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label { Text="Terms and condition text bla bla bla testing blabla",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                        },
                    }
                }
            };

            //Picker = combox showing list of countries
            Picker picker = new Picker()
            {
                Title = "Countries",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (string colorName in nameToColor.Keys)
            {
                picker.Items.Add(colorName);
            }

            var registrationPage2 = new ContentView
            {
                Content = new StackLayout
                {
                    Spacing = 20,
                    Padding = 50,
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {Text = "Company Profile", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        new Label { Text = "Company Name: *" },
                        new Entry {},
                        new Label { Text = "Company Registration No: *" },
                        new Entry {},
                        new Label { Text = "Mailing Address: *" },
                        picker,

                    },
                }
            };

            MyItemsSource = new ObservableCollection<View>()
            {
                registrationPage1,
                registrationPage2
            };


            MyCommand = new Command(() =>
            {
                Debug.WriteLine("Position selected.");
            });
        }

        ObservableCollection<View> _myitemsSource;
        public ObservableCollection<View> MyItemsSource
        {
            set
            {
                _myitemsSource = value;
                OnPropertyChanged("MyItemsSource");
            }
            get
            {
                return _myitemsSource;
            }
        }
        
        public Command MyCommand { protected set; get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
