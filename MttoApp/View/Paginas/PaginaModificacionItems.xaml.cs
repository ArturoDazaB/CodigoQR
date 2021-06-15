using MttoApp.Model;
using MttoApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaModificacionItems : Rg.Plugins.Popup.Pages.PopupPage
    {
        //DECLARACION DE VARIABLES GLOBALES DE LA CLASE
        private RegistroTableroViewModel DatosPagina;

        //===========================================================================================================================================
        //===========================================================================================================================================
        public PaginaModificacionItems(Personas persona, Usuarios usuario, ItemTablero item)
        {
            InitializeComponent();
            //SE ENLAZA CON LA CLASE "PaginaInformacionViewModel"
            //NOTA: SE UTILIZA ESTE VIEWMODEL DEBIDO A QUE LA NATURALEZA DE ESTA
            //PAGINA ES DE TIPO POPUP.
            DatosPagina = (RegistroTableroViewModel)(BindingContext = new RegistroTableroViewModel(persona, usuario, item));
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
        }

        //===========================================================================================================================================
        //===========================================================================================================================================
        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            //return base.OnBackgroundClicked();
            return false;
        }

        //===========================================================================================================================================
        //===========================================================================================================================================
        //CLAURUSA DE LA PAGINA MEDIANTE BOTON
        private async void OnClose(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync();
        }

        private async void OnModifyItem(object sender, EventArgs e)
        {
            //VARIABLE QUE RECIBIRA LA RESPUESTA AL METODO DE GUARDADO
            bool response = false;

            if (await DisplayAlert("Alerta",
                DatosPagina.ModificacionItemsMethodMessage,
                DatosPagina.AffirmativeText,
                DatosPagina.NegativeText))
            {
                //SE VERIFICAN QUE LOS CAMPOS DEL NUEVO ITEM A REGISTRAR NO SE
                //ENCUENTREN VACIOS
                if (!string.IsNullOrEmpty(entryDescripcion.Text) &&
                    !string.IsNullOrEmpty(entryCantidad.Text))
                {
                    //SE ACTIVA EL ACTIVITY INDICATOR MIENTRAS SE EJECUTA DE MANERA ASINCRONA LA FUNCION REGISTRARTABLERO
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    //SE INICIA EL LLAMADO ASINCRONO DE LA FUNCION "ModificacionRegistroItems"
                    await Task.Run(async () =>
                    {
                        //RECIBIMOS LA LISTA RETORNADA EN EL LLAMADO DE LA FUNCION "ModificacionRegistroItems"
                        response = await DatosPagina.ModificarRegistroItem();
                        //DETENEMOS EL ACTIVITY INDICATO
                        ActivityIndicator.IsRunning = false;
                    });

                    //VOLVEMOS INVISIBLE EL ACTIVITY INDICATOR
                    ActivityIndicator.IsVisible = false;

                    //EVALUAMOS EL RESULTADO OBTENIDO EN EL LLAMADO A LA FUNCION "ModificacioNRegistroItems"
                    if (response) //TRUE => SE MODIFICO EL ITEM SELECCIONADO
                    {
                        //INICIALIZACION DEL PATRON "Publisher - Subscriber" (SEND en este caso) DONDE
                        //ENVIAREMOS LA LISTA DE ITEMS DEL TABLERO AL QUE PERTENECE EL ITEM QUE CABAMOS 
                        //DE MODIFICAR
                        MessagingCenter.Send<PaginaModificacionItems, List<ItemTablero>>(this, App.ItemUpdate, DatosPagina.Items);
                        //CERRAMOS LA PAGINA POP-UP "PaginaModificacionItems"
                        await Navigation.PopAllPopupAsync();
                    }
                    else //FALSE => NO SE MODIFICO EL ITEM SELECCIONADO
                    {
                        //SE NOTIFICA POR PANTALLA AL USUARIO UN MENSAJE INDICANDO EL ERROR DE LA MODIFICACION.
                        DatosPagina.MensajePantalla(DatosPagina.HttpErrorResponse);
                    }
                }
                //UNO DE LOS DOS CAMPOS (O LOS DOS) SE ENCUENTRA NULO O VACIO
                else
                {
                    //SE NOTIFICA POR PANTALLA AL USUARIO CUAL DE LAS DOS PROPIEDADES SE ENCUENTRA VACIA
                    DatosPagina.MensajePantalla(DatosPagina.AddItemMessage);
                }
            }
        }
    }
}