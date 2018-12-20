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

namespace HRM
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        /// <summary>
        /// Флаг "Форма модифицирована" - изменились реквизиты отдела, надо задать вопрос при закрытии формы
        /// </summary>
        private bool Modify;

        /// <summary>
        /// текущий Сотрудник
        /// </summary>
        public Employee CurrentEmployee { get; set; }

        /// <summary>
        /// конструктор Сотрудник
        /// </summary>
        /// <param name="em">ссылка на текущего сотрудника</param>
        public EmployeeWindow( ref Employee em )
        {
            InitializeComponent();

            CurrentEmployee = em;

            cbDepartment.ItemsSource = Department.Departments;
            cbDepartment.SelectedItem = CurrentEmployee?.Department;

            this.DataContext = CurrentEmployee;
        }

        /// <summary>
        /// сохраняем изменения текущего объекта Сотрудник
        /// </summary>
        private void CurrentEmployeeSave()
        {

            if (CurrentEmployee == null) // если пустой - создаем нового сотрудника
            {
                CurrentEmployee = new Employee( tbLName.Text, tbName.Text, ((Department)cbDepartment.SelectedItem == null) ? Guid.Empty : ((Department)cbDepartment.SelectedItem).DepartmentGuid );
            }
            else
            {
                CurrentEmployee.LastName = tbLName.Text;
                CurrentEmployee.Name = tbName.Text;
                CurrentEmployee.Department = (Department)cbDepartment.SelectedItem;
            }
        }

        /// <summary>
        /// обработка события - клик по кнопке "Закрыть"
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void TbClose_Click( object sender, RoutedEventArgs e )
        {
            if (Modify)
            {
                MessageBoxResult userAns = MessageBox.Show( this, "Сохранить изменения?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes );

                if (userAns == MessageBoxResult.Yes) CurrentEmployeeSave();
            }
            this.Close();

        }

        /// <summary>
        /// обработка события - клик по кнопке "Записать"
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtSave_Click( object sender, RoutedEventArgs e )
        {
            CurrentEmployeeSave();
            Modify = false;
        }

        /// <summary>
        /// обработка события - загрузка формы
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            tbLName.TextChanged += TbLName_TextChanged;
            tbName.TextChanged += TbName_TextChanged;
            cbDepartment.SelectionChanged += CbDepartment_SelectionChanged;
        }

        /// <summary>
        /// обработка события - изменения текста в поле tbLName
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void TbLName_TextChanged( object sender, TextChangedEventArgs e )
        {
            Modify = true;
        }

        /// <summary>
        /// обработка события - изменения текста в поле tbName
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void TbName_TextChanged( object sender, TextChangedEventArgs e )
        {
            Modify = true;
        }

        /// <summary>
        /// обработка события - изменения выбранного значения Департамент в поле cbDepartment
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void CbDepartment_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            Modify = true;
        }
    }
}
