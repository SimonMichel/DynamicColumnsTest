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

            foreach (var item in viewmodel2.User.Groups)
            {
                GridViewColumn column = new GridViewColumn()
                {
                    Header = item.Name,
                    Width = 100               
                };
                CheckBox cb = new CheckBox();
                cb.SetBinding(CheckBox.IsCheckedProperty, new Binding("Allowed") { Source = item });

                column.CellTemplate = new DataTemplate(typeof(CheckBox));

                view.Columns.Add(column);
            }

            return view;
        }
    }
}
