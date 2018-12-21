using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HRM
{
    #region Сотрудник
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee : INotifyPropertyChanged
    {
        /// <summary>
        /// событие - реализация интерфейса INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// коллекция всех объектов Сотрудник
        /// </summary>
        public static ObservableCollection<Employee> Employees;

        /// <summary>
        /// Гуид объекта Сотрудник
        /// </summary>
        private Guid emplguid;

        /// <summary>
        /// Фамилия объекта Сотрудник
        /// </summary>
        private string lname;

        /// <summary>
        /// Имя объекта Сотрудник
        /// </summary>
        private string name;

        /// <summary>
        /// место работы - Отдел объекта Сотрудник
        /// </summary>
        private Department dept;

        /// <summary>
        /// Гуид объекта Сотрудник
        /// </summary>
        public Guid EmployeeGuid => this.emplguid;

        /// <summary>
        /// Фамилия объекта Сотрудник
        /// </summary>
        public string LastName
        {
            get { return this.lname; }
            set
            {
                this.lname = value;

                if(this.PropertyChanged != null) this.PropertyChanged( this, new PropertyChangedEventArgs( nameof(this.LastName ) ) );
            }
        }

        /// <summary>
        /// Имя объекта Сотрудник
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;

                if (this.PropertyChanged != null) this.PropertyChanged( this, new PropertyChangedEventArgs( nameof( this.Name ) ) );
            }
        }

        /// <summary>
        /// стат. конструктор объекта Сотрудник
        /// </summary>
        static Employee()
        {
            Employees = new ObservableCollection<Employee>();
        }

        /// <summary>
        /// Отдел объекта Сотрудник
        /// </summary>
        public Department Department
        {
            get { return this.dept; }
            set
            {
                this.dept = value;

                if (this.PropertyChanged != null) this.PropertyChanged( this, new PropertyChangedEventArgs( nameof( this.Department ) ) );
            }
        }

        /// <summary>
        /// Конструктор объекта Сотрудник
        /// </summary>
        /// <param name="lname">Фамилия</param>
        /// <param name="name">Имя</param>
        public Employee( string lname, string name ) : this( lname, name, Guid.Empty, Guid.Empty )
        {
        }

        /// <summary>
        /// Конструктор объекта Сотрудник
        /// </summary>
        /// <param name="lname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="deptGuid">Гуид объекта Отдел</param>
        public Employee( string lname, string name, Guid deptGuid ) : this( lname, name, deptGuid, Guid.Empty )
        {
        }

        /// <summary>
        /// Конструктор объекта Сотрудник
        /// </summary>
        /// <param name="lname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="deptGuid">Гуид объекта Отдел</param>
        /// <param name="guid">Гуид объекта Сотрудник</param>
        public Employee( string lname, string name, Guid deptGuid, Guid guid )
        {
            this.emplguid = ( guid == Guid.Empty ) ? Guid.NewGuid() : guid;
            LastName = lname;
            Name = name;
            Department = ( deptGuid == Guid.Empty ) ? null : Department.GetDepartmentByGuid( deptGuid );

            //добавляем в общую коллекцию
            Employees.Add( this );
        }

        /// <summary>
        /// Строковое представление объекта Сотрудник
        /// </summary>
        /// <returns>Строковое представление</returns>
        public override string ToString()
        {
            return $"Сотрудник. Фамилия:{LastName} Имя:{Name}";
        }
    }
    #endregion Сотрудник

    #region Отдел
    /// <summary>
    /// Отдел
    /// </summary>
    public class Department : INotifyPropertyChanged
    {
        /// <summary>
        /// событие - реализация интерфейса INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// коллекция всех объектов Отдел
        /// </summary>
        public static ObservableCollection<Department> Departments;

        /// <summary>
        /// Гуид объекта Отдел
        /// </summary>
        private Guid deptguid;

        /// <summary>
        /// Наименование объекта Отдела
        /// </summary>
        private string name;

        /// <summary>
        /// Гуид объекта Отдел
        /// </summary>
        public Guid DepartmentGuid => this.deptguid;

        /// <summary>
        /// Наименование объекта Отдел
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;

                if(this.PropertyChanged != null) this.PropertyChanged( this, new PropertyChangedEventArgs( nameof( this.Name ) ) );
            }
        }

        /// <summary>
        /// стат. конструктор объекта Отдел
        /// </summary>
        static Department()
        {
            Departments = new ObservableCollection<Department>();
        }

        /// <summary>
        /// Конструктор объекта Отдела
        /// </summary>
        /// <param name="name">Наименование</param>
        public Department( string name ) : this( name, Guid.Empty )
        {
        }

        /// <summary>
        /// Конструктор объекта Отдела
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="guid">Гуид отдела</param>
        public Department( string name, Guid guid )
        {
            this.deptguid = ( guid == Guid.Empty ) ? Guid.NewGuid() : guid;
            Name = name;

            //добавляем в общую коллекцию
            Departments.Add( this );
        }

        /// <summary>
        /// найти Отдел из общей коллекции по его Guid-у
        /// </summary>
        /// <param name="guid">Гуид отдела</param>
        /// <returns>объект Отдел</returns>
        public static Department GetDepartmentByGuid( Guid guid )
        {
            Department d = Departments.First( x => x.DepartmentGuid == guid );

            if (d == null)
            {
                d = new Department( $"Объект не найден: <{guid}>", guid );
            }

            return d;
        }

        /// <summary>
        /// Строковое представление объекта Отдела
        /// </summary>
        /// <returns>Строковое представление</returns>
        public override string ToString()
        {
            return $"Отдел. Наименование: {Name}";
        }
    }
    #endregion Отдел
}