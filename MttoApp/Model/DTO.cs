using System;
using System.Collections.Generic;
using System.Text;

namespace MttoApp.Model
{
    //===================================================================================
    //===================================================================================
    //CLASES DTO (DATA TRANSFER OBJECT). ESTAS CLASES SON CREADAS PARA FUNCIONAR COMO
    //CLASE MODELO PARA LA INFORMACION DE LOS OBJETOS QUE SON ENVIADOS O RECIBIDOS.
    //POR EJEMPLO: SI QUISIERAMOS MODIFICAR UN ATRIBUTO EN ESPECIFICO DE UN REGISTRO
    //EN LA TABLAS PERSONAS MEDIANTE LA SOLICITUD HTTP "PUT" DEBERIAMOS ENVIAR EL
    //PRIMARY KEY O ID DE DICHO REGISTRO JUNTO CON UN OBJETO JSON HOMOLOGO A
    //EL OBJETO PERSONAS, ES DECIR CON TODOS LOS ATRIBUTOS QUE SE DESEAN MODIFICAR
    //ADEMAS DE LOS ATRIBUTOS QUE YA EXISTIAN ANTES.

    //ES DEBIDO A ESTO QUE SE CREARON LOS OBJETOS DTO LOS CUALES SOLO POSEERAN LOS
    //ATRIBUTOS CON LOS QUE REGULARMENTE SE TRABAJARAN MEDIANTE LAS SOLICITUDES HTTP
    public class ConfiguracionU
    {
        public double Cedula { get; set; }
        public double Telefono { get; set; }
        public string Correo { get; set; }
        public string Userpassword { get; set; }

        public static ConfiguracionU NewConfiguracionU(ConfiguracionU newinfo)
        {
            return new ConfiguracionU
            {
                Cedula = newinfo.Cedula,
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,
                Userpassword = newinfo.Userpassword,
            };
        }
    }

    //-----------------------------------------------------------------------------------
    public class ConfiguracionA : ConfiguracionU
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        //public DateTime FechaNacimiento { get; set; }
        public string Username { get; set; }

        public int NivelUsuario { get; set; }

        public static ConfiguracionA NewConfiguracionA(ConfiguracionA newinfo)
        {
            return new ConfiguracionA
            {
                Nombres = newinfo.Nombres,
                Apellidos = newinfo.Apellidos,
                Cedula = newinfo.Cedula,
                //FechaNacimiento = newinfo.FechaNacimiento,
                Telefono = newinfo.Telefono,
                Correo = newinfo.Correo,

                Username = newinfo.Username,
                Userpassword = newinfo.Userpassword,
                NivelUsuario = newinfo.NivelUsuario,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    public class InformacionGeneral
    {
        public static InformacionGeneral NewInformacionGeneral(Personas persona, Usuarios usuario)
        {
            return new InformacionGeneral()
            {
                Persona = persona,
                Usuario = usuario,
            };
        }
        public Personas Persona { get; set; }
        public Usuarios Usuario { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public class LogInResponse
    {
        public InformacionGeneral UserInfo { get; set; }
        public DateTime UltimaFechaIngreso { get; set; }

        public static LogInResponse NewLogInResponse(InformacionGeneral info, DateTime ultimoingreso)
        {
            return new LogInResponse()
            {
                UserInfo = info,
                UltimaFechaIngreso = ultimoingreso,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public class ResponseQueryAdmin
    {
        public static ResponseQueryAdmin NewQueryAdmin(Personas persona)
        {
            return new ResponseQueryAdmin()
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
            };
        }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public double Cedula { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class RegistroTablero
    {
        public Tableros tableroInfo { get; set; }
        public List<ItemTablero> itemsTablero { get; set; }

        public static RegistroTablero NewRegistroTablero(Tableros tableroinfo, List<ItemTablero> itemstablero)
        {
            return new RegistroTablero()
            {
                tableroInfo = tableroinfo,
                itemsTablero = itemstablero,
            };
        }
    }

    //===================================================================================
    //===================================================================================
    public partial class RequestConsultaTablero
    {
        public double UserId { get; set; }
        public string TableroId { get; set; }
        public string SapId { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class RequestRegistroUsuario
    {
        public InformacionGeneral NewUser { get; set; }
        public double UserId { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class RequestQueryAdmin
    {
        public string Parametro { get; set; }
        public double UserId { get; set; }
    }

    //===================================================================================
    //===================================================================================
    public partial class UserSelectedRequest
    {
        public double UserIdSelected { get; set; }
        public double UserIdRequested { get; set; }

        /* CLASE OBJETO FORMADA POR DOS PARAMETROS/PROPIEDADES, 
         * UserIdSelected => Id del usuario a retornar
         * UserIdRequested => Id del usuario que realiza al solicitud
         */
    }
}
