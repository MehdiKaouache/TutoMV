using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Data;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Student?> GetByIdAsync(int id) => await _context.Students.FindAsync(id);
        public async Task<IEnumerable<Student>> GetAllAsync() => await _context.Students.ToListAsync();
        public async Task<Student?> GetByDAAsync(string da) => await _context.Students.FirstOrDefaultAsync(s => s.DA == da);
        public async Task AddAsync(Student entity) => await _context.Students.AddAsync(entity);
        public void Update(Student entity) => _context.Students.Update(entity);
        public void Delete(Student entity) => _context.Students.Remove(entity);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}