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

        // Dictionary storing countries list in countrypicker for Registration page 2
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
            //Registration first page
            var registrationPage1 = new ContentView
            {
                Content = new StackLayout
                {
                    Children = {
                        new Label { Text="Terms and condition text bla bla bla testing blabla",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                        },
                    }
                }
            };

            //Picker for second page= combox showing list of countries
            Picker countrypicker = new Picker()
            {
                Title = "Countries",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (string colorName in nameToColor.Keys)
            {
                countrypicker.Items.Add(colorName);
            }

            //Registration second page
            var registrationPage2 = new ContentView
            {
                Content = new StackLayout
                {
                    //Spacing = 20,
                    //Padding = 50,
                    Children = {
                        new Label {Text = "Company Profile", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        new Label { Text = "Company Name: *" },
                        new Entry {},
                        new Label { Text = "Company Registration No: *" },
                        new Entry {},
                        new Label { Text = "Mailing Address: *" },
                        countrypicker,

                    },
                }
            };

            //Registration third page
            var registrationPage3 = new ContentView
            {
                Content = new StackLayout
                {
                    Children = {
                        new Label {Text = "Contact Person", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        new Label { Text = "Name: *" },
                        new Entry {},
                        new Label { Text = "Telephone No: *" },
                        new Entry {},
                        new Label { Text = "Mobile No:" },
                        new Entry {},
                        new Label { Text = "Fax No: *" },
                        new Entry {},
                        new Label { Text = "Email Address: *" },
                        new Entry {},
                    },
                }
            };

            //Registration fourth page
            var registrationPage4 = new ContentView
            {
                Content = new StackLayout
                {
                    Children = {
                        new Label {Text = "User Id and Password", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        new Label { Text = "User ID: *" },
                        new Entry {Placeholder = "At least 6 characters"},
                        new Label { Text = "Password: *" },
                        new Entry {Placeholder = "At least 8 characters", IsPassword = true},
                        new Label { Text = "Retype Password: *" },
                        new Entry {Placeholder = "Must be same as above", IsPassword = true},

                    },
                }
            };



            //Grid design for Registration fifth page
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //Interface for UPK section
            CustomCheckbox checkbox1 = new CustomCheckbox();
            var instruction1 = new Label { Text = "Please enter the exact license number as stated on the certificate (e.g. UPKJ-B/00011)" };
            var instruction2 = new Label { Text = "For those who registered in the year 2004 and above please enter the license number as 'UPKJ' at the beginning of the number instead of 'UPK' . If any issue arises, please contact SFS for verification." };
            var firstcheckboxtext = new Label { Text = "UPK", HorizontalTextAlignment = TextAlignment.Center };
            var license1 = new Entry { Placeholder = "1." };
            var license2 = new Entry { Placeholder = "2." };
            var license3 = new Entry { Placeholder = "3." };
            var license4 = new Entry { Placeholder = "4." };

            grid.Children.Add(instruction1, 0, 3, 0, 4);

            grid.Children.Add(checkbox1, 0, 4);
            grid.Children.Add(firstcheckboxtext, 0, 5);
            grid.Children.Add(license1, 1, 4);
            grid.Children.Add(license2, 2, 4);
            grid.Children.Add(license3, 1, 5);
            grid.Children.Add(license4, 2, 5);

            //Interface for CIDB section
            CustomCheckbox checkbox2 = new CustomCheckbox();
            var gradelabel = new Label { Text = "Grade :", HorizontalTextAlignment = TextAlignment.Center };
            var categorylabel = new Label { Text = "Category :", HorizontalTextAlignment = TextAlignment.Center };
            var specializationlabel = new Label { Text = "Specialization: " };
            var secondcheckboxtext = new Label { Text = "CIDB", HorizontalTextAlignment = TextAlignment.Center };
            Picker gradepicker = new Picker()
            {
                Title = "- Grade -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            for (int i = 1; i < 8; i++)
            {

                gradepicker.Items.Add("G" + i);

            }

            Picker categorypicker = new Picker()
            {
                Title = "- Category -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            categorypicker.Items.Add("CE - Civil Engineering Construction");
            categorypicker.Items.Add("B - Building Construction");
            categorypicker.Items.Add("ME - Mechanical and Electrical");

            grid.Children.Add(checkbox2, 0, 7);
            grid.Children.Add(secondcheckboxtext, 0, 8);
            grid.Children.Add(gradelabel, 0, 9);
            grid.Children.Add(categorylabel, 1, 9);
            grid.Children.Add(specializationlabel, 2, 9);

            grid.Children.Add(gradepicker, 0, 10);
            grid.Children.Add(categorypicker, 1, 10);

            //Registration fifth page
            var registrationPage5 = new ContentView
            {
                Content = new StackLayout
                {
                    
                    Children = {
                        new Label {Text = "Please select where applicable:", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        grid,
                        },
                       
                        
                    },
                
            };

            MyItemsSource = new ObservableCollection<View>()
            {
                registrationPage1,
                registrationPage2,
                registrationPage3,
                registrationPage4,
                registrationPage5
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



    public class CustomCheckbox : Image
    {
        private const string CheckboxUnCheckedImage = "checkbox_unchecked.png";
        private const string CheckboxCheckedImage = "checkbox_checked";

        public CustomCheckbox()
        {
            Source = CheckboxUnCheckedImage;
            var imageTapGesture = new TapGestureRecognizer();
            imageTapGesture.Tapped += ImageTapGestureOnTapped;
            GestureRecognizers.Add(imageTapGesture);
            PropertyChanged += OnPropertyChanged;
        }

        private void ImageTapGestureOnTapped(object sender, EventArgs eventArgs)
        {
            if (IsEnabled)
            {
                Checked = !Checked;
            }
        }

        /// <summary>
        /// The checked changed event.
        /// </summary>
        public event EventHandler<bool> CheckedChanged;

        /// <summary>
        /// The checked state property.
        /// </summary>
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create("Checked", typeof(bool), typeof(CustomCheckbox), false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

        public bool Checked
        {
            get
            {
                return (bool)GetValue(CheckedProperty);
            }

            set
            {
                if (Checked != value)
                {
                    SetValue(CheckedProperty, value);
                    CheckedChanged?.Invoke(this, value);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e?.PropertyName == IsEnabledProperty.PropertyName)
            {
                Opacity = IsEnabled ? 1 : 0.5;
            }
        }

        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var checkBox = bindable as CustomCheckbox;
            if (checkBox != null)
            {
                var value = newValue as bool?;
                checkBox.Checked = value.GetValueOrDefault();
                checkBox.Source = value.GetValueOrDefault() ? CheckboxCheckedImage : CheckboxUnCheckedImage;
            }
        }
    }


}
