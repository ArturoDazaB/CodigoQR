using System;

namespace MttoApp.Model
{
    internal class MasterDetailPageUserInfoModel
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES LOCALES
        protected string username, fullname, ultimaconexion, usericonfile = string.Empty;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE

        public MasterDetailPageUserInfoModel(Personas Persona, Usuarios Usuario, UltimaConexion Ultima)
        {
            if (Persona.Cedula == Usuario.Cedula)
            {
                //SE CARGA LA INFORMACION QUE VA A SER DESPLEGADA
                username = Metodos.Mayuscula(Usuario.Username);

                if (Persona.Cedula == 0)
                    fullname = "N/A.";
                else
                    fullname = Persona.Apellidos + ", " + Persona.Nombres + ".";

                ultimaconexion = Ultima.UltimaCon.ToString();
                usericonfile = "UserIcon.png";
            }
        }

        public MasterDetailPageUserInfoModel(Personas Persona, Usuarios Usuario, DateTime ultimafechaingreso)
        {
            if (Persona.Cedula == Usuario.Cedula)
            {
                //SE CARGA LA INFORMACION QUE VA A SER DESPLEGADA
                username = Metodos.Mayuscula(Usuario.Username);

                if (Persona.Cedula == 0)
                    fullname = "N/A.";
                else
                    fullname = Persona.Apellidos + ", " + Persona.Nombres + ".";

                ultimaconexion = ultimafechaingreso.ToString();
                usericonfile = "UserIcon.png";
            }
        }

        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA CLASE

        public string UserName { get { return username; } }

        public string FullName { get { return fullname; } }

        public string UltimaConexion { get { return ultimaconexion; } }

        public string UserIconFile { get { return usericonfile; } }
    }
}