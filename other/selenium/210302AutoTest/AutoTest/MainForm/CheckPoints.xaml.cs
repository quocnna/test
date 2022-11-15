using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AutoTest
{
    /// <summary>
    /// Interaction logic for CheckPoints.xaml
    /// </summary>
    public partial class CheckPoints : Window
    {
        public CheckPoints()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.Loaded += delegate
            {
                BindingOperations.ClearBinding(imgApplication, Image.SourceProperty);
                CheckPointImageConverter converter = new CheckPointImageConverter(this.CheckPointsFolder);
                Binding bind = new Binding("SelectedItem.ApplicationImageFileName");
                bind.Mode = BindingMode.OneWay;
                bind.Source = lstCheckPoints;
                bind.Converter = converter;
                BindingOperations.SetBinding(imgApplication, Image.SourceProperty, bind);
            };
        }

        #region Properties

        public IList<CheckPoint> DataSource
        {
            get
            {
                return lstCheckPoints.ItemsSource as IList<CheckPoint>;
            }
            set
            {
                lstCheckPoints.ItemsSource = value;
                if (value != null)
                    lstCheckPoints.SelectedIndex = 0;
            }
        }

        public string CheckPointsFolder { get; set; }

        #endregion

        #region Inner Class

        private class CheckPointImageConverter : IValueConverter
        {
            private string _checkPointsFolder;

            public CheckPointImageConverter(string checkPointsFolder)
            {
                _checkPointsFolder = checkPointsFolder;
            }

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return System.IO.Path.Combine(_checkPointsFolder, value != null ? value.ToString() : string.Empty);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
