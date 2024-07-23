using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFriends2.DAL;
using MyFriends2.DAL.Models;
using MyFriends2.Services;
using MyFriends2.ViewModels;

namespace MyFriends2.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ImagesController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }


        // GET: Images
        public async Task<IActionResult> Index()
        {
            var images = await _context.Images.Include(i => i.User).Select(img => new ImageViewModel
            {
                Id = img.Id,
                FileName = img.FileName,
                ImageBase64 = img.ImageFile != null ? Convert.ToBase64String(img.ImageFile) : null,
                UserEmail = img.User.Email
            }).ToListAsync();

            return View(images);
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOld([Bind("Id,ImageFile,FileName,UserId")] Image image)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", image.UserId);
            return View(image);
        }
        */
        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,UserId")] Image image, IFormFile imageFile)
        {
            if (imageFile != null && _imageService.IsValidImageFile(imageFile))
            {
                image.ImageFile = await _imageService.ConvertToByteArray(imageFile);
                image.FileName = image.FileName ?? imageFile.FileName;

                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("ImageFile", "An image file is required and must be a valid image format.");
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", image.UserId);
            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", image.UserId);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageFile,FileName,UserId")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", image.UserId);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
