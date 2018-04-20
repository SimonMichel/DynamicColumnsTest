using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace DynamicColumnsTest
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        ViewModel viewmodel = new ViewModel();
        public Window1()
        {
            InitializeComponent();
            this.listview.SetBinding(ListView.ItemsSourceProperty, new Binding("QueryResult") { Source = viewmodel });
            viewmodel.PropertyChanged += new PropertyChangedEventHandler(viewmodel_PropertyChanged);
        }

        void viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GridView view = GenerateGridView(viewmodel.QueryResult);
            this.listview.View = view;
        }
        private GridView GenerateGridView(DataTable table)
        {
            GridView view = new GridView();

            foreach (DataColumn column in table.Columns)
            {
                view.Columns.Add(new GridViewColumn()
                {
                    Header = column.ColumnName,
                    DisplayMemberBinding = new Binding(column.ColumnName)
                });
            }

            return view;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("column 1"));
            table.Columns.Add(new DataColumn("column 2"));
            table.Rows.Add(new object[] { "1", "a" });
            table.Rows.Add(new object[] { "2", "b" });
            viewmodel.QueryResult = table;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("column 3"));
            table.Columns.Add(new DataColumn("column 4"));
            table.Rows.Add(new object[] { "3", "c" });
            table.Rows.Add(new object[] { "4", "d" });
            viewmodel.QueryResult = table;
        }
    }
}