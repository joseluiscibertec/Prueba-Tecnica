using PruebaTecnica.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.ViewModels
{
    public class CartillaViewModels : EditImageViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Título de Cartilla")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(400)]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(60)]
        [Display(Name = "Agrega texto del boton")]
        public string TextoBoton { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Inserta Link")]
        public string Link { get; set; }
    }
}
