using MttoApp.Model;
using MttoApp.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Essentials;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPrincipal : ContentPage
    {
        //CREACION E INICIALIZACION DE LOS OBJETOS
        private PaginaPrincipalViewModel ConexionDatos;
        private LogInResponse loginresponse;

        //=====================================================================================================================
        //=====================================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaPrincipal()
        {
            InitializeComponent();

            BindingContext = ConexionDatos = new PaginaPrincipalViewModel();
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //VALIDACION PARA EL INGRESO DE USUARIO
        async public void OnIngresar(Object sender, EventArgs e)
        {
            //SE VERICA SI EL ESTADO DEL ACCESO A LA RED ES NULO
            if(Connectivity.NetworkAccess == NetworkAccess.None) //=> true => NO HAY CONEXION A LA RED
            {
                //SE LE NOTIFICA AL USUARIO MEDIANTE UN MENSAJE QUE NO HAY CONEXION A LA RED
                await DisplayAlert("Mensaje", ConexionDatos.NetWorkConnectivityChangeMessage, ConexionDatos.OkText);
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.None) // true => SI HAY CONEXION A LA RED
            {
                //SE VERIFICA SI LAS PROPIEDADES DE LA CLASE VIEW MODEL SE ENCUENTREN VACIOS
                if (string.IsNullOrEmpty(ConexionDatos.Username) || 
                string.IsNullOrEmpty(ConexionDatos.Password))       //=> true => LAS PROPIEDADES SE ENCUENTRAN VACIAS O NULAS
                {
                    //SE LE NOTIFICA AL USUARIO MEDIANE UN MENSAJE QUE NINGUNA DE LAS DOS PROPIEDADES PUEDE SER VACIAS 
                    ConexionDatos.MensajePantalla(ConexionDatos.UsernamePasswordEmptyMessage);
                }
                else //=> false => NINGUNA DE LAS PROPIEDADES SE ENCUENTRA VACIA
                {
                    //------------------------------------------------------------------------------
                    //CONEXION CON LA BASE DE DATOS (CLIENTE - SERVIDOR)
                    //SE LLAMA AL METODO QUE REALIZA LA SOLICITUD HTTP
                    //SE HACE VISIBLE EL ACTIVITY INDICATOR
                    ActivityIndicator.IsVisible = true;
                    //SE ACTIVA EL ACTIVITY INDICATOR
                    ActivityIndicator.IsRunning = true;
                    //INICIAMOS UNA SECCION DE CODIGO QUE SE EJECUTARA EN SEGUNDO PLANO UTILIZANDO LA FUNCION Run DE LA CLASE TasK
                    await Task.Run(async () =>
                    {
                        //LLAMAMOS AL METODO "LogInReques" DE LA CLASE "PaginaPrincipalViewModel" Y GUARDAMOS LA RESPUESTA OBTENIDA
                        loginresponse = await ConexionDatos.LogInRequest();
                        //SE DESACTIVA EL ACTIVITY INDICATOR
                        ActivityIndicator.IsRunning = false;
                    });

                    //SE DESACTIVA LA VISIBILIDAD DEL OBJETO ACTIVITYINDICATOR
                    ActivityIndicator.IsVisible = false;

                    //SE EVALUA EL ESTADO DE LA RESPUESTA
                    if (loginresponse != null)
                    {
                        //SI LA RESPUESTA ES DIFERENTE DE NULL => SE OBTUVO UN RESULTADO
                        //SE LIMPIAN LAS ENTRADAS DE TEXTO PARA EL USERNAME Y PARA EL PASSWORD
                        usernameEntry.Text = string.Empty;
                        passwordEntry.Text = string.Empty;

                        App.Token.TokenInfo();

                        //SE REALIZA EL LLAMADO A LA PAGINA "UserMainPage"
                        await Navigation.PushModalAsync(new UserMainPage(loginresponse.UserInfo.Persona, 
                                                                         loginresponse.UserInfo.Usuario, 
                                                                         loginresponse.UltimaFechaIngreso));
                    }
                    else
                    {
                        //SI LA RESPUESTA ES NULA => NO SE OBTUVO RESULTADOS
                        //SE NOTIFICA AL USUARIO SOBRE EL INGRESO A LA PLATAFORMA
                        ConexionDatos.MensajePantalla(ConexionDatos.Result);
                        passwordEntry.Text = String.Empty;
                    }
                }
            }
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //FUNCION ACTIVADA CUANDO LA PAGINA APARECE
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //FUNCION ACTIVADA CUANDO LA PAGINA DESAPARECE
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        //=====================================================================================================================
        //=====================================================================================================================
        //METODO LLAMADO CADA QUE SE GENERE UN CAMBIO EN LA CONEXION A RED
        async private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            Console.WriteLine("\n\n==================================================");
            Console.WriteLine("==================================================");
            Console.WriteLine("\nDireccion IP del dispositivo movil: "+App.IPAddress+"\n");
            Console.WriteLine("==================================================");
            Console.WriteLine("==================================================\n\n");

            if (e.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("Mensaje", ConexionDatos.NetWorkConnectivityChangeMessage, "Entendido");
                return;
            }

        }
    }
}