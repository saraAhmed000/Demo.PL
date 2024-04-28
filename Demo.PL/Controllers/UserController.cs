using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var Users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    PhoneNumber = U.PhoneNumber,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);

                var mappUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel>() { mappUser });
            }


        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();//400
            var user = await _userManager.FindByIdAsync(id);



            if (user is null)
                return NotFound();
            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);

            return View(ViewName, mappedUser);
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
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UpdatedUser)
        {
            if (id != UpdatedUser.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user=await _userManager.FindByIdAsync(id);
                    user.FName = UpdatedUser.FName;
                    user.LName = UpdatedUser.LName; 
                    user.PhoneNumber = UpdatedUser.PhoneNumber; 
                    user.Email = UpdatedUser.Email;

                    user.SecurityStamp=Guid.NewGuid().ToString();
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(UpdatedUser);
        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete( string id, UserViewModel deletedUser)
        {
            if (id != deletedUser.Id)
                return BadRequest();

            try
            {

                var user = await _userManager.FindByIdAsync(id);
               

                //user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.DeleteAsync(user);


                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Error","Home");
        }



    }
}
