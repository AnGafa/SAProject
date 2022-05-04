using FileTypeChecker;
using FileTypeChecker.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAProject.Data;
using SAProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SAProject.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userFiles = _context.UserFiles
                .Include(userFile => userFile.User).Where(userr => userr.User.Email == User.Identity.Name)
                .Include(userFile => userFile.File);

            return View(await userFiles.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(n => n.UserFiles)
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        public IActionResult Create()
        {

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["Email"] = new SelectList(_context.Users, "Email", "Email");
            List<string> list = new List<string>();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,Title,Password,FileExpiry")] Models.File file, List<UserFile> userFiles, ApplicationUser user, IFormFile uploadData)
        {
            Task<ApplicationUser> taskUser = _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            userFiles.Add(new UserFile() { File = file, FileId = file.FileId, UserId = taskUser.Result.Id });

            var email = Request.Form["txtEmails"].ToString();

            if (email != "")
            {
                string[] emailArr = email.Split(' ');
                foreach (string s in emailArr)
                {
                    taskUser = _context.Users.FirstOrDefaultAsync(u => u.Email == s);
                    userFiles.Add(new UserFile() { File = file, FileId = file.FileId, UserId = taskUser.Result.Id });
                }
            }

            if (uploadData != null && uploadData.Length > 0)
            {

                var fileStream = uploadData.OpenReadStream();

                var isRecognizableType = FileTypeValidator.IsTypeRecognizable(fileStream);
                if (!isRecognizableType)
                {
                    ViewBag.Error = "File is unrecognized";
                }
                else
                {
                    IFileType fileType = FileTypeValidator.GetFileType(fileStream);

                    if (fileType.Extension == "PDF" || fileType.Extension == "JPG")
                    {
                        using (var target = new MemoryStream())
                        {
                            uploadData.CopyTo(target);
                            var fileBytes = target.ToArray();
                            file.FileData = fileBytes;
                        }

                        if (ModelState.IsValid)
                        {
                            _context.Add(file);
                            int a = 0;
                            foreach (var userFile in userFiles)
                            {
                                _context.Add(userFiles[a]);
                                a++;
                            }
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                        ViewBag.Error = "accepted file extensions are PDF and JPG";
                }
            }
            else
                ViewBag.Error = "File is required or unrecognized file";


            return View(file);
        }
    }
}
