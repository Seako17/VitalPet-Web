using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLEvento
    {
        DALEvento dalEvento;
        public BLLEvento()
        {
            dalEvento = new DALEvento();
        }
        public DataTable ObtenerEventos()
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            DataTable eventos = dalEvento.ObtenerEventos();

            if (eventos.Columns.Contains("criticidad") && eventos.Columns["criticidad"].DataType != typeof(string))
            {
                DataTable dtClon = eventos.Clone();
                dtClon.Columns["criticidad"].DataType = typeof(string);
                foreach (DataRow row in eventos.Rows)
                {
                    dtClon.ImportRow(row);
                }
                eventos = dtClon;
            }

            var listaUsuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();

            foreach (var item in eventos.AsEnumerable())
            {
                item["DNI"] = listaUsuarios
                    .Where(u => u["DNI"].ToString() == item["DNI"].ToString())
                    .Select(u => u["usuario"].ToString())
                    .FirstOrDefault();

                string valorCriticidad = item["criticidad"].ToString().Trim();
                switch (valorCriticidad)
                {
                    case "1":
                        item["criticidad"] = "1 (Crítica)";
                        break;
                    case "2":
                        item["criticidad"] = "2 (Importante)";
                        break;
                    case "3":
                        item["criticidad"] = "3 (Media)";
                        break;
                    case "4":
                        item["criticidad"] = "4 (Baja)";
                        break;
                    case "5":
                        item["criticidad"] = "5 (Mínima)";
                        break;
                    default:
                        break;
                }
            }

            DataView dv = eventos.DefaultView;
            dv.Sort = "fechaHora DESC";
            return dv.ToTable();
        }
        public void AgregarEvento(BEEvento evento)
        {
            dalEvento.AgregarEvento(evento);
        }
        public DataTable ObtenerEventosFiltrados(string usuario, DateTime fechaHora, string modulo, string evento, int criticidad)
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            DataTable eventos = dalEvento.ObtenerEventos();

            IEnumerable<DataRow> query = eventos.AsEnumerable();

            if (!string.IsNullOrEmpty(usuario))
            {
                string dni = bllUsuario.ObtenerUsuarios()
                    .AsEnumerable()
                    .Where(u => u["usuario"].ToString() == usuario)
                    .Select(u => u["DNI"].ToString())
                    .FirstOrDefault();

                query = query.Where(e => e["DNI"].ToString() == dni);
            }

            if (fechaHora != DateTime.MinValue)
            {
                query = query.Where(e => Convert.ToDateTime(e["fechaHora"]).Date == fechaHora.Date);
            }

            if (!string.IsNullOrEmpty(modulo))
            {
                query = query.Where(e => e["modulo"].ToString() == modulo);
            }

            if (!string.IsNullOrEmpty(evento))
            {
                query = query.Where(e => e["evento"].ToString() == evento);
            }

            if (criticidad != 0)
            {
                query = query.Where(e => int.Parse(e["criticidad"].ToString()) == criticidad);
            }

            DataTable resultado = query.Any()
                ? query.OrderByDescending(e => Convert.ToDateTime(e["fechaHora"])).CopyToDataTable()
                : eventos.Clone();

            if (resultado.Columns.Contains("criticidad") && resultado.Columns["criticidad"].DataType != typeof(string))
            {
                DataTable dtClon = resultado.Clone();
                dtClon.Columns["criticidad"].DataType = typeof(string);
                foreach (DataRow row in resultado.Rows)
                {
                    dtClon.ImportRow(row);
                }
                resultado = dtClon;
            }

            var listaUsuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();

            foreach (var item in resultado.AsEnumerable())
            {
                item["DNI"] = listaUsuarios
                    .Where(u => u["DNI"].ToString() == item["DNI"].ToString())
                    .Select(u => u["usuario"].ToString())
                    .FirstOrDefault();

                string valorCriticidad = item["criticidad"].ToString().Trim();
                switch (valorCriticidad)
                {
                    case "1":
                        item["criticidad"] = "1 (Crítica)";
                        break;
                    case "2":
                        item["criticidad"] = "2 (Importante)";
                        break;
                    case "3":
                        item["criticidad"] = "3 (Media)";
                        break;
                    case "4":
                        item["criticidad"] = "4 (Baja)";
                        break;
                    case "5":
                        item["criticidad"] = "5 (Mínima)";
                        break;
                    default:
                        break;
                }
            }

            return resultado;
        }
    }
}