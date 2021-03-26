using Android.Widget;
using MttoApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MttoApp.ViewModel
{
    internal class QueryAdminViewModel
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES Y OBJETOS LOCALES 
        private Personas Persona; //=> OBJETO QUE RECIBIRA LA INFORMACION PERSONAL COMPLETA DEL USUARIO QUE SE ENCUENTRA NAVEGANDO
        private Usuarios Usuario; //=> OBJETO QUE RECIBIRA LA INFORMACION DE USUARIO COMPLETA DEL USUARIO QUE SE ENCUENTRA NAVEGANDO
        private string errormessage; 
        public int OpcionSeleccionada;
        //---------------------------------------------------------------------------------------------------------
        //PROPIEDADES DE LA CLASE
        //PROPIEDAD QUE RECIBIRA LA LISTA DE OPCIONES DE METODOS DE BUSQUEDA DE USUARIOS
        public List<QueryAdminModel> InfoConfig { get { return new QueryAdminModel().GetConfiguracion(); } }
        //---------------------------------------------------------------------------------------------------------
        //TEXTOS
        public string TituloPagina { get { return "Busqueda de Usuario"; } }
        public string BusquedaPH { get { return "Busqueda:"; } }
        public string EntryDatosPH { get { return "Ingrese el dato a buscar"; } }
        public string ColumnaCedula { get { return "Cedula (ID)"; } }
        public string ColumnaNombres { get { return "Nombre(s)"; } }
        public string ColumnaApellidos { get { return "Apellido(s)"; } }

        //---------------------------------------------------------------------------------------------------------
        //COLOR DE FONTO Y DE BOTONES
        public string BackGroundColor { get { return App.BackGroundColor; } }
        public string ButtonColor { get { return App.ButtonColor; } }
        //---------------------------------------------------------------------------------------------------------
        //TAMAÑO DE LAS LETRAS
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
        //---------------------------------------------------------------------------------------------------------
        public string ErrorMessage { get { return errormessage; } }
        //TEXTO USADO EN EL MENSAJE DEL TIPO POP-UP INFORMATIVO AL EJECUTAR EL METODO "OnBuscar" Y NO OBTENER RESULTADOS
        public string OnBuscarMethodMessage
        {
            //NOTA: PUESTO QUE EXISTEN VARIOS METODOS DE BUSQUEDA: Id (cedula), ficha, nombres, apellidos y username
            //SE DECIDIO CREA UNA PROPIEDAD QUE CONTIVERA EL TEXTO INFORMATIVO CUANDO UNA DE LAS CONDICIONES DE
            //BUSQUEDA NO SE CUMPLEN PARA CADA UNA DE LAS OPCIONES DE BUSQUEDA (SIENDO ESTE EL CASO EL FORMATO DEL 
            //TEXTO INGRESADO COMO PARAMETRO DE CONSULTA)
            get
            {
                //SE CREA E INICIALIZA LA VARIABLE QUE RETORNARA EL TEXTO CORRESPONDIENTE
                string message = string.Empty;

                switch(OpcionSeleccionada)
                {
                    //CONSULTA POR Id (NUMERO DE CEDULA)
                    case 0:
                        message = "El número de ID (cedula) no puede contener espacios en blanco";
                        break;
                    //CONSULTA POR NUMERO DE FICHA
                    case 1:
                        message = "El número de ficha no puede contener espacios en blanco";
                        break;
                    //CONSULTA POR NOMBRE(S) DE USUARIO
                    case 2:
                        message = "Verifique la cantidad de espacios en blanco ingresados";
                        break;
                    //CONSULTA POR APELLIDO(S) DE USUARIO
                    case 3:
                        message = "Verifique la cantidad de espacios en blanco usados";
                        break;
                    //CONSULTA POR NOMBRE DE USUARIO
                    case 4:
                        message = "El nombre de usuario no puede contener espacios en blanco";
                        break;
                }

                return message;
            }
        }
        //TEXTO USADO EN EL MENSAJE DEL TIPO POP-UP AL EJECUTAR EL METODO "OnItemSelected" y
        public string OnItemSelectedMethodMessage { get { return "¿Desea modificar la información del usuario seleccionado?"; } }
        //TEXTOS UTILIZADOS PARA REPRESENTAR LA AFIRMACION O NEGACION DEL USUARIO ANTE UNA PETICION
        //NOTA: DICHOS TEXTOS SON USADOS EN LOS DISPLAYALERT EN LAS SECCIONES DE AFIRMACION (SI) Y NEGACION (NO) DEL MENSAJE APARENTE
        public string AffirmativeText { get { return App.AffirmativeText; } } //=> SI
        public string NegativeText { get { return App.NegativeText; } } //=> NO
        //TEXTO USADO EN EL BOTON DEL MENSAJE DEL TIPO POP-UP INFORMATIVO
        public string OkText { get { return App.OkText; } } //=> Entendido
        //=========================================================================================================
        //-------------------------------------------------METODOS-------------------------------------------------
        //CONSTRUCTOR DE LA CLASE 
        public QueryAdminViewModel(Personas persona, Usuarios usuario)
        {
            //SE ASIGNA LA INFORMACION DEL USUARIO QUE SE ENCUENTRA NAVEGANDO
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);
            //SE INICIALIZA LA VARIABLE "errormessage"
            errormessage = string.Empty;
        }

        //=========================================================================================================
        //=========================================================================================================
        //FUNCION PARA GENERAR UN MENSAJE NO INTERACTIVO POR PANTALLA
        public void MensajePantalla(string mensaje)
        {
            Toast.MakeText(Android.App.Application.Context, mensaje, ToastLength.Short).Show();
        }

        //=========================================================================================================
        //=========================================================================================================
        //BUSQUEDA DE USUARIOS (STAND ALONE)
        private List<Personas> ListaPersonasStandAlone(int SeleccionBusqueda, string Dato)
        {
            List<Personas> QueryPersonas = new List<Personas>();
            List<double> ListaUsuarioID = new List<double>();

            //SE APERTURA LA BASE DE DATOS
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.FileName))
            {
                //LISTA DE PERSONAS Y USUARIOS REGISTRADOS
                List<Personas> Personas = connection.Table<Personas>().ToList();
                List<Usuarios> Usuarios = connection.Table<Usuarios>().ToList();

                //DEPENDIENDO DEL VALOR QUE POSEA SeleccionBusqueda:
                //(EN ESTE CASO SOLO PUEDE TENER 4, QUE SON: 0-ID, 1-NUMERO DE FICHA, 2-NOMBRES, 3-APELLIDOS, 4-USERNAME)
                switch (SeleccionBusqueda)
                {
                    //CASO BUSQUEDA POR CEDULA
                    case 0:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NUMERO DE CEDULA (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                string w = persona.Cedula.ToString().Substring(0, Dato.Length);

                                if (Dato == w)
                                    QueryPersonas.Add(persona);
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR NUMERO DE FICHA
                    case 1:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NUMERO DE FICHA (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.NumeroFicha != 0)
                            {
                                if (Dato == persona.NumeroFicha.ToString().Substring(0, Dato.Length))
                                    QueryPersonas.Add(persona);
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR NOMBRE
                    case 2:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NOMBRE PERSONAL (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                if (Dato.ToLower() == persona.Nombres.Substring(0, Dato.Length).ToLower())
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR APELLIDO
                    case 3:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR APELLIDO PERSONAL (STAND ALONE)
                        foreach (Personas persona in Personas)
                        {
                            if (persona.Cedula != 0)
                            {
                                if (Dato.ToLower() == persona.Apellidos.Substring(0, Dato.Length).ToLower())
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                    //CASO BUSQUEDA POR USERNAME
                    case 4:
                        //=======================================================================================
                        //=======================================================================================
                        //BUSQUEDA DE USUARIOS POR NOMBRE DE USUARIO (STAND ALONE)
                        //SE GUARDAN LOS ID DE LOS USUARIOS QUE CUMPLEN CON EL DATO SUMINISTRADO
                        foreach (Usuarios usuario in Usuarios)
                        {
                            if (usuario.Cedula != 0)
                            {
                                if (Dato.ToLower() == usuario.Username.Substring(0, Dato.Length).ToLower())
                                {
                                    ListaUsuarioID.Add(usuario.Cedula);
                                }
                            }
                        }
                        //SE AÑADEN LAS PERSONAS QUE CUMPLAN CON EL ID DE LA LISTA PREVIAMENTE LLENADA
                        foreach (Personas persona in Personas)
                        {
                            foreach (int ID in ListaUsuarioID)
                            {
                                if (persona.Cedula == ID)
                                {
                                    QueryPersonas.Add(persona);
                                }
                            }
                        }
                        break;
                        //=======================================================================================
                        //=======================================================================================
                }
                //SE CIERRA LA BASE DE DATOS
                connection.Close();
            }
            //SE RETORNA LA LISTA CON LAS PERSONAS QUE CUMPLAN CON LA INFORMACION SOLICITADA
            return QueryPersonas;
        }

        //=========================================================================================================
        //=========================================================================================================
        //BUSQUEDA DE USUARIOS (CONSUMO DE SERVICIOS WEB)
        public async Task<List<ResponseQueryAdmin>> ListaPersonasHttpClient(int SeleccionBusqueda, string Dato)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE CONTENDRA EL URL
            string url = App.BaseUrl + "/queryadmin";

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA LISTA DE USUARIOS ENCONTRADOS
            List<ResponseQueryAdmin> lista = null;

            //SE CREA E INICIALIZA LA VARIABLE QUE SERVIRA DE MODELO PARA EL OBJETO JSON ENVIADO EN LA SOLICITUD
            var model = new RequestQueryAdmin() { Parametro = Dato, UserId = Persona.Cedula, };

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LA SOLICITUD HTTP
            HttpResponseMessage response = new HttpResponseMessage();

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
                        //--------------------------------------------------------------------------------------------------------
                        //DEPENDIENDO DEL VALOR QUE POSEA SeleccionBusqueda:
                        //(EN ESTE CASO SOLO PUEDE TENER 4, QUE SON: 0-ID, 1-NUMERO DE FICHA, 2-NOMBRES, 3-APELLIDOS, 4-USERNAME)
                        //--------------------------------------------------------------------------------------------------------
                        switch (SeleccionBusqueda)
                        {
                            //CASO BUSQUEDA POR CEDULA
                            case 0:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                url = url + "/cedula";
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.PostAsync(url, httpContent);
                                break;
                            //CASO BUSQUEDA POR NUMERO DE FICHA
                            case 1:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                url = url + "/ficha";
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.PostAsync(url, httpContent);
                                break;
                            //CASO BUSQUEDA POR NOMBRE
                            case 2:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                url = url + "/nombres";
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.PostAsync(url, httpContent);
                                break;
                            //CASO BUSQUEDA POR APELLIDO
                            case 3:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                url = url + "/apellidos";
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.PostAsync(url, httpContent);
                                break;
                            //CASO BUSQUEDA POR USERNAME
                            case 4:
                                //SE TERMINA DE ESPECIFICAR EL URL AL CUAL SE DEBE REALIZAR LA SOLICITUD CUANDO ES BUSQUEDA POR CEDULA
                                url = url + "/username";
                                //SE HACE EL ENVIO DE LA SOLICITUD
                                response = await client.PostAsync(url, httpContent);
                                break;
                        }
                    }
                }
                //SECCION DE CODIGO QUE SE EJECUTA CUANDO EL COMANDO DE SOLICITUD HTTP FALLE
                catch (Exception ex) when (ex is HttpRequestException ||
                                           ex is Javax.Net.Ssl.SSLException ||
                                           ex is Javax.Net.Ssl.SSLHandshakeException ||
                                           ex is System.Threading.Tasks.TaskCanceledException)
                {
                    //SE RETORNA LA LISTA NULA
                    return await Task.FromResult(lista);
                }
            }

            //SE VERIFICA SI EL CODIGO DE ESTATUS DEL CONSUMO DEL SERVICIO WEB ES POSITIVO
            if (response.IsSuccessStatusCode)
                //DE SER POSITIVO, SE DESERILIZA EL OBJETO JSON (List<>)CONTENIDO EN LA RESPUESTA RECIBIDA LUEGO DEL CONSUMO DEL SERVICIO WEB
                lista = JsonConvert.DeserializeObject<List<ResponseQueryAdmin>>(await response.Content.ReadAsStringAsync());
            else
                //DE SER NEGATIVO, SE DESERIALIZA EL OBJETO JSON (STRING)
                errormessage = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            //SE RETORNA LA LISTA DE USUARIOS ENCONTRADOS
            return await Task.FromResult(lista);
        }

        //=========================================================================================================
        //=========================================================================================================
        //SELECCION DE USUARIO (CONSUMO DE SERVICIOS WEB)
        public async Task<InformacionGeneral> OnUserSelected(ResponseQueryAdmin userselected)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE CONTENDRA EL URL
            string url = App.BaseUrl + "/queryadmin/onuserselected";

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LA SOLICITUD HTTP
            HttpResponseMessage response = new HttpResponseMessage();

            //SE CREA E INICIALIZA LA VARIABLE QUE VERIFICARA EL ESTADO DE CONEXION A INTERNET
            var current = Connectivity.NetworkAccess;

            //SE CREA LA CLASE MODELO QUE SERA CONVERTIDA A OBJETO JSON Y ENVIADA DENTRO DE LA SOLICITUD HTTP
            var model = new UserSelectedRequest() { UserIdRequested = Usuario.Cedula, UserIdSelected = userselected.Cedula };

            //SE CREA E INICIALIZA LA VARIABLE QUE RECIBIRA EL OBJETO JSON RETORNADO POR LA SOLICITUD HTTP
            InformacionGeneral informacion = null;

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
                        //SE REALIZA LA SOLICITUD HTTP POST
                        response = await client.PostAsync(url, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            //SE DESERIABLIZA EL OBJETO JSON CONTENIDO EN EL OBJETO "response" (<InformacionGeneral>)
                            informacion = JsonConvert.DeserializeObject<InformacionGeneral>(await response.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            //SE DESEREALIZA EL OBJETO JSON CONTENIDO EN EL OBJETO "response" (<string>)
                            errormessage = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException ||
                                           ex is Javax.Net.Ssl.SSLException ||
                                           ex is Javax.Net.Ssl.SSLHandshakeException ||
                                           ex is System.Threading.Tasks.TaskCanceledException)
                {
                    return await Task.FromResult(informacion);
                }
            }

            return await Task.FromResult(informacion);
        }
    }
}