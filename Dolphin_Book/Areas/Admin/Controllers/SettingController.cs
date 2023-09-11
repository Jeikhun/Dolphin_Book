using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Extentions;
using Dolphin_Book.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SettingController : Controller
    {
        private readonly DolphinDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(DolphinDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Setting>? settings = await _context.Settings
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(settings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (setting.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Wrong!");
                return View();
            }
            if (!Helper.isImage(setting.FormFile))
            {
                ModelState.AddModelError("FormFile", "Is not Image!");
                return View();
            }
            if (!Helper.isSizeOk(setting.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "wrong Size");
                return View();
            }

            setting.Image = setting.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");

            setting.CreatedAt = DateTime.Now;
            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Setting? setting = await _context.Settings
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Setting newSetting)
        {
            Setting? setting = await _context.Settings.AsNoTracking()
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (setting == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(setting);
            }


            if (newSetting.FormFile != null)
            {
                if (!Helper.isImage(newSetting.FormFile))
                {
                    ModelState.AddModelError("FormFile", "Is not Image!");
                    return View();
                }
                if (!Helper.isSizeOk(newSetting.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "Wrong Size!");
                    return View();
                }
                if (setting.Image != null)
                {

                    Helper.RemoveImage(_env.WebRootPath, "admin/img/images", setting.Image);
                    newSetting.Image = newSetting.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
                }
                newSetting.Image = newSetting.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
                newSetting.UpdatedAt = DateTime.Now;
                _context.Settings.Update(newSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            newSetting.Image = setting.Image;
            newSetting.UpdatedAt = DateTime.Now;
            _context.Settings.Update(newSetting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Setting? setting = await _context.Settings.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
            if (setting == null)
            {
                return NotFound();
            }

            setting.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
