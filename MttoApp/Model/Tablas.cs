using SQLite;
using System;
using System.Collections.Generic;

namespace MttoApp.Model
{
    //======================================================================================================
    //======================================================================================================
    public class Personas
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        //--------------------------------------------------------------------------------------------------
        //ID IDENTIFICATIVO
        [PrimaryKey]
        public double Cedula { get; set; }

        [Unique, MaxLength(4)]
        public int NumeroFicha { get; set; }

        //--------------------------------------------------------------------------------------------------
        [MaxLength(10)]
        public double? Telefono { get; set; }

        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }

        //======================================================================================================
        //======================================================================================================
        // PARA PROXIMAS ACTUALIZACIONES SE TRABAJARA CON LA FICHA COMO PRIMARY KEY

        //SE ADICIONA EL DEPARTAMENTO AL CUAL SE ENCUENTRA ASIGNADO
        /*public string Dpto { get; set; }*/

        //EL CARGO QUE DESEMPEÑA
        /*public string Cargo {get; set; }*/

        //EL SEXO (GENERO)
        //public char Sexo {get; set;}

        //======================================================================================================
        //======================================================================================================
        //FUNCION QUE AL SER LLAMADA PERMITE LLENAR TODOS LOS ATRIBUTOS DE UN OBJETO PERSONA

        public Personas NewPersona(Personas persona)
        {
            return new Personas()
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                Cedula = persona.Cedula,
                NumeroFicha = persona.NumeroFicha,
                Telefono = persona.Telefono,
                Correo = persona.Correo,
                FechaNacimiento = persona.FechaNacimiento,
                FechaCreacion = persona.FechaCreacion,
            };
        }

        public Personas NewPersona(DateTime fechacreacion, string nombres, string apellidos, string cedula, string numeroficha,
            DateTime fechanacimiento, string telefono, string correo)
        {
            return new Personas()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Cedula = Convert.ToDouble(cedula),
                NumeroFicha = int.Parse(numeroficha),
                Telefono = Convert.ToDouble(telefono),
                Correo = correo,
                FechaCreacion = fechacreacion,
                FechaNacimiento = fechanacimiento,
            };
        }

        //======================================================================================================
        //======================================================================================================
        //ADICIONALMENTE SE CONFIGURAN LAS PERSONAS QUE EXISTIRAN POR DEFECTO DENTRO DE LA APLICACION
        public List<Personas> GetDefaultPersonas()
        {
            List<Personas> DefaultPersonas = new List<Personas>()
            {
                //PERSONA ADMINISTRATOR
                new Personas()
                {
                    Nombres = "N/A",
                    Apellidos = "N/A",
                    Cedula = 0,                     //ID ADMINISTRATOR
                    NumeroFicha = 0,
                    FechaNacimiento = default,
                    Telefono = 0,
                    Correo = "N/A",
                    FechaCreacion = default
                },

                //PERSONA CARLOS ARTURO DAZA
                new Personas()
                {
                    Nombres = "Carlos Arturo",
                    Apellidos = "Daza Bohorquez",
                    Cedula = 24778839,
                    NumeroFicha = 2929,
                    FechaNacimiento = new DateTime(1996, 06, 03),
                    Telefono = 4147391846,
                    Correo = "carlos.arturo.dazab@gmail.com",
                    FechaCreacion = default,
                },

                //PERSONA JOSE ESCALONA
                new Personas()
                {
                    Nombres = "José Gregorio",
                    Apellidos = "Escalona Torres",
                    Cedula = 20943030,
                    NumeroFicha = 2867,
                    FechaNacimiento = new DateTime(1994, 02, 22),
                    Telefono = 4143511786,
                    Correo = "jose_escalona@corimon.com",
                    FechaCreacion = default,
                },

                //PERSONA ALEJANDRO PADRA
                new Personas()
                {
                    Nombres = "Alejandro Rafael",
                    Apellidos = "Padra Salom",
                    Cedula = 22515348,
                    NumeroFicha = 2824,
                    FechaNacimiento = new DateTime(1993, 12, 09),
                    Telefono = 4142022189,
                    Correo = "alejandro_padra@corimon.com",
                    FechaCreacion = default,
                },
            };

            return DefaultPersonas;
        }

        //======================================================================================================
        //======================================================================================================
        //FUNCION PARA RETORNAR LOS NIVELES DE USUARIOS
    }

    //======================================================================================================
    //======================================================================================================

    public class Usuarios
    {
        [MaxLength(15)]
        public string Username { get; set; }

        [MaxLength(15)]
        public string Password { get; set; }

        [PrimaryKey]
        public double Cedula { get; set; }

        public DateTime FechaCreacion { get; set; }

        //------------------------------------------------------------------------------------------------------
        public int NivelUsuario { get; set; }

        /*LOS NIVELES DE USUARIO VAN A SER JERARQUIZADOS EN UNA ESCALA DEL 0 - 10, DONDE:
            -0  => Nivel Basico
                * Consulta de Tableros.
                * Modificacion de datos personales.
            -5  => Nivel Medio
                * Consulta y escritura de tableros.
                * Creacion de Usuarios (?).
                * Creacion de Tableros (?).
            -10 => Nivel Alto
                * Consulta, escritura y Modificacion de Tableros.
                * Creacion y Modificacion de Usuarios.
                * Creacion de Tableros.*/

        //------------------------------------------------------------------------------------------------------
        //======================================================================================================
        //======================================================================================================
        //FUNCION QUE REGRESA UN OBJETO DEL TIPO USUARIO CON TODOS LOS ATRIBUTOS LLENOS.
        public Usuarios NewUsuario(Usuarios usuario)
        {
            return new Usuarios()
            {
                Cedula = usuario.Cedula,
                Username = usuario.Username,
                Password = usuario.Password,
                FechaCreacion = usuario.FechaCreacion,
                NivelUsuario = usuario.NivelUsuario,
            };
        }

        public Usuarios NewUsuario(string username, string password, string cedula, DateTime fechacreacion, int nivelusuario)
        {
            return new Usuarios()
            {
                Username = username,
                Password = password,
                Cedula = Convert.ToDouble(cedula),
                FechaCreacion = fechacreacion,
                NivelUsuario = nivelusuario,
            };
        }

        //======================================================================================================
        //======================================================================================================
        //ADICIONALMENTE SE CONFIGURAN LOS USUARIOS QUE EXISTIRAN POR DEFECTO DENTRO DE LA APLICACION
        public List<Usuarios> GetDefaultUsuarios()
        {
            List<Usuarios> DefaultUsuarios = new List<Usuarios>()
            {
                //USUARIO ADMINISTRATOR
                new Usuarios
                {
                    Username = "Administrator",
                    Cedula = 0,                         //ID USUARIO ADMINISTRATOR
                    Password = "4dm1n1str4t0r",
                    FechaCreacion = default,
                    NivelUsuario = 10,
                },

                //USUARIO CARLOS ARTURO DAZA
                new Usuarios
                {
                    Username = "ArturoDazaB",
                    Cedula = 24778839,
                    Password = "*960205*",
                    FechaCreacion = default,
                    NivelUsuario = 0,
                },

                //USUARIO JOSE ESCALONA
                new Usuarios
                {
                    Username = "jescalona",
                    Cedula = 20943030,
                    Password = "MaJo.,01*",
                    FechaCreacion = default,
                    NivelUsuario = 5,
                },

                //USUARIO ALEJANDRO PADRA
                new Usuarios
                {
                    Username = "apadra",
                    Cedula = 22515348,
                    Password = "arlettesofia",
                    FechaCreacion = default,
                    NivelUsuario = 5,
                },
            };

            return DefaultUsuarios;
        }

        //======================================================================================================
        //======================================================================================================
        //FUNCION PARA RETORNAR LOS NIVELES DE USUARIOS
        public List<string> NivelUsuarioLista()
        {
            return new List<string>()
            {
                "Nivel bajo",
                "Nivel medio",
                "Nivel superior",
            };
        }
    }

    //======================================================================================================
    //======================================================================================================

    public class UltimaConexion
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }

        public DateTime UltimaCon { get; set; }
        public string Username { get; set; }
        public double Cedula { get; set; }

        //===============================================================================================
        //===============================================================================================
        //SE MANDAN LOS DATOS NECESARIOS PARA PODER LLENAR TODOS LOS ATRIBUTOS
        //CADA QUE SE GENERE UN NUEVO REGISTRO EN LA TABLA UltimaConexion

        //FUNCION CON PARAMETROS Personas Y Usuarios
        public UltimaConexion NewUltimaConexion(Personas Persona, Usuarios Usuario)
        {
            return new UltimaConexion
            {
                UltimaCon = DateTime.Now,
                Username = Usuario.Username,
                Cedula = Persona.Cedula,
            };
        }

        //FUNCION CON PARAMETRO UltimaConexion
        public UltimaConexion NewUltimaConexion(UltimaConexion UltimaConexion)
        {
            return new UltimaConexion
            {
                UltimaCon = UltimaConexion.UltimaCon,
                Username = UltimaConexion.Username,
                Cedula = UltimaConexion.Cedula,
            };
        }
    }

    //======================================================================================================
    //======================================================================================================

    public class ModificacionesUsuarios
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        //ID DE LA PERSONA Y USUARIO A LA CUAL SE LE REALIZO MODIFICACIONES
        public double IDModificado { get; set; }

        //ID DE LA PERSONA Y USUARIO QUE REALIZO LA MODIFICACION
        public double IDModificador { get; set; }

        //FECHA Y HORA EN LA CUAL SE REALIZO LA MODIFICACION
        public DateTime FechaHora { get; set; }

        //---------------------------------------------------------------------------
        //QUE TIPO DE MODIFICACION SE REALIZO EN EL MOMENTO
        //ATRIBUTOS MODIFICABLES POR EL ADMINISTRADOR
        public bool ModificacionNombres { get; set; }

        public bool ModificacionApellidos { get; set; }
        public bool ModificacionUsername { get; set; }

        //ATRIBUTOS MODIFICACBLES POR EL ADMINISTRATOR Y POR USUARIOS DE BAJO NIVEL
        public bool ModificacionFecha { get; set; }

        public bool ModificacionTelefono { get; set; }
        public bool ModificacionCorreo { get; set; }
        public bool ModificacionPassword { get; set; }

        //===============================================================================
        //===============================================================================

        public ModificacionesUsuarios NewModificacionesUsuarios(Personas Persona, Personas PrevPersona,
                                                                    Usuarios Usuario, Usuarios PrevUsuario, DateTime fechahora, double UserId)
        {
            //SE CREA UN NUEVO OBJETO ModificacionesUsuarios
            ModificacionesUsuarios Modificaciones = new ModificacionesUsuarios();
            Modificaciones.FechaHora = DateTime.Now;

            Modificaciones.IDModificador = UserId;

            Modificaciones.IDModificado = Persona.Cedula;

            //SE EVALUA CUAL FUE EL CAMBIO REALIZADO
            if (Persona.Nombres != PrevPersona.Nombres)
                Modificaciones.ModificacionNombres = true;
            if (Persona.Apellidos != PrevPersona.Apellidos)
                Modificaciones.ModificacionApellidos = true;
            if (Persona.FechaNacimiento != PrevPersona.FechaNacimiento)
                Modificaciones.ModificacionFecha = true;
            if (Persona.Telefono != PrevPersona.Telefono)
                Modificaciones.ModificacionTelefono = true;
            if (Persona.Correo != PrevPersona.Correo)
                Modificaciones.ModificacionCorreo = true;
            if (Usuario.Username != PrevUsuario.Username)
                Modificaciones.ModificacionUsername = true;
            if (Usuario.Password != PrevUsuario.Password)
                Modificaciones.ModificacionPassword = true;

            return Modificaciones;
        }
    }

    //======================================================================================================
    //======================================================================================================

    public class Tableros
    {
        [PrimaryKey, Unique, MaxLength(20)]
        public string TableroID { get; set; }

        public string SapID { get; set; }
        public double IDCreador { get; set; }
        public string Filial { get; set; }
        public string AreaFilial { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodigoQRData { get; set; }
        public string CodigoQRFilename { get; set; }
        //========================================================================================================
        //========================================================================================================
        //FUNCION PARA LLENAR LOS ATRIBUTOS DE LA TABLA TABLEROS
        //CADA QUE SE GENERA UN NUEVO REGISTRO

        //METODO QUE RECIBE UN OBJETO TABLERO COMO PARAMETRO
        public static Tableros NuevoTablero(Tableros Tablero, double IDCreador)
        {
            return new Tableros
            {
                //------------------------------------------
                //INFORMACION DEL TABLERO
                TableroID = Tablero.TableroID.ToUpper(),
                SapID = Tablero.SapID,
                Filial = Tablero.Filial.ToLower(),
                AreaFilial = Tablero.AreaFilial.ToLower(),
                FechaRegistro = Tablero.FechaRegistro,
                //------------------------------------------
                //INFORMACION DEL CODIGO QR
                CodigoQRData = Tablero.CodigoQRData,
                CodigoQRFilename = Tablero.CodigoQRFilename,
                //------------------------------------------
                //ID DEL USUARIO QUE ACABA DE CREAR EL TABLERO
                IDCreador = IDCreador,
            };
        }

        //========================================================================================================
        //========================================================================================================
        //METODO QUE RECIBE LOS ATRIBUTOS POR SEPARADOS COMO PARAMETROS

        public static Tableros NuevoTablero(string tableroid, string sapid, string filial, string area, DateTime fecharegistro,
            string codigoqrdata, string codigoqrfilename, double id)

        {
            return new Tableros
            {
                //------------------------------------------
                //INFORMACION DEL TABLERO
                TableroID = tableroid.ToUpper(),
                SapID = sapid,
                Filial = filial.ToLower(),
                AreaFilial = area.ToLower(),
                FechaRegistro = fecharegistro,
                //------------------------------------------
                //INFORMACION DEL CODIGO QR
                CodigoQRData = codigoqrdata,
                CodigoQRFilename = codigoqrfilename,
                //------------------------------------------
                //ID DEL USUARIO QUE ACABA DE CREAR EL TABLERO
                IDCreador = id,
            };
        }
    }

    //======================================================================================================
    //======================================================================================================

    public class HistorialTableros
    {
        [PrimaryKey, AutoIncrement, Unique]
        //ID PARA CADA NUEVA ENTRADA EN LA TABLA "HistorialTableros"
        public int ID { get; set; }

        public string TipoDeConsulta { get; set; }

        //ID DEL TABLERO
        public string TableroID { get; set; }

        //ID DEL EMPLEADO QUE REALIZO LA ULTIMA REVISION DE TABLERO
        public double UsuarioID { get; set; }

        //ULTIMA REVISION DEL TABLERO
        public DateTime FechaDeConsulta { get; set; }

        //========================================================================================================
        //========================================================================================================
        //FUNCION PARA LLENAR LOS ATRIBUTOS DE LA TABLA HISTORIAL
        //TABLEROS CADA QUE SE GENERA UN NUEVO REGISTRO

        //METODO QUE RECIBE UN OBJETO TABLERO COMO PARAMETRO
        public HistorialTableros NewRegistroHistorial(HistorialTableros Historial)
        {
            return new HistorialTableros
            {
                ID = Historial.ID,
                TipoDeConsulta = Historial.TipoDeConsulta,
                TableroID = Historial.TableroID,
                UsuarioID = Historial.UsuarioID,
                FechaDeConsulta = Historial.FechaDeConsulta,
            };
        }

        //========================================================================================================
        //========================================================================================================
        //METODO QUE RECIBE LOS ATRIBUTOS POR SEPARADOS COMO PARAMETROS
        public HistorialTableros NewRegistroHistorial(string TableroID, double Cedula, DateTime Fecha, string TipoDeConsulta)
        {
            return new HistorialTableros
            {
                TableroID = TableroID,
                TipoDeConsulta = TipoDeConsulta,
                UsuarioID = Cedula,
                FechaDeConsulta = Fecha,
            };
        }
    }

    //======================================================================================================
    //======================================================================================================

    public class ActivoSAP
    {
        [PrimaryKey, MaxLength(20)]
        public string SapID { get; set; }

        public string Descripcion { get; set; }
    }

    //======================================================================================================
    //======================================================================================================

    public class ItemTablero
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string TableroId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }

        //======================================================================================================
        //======================================================================================================
        //FUNCIONES PARA LA CREACION DE UN NUEVO OBJETO DEL TIPO Item

        //FUNCION PARA LA CREACION DE UN NUEVO OBJETO EN BASE A UN OBJETO EXISTENTE
        public static ItemTablero NewItem(ItemTablero item)
        {
            return new ItemTablero
            {
                TableroId = item.TableroId.ToLower(),
                Descripcion = item.Descripcion.ToLower(),
                Cantidad = item.Cantidad,
            };
        }

        //FUNCION PARA LA CREACION DE UN NUEVO OBJETO EN BASE A CADA UNO DE LOS PARAMETROS
        public static ItemTablero NewItem(string tableroId, string descripcion, int cant)
        {
            return new ItemTablero
            {
                TableroId = tableroId.ToLower(),
                Descripcion = descripcion.ToLower(),
                Cantidad = cant,
            };
        }
    }
}