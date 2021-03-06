using Questionario_Agrotools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionario_Agrotools.ViewModel
{
    public class QuestionarioViewModel
    {
        public Questionario Questionario { get; set; }
        public string Perguntas { get; set; }
        public string Respostas { get; set; }
        public string LatitudeLongitude { get; set; }
        public List<Questionario> Questionarios { get; set; }

        public QuestionarioViewModel()
        {
            Questionarios = new List<Questionario>();
        }
    }
}