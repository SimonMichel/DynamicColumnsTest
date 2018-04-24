using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            this.datagrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Table") { Source = viewmodel2 });

            viewmodel2.PropertyChanged += new PropertyChangedEventHandler(viewmodel_PropertyChanged);

            GenerateTable();
        }

        void viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Je ne sais pas quoi mettre ici (ni même si c'est utile)
        }
        private void GenerateTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Friend Name", typeof(string));

            IEnumerable<Group> groups = viewmodel2.Froups.Select(x => x.Group).Distinct();

            foreach (var item in groups)
            {
                dt.Columns.Add(item.Name, typeof(bool));
            }

            foreach (var item in viewmodel2.Froups.Select(x => x.Friend).Distinct())
            {
                DataRow row = dt.NewRow();
                row["Friend Name"] = item.Name;

                foreach (var group in groups)
                {
                    row[group.Name] = viewmodel2.Froups.Where(x => x.Group == group && x.Friend == item).Select(x => x.Allowed).FirstOrDefault();
                }
                dt.Rows.Add(row);
            }

            viewmodel2.Table = dt;
        }

        //Ne plus avoir ça dans agenda (ICommand dans le viewmodel est plus propre)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void Save()
        {
            viewmodel2.SaveHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
