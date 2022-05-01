using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAProject.Data;
using SAProject.Models;
using System.Collections.Generic;
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




            //var t = _context.Files.Where(f => f.FileId == userFile.FileId);

            //var applicationDbContext = _context.Files.Include(n => n.UserFiles);
            var applicationDbContext = _context.Files
                .Include(File => File.UserFiles.Where(userFile => userFile.UserId == "a"));
                //.ThenInclude(UserFile => UserFile.User.Where(User => User.Id == UserFile.UserId));
                //.Where(User => User.Id == UserFile.userId));

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
        public async Task<IActionResult> Create([Bind("FileId,Title,Password,FileExpiry")] File file, List<UserFile> userFiles, ApplicationUser user)
        {
            Task<ApplicationUser> taskUser = _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            userFiles.Add(new UserFile() { File = file, FileId = file.FileId, UserId = taskUser.Result.Id});

            var email = Request.Form["txtEmails"].ToString();
            string[] emailArr = email.Split(' ');

            foreach (string s in emailArr)
            {
                taskUser = _context.Users.FirstOrDefaultAsync(u => u.Email == s);
                userFiles.Add(new UserFile() { File = file, FileId = file.FileId, UserId = taskUser.Result.Id });
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
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", file.UserId);
            return View(file);
        }
    }
}
