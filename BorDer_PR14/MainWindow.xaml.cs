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

namespace BorDer_PR14
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities db = new Entities();
        public MainWindow()
        {
            InitializeComponent();
            UpdateList();
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            Пациент selectedPatient = listPatients.SelectedItem as Пациент;
            selectedPatient.Фактический_адрес = Address.Text;
            selectedPatient.Номер_телефона = PhoneNumber.Text;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ((Button)sender).IsEnabled = false;
        }

        private void buttonWatchWrites_Click(object sender, RoutedEventArgs e)
        {
            if (listPatients.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите пациента!");
                return;
            }
            this.Hide();
            new WritesList(db, listPatients.SelectedItem as Пациент, this).Show();
        }

        private void buttonWritePatient_Click(object sender, RoutedEventArgs e)
        {
            if (listPatients.SelectedItem == null)
            {
                MessageBox.Show("Сначала выберите пациента!");
                return;
            }
            this.Hide();
            new WritePatient(db, listPatients.SelectedItem as Пациент, this).Show();
        }

        private void buttonAddPatient_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Surname.Text) ||
                String.IsNullOrEmpty(Name.Text) ||
                String.IsNullOrEmpty(Patronymic.Text) ||
                DateOfBirth.SelectedDate == null ||
                String.IsNullOrEmpty(Address.Text) ||
                String.IsNullOrEmpty(Polis.Text) ||
                String.IsNullOrEmpty(PhoneNumber.Text))
            {
                MessageBox.Show("Заполнены не все поля! Заполните их!");
                return;
            }

            if (Polis.Text.Length != 16)
            {
                MessageBox.Show("Полис должен состоять из 16 символов!");
                return;
            }

            if (PhoneNumber.Text.Length != 11)
            {
                MessageBox.Show("Номер телефона должен состоять из 11 символов!");
                return;
            }

            if (DateOfBirth.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Дата рождения не может быть больше текущей!");
                return;
            }




            Пациент новый_пациент = new Пациент();
            новый_пациент.Фамилия = Surname.Text;
            новый_пациент.Имя = Name.Text;
            новый_пациент.Отчество = Patronymic.Text;
            новый_пациент.Пол = Male.IsChecked == true ? "М" : "Ж";
            новый_пациент.Дата_рождения = DateOfBirth.SelectedDate;
            новый_пациент.Фактический_адрес = Address.Text;
            новый_пациент.Номер_полиса = Polis.Text;
            новый_пациент.Номер_телефона = PhoneNumber.Text;

            db.Пациент.Add(новый_пациент);
            db.SaveChanges();

            UpdateList();
            listPatients.SelectedIndex = 0;
        }

        private void listPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Пациент selected = (Пациент)listPatients.SelectedItem;

            Surname.Text = selected.Фамилия;
            Name.Text = selected.Имя;
            Patronymic.Text = selected.Отчество;
            Male.IsChecked = selected.Пол == "М" ? true : false;
            Female.IsChecked = selected.Пол == "Ж" ? true : false;
            DateOfBirth.SelectedDate = selected.Дата_рождения;
            Address.Text = selected.Фактический_адрес;
            Polis.Text = selected.Номер_полиса;
            PhoneNumber.Text = selected.Номер_телефона;

            buttonChange.IsEnabled = true;
        }

        public void UpdateList()
        {
            var patients = db.Пациент.ToList();
            listPatients.ItemsSource = patients;
        }
    }
}
