using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NeogovVN.WpfUIControls
{
    public class SplitButton : Grid
    {
        #region Constructors

        public SplitButton()
        {
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(16) });

            this.Children.Add(_btnCurrentItem);
            _btnCurrentItem.Background = Brushes.Transparent;
            _btnCurrentItem.Padding = new System.Windows.Thickness(2);
            _btnCurrentItem.BorderThickness = new System.Windows.Thickness(0);

            this.Children.Add(_btnDropDonwIcon);
            _btnDropDonwIcon.Background = Brushes.Transparent;
            _btnDropDonwIcon.BorderThickness = new System.Windows.Thickness(0);
            _btnDropDonwIcon.PreviewMouseDown += _btnDropDonwIcon_PreviewMouseDown;
            Grid.SetColumn(_btnDropDonwIcon, 1);

            _btnDropDonwIcon.Content = new Path()
            {
                Data = Geometry.Parse("M0,0L2,2 4,0z"),
                Stroke = Brushes.Black,
                Fill = Brushes.Black
            };
        }

        #endregion

        #region Fields

        private Button _btnCurrentItem = new Button();
        private Button _btnDropDonwIcon = new Button();

        #endregion

        #region Properties

        public int SelectedIndex { get; set; }

        public MenuItem SelectedItem
        {
            get { return _btnCurrentItem.Tag as MenuItem; }
            set
            {
                DockPanel panel = new DockPanel();
                if (value != null)
                {
                    Image icon = value.Icon as Image;
                    if (icon != null)
                    {
                        Image image = new Image();
                        image.Source = icon.Source;
                        image.Height = 16;
                        panel.Children.Add(image);
                    }

                    panel.Children.Add(new Label() { Content = value.Header });
                }
                _btnCurrentItem.Tag = value;
                _btnCurrentItem.Content = panel;
            }
        }

        #endregion

        #region Events

        void _btnDropDonwIcon_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _btnDropDonwIcon.ContextMenu.PlacementTarget = this;
            _btnDropDonwIcon.ContextMenu.Placement = PlacementMode.Bottom;
            _btnDropDonwIcon.ContextMenu.IsOpen = true;
        }

        #endregion

        #region Methods

        public override void EndInit()
        {
            _btnDropDonwIcon.ContextMenu = new ContextMenu();

            int i = this.Children.Count;
            while (--i >= 0)
            {
                MenuItem o = this.Children[i] as MenuItem;
                if (o != null)
                {
                    this.Children.RemoveAt(i);
                    _btnDropDonwIcon.ContextMenu.Items.Insert(0, o);
                }
            }

            if (_btnDropDonwIcon.ContextMenu != null && _btnDropDonwIcon.ContextMenu.Items.Count > this.SelectedIndex && this.SelectedIndex > -1)
                this.SelectedItem = _btnDropDonwIcon.ContextMenu.Items[this.SelectedIndex] as MenuItem;

            base.EndInit();
        }

        #endregion

    }
}
