using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task AddAsync(T entity);
        public void RemoveAsync(T entity);
        public void UpdateAsync(T entity);

    }
}
