using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Repositry;
using System;
using Demo.PL.ViewModels;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IUnitOfWork<Department> _unitOfWork;

        public DepartmentController(/*IDepartmentRepositry departmentRepositry*/ IMapper mapper, IUnitOfWork<Department> unitOfWork)
        {
            _mapper = mapper;
            //_departmentRepositry = departmentRepositry;
            _unitOfWork = unitOfWork;
        }

        public async Task <IActionResult> Index(string SearchValue)
        {
            // Binding is one way 
            // View Date => send info from action to view [Binding] ( first way =>Dictionary object )
            ViewData["Message"] = "Hello from View Data";

            //ViewBag=>No diffrence till now [Dynamic property as Dynamic Featuere ]
            ViewBag.Message = "Hello from View Bag";



            var departments =  await _unitOfWork.DepartmentRepositry.getAll();
            var mappEmp = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(mappEmp);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task < IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mappdEmp = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

               await _unitOfWork.DepartmentRepositry.Add(mappdEmp);
                /*int count =*/ await _unitOfWork.Complete();
                //if (count>0)
                // Temp data [send form action to action]
                          //TempData["Message"] = "Department is added Successfuly";




                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);

        }

        public async Task <IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();//400
            var department = await _unitOfWork.DepartmentRepositry.get(id.Value);



            if (department is null)
                return NotFound();
            var mappdEmp=_mapper.Map<Department, DepartmentViewModel>(department);

            return View(ViewName, mappdEmp);
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
        public async Task <IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappEmp=_mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepositry.Update(mappEmp);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        public async Task <IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete([FromRoute]int id,DepartmentViewModel departmentVM)
        {
            try
            {
                var mappdEmp = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepositry.Delete(mappdEmp);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(departmentVM);
        }


    }
}
