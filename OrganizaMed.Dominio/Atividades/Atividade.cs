using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Dominio.Atividades
{
    public abstract class Atividade
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Medico> MedicosEnvolvidos { get; set; } = new List<Medico>();

        public abstract TimeSpan RecoveryTime { get; }
    }
}

public class Consulta : Atividade
{
    public override TimeSpan RecoveryTime => TimeSpan.FromMinutes(10);
}

public class Cirurgia : Atividade
{
    public override TimeSpan RecoveryTime => TimeSpan.FromHours(4);
}