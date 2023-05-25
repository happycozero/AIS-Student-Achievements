using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Achievements.Classes
{
    class DB_Connect : IDisposable
    {
        private readonly MySqlConnection connection;

        public DB_Connect()
        {
            connection = new MySqlConnection("host=localhost;uid=root;pwd=;database=student_achievements;");
            OpenConnect();
        }
        public void Dispose()
        {
            CloseConnect();
        }

        internal void OpenConnect()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        internal void CloseConnect()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public MySqlConnection GetConnect()
        {
            return connection;
        }
    }
}
