using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace BorDer_PR14
{
    /// <summary>
    /// Логика взаимодействия для WritesList.xaml
    /// </summary>
    public partial class WritesList : Window
    {
        Entities db;
        MainWindow Пациенты;
        Пациент пациент;
        List<Запись> Writes;
        public WritesList(Entities db, Пациент пациент, MainWindow пациенты)
        {
            InitializeComponent();
            this.db = db;
            this.пациент = пациент;
            Пациенты = пациенты;

            patient.Text = пациент.ФИОКороткое;
            UpdateList();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Пациенты.Show();
        }

        private void buttonCancelWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.Запись.Remove(listWrites.SelectedItem as Запись);
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            UpdateList();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Пациенты.Show();
        }

        private void listWrites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonCancelWrite.IsEnabled = true;
        }

        public void UpdateList()
        {
            Writes = db.Запись.Where(x => x.Пациент.Код == пациент.Код).ToList();
            listWrites.ItemsSource = Writes;
        }
    }
}
