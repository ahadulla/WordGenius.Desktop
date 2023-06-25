namespace WordGenius.Desktop.Constants
{
    public class DbConstans
    {
        public const string DB_HOST = "localhost";
        public const string DB_PORT = "5432";
        public const string DB_DATABASE = "wordgenius_db";
        public const string DB_USER = "postgres";
        public const string DB_PASSWORD = "root";

        public const string DB_CONNECTIONSTRING = $"Host = {DB_HOST};" +
            $"Port = {DB_PORT};" +
            $"Database = {DB_DATABASE};" +
            $"User ID = {DB_USER};" +
            $"Password = {DB_PASSWORD}";
    }
}
