using System;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Threading.Tasks;
using MttoApp.Model;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Android.Widget;
using System.Text;

namespace MttoApp.ViewModel
{
    public class PaginaPrincipalViewModel : INotifyPropertyChanging
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES LOCALES
        protected string username = string.Empty,
                         password = string.Empty;

        protected string result;

        //=========================================================================================================
        //----------------------------------------PROPIEDADES DE LA CLASE------------------------------------------
        //PROPIEDAD ASIGNADA A REPRESENTAR EL NOMBRE DE USUARIO
        public string Username
        {
            get { return username; }
            set
            {
                //CADA QUE SE MODIFIQUE LA PROPIEDAD "Username" SE EJECUTARA ESTE CODIGO
                //LLAMADO PARA DETECTAR CAMBIO DE LA PROPIEDAD
                OnPropertyChanged();
                //ASIGNACION DEL DATO QUE POSEA LA PROPIEDAD A LA VARIABLE
                username = value;
                //--------------------------------------------------------------
                //NOTA:COMANDO UTIL CUANDO EL TELEFONO SE ENCUENTRE DEBUGGIANDO
                //IMPRESION POR CONSOLA DEL VALOR DE LA VARIABLE "username"
                ConsoleWriteline("USERNAME/NOMBRE DE USUARIO", username);
                //--------------------------------------------------------------
            }
        }
        //PROPIEDAD ASIGNADA A REPRESENTAR LA CONTRASEÑA DEL USUARIO
        public string Password
        {
            get { return password; }
            set
            {
                //CADA QUE SE MODIFIQUE LA PROPIEDAD "Password" SE EJECUTARA ESTE CODIGO
                //LLAMADO PARA DETECTAR CAMBIO DE LA PROPIEDAD
                OnPropertyChanged();
                //ASIGNACION DEL DATO QUE POSEA LA PROPIEDAD A LA VARIABLE
                password = value;
                //--------------------------------------------------------------
                //NOTA:COMANDO UTIL CUANDO EL TELEFONO SE ENCUENTRE DEBUGGIANDO
                //IMPRESION POR CONSOLA DEL VALOR DE LA VARIABLE "username"
                ConsoleWriteline("PASSWORD/CONTRASEÑA", password);
                //--------------------------------------------------------------
            }
        }
        //PROPIEDAD ASIGNADA A REPRESENTAR LA RESPUESTA OBTENIDA POR LA SOLICITUD HTTP
        public string Result
        {
            get { return result; }
        }

        //=========================================================================================================
        //----------------------------------------PROPIEDADES DE LA PAGINA-----------------------------------------
        //=========================================================================================================
        //COLORES
        public string BackGroundColor { get { return App.BackGroundColor; } } //=> COLOR DE FONDO DE LA PANTALLA PRINCIPAL
        public string HeaderBackGroundColor { get { return "#E53835"; } } //=> COLOR DEL TEXTO DEL TITULO DE LA PANTALLA
        public string EntryBackGroundColor { get { return "#424242"; } } //=> COLOR DEL TEXTO DE LAS ENTRADAS DE NOMBRE DE USUARIO Y CONTRASEÑA
        public string IngresoDatosBackGroundColor { get { return "#ff6e60"; } } //=> COLOR DEL BOTON DE INGRESO

        //--------------------------------------------------------------------------------------------------------
        //TEXTOS
        //TITULO DE LA APLICACION QUE SE VERA EN LA PARTE SUPERIOR DE LA PANTALLA DE INICIO (PantallaPrincipal)
        public string HeaderText { get { return "MTTO App"; } }
        //TEXTO PLACEHOLDER DEL ENTRY USADO PARA INGRESAR EL NOMBRE DE USUARIO
        public string UsernamePH { get { return "Nombre de usuario"; } }
        //TEXTO PLACEHOLDER DEL ENTRY USADO PARA INGRESAR EL NOMBRE DE USUARIO
        public string PasswordPH { get { return "Contraseña"; } }
        //TEXTO DEL BOTON USADO PARA INGRESAR 
        public string ButtonPH { get { return "INGRESAR"; } }
        //MENSAJE EMERGENTE CUANDO LA APLICACION DETECTE QUE NO HAY ACCESO A INTERNET 

        public string SignText { get { return "Digitalización Industrial, 2020"; } }
        public string NetWorkConnectivityChangeMessage 
        { 
            get { return App.NoNetworkAccessMessage; } 
        }
        //MENSAJE EMERGENTE QUE INFORMARÁ AL USUARIO CUANDO FALTEN EL NOMBRE DE USUARIO Y/O
        //LA CONTRASEÑA PARA INGRESAR
        public string UsernamePasswordEmptyMessage
        {
            get { return "Debe ingresar el nombre de usuario y la contraseña para poder ingresar"; }
        }
        //TEXTO USADO EN EL BOTON DEL MENSAJE DEL TIPO POP-UP INFORMATIVO
        public string OkText { get { return App.OkText; } }

        //--------------------------------------------------------------------------------------------------------
        //TAMAÑO DEL TEXTO
        //TEXTO DE ETIQUETAS Y TEXTOS INFORMATIVOS
        public int LabelFontSize { get { return App.LabelFontSize; } }
        //TEXTO DE ENTRADAS (ENTRYs)
        public int EntryFontSize { get { return App.EntryFontSize; } }
        //TEXTO DE LOS TITULOS DE PAGINAS
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
        //=========================================================================================================
        //---------------------------------------------------EVENTOS-----------------------------------------------
        public event PropertyChangingEventHandler PropertyChanging;

        //=========================================================================================================
        //---------------------------------------------------METODOS-----------------------------------------------
        //=========================================================================================================
        //=========================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaPrincipalViewModel()
        {
            //SE NOTIFICA MENDIANTE UN MENSAJE POR CONSOLA
            ConsoleWriteline("SE HA INICIADO LA APLICACION");
        }

        //=========================================================================================================
        //=========================================================================================================
        //METODO PARA IMPRIMIR POR CONSOLA EL CAMBIO DE LA VARIABLE
        private void ConsoleWriteline(string PROPIEDAD, string mensaje)
        {
            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nPROPIEDAD: " + PROPIEDAD + "\nVALOR: " + mensaje);
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");
        }

        //=========================================================================================================
        //=========================================================================================================
        //METODO PARA IMPRIMIR UN MENSAJE POR PANTALLAS
        private void ConsoleWriteline(string mensaje)
        {
            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nMENSAJE: \n" + mensaje);
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");
        }

        //==================================================================================
        //==================================================================================
        //FUNCION PARA GENERAR UN MENSAJE NO INTERACTIVO POR PANTALLA
        public void MensajePantalla(string mensaje)
        {
            Toast.MakeText(Android.App.Application.Context, mensaje, ToastLength.Short).Show();
        }

        //=========================================================================================================
        //=========================================================================================================
        //METODO PARA VALIDAR EL INGRESO DE UN USUARIO POR MEDIO DE CONSUMO DE SERVICIOS WEB
        public async Task<LogInResponse> LogInRequest()
        {
            //SE CREA E INICIALIZA LA VARIABLE URL
            string url = string.Empty;

            //SE CREA E INICIALIZA LA VARIABLE QUE RETENDRA EL URL PARA REALIZAR LA SOLICITUD HTTP
            url = App.BaseUrl + $"/login";

            LogInRequest model = null;

            //SE EVALUA SI EL USUARIO QUE INTENTA ACCEDER A LA PLATAFORMA ES EL USUARIO ADMINISTRATOR
            if (username.ToLower() == "administrator")
            {
                model = new LogInRequest()
                {
                    Username = username,
                    Password = password,
                };
            }
            else
            {
                model = new LogInRequest()
                {
                    Username = username,
                    Password = Metodos.EncryptString(password),
                };
            }

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

                        //SE REALIZA EL LLAMADO Y SE RECIBE UNA RESPUESTA
                        HttpResponseMessage response = await client.PostAsync(url, httpContent);

                        //SE EVALUA SI EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //EL CODIGO DE ESTATUS RETORNADO ES EL CODIGO 200 OK
                            //SE RECIBE LA RESPUESTA COMPLETA OBTENIDA POR EL SERVIDOR (STRING) 
                            result = await response.Content.ReadAsStringAsync();
                            //SE REALIZA LA EL MAPEO DE UN OBJETO JSON A UN OBJETO "LogInResponse"
                            LogInResponse loginresponse = await Task.FromResult(JsonConvert.DeserializeObject<LogInResponse>(result));
                            //SE DESENCRIPTA LA CONTRASEÑA
                            loginresponse.UserInfo.Usuario.Password = Metodos.DecryptString(loginresponse.UserInfo.Usuario.Password);
                            //SE RETORNA EL OBJETO 
                            return loginresponse;
                        }
                        else
                        {
                            //EL CODIGO DE ESTATUS RETORNADO ES DIFERENTE AL CODIGO DE ESTATUS 200 OK
                            //NOTA: EN EL CASO DEL CONTROLADOR ASIGNADO A PROCESAR LAS SOLICITUDES CON DICHA DIRECCION URL
                            //DENTRO DEL PROYECTO API SOLO TIENE TRES TIPOS DE RESPUESTA:
                            // - Ok (Se consiguo el nombre de usuario y la contraseña coincide)
                            // - Bad Request (Se consiguio el nombre de usuario pero la contraseña no coincide)
                            // - NotFound (No se consiguio el nombre de usuario)
                            result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                            //PUESTO QUE NO SE OBTUVO EL CODIGO DE ESTATUS 200 OK SE DESERIALIZA LA RESPUESTA OBTENIDA
                            //Y SE ALMACENA EN LA VARIABLE LOCAL "result"
                            //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                            return null;
                        }
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
                    //SE MANDA A IMPRIMIR POR CONSOLA EL ERROR OBTENIDO (EJECUTADO SOLO CUANDO SE DEPURA EL PROYECTO)
                    ConsoleWriteline("\nOcurrio un error => \n\n" + ex.Message.ToString());
                    //SE NOTIFICA AL USUARIO QUE NO SE PUDO REALIZAR LA SOLICITUD WEB.
                    result = "\nProblemas de conexión con el servidor";
                    //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                    return null;
                }
            }
            else
            {
                //EL EQUIPO NO ENCUENTRA CONECTADO A INTERNET, SE LE NOTIFICA AL USUARIO.
                result = "No hay conexión a internet";
                //PUESTO QUE NO SE OBTUVO EL CODIGO 200 OK RETORNAREMOS NULL COMO RESPUESTA AL LLAMADO DEL METODO
                return null;
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        //ACTUALIZA LA INFORMACION DE LA PROPIEDAD CADA QUE SE DECTECTA UN CAMBIO MINIMO
        private void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nombre));
        }
    }
}