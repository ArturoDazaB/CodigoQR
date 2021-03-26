using MttoApp.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MttoApp.Model;

namespace MttoApp.View.Paginas.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        //================================================================================
        //================================================================================
        //OBJETOS
        //SE CREAN LAS VARIABLES Y OBJETOS GLOBALES DE LA CLASE
        private Personas Persona;
        private Usuarios Usuario;
        private UltimaConexion UltimaConexion; //=> APP STANS ALONE
        private DateTime UltimaFechaIngreso;//=> APP CONSUMO DE API
        private MasterDetailPageUserInfoViewModel DatosPagina;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (APP STAND ALONE)
        public DetailPage(Personas per, Usuarios usu, UltimaConexion ulti)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Y SE EJECUTA EL ENLACE DE LA CLASE VISTA (VIEW) Y LA CLASE (VIEWMODEL)
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaConexion = new UltimaConexion().NewUltimaConexion(ulti);
            //SE INSTANCIA LOS DATOS DE LA PAGINA MEDIANTE EL LLAMADO DE LA CLASE "MasterDetailPageUserInfoViewModel.cs" 
            //JUNTO CON LOS DATOS DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaConexion);
        }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (APP CONSUMO DE API)
        public DetailPage(Personas per, Usuarios usu, DateTime ulti)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS Y SE EJECUTA EL ENLACE DE LA CLASE VISTA (VIEW) Y LA CLASE (VIEWMODEL)
            Persona = new Personas().NewPersona(per);
            Usuario = new Usuarios().NewUsuario(usu);
            UltimaFechaIngreso = ulti;
            //SE INSTANCIA LOS DATOS DE LA PAGINA MEDIANTE EL LLAMADO DE LA CLASE "MasterDetailPageUserInfoViewModel.cs" 
            //JUNTO CON LOS DATOS DEL USUARIO QUE SE ENCUENTRE LOGGEADO
            BindingContext = DatosPagina = new MasterDetailPageUserInfoViewModel(Persona, Usuario, UltimaFechaIngreso);
        }

        //===================================================================================================================================================
        //===================================================================================================================================================
        //FUNCION QUE INHABILITA EL BackButton NATIVO DE ANDROID
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}