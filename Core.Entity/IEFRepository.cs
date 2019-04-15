using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Entities;

namespace Core.Entity
{
    public interface IEFRepository
    {
        // Basic DB Operations
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();

        //Employee
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployee(int id);
        
       
    }
}