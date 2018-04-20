using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace DynamicColumnsTest
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        ViewModel2 viewmodel2;
        public Window2()
        {
            InitializeComponent();

            viewmodel2 = new ViewModel2();

            //Not sure about this
            this.listview.SetBinding(ListView.ItemsSourceProperty, new Binding("Friends") { Source = viewmodel2.User });

            viewmodel2.PropertyChanged += new PropertyChangedEventHandler(viewmodel_PropertyChanged);

            GridView view = GenerateGridView();
            this.listview.View = view;
        }

        void viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GridView view = GenerateGridView();
            this.listview.View = view;
        }
        private GridView GenerateGridView()
        {
            GridView view = new GridView();

            GridViewColumn column0 = new GridViewColumn()
            {
                Header = "Friend Name",
                Width = 100,
                DisplayMemberBinding = new Binding("Name")
            };

            view.Columns.Add(column0);

            foreach (var item in viewmodel2.User.Groups)
            {
              
                DataTemplate template = new DataTemplate { DataType = typeof(CheckBox) };
                
                FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);

                FrameworkElementFactory checkbox = new FrameworkElementFactory(typeof(CheckBox));
                checkbox.SetBinding(CheckBox.IsCheckedProperty, new Binding("Allowed") { Source = item });
                stackPanelFactory.AppendChild(checkbox);

                template.VisualTree = stackPanelFactory;


                view.Columns.Add(new GridViewColumn
                {

                    Header = item.Name,
                    CellTemplate = template,
                    Width = 100
                }
                );
               
            }
            return view;
        }
    }
}
