using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
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
        public BLLMascota()
        {
            dalMascota = new DALMascota();
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