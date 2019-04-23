using SQLite;

namespace App2.Db
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
