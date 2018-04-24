using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DynamicColumnsTest
{
    public class ViewModel2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        public User User { get; set; }

        private List<Froup> froups;
        public List<Froup> Froups
        {
            get { return froups; }
            set { froups = value; OnPropertyChanged(nameof(Froups)); }
        }

        private DataTable table;
        public DataTable Table
        {
            get { return table; }
            set
            {
                if (table != value)
                {
                    table = value;
                    OnPropertyChanged("Table");
                }
            }
        }

        public ViewModel2()
        {
            this.User = new User("Simon");

            Froups = new List<Froup>();

            foreach (Friend friend in User.Friends)
            {
                foreach (Group group in User.Groups)
                {
                    Froup f = new Froup(User, friend, group, false);
                    froups.Add(f);
                }
            }
        }

        //Changer ça en ICommand dans Agenda
        private EventHandler saveHandler;
        public EventHandler SaveHandler
        {
            get => saveHandler ?? (saveHandler += (s, e) => Save());
        }

        private void Save()
        {
            IEnumerable<Group> groups = Froups.Select(x => x.Group).Distinct();

            foreach (DataRow row in Table.Rows)
            {
                foreach (var group in groups)
                {
                    //Mettre dans un try
                    bool newValue = (bool)row[group.Name];
                    Froups.Where(x => x.Friend.Name == (string)row[0] && x.Group == group).FirstOrDefault().Allowed = newValue;
                }
            }

            //Apporter les modifications à la db.
        }
    }




    //Classes for this viewmodel
    public abstract class Person
    {
        public string Name { get; set; }
        public Person() { }
        public Person(string name) { Name = name; }
    }

    public class User : Person
    {
        private List<Friend> friends;
        public List<Friend> Friends
        {
            get { return friends; }
            set { friends = value; }
        }

        private List<Group> groups;
        public List<Group> Groups
        {
            get { return groups; }
            set { groups = value; }
        }

        public User(string name) : base(name)
        {
            friends = new List<Friend>() { new Friend("Pierre"), new Friend("Paul"), new Friend("Jean"), new Friend("Jacques") };
            groups = new List<Group>() { new Group("Travail"), new Group("Foot"), new Group("Poney"), new Group("Secret"), new Group("Piscine") };
        }
    }

    public class Group
    {
        public string Name;

        public Group(string name)
        {
            Name = name;
        }
    }

    public class Friend : Person
    {
        public Friend() : base()
        {

        }

        public Friend(string name) : base(name)
        {

        }
    }

    public class Froup
    {
        public User User { get; set; }
        public Friend Friend { get; set; }
        public Group Group { get; set; }
        public bool Allowed { get; set; }

        public Froup(User u, Friend f, Group g, bool b)
        {
            User = u;
            Friend = f;
            Group = g;
            Allowed = b;
        }
    }
}