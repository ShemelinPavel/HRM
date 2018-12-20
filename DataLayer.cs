using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        static void DeptsLoad()
        {
            new Department( "Администрация", Guid.Parse( "D772844B-07DC-4DC1-B4BC-D168699BB0B4" ) );
            new Department( "Бухгалтерия", Guid.Parse( "1134BC2A-8406-4503-A401-BD5B94C936DB" ) );
            new Department( "Цех №1", Guid.Parse( "50D15AB8-9A12-4AA0-8FBF-BF3EE662F0D6" ) );
            new Department( "Цех №2", Guid.Parse( "94CD5301-BD0A-4BBF-BF33-1A9DCC2F3D3E" ) );
        }

        /// <summary>
        /// генерация Сотрудников
        /// </summary>
        static void EmplLoad()
        {
            new Employee("Иванов", "Иван", Guid.Parse("D772844B-07DC-4DC1-B4BC-D168699BB0B4"));
            new Employee( "Сидоров", "Сидор", Guid.Parse( "D772844B-07DC-4DC1-B4BC-D168699BB0B4" ) );
            new Employee( "Петров", "Петр", Guid.Parse( "50D15AB8-9A12-4AA0-8FBF-BF3EE662F0D6" ) );
            new Employee( "Кузин", "Юрий", Guid.Parse( "94CD5301-BD0A-4BBF-BF33-1A9DCC2F3D3E" ) );
            new Employee( "Миронова", "Анастасия", Guid.Parse( "1134BC2A-8406-4503-A401-BD5B94C936DB" ) );
        }

        /// <summary>
        /// инициализация - загрузка данных
        /// </summary>
        public static void Init()
        {
            DataLayer.DeptsLoad();
            DataLayer.EmplLoad();
        }
    }
}