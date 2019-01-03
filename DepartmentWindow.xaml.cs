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
    /// Логика взаимодействия для DepartmentWindow.xaml
    /// </summary>
    public partial class DepartmentWindow : Window
    {
        /// <summary>
        /// Флаг "Форма модифицирована" - изменились реквизиты отдела, надо задать вопрос при закрытии формы
        /// </summary>
        private bool Modify;

        /// <summary>
        /// конструктор формы редактирования объекта Отдел
        /// </summary>
        /// <param name="dept">ссылка на существующий Отдел</param>
        public DepartmentWindow( ref DataRowView dept )
        {
            InitializeComponent();
  
            if (dept.IsNew) dept.Row[0] = Guid.NewGuid();
            this.DataContext = dept;
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
        /// обработка события - клик по кнопке "Закрыть"
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtClose_Click( object sender, RoutedEventArgs e )
        {
            if(Modify)
            {
                MessageBoxResult userAns = MessageBox.Show(this,  "Сохранить изменения?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes );

                if (userAns == MessageBoxResult.Yes) this.DialogResult = true;
                else DialogResult = false;
            }
            this.Close();
        }

        /// <summary>
        /// обработка события - изменения текста в TextBox TbName
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void TbName_TextChanged( object sender, TextChangedEventArgs e )
        {
            Modify = true;
        }

        /// <summary>
        /// обработка события - загрузка формы
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            tbName.TextChanged += TbName_TextChanged;
        }
    }
}
