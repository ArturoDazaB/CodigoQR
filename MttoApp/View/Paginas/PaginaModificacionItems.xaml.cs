using MttoApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaModificacionItems : Rg.Plugins.Popup.Pages.PopupPage
    {
        //===========================================================================================================================================
        //===========================================================================================================================================
        public PaginaModificacionItems()
        {
            InitializeComponent();
            //SE ENLAZA CON LA CLASE "PaginaInformacionViewModel"
            //NOTA: SE UTILIZA ESTE VIEWMODEL DEBIDO A QUE LA NATURALEZA DE ESTA 
            //PAGINA ES DE TIPO POPUP.
            BindingContext = new RegistroTableroViewModel();
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

    }
}