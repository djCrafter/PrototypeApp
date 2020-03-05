using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamTestInvestigate
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ObservableCollection<DataSource> cars = new ObservableCollection<DataSource>();
        ObservableCollection<DataSource> company = new ObservableCollection<DataSource>();

        DataSource tempDataSource;

        public ICommand DeleteCommand { protected set; get; }
        public ICommand CopyCommand { protected set; get; }
        public ICommand ReplaceCommand { protected set; get; }

        public MainPage()
        {
            DeleteCommand = new Command(OnDelete);
            CopyCommand = new Command(OnCopy);
            ReplaceCommand = new Command(OnReplace);

            InitializeComponent();
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Mercedes", Message = "1111111111111111111111111111111111111111111111111111111111111", CardColor = Color.YellowGreen }); ;
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "BMW", Message = "22222222222222222222222222222222222222222222", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "OPEL", Message = "333333333333333333333333333333", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "KRAZ", Message = "4444444444444444444444444444444", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "KAMAZ", Message = "55555555555555555555555555555555555", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "DAEWOO", Message = "6666666666666666666666666666666666", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Mazda", Message = "777777777777777777777777777777777777777777777777", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Ferrari", Message = "8888888888888888888888888888888888888888", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Lanos", Message = "9999999999999999999999999999999999999999", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Lada", Message = "000000000000000000000000000000000000000000", CardColor = Color.YellowGreen });
            cars.Add(new DataSource() { DataGuid = new Guid(), Title = "Pontiac", Message = "121212121212121212121212121212121212121212", CardColor = Color.YellowGreen });


            company.Add(new DataSource() { DataGuid = new Guid(), Title = "IBM", Message = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Apple", Message = "BBBBBBBBBBBBBBBBBBBBBB", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Microsoft", Message = "CCCCCCCCCCCCCCCCCCCCCCCCC", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Oracle", Message = "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Google", Message = "EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "HeadWorks", Message = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Blizzard", Message = "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Bethesda", Message = "HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Ubisoft", Message = "KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK", CardColor = Color.Pink });
            company.Add(new DataSource() { DataGuid = new Guid(), Title = "Konami", Message = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT", CardColor = Color.Pink });

            listView1.Items = cars;
            listView2.Items = company;

            cars.CollectionChanged += Cars_CollectionChanged;
        }

        private void OnReplace(object item)
        {
            if (tempDataSource != null)
            {
                var listViewItem = item as DataSource;

                if (cars.Contains(listViewItem))
                {
                    var index = cars.IndexOf(listViewItem);
                    cars.Remove(listViewItem);
                    company.Remove(tempDataSource);
                    cars.Insert(index, tempDataSource);
                    tempDataSource = null;
                }
                else
                {
                    var index = company.IndexOf(listViewItem);
                    company.Remove(listViewItem);
                    cars.Remove(tempDataSource);
                    company.Insert(index, tempDataSource);
                    tempDataSource = null;
                }
            }
        }

        private void OnCopy(object item)
        {
            tempDataSource = item as DataSource;
        }

        private void OnDelete(object item)
        {
            var listViewItem = item as DataSource;
            
            if (tempDataSource == listViewItem)
            {
                tempDataSource = null;
            }

            if (cars.Contains(listViewItem))
            {
                cars.Remove(listViewItem);
            }
            else
            {
                company.Remove(listViewItem);
            }
        }

        private void Cars_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var ssss = cars;
            Console.WriteLine();
        }
    }
}
