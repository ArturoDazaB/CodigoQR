using Android.Widget;
using MttoApp.ViewModel;
using MttoApp.Model;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MttoApp.View.Paginas.PaginasInformacion;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConfiguracionAdmin : ContentPage
    {
        //==============================================================================
        //==============================================================================
        //OBJETOS
        //SE CREAN LAS VARIABLES Y OBJETOS GLOBALES DE LA CLASE
        private Personas Persona;
        private Usuarios Usuario;
        private ConfiguracionAdminViewModel ConexionDatos;

        //===============================================================================================================================
        //===============================================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaConfiguracionAdmin(Personas per, Usuarios usu)
        {
            InitializeComponent();

            //INICIALIZACION DE OBJETOS
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            BindingContext = ConexionDatos = new ConfiguracionAdminViewModel(true, Persona, Usuario, 0);

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
            

            //INDICAMOS AL "nivelusuarioPicker" QUE NIVEL DE USUARIO POSEE EL USUARIO A MODIFICAR
            switch (ConexionDatos.NivelUsuario)
            {
                //----------------------------------------------
                //NIVEL BAJO (0)
                case 0:
                    nivelusuarioPicker.SelectedIndex = 0;
                    break;
                //----------------------------------------------
                //NIVEL MEDIO (5)
                case 5:
                    nivelusuarioPicker.SelectedIndex = 1;
                    break;
                //----------------------------------------------
                //NIVEL ALTO (10)
                case 10:
                    nivelusuarioPicker.SelectedIndex = 2;
                    break;
            }
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

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA LA VERIFICACION DEL NOMBRE(S) CUANDO EL USUARIO DEJE DE INTERACTUAR CON EL 
        private void OnUnfocusedNombres(object sender, EventArgs e)
        {
            //SE VERIFICA SI EL NOMBRE PROPORCIONADO ESTA REGISTRADO
            if (ConexionDatos.flagsameNombre) //=> true => SE INGRESO EL MISMO DATO QUE SE ENCUENTRA REGISTRADO
            {
                //DE SER EL MISMO NOMBRE SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON EL MISMO NOMBRE O DESEA CAMBIARLO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedNombreSameNombre);
            }
            else //false => NO SE INGRESO EL MISMO DATO QUE SE ENCUENTRA REGISTRADO
            {
                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(ConexionDatos.Nombres)) //=> true => Existen caracteres prohibdos
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryNombres.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USUARIO
                    ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
                }
                else //=> false => No existen caracteres prohibidos
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A NEGRO
                    entryNombres.TextColor = Color.Black;
                }
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA LA VERIFICACION DEL APELLIDO(S) CUANDO EL USUARIO DEJE DE INTERACTUAR CON EL 
        private void OnUnfocusedApellidos(object sender, EventArgs e)
        {
            //SE VERIFICA SI EL NOMBRE PROPORCIONADO ESTA REGISTRADO
            if (ConexionDatos.flagsameApellido) //true => Se ingreso un apellido que ya se encuentra registrado
            {
                //DE SER EL MISMO NOMBRE SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON EL MISMO NOMBRE O DESEA CAMBIARLO
                //await DisplayAlert("Alerta", "El apellido es igual al que se encontraba previamente registrado.", "Entendido");
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedApellidoSameApellido);
            }
            else //true => No se ingreso un apellido que ya se encuentra registrado
            {
                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(ConexionDatos.Apellidos)) //=> true => Existen caracteres prohibidos
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryApellidos.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USUARIO DE LA EXISTENCIA DE CARACTERES PROHIBIDOS EN EL TEXTO QUE ACABA DE INGRESAR
                    ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
                }
                else //=> false => No existen caracteres prohibidos 
                    entryApellidos.TextColor = Color.Black;
            }
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL pickerFechaNacimiento
        protected void OnUnfocusedDate(object sender, EventArgs e)
        {
            //SE VERIFICA SI LA FECHA DE NACIMIENTO ES LA MISMA QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsameFecha ||                  //=> true => LA FECHA INGRESADA ES IGUAL A LA QUE SE ENCUENTRA PREVIAMENTE REGISTRADA
                ConexionDatos.FechaNacimiento > DateTime.Today) //=> true => LA FECHA INGRESADA ES MAYOR A LA FECHA ACTUAL
            {
                //DE SER LA MISMA SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE QUE NO SE HA REALIZADO 
                //NINGUN CAMBIO EN ESTE CAMPO A PESAR DE HABER SIDO INSPECCIONADO
                ConexionDatos.MensajePantalla(ConexionDatos.OnDateSelectedMessage);
            }
        }

        //=========================================================================================================================================================
        //=========================================================================================================================================================
        //FUNCION ACTIVADA LUEGO DE QUE SE DEJE DE ENFOCAR EL ENTRY DEL entryTelefono
        protected void OnUnfocusedTelefono(object sender, EventArgs e)
        {
            //SE VERIFICA SI EL NUMERO TELEFONICO ES EL MISMO QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsameTelefono) //=> true => EL TELEFONO INGRESADO ES IGUAL AL QUE SE ENCUENTRA REGISTRADO
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
            if (ConexionDatos.flagsameCorreo) //=> true => EL CORREO INGRESADO ES IGUAL AL QUE SE ENCUENTRA REGISTRADO
            {
                //DE SER LA MISMA SE GENERA UN AVISO AL USUARIO PARA NOTIFICARLE QUE NO SE HA REALIZADO 
                //NINGUN CAMBIO EN ESTE CAMPO A PESAR DE HABER SIDO INSPECCIONADO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedCorreoSameCorreo);
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA LA VERIFICACION DEL NOMBRE DE USUARIO CUANDO EL USUARIO DEJE DE INTERACTUAR CON EL 
        private void OnUnfocusedUsername(object sender, EventArgs e)
        {
            //SE DA SET AL COLOR DE TEXTO
            entryUsername.TextColor = Color.Black;

            //SE VERIFICA SI EL NOMBRE PROPORCIONADO ESTA REGISTRADO
            if (ConexionDatos.flagsameUsername) //=> true => EL NOMBRE DE USUARIO INGRESADO YA SE ENCUENTRA ACTUALMENTE REGISTRADO
            {
                //DE SER EL MISMO NOMBRE SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON EL MISMO NOMBRE O DESEA CAMBIARLO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedUsernameSameUsername);
            }
            else //=> false => EL NOMBRE DE USUARIO INGRESADO NO SE ENCUENTRA ACTUALMENTE REGISTRADO
            {
                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(ConexionDatos.Username)) //=> TRUE => EXISTEN CARACTERES PROHIBIDOS EN EL TEXTO
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryUsername.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USAURIO MEDIANTE UN MENSAJE
                    ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
                }

                //VERIFICACION DE ESPACIOS EN BLANCO 
                if (Metodos.EspacioBlanco(ConexionDatos.Username))  //=> TRUE => EXISTEN ESPACIOS EN BLANCO EN EL TEXTO
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryUsername.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USAURIO MEDIANTE UN MENSAJE
                    ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedUsernameWhiteSpace);
                }
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA VOLVER "PASSWORD" EL CAMPO "entryPassword"
        private void FocusedPassword(object sender, EventArgs e)
        {
            //SE DESACTIVA LA VISTA COMO PASSWORD
            entryPassword.IsPassword = false;
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA LA VERIFICACION DE LA CONTRASEÑA CUANDO ESTA NO SE ENCUENTRE ENFOCADA
        private void UnFocusedPassword(object sender, EventArgs e)
        {
            //SE VUELVE A ACTIVAR LA VISTA COMO PASSWORD
            entryPassword.IsPassword = true;

            //SE DA SET AL COLOR DE TEXTO
            entryPassword.TextColor = Color.Black;

            //SE VERIFICA SI LA CONTRASEÑA ES LA MISMA QUE EXISTE ACTUALMENTE
            if (ConexionDatos.flagsamePassword) //=> true => LA CONTRASEÑA INGRESADA ES IGUAL A LA QUE SE ENCUENTRA ACTUALMENTE REGISTRADA
            {
                //DE SER LA MISMA CONTRASEÑA SE GENERA UNA PREGUNTA AL USUARIO PARA
                //NOTIFICARLE SI DEASEA CONTINUAR CON LA MISMA CONTRASEÑA O DESEA CAMBIARLA
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedPasswordSamePassword);
            }
            else //=> false => LA CONTRASE;A INGRESADA NO ES IGUAL A LA QUE SE ENCUENTRA ACTUALMENTE REGISTRADA.
            {
                //SI LA CONTRASEÑA ES DISTINTA, SE PROCEDE A VERIFICAR QUE LA NUEVA CONTRASEÑA CUMPLA CON LOS REQUISITOS MINIMOS ESTABLECIDOS

                //VERIFICACION DE CARACTERES NO PERMITIDOS
                if (Metodos.Caracteres(ConexionDatos.Password)) //=> true => Existen caracteres prohibidos dentro del texto
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryPassword.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USAURIO A TRAVES DE UN MENSAJE
                    //await DisplayAlert("Alerta", "No se aceptan los siguientes caracteres: '!', '@', '#', '$', '%', '&', '(', ')', '+', '=', '/', '|'", "Entendido");
                    ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
                }

                //VERIFICACION DE ESPACIOS EN BLANCO
                if (Metodos.EspacioBlanco(ConexionDatos.Password)) //=> true => Existen espacios en blanco dentro del texto
                {
                    //SE CAMBIA EL COLOR DEL TEXTO A ROJO
                    entryPassword.TextColor = Color.Red;
                    //SE LE NOTIFICA AL USUARIO A TRAVES DE UN MENSAJE
                    //await DisplayAlert("Alerta", "La contraseña no puede contener espacios en blanco", "Entendido");
                    ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordWhiteSpace);
                }
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA LA ACTUALIZACION DE DATOS SOBRE LA BASE DE DATOS
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
                    await Navigation.PopAsync();
                }
            }
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //METODO PARA EJECUTAR EL LLAMADO A LA VENTANA, DE TIPO POP-UP, DE INFORMACION DE LA PAGINA ACTUAL
        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PaginaInformacionConfiguracion("CONFIGURACIONADMIN"));
        }
    }
}