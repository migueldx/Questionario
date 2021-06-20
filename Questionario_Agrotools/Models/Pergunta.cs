using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Questionario_Agrotools.Models
{
    public class Pergunta
    {
        [Key]
        public int PerguntaId { get; set; }
        [DisplayName("Pergunta")]
        public string DescricaoPergunta { get; set; }
        [DisplayName("Resposta")]
        public string DescricaoResposta { get; set; }
        public int QuestionarioId { get; set; }
        public Questionario Questionario { get; set; }
        [DisplayName("Data da Resposta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCadastroResposta { get; set; }
        public DbGeography Localizacao { get; set; }

    }
}