using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Entity
{
    public class EFRepository : IEFRepository
    {
        private EFContext _context;

        public EFRepository(EFContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees
                      .ToList();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees
              .FirstOrDefault(c => c.Id == id);
        }
       

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
