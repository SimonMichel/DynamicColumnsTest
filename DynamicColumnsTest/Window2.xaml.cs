using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        ViewModel2 viewmodel2;

        public Window2()
        {
            InitializeComponent();

            viewmodel2 = new ViewModel2();

            this.listview.SetBinding(ListView.ItemsSourceProperty, new Binding("Friends") { Source = viewmodel2.User });
            this.number.SetBinding(Label.ContentProperty, new Binding("Number") { Source = viewmodel2 });

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
                GridViewColumn column = new GridViewColumn()
                {
                    Header = item.Name,
                    Width = 100
                };
                CheckBox cb = new CheckBox();
                cb.SetBinding(CheckBox.IsCheckedProperty, new Binding("Allowed") { Source = item });

                column.CellTemplate = Create(typeof(CheckBox));

                view.Columns.Add(column);
            }

            return view;
        }

        public DataTemplate Create(Type type)
        {
            StringReader stringReader = new StringReader(
            @"<DataTemplate 
        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
            <" + type.Name + @" IsChecked=""{Binding " + "Allowed" + @"}""/> 
        </DataTemplate>");
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }
    }
}
