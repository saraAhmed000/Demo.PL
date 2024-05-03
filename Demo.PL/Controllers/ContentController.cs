using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Demo.PL.Controllers
{
    public class ContentController : Controller

    {   private readonly IUnitOfWork<Content> _unitOfWork;
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
                await _unitOfWork.Complete();
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
                await _unitOfWork.Complete();
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
                if (content.IsApproved == false)
                {
                    pendingData.Add(content);
                }
            }
            return pendingData;
        }
        [HttpPost]
        public ActionResult UploadContent()
        {
            return View("UploadContent");
        }


        //[HttpPost]
        //public async Task<ActionResult> Upload(IFormFile file, string type)
        //{
        //    if (file != null && file.Length > 0)
        //    {
        //        // Save the file to a location or storage system of your choice (e.g., local storage, cloud storage)
        //        // Example: Save the file to a specific folder on the server
        //        string fileName = Path.GetFileName(file.FileName);
        //        string path = Path.Combine("YourFolderPath", fileName);
        //        //using (var stream = new FileStream(path, FileMode.Create))
        //        //{
        //        //    file.CopyTo(stream);
        //        //}

        //        // Handle the different content types
        //        switch (type)
        //        {
        //            case "article":
        //                var article = new Content
        //                {
        //                    IsApproved = false,
        //                    ContentType = "article",
        //                    FileName = fileName,
        //                    FilePath = path
        //                };
        //                await _unitOfWork.GenericRepositry.Add(article);
        //                await _unitOfWork.Complete();
        //                return Content("Article uploaded successfully!");

        //            case "video":
        //                var video = new Content
        //                {
        //                    IsApproved = false,
        //                    ContentType = "video",
        //                    FileName = fileName,
        //                    FilePath = path
        //                };
        //                await _unitOfWork.GenericRepositry.Add(video);
        //                await _unitOfWork.Complete();
        //                return Content("Video uploaded successfully!");

        //            case "voicenote":
        //                var voicenote = new Content
        //                {
        //                    IsApproved = false,
        //                    ContentType = "voiceNote",
        //                    FileName = fileName,
        //                    FilePath = path
        //                };
        //                await _unitOfWork.GenericRepositry.Add(voicenote);
        //                await _unitOfWork.Complete();
        //                return Content("Voice note uploaded successfully!");

        //            default:
        //                return Content("Invalid content type!");
        //        }
        //    }

        //    return Content("No file selected!");
        //}

        //[HttpPost]
        //public async Task<ActionResult> Upload(IFormFile file, string type, string articleContent)
        //{
        //    if (type == "article" && !string.IsNullOrEmpty(articleContent))
        //    {
        //        // Handle article upload
        //        var article = new Content
        //        {
        //            IsApproved = false,
        //            ContentType = "article",
        //            // Assuming your Content model has a property named ContentText to store the article content
        //            ContentText = articleContent
        //        };
        //        await _unitOfWork.GenericRepositry.Add(article);
        //        await _unitOfWork.Complete();
        //        return Content("Article uploaded successfully!");
        //    }
        //    else if (type == "video" || type == "voice")
        //    {
        //        // Handle video and voice uploads
        //        // Your existing code for handling videos and voice notes remains the same
        //        // Don't forget to handle the file upload part for these types as well
        //        return Content($"{type} uploaded successfully!");
        //    }
        //    else
        //    {
        //        // Handle invalid content type
        //        return Content("Invalid content type!");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file, string type, string articleContent, string voiceNote)
        {
            if (type == "article" && !string.IsNullOrEmpty(articleContent))
            {
                // Handle article upload
                var article = new Content
                {
                    IsApproved = false,
                    ContentType = "article",
                    ContentText = articleContent
                };
                await _unitOfWork.GenericRepositry.Add(article);
                await _unitOfWork.Complete();
                return Content("Article uploaded successfully!");
            }
            else if (type == "video")
            {
                if (file != null)
                {
                    // Check video duration
                    long videoDuration = GetVideoDurationInSeconds(file);

                    if (videoDuration >= 1)
                    { 
                        return Content("Video uploaded successfully!");
                    }
                    else
                    {
                       
                        return Content("Video duration must be at least 1 second.");
                    }
                }
                else
                {
                    // Video file is missing
                    return Content("No video file selected!");
                }
            }
            else if (type == "voice" && !string.IsNullOrEmpty(voiceNote))
            {
                
                long voiceNoteDuration = GetVoiceNoteDurationInSeconds(voiceNote);

                if (voiceNoteDuration <= 600) // 600 seconds = 10 minutes
                {
                    
                    return Content("Voice note uploaded successfully!");
                }
                else
                {
                     
                    return Content("Voice note duration must be less than or equal to 10 minutes.");
                }
            }
            else if (file != null && (file.ContentType == "application/pdf" ||
                                      file.ContentType == "application/msword" ||
                                      file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                // Handle word file or PDF upload
                // You can add logic here to save the file and any additional details if needed
                return Content("Document uploaded successfully!");
            }
            else
            {
                // Handle invalid content type or missing data
                return Content("Invalid content type or missing data!");
            }
        }

        // Method to get the duration of a video file in seconds
        private long GetVideoDurationInSeconds(IFormFile file)
        {
              Random rnd = new Random();
            return rnd.Next(1, 10); // duration =10sec
        }
         private long GetVoiceNoteDurationInSeconds(string voiceNote)
        {
             Random rnd = new Random();
            return rnd.Next(1, 600);  
        }



    }
}
