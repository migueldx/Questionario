using Questionario_Agrotools.Models;
using System.Data.Entity;

namespace Questionario_Agrotools.Contexto
{
    public class ContextoAgroTools : DbContext
    {
        public DbSet<Questionario> Questionarios { get; set; }

        public System.Data.Entity.DbSet<Questionario_Agrotools.Models.Pergunta> Perguntas { get; set; }
    }
}