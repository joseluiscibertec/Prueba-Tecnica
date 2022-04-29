using PruebaTecnica.Data;
using PruebaTecnica.Models;
using PruebaTecnica.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Controllers
{
    public class CartillasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CartillasController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cartilla.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartilla = await _context.Cartilla
                .FirstOrDefaultAsync(m => m.Id == id);

            var speakerViewModel = new CartillaViewModels()
            {
                Id = cartilla.Id,
                Titulo = cartilla.Titulo,
                Descripcion = cartilla.Descripcion,
                ExistingImage = cartilla.Foto,
                TextoBoton = cartilla.TextoBoton,
                Link = cartilla.Link
             
            };

            if (cartilla == null)
            {
                return NotFound();
            }

            return View(cartilla);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartillaViewModels model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Cartilla cartilla = new Cartilla
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    Foto = uniqueFileName,
                    TextoBoton = model.TextoBoton,
                    Link = model.Link  
                };

                _context.Add(cartilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartilla = await _context.Cartilla.FindAsync(id);
            var cartillaViewModel = new CartillaViewModels()
            {
                Id = cartilla.Id,
                Titulo = cartilla.Titulo,
                Descripcion = cartilla.Descripcion,
                ExistingImage = cartilla.Foto,
                TextoBoton = cartilla.TextoBoton,
                Link = cartilla.Link
            };

            if (cartilla == null)
            {
                return NotFound();
            }
            return View(cartillaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CartillaViewModels model)
        {
            if (ModelState.IsValid)
            {
                var cartilla = await _context.Cartilla.FindAsync(model.Id);
                cartilla.Titulo = model.Titulo;
                cartilla.Descripcion = model.Descripcion;

                if (model.CartillaFoto != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    cartilla.Foto = ProcessUploadedFile(model);
                }
                cartilla.TextoBoton = model.TextoBoton;
                cartilla.Link = model.Link;
                _context.Update(cartilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartilla = await _context.Cartilla
                .FirstOrDefaultAsync(m => m.Id == id);

            var cartillaViewModel = new CartillaViewModels()
            {
                Id = cartilla.Id,
                Titulo = cartilla.Titulo,
                Descripcion = cartilla.Descripcion,
                ExistingImage = cartilla.Foto,
                TextoBoton = cartilla.TextoBoton,
                Link = cartilla.Link
                
            };
            if (cartilla == null)
            {
                return NotFound();
            }

            return View(cartillaViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartilla = await _context.Cartilla.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), FileLocation.DeleteFileFromFolder, cartilla.Foto);
            _context.Cartilla.Remove(cartilla);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartillaExists(int id)
        {
            return _context.Cartilla.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(CartillaViewModels model)
        {
            string uniqueFileName = null;

            if (model.CartillaFoto != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CartillaFoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CartillaFoto.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
