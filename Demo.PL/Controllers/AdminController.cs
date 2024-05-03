using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork<Content> _unitOfWork;
        public AdminController(IUnitOfWork<Content> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ActionResult> AdminHome()
        {
            // Fetch data from the Content table and map it to the view model
            var contents = await _unitOfWork.GenericRepositry.getAll();
            var NotApprovedContent = new List<Content>();

            foreach (var content in contents)
            {
                if (content.IsApproved == false)
                {
                    NotApprovedContent.Add(content);
                }
            }

            //Select(c => new olhjj
            //{
            //    Id = c.Id,
            //    Title = c.Title,
            //    Description = c.Description,
            //    ContentType = c.ContentType,
            //    IsApproved = c.IsApproved,
            //    FileName = c.FileName,
            //    FilePath = c.FilePath,
            //    ContentText = c.ContentText
            //}).ToList();

            return View(NotApprovedContent);
        }
    }
}
