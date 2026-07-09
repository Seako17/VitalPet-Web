using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLDigitoVerificador
    {
        private DALDigitoVerificador _dalDigito;
        public BLLDigitoVerificador()
        {
            _dalDigito = new DALDigitoVerificador();
        }

        #region PRUEBA
        public void GuardarDigitoVerificador(string nombreTabla)
        {
            List<FilaGenerica> filas = _dalDigito.ObtenerFilasDeTablaNegocio(nombreTabla);

            BigInteger sumaVertical = 0;

            foreach (var fila in filas)
            {
                string dvHorizontalCalculado = _dalDigito.CalcularDVHorizontalFila(fila);

                string hexCol = Encriptacion.Encriptar(dvHorizontalCalculado);
                sumaVertical += BigInteger.Parse("00" + hexCol, NumberStyles.HexNumber);
            }

            BEDigitoVerificador dvGlobal = new BEDigitoVerificador();
            dvGlobal.NombreTabla = nombreTabla;
            dvGlobal.DV_Vertical = sumaVertical.ToString("X");

            _dalDigito.GuardarDigitoVerificador(dvGlobal);
        }

        public void InicializarTablaCompleta(string nombreTabla)
        {
            List<FilaGenerica> filas = _dalDigito.ObtenerFilasDeTablaNegocio(nombreTabla);

            string nombrePK = "id";
            if (nombreTabla.Equals("Usuario", StringComparison.OrdinalIgnoreCase)) nombrePK = "dni";
            if (nombreTabla.Equals("Evento", StringComparison.OrdinalIgnoreCase)) nombrePK = "id";

            BigInteger sumaVertical = 0;

            using (SqlConnection con = new SqlConnection(_dalDigito.ObtenerConexionString()))
            {
                con.Open();
                foreach (var fila in filas)
                {
                    string dvHorizontalCalculado = _dalDigito.CalcularDVHorizontalFila(fila);

                    string queryUpdateFila = $"UPDATE {nombreTabla} SET DV_Horizontal = @dvh WHERE {nombrePK} = @id";
                    using (SqlCommand cmdFila = new SqlCommand(queryUpdateFila, con))
                    {
                        cmdFila.Parameters.AddWithValue("@dvh", dvHorizontalCalculado);
                        cmdFila.Parameters.AddWithValue("@id", fila.Id);
                        cmdFila.ExecuteNonQuery();
                    }

                    string hexCol = Encriptacion.Encriptar(dvHorizontalCalculado);
                    sumaVertical += BigInteger.Parse("00" + hexCol, NumberStyles.HexNumber);
                }
            }

            BEDigitoVerificador dvGlobal = new BEDigitoVerificador();
            dvGlobal.NombreTabla = nombreTabla;
            dvGlobal.DV_Vertical = sumaVertical.ToString("X");
            dvGlobal.CantidadRegistros = filas.Count; 

            _dalDigito.GuardarDigitoVerificador(dvGlobal);
        }

        public List<InconsistenciaReporte> CompararDigito()
        {
            List<InconsistenciaReporte> reporteInconsistencias = new List<InconsistenciaReporte>();

            foreach (BEDigitoVerificador tablaControlada in _dalDigito.ObtenerTodos())
            {
                string nombreTabla = tablaControlada.NombreTabla;

                //BORRAR CUANDO PONGAMOS LAS DEMÁS TABLAS
                if (!nombreTabla.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                List<FilaGenerica> filasActuales = _dalDigito.ObtenerFilasDeTablaNegocio(nombreTabla);
                BEDigitoVerificador dv_Tabla = _dalDigito.ObtenerDV_Tabla(nombreTabla);
                int cantidadRegistrosGuardada = dv_Tabla.CantidadRegistros;

                if (dv_Tabla == null) continue;

                BigInteger sumaVerticalCalculada = 0;
                bool huboModificacionEnTabla = false;

                foreach (FilaGenerica fila in filasActuales)
                {
                    string dvHorizontalCalculado = _dalDigito.CalcularDVHorizontalFila(fila);

                    if (dvHorizontalCalculado != fila.DV_HorizontalGuardado)
                    {
                        reporteInconsistencias.Add(new InconsistenciaReporte
                        {
                            NombreTabla = nombreTabla,
                            IdRegistro = fila.Id,
                            TipoFalla = "Hubo una modificación en el registro."
                        });
                        huboModificacionEnTabla = true;
                    }

                    string hexCol = Encriptacion.Encriptar(dvHorizontalCalculado);
                    sumaVerticalCalculada += BigInteger.Parse("00" + hexCol, NumberStyles.HexNumber);
                }

                string dvVerticalCalculadoFinal = sumaVerticalCalculada.ToString("X");

                if (dvVerticalCalculadoFinal != dv_Tabla.DV_Vertical)
                {
                    if (!huboModificacionEnTabla)
                    {
                        if (filasActuales.Count < cantidadRegistrosGuardada)
                        {
                            reporteInconsistencias.Add(new InconsistenciaReporte
                            {
                                NombreTabla = nombreTabla,
                                IdRegistro = "N/A",
                                TipoFalla = "Hubo una eliminación no registrada."
                            });
                        }
                        else
                        {
                            reporteInconsistencias.Add(new InconsistenciaReporte
                            {
                                NombreTabla = nombreTabla,
                                IdRegistro = "N/A",
                                TipoFalla = "Hubo un alta no registrada."
                            });
                        }
                    }
                }
            }

            return reporteInconsistencias;
        }
        #endregion
    }
}