using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Auditoria_Parametro : BaseDomainModel<int>
    {
        // De [Id] IDENTITY(1,1) --> vendrá del BaseDomainModel<int>.Id

        // De la tabla:
        // [FechaRegistro] [datetime2](7) NOT NULL
        public DateTime FechaRegistro { get; set; }

        // [TipoMovimiento] [char](1) NOT NULL
        public string TipoMovimiento { get; set; } = null!;

        // [IdParametro] [int] NOT NULL
        public int IdParametro { get; set; }

        // [IdEvolucion] [int] NOT NULL
        public int IdEvolucion { get; set; }

        // [IdEstadoIncendio] [int] NULL
        public int? IdEstadoIncendio { get; set; }

        // [FechaFinal] [datetime2](7) NULL
        public DateTime? FechaFinal { get; set; }

        // [IdPlanEmergencia] [int] NULL
        public int? IdPlanEmergencia { get; set; }

        // [IdFaseEmergencia] [int] NULL
        public int? IdFaseEmergencia { get; set; }

        // [IdPlanSituacion] [int] NULL
        public int? IdPlanSituacion { get; set; }

        // [IdSituacionEquivalente] [int] NULL
        public int? IdSituacionEquivalente { get; set; }

        // [SuperficieAfectadaHectarea] [decimal](10, 2) NULL
        public decimal? SuperficieAfectadaHectarea { get; set; }

        // [FechaCreacion] [datetime2](7) NOT NULL
        // [CreadoPor], [FechaModificacion], [ModificadoPor], [FechaEliminacion], [EliminadoPor], [Borrado]
        // vendrán de BaseDomainModel<int>, SIEMPRE Y CUANDO tu BaseDomainModel ya maneje estos campos.
        // Si no, los creas aquí igual que en Auditoria_Incendio.

        // Ejemplo si BaseDomainModel<int> NO define algo como FechaCreacion, CreadoPor, etc.:
        public DateTime FechaCreacion { get; set; }
        public Guid? CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public Guid? ModificadoPor { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public Guid? EliminadoPor { get; set; }
        public bool Borrado { get; set; }
    }
}
