using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Data;
using PruebaTecnica.Models;
using PruebaTecnica.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public APIController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _context.Cartilla.ToListAsync());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Cartilla model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = model.Foto;
                Cartilla cartilla = new Cartilla
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    Foto = uniqueFileName,
                    TextoBoton = model.TextoBoton,
                    Link = model.Link
                };

                if (!string.IsNullOrEmpty(uniqueFileName))
                {
                    var path = Path.GetFullPath("wwwroot");
                    path = Path.Combine(path, "Uploads");
                    string extension = GetFileExtension(uniqueFileName);
                    string nombreImg = Guid.NewGuid().ToString() + extension;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, nombreImg);
                    byte[] imageBytes = Convert.FromBase64String(uniqueFileName);
                    System.IO.File.WriteAllBytes(path, imageBytes);

                    cartilla.Foto = nombreImg;
                }
                else
                {
                    cartilla.Foto = string.Empty;
                }

                _context.Add(cartilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(model);
        }

        [HttpPost]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Cartilla model)
        {
            if (ModelState.IsValid)
            {
                var cartilla = await _context.Cartilla.FindAsync(model.Id);
                cartilla.Titulo = model.Titulo;
                cartilla.Descripcion = model.Descripcion;

                if (!string.IsNullOrEmpty(model.Foto))
                {
                    var path = Path.GetFullPath("wwwroot");
                    path = Path.Combine(path, "Uploads");
                    string extension = GetFileExtension(model.Foto);
                    string nombreImg = Guid.NewGuid().ToString() + extension;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, nombreImg);
                    byte[] imageBytes = Convert.FromBase64String(model.Foto);
                    System.IO.File.WriteAllBytes(path, imageBytes);

                    cartilla.Foto = nombreImg;
                }
                else
                {
                    cartilla.Foto = string.Empty;
                }
                cartilla.TextoBoton = model.TextoBoton;
                cartilla.Link = model.Link;
                _context.Update(cartilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(model);
        }
        [HttpDelete]
        [Route("Deleted/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cartilla = await _context.Cartilla.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), FileLocation.DeleteFileFromFolder, cartilla.Foto);
            _context.Cartilla.Remove(cartilla);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            await _context.SaveChangesAsync();
            return Ok(); 
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return ".png";
                case "/9J/4":
                    return ".jpg";
                case "AAAAF":
                    return ".mp4";
                case "JVBER":
                    return ".pdf";
                case "AAABA":
                    return ".ico";
                case "UMFYI":
                    return ".rar";
                case "E1XYD":
                    return ".rtf";
                case "U1PKC":
                    return ".txt";
                case "MQOWM":
                case "77U/M":
                    return ".srt";
                default:
                    return string.Empty;
            }
        }

        //private string ProcessUploadedFile(CartillaViewModels model)
        //{
        //    string uniqueFileName = null;

        //    if (model.Foto != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder);
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CartillaFoto.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.CartillaFoto.CopyTo(fileStream);
        //        }
        //    }

        //    return uniqueFileName;
        //}
    }
}
