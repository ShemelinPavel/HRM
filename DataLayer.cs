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
                                        ADD CONSTRAINT FK_DepartmentId FOREIGN KEY(DepartmentId) REFERENCES Departments (Id) ON DELETE CASCADE;";

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

        /// <summary>
        /// проверка наличия таблицы в БД
        /// </summary>
        /// <param name="tableName">имя таблицы</param>
        /// <param name="except">исключение</param>
        /// <returns></returns>
        public static bool CheckTableExist( string tableName, out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {

                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                    return false;
                }
                else
                {
                    SqlCommand sqlCom = new SqlCommand( $@"IF EXISTS( SELECT 1
                                                        FROM INFORMATION_SCHEMA.TABLES
                                                        WHERE TABLE_TYPE = 'BASE TABLE'
                                                        AND TABLE_NAME = '{tableName}' )
                                                        SELECT 1 AS res ELSE SELECT 0 AS res;", sqlCon );

                    sqlCon.Open();
                    int result = (int)sqlCom.ExecuteScalar();

                    except = null;
                    return ( result == 1 ) ? true : false;
                }
            }
        }

        /// <summary>
        /// датасет с таблицами отделы/сотрудники сконфигурированные в отношение parent/child
        /// </summary>
        /// <param name="except">исключение</param>
        /// <returns>датасет</returns>
        public static DataSet GetDataSet_DepartmentsAndEmployees( out Exception except )
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
                    sqlCon.Open();

                    SqlCommand cmd = new SqlCommand( "SELECT Id, Name FROM Departments ORDER BY Name ASC", sqlCon );

                    SqlDataAdapter adapter = new SqlDataAdapter( cmd );

                    DataSet ds = new DataSet();
                    adapter.Fill( ds, "Departments" );
                    cmd.CommandText = "SELECT Employees.Id as Id, Employees.LastName as LastName, Employees.Name as Name, Employees.DepartmentId as DepartmentId, Departments.Name as DepartmentsName FROM Departments left join Employees on Departments.Id = Employees.DepartmentId";
                    adapter.Fill( ds, "Employees" );

                    //отношения между таблицами
                    DataRelation relDeptsEmps = new DataRelation( "DeptsEmpsRelation", ds.Tables["Departments"].Columns["Id"], ds.Tables["Employees"].Columns["DepartmentId"] );
                    ds.Relations.Add( relDeptsEmps );

                    except = null;
                    return ds;
                }
            }
        }

        /// <summary>
        /// сохранение таблицы Отделы
        /// </summary>
        /// <param name="table">таблица Отделы</param>
        /// <param name="except">исключение</param>
        /// <returns></returns>
        public static bool Dept_SaveTable( DataTable table, out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {

                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                    return false;
                }
                else
                {
                    SqlCommand cmdUpdate = new SqlCommand( "UPDATE Departments SET Name = @Name  WHERE Id = @Id", sqlCon );
                    SqlCommand cmdInsert = new SqlCommand( "INSERT INTO Departments (Name, Id) Values(@Name, @Id)", sqlCon );
                    SqlCommand cmdDel = new SqlCommand( "DELETE Departments WHERE Id = @Id", sqlCon );

                    try
                    {
                        sqlCon.Open();
                        DataTable dtM = table.GetChanges( DataRowState.Modified );
                        if (dtM != null)
                        {
                            foreach (DataRow item in dtM.Rows)
                            {
                                cmdUpdate.Parameters.Clear();
                                cmdUpdate.Parameters.AddWithValue( "@Name", item["Name"] );
                                cmdUpdate.Parameters.AddWithValue( "@Id", item["Id"] );
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }

                        DataTable dtA = table.GetChanges( DataRowState.Added );
                        if (dtA != null)
                        {
                            foreach (DataRow item in dtA.Rows)
                            {
                                cmdInsert.Parameters.Clear();
                                cmdInsert.Parameters.AddWithValue( "@Name", item["Name"] );
                                cmdInsert.Parameters.AddWithValue( "@Id", item["Id"] );
                                cmdInsert.ExecuteNonQuery();
                            }
                        }

                        DataTable dtD = table.GetChanges( DataRowState.Deleted );
                        if (dtD != null)
                        {
                            foreach (DataRow item in dtD.Rows)
                            {
                                cmdDel.Parameters.Clear();
                                cmdDel.Parameters.AddWithValue( "@Id", item["Id", DataRowVersion.Original] );
                                cmdDel.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        except = ex;
                        return false;
                    }
                    except = null;
                    return true;
                }
            }
        }

        /// <summary>
        /// сохранение таблицы Сотрудники
        /// </summary>
        /// <param name="table">таблица Сотрудники</param>
        /// <param name="except">исключение</param>
        /// <returns>результат</returns>
        public static bool Emp_SaveTable( DataTable table, out Exception except )
        {
            using (SqlConnection sqlCon = GetSqlConnection( out Exception except_GetSqlConnection ))
            {

                if (sqlCon == null)
                {
                    except = except_GetSqlConnection;
                    return false;
                }
                else
                {
                    SqlCommand cmdUpdate = new SqlCommand( "UPDATE Employees SET LastName = @LastName, Name = @Name, DepartmentId = @DepartmentId WHERE Id = @Id", sqlCon );
                    SqlCommand cmdInsert = new SqlCommand( "INSERT INTO Employees (LastName, Name, Id, DepartmentId) Values(@LastName, @Name, @Id, @DepartmentId)", sqlCon );
                    SqlCommand cmdDel = new SqlCommand( "DELETE Employees WHERE Id = @Id", sqlCon );

                    try
                    {
                        sqlCon.Open();
                        DataTable dtM = table.GetChanges( DataRowState.Modified );
                        if (dtM != null)
                        {
                            foreach (DataRow item in dtM.Rows)
                            {
                                cmdUpdate.Parameters.Clear();
                                cmdUpdate.Parameters.AddWithValue( "@LastName", item["LastName"] );
                                cmdUpdate.Parameters.AddWithValue( "@Name", item["Name"] );
                                cmdUpdate.Parameters.AddWithValue( "@Id", item["Id"] );
                                cmdUpdate.Parameters.AddWithValue( "@DepartmentId", item["DepartmentId"] );
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }

                        DataTable dtA = table.GetChanges( DataRowState.Added );
                        if (dtA != null)
                        {
                            foreach (DataRow item in dtA.Rows)
                            {
                                cmdInsert.Parameters.Clear();
                                cmdInsert.Parameters.AddWithValue( "@LastName", item["LastName"] );
                                cmdInsert.Parameters.AddWithValue( "@Name", item["Name"] );
                                cmdInsert.Parameters.AddWithValue( "@Id", item["Id"] );
                                cmdInsert.Parameters.AddWithValue( "@DepartmentId", item["DepartmentId"] );
                                cmdInsert.ExecuteNonQuery();
                            }
                        }

                        DataTable dtD = table.GetChanges( DataRowState.Deleted );
                        if (dtD != null)
                        {
                            foreach (DataRow item in dtD.Rows)
                            {
                                cmdDel.Parameters.Clear();
                                cmdDel.Parameters.AddWithValue( "@Id", item["Id", DataRowVersion.Original] );
                                cmdDel.ExecuteNonQuery();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        except = ex;
                        return false;
                    }
                    except = null;
                    return true;
                }
            }

        }
    }
}