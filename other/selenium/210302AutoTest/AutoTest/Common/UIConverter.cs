using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoTest.Core
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
                flag = (bool)value;
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (!flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) != Visibility.Visible));
        }
    }

    public class BooleanToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && (bool)value ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TextFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value;
            return string.Format(parameter.ToString(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TestStepsWrapper : INotifyPropertyChanged
    {
        public ObservableCollection<TestStep> Steps { get; set; }

        bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class TestCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TestCase tc = value as TestCase;
            if (tc != null)
            {
                if (!tc.Children.IsEmpty())
                    return tc.Children;
                else if (!tc.Steps.IsEmpty())
                {
                    return new List<TestStepsWrapper>
                    {
                        new TestStepsWrapper
                        {
                            Steps = tc.Steps
                        }
                    };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GlobalVariablesWrapper
    {
        public ObservableCollection<GlobalVariable> Variables { get; set; }
    }
    public class GlobalDataTableWrapper
    {
        public TableData DataTable { get; set; }
    }
    public class GlobalVariableGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GlobalVariableGroup group = value as GlobalVariableGroup;
            if (group != null)
            {
                if (!group.Children.IsEmpty())
                    return group.Children;
                else if (!group.Variables.IsEmpty())
                {
                    return new List<GlobalVariablesWrapper>
                    {
                        new GlobalVariablesWrapper
                        {
                            Variables = group.Variables
                        }
                    };
                }
                else if (group.DataTable != null)
                {
                    return new List<GlobalDataTableWrapper>
                    {
                        new GlobalDataTableWrapper
                        {
                            DataTable = group.DataTable
                        }
                    };
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
