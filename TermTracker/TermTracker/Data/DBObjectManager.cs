using System;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace TermTracker.Data
{
    public class DBObjectManager<T> where T : new()
    {
        readonly SQLiteAsyncConnection dbconnection;

        public DBObjectManager(SQLiteAsyncConnection _dbconnection)
        {
            dbconnection = _dbconnection;
            dbconnection.CreateTableAsync<T>().Wait();
        }

        public Task<List<T>> GetAllAsync()
        {
            return dbconnection.Table<T>().ToListAsync();
        }

        public T GetAt(int i)
        {
            var list_task = GetAllAsync();
            list_task.Wait();
            var list = list_task.Result;
            if (i < 0 || i >= list.Count)
                throw new IndexOutOfRangeException();
            else
                return list[i];
        }

        public Task<int> AddAsync(T t)
        {
            return dbconnection.InsertAsync(t);
        }

        public Task<int> UpdateAsync(T t)
        {
            return dbconnection.UpdateAsync(t);
        }

        public Task<int> DeleteAsync(T t)
        {
            return dbconnection.DeleteAsync(t);
        }
    }
}
