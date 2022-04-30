using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAProject.Data;
using SAProject.Models;
using System.Collections.Generic;
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
            var applicationDbContext = _context.Files.Include(n => n.UserFiles);
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("FileId,Title,Password,FileExpiry")] File file, UserFile userFile, UserFile userFile1, ApplicationUser user)
        {
            userFile.File = file;
            userFile1.File = file;
            //user.Email = "admin@admin.com";
            //userFile.User = user;

            userFile.FileId = file.FileId;
            userFile1.FileId = file.FileId;

            var email = Request.Form["txtEmails"].ToString();
            string[] emailArr = email.Split(' ');

            string[] aa = new string[2];
            int a = 0;
            foreach (string s in emailArr)
            {
                Task<ApplicationUser> taskUser = _context.Users.FirstOrDefaultAsync(u => u.Email == s);

                aa[a] = taskUser.Result.Id;

                a++;
            }
            userFile.UserId = aa[0];
            userFile1.UserId = aa[1];

            if (ModelState.IsValid)
            {
                _context.Add(file);
                _context.Add(userFile);
                _context.Add(userFile1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", file.UserId);
            return View(file);
        }
    }
}
