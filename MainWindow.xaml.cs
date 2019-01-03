using System;
using System.Collections.ObjectModel;
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
using System.Data;
using System.Data.SqlClient;


namespace HRM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// открыть форму редактирования отдела
        /// </summary>
        /// <param name="drv">ссылка на существующий отдел</param>
        private void Department_Edit_WindowOpen( ref DataRowView drv )
        {
            DepartmentWindow win = new DepartmentWindow( ref drv ) { Owner = this };
            Nullable<bool> res = win.ShowDialog();

            if (res.HasValue && res == true)
            {
                Exception exception = null;

                drv.EndEdit();
                DataLayer.Dept_SaveTable( ( (DataView)DeptList.DataContext ).Table, out exception );

                if (!( exception == null )) MessageBox.Show( exception.Message );
            }
        }

        /// <summary>
        /// открыть форму редактирования сотрудника
        /// </summary>
        /// <param name="e">ссылка на существующего сотрудника</param>
        private void Employee_Edit_WindowOpen( ref DataRowView drv )
        {
            EmployeeWindow win = new EmployeeWindow( ref drv ) { Owner = this };
            Nullable<bool> res = win.ShowDialog();

            if (res.HasValue && res == true)
            {
                Exception exception = null;

                drv.EndEdit();
                DataLayer.Emp_SaveTable( ( (DataView)EmployeesList.ItemsSource ).Table, out exception );

                if (!( exception == null )) MessageBox.Show( exception.Message );
            }
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму нового объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtnDeptNew_Click( object sender, RoutedEventArgs e )
        {
            DataView dv = DeptList.DataContext as DataView;
            DataRowView drv = dv.AddNew();
            drv.BeginEdit();

            Department_Edit_WindowOpen( ref drv );

            DataRefresh();
        }

        /// <summary>
        /// обработка события создание нового объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtEmplNew_Click( object sender, RoutedEventArgs e )
        {
            DataView dv = EmployeesList.ItemsSource as DataView;
            DataRowView drv = dv.AddNew();
            drv.BeginEdit();

            Employee_Edit_WindowOpen( ref drv );

            DataRefresh();
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму существующего объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtDeptEdit_Click( object sender, RoutedEventArgs e )
        {
            DataRowView drv = (DataRowView)DeptList.SelectedItem;
            drv.BeginEdit();
            Department_Edit_WindowOpen( ref drv );
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму существующего объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtEmplEdit_Click( object sender, RoutedEventArgs e )
        {
            DataRowView drv = (DataRowView)EmployeesList.SelectedItem;
            drv.BeginEdit();
            Employee_Edit_WindowOpen( ref drv );
        }

        /// <summary>
        /// обработка события дв. щелчек мышью - редактирование существующего объета в списке объекта Отдел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeptList_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            DataRowView drv = (DataRowView)DeptList.SelectedItem;

            if (drv != null) Department_Edit_WindowOpen( ref drv );
        }

        /// <summary>
        /// обработка события двойной щелчок мышью по выбранному из списка объекту Сотрудник (редактирование)
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void EmployeesList_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            DataRowView drv = EmployeesList.SelectedItem as DataRowView;

            if (drv != null) Employee_Edit_WindowOpen( ref drv );
        }

        /// <summary>
        /// событие удаление объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtDeptDel_Click( object sender, RoutedEventArgs e )
        {
            DataRowView drv = DeptList.SelectedItem as DataRowView;

            MessageBoxResult userAus = MessageBox.Show( this, "Удаление отдела приведет к удалению всех сортудников в нем.\nПродолжить удаление?", "Предупреждение:", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes );
            if (userAus == MessageBoxResult.Yes)
            {
                drv.Delete();
                DataLayer.Dept_SaveTable( ( (DataView)DeptList.DataContext ).Table, out Exception exception );

                if (!( exception == null )) MessageBox.Show( exception.Message );
            }
        }

        /// <summary>
        /// событие удаление объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtEmplDel_Click( object sender, RoutedEventArgs e )
        {
            DataRowView drv = EmployeesList.SelectedItem as DataRowView;

            drv.Delete();
            DataLayer.Emp_SaveTable( ( (DataView)EmployeesList.ItemsSource ).Table, out Exception exception );

            if (!( exception == null )) MessageBox.Show( exception.Message );
        }

        /// <summary>
        /// создание таблиц в БД
        /// </summary>
        /// <param name="sender">объект-источник события</param>
        /// <param name="e">параметры события</param>
        private void btCreateBaseTables_Click( object sender, RoutedEventArgs e )
        {
            DataLayer.Dept_CreateTable( out Exception ex );

            if (ex != null) MessageBox.Show( ex.Message );
            else MessageBox.Show( "Создана таблица Departments" );

            DataLayer.Emp_CreateTable( out Exception ex1 );

            if (ex1 != null) MessageBox.Show( ex1.Message );
            else MessageBox.Show( "Создана таблица Employees" );

            CheckTablesExists( true );
        }

        /// <summary>
        /// заполнение БД демонстрационными данными
        /// </summary>
        /// <param name="sender">объект-источник события</param>
        /// <param name="e">параметры события</param>
        private void btFillDemoBase_Click( object sender, RoutedEventArgs e )
        {
            DataLayer.Dept_InsertDemoData( out Exception ex );

            if (ex != null) MessageBox.Show( ex.Message );
            else MessageBox.Show( "Заполнена таблица Departments" );

            DataLayer.Empl_InsertDemoData( out Exception ex1 );

            if (ex1 != null) MessageBox.Show( ex1.Message );
            else MessageBox.Show( "Заполнена таблица Employees" );
        }

        /// <summary>
        /// событие - загрузка окна
        /// </summary>
        /// <param name="sender">объект-источник события</param>
        /// <param name="e">параметры события</param>
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            if (!( CheckTablesExists() ))
            {
                return;
            }
            else
            {
                DataRefresh();
            }
        }

        /// <summary>
        /// обновление источников данных
        /// </summary>
        private void DataRefresh()
        {
            DataSet ds = DataLayer.GetDataSet_DepartmentsAndEmployees( out Exception exception );

            if (exception == null)
            {
                DeptList.DataContext = ds.Tables["Departments"].DefaultView;
                EmployeesList.DataContext = ds.Tables["Departments"].DefaultView;

            }
            else
            {
                MessageBox.Show( exception.Message );
            }
        }

        /// <summary>
        /// проверка существования таблиц в БД + реакция gui на существование
        /// </summary>
        /// <param name="mute">не сообщать о результате проверки пользователю</param>
        /// <returns>таблицы существует/не существует</returns>
        private bool CheckTablesExists( bool mute = false )
        {
            bool res = true;

            if (!( TableExist( "Departments", mute ) && TableExist( "Employees", mute ) )) res = false;

            ItemsButtonsOnOff( res );

            return res;
        }

        /// <summary>
        /// проверка существования таблицы в БД
        /// </summary>
        /// <param name="tName">имя таблицы</param>
        /// /// <param name="mute">сообщать пользователю что таблицы нет</param>
        /// <returns>таблица существует/не существует</returns>
        private bool TableExist( string tName, bool mute )
        {
            bool tableExist = DataLayer.CheckTableExist( tName, out Exception exceptionD );

            if (exceptionD != null)
            {
                MessageBox.Show( exceptionD.Message );
                return false;
            }
            else
            {
                if (( !( mute ) ) && !( tableExist )) MessageBox.Show( $"Таблица: {tName} не найдена в базе данных!", "Ошибка:", MessageBoxButton.OK, MessageBoxImage.Error );
            }

            return tableExist;
        }

        /// <summary>
        /// включить/выключить доступность кнопок редактирования отделов и сотрудников
        /// </summary>
        /// <param name="btEnable">вкл/выкл</param>
        private void ItemsButtonsOnOff( bool btEnable )
        {
            grDepts.IsEnabled = btEnable;
            grEmps.IsEnabled = btEnable;
        }
    }
}