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
            var listaUsuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();
            foreach (var item in eventos.AsEnumerable())
            {
                item["DNI"] = listaUsuarios
                .Where(u => u["DNI"].ToString() == item["DNI"].ToString())
                .Select(u => u["usuario"].ToString())
                .FirstOrDefault();
            }
            DataView dv = eventos.DefaultView;
            dv.Sort = "fechaHora DESC";
            return eventos;
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
            var listaUsuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();
            foreach (var item in resultado.AsEnumerable())
            {
                item["DNI"] = listaUsuarios
            .Where(u => u["DNI"].ToString() == item["DNI"].ToString())
            .Select(u => u["usuario"].ToString())
            .FirstOrDefault();
            }



            return resultado    ;
        }
    }
}