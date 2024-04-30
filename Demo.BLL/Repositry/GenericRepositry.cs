using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class GenericRepositry<T>:IGenericRepositry<T> where T : class
    {
        private protected MVCAppG0XDbContext _dbContext;
        public GenericRepositry (MVCAppG0XDbContext dbContext)
        {
           
            _dbContext = dbContext;
        }


        public /*int*/ async Task Add(T item)
        
       => await _dbContext.Set<T>().AddAsync(item);
            //return _dbContext.SaveChanges();

        

        public  void Delete(T item)
        
        =>  _dbContext.Set<T>().Remove(item);
            //return _dbContext.SaveChanges();

        

        public async Task <T>get(int id)

       
        => await _dbContext.Set<T>().FindAsync(id);
           
        //refactor Code TO DEsgin Pattern Called [Specification]
        // NOTE : I refactor this part
        public async Task  <List<T>> getAll()
        {
            if (typeof(T) == typeof(Employee))
            return  await _dbContext.Set<T>().ToListAsync(); 
            else 
                return await _dbContext.Set<T>().ToListAsync();
        }

           

        public void Update(T item) => _dbContext.Set<T>().Update(item);
    }
}
