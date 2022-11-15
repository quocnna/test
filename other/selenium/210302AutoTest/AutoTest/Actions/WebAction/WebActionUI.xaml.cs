using AutoTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoTest.WebAction
{
    partial class WebActionUI : UserControl
    {
        #region Fields

        private bool _IsBinding;
        private WebAction _Instance;

        #endregion

        #region Properties

        private SimpleActionState State
        {
            get { return _Instance._State; }
        }

        public string Event
        {
            get { return this.State[WebAction.Event]; }
            set { this.State[WebAction.Event] = value; }
        }

        #endregion

        #region Constructors

        public WebActionUI()
        {
            InitializeComponent();

            initEvents();

            cmbAction.SelectionChanged += cmbAction_SelectionChanged;
        }

        #endregion

        #region Events

        void cmbAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SimpleActionState oldState = (SimpleActionState)this.State.Clone();

            if (!_IsBinding)
            {
                this.State.Clear();
                this.Event = oldState[WebAction.Event];
            }

            stackParas.Children.Clear();

            ActionInfo info = cmbAction.SelectedItem as ActionInfo;
            this.Event = info == null ? string.Empty : info.Event.ToString();

            if (info != null && info.Paras != null)
                foreach (var ele in info.Paras)
                {
                    stackParas.Children.Add(new Label() { Content = ele.Label, ToolTip = ele.ToolTip, Foreground = Brushes.Gray });
                    string value = oldState[ele.Name];
                    if (ele.Items == null || ele.Items.Count == 0)
                    {
                        TextBox text = new TextBox() { Text = value, Padding = new Thickness(2) };
                        if (ele == ParaInfo.ScriptValue)
                        {
                            text.AcceptsReturn = true;
                            text.AcceptsTab = true;
                            text.Height = 150;
                            text.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                        }
                        text.TextChanged += (s, arg) => { this.State[ele.Name] = ((TextBox)s).Text; };

                        this.State[ele.Name] = value;
                        stackParas.Children.Add(text);
                    }
                    else
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.Padding = new Thickness(4);
                        cmb.Width = 200;
                        cmb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        cmb.ItemsSource = ele.Items;
                        cmb.SelectedItem = string.IsNullOrEmpty(value) ? cmb.Items[0] : value;
                        cmb.SelectionChanged += (s, arg) => { this.State[ele.Name] = ((ComboBox)cmb).SelectedItem as string; };

                        this.State[ele.Name] = cmb.SelectedItem as string;

                        stackParas.Children.Add(cmb);
                    }
                }
        }

        #endregion

        #region Methods

        private void initEvents()
        {
            ListCollectionView lcv = new ListCollectionView(ActionInfo.AllActions);
            lcv.GroupDescriptions.Add(new PropertyGroupDescription("IO"));
            cmbAction.ItemsSource = lcv;
            cmbAction.SelectedIndex = -1;
        }
        public void Bind(WebAction instance)
        {
            var temp = _Instance;
            _Instance = instance;
            if (object.Equals(temp, instance) && object.Equals(temp.State, instance.State))
                return;

            _IsBinding = true;

            stackParas.Children.Clear();

            string actionName = this.Event;
            cmbAction.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(actionName))
            {
                bool stop = false;
                for (int i = 0; i < cmbAction.Items.Count && !stop; i++)
                    if (stop = (cmbAction.Items[i] as ActionInfo).Event.ToString().Equals(actionName))
                        cmbAction.SelectedIndex = i;
            }

            _IsBinding = false;
        }

        #endregion
    }
}
