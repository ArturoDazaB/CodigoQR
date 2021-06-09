using Android.Widget;
using MttoApp.ViewModel;
using MttoApp.Model;
using MttoApp.View.Paginas.PaginasInformacion;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaRegistro : ContentPage
    {
        //================================================================================
        //================================================================================
        //OBJETOS
        //SE CREAN LAS VARIABLES Y OBJETOS GLOBALES DE LA CLASE
        private ConfiguracionAdminViewModel ConexionDatos;
        private Personas Persona; 
        private Usuarios Usuario;

        //===============================================================================================================================
        //===============================================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaRegistro(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Y SE EJECUTA EL ENLACE DE LA CLASE VISTA (VIEW) Y LA CLASE (VIEWMODEL)
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);
            BindingContext = ConexionDatos = new ConfiguracionAdminViewModel(false, Persona, Usuario, Usuario.Cedula);
            //SE DESACTIVA Y SE VUELVE INVISIBLE EL "ActivityIndicator"
            ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = false;
            //SE VUELVE INVISIBLE LA SECCION DE LOS DATOS DE USUARIO
            DatosUsuarioGrid.IsVisible = false;
            //SE HABILITAN LOS ENTRY nombreEntry Y apellidosEntry
            nombresEntry.IsEnabled = apellidosEntry.IsEnabled = true;
            //SE ASIGNA EL TEXTO "CONTINUAR" AL TEXTO DEL BOTON DE LA PAGINA
            Boton.Text = "CONTINUAR";
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //FUNCION PARA VERIFICAR SI correoEntry TIENE ESPACIOS EN BLANCO
        private void OnCompletedCorreo(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO
            //BANDERA QUE VERIFICA SI LA CADENA DE CARACTERES ALMACENA UN ESPACIO EN BLANCO
            if (Metodos.EspacioBlanco(ConexionDatos.Correo.ToLower()))
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL correoEntry A ROJO
                correoEntry.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER ESPACIOS EN BLANCO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedCorreoWhiteSpace);
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //FUNCION PARA VERIFICAR QUE usernameEntry NO TENGA ESPACIOS EN BLANCO Y QUE NO TENGA LOS CARACTERES NO PERMITIDOS
        private void OnCompletedUserName(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO
            if (Metodos.EspacioBlanco(ConexionDatos.Username.ToLower())) //=> true => Existen espacios en blanco en el nombre de usuario
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL usernameEntry A ROJO
                usernameEntry.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE EL NOMBR DE USUARIO NO PUEDE TENER ESPACIOS EN BLANCO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletedUsernameWhiteSpace);
            }

            //BUSQUEDA DE CARACTERES
            if (Metodos.Caracteres(ConexionDatos.Username.ToLower())) //=> true => Existen caracteres prohibidos en el nombre de usuario.
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL usernameEntry A ROJO
                usernameEntry.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE EL NOMBRE DE USUARO NO PUEDE TENER CARACTERES ESPECIFICOS
                ConexionDatos.MensajePantalla(ConexionDatos.ForbiddenCharacters);
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //FUNCION PARA VERIFICAR QUE passwordEntry1 NO TENGA ESPACIOS EN BLANCO QUE NO SEA MENOR A 6 CARACTERES Y QUE NO POSEA LOS
        //CARACTERES NO PERMITIDOS
        private async void OnCompletedPassword1(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO
            if (Metodos.EspacioBlanco(ConexionDatos.Password.ToLower())) //=> true = Existen espacios en blanco
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL passwordEntry1 A ROJO
                passwordEntry1.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER ESPACIOS EN BLANCO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordWhiteSpace);
            }
            else //=> false = No existen espacios en blanco
            {
                //VERIFICACION DE CANTIDAD DE CARACTERES
                if (ConexionDatos.Password.Length < 6)
                {
                    //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER MENOS DE 6 CARACTERES
                    ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordMinimunLenght);
                }
            }

            //VERIFICACION DE CARACTERES NO PERMITIDOS

            if (Metodos.Caracteres(ConexionDatos.Password.ToLower())) //=> true = Existen caracteres prohibidos.
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL passwordEntry1 A ROJO
                passwordEntry1.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER NINGUNO DE LOS CARACTERES PROHIBIDOS
                await DisplayAlert("Alerta", ConexionDatos.ForbiddenCharacters, ConexionDatos.OkText);
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //FUNCION PARA VERIFICAR QUE passwordEntry2 NO TENGA ESPACIOS EN BLANCO, QUE NO SEA MENOR A 6 CARACTERES Y QUE NO POSEA LOS
        //CARACTERES NO PERMITIDOS
        private async void OnCompletedPassword2(object sender, EventArgs e)
        {
            //BUSQUEDA DE ESPACIOS EN BLANCO

            //SI LAS CONTRASEÑAS SON DISTINTAS SE DISPARA LA BANDERA "flagdifferentPassword"
            if (ConexionDatos.flagdifferentPassword)
                await DisplayAlert("Alerta", ConexionDatos.PasswordDoesNotMatch ,ConexionDatos.OkText);

            //SI LA CONTRASEÑA TIENE ESPACIOS EN BLANCO SE DESPLIEGA UN MENSAJE DE NOTIFICACION
            if (Metodos.EspacioBlanco(ConexionDatos.ConfirmacionPassword.ToLower())) //=> true = Existen espacios en blanco 
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL passwordEntry2 A ROJO
                passwordEntry2.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER ESPACIOS EN BLANCO
                ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordWhiteSpace);
            }
            else //=> false = No existen espacios en blanco
            {
                //VERIFICACION DE CANTIDAD DE CARACTERES
                if (ConexionDatos.ConfirmacionPassword.Length < 6)
                {
                    //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER MENOS DE 6 CARACTERES
                    ConexionDatos.MensajePantalla(ConexionDatos.OnCompletePasswordMinimunLenght);
                }
            }

            //VERIFICACION DE CARACTERES NO PERMITIDOS

            if (Metodos.EspacioBlanco(ConexionDatos.ConfirmacionPassword.ToLower())) //=> true = Existen caracteres prohibidos.
            {
                //SE CAMBIA EL COLOR DEL TEXTO DEL passwordEntry2 A ROJO
                passwordEntry2.TextColor = Color.Red;
                //SE LE NOTIFICA AL USUARIO QUE LA CONTRASEÑA NO PUEDE TENER NINGUNO DE LOS CARACTERES PROHIBIDOS
                await DisplayAlert("Alerta", ConexionDatos.ForbiddenCharacters, ConexionDatos.OkText);
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //FUNCION PARA REGRESAR A COLOR NEGRO CUANDO SE CORRIJAN LOS TEXTOS DE LOS ENTRY ASIGNADOS A: CORREO, USERNAME y PASSWORD(1 y 2)
        private void CorreccionCorreo(object sender, TextChangedEventArgs e)
        {
            //LA ACCION DE ESTE LLAMADO SOLO SERA PERCEPTIBLE CUANDO LA CADENA DE CARACTERES
            //QUE ARROJA correoEntry TENGRA UN ESPACIO EN BLANCO EN ELLA (' ' o " ")
            correoEntry.TextColor = Color.Black;
        }

        //==================================================================================================================================
        //==================================================================================================================================
        //FUNCION PARA REALIZAR EL CAMBIO DE COLOR DE ACUERDO A LA CANTIDAD DE CARACTEREES QUE POSEE DE LA ENTRADA passwordEntry1
        private void CorreccionPassword1(object sender, TextChangedEventArgs e)
        {
            if (ConexionDatos.Password.Length >= ConexionDatos.PasswordYellowColorLegnth && 
                ConexionDatos.Password.Length <= ConexionDatos.PasswordGreenColorLegnth)
                passwordEntry1.TextColor = Color.Yellow;

            if (ConexionDatos.Password.Length > ConexionDatos.PasswordGreenColorLegnth)
                passwordEntry1.TextColor = Color.Green;

            if (ConexionDatos.Password.Length <= ConexionDatos.PasswordYellowColorLegnth)
                passwordEntry1.TextColor = Color.Black;
        }

        //==================================================================================================================================
        //==================================================================================================================================
        //FUNCION PARA REALIZAR EL CAMBIO DE COLOR DE ACUERDO A LA CANTIDAD DE CARACTEREES QUE POSEE DE LA ENTRADA passwordEntry1
        private async void CorreccionPassword2(object sender, TextChangedEventArgs e)
        {

            if (ConexionDatos.ConfirmacionPassword.Length >= ConexionDatos.PasswordYellowColorLegnth && 
                ConexionDatos.ConfirmacionPassword.Length <= ConexionDatos.PasswordGreenColorLegnth)
                passwordEntry2.TextColor = Color.Yellow;

            if (ConexionDatos.ConfirmacionPassword.Length > ConexionDatos.PasswordGreenColorLegnth)
                passwordEntry2.TextColor = Color.Green;

            if (ConexionDatos.ConfirmacionPassword.Length <= ConexionDatos.PasswordYellowColorLegnth)
                passwordEntry2.TextColor = Color.Black;

            //==================================================================================================================================
            //==================================================================================================================================
            //SI LAS CONTRASEÑAS NO COINCIDEN SE DISPARA LA ALARMA "flagdifferentPassword"
            if (ConexionDatos.flagdifferentPassword) //=> true = LAS CONTRASEÑAS (Password y ConfirmacionPassword) NO COINCIDEN
                //SE LE NOTIFICA AL USUARIO QUE LAS CONTRASEÑAS NO COINCIDEN
                await DisplayAlert("Alerta", ConexionDatos.PasswordDoesNotMatch, ConexionDatos.OkText);
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO DE REGISTRO: METODO LLAMADO CUANDO SE PRECIONA EL BOTON DE LA PAGINA DE REGISTRO
        public async void OnButtonPush(object sender, EventArgs e)
        {
            //CREACION E INICIALIZACION DE LA VARIABLE QUE RECIBIRA LA RESPUESTA DE LOS LLAMADOS DE LOS METODOS DE LA CLASE "ConfiguracionAdminViewModel.cs"
            string respuesta = string.Empty;

            //SE EVALUARA EL TEXTO QUE CONTIENE EL BOTON DE LA PAGINA
            switch (Boton.Text)
            {
                //TEXTO CONTINUAR: EN ESTA SECCION EL RECUADRO QUE CONTIENE LAS ENTRADAS PARA LA INFORMACION DE USUARIO SE ENCUENTRA
                //ESCONDIDA (NO ES VISIBLE). PRIMERO SE DEBEN LLENAR TODOS LOS CAMPOS DE LA INFORMACION PERSONAL
                case "CONTINUAR":
                    //ACTIVAMOS EL ActivityIndicator
                    ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = true;
                    //ESPERAMOS 1.250 SEGUNDOS
                    await Task.Delay(1250);
                    //DESACTIVAMOS EL ActivityIndicator
                    ActivityIndicatorA.IsRunning = ActivityIndicatorA.IsVisible = false;

                    //EVALUAMOS SI LOS CAMPOS DE INFORMACION PERSONAL SE ENCUENTRAN NO VACIO O NO NULOS
                    if (ConexionDatos.ContinuarRegistro(true))
                    {
                        //TODOS LOS CAMPOS SE ENCUENTRAN LLENOS
                        //------------------------------------------------------------------------------
                        //SE CAMBIA EL TEXTO DEL BOTON
                        Boton.Text = "REGISTRAR";
                        //SE MUESTRA EL NOMBRE DE USUARIO GENERADO A PARTIR DE LA COMBINACION DE NOMBRE(S) Y APELLIDO(S)
                        usernameEntry.Text = ConexionDatos.Username;
                        //SE DESHABILITAN LAS ENTRADAS ASIGNADAS A LOS CAMPOS NOMBRE(S) Y APELLIDO(S)
                        nombresEntry.IsEnabled = apellidosEntry.IsEnabled = false;
                        //SE VUELVE VISIBLE LA SECCION QUE CONTIENE LOS CAMPOS DE INFORMACION PERSONAL
                        DatosUsuarioGrid.IsVisible = true;
                        //------------------------------------------------------------------------------
                    }
                    else
                    {
                        //ALGUNO DE LOS CAMPOS NO SE ENCUENTRAN LLENOS
                        //------------------------------------------------------------------------------
                        //SE NOTIFICA AL USUARIO POR MEDIO DE UN MENSAJE POP UP QUE DEBE LLENAR TODOS LOS CAMPOS
                        await DisplayAlert("Mensaje", "Todos los campos referentes a la informacion personal deben llenarse.", "Entendido");
                        //SE VUELVE INVISIBLE LA SECCION QUE CONTIENE LOS CAMPOS DE INFORMACION PERSONAL
                        DatosUsuarioGrid.IsVisible = false;
                        //------------------------------------------------------------------------------
                    }
                    break;
                //TEXTO REGISTRAR: EN ESTA SECCION SE ENCUENTRAN VISIBLES LAS DOS SECCIONES DE LA PAGINA REGISTRAR: "Informacion Personal"
                //e "Informacion de Usuario".
                case "REGISTRAR":
                    //SE REALIZA UNA PREGUNTA Y SE EVALUA LA RESPUESTA
                    if (await DisplayAlert("Alerta", 
                        ConexionDatos.OnButtonPushMethodMessage, ConexionDatos.AffirmativeText, ConexionDatos.NegativeText))
                    {
                        //INICIAMOS EL ACTIVITY INDICATOR
                        ActivityIndicatorA.IsVisible = true;
                        ActivityIndicatorA.IsRunning = true;
                        //INICIAMOS UNA SECCION DE CODIGO QUE SE EJECUTARA EN SEGUNDO PLANO UTILIZANDO LA FUNCION Run DE LA CLASE TasK
                        await Task.Run(async () =>
                        {
                            //LLAMAMOS AL METODO "Save" DE LA CLASE "ConfiguracionAdminViewModel" Y GUARDAMOS LA RESPUESTA OBTENIDA
                            respuesta = await ConexionDatos.Save();
                            //DETENEMOS EL ACTIVITY INDICATOR
                            ActivityIndicatorA.IsRunning = false;
                        });

                        ActivityIndicatorA.IsVisible = false;

                        //SE MUESTRA EL MENSAJE OBTENIDO
                        ConexionDatos.MensajePantalla(respuesta);
                        //SE EVALUA SI LA RESPUESTA OBTENIDA ES "Registro Exitoso"
                        if(respuesta.ToLower() == "registro existoso")
                        {
                            //SE HACE UN RETRAZO DE UN SEGUNDO
                            await Task.Delay(1000);
                            //SE CIERRA LA PAGINA "PaginaRegistro"
                            await Navigation.PopAsync();
                        }
                    }
                    break;
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //METODO QUE RETORNA A LA CLASE "ConfiguracionAdminViewModel.cs" EL NUMERO DEL ITEM SELECCIONADO EN EL PICKER "nivelusuarioPicker"
        private void OnSelectedNivelUsuarios(object sender, EventArgs e)
        {
            //---------------------------------------NOTA---------------------------------------
            /*PARA CONSULTAR O MODIFICAR LOS NIVELES DE USUARIOS DIRIJASE A LA SUB CLASE "Usuarios"
             DENTRO DE LA CLASE "Tablas.cs". En ella se encontrara con una funcion llamada
             "NivelUsuarioLista" la cual retornara un lista del tipo string*/
            //----------------------------------------------------------------------------------
            //CREAMOS UN OBJETO DEL TIPO "Picker" Y LO ENLAZAMOS "nivelusuarioPicker" (ubicado
            //EN "PaginaRegistro.xaml")
            Picker picker = sender as Picker;
            //EVALUAMOS EL VALOR DE LA POSICION DE LA OPCION SELECCIONADA
            switch (picker.SelectedIndex)
            {
                //NIVEL BAJO (1)
                case 0:
                    ConexionDatos.nivelusuario = 1;
                    break;
                //NIVEL MEDIO (5)
                case 1:
                    ConexionDatos.nivelusuario = 5;
                    break;
                //NIVEL ALTO (8)
                case 2:
                    ConexionDatos.nivelusuario = 8;
                    break;
            }
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //LLAMADO A LA PAGINA DE INFORMACION
        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PaginaInformacionRegistro());
        }
    }
}