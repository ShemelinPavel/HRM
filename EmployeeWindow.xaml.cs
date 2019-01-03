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
using System.Data;

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
        /// конструктор Сотрудник
        /// </summary>
        /// <param name="em">ссылка на текущего сотрудника</param>
        public EmployeeWindow( ref DataRowView em )
        {
            InitializeComponent();

            if (em.IsNew) em.Row[0] = Guid.NewGuid();

            this.DataContext = em;
            cbDepartment.DataContext = em.DataView.Table.DataSet.Tables["Departments"].DefaultView;
            cbDepartment.SelectedValue = em.Row.ItemArray[3];
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

                if (userAns == MessageBoxResult.Yes) this.DialogResult = true;
                else DialogResult = false;
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
            Modify = false;
            this.DialogResult = true;
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
            ( (DataRowView)this.DataContext ).Row[3] = cbDepartment.SelectedValue;
        }
    }
}
