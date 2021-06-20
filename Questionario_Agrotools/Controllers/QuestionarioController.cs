using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: Questionario
        public ActionResult Index()
        {
            return View(db.Questionarios.ToList());
        }

        // GET: Questionario/Details/5
        public ActionResult Details(int? id)
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

        // GET: Questionario/Create
        public ActionResult Criar()
        {
            return View();
        }

        // POST: Questionario/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
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
                        DescricaoPergunta = pergunta
                    });
                }
                db.Questionarios.Add(questionarioViewModel.Questionario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionarioViewModel);
        }

        // GET: Questionario/Edit/5
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

        // POST: Questionario/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Responder([Bind(Include = "Questionario,Respostas")] QuestionarioViewModel questionarioViewModel)
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
                    db.Entry(pergunta).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionarioViewModel);
        }

        // GET: Questionario/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Questionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
