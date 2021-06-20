using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Questionario_Agrotools.Models
{
    public class Questionario
    {
        [Key]
        public int QuestionarioId { get; set; }
        [DisplayName("Título")]
        public string Titulo { get; set; }
        [DisplayName("Usuário")]
        public string Usuario { get; set; }
        [DisplayName("Data de Cadastro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCadastro { get; set; }

        public ICollection<Pergunta> Perguntas { get; set; }

        public Questionario()
        {
            Perguntas = new List<Pergunta>();
        }
    }
}