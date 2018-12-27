using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HRM
{
    /// <summary>
    /// слой данных
    /// </summary>
    public static class DataLayer
    {
        /// <summary>
        /// генерация Отделов
        /// </summary>
        static Department[] DeptsLoad()
        {
            Department[] d =
                {

                new Department( "Администрация", Guid.Parse( "D772844B-07DC-4DC1-B4BC-D168699BB0B4" ) ),
                new Department( "Бухгалтерия", Guid.Parse( "1134BC2A-8406-4503-A401-BD5B94C936DB" ) ),
                new Department( "Цех №1", Guid.Parse( "50D15AB8-9A12-4AA0-8FBF-BF3EE662F0D6" ) ),
                new Department( "Цех №2", Guid.Parse( "94CD5301-BD0A-4BBF-BF33-1A9DCC2F3D3E" ) )
            };

            return d;
        }

        /// <summary>
        /// генерация Сотрудников
        /// </summary>
        static Employee[] EmplLoad()
        {
            Employee[] em = {

            new Employee( "Иванов", "Иван", Guid.Parse( "D772844B-07DC-4DC1-B4BC-D168699BB0B4" ) ),
            new Employee( "Сидоров", "Сидор", Guid.Parse( "D772844B-07DC-4DC1-B4BC-D168699BB0B4" ) ),
            new Employee( "Петров", "Петр", Guid.Parse( "50D15AB8-9A12-4AA0-8FBF-BF3EE662F0D6" ) ),
            new Employee( "Кузин", "Юрий", Guid.Parse( "94CD5301-BD0A-4BBF-BF33-1A9DCC2F3D3E" ) ),
            new Employee( "Миронова", "Анастасия", Guid.Parse( "1134BC2A-8406-4503-A401-BD5B94C936DB" ) )
            };

            return em;

        }


        /// <summary>
        /// инициализация - загрузка данных
        /// </summary>
        public static void Init()
        {
            //DataLayer.DeptsLoad();
            //DataLayer.EmplLoad();
        }

        /// <summary>
        /// каскадное удаление объекта Отдел и объектов Сотрудник связанных с ним
        /// </summary>
        /// <param name="d">ссылка на Отдел</param>
        public static void DepartmentCascadeDelete( ref Department d )
        {
            Guid guid = d.Id;
            Employee[] emplArray = Employee.Employees.Where( x => x.Department.Id == guid ).ToArray();

            foreach (Employee item in emplArray)
            {
                Employee.Employees.Remove( item );
            }

            Department.Departments.Remove( d );
        }

        /// <summary>
        /// удаление объекта Сотрудник из общей коллекци
        /// </summary>
        /// <param name="e">ссылка на Сотрудник</param>
        public static void EmployeeDelete( ref Employee e )
        {
            Employee.Employees.Remove( e );
        }

        /// <summary>
        /// создает подключение (SqlConnection) к базе данных
        /// </summary>
        /// <param name="except">перехваченное исключение</param>
        /// <returns>объект типа SqlConnection</returns>
        public static SqlConnection GetSqlConnection( out Exception except )
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["HRM"];
            except = null;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = new SqlConnection( settings.ConnectionString );

            }
            catch (Exception ex)
            {
                except = ex;
                sqlCon = null;
            }
            return sqlCon;
        }

        /// <summary>
        /// создать в БД таблицу Departments
        /// </summary>
        /// <param name="except">перехваченное исключение</param>
        public static void Dept_CreateTable( out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {
                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                }
                else
                {
                    string createExp = @"CREATE TABLE[dbo].[Departments](
                                    [Id]   UNIQUEIDENTIFIER NOT NULL,
                                    [Name] NCHAR(20)       NOT NULL,
                                    PRIMARY KEY CLUSTERED( [Id] ASC));";

                    try
                    {
                        SqlCommand sqlCom = new SqlCommand( createExp, sqlCon );
                        sqlCon.Open();
                        int result = sqlCom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        except = ex;
                    }
                    except = null;
                }
            }
        }

        /// <summary>
        /// создать в БД таблицу Employees
        /// </summary>
        /// <param name="except">перехваченное исключение</param>
        public static void Emp_CreateTable( out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {
                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                }
                else
                {
                    string createExp = @"CREATE TABLE Employees
                                        (
                                        [Id] UNIQUEIDENTIFIER NOT NULL,
                                        [LastName] NCHAR(50)       NOT NULL,
                                        [Name]         NCHAR(50)       NOT NULL,
                                        [DepartmentId] UNIQUEIDENTIFIER NOT NULL,
                                        PRIMARY KEY CLUSTERED( [Id] ASC)
                                        );
                                        ALTER TABLE Employees
                                        WITH NOCHECK
                                        ADD CONSTRAINT FK_DepartmentId FOREIGN KEY(DepartmentId) REFERENCES Departments (Id);";

                    try
                    {
                        SqlCommand sqlCom = new SqlCommand( createExp, sqlCon );
                        sqlCon.Open();
                        int result = sqlCom.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        except = ex;
                    }
                    except = null;
                }
            }
        }

        /// <summary>
        ///  заполнить таблицу Departments тестовыми данными
        /// </summary>
        /// <param name="except">перехваченное исключение</param>
        public static void Dept_InsertDemoData( out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {
                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                }
                else
                {
                    string insertExp = @"INSERT INTO Departments (Id, Name) VALUES(@Id, @Name);";
                    SqlCommand sqlCom = new SqlCommand( insertExp, sqlCon );

                    Department[] depts = DeptsLoad();

                    try
                    {
                        sqlCon.Open();

                        foreach (Department item in depts)
                        {
                            sqlCom.Parameters.Clear();
                            sqlCom.Parameters.AddWithValue( "@Id", item.Id );
                            sqlCom.Parameters.AddWithValue( "@Name", item.Name );

                            int res = sqlCom.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        except = ex;
                    }
                    except = null;
                }
            }
        }

        /// <summary>
        ///  заполнить таблицу Employees тестовыми данными
        /// </summary>
        /// <param name="except">перехваченное исключение</param>
        public static void Empl_InsertDemoData( out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {
                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                }
                else
                {
                    string insertExp = @"INSERT INTO Employees (Id, LastName, Name, DepartmentId) VALUES(@Id, @LastName, @Name, @DepartmentId);";
                    SqlCommand sqlCom = new SqlCommand( insertExp, sqlCon );

                    Employee[] empls = EmplLoad();

                    try
                    {
                        sqlCon.Open();

                        foreach (Employee item in empls)
                        {
                            sqlCom.Parameters.Clear();
                            sqlCom.Parameters.AddWithValue( "@Id", item.Id );
                            sqlCom.Parameters.AddWithValue( "@LastName", item.LastName );
                            sqlCom.Parameters.AddWithValue( "@Name", item.Name );
                            sqlCom.Parameters.AddWithValue( "@DepartmentId", item.Department.Id );

                            int res = sqlCom.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        except = ex;
                    }
                    except = null;
                }
            }
        }

        public static DataTable GetDataTable_Dept( out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {

                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                    return null;
                }
                else
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand( "SELECT ID, Name FROM Departments", sqlCon );
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill( dt );

                    except = null;
                    return dt;
                }
            }
        }
    }
}