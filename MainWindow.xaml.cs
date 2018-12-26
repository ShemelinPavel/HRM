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

            DataLayer.Init();

            DeptList.DataContext = Department.Departments;
        }

        /// <summary>
        /// добавить Отдел в список
        /// </summary>
        /// <param name="d">ссылка на добавляемый Отдел</param>
        private void DepartmentAdd( Department d )
        {
            if (d != null && ( !( DeptList.Items.Contains( d ) ) ))
            {
                ( (ObservableCollection<Department>)DeptList.ItemsSource ).Add( d );
            }
        }

        /// <summary>
        /// добавить Сотрудник в список
        /// </summary>
        /// <param name="d">ссылка на добавляемого Сотрудника</param>
        private void EmployeeAdd( Employee e )
        {
            if (e != null && ( !( EmployeesList.Items.Contains( e ) ) ))
            {
                ( (ObservableCollection<Employee>)EmployeesList.ItemsSource ).Add( e );
            }
        }

        /// <summary>
        /// открыть форму редактирования отдела
        /// </summary>
        /// <param name="d">ссылка на существующий отдел</param>
        private void Department_Edit_WindowOpen( ref Department d )
        {
            DepartmentWindow win = new DepartmentWindow( ref d ) { Owner = this };
            win.Closed += Department_Edit_WindowClosed;
            win.ShowDialog();
        }

        /// <summary>
        /// открыть форму редактирования сотрудника
        /// </summary>
        /// <param name="e">ссылка на существующего сотрудника</param>
        private void Employee_Edit_WindowOpen( ref Employee e )
        {
            EmployeeWindow win = new EmployeeWindow( ref e ) { Owner = this };
            win.Closed += Employee_Edit_WindowClosed;
            win.ShowDialog();
        }


        /// <summary>
        /// обработка события - закрытие формы редактирование объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void Department_Edit_WindowClosed( object sender, EventArgs e )
        {
            Department curDept = ( (DepartmentWindow)sender ).CurrentDepartment;

            DepartmentAdd( curDept );
        }

        /// <summary>
        /// обработка события - закрытие формы редактирование объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void Employee_Edit_WindowClosed( object sender, EventArgs e )
        {
            Employee curEmpl = ( (EmployeeWindow)sender ).CurrentEmployee;

            EmployeeAdd( curEmpl );
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму нового объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtnDeptNew_Click( object sender, RoutedEventArgs e )
        {
            Department d = null;

            Department_Edit_WindowOpen( ref d );
        }

        /// <summary>
        /// обработка события создание нового объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtEmplNew_Click( object sender, RoutedEventArgs e )
        {
            Employee em = null;

            Employee_Edit_WindowOpen( ref em );
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму существующего объекта Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtDeptEdit_Click( object sender, RoutedEventArgs e )
        {
            Department d = (Department)DeptList.SelectedItem;

            Department_Edit_WindowOpen( ref d );
        }

        /// <summary>
        /// обработка события - открыть на редактирование форму существующего объекта Сотрудник
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void BtEmplEdit_Click( object sender, RoutedEventArgs e )
        {
            Employee em = (Employee)EmployeesList.SelectedItem;

            Employee_Edit_WindowOpen( ref em );
        }

        /// <summary>
        /// обработка события дв. щелчек мышью - редактирование существующего объета в списке объекта Отдел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeptList_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            Department d = (Department)DeptList.SelectedItem;

            if (d != null) Department_Edit_WindowOpen( ref d );
        }

        /// <summary>
        /// обработка события двойной щелчок мышью по выбранному из списка объекту Сотрудник (редактирование)
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void EmployeesList_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            Employee em = (Employee)EmployeesList.SelectedItem;

            if (em != null) Employee_Edit_WindowOpen( ref em );
        }

        /// <summary>
        /// обновление списка сотрудников
        /// </summary>
        public void EmployeesList_Refresh()
        {
            Department d = (Department)DeptList.SelectedItem;

            if (d != null) EmployeesList.DataContext = Employee.Employees.Where( x => x.Department == d );
            else EmployeesList.DataContext = null;
        }

        /// <summary>
        /// обработка события смена выбранной строки списка объектов Отдел
        /// </summary>
        /// <param name="sender">объект-поставщик события</param>
        /// <param name="e">параметры события</param>
        private void DeptList_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            EmployeesList_Refresh();
        }

        private void BtDeptDel_Click( object sender, RoutedEventArgs e )
        {
            Department d = (Department)DeptList.SelectedItem;
            int dIndex = DeptList.Items.IndexOf( d );

            if (d != null)
            {
                int emplCount = Employee.Employees.Count( x => x.Department == d );

                if (emplCount != 0)
                {
                    string t = ( emplCount == 1 ) ? "сотрудника" : "сотрудников";
                    MessageBoxResult userAns = MessageBox.Show( this, $"Данный отдел <{d.Name}> указан у {emplCount} {t}.\nУдаление отдела приведет к удалению данных сотрудников.\nПродолжить удаление?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes );

                    if (userAns == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                DataLayer.DepartmentCascadeDelete( ref d );

                if (DeptList.Items.Count - 1 < dIndex) dIndex--;
                if (dIndex >= 0)DeptList.SelectedItem = DeptList.Items[dIndex];
            }
        }

        private void BtEmplDel_Click( object sender, RoutedEventArgs e )
        {
            Employee em = (Employee)EmployeesList.SelectedItem;
            int dIndex = EmployeesList.Items.IndexOf( em );

            if (em != null)
            {
                DataLayer.EmployeeDelete( ref em );
                EmployeesList_Refresh();

                if (EmployeesList.Items.Count - 1 < dIndex) dIndex--;
                if (dIndex >= 0) EmployeesList.SelectedItem = EmployeesList.Items[dIndex];
            }
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {

            bool res = DataLayer.Dept_CreateTable(out Exception ex);

            if (!( res )) MessageBox.Show( ex.Message );

            bool res1 = DataLayer.Emp_CreateTable( out Exception ex1 );

            if (!( res1 )) MessageBox.Show( ex1.Message );


        }
    }
}