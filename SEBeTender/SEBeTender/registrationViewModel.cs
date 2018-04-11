using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;


using Xamarin.Forms;
using Android.Graphics;
using System.Globalization;

namespace SEBeTender
{
    public class registrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Grid gridUPK = new Grid();
        public Grid gridCIBD = new Grid();
        IWebDriver driver = new FirefoxDriver();
        
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
            var registrationPage1 = new ScrollView
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
            var registrationPage2 = new ScrollView
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
            var registrationPage3 = new ScrollView
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
            var registrationPage4 = new ScrollView
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



            //Grid rows and columns number for registrationPage5 UPK 
            for (int i = 0; i < 6; i++)
            {
                gridUPK.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int i = 0; i < 6; i++)
            {
                gridUPK.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            //Grid rows and columns number for registrationPage5 CIBD
            for (int i = 0; i < 6; i++)
            {
                gridCIBD.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int i = 0; i < 6; i++)
            {
                gridCIBD.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            //Checkbox and its listener for UPK checkbox
            CustomCheckbox UPKcheckbox = new CustomCheckbox();
            var UPKcheckboxTapRecognizer = new TapGestureRecognizer();
            UPKcheckboxTapRecognizer.Tapped += onUPKcheckboxClicked;
            UPKcheckbox.GestureRecognizers.Add(UPKcheckboxTapRecognizer);

            
            var firstcheckboxtext = new Label { Text = "UPK", HorizontalTextAlignment = TextAlignment.Center };

            gridUPK.Children.Add(UPKcheckbox, 0, 6, 0, 1);
            gridUPK.Children.Add(firstcheckboxtext, 0, 6, 1, 2);


            //Checkbox and its listener for CIBD checkbox
            CustomCheckbox CIBDcheckbox = new CustomCheckbox();
 
            var CIBDcheckboxTapRecognizer = new TapGestureRecognizer();
            CIBDcheckboxTapRecognizer.Tapped += onCIBDcheckboxClicked;
            CIBDcheckbox.GestureRecognizers.Add(CIBDcheckboxTapRecognizer);

            var CIBDcheckboxtext = new Label { Text = "CIDB", HorizontalTextAlignment = TextAlignment.Center };
            
            gridCIBD.Children.Add(CIBDcheckbox, 0, 6, 0, 1);
            gridCIBD.Children.Add(CIBDcheckboxtext, 0, 6, 1, 2);

            //Captcha content
            var captchalabel = new Label { Text = "Please enter the CODE displayed:" };
            //Registration fifth page
            var registrationPage5 = new ScrollView
            {
                Content = new StackLayout
                {
                    
                    Children = {
                        new Label {Text = "Please select where applicable:", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.FromHex("#E5E7E8")},
                        gridUPK,
                        gridCIBD,
                        captchalabel,
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

        //GAVE UP *CANNOT SOLVE RECAPTCHA ISSUE
        /*public void TakeCaptcheScreenshot()
        {
            driver.Navigate().GoToUrl("http://www2.sesco.com.my/etender/registration/vendor_register.jsp");
            
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(@"D:\Screenshots\SeleniumTestingScreenshot.jpg", ScreenshotImageFormat.Jpeg);

            //Get screenshot of specific element
            if (Device.RuntimePlatform == Device.iOS)
            {

            }else if(Device.RuntimePlatform == Device.Android)
            {
                var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));
                IWebElement element = driver.FindElement(By.CssSelector("label.realperson-challenge"));
                
                var cropArea = new Rectangle(element.Location, element.Size);
                return bmpScreen.Clone(cropArea, bmpScreen.PixelFormat);

            }
        }*/

        /*public class PointConverter : Xamarin.Forms.IValueConverter {  
            public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
            {
                System.Drawing.Point dp = (System.Drawing.Point)value;
                return new Xamarin.Forms.Point(dp.X, dp.Y);
            }

            public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
            {
                Xamarin.Forms.Point wp = (Xamarin.Forms.Point)value;
                return new System.Drawing.Point((int)wp.X, (int)wp.Y);
            }
        }*/


        // UPK Checkbox toggle actions 
        public void onUPKcheckboxClicked(object sender, EventArgs eventArgs)
        {
            var checkbox = (CustomCheckbox)sender;
            var license1 = new Entry { Placeholder = "1." };
            var license2 = new Entry { Placeholder = "2." };
            var license3 = new Entry { Placeholder = "3." };
            var license4 = new Entry { Placeholder = "4." };
            var instruction1 = new StackLayout{Children = {new Label { Text = "Please enter the exact license number as stated on the certificate (e.g. UPKJ-B/00011)" },},};
            var instruction2 = new StackLayout{Children ={new Label { Text = "For those who registered in the year 2004 and above please enter the license number as 'UPKJ' at the beginning of the number instead of 'UPK' . If any issue arises, please contact SFS for verification.", TextColor = Color.Red },},};

            if (checkbox.Checked)
            {
                gridUPK.Children.Add(instruction1, 0, 6, 2, 3);
                gridUPK.Children.Add(instruction2, 0, 6, 3, 4);
                gridUPK.Children.Add(license1, 0, 3, 4, 5);
                gridUPK.Children.Add(license2, 3, 6, 4, 5);
                gridUPK.Children.Add(license3, 0, 3, 5, 6);
                gridUPK.Children.Add(license4, 3, 6, 5, 6);
            } 

            if (checkbox.Checked == false) {
                foreach (var child in gridUPK.Children.Reverse())
                {
                    var childTypeName = child.GetType().Name;
                    if (childTypeName == "Entry")
                    {
                        gridUPK.Children.Remove(child);
                    } else if(childTypeName == "StackLayout")
                    {
                        gridUPK.Children.Remove(child);
                    }
                    
                }
                
            }
        }

        // CIBD Checkbox toggle actions 
        public void onCIBDcheckboxClicked(object sender, EventArgs eventArgs)
        {
            var checkbox = (CustomCheckbox)sender;
            var gradelabel = new StackLayout { Children = { new Label { Text = "Grade :", HorizontalTextAlignment = TextAlignment.Center }, }, };
            var categorylabel = new StackLayout { Children = { new Label { Text = "Category :", HorizontalTextAlignment = TextAlignment.Center }, }, };
            var specializationlabel = new StackLayout { Children = { new Label { Text = "Specialization: ", HorizontalTextAlignment = TextAlignment.Center }, }, };

            var specializationentry1 = new Entry();
            var specializationentry2 = new Entry();
            var specializationentry3 = new Entry();
            Picker gradepicker1 = new Picker()
            {
                Title = "- Grade -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Picker gradepicker2 = new Picker()
            {
                Title = "- Grade -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Picker gradepicker3 = new Picker()
            {
                Title = "- Grade -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            for (int i = 1; i < 8; i++)
            {

                gradepicker1.Items.Add("G" + i);
                gradepicker2.Items.Add("G" + i);
                gradepicker3.Items.Add("G" + i);

            }

            Picker categorypicker1 = new Picker()
            {
                Title = "- Category -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Picker categorypicker2 = new Picker()
            {
                Title = "- Category -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Picker categorypicker3 = new Picker()
            {
                Title = "- Category -",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            categorypicker1.Items.Add("CE - Civil Engineering Construction");
            categorypicker1.Items.Add("B - Building Construction");
            categorypicker1.Items.Add("ME - Mechanical and Electrical");
            categorypicker2.Items.Add("CE - Civil Engineering Construction");
            categorypicker2.Items.Add("B - Building Construction");
            categorypicker2.Items.Add("ME - Mechanical and Electrical");
            categorypicker3.Items.Add("CE - Civil Engineering Construction");
            categorypicker3.Items.Add("B - Building Construction");
            categorypicker3.Items.Add("ME - Mechanical and Electrical");

            if (checkbox.Checked)
            {
                gridCIBD.Children.Add(gradelabel, 0, 2, 2, 3);
                gridCIBD.Children.Add(categorylabel, 2, 4, 2, 3);
                gridCIBD.Children.Add(specializationlabel, 4, 6, 2, 3);

                gridCIBD.Children.Add(gradepicker1, 0, 2, 3, 4);
                gridCIBD.Children.Add(categorypicker1, 2, 4, 3, 4);
                gridCIBD.Children.Add(specializationentry1, 4, 6, 3, 4);
                gridCIBD.Children.Add(gradepicker2, 0, 2, 4, 5);
                gridCIBD.Children.Add(categorypicker2, 2, 4, 4, 5);
                gridCIBD.Children.Add(specializationentry2, 4, 6, 4, 5);
                gridCIBD.Children.Add(gradepicker3, 0, 2, 5, 6);
                gridCIBD.Children.Add(categorypicker3, 2, 4, 5, 6);
                gridCIBD.Children.Add(specializationentry3, 4, 6, 5, 6);
            }

            if (checkbox.Checked == false)
            {
                foreach (var child in gridCIBD.Children.Reverse())
                {
                    var childTypeName = child.GetType().Name;
                    if (childTypeName == "Picker")
                    {
                        gridCIBD.Children.Remove(child);
                    }else if(childTypeName == "Entry")
                    {
                        gridCIBD.Children.Remove(child);
                    }else if(childTypeName == "StackLayout")
                    {
                        gridCIBD.Children.Remove(child);
                    }

                }

            }
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
        private const string CheckboxCheckedImage = "checkbox_checked.png";

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
            //if (IsEnabled)
            //{
                Checked = !Checked;
            //}
        }

        /// <summary>
        /// The checked changed event.
        /// </summary>
        public event EventHandler<bool> CheckedChanged;

        /// <summary>
        /// The checked state property.
        /// </summary>
        public static BindableProperty CheckedProperty = BindableProperty.Create("Checked", typeof(bool), typeof(CustomCheckbox), false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

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
