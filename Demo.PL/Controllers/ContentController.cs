using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class ContentController : Controller
    {
        private readonly IUnitOfWork<Content> _unitOfWork;
        public ContentController(IUnitOfWork<Content> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult ReviewToAdmin()
        {
                var contentToReview = GetPendingContent();

                return View(contentToReview);
        }

            [HttpPost]
            public async Task<ActionResult> ApproveContent(int id)
            {
                var contentToApprove = await _unitOfWork.GenericRepositry.get(id);
            if (contentToApprove != null)
                {
                    contentToApprove.IsApproved = true;
                    _unitOfWork.Complete();
                }

                // Redirect to the review page or any other appropriate page
                return RedirectToAction("Review");
            }

            [HttpPost]
            public async Task<ActionResult> RejectContent(int id)
            {
            // Retrieve the content by its ID and mark it as rejected or delete it
            var contentToReject = await _unitOfWork.GenericRepositry.get(id);
                if (contentToReject != null)
                {
                   _unitOfWork.GenericRepositry.Delete(contentToReject);
                   _unitOfWork.Complete();
                }

                // Redirect to the review page or any other appropriate page
                return RedirectToAction("Review");
            }

            private async Task<List<Content>> GetPendingContent()
            {
                 var pendingData = new List<Content>();
                 var pendingContent = await _unitOfWork.GenericRepositry.getAll();
                 foreach (var content in pendingContent)
                 {
                   if(content.IsApproved == false)
                   {
                    pendingData.Add(content);
                   }
                 }
                 return pendingData;
            }
        
    }
}
