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
using System.Windows.Shapes;

namespace BorDer_PR14
{
    /// <summary>
    /// Логика взаимодействия для WritePatient.xaml
    /// </summary>
    public partial class WritePatient : Window
    {
        Entities db;
        MainWindow Пациенты;
        Пациент пациент;

        public WritePatient(Entities db, Пациент пациент, MainWindow пациенты)
        {
            InitializeComponent();
            this.db = db;
            this.пациент = пациент;
            Пациенты = пациенты;

            Patient.Text = пациент.ФИОКороткое;

            FillTimeComboBox();

            var listSpec = db.Специальность.ToList();
            Speciality.ItemsSource = listSpec;
        }

        private void Speciality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Speciality.Items.Count == 0) return;
            var item = (sender as ComboBox).SelectedItem as Специальность;
            Doctor.ItemsSource = db.Врач.Where(x => x.Специальность_Код == item.Код).ToList();
        }

        private void FillTimeComboBox()
        {
            timeCombo.Items.Clear();

            // Начало в 08:00, конец в 20:00, шаг 15 мин
            TimeSpan start = TimeSpan.FromHours(8);
            TimeSpan end = TimeSpan.FromHours(20);
            TimeSpan step = TimeSpan.FromMinutes(15);

            for (TimeSpan t = start; t <= end; t = t.Add(step))
            {
                timeCombo.Items.Add(t);
            }

            // Чтобы в выпадающем списке было красиво (08:00), а не 08:00:00
            timeCombo.ItemStringFormat = @"hh\:mm";
            timeCombo.SelectedIndex = 0;
        }

        //Запись
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Speciality.SelectedItem == null || Doctor.SelectedItem == null || Date.SelectedDate == null || timeCombo.SelectedItem == null)
            {
                MessageBox.Show("Заполнены не все поля!");
                return;
            }

            if (Date.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Дата записи не может быть меньше текущей!");
                return;
            }

            try
            {
                Запись запись = new Запись();
                запись.Пациент_Код = пациент.Код;
                запись.Врач_Код = (Doctor.SelectedItem as Врач).Код;
                запись.Дата_записи = Date.SelectedDate.Value;
                запись.Время_записи = (TimeSpan)timeCombo.SelectedItem;
                db.Запись.Add(запись);
                db.SaveChanges();


                this.Close();
                Пациенты.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Пациенты.Show();
        }
    }
}
