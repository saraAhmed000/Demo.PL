using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepositry<T>where T : class
    {

     Task<List<T>> getAll();
      Task < T> get(int id);

        Task Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
