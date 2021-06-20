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

namespace Questionario_Agrotools.Controllers
{
    public class PerguntaController : Controller
    {
        private ContextoAgroTools db = new ContextoAgroTools();

        // GET: Pergunta
        public ActionResult Index()
        {
            var perguntas = db.Perguntas.Include(p => p.Questionario);
            return View(perguntas.ToList());
        }

        // GET: Pergunta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pergunta pergunta = db.Perguntas.Find(id);
            if (pergunta == null)
            {
                return HttpNotFound();
            }
            return View(pergunta);
        }

        // GET: Pergunta/Create
        public ActionResult Create()
        {
            ViewBag.QuestionarioId = new SelectList(db.Questionarios, "QuestionarioId", "Titulo");
            return View();
        }

        // POST: Pergunta/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PerguntaId,DescricaoPergunta,DescricaoResposta,QuestionarioId")] Pergunta pergunta)
        {
            if (ModelState.IsValid)
            {
                db.Perguntas.Add(pergunta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionarioId = new SelectList(db.Questionarios, "QuestionarioId", "Titulo", pergunta.QuestionarioId);
            return View(pergunta);
        }

        // GET: Pergunta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pergunta pergunta = db.Perguntas.Find(id);
            if (pergunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionarioId = new SelectList(db.Questionarios, "QuestionarioId", "Titulo", pergunta.QuestionarioId);
            return View(pergunta);
        }

        // POST: Pergunta/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PerguntaId,DescricaoPergunta,DescricaoResposta,QuestionarioId")] Pergunta pergunta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pergunta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionarioId = new SelectList(db.Questionarios, "QuestionarioId", "Titulo", pergunta.QuestionarioId);
            return View(pergunta);
        }

        // GET: Pergunta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pergunta pergunta = db.Perguntas.Find(id);
            if (pergunta == null)
            {
                return HttpNotFound();
            }
            return View(pergunta);
        }

        // POST: Pergunta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pergunta pergunta = db.Perguntas.Find(id);
            db.Perguntas.Remove(pergunta);
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
