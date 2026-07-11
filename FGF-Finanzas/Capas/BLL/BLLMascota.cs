using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLMascota
    {
        DALMascota dalMascota;
        BLLEvento bllEvento;
        public BLLMascota()
        {
            dalMascota = new DALMascota();
            bllEvento = new BLLEvento();
        }
        public DataTable ObtenerMascotas()
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            DataTable mascotas = dalMascota.ObtenerMascotas();
            var listaUsuarios = bllUsuario.ObtenerUsuarios().AsEnumerable().ToList();
            foreach (var item in mascotas.AsEnumerable())
            {
                item["DNI"] = listaUsuarios
                .Where(u => u["DNI"].ToString() == item["DNI"].ToString())
                .Select(u => u["usuario"].ToString())
                .FirstOrDefault();
            }
            DataView dv = mascotas.DefaultView;
            return mascotas;
        }
        public void AgregarMascota(BEMascota mascota)
        {
            dalMascota.AgregarMascota(mascota);
            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Clientes", "Registrar Mascota", 4));
        }
        public DataTable ObtenerMascotasDeUsuario(BEUsuario usuario)
        {
            DataTable mascotas = ObtenerMascotas();

            var filas = mascotas.AsEnumerable()
                .Where(m => m["DNI"].ToString() == usuario.Usuario);

            if (filas.Any())
                return filas.CopyToDataTable();

            return mascotas.Clone();
        }
    }
}