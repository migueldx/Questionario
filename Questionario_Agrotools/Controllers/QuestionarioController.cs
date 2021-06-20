using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Questionario_Agrotools.Contexto;
using Questionario_Agrotools.Models;
using Questionario_Agrotools.ViewModel;

namespace Questionario_Agrotools.Controllers
{
    public class QuestionarioController : Controller
    {
        private ContextoAgroTools db = new ContextoAgroTools();

        public ActionResult Index()
        {
            QuestionarioViewModel questionarioViewModel = new QuestionarioViewModel();
            questionarioViewModel.Questionarios = db.Questionarios.Include(i => i.Perguntas).ToList();
            questionarioViewModel.Questionarios.ForEach(q => q.Respondido = q.Perguntas.Where(p => p.DescricaoResposta != null).Any());
            return View(questionarioViewModel);
        }

        public ActionResult Visualizar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionario questionario = db.Questionarios.Find(id);
            questionario.Perguntas = db.Perguntas.Where(p => p.QuestionarioId == id).ToList();
            if (questionario == null)
            {
                return HttpNotFound();
            }
            return View(questionario);
        }

        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar([Bind(Include = "Questionario,Perguntas")] QuestionarioViewModel questionarioViewModel)
        {
            if (ModelState.IsValid)
            {
                var perguntas = questionarioViewModel.Perguntas.Split('µ');

                questionarioViewModel.Questionario.DataCadastro = DateTime.Now;
                foreach (string pergunta in perguntas)
                {
                    questionarioViewModel.Questionario.Perguntas.Add(new Pergunta
                    {
                        DescricaoPergunta = pergunta,
                        DataCadastroResposta = DateTime.Now
                    });
                }
                db.Questionarios.Add(questionarioViewModel.Questionario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionarioViewModel);
        }

        public ActionResult Responder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionarioViewModel questionarioViewModel = new QuestionarioViewModel();
            questionarioViewModel.Questionario = db.Questionarios.Find(id);
            questionarioViewModel.Questionario.Perguntas = db.Perguntas.Where(p => p.QuestionarioId == id).ToList();
            if (questionarioViewModel.Questionario == null)
            {
                return HttpNotFound();
            }
            return View(questionarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Responder([Bind(Include = "Questionario,Respostas,LatitudeLongitude")] QuestionarioViewModel questionarioViewModel)
        {
            if (ModelState.IsValid)
            {
                var respostas = questionarioViewModel.Respostas.Split('µ');
                foreach (string resposta in respostas)
                {
                    var perguntaId = resposta.Split('þ')[0];
                    var descricaoResposta = resposta.Split('þ')[1];
                    Pergunta pergunta = db.Perguntas.Find(Convert.ToInt32(perguntaId));
                    pergunta.DescricaoResposta = descricaoResposta;
                    pergunta.DataCadastroResposta = DateTime.Now;
                    pergunta.Localizacao = DbGeography.FromText("POINT( " + questionarioViewModel.LatitudeLongitude + ")");
                    db.Entry(pergunta).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionarioViewModel);
        }

        public ActionResult Apagar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionario questionario = db.Questionarios.Find(id);
            if (questionario == null)
            {
                return HttpNotFound();
            }
            return View(questionario);
        }

        [HttpPost, ActionName("Apagar")]
        [ValidateAntiForgeryToken]
        public ActionResult Apagar(int id)
        {
            Questionario questionario = db.Questionarios.Find(id);
            db.Questionarios.Remove(questionario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
