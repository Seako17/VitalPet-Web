using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.Servicios
{
    public class SessionManager
    {
        private static object _lock = new Object();
        private static SessionManager _session;
        public BEUsuario Usuario { get; set; }
        public static SessionManager Instancia
        {
            get
            {
                lock (_lock)
                {
                    return _session;
                }
            }
        }
        public static void Login(BEUsuario u)
        {
            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                    _session.Usuario = u;
                }
            }
        }
        public static void LogOut()
        {
            lock (_lock)
            {
                if (_session != null)
                {
                    _session = null;
                }
            }
        }
        public static bool IsLogged()
        {
            return _session != null;
        }
    }
}