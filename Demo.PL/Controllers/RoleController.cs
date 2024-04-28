using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,

                    RoleName = R.Name
                }).ToListAsync();
                return View(roles);
            }
            else
            {

                var role = await _roleManager.FindByNameAsync(name);

                if(role is not null)
                {

                    var mappRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name


                    };
                    return View(new List<RoleViewModel>() { mappRole });

                }
                return View(Enumerable.Empty<RoleViewModel>());
            }


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task < IActionResult>Create(RoleViewModel roleVM)

        {
            if (ModelState.IsValid)
            {
                var mappedRole= _mapper.Map<RoleViewModel ,IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappedRole); 
                
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }


        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();//400
            var role = await _roleManager.FindByIdAsync(id);



            if (role is null)
                return NotFound();
            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(ViewName, mappedRole);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
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
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel UpdatedRole)
        {
            if (id != UpdatedRole.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = UpdatedRole.RoleName;
                    
                    
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(UpdatedRole);
        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id, RoleViewModel deletedRole)
        {
            if (id != deletedRole.Id)
                return BadRequest();

            try
            {

                var role = await _roleManager.FindByIdAsync(id);


                //user.SecurityStamp = Guid.NewGuid().ToString();
                await _roleManager.DeleteAsync(role);


                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Error", "Home");
        }


    }


}
