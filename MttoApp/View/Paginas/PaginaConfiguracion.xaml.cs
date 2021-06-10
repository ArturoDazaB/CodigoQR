using MttoApp.Model;
using MttoApp.View.Paginas.PaginasInformacion;
using MttoApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConfiguracion : ContentPage
    {
        //================================================================================
        //================================================================================
        //OBJETOS
        //SE CREAN LAS VARIABLES Y OBJETOS GLOBALES DE LA CLASE
        private Personas Persona;

        private Usuarios Usuario;
        private ConfiguracionAdminViewModel ConexionDatos;

        //===============================================================================================================================
        //===============================================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaConfiguracion(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Y SE EJECUTA EL ENLACE DE LA CLASE VISTA (VIEW) Y LA CLASE (VIEWMODEL)
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);
            BindingContext = ConexionDatos = new ConfiguracionAdminViewModel(true, Persona, Usuario, Usuario.Cedula);

            //SE DESACTIVA LA VISIBILIDAD DEL ACTIVITY INDICATOR DE LA PAGINA
            ActivityIndicator.IsVisible = false;

            //SE EVALUA SI OCURRIO UN ERROR AL MOMENTO DE APERTURAR LA PAGINA
            //NOTA: PUESTO QUE ESTA PAGINA ESTA DEDICADA A LA MODIFICACION DE LOS DATOS PERSONALES
            //(EL MISMO USUARIO MODIFICARA SU INFORMACION) SE EVALUA QUE LAS CEDULAS (ID) DE LOS
            //OBJETOS Persona Y Usuario SON IGUALES
            if (ConexionDatos.Error)
            {
                //Error -> TRUE: LAS CEDULAS (ID) DE LOS OBJETOS Persona Y Usuario SON DISTINTOS
                //         FALSE: LAS CEDULAS (ID) DE LOS OBJETOS Persona Y Usuario SON IGUALES
                Navigation.PopAsync();
            }

            //INDICAMOS AL "nivelusuarioPicker" QUÉ NIVEL DE USUARIO POSEÉ EL USUARIO A MÓDIFICAR
            switch (ConexionDatos.NivelUsuario)
            {
                //----------------------------------------------
                //NIVEL BAJO (1)
                case 1:
                    nivelusuarioPicker.SelectedIndex = 0;
                    break;
                //----------------------------------------------
                //NIVEL MEDIO (5)
                case 5:
                    nivelusuarioPicker.SelectedIndex = 1;
                    break;
                //----------------------------------------------
                //NIVEL SUPERIOR (8)
                case 8:
                    nivelusuarioPicker.SelectedIndex = 2;
                    break;
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA EJECUTAR EL LLAMADO A LA VENTANA, DE TIPO POP-UP, DE INFORMACION DE LA PAGINA ACTUAL
        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PaginaInformacionConfiguracion("CONFIGURACION"));
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION QUE INHABILITA EL BackButton NATIVO DE ANDROID
        //NOTA: SE HABILITO ESTA FUNCION DEBIDO A LA NATURALEZA DE LA PAGINA (MODIFICACION) DE ESTA MANERA SI EL USUARIO
        //RETROCEDE DE MANERA ININTENCIONAL (PRESIONANDO EL BOTON DE REGRESO NATIVO) LA PAGINA NO SE VERA CERRADA
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL entryTelefono
        protected void OnUnfocusedTelefono(object sender, EventArgs e)
        {
            //SE VERIFICA SI EL NUMERO TELEFONICO ES EL MISMO QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsameTelefono)
            {
                //DE SER EL MISMO SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE QUE NO SE HA REALIZADO
                //NINGUN CAMBIO EN ESTE CAMPO A PESAR DE HABER SIDO INSPECCIONADO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedTelefonoSameTelefono);
            }
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL entryCorreo
        protected void OnUnfocusedCorreo(object sender, EventArgs e)
        {
            //SE VERIFICA SI LA DIRECCION DE CORREO ES LA MISMA QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsameCorreo)
            {
                //DE SER LA MISMA SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE QUE NO SE HA REALIZADO
                //NINGUN CAMBIO EN ESTE CAMPO A PESAR DE HABER SIDO INSPECCIONADO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedCorreoSameCorreo);
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION ACTIVADA CUANDO EL USUARIO INTERACTUA CON EL OBJETO "entryPassword"
        private void FocusedPassword(object sender, EventArgs e)
        {
            //SE CAMBIA LA VISTA DEL entryPassword DE PASSWORD a NORMAL
            entryPassword.IsPassword = false;
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION USADA PARA LA VERIFICACION DE LA CONTRASEÑA CUANDO EL USUARIO DEJE DE INTERACTUAR CON EL OBJETO "entryPassword"
        private void UnFocusedPassword(object sender, EventArgs e)
        {
            //SE VUELVE A ACTIVAR LA VISTA COMO PASSWORD
            entryPassword.IsPassword = true;

            //SE VERIFICA SI LA CONTRASEÑA ES LA MISMA QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsamePassword)
            {
                //DE SER LA MISMA CONTRASEÑA SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON LA MISMA CONTRASEÑA O DESEA CAMBIARLA
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedPasswordSamePassword);
            }
            else
            {
                //SI LA CONTRASEÑA ES DISTINTA, SE PROCEDE A VERIFICAR QUE LA NUEVA CONTRASEÑA
                //CUMPLA CON LOS REQUISITOS MINIMOS ESTABLECIDOS

                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(ConexionDatos.Password)) //true => Existen los caracteres no permitidos
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryPassword.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USUARIO
                    ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
                }
                else
                    //SE DEJA EL COLOR DEL TEXTO EN ROJO
                    entryPassword.TextColor = Color.Black;

                //VERIFICACION DE ESPACIOS EN BLANCO
                if (Metodos.EspacioBlanco(ConexionDatos.Password))
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryPassword.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USUARIO
                    ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordWhiteSpace);
                }
                else
                    entryPassword.TextColor = Color.Black;
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION USADA CUANDO SE DESEA ACTUALIZAR LA INFORMACION DE UN USUARIO.
        async protected void OnActualizar(object sender, EventArgs e)
        {
            //VARIABLE LOCAL
            string respuesta = string.Empty;

            //EJECUCION DE PREGUNTA DE CONFIRMACION Y EVALUACION DE LA RESPUESTA
            if (await DisplayAlert("Alerta", ConexionDatos.OnActualizarMethodMessage, ConexionDatos.AffirmativeText, ConexionDatos.NegativeText))
            {
                //------------------------------------------------------------------------------------------------------
                //----------------------CODIGO PARA REGISTRAR UN USUARIO MEDIANTE CONSUMO DE API------------------------
                //LLAMAMOS AL METODO "Save" DE LA CLASE "ConfiguracionAdminViewModel" Y GUARDAMOS LA RESPUESTA OBTENIDA
                //VOLVEMOS VISIBLE EL ACTIVITY INDICATOR
                ActivityIndicator.IsVisible = true;
                //INICIAMOS EL ACTIVITY INDICATOR
                ActivityIndicator.IsRunning = true;
                //INICIAMOS UNA SECCION DE CODIGO QUE SE EJECUTARA EN SEGUNDO PLANO UTILIZANDO LA FUNCION Run DE LA CLASE TasK
                await Task.Run(async () =>
                {
                    //LLAMAMOS AL METODO "Save" DE LA CLASE "ConfiguracionAdminViewModel" Y GUARDAMOS LA RESPUESTA OBTENIDA
                    respuesta = await ConexionDatos.Save();
                    //DETENEMOS EL ACTIVITY INDICATOR
                    ActivityIndicator.IsRunning = false;
                });

                //SE DESACTIVA LA VISIBILIDAD DEL ACTIVITY INDICATOR
                ActivityIndicator.IsVisible = false;

                //SE MUESTRA EL MENSAJE OBTENIDO
                ConexionDatos.MensajePantalla(respuesta);

                //SE VERIFICA EL TEXTO CONTENIDO DENTRO DE LA RESPUESTA DE LA APLICACION
                if (respuesta.ToLower() == "datos actualizados")
                {
                    //SE REALIZA UNA PAUSA DE 2 SEGUNDOS
                    await Task.Delay(1000);
                    //SI LA RESPUESTA ES POSITIVA SE PROCEDE A CERRAR LA PAGINA
                    await App.MasterDetail.Navigation.PopModalAsync();
                }
            }
        }
    }
}