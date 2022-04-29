using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Models
{
    public class Cartilla
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Título de Cartilla")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(400)]
        public string Descripcion { get; set; }

        [Display(Name = "Image")]
        public string Foto { get; set; }
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
