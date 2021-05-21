using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net.Http;
using MttoApp.Model;
using System.Text;
using System.Net.Http.Headers;

namespace MttoApp.ViewModel
{
    internal class ConfiguracionAdminViewModel : INotifyPropertyChanging
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES LOCALES
        protected DateTime fechacreacion, fechanacimiento;
        protected string nombres, apellidos, correo, username, password, cedula, telefono, confirmacionpassword, numeroficha;
        public int nivelusuario;
        //----------------------------------------------------------------------------
        protected bool onsave = false;
        protected bool isbusy = true;
        protected bool flagLecturaEscritura = false;    //=> Bandera que detectara desde que clase esta siendo invocada la clase "ConfiguracionAdminViewModel"
        protected bool flagUsername = false;

        //===============================================================================================
        //===============================================================================================
        //CONSTANTES
        protected const int telefonomaxlengt = 11;  //=> Maxima cantidad de caracteres permitidos para el campo Telefono.
        protected const int passwordmaxlengt = 12;  //=> Maxima cantidad de caracteres permitidos para los campos Password y ConfirmacionPassword.
        protected const int passwordyellowcolor = 6;    //=> Minima cantidad de caracteres permitidos para que el texto de los campos Password y ConfirmacionPassword cambie a color amarillo.
        protected const int passwordgreencolor = 9; //=> Minima cantidad de caracteres permitidos para que el texto de los campos Password y ConfirmacionPassword cambie a color verde.

        //===============================================================================================
        //===============================================================================================
        //BANDERAS:
        //flagCedula -> TRUE: LAS CEDULAS (ID) SON DISTINTAS
        //              FALSE: LAS CEDULAS (ID) SON IGUALES
        public bool flagCedula = false,
        //flagNumeroFicha -> TRUE: LAS FICHAS (NumeroFicha) SON DISTINTAS
        //              FALSE: LAS FICHAS (NumeroFicha) SON IGUALES
                    flagNumeroFicha = false;

        //BANDERAS DE VERIFICACION (Paginas de Lectura y Escritura)
        //NOTA: LAS ACCIONES DE ESCRITURA DE ESTAS VARIABLES SE EJECUTAN EN EL CODIGO "set" DE LAS PROPIEDADES DE LA CLASE
        public bool flagsameNombre = false,     //=> Bandera comparativa de modificacion de nombres (Pagina de Lectura y Escritura)
                    flagsameApellido = false,   //=> Bandera comparativa de modificacion de apellidos (Pagina de Lectura y Escritura)
                    flagsameFecha = false,      //=> Bandera comparativa de modificacion de fecha de nacimiento (Pagina de Lectura y Escritura)
                    flagsameTelefono = false,   //=> Bandera comparativa de modificacion de telefono (Pagina de Lectura y Escritura)
                    flagsameCorreo = false,     //=> Bandera comparativa de modificacion de correo (Pagina de Lectura y Escritura)
                    flagsameUsername = false,   //=> Bandera comparativa de modificacion de nombre de usuario (Pagina de Lectura y Escritura)
                    flagsamePassword = false,   //=> Bandera comparativa de modificacion de contraseña/password (Pagina de Lectura y Escritura)
                    flagdifferentPassword = false; //=> Bandera comparativa entre la contraseña/password y la confirmacion de la contraseña/password (Pagina de Escritura)

        //BANDERAS DE VERIFICACION (Paginas de Escritura)
        protected bool flagExistingUsername = false; //=> Bandera que se dispara cuando se detecte que la cedula ya se encuentra registrada

        //===============================================================================================
        //===============================================================================================
        //------------------------------------------OBJETOS LOCALES--------------------------------------
        //NOTA: OBJETOS UTILIZADOS CUANDO LA CLASE "ConfiguracionAdminViewModel" ES INVOCADA DESDE
        //LAS PAGINAS DE LECTURA Y ESCRITURA => "PaginaConfiguracion" y "PaginaConfiguracionAdmin"
        protected Personas Persona; //=> OBJETO QUE CONTENDRA LA INFORMACION DEL USUARIO A MODIFICAR
        protected Usuarios Usuario; //=> OBJETO QUE CONTENDRA LA INFORMACION DEL USUARIO A MODIFICAR
        protected double UserId;    //=> VARIABLE QUE RECIBE EL ID DEL USUARIO QUE SE ENCUENTRA NAVEGANDO

