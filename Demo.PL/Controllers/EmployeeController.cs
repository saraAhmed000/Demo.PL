using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        
        //private readonly IEmployeeRepositry _employeeRepositry;
        //private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Employee> _unitOfWork;

        public EmployeeController(/*IEmployeeRepositry employeeRepositry, IDepartmentRepositry departmentRepositry, IMapper mapper*/ IUnitOfWork<Employee> unitOfWork,IMapper mapper )
        {
            //_employeeRepositry = employeeRepositry;
            //_departmentRepositry = departmentRepositry;
            _mapper = mapper;
           _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult>Index(string SearchValue) 
        {
            // Binding is one way 
            // View Date => send info from action to view [Binding] ( first way =>Dictionary object )
            //ViewData["Message"] = "Hello from View Data";

            //ViewBag=>No diffrence till now [Dynamic property as Dynamic Featuere ]
            //ViewBag.Message = "Hello from View Bag";

            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))

                employees =await _unitOfWork.EmployeeRepositry.getAll();
            else

                employees = _unitOfWork.EmployeeRepositry.SerachEmployeeByName(SearchValue);

            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmp);

        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepositry.getAll();

            return View();
        }
        [HttpPost]
        public  async Task <IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            //Manual Mapping
            {
                ///    var employee =new Employee()
                ///    {
                ///        Name=employeeVM.Name,
                ///        Address=employeeVM.Address,
                ///        EmailAddress=employeeVM.EmailAddress,
                ///        Salary=employeeVM.Salary,   
                ///        Age=employeeVM.Age,
                ///        DepartmentId=employeeVM.DepartmentId,   
                ///        IsActive=employeeVM.IsActive,
                ///        HireDate=employeeVM.HireDate,
                ///        Phone=employeeVM.Phone, 
                ///    };

                employeeVM.ImageName= DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

               await _unitOfWork.EmployeeRepositry.Add(mappedEmp);

                //Updte
                //Delete

               await  _unitOfWork.Complete();
                
                


                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);

        }
        public async Task <IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();//400
            var employee =await _unitOfWork.EmployeeRepositry.get(id.Value);



            if (employee is null)
                return NotFound();
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(ViewName, mappedEmp);
        }
        [HttpGet]
        public async Task <IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();//400
            //var department = _departmentRepositry.get(id.Value);

            //if (department is null)
            //    return NotFound();

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepositry.Update(mappedEmp);
                  await  _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public async  Task <IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepositry.Delete(mappedEmp);
               int count= await _unitOfWork.Complete();

                if (count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");

                
                
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeVM);
        }

    }
}



