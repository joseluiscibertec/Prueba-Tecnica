using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Foto")]
        public IFormFile CartillaFoto { get; set; }
    }
}
