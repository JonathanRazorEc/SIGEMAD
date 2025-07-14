using DGPCE.Sigemad.Application.Constants;

namespace DGPCE.Sigemad.Application.Helpers
{
    public static class Fecha
    {
        public static bool CompararFechas(DateTime fecha1, DateTime fecha2, int comparacion)
        {
            return comparacion switch
            {
                ComparacionTipos.IgualA => fecha1 == fecha2,
                ComparacionTipos.MayorQue => fecha1 > fecha2,
                ComparacionTipos.MenorQue => fecha1 < fecha2,
                _ => throw new ArgumentException("Operador de comparar fechas no válido"),
            };
        }

        public static bool CompararFechasNullable(DateTime? fecha1, DateTime? fecha2, int comparacion)
        {
            if (!fecha1.HasValue) return false;
            if (!fecha2.HasValue) return false;
            return comparacion switch
            {
                ComparacionTipos.IgualA => fecha1.Value == fecha2,
                ComparacionTipos.MayorQue => fecha1.Value > fecha2,
                ComparacionTipos.MenorQue => fecha1.Value < fecha2,
                _ => throw new ArgumentException("Operador de comparar fechas no válido"),
            };
        }
    }
}