        //===============================================================================================
        //===============================================================================================
        //-----------------------------------PROPIEDADES DE LA CLASE-------------------------------------
        //PARA EL RESTO DE PROPIEDADES DE LA CLASE (EXCEPTO LA PROPIEDAD CEDULA y FECHACREACION)
        //SE APLICA EL MISMO CODIGO DE:
        //	1)DETECCION DE CAMBIOS
        //	2)IMPRESION POR CONSOLA
        //	3)DISPARO DE BANDERAS
        public DateTime FechaCreacion
        {
            //NOTA: EN EL CASO DE LA PROPIEAD "FechaCreacion" NO SE UTILIZA NINGUNO DE LOS EVENTOS ASIGNADOS
            //A LAS DEMAS PROPIEDADES (COMO ES EL CASO DE: "OnPropertyChanged" O "ConsoleWriteline", ESTO
            //DEBIDO A QUE NINGUN USUARIO TIENE LA FACULTAD DE MODIFICAR ESTE CAMPO DE LA TABLA "Personas"

            get
            {
                //SE LEE LA INFORMACION QUE POSEE fechaCreacion
                return fechacreacion;
            }
            set
            {
                //LE ASIGNA EL VALOR A LA VARIABLE LOCAL "fechaCreacion" LA INFORMACION QUE "Persona.FechaCreacion" ALMACENA
                fechacreacion = value;
            }
        }
        public string Nombres
        {
            get { return nombres; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "nombres" EL VALOR INGRESADO POR EL USUARIO
                nombres = value;

                //=================================================================================================
                //=================================================================================================
                //EVENTO QUE SE DISPARA CUANDO SE MODIFICA O ACTUALIZA LA PROPIEDAD (CUANDO EL USUARIO SE ENCUENTRA
                //INTERACTUANDO CON LAS ENTRADAS DE LA APLICACION: Entry, Picker, DatePicker, etc).
                OnPropertyChanged();

                //flagLecturaEscritura => true => Pagina de LecturaEscritura => "PaginaConfiguracionAdmin" y "PaginaConfiguracion"
                //flagLecturaEscritura => false => Pagina de Escritura => "PaginaRegistro"

                if (flagLecturaEscritura == false)
                {
                    //SI SE ACCEDE DESDE UNA PAGINA DE ESCRITURA (PaginaRegistro.xaml.cs) SE PROCEDE A ACTUALIZAR 
                    //EL NOMBRE DE USUARIO CADA QUE ESTE SEA MODIFICADO
                    OnPropertyChanged(Username);
                }

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs")

                if (flagLecturaEscritura) //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Nombre(s)", Persona.Nombres, Nombres);
                }
                else //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Nombres(s)", Nombres);
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (Nombres == Persona.Nombres)
                    flagsameNombre = true;
                else
                    flagsameNombre = false;
            }
        }
        public string Apellidos
        {
            get { return apellidos; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "apellidos" EL VALOR INGRESADO POR EL USUARIO
                apellidos = value;

                //=================================================================================================
                //=================================================================================================
                //EVENTO QUE SE DISPARA CUANDO SE MODIFICA O ACTUALIZA LA PROPIEDAD (CUANDO EL USUARIO SE ENCUENTRA
                //INTERACTUANDO CON LAS ENTRADAS DE LA APLICACION: Entry, Picker, DatePicker, etc).
                OnPropertyChanged();

                //flagLecturaEscritura => true => Pagina de LecturaEscritura => "PaginaConfiguracionAdmin" y "PaginaConfiguracion"
                //flagLecturaEscritura => false => Pagina de Escritura => "PaginaRegistro"
                if (flagLecturaEscritura == false)
                {
                    //SI SE ACCEDE DESDE UNA PAGINA DE ESCRITURA (PaginaRegistro.xaml.cs o PaginaRegistro2.xaml.cs)
                    //SE PROCEDE A ACTUALIZAR EL NOMBRE DE USUARIO CADA QUE ESTE SEA MODIFICADO
                    OnPropertyChanged(Username);
                }

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs")

                if (flagLecturaEscritura)   //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Apellido(s)", Persona.Apellidos, Apellidos);
                }
                else //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Apellido(s)", Apellidos);
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (Apellidos == Persona.Apellidos)
                    flagsameApellido = true;
                else
                    flagsameApellido = false;
            }
        }
        public string Cedula
        {
            get { return cedula; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "apellidos" EL VALOR INGRESADO POR EL USUARIO
                cedula = value;

                //PUESTO QUE, TANTO EN LAS PAGINAS DE ESCRITURA COMO EN LAS PAGINAS DE LECTURA Y ESCRITURA
                //LA CEDULA (ID) ES UN CAMPO NO MODIFICABLE, ESTA PROPIEDAD SOLO SE IMPRIMIRA POR CONSOLA
                //CADA QUE SEA MODIFICADA (=> "PaginaRegistro")
                ConsoleWriteline("Cedula (ID)", Cedula);
            }
        }
        public string NumeroFicha
        {
            get { return numeroficha; }
            set
            {
                //SE LLAMA A ESTA FUNCION PARA DETECTAR CAMBIOS EN LA PROPIEDAD
                OnPropertyChanged();
                //SE LE ASIGNA A LA VARIABLE "numeroficha" EL VALOR INGRESADO POR EL USUARIO
                numeroficha = value;

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs")

                if (flagLecturaEscritura) //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Numero de Ficha", Persona.NumeroFicha.ToString(), FechaNacimiento.ToString());
                }
                else //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Numero de Ficha", NumeroFicha.ToString());
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PÁGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTÚE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (NumeroFicha == Persona.NumeroFicha.ToString())
                    flagNumeroFicha = true;
                else
                    flagNumeroFicha = false;
            }
        }
        public DateTime FechaNacimiento
        {
            get { return fechanacimiento; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "fechanacimiento" EL VALOR INGRESADO POR EL USUARIO
                fechanacimiento = value;
                OnPropertyChanged();

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs")
                if (flagLecturaEscritura)//=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Fecha de Nacimiento", Persona.FechaNacimiento.ToString(), FechaNacimiento.ToString());
                }
                else  //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Fecha de Nacimiento", FechaNacimiento.ToString());
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PÁGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTÚE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (FechaNacimiento == Persona.FechaNacimiento)
                    flagsameFecha = true;
                else
                    flagsameFecha = false;
            }
        }
        public string Telefono
        {
            get { return telefono; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "nombres" EL VALOR INGRESADO POR EL USUARIO
                telefono = value;
                OnPropertyChanged();

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs").

                if (flagLecturaEscritura)  //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Telefono", Persona.Telefono.ToString(), Telefono);
                }
                else  //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Telefono", Telefono);
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (Telefono == Persona.Telefono.ToString())
                    flagsameTelefono = true;
                else
                    flagsameTelefono = false;
            }
        }
        public string Correo
        {
            get { return correo; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "correo" EL VALOR INGRESADO POR EL USUARIO
                correo = value;
                OnPropertyChanged();

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs").

                if (flagLecturaEscritura)  //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Correo", Persona.Correo, Correo);
                }
                else  //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Correo", Correo);
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                if (Correo == Persona.Correo)
                    flagsameCorreo = true;
                else
                    flagsameCorreo = false;
            }
        }
        public string Username
        {
            //------------------------------------NOTA--------------------------------------
            //EL NOMBRE DE USUARIO SE CREARÁ DE MANERA AUTOMÁTICA BAJO LA SIGUIENTE PREMISA:
            //  USERNAME = Inicial Primer Nombre + Apellido
            //DE EXISTIR ALGUN REGISTRO EN LA TABLA "Usuarios" QUE CONTENGA DICHO NOMBRE
            //DE USUARIO SE PROCEDERA GENERAR EL NOMBRE DE USUARIO DE LA SIGUIENTE MANERA:
            //  USERNAME = Inicial Primer Nombre + Inicial Segundo Nombre + Apellido
            //------------------------------------------------------------------------------

            get
            {
                //EVALUAMOS DESDE QUE PAGINA ESTAMOS INGRESANDO
                if (flagLecturaEscritura == false) //=> flagLecturaEscritura = false => PaginaRegistro
                {
                    //SE INICIALIZA LA VARIABLE "username"
                    username = string.Empty;

                    //SE EVALUA QUE LOS ATRIBUTOS "Nombre(s)" Y "Apellido(s)" NO SE ENCUENTREN VACIAS
                    if (!string.IsNullOrEmpty(Nombres) && !string.IsNullOrEmpty(Apellidos))
                    {
                        //SE GENERA EL NOMBRE DE USUARIO
                        //USERNAME = Inicial Primer Nombre + Apellido
                        username = char.ToLower(Nombres[0]) + Metodos.FirstString(Apellidos.ToLower());

                        //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
                        var current = Connectivity.NetworkAccess;
                        //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
                        if (current == NetworkAccess.Internet)
                        {
                            //EL EQUIPO SE ENCUENTRA CONECTADO A INTERNET
                            //SE INICIA EL CICLO TRY...CATCH
                            try
                            {
                                //INICIAMOS EL SEGMENTO DEL CODIGO EN EL CUAL REALIZAREMOS EL CONSUMO DE SERVICIOS WEB MEDIANTE
                                //LA INICIALIZACION Y CREACION DE UNA VARIABLE QUE FUNCIONARA COMO CLIENTE EN LAS SOLICITUDES 
                                //Y RESPUESTAS ENVIADAS Y RECIBIDAS POR EL SERVIDOR (WEB API) 
                                //----------------------------------------------------------------------------------------------
                                //NOTA: CUANDO SE REALIZA LA CREACION E INICIALIZACION DE LA VARIABLE DEL TIPO HttpClient SE
                                //HACE UN LLAMADO A UN METODO ALOJADO EN LA CLASE "App" Y QUE ES ENVIADO COMO PARAMETRO DEL 
                                //TIPO HttpClientHandler => 
                                //----------------------------------------------------------------------------------------------
                                using (HttpClient client = new HttpClient(App.GetInsecureHandler()))
                                {
                                    //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
                                    string url = App.BaseUrl + $"/verifygeneratedusername?generatedusername={username}";

                                    //SE DA SET AL TIEMPO MAXIMO DE ESPERA PARA RECIBIR UNA RESPUESTA DEL SERVIDOR
                                    client.Timeout = TimeSpan.FromSeconds(App.TimeInSeconds);

                                    //SE CONFIGURAN LOS HEADERS DE LA SOLICITUD HTTP (HTTP REQUEST)
                                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                    //SE CREA LA VARIABLE QUE CONTENDRA LA RESPUESTA DE LA SOLICITUD
                                    HttpResponseMessage response = new HttpResponseMessage();
                                    string result = string.Empty;

                                    //SE REALIZA EL LLAMADO Y SE RECIBE UNA RESPUESTA
                                    Task.Run(async () =>
                                    {
                                        //RECIBIMOS LA RESPUESTA A LA SOLICITUD HTTP
                                        response = await client.GetAsync(url);
                                    });

                                    //SE EVALUA SI EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        //EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                                        //SE RECIBE LA RESPUESTA COMPLETA OBTENIDA POR EL SERVIDOR (STRING)
                                        Task.Run(async () =>
                                        {
                                            //SE RECIBE UN OBJETO JSON CON LA RESPUESTA Y SE TRANSFORMA A SU EQUIVALENTE BOOLEANO
                                            flagExistingUsername = await Task.FromResult(JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync()));
                                            //flagExistingUsername = true => Nombre de usuario ya existente
                                            //flagExistingUsername = false => Nombre de usuario no existente
                                        });
                                    }
                                }
                            }
                            catch (Exception ex) when (ex is HttpRequestException ||
                                                       ex is Javax.Net.Ssl.SSLException ||
                                                       ex is Javax.Net.Ssl.SSLHandshakeException ||
                                                       ex is System.Threading.Tasks.TaskCanceledException)
                            {

                            }

                            //EVALUACION DE LA RESPUESTA OBTENIDA POR LA SOLICITUD WEB
                            if (flagExistingUsername == true)
                            {
                                //SE VUELVE A GENERAR EL NOMBRE DE USUARIO BAJO LA SIGUIENTE PREMISA
                                //USERNAME = INICIAL PRIMER NOMBRE + INICIAL SEGUNDO NOMBRE + PRIMER APELLIDO
                                username = char.ToUpper((Metodos.FirstString(Nombres))[0]) + char.ToUpper((Metodos.SecondString(Nombres))[0]) + Metodos.FirstString(Apellidos.ToLower());

                                //SE IMPRIME POR CONSOLA EL NOMBRE DE USUARIO
                                ConsoleWriteline("Username", username);
                            }
                            else
                            {
                                //SE IMPRIME POR CONSOLA EL NOMBRE DE USUARIO
                                ConsoleWriteline("Username", username);
                            }

                        }
                        else
                        {

                        }
                    }
                }

                return username;
            }
            set
            {
                if (flagLecturaEscritura)//=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    //SE LE ASIGNA A LA VARIABLE "correo" EL VALOR INGRESADO POR EL USUARIO
                    username = value;
                    OnPropertyChanged();

                    //SE IMPRIME POR CONSOLA EL NUEVO NOMBRE DE USUARIO
                    ConsoleWriteline("Username", Usuario.Username, Username);

                    //=================================================================================================
                    //=================================================================================================
                    //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                    //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                    //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA
                    if (Username == Usuario.Username)
                        flagsameUsername = true;
                    else
                        flagsameUsername = false;
                }
            }
        }
        public int NivelUsuario
        {
            //NOTA: ESTA PROPIEDAD ES DISTINTA A LAS DEMAS DEBIDO A QUE ES DEL TIPO INT 
            //EN VEZ DE STRING. PUESTO QUE LOS NIVELES DE USUARIOS SE SELECCIONAN MEDIANTE
            //UNA LISTA DESPLEGABLE SE OBTIENE CUAL OPCION FUE SELECCIONADA DIRECTAMENTE
            //EN EL METODO "OnSeleccionNivelUsuarios" DE LA PAGINA "PaginaRegistro". ADEMAS
            //ESTA PROPIEDAD NO ES MODIFICABLE EN NINGUNA DE LAS PAGINAS DE CONFIGURACION
            get
            {
                OnPropertyChanged();
                return nivelusuario;
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "password" EL VALOR INGRESADO POR EL USUARIO
                password = value;
                OnPropertyChanged();

                //=================================================================================================
                //=================================================================================================
                //EVENTOS QUE, DEPENDIENDO DESDE QUE PAGINA SE INVOCA A LA CLASE "ConfiguracionAdminViewModel.cs",
                //MOSTRARA EL VALOR QUE SE ESTA INGREANDO (CASO PAGINA "PaginaRegistro.xaml.cs") O MOSTRANDO EL
                //VALOR PREVIO Y EL VALOR MODIFICADO (CASO PAGINA "PaginaConfiguracion.xaml.cs").

                if (flagLecturaEscritura) //=> flagLecturaEscritura = true => Pagina de Lectura y Escritura
                {
                    ConsoleWriteline("Password", Usuario.Password, Password);
                }
                else //=> flagLecturaEscritura = false => Pagina DE Escritura
                {
                    ConsoleWriteline("Password", Password);
                }

                //=================================================================================================
                //=================================================================================================
                //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA

                if (Password == Usuario.Password)
                    flagsamePassword = true;
                else
                    flagsamePassword = false;
            }
        }
        public string ConfirmacionPassword
        {
            get { return confirmacionpassword; }
            set
            {
                //SE LE ASIGNA A LA VARIABLE "confirmacionpassword" EL VALOR INGRESADO POR EL USUARIO
                confirmacionpassword = value;
                OnPropertyChanged();

                //SE IMPRIME POR CONSOLA EL NUEVO NOMBRE DE USUARIO
                ConsoleWriteline("Confirmacion Password", ConfirmacionPassword);

                //SE COMPARA EL TAMAÑO DE LOS TEXTOS INGRESADOS EN LOS CAMPOS "Password" Y "ConfirmacionPassword"
                if (ConfirmacionPassword.Length >= Password.Length)
                {
                    //=================================================================================================
                    //=================================================================================================
                    //PARA EL CASO DE LAS PAGINAS DE LECTURA Y ESCRITURA CUANDO EL USUARIO INTERACTUE CON ALGUNA
                    //DE LAS PROPIEDADES DE LA CLASE ESTAS EVALUARAN SI EL VALOR QUE POSEEN ES IGUAL AL VALOR QUE 
                    //YA SE ENCUENTRA REGISTRADO, DE SER CIERTO SE DISPARA UNA BANDERA

                    if (ConfirmacionPassword != Password)
                        flagdifferentPassword = true;
                    else
                        flagdifferentPassword = false;
                }
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        //TAMAÑOS DE CARACTERES PARA LOS COLORES DEL TEXTO ASIGNADOS A LOS CAMPOS Password y ConfirmacionPassword 
        public int PasswordMaxLegnth { get { return passwordmaxlengt; } }
        public int PasswordYellowColorLegnth { get { return passwordyellowcolor; } }
        public int PasswordGreenColorLegnth { get { return passwordgreencolor; } }

        //---------------------------------------------------------------------------------------------------------
        //MAXIMA CANTIDAD DE NUMEROS PARA EL CAMPO TELEFONO
        public int TelefonoMaxLegnt { get { return telefonomaxlengt; } }

        //---------------------------------------------------------------------------------------------------------
        //TAMAÑO DE LAS LETRAS
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }

        //---------------------------------------------------------------------------------------------------------
        //COLOR DE FONDO Y DE BOTONES
        public string BackGroundColor { get { return App.BackGroundColor; } }
        public string ButtonColor { get { return App.ButtonColor; } }

        //---------------------------------------------------------------------------------------------------------
        //TEXTOS
        //=> TEXTOS UTILIZADOS EN LOS PlaceHolder DE LOS ELEMENTOS DE LA PAGINA
        public string RegistroPH { get { return "Registro de Usuario"; } }
        public string ConfiguracionPH { get { return "Informacion de Usuario"; } }
        public string InformacionP { get { return "Datos Personales"; } }
        public string InformacionU { get { return "Datos de Usuario"; } }
        public string FechaRegistroPH { get { return "Fecha de Registro: "; } }
        public string NombresPH { get { return "Nombre(s): "; } }
        public string ApellidosPH { get { return "Apellido(s): "; } }
        public string CedulaPH { get { return "Cedula: "; } }
        public string NumeroFichaPH { get { return "Número de Ficha"; } }
        public string FechaNacimientoPH { get { return "Fecha de Nacimiento: "; } }
        public string TelefonoPH { get { return "Teléfono celular/móvil: "; } }
        public string CorreoPH { get { return "Correo electrónico (email): "; } }
        public string UsernamePH { get { return "Nombre de usuario (username): "; } }
        public string NivelUsuarioPH { get { return "Nivel de Usuario: "; } }
        public string PickerTitulo { get { return "Opciones"; } }
        public string PasswordPH { get { return "Contraseña (password): "; } }
        public string Actualizar { get { return "ACTUALIZAR"; } }
        public string Registrar { get { return "REGISTRAR"; } }
        //_______________________________________________________________________________________________________________________________________________________________________________________
        //TEXTO UTILIZADO EN LA FUNCION "OnActualizar" => FUNCION ACTIVADA CUANDO SE MODIFICAN LOS DATOS DE UN USUARIO EN LA PAGINA "PaginaConfiguracion"
        public string OnActualizarMethodMessage { get { return "Está a punto de realizar una modificación de datos. Toda la información suministrada será modificada.\n\n¿Desea continuar?"; } }
        //TEXTO UTILIZADO EN LA FUNCION "OnButtonPuch" => FUNCION ACTIVADA CUANDO SE DESEA REGISTRAR UN USUARIO DENTRO DE LA PLATAFORMA
        public string OnButtonPushMethodMessage { get { return "Está a punto de realizar un nuevo registro de usuario.\n\n¿Desea continuar?"; } }
        //TEXTO UTILIZADO EN LAS FUNCIONES "OnCompletedPassword1" y "OnCompletedPassword2" => FUNCIONES ACTIVADAS CUANDO SE TERMINA DE INGRESAR 
        //TEXTO EN LOS CAMPOS ASIGNADOS A LA CONTRASEÑA Y LA CONFIRMACION DE LA CONTRASEÑA
        public string OnCompletePasswordWhiteSpace { get { return "La contraseña no puede contener espacios en blanco."; } }
        //TEXTO UTILIZADO EN LA FUNCION "OnCompletedCorreo" => FUNCION ACTIVADA CUANDO SE TERMINA DE INGRESAR TEXTO EN EL CAMPO ASIGNADO AL CORREO
        public string OnCompletedCorreoWhiteSpace { get { return "El correo no puede contener espacios en blanco."; } }
        //TEXTO UTILIZADO EN LA FUNCION "OnCompletedUsername" => FUNCION ACTIVADA CUANDO SE TERMINA DE INGRESAR TEXTO EN EL CAMPO ASIGNADO AL USERNAME
        public string OnCompletedUsernameWhiteSpace { get { return "El nombre de usuario no puede contener espacios en blanco."; } }
        //TEXTO UTILIZADO EN LAS FUNCIONES "OnCompletedPassword1" y "OnCompletedPassword2" => FUNCIONES ACTIVADAS CUANDO SE TERMINA DE INGRESAR 
        //TEXTO EN LOS CAMPOS ASIGNADOS A LA CONTRASEÑA Y LA CONFIRMACION DE LA CONTRASEÑA
        public string OnCompletePasswordMinimunLenght { get { return "La contraseña no puede tener menos de seis (6) caracteres."; } }
        //TEXTOS UTILIZADO EN LA FUNCION "OnDateSelected" => FUNCION ACTIVADA CUANDO SE TERMINA DE SELECCIONAR LA FECHA 
        //PROPIEDAD UTILIZADA EN LAS CLASES "PaginaRegistro", "PaginaConfiguracion" Y "PaginaConfiguracionAdmin"
        public string OnDateSelectedMessage
        {
            //NOTA: PUESTO QUE LA FUNCION "OnDateSelected" (FUNCION DE LA CLASE
            //"PaginaRegistro") SE EVALUAN LAS DOS CONDICIONES (NO SELECCIONAR
            //LA FECHA ACTUAL Y NO SELECCIONAR UNA FECHA MAYOR) SE DECIDIO CREAR 
            //UNA PROPIEDAD QUE RETORNARA EL MENSAJE PARA LAS DOS SITUACIONES.

            get
            {
                //SE CREA E INICIALIZA LA VARIABLE QUE RETORNARA EL TEXTO A MOSTRAR
                string text = string.Empty;

                //SE EVALUA SI LA FECHA SELECCIONADA ES LA FECHA ACTUAL 
                if (FechaNacimiento == DateTime.Today)  //=> true => Se selecciono la fecha actual 
                    text = "No se permite seleccionar la fecha actual como fecha de nacimiento.";

                //SE EVALUA SI LA FECHA SELECCIONADA ES MAYOR A LA FECHA ACTUAL
                if (FechaNacimiento > DateTime.Today) //=> true => Se selecciono una fecha superior a la fecha actual
                    text = "No se permite seleccionar una fecha que no ha existido todavía.";

                if (flagsameFecha)
                    text = "La fecha seleccionada es igual a la que se encuentra registrada actualmente.";

                //SE RETORNA EL TEXTO SELECCIONADO
                return text;

            }
        }
        //TEXTO UTILIZADO EN LAS FUNCIONES "OnCompletedPassword2" y "CorreccionPassword2" => FUNCIONES ACTIVADAS CUANDO SE DEJA DE ENFOCAR EL
        //ENTRY "passwordEntry2" Y CUANDO EL TEXTO DENTRO DEL ENTRY "passwordEntry2" CAMBIA (respectivamente), AMBAS FUNCIONES DE LA CLASE "PaginaRegistro"
        public string PasswordDoesNotMatch { get { return "Las contraseñas ingresadas no coinciden\n\nVerifique e intente nuevamente."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO DE LOS CARACTERES NO PERMITIDOS => PROPIEDAD USADA EN LAS FUNCIONES
        //PERTENECIENTE A LAS CLASE "PaginaRegistro" Y "PaginaConfiguracion".
        public string ForbiddenCharacters { get { return "No se aceptan los siguientes caracteres:\n\n" + App.ForbiddenCharacters; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedNombres" DE LA CLASE "PaginaConfiguracionAdmin".
        public string OnCompletedNombreSameNombre { get { return "El/los nombre(s) ingresado(s) es igual al que se encuentra actualmente registrado."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedApellidos" DE LA CLASE "PaginaConfiguracionAdmin".
        public string OnCompletedApellidoSameApellido { get { return "El/los apellido(s) ingresado(s) es igual al que se encuentra actualmente registrado."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO QUE SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedTelefono" DE LA CLASE "PaginaConfiguracionAdmin".
        public string OnCompletedTelefonoSameTelefono { get { return "El número de teléfono ingresado es igual al que se encuentra registrado."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedCorreo" DE LAS CLASES "PaginaConfiguracion" Y "PaginaConfiguracionAdmin".
        public string OnCompletedCorreoSameCorreo { get { return "El correo ingresado es igual al que se encuentra registrado."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedUsername" DE LA CLASE "PaginaConfiguracionAdmin".
        public string OnCompletedUsernameSameUsername { get { return "El nombre de usuario ingresado es igual al que se encuentra actualmente registrado."; } }
        //TEXTO UTILIZADO PARA INFORMAR AL USUARIO SOBRE EL INGRESO DE LA MISMA INFORMACION YA ALMACENADA => PROPIEDAD USADA EN LAS FUNCIONES
        //"OnUnfocusedPassword" DE LA CLASE "PaginaConfiguracion" Y "PaginaConfiguracionAdmin".
        public string OnCompletedPasswordSamePassword { get { return "La contraseña ingresada es igual a la que se encuentra registrada."; } }
        //_______________________________________________________________________________________________________________________________________________________________________________________
        //----------------------------------------------------------------------------------------------------------
        //TEXTOS UTILIZADOS PARA REPRESENTAR LA AFIRMACION O NEGACION DEL USUARIO ANTE UNA PETICION
        //NOTA: DICHOS TEXTOS SON USADOS EN LOS DISPLAYALERT EN LAS SECCIONES DE AFIRMACION (SI) Y NEGACION (NO) DEL MENSAJE APARENTE
        public string AffirmativeText { get { return App.AffirmativeText; } } //=> SI
        public string NegativeText { get { return App.NegativeText; } } //=> NO
        public string OkText { get { return App.OkText; } } //=> Entendido

        //----------------------------------------------------------------------------------------------------------
        //LISTA DE NIVELES DE USUARIO (USADO EN LA PAGINA "PaginaRegistro.xaml.cs")
        public List<string> NivelUsuarioList
        {
            get
            {
                //LISTA DE NIVELES DE USUARIO
                return new Usuarios().NivelUsuarioLista();
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        public bool Error
        {
            get
            {
                if (flagCedula)
                    MensajeConsole("OCURRIO UN ERROR AL ABRIR LA PAGINA");
                return flagCedula;
            }
        }

        public bool OnSave
        {
            get
            {
                return onsave;
            }

            set
            {
                onsave = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;

                OnPropertyChanged();
            }
        }

        //===============================================================================================
        //===============================================================================================
        //-------------------------------------------EVENTOS---------------------------------------------
        public event PropertyChangingEventHandler PropertyChanging;

        //===============================================================================================
        //===============================================================================================
        //-------------------------------------------METODOS---------------------------------------------
        //CONSTRUCTOR DE LA CLASE
        public ConfiguracionAdminViewModel(bool flag, Personas persona, Usuarios usuario, double userid)
        {
            //=================================================================================================================================
            //=================================================================================================================================
            //LA VARIABLE "flagLecturaEscritura" (DEL TIPO BOOL) INDICARA SI SE ESTA INVOCANDO LA CLASE "ConfiguracionAdminViewModel" DESDE UNA
            //PAGINA DE LECTURA Y ESCRITURA (COMO ES EL CASO DE LA PAGINA "ConfiguracionAdmin.xaml.cs" LA CUAL LEE Y MODIFICA
            //LOS ATRIBUTOS DE LOS OBJETOS Persona y Usuario) O SIMPLEMENTE ESCRITURA (COMO ES EL CASO DE LA PAGINA "PaginaRegistro.xaml.cs")

            //flagLecturaEscritura => true => Pagina de LecturaEscritura
            //flagLecturaEscritura => false => Pagina de Escritura

            flagLecturaEscritura = flag;

            //=================================================================================================================================
            //=================================================================================================================================
            //SI "ConfiguracionAdminViewModel.cs" ES INVOCADA DESDE LAS PAGINAS "PaginaConfiguracion.xaml.cs" y "PaginaConfiguracionAdmin.xaml.cs"
            //SE PROCEDE A CARGAR TODA LA INFORMACION DE LA PERSONA A MODIFICAR. POR OTRO LADO, SI "ConfiguracionAdminViewModel.CS" ES INVOCADA
            //DESDE LA PAGINA "PaginaRegistro.xaml.cs" SE DEJAN VACIOS LOS CAMPOS

            if (flagLecturaEscritura == false)//CLASE INVOCADA DESDE LA CLASE "PaginaRegistro"
            {
                //SE INICIALIZAN LAS VARIABLES LOCALES
                nombres = apellidos = cedula = numeroficha = telefono = correo = username = password = confirmacionpassword = string.Empty;
                nivelusuario = 0;
                fechacreacion = fechanacimiento = DateTime.Now;

                //SE ALMACENA LA INFORMACION DEL USUARIO QUE SE ENCUENTRA LOGGEADO EN EL MOMENTO
                Persona = new Personas();
                Usuario = new Usuarios();

                //SE ALMACENA EL ID DEL USUARIO QUE SE ENCUENTRA LOGGEADO DENTRO DE LA APLICACION
                UserId = userid;

                MensajeConsole("PUNTO DE ACCESO => PAGINA REGISTRO");
            }
            else//CLASE INVOCADA DESDE LA CLASE "PaginaConfiguracion.xaml.cs" O "PaginaConfiguracionAdmin.xaml.cs"
            {
                //SE CREAN LOS OBJETOS QUE CONTENDRAN LA INFORMACION DEL USUARIO A MODIFICAR
                Persona = new Personas().NewPersona(persona);
                Usuario = new Usuarios().NewUsuario(usuario);

                //SE ALMACENA EL ID DEL USUARIO QUE SE ENCUENTRA LOGGEADO DENTRO DE LA APLICACION
                UserId = userid;

                //SE CARGA LA INFORMACION EN LAS VARIABLES LOCALES
                nombres = Persona.Nombres;
                apellidos = Persona.Apellidos;
                cedula = Persona.Cedula.ToString();
                numeroficha = Persona.NumeroFicha.ToString();
                fechacreacion = Persona.FechaCreacion;
                fechanacimiento = Persona.FechaNacimiento;
                telefono = Persona.Telefono.ToString();
                correo = Persona.Correo;

                username = Usuario.Username;
                nivelusuario = Usuario.NivelUsuario;
                password = Usuario.Password;

                MensajeConsole("PUNTO DE ACCESO => PAGINA CONFIGURACION");

                //=================================================================================================================================
                //=================================================================================================================================
                //SE VERIFICA QUE LOS DOS OBJETOS CONTENGAN EL MISMO IDENTIFICATIVO
                if (Persona.Cedula != Usuario.Cedula)
                    flagCedula = true;
                else
                    flagCedula = false;
            }
        }

        //===============================================================================================
        //===============================================================================================
        //ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO MINIMO
        protected void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nombre));
        }

        //===============================================================================================
        //===============================================================================================
        //METODO PARA ACTUALIZAR ("PaginaConfiguracion2.xaml.cs" y "PaginaConfiguracionAdmin2.xaml.cs"
        //Y GUARDAR DATOS ("PaginaRegistro.xaml.cs")
        public async Task<string> Save()
        {
            //SE CREA E INICIALIZA EL MENSAJE QUE SERA RETORNADO A LA PAGINA DESDE
            //DONDE SE HAYA INVOCADO A LA CLASE
            string respuesta = string.Empty;

            //SE VERIFICA SI SE HA REALIZADO AL MENOS ALGUN CAMBIO A ALGUNO DE LOS CAMPOS
            //NOTA: DE INVOCAR EL METODO DESDE LA PAGINA "PaginaRegistro.xaml.cs" EL METODO
            //RETORNARA TRUE POR DEFECTO
            if (EvaluacionDeCampos1())
            {
                //SE VERIFICA QUE LAS PROPIEDADES/ATRIBUTOS DE LA PAGINA NO SEAN VACIOS O NULOS
                //ADEMAS DE VERIFICAR QUE LA FECHA SELECCIONADA NO SEA LA ACTUAL Y NO SEA UNA FECHA QUE
                //NO HA EXISTIDO O SUCEDIDO.
                if (EvaluacionDeCampos2(false))
                {
                    //==========================================================================================
                    //==========================================================================================

                    //SE VERIFICA QUE CADA UNA DE LAS PROPIEDADES CUMPLA CON LOS REQUISITOS MINIMOS
                    if (EvaluacionDeCampos3())
                    {
                        //TODAS LAS PROPIEDADES CUMPLEN CON LOS REQUISITOS MINIMOS POR ULTIMO SE VERIFICA SI 
                        //SE ESTAN MODIFICANDO DATOS O REALIZANDO UN NUEVO REGISTRO

                        //flagLecturaEscritura = true => Acceso desde "PaginaConfiguracionAdmin.xaml.cs" o "PaginaConfiguracion.xaml.cs"
                        //flagLecturaEscritura = false => Acceso desde "PaginaRegistro.xaml.cs"
                        if (flagLecturaEscritura)
                        {
                            //----------------------------------------------------------------------------------------------------
                            //----------------------------------------------------------------------------------------------------
                            //SE LLAMA AL METODO DE REGISTRO DE USUARO CUANDO LA APLICACION SE ENCUENTRE FUNCIONANDO STAND ALONE
                            //ModifyUserStandAlone();
                            //----------------------------------------------------------------------------------------------------
                            //----------------------------------------------------------------------------------------------------
                            //SE LLAMA AL METODO DE REGISTRO DE USUARO CUANDO LA APLICACION SE ENCUENTRE FUNCIONANDO COMO CLIENTE HTTP
                            respuesta = await ModifyUserHttpClient();
                        }
                        else
                        {
                            //----------------------------------------------------------------------------------------------------
                            //----------------------------------------------------------------------------------------------------
                            //SE LLAMA AL METODO DE REGISTRO DE USUARO CUANDO LA APLICACION SE ENCUENTRE FUNCIONANDO STAND ALONE
                            //respuesta = RegisterUserStandAlone();
                            //----------------------------------------------------------------------------------------------------
                            //----------------------------------------------------------------------------------------------------
                            //SE LLAMA AL METODO DE REGISTRO DE USUARO CUANDO LA APLICACION SE ENCUENTRE FUNCIONANDO COMO CLIENTE HTTP
                            respuesta = await RegisterUserHttpClient();
                            //----------------------------------------------------------------------------------------------------
                            //----------------------------------------------------------------------------------------------------
                        }
                    }
                    else
                    {
                        //EN CASO DE QUE ALGUNA DE LAS CONDICIONES NO SE CUMPLA SE VERIFICA CUAL DE ELLAS ES
                        //Y SE PROCEDE A ENVIAR UN MENSAJE AL USUARIO.

                        if (Metodos.EspacioBlanco(Correo)) //=> true => El correo tiene espacios en blanco
                            respuesta = OnCompletedCorreoWhiteSpace;

                        if (Metodos.EspacioBlanco(Password)) //=> true => La contraseña tiene espacios en blanco
                            respuesta = OnCompletePasswordWhiteSpace;

                        if (Metodos.EspacioBlanco(Username)) //=> true => El nombre de usuario tiene espacios en blanco
                            respuesta = OnCompletedUsernameWhiteSpace;

                        if (Metodos.Caracteres(Password)) //=> true => La contraseña tiene caracteres prohibidos
                            respuesta = "La contraseña no puede contener los siguientes caracteres: " + App.ForbiddenCharacters;

                        if (Metodos.Caracteres(Username)) //=> true => El nombre de usuario tiene caracteres prohibidos
                            respuesta = "El nombre de usuario no puede contener los siguientes caracteres: " + App.ForbiddenCharacters;

                        if (Metodos.Caracteres(Nombres)) //=> true => El/los Nombre(s) tiene(n) caracteres prohibidos
                            respuesta = "El/los nombre(s) no puede(n) contener los siguientes caracteres: " + App.ForbiddenCharacters;

                        if (Metodos.Caracteres(Apellidos)) //=> true => El/los Apellido(s) tiene(n) caracteres prohibidos
                            respuesta = "El/los apellido(s) no puede(n) contener los siguientes caracteres: " + App.ForbiddenCharacters;
                    }
                }
                else
                {
                    respuesta = "Ningún campo debe quedar en blanco";
                }
            }
            else
            {
                respuesta = "No se ha modificado ninguno de los campos";
            }

            return respuesta;
        }

        //===================================================================
        //===================================================================
        //FUNCION DE ACTIVADA PARA DAR UN TIEMPO DE ESPERA
        public async Task SaveContact()
        {
            onsave = isbusy = true;
            await Task.Delay(500);
            onsave = isbusy = false;
        }

        public bool ContinuarRegistro(bool flag)
        {
            //SE EVALUAN QUE LOS CAMPOS CORRESPONDIENTES A LOS CAMPOS PERSONALES NO SE ENCUENTREN VACIOS O NULOS
            if (EvaluacionDeCampos2(flag))
                return true;
            else
                return false;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE EVALUA SI SE HA REALIZADO ALGUN CAMBIO SIGNIFICATIVO
        //SOBRE ALGUNO DE LOS ATRIBUTOS DEL OBJETO PERSONA Y USUARIO
        protected bool EvaluacionDeCampos1()
        {
            //SE VERIFICA DESDE QUE TIPO DE PAGINA FUE INVOCADA LA CLASE
            //SI FUE INVOCADA DESDE UNA PAGINA DE LECTURAESCRITURA SE TIENE
            //  flagLecturaEscritura => true => Pagina de LecturaEscritura
            //  flagLecturaEscritura => false => Pagina de Escritura

            if (flagLecturaEscritura) //=> true => Pagina de Lectura y Escritura (PaginaConfiguracion & PaginaConfiguracionAdmin)
            {
                //SE EVALUAN QUE TODOS LOS CAMPOS A MODIFICAR SEAN SIMILARES A LOS VALORES REGISTRADOS 
                if (Persona.Nombres == Nombres &&                   //VERIFICA SI LOS NOMBRES SON IGUALES
                    Persona.Apellidos == Apellidos &&               //VERIFICA SI LOS APELLIDOS SON IGUALES
                    Persona.FechaNacimiento == FechaNacimiento &&   //VERIFICA SI LAS FECHAS DE NACIMIENTO SON IGUALES
                    Persona.Telefono.ToString() == Telefono &&      //VERIFICA SI LOS TELEFONOS SON IGUALES
                    Persona.Correo == Correo &&                     //VERIFICA SI LOS CORREOS SON IGUALES
                    Usuario.Username == Username &&                 //VERIFICA SI LOS NOMBRES DE USUARIOS SON IGUALES
                    Usuario.Password == Password &&                 //VERFICIA SI LAS CONTRASEÑAS SON IGUALES
                    Usuario.NivelUsuario == NivelUsuario)   //VERIFICA SI EL NIVEL DE USUARIO ES IGUAL(?)
                    return false;
                else
                    return true;
            }
            else // false => Pagina de Escritura (PaginaRegistro)
                return true;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE VERIFICA QUE NINGUN CAMPO SE ENCUENTRE NULO O VACIO

        protected bool EvaluacionDeCampos2(bool modo)
        {
            //SE CREA E INICIALIZA (false) LA VARIABLE QUE RETORNARÁ EL RESULTADO DE LA EVALUACION DE CAMPOS
            bool resultado = false;

            //EL PARAMETRO RECIBIDO DEL TIPO BOOL "modo" INDICA SI ESTA FUNCION ES LLAMADA PARA CONTINUAR LLENANDO
            //INFORMACION DE REGISTRO (INFORMACION DE USUARIO) O ES LLAMADA PARA REALIZAR EL REGISTRO COMPLETO DE USUARIO
            //modo = true => "Continuar" => CONTINUAR LLENANDO INFORMACION DE REGISTRO
            //modo = false => "Registrar" => LLAMADO PARA REGISTRAR EN LA BASE DE DATOS EL NUEVO USUARIO
            if (modo)
            {
                //SE EVALUAN QUE TODOS LOS CAMPOS DE "Información Personal" CUMPLAN CON LAS SIGUIENTES CONDICIONES
                if (!string.IsNullOrEmpty(Nombres) &&   //EL NOMBRE NO PUEDE SER VACIO O NULO
                !string.IsNullOrEmpty(Apellidos) &&     //EL APELLIDO NO PUEDE SER VACIO O NULO
                FechaNacimiento != DateTime.Now &&      //NO SE PUEDE REGISTRAR LA FECHA ACTUAL
                FechaNacimiento < DateTime.Now &&       //LA FECHA NO PUEDE SER MAYOR A LA FECHA ACTUAL
                !string.IsNullOrEmpty(Telefono) &&      //EL TELEFONO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Correo) &&        //EL CORREO NO PUEDE SER VACIO O NULLO
                !Error)                                 //EL ATRIBUTO ERROR NO PUEDE SER VERDADERO
                {
                    //SI TODOS LOS CAMPOS CUMPLEN CON LAS CONDICIONES MINIMAS 
                    resultado = true;
                }
            }
            else
            {
                //SE EVALUAN QUE TODOS LOS CAMPOS CUMPLAN CON LAS SIGUIENTES CONDICIONES
                if (!string.IsNullOrEmpty(Nombres) &&  //EL NOMBRE NO PUEDE SER VACIO O NULO
                !string.IsNullOrEmpty(Apellidos) &&  //EL APELLIDO NO PUEDE SER VACIO O NULO
                FechaNacimiento != DateTime.Now &&  //NO SE PUEDE REGISTRAR LA FECHA ACTUAL
                FechaNacimiento < DateTime.Now &&  //LA FECHA NO PUEDE SER MAYOR A LA FECHA ACTUAL
                !string.IsNullOrEmpty(Telefono) &&  //EL TELEFONO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Correo) &&  //EL CORREO NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Username) &&  //EL USERNAME NO PUEDE SER VACIO O NULLO
                !string.IsNullOrEmpty(Password) &&  //EL PASSWORD NO PUEDE SER VACIO O NULLO
                !Error)                                 //EL ATRIBUTO ERROR NO PUEDE SER VERDADERO
                {
                    //SI TODOS LOS CAMPOS CUMPLEN CON LAS CONDICIONES MINIMAS
                    resultado = true;
                }
            }

            //SE RETORNA EL ULTIMO VALOR OBTENIDO POR LA VARIABLE "resultado"
            return resultado;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION QUE VERIFICA QUE TODOS LOS CAMPOS CUMPLAN CON LAS CONDICIONES MINIMAS
        protected bool EvaluacionDeCampos3()
        {
            //SE EVALUAN QUE TODOS LOS CAMPOS CUMPLAN CON LAS CONDICIONES MINIMAS ESTABLECIDAS PARA REGISTRAR
            //UN USUARIO O PARA MODIFICAR LA INFORMACION DE UN USUARIO
            if (!Metodos.EspacioBlanco(Correo) &&        //CORREO NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.EspacioBlanco(Password) &&        //PASSWORD NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(Password) &&        //PASSWORD NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(Username) &&        //USERNAME NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.EspacioBlanco(Username) &&        //USERNAME NO PUEDE CONTENER ESPACIOS EN BLANCO
                !Metodos.Caracteres(Nombres) &&        //NOMBRES NO PUEDE CONTENER CARACTERES ESPECIFICOS
                !Metodos.Caracteres(Apellidos))               //APELLIDOS NO PUEDE CONTENER CARACTERES ESPECIFICOS
                return true;
            else
                return false;
        }

        //===================================================================================
        //===================================================================================
        //FUNCION LLAMADA CADA QUE SE EJECUTA UN CAMBIO EN ALGUNA DE LAS PROPIEDADES DE LA
        //PAGINA (PaginaConfiguracion y PaginaConfiguracionAdmin)
        protected void ConsoleWriteline(string WHO, string ValorPrevio, string ValorActual)
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + WHO);
            Console.WriteLine("\n\nValor previo: " + ValorPrevio + "\nValor actual: " + ValorActual);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //===================================================================================
        //===================================================================================
        //FUNCION LLAMADA PARA IMPRIMIR POR CONSOLA EL VALOR INGRESADO DE LAS PROPIEDADES DE LA
        //PAGINA (PaginaRegistro)
        protected void ConsoleWriteline(string WHO, string ValorActual)
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("=============================================");
            Console.WriteLine("\nPROPIEDAD: " + WHO);
            Console.WriteLine("\n\nValor actual: " + ValorActual);
            Console.WriteLine("=============================================");
            Console.WriteLine("=============================================\n\n");
        }

        //===================================================================================
        //===================================================================================
        //FUNCION LLAMADA PARA IMPRIMIR POR CONSOLA TODOS LOS VALORES DE LAS PROPIEDADES DE LA
        //PAGINA (PaginaRegistro)
        protected void NotificacionRegistro()
        {
            Console.WriteLine("\n\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("\nREGISTRO EXITOSO: \n");
            Console.WriteLine("\nNombres: " + Nombres);
            Console.WriteLine("\nApellidos: " + Apellidos);
            Console.WriteLine("\nCedula: " + Cedula);
            Console.WriteLine("\nFecha de Nacimiento: " + FechaNacimiento);
            Console.WriteLine("\nTelefono: " + Telefono);
            Console.WriteLine("\nCorreo: " + Correo);
            Console.WriteLine("\n\nUsername: " + Username);
            Console.WriteLine("\nPassword: " + Password);
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n\n");
        }

        //===================================================================================
        //===================================================================================
        //FUNCION PARA RETORNAR UN MENSAJE POR CONSOLA
        protected void MensajeConsole(string mensaje)
        {
            Console.WriteLine("\n\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("Mensaje: " + mensaje);
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n\n");
        }

        //==================================================================================
        //==================================================================================
        //FUNCION PARA GENERAR UN MENSAJE NO INTERACTIVO POR PANTALLA
        public void MensajePantalla(string mensaje)
        {
            Toast.MakeText(Android.App.Application.Context, mensaje, ToastLength.Short).Show();
        }

        //==================================================================================
        //==================================================================================
        //METODOS PARA EL REGISTRO Y MODIFICACION DE USUARIOS CUANDO LA APLICACION
        //SE ENCUENTRA FUNCIONANDO COMO CLIENTE HTTP
        protected async Task<string> RegisterUserHttpClient()
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
            string url = App.BaseUrl + "/registro";
            //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
            var current = Connectivity.NetworkAccess;
            //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
            if (current == NetworkAccess.Internet)
            {
                //EL EQUIPO SE ENCUENTRA CONECTADO A INTERNET
                //SE INICIA EL CICLO TRY...CATCH
                try
                {
                    //INICIAMOS EL SEGMENTO DEL CODIGO EN EL CUAL REALIZAREMOS EL CONSUMO DE SERVICIOS WEB MEDIANTE
                    //LA INICIALIZACION Y CREACION DE UNA VARIABLE QUE FUNCIONARA COMO CLIENTE EN LAS SOLICITUDES 
                    //Y RESPUESTAS ENVIADAS Y RECIBIDAS POR EL SERVIDOR (WEB API) 
                    //----------------------------------------------------------------------------------------------
                    //NOTA: CUANDO SE REALIZA LA CREACION E INICIALIZACION DE LA VARIABLE DEL TIPO HttpClient SE
                    //HACE UN LLAMADO A UN METODO ALOJADO EN LA CLASE "App" Y QUE ES ENVIADO COMO PARAMETRO DEL 
                    //TIPO HttpClientHandler => 
                    //----------------------------------------------------------------------------------------------
                    using (HttpClient client = new HttpClient(App.GetInsecureHandler()))
                    {
                        //SE DA SET AL TIEMPO MAXIMO DE ESPERA PARA RECIBIR UNA RESPUESTA DEL SERVIDOR
                        client.Timeout = TimeSpan.FromSeconds(App.TimeInSeconds);

                        //SE CREA EL OBJETO MODELO DEL TIPO "RequestRegistroUsuario"
                        var model = new RequestRegistroUsuario()
                        {
                            NewUser = new InformacionGeneral()
                            {
                                Persona = new Personas().NewPersona(FechaCreacion, Metodos.Mayuscula(Nombres), Metodos.Mayuscula(Apellidos),
                                                                            Cedula, NumeroFicha, FechaNacimiento, Telefono, Correo.ToLower()),
                                Usuario = new Usuarios().NewUsuario(Username.ToLower(), Password, Cedula, FechaCreacion, NivelUsuario),
                            },
                            UserId = UserId,
                        };

                        //SE REALIZA LA CONVERSION A OBJETO JSON
                        var json = JsonConvert.SerializeObject(model);
                        //SE AÑADE EL OBJETO JSON RECIEN CREADO COMO CONTENIDO BODY DEL NUEVO REQUEST
                        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //SE REALIZA LA SOLICITUD HTTP
                        HttpResponseMessage response = await client.PostAsync(url, httpContent);
                        //SE RETORNA EL MENSAJE OBTENIDO POR 
                        return await Task.FromResult(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
                    }
                }
                //DE OCURRIR UNA EXCEPCION DENTRO DEL CICLO TRY...CATCH SE PROCEDE A EJECUTAR LAS LINEAS DE CODIGO
                //CONTENIDAS DENTRO DE LA SECCION CATCH. AQUI SE EVALUARAN LAS EXCEPCIONES MAS COMUNES OBTENIDAS DURANTE
                //LAS PRUEBAS DE CONEXION CON EL SERVICIO WEB API
                catch (Exception ex) when (ex is HttpRequestException ||
                                           ex is Javax.Net.Ssl.SSLException ||
                                           ex is Javax.Net.Ssl.SSLHandshakeException ||
                                           ex is System.Threading.Tasks.TaskCanceledException)
                {
                    return "Problemas de conexion con el servidor";
                }
            }

            return string.Empty;
        }

        protected async Task<string> ModifyUserHttpClient()
        {
            //SE EVALUA SI EL ACCESO A LA PAGINA "Configuracion" FUE LLAMADO 
            //POR EL USUARIO ADMINISTRATOR O NO
            if (UserId == 0)
            {
                //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
                string url = App.BaseUrl + $"/configuracion/administrator/{Persona.Cedula}";

                //SE CREA EL OBJETO MODELO DEL TIPO "ConfiguracionA"
                var model = new ConfiguracionA
                {
                    Cedula = Convert.ToDouble(Cedula),
                    Nombres = Nombres, 
                    Apellidos = Apellidos,
                    FechaNacimiento = FechaNacimiento,
                    Telefono = Convert.ToDouble(Telefono),
                    Correo = Correo,
                    Username = Username,
                    Userpassword = Password,
                    NivelUsuario = NivelUsuario,
                };

                //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
                var current = Connectivity.NetworkAccess;
                //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
                if (current == NetworkAccess.Internet)
                {
                    //EL EQUIPO SE ENCUENTRA CONECTADO A INTERNET
                    //SE INICIA EL CICLO TRY...CATCH
                    try
                    {
                        //INICIAMOS EL SEGMENTO DEL CODIGO EN EL CUAL REALIZAREMOS EL CONSUMO DE SERVICIOS WEB MEDIANTE
                        //LA INICIALIZACION Y CREACION DE UNA VARIABLE QUE FUNCIONARA COMO CLIENTE EN LAS SOLICITUDES 
                        //Y RESPUESTAS ENVIADAS Y RECIBIDAS POR EL SERVIDOR (WEB API) 
                        //----------------------------------------------------------------------------------------------
                        //NOTA: CUANDO SE REALIZA LA CREACION E INICIALIZACION DE LA VARIABLE DEL TIPO HttpClient SE
                        //HACE UN LLAMADO A UN METODO ALOJADO EN LA CLASE "App" Y QUE ES ENVIADO COMO PARAMETRO DEL 
                        //TIPO HttpClientHandler => 
                        //----------------------------------------------------------------------------------------------
                        using (HttpClient client = new HttpClient(App.GetInsecureHandler()))
                        {
                            //SE DA SET AL TIEMPO MAXIMO DE ESPERA PARA RECIBIR UNA RESPUESTA DEL SERVIDOR
                            client.Timeout = TimeSpan.FromSeconds(App.TimeInSeconds);
                            //SE REALIZA LA CONVERSION A OBJETO JSON
                            var json = JsonConvert.SerializeObject(model);
                            //SE AÑADE EL OBJETO JSON RECIEN CREADO COMO CONTENIDO BODY DEL NUEVO REQUEST
                            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            //SE REALIZA LA SOLICITUD HTTP
                            HttpResponseMessage response = await client.PutAsync(url, httpContent);
                            //SE RETORNA EL MENSAJE OBTENIDO POR 
                            return await Task.FromResult(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
                        }
                    }
                    //DE OCURRIR UNA EXCEPCION DENTRO DEL CICLO TRY...CATCH SE PROCEDE A EJECUTAR LAS LINEAS DE CODIGO
                    //CONTENIDAS DENTRO DE LA SECCION CATCH. AQUI SE EVALUARAN LAS EXCEPCIONES MAS COMUNES OBTENIDAS DURANTE
                    //LAS PRUEBAS DE CONEXION CON EL SERVICIO WEB API
                    catch (Exception ex) when (ex is HttpRequestException ||
                                               ex is Javax.Net.Ssl.SSLException ||
                                               ex is Javax.Net.Ssl.SSLHandshakeException ||
                                               ex is System.Threading.Tasks.TaskCanceledException)
                    {
                        return "Problemas de conexion con el servidor";
                    }
                }
            }
            else if(UserId != 0 )
            {
                //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
                string url = App.BaseUrl + $"/configuracion/usuario/{Persona.Cedula}";

                //SE CREA EL OBJETO MODELO DEL TIPO "ConfiguracionU"
                var model = new ConfiguracionU
                {
                    Cedula = Convert.ToDouble(Cedula),
                    Telefono = Convert.ToDouble(Telefono),
                    Correo = Correo,
                    Userpassword = Password,
                };

                //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
                var current = Connectivity.NetworkAccess;
                //SE VERIFICA SI EL DISPOSITIVO SE ENCUENTRA CONECTADO A INTERNET
                if (current == NetworkAccess.Internet)
                {
                    //EL EQUIPO SE ENCUENTRA CONECTADO A INTERNET
                    //SE INICIA EL CICLO TRY...CATCH
                    try
                    {
                        //INICIAMOS EL SEGMENTO DEL CODIGO EN EL CUAL REALIZAREMOS EL CONSUMO DE SERVICIOS WEB MEDIANTE
                        //LA INICIALIZACION Y CREACION DE UNA VARIABLE QUE FUNCIONARA COMO CLIENTE EN LAS SOLICITUDES 
                        //Y RESPUESTAS ENVIADAS Y RECIBIDAS POR EL SERVIDOR (WEB API) 
                        //----------------------------------------------------------------------------------------------
                        //NOTA: CUANDO SE REALIZA LA CREACION E INICIALIZACION DE LA VARIABLE DEL TIPO HttpClient SE
                        //HACE UN LLAMADO A UN METODO ALOJADO EN LA CLASE "App" Y QUE ES ENVIADO COMO PARAMETRO DEL 
                        //TIPO HttpClientHandler => 
                        //----------------------------------------------------------------------------------------------
                        using (HttpClient client = new HttpClient(App.GetInsecureHandler()))
                        {
                            //SE DA SET AL TIEMPO MAXIMO DE ESPERA PARA RECIBIR UNA RESPUESTA DEL SERVIDOR
                            client.Timeout = TimeSpan.FromSeconds(App.TimeInSeconds);
                            //SE REALIZA LA CONVERSION A OBJETO JSON
                            var json = JsonConvert.SerializeObject(model);
                            //SE AÑADE EL OBJETO JSON RECIEN CREADO COMO CONTENIDO BODY DEL NUEVO REQUEST
                            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            //SE HACE LA CONFIGURACION DE LOS HEADERS DEL REQUEST
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            //SE REALIZA LA SOLICITUD HTTP
                            HttpResponseMessage response = await client.PutAsync(url, httpContent);
                            //SE RETORNA EL MENSAJE OBTENIDO POR 
                            return await Task.FromResult(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
                        }
                    }
                    //DE OCURRIR UNA EXCEPCION DENTRO DEL CICLO TRY...CATCH SE PROCEDE A EJECUTAR LAS LINEAS DE CODIGO
                    //CONTENIDAS DENTRO DE LA SECCION CATCH. AQUI SE EVALUARAN LAS EXCEPCIONES MAS COMUNES OBTENIDAS DURANTE
                    //LAS PRUEBAS DE CONEXION CON EL SERVICIO WEB API
                    catch (Exception ex) when (ex is HttpRequestException ||
                                               ex is Javax.Net.Ssl.SSLException ||
                                               ex is Javax.Net.Ssl.SSLHandshakeException ||
                                               ex is System.Threading.Tasks.TaskCanceledException)
                    {
                        return "Problemas de conexion con el servidor";
                    }
                }
            }

            return string.Empty;
        }
    }
}