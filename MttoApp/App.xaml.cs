using MttoApp.View.Paginas;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MttoApp
{
    public partial class App : Application
    {
        //VARIABLE QUE ALMACENARA EL NOMBRE DEL ARCHIVO QUE TENDRA LA BASE DE DATOS
        //LOCAL QUE MANEJARA LA APLICACION CUANDO SE ENCUENTRE FUNCIONANDO STAND ALONE
        public static string FileName;

        //===============================================================================================
        //===============================================================================================
        //CONSTANTES QUE SERAN LLAMADOS EN LOS DISTINTOS "VIEWMODEL" EMPLEADOS PARA CADA PAGINA

        //DIMENSIONES DEL CODIGO QR
        public const int ByWSize = 22;

        //TAMAÑO DE LAS ETIQUETAS
        public const int LabelFontSize = 12;

        //TAMAÑO DE LAS ENTRADAS
        public const int EntryFontSize = 15;

        //TAMAÑO DE LOS TITULOS DE SECCION
        public const int HeaderFontSize = 25;

        //TAMAÑO DEL NOMBRE DE USUARIO EN LA CLASE "MasterPage.xaml.cs"
        public const int SmallHeaderFontSize = 20;

        //COLOR DEL FONDO
        public const string BackGroundColor = "#fcf3e3";

        //COLOR DEL FONDO (PAGINAS DE INFORMACION)
        public const string BackGroundColorPopUp = "#FFFDE7";

        //COLOR DEL MARCO (PAGINAS DE INFORMACION)
        public const string FrameColorPopUp = "#000000";

        //COLOR DE LOS BOTONES
        public const string ButtonColor = "#E53935";

        //DIRECCION URL BASE PARA LAS SOLICITUDES HTTP
        public const string BaseUrl = "https://10.10.0.83:5001/mttoapp";

        //TIEMPO DE ESPERA CUANDO SE REALIZA UNA SOLICITUD HTTP
        public const int TimeInSeconds = 5;

        //TEXTO INFORMATIVO QUE USADO PARA INDICAR QUE EL DISPOSITIVO NO POSEE ACCESO A INTERNET
        public const string NoNetworkAccessMessage = "Sin Acceso a Internet Enciende el WIFI o la Red Móvil para poder acceder";

        //DIRECCION IP DEL DISPOSITIVO
        public static string IPAddress { get { return DependencyService.Get<MttoApp.Servicios.IIPAddressManager>().GetIPAddress(); } }

        //TEXTO USADO PARA AFIRMAR (PROCEDER)
        public const string AffirmativeText = "Si";

        //TEXTO USADO PARA NEGAR (DENEGAR)
        public const string NegativeText = "No";

        //TEXTO USADO PARA AFIRMAR (ENTENDIDO)
        public const string OkText = "Entendido";

        //TEXTO USADO PARA INDICAR CUALES SON LOS CARACTERES PROHIBIDOS
        public const string ForbiddenCharacters = "'!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'";

        //TEXTO USADO PARA INDICAR EL "KEY" / "LLAVE" USADA EN LOS METODOS
        //SUBSCRIBE Y SEND DE LA CLASE "MessageCenter"
        public const string ItemUpdate = "ItemUpdate";

        public const string ItemAdd = "ItemAdd";

        


        //===============================================================================================
        //===============================================================================================
        //PROPIEDADES DE LA APLICACION
        public static MasterDetailPage MasterDetail { get; set; }

        //SE DEFINEN LAS PROPIEDADES "SecretKey" y "PublicKey" PARA LAS FUNCIONES DE ECRIPTACION
        //Y DESENCRIPTACION (FUNCIONES ALBERGADAS EN EL ARCHIVO "Metodos").
        public static string SecretKey { get { return "12345678"; } }

        public static string PublicKey { get { return "98765432"; } }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE
        public App()
        {
            InitializeComponent();
            //SE CONFIGURA A LA PAGINA "PaginaPrincipal" COMO LA PAGINA INICIAL QUE
            //APARECERA AL MOMENTO DE INICIAR LA APLICACION
            MainPage = new NavigationPage(new PaginaPrincipal());
        }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (EXISTEN DOS METODOS CONTRUCTORES, CON LA DIFERENCIA DE QUE UNO DE ELLOS
        //REQUIERE QUE SEA ENVIADO UN PARAMETRO QUE LLEVA POR NOMBRE "filename" (PARAMETRO QUE LLEVARA EL
        //NOMBRE DE ARCHIVO CON EL  CUAL SE IDENTIFICARA LA BASE DE DATOS LOCAL Sqlite QUE UTILIZA LA
        //APLICACION CUANDO SE ENCUENTRA FUNCIONANDO STAND ALONE)
        //-----------------------------------------------------------------------------------------------
        //NOTA: ESTE METODO CONSTRUCTOR ES LLAMADO EN LAS CLASES "MainActivity" Y "AppDelegate" DE LOS
        //PROYECTOS MttoApp.Android Y MttoApp.iOS RESPECTIVAMENTE
        //-----------------------------------------------------------------------------------------------
        //===============================================================================================
        //===============================================================================================
        public App(string fileName)
        {
            InitializeComponent();
            //SE CONFIGURA A LA PAGINA "PaginaPrincipal" COMO LA PAGINA INICIAL QUE
            //APARECERA AL MOMENTO DE INICIAR LA APLICACION
            MainPage = new NavigationPage(new PaginaPrincipal());
            FileName = fileName;
        }

        //===============================================================================================
        //===============================================================================================
        //LA CLASE APLICACION (App) CONTIENE TRES METODOS "VIRTUALES" (PALABRA CLAVE QUE INVALIDA LAS
        //ACCIONES EJECUTADAS EN EL METODO MATRIZ Y PERMITE REDEFINIRLO EN UNA CLASE DERIVADA) QUE
        //PUEDEN SER REEMPLAZADOS PARA RESPONDER A CAMBIOS DE CICLO DE VIDA DE LA APLICACION

        //METODO INVOCADO CUANDO SE INICIA LA APLICACION
        protected override void OnStart()
        {
            Console.WriteLine("\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("\nINICIO DE LA APLICACION");
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n");
        }

        //METODO INVOCADO CADA VEZ QUE LA APLICACION PASA A SEGUNDO PLANO.
        protected override void OnSleep()
        {
        }

        //METODO INVOCADO CUANDO SE REANUDA LA APLICACION (DESPUES DE HABERLA ENVIADO A SEGUNDO PLANO)
        protected override void OnResume()
        {
        }

        //===============================================================================================
        //===============================================================================================
        //FUNCION LLAMADA PARA ACEPTAR EL CERTIFICADO DE SEGURIDAD GENERADO POR LOS EQUIPO SERVIDOR
        public static HttpClientHandler GetInsecureHandler()
        {
            //SE CREA E INICIALIZA UN OBJETO DE TIPO "HttpClientHandler" (CLASE QUE PERMITE
            //A LOS DESARROLLADORES CONFIGURAR OPCIONES DE AUTENTICACION)
            HttpClientHandler handler = new HttpClientHandler();

            //SE HACE EL LLAMADO AL METODO "ServerCertificateCustomValidationCallBack" PARA OBTENER
            //Y VALIDAR EL CERTIFICADO DE SERVIDOR
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                //SE EVALUA EL CERTIFICADO DE AUTENTIFICACION DE LA URL UTILIZADA PARA LA COMUNICACION CON EL SERVIDOR
                if (cert.Issuer.Equals("CN=DESKTOP-BEEFDVC") || //=> CERTIFICADO EMITIDO POR MI COMPUTADORA PERSONAL (Carlos Arturo Daza Bohorquez; Programador Industrial)
                    cert.Issuer.Equals("CN=localhost"))         //=> CERTIFICADO EMITIDO POR ASP.NET Core CUANDO SE EJECUTA EL SERVICIO WEB DESDE EL PROYECTO "MttoApi"
                {
                    //SI EL CERTIFICADO EMITIDO POR EL URL ES ALGUNO DE LOS ANTERIORES ENTONCES SE PERMITE LA COMUNICACION CON EL SERVIDOR
                    return true;
                }

                // SE DA SET A EL PARAMETRO errors CON "None" => "NO SSL policy errors"
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            //SE RETORNA EL OBJETO CREADO
            return handler;
        }

        public class Token
        {
            private const string TokenExpired = "El tiempo de navegacion ha expirado.\nVuelva a ingresar nuevamente.";

            private const string NoAuthenticated = "Usuario no autenticado.\nSolicitud denegada.";

            public static async void TokenInfo()
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(await SecureStorage.GetAsync("token"));

                Console.WriteLine("\n\n==================================================");
                Console.WriteLine("==================================================");
                Console.WriteLine("FECHA DE CREACION DEL TOKEN: " +
                    DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(token.Claims.FirstOrDefault(c => c.Type == "nbf")?.Value)));
                Console.WriteLine("FECHA DE EXPIRACION: " +
                    DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value)));
                Console.WriteLine("==================================================");
                Console.WriteLine("==================================================\n\n");
            }

            public static async Task<string> UserInfoMessage()
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(await SecureStorage.GetAsync("token"));

                if (DateTimeOffset.Now >= DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value)))
                {
                    return TokenExpired;
                }
                else
                {
                    return NoAuthenticated;
                }
            }

            public static async Task<bool> IsExpired()
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(await SecureStorage.GetAsync("token"));

                if (DateTimeOffset.Now >= DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value)))
                    return true;
                else
                    return false;
            }

        }
    }
}