using MySql.Data.MySqlClient;
using System;

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
        public string Fill_FIO()
        {
            OpenConnect();

            string sql_fio = @"SELECT employer.employer_FIO 
                       FROM `user` 
                       INNER JOIN employer ON user.user_employer = employer.id_employer 
                       WHERE user.id_user = " + Authorization.id_user + ";";
            MySqlCommand com1 = new MySqlCommand(sql_fio, GetConnect());
            string userFio = com1.ExecuteScalar().ToString();

            CloseConnect();

            return userFio;
        }
        public string Fill_Access()
        {
            OpenConnect();

            string sql_role = @"SELECT user_role.role_user_name 
                        FROM `user` 
                        INNER JOIN user_role ON user.user_role = user_role.id_user_role 
                        WHERE user.id_user = @userId;";
            MySqlCommand com2 = new MySqlCommand(sql_role, GetConnect());
            com2.Parameters.AddWithValue("@userId", Authorization.id_user);
            object result = com2.ExecuteScalar();

            string userAccess = (result != null) ? result.ToString() : string.Empty;

            CloseConnect();

            return userAccess;
        }
    }
}
