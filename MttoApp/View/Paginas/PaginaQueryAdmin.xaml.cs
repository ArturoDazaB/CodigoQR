using MttoApp.ViewModel;
using MttoApp.Model;
using MttoApp.View.Paginas.PaginasInformacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaQueryAdmin : ContentPage
    {
        //SE CREAN E INICIALIZAN LAS CONSTANTES
        private const int HeightRow = 45;

        //SE CREAN LOS OBJETOS
        private QueryAdminViewModel ConexionDatos;

        //LISTA QUE ALMACENARA TODOS LOS OBJETOS PERSONAS
        //QUE SON OBTENIDOS LUEGO DE REALIZAR LA BUSQUEDA
        private List<ResponseQueryAdmin> Lista;

        //SE CREAN LAS VARIABLES
        private int Seleccion;
        //========================================================================================================
        //========================================================================================================
        //CONSTRUCTOR
        public PaginaQueryAdmin(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //============================================================================
            //============================================================================
            //REALIZAMOS LA CONEXION CON LA CLASE VIEW MODEL "PaginaQueryViewModel.cs"
            BindingContext = ConexionDatos = new QueryAdminViewModel(persona, usuario);

            //============================================================================
            //============================================================================
            //LLENADO DEL STACKLAYOUT MENU
            BindableLayout.SetItemsSource(MenuBusqueda, ConexionDatos.InfoConfig);

            //PUESTO QUE NO SE HA REALIZADO NINGUNA BUSQUEDA, LA SECCION QUE ORDENARA
            //EN FORMA DE LISTA TODOS LOS RESULTADOS ENCONTRADOS ESTARA OCULTA HASTA QUE
            //SE GENERE UNA NUEVA BUSQUEDA
            FrameListaBusqueda.IsVisible = false;
            //ADEMAS EL OBJETO ActivityIndicator DEBE SER SETTEADO (FALSE)
            ActivityIndicatorBusqueda.IsVisible = false;
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL PRESIONAR EL BOTON BUSCAR
        private async void OnBuscar(Object sender, EventArgs e)
        {
            Lista = new List<ResponseQueryAdmin>();
            FrameListaBusqueda.IsVisible = false;
            ListViewPersonas.ItemsSource = Lista = null;

            //================================================================================
            //================================================================================

            //PRIMERO SE VERIFICA QUE LA ENTRADA "EntryDatos" NO SE ENCUENTRE VACIO

            if (string.IsNullOrEmpty(EntryDatos.Text))
                //DE ESTARLO SE LE NOTIFICARA AL USUARIO
                ConexionDatos.MensajePantalla("Debe ingresar los datos a buscar");
            else
            {
                //SE VUELVE VISIBLE EL ACTIVITY INDICATOR
                ActivityIndicatorBusqueda.IsVisible = true;

                //LUEGO SE EVALUA CUAL ES LA OPCION DE BUSQUEDA SELECCIONADA
                switch (Seleccion)
                {
                    //OPCION BUSQUEDA POR ID
                    case 0:
                        //SE VERIFICA QUE NO EXISTAN ESPACIOS EN BLANCO
                        //AL MOMENTO DE TOMAR EL ID PROPORCIONADO POR
                        //EL USUARIO
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL
                            //METODO "ListasPersonas", PERTENECIENTE A LA CLASE
                            //"Metodos"
                            ActivityIndicatorBusqueda.IsRunning = true;
                            await Task.Run(async () =>
                            {
                                //SE REALIZA EL LLAMADO A LA FUNCION QUE GENERA LA SOLICITUD HTTP Y RETORNA UNA LISTA
                                Lista = await ConexionDatos.ListaPersonasHttpClient(Seleccion, EntryDatos.Text);
                                ActivityIndicatorBusqueda.IsRunning = false;
                            });

                            //SE EVALUA EL ESTADO DE LA LISTA (SI ESTA POPULADA O VACIA)
                            if (Lista == null)
                            {
                                //DE ESTAR VACIA SE LE NOTIFICARA AL USUARIO QUE
                                //NO SE ENCONTRO NINGUN USUARIO QUE RESPONDA A ESE ID (CEDULA)
                                ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                            }
                            else
                            {
                                //DE ENCONTRAR RESULTADOS (LISTA POPULADA) SE PROCEDE A
                                //LLENAR EL LISTVIEW "ListViewPersonas" CON EL RESULTADO
                                //OBTENIDO PREVIAMENTE
                                ListViewPersonas.ItemsSource = Lista;
                                //SE PROCEDE A CONTAR LA CANTIDAD DE PERSONAS QUE POSEE LA LISTA
                                //PARA LUEGO DIMENSIONAR EL TAMAÑO DEL LISTVIEW
                                ListViewPersonas.HeightRequest = (Lista.Count * HeightRow);
                                //POR ULTIMO SE HABILITA LA VISIBILIDAD DEL "FrameListaBusqueda"
                                //QUE ALBERGA A "ListViewPersonas".
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            //SI EL ID INTEGRADO TIENE UN ESPACIO EN BLANCO
                            //SE LE NOTIFICA AL USUARIO MEDIANTE UN MENSAJE DE ALERTA
                            await DisplayAlert("Alerta", ConexionDatos.OnBuscarMethodMessage, ConexionDatos.OkText);
                        break;
                    //OPCION BUSQUEDA POR NUMERO DE FICHA
                    case 1:
                        //SE VERIFICA QUE NO EXISTAN MAS DE DOS ESPACIOS EN BLANCO
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL METODO "ListaPersonas"
                            ActivityIndicatorBusqueda.IsRunning = true;
                            await Task.Run(async () =>
                            {
                                Lista = await ConexionDatos.ListaPersonasHttpClient(Seleccion, EntryDatos.Text);
                                ActivityIndicatorBusqueda.IsRunning = false;
                            });

                            if (Lista == null)
                            {
                                //LA LISTA SE ENCUENTRA VACIA: NO SE ENCONTRO NINGUN RESULTADO
                                ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                            }
                            else
                            {
                                //DE ENCONTRAR RESULTADOS (LISTA POPULADA) SE PROCEDE A LLENAR EL LISTVIEW "ListViewPersonas" CON EL RESULTADO
                                //OBTENIDO PREVIAMENTE
                                ListViewPersonas.ItemsSource = Lista;
                                //SE PROCEDE A CONTAR LA CANTIDAD DE PERSONAS QUE POSEE LA LISTA PARA LUEGO DIMENSIONAR EL TAMAÑO DEL LISTVIEW
                                ListViewPersonas.HeightRequest = (Lista.Count * HeightRow);
                                //POR ULTIMO SE HABILITA LA VISIBILIDAD DEL "FrameListaBusqueda" QUE ALBERGA A "ListViewPersonas".
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                        {
                            //SI EL NUMERO DE FICHA INGRESADO TIENE UN ESPACIO EN BLANCO SE LE NOTIFICA AL USUARIO MEDIANTE UN MENSAJE DE ALERTA
                            await DisplayAlert("Alerta", ConexionDatos.OnBuscarMethodMessage, ConexionDatos.OkText);
                        }
                        break;
                    //OPCION BUSQUEDA POR NOMBRE(S)
                    case 2:
                        //SE VERIFICA QUE NO EXISTAN MAS DE DOS ESPACIOS EN BLANCO
                        //(ESTO TOMANDO EN CUENTA QUE SOLO SE PUEDE REGISTRAR UN MAXIMO
                        //DE DOS NOMBRES POR REGISTRO)
                        if (Metodos.CuantosEspaciosBlanco(EntryDatos.Text) < 2)
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL METODO "ListaPersonas"
                            ActivityIndicatorBusqueda.IsRunning = true;
                            await Task.Run(async () =>
                            {
                                Lista = await ConexionDatos.ListaPersonasHttpClient(Seleccion, EntryDatos.Text);
                                ActivityIndicatorBusqueda.IsRunning = false;
                            });

                            if (Lista == null)
                                ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                ListViewPersonas.HeightRequest = (Lista.Count * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            //SI EL TEXTO INGRESADO TIENE MAS DE DOS ESPACIOS EN BLANCO
                            //SE PROCEDE A NOTIFICARLE AL USUARIO
                            await DisplayAlert("Alerta", ConexionDatos.OnBuscarMethodMessage, ConexionDatos.OkText);
                        break;
                    //OPCION BUSQUEDA POR APELLIDO(S)
                    case 3:
                        //BASICAMENTE CUMPLE LA MISMA RUTINA DE BUSQUEDA
                        //QUE LA OPCION DE BUSQUEDA POR NOMBRE(S)
                        if (Metodos.CuantosEspaciosBlanco(EntryDatos.Text) < 2)
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL METODO "ListaPersonas"
                            ActivityIndicatorBusqueda.IsRunning = true;
                            await Task.Run(async () =>
                            {
                                Lista = await ConexionDatos.ListaPersonasHttpClient(Seleccion, EntryDatos.Text);
                                ActivityIndicatorBusqueda.IsRunning = false;
                            });

                            if (Lista == null)
                                ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                ListViewPersonas.HeightRequest = (Lista.Count * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            await DisplayAlert("Alerta", ConexionDatos.OnBuscarMethodMessage, ConexionDatos.OkText);
                        break;
                    //OPCION BUSQUEDA POR NOMBRE DE USUARIO
                    case 4:
                        //BASICAMENTE CUMPLE LA MISMA RUTINA DE BUSQUEDA
                        //QUE LA OPCION DE BUSQUEDA POR ID
                        if (!Metodos.EspacioBlanco(EntryDatos.Text))
                        {
                            //SE LLENA LA LISTA CON EL RESULTADO ARROJADO POR EL METODO "ListaPersonas"
                            ActivityIndicatorBusqueda.IsRunning = true;
                            await Task.Run(async () =>
                            {
                                Lista = await ConexionDatos.ListaPersonasHttpClient(Seleccion, EntryDatos.Text);
                                ActivityIndicatorBusqueda.IsRunning = false;
                            });

                            if (Lista == null)
                                ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                            else
                            {
                                ListViewPersonas.ItemsSource = Lista;
                                ListViewPersonas.HeightRequest = (Lista.Count * HeightRow);
                                FrameListaBusqueda.IsVisible = true;
                            }
                        }
                        else
                            await DisplayAlert("Alerta", ConexionDatos.OnBuscarMethodMessage, ConexionDatos.OkText);
                        break;
                }
            }

            //SE VUELVE INVISIBLE EL ACTIVITY INDICATOR
            ActivityIndicatorBusqueda.IsVisible = false;
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL SELECCIONAR UN ITEM DE LA LISTA DE RESULTADOS DE BUSQUEDA
        async private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            //TOMAMOS LA INFORMACION DEL ITEM SELECCIONADO
            var Item = e.Item as ResponseQueryAdmin;

            //CREAMOS EL OBJETO QUE RECIBIRA LA RESPUESTA DEL METODO "OnUserSelected"
            InformacionGeneral userinfo = null;

            //REALIZAMOS LA PREGUNTA SOBRE SI SE DESEA MODIFICAR LA INFORMACION DEL USUARIO
            bool response = await DisplayAlert("Mensaje", ConexionDatos.OnItemSelectedMethodMessage, ConexionDatos.AffirmativeText, ConexionDatos.NegativeText);

            if (response)
            {
                //SE VUELVE VISIBLE EL ACTIVITYINDICATOR
                ActivityIndicatorBusqueda.IsVisible = true;

                //SE ACTIVA AL ACTIVITYINDICATOR MIENTRAS SE EJECUTA DE MANERA ASYNCORNA EL CONSUMO DEL METODO "OnUserSelected"
                ActivityIndicatorBusqueda.IsRunning = true;
                await Task.Run(async () =>
                {
                    //SE HACE UN LLAMADO AL METODO "OnUserSelected"
                    userinfo = await ConexionDatos.OnUserSelected(Item);
                    //DESPUES DE RECIBIR LA RESPUESTA SE DESACTIVA EL ACTIVITY INDICATOR
                    ActivityIndicatorBusqueda.IsRunning = false;
                });

                //SE DESACTIVA LA VISIBILIDAD DEL ACTIVITYINDICATOR
                ActivityIndicatorBusqueda.IsVisible = false;

                //SE VERIFICA SI LA INFORMACION RETORNADA POR EL METODO "OnUserSelected" ES NULA O NO
                if (userinfo == null)
                {
                    //LA INFORMACION CONTENIDA EN EL OBJETO "userinfo" ES NULA
                    //SE PROCEDE A NOTIFICA AL USUARIO SOBRE EL ERROR
                    ConexionDatos.MensajePantalla(ConexionDatos.ErrorMessage);
                }
                else
                {
                    //LA INFORMACION CONTENIDA EN EL OBJETO "userinfo" NO ES NULA
                    //SE PROCEDE A REALIZAR EL LLAMADO A LA PAGINA "PaginaConfiguracionAdmin"
                    await App.MasterDetail.Detail.Navigation.PushAsync(new PaginaConfiguracionAdmin(userinfo.Persona, userinfo.Usuario));
                    //SE BORRA LA INFORMACION SUMINISTRADA POR EL USUARIO
                    EntryDatos.Text = string.Empty;
                    //SE LIMPIA LA LISTA
                    ListViewPersonas.ItemsSource = null;
                    //SE ESCONDE LA LISTA
                    FrameListaBusqueda.IsVisible = false;
                }
            }

            //SE LIBERA EL OBJETO SELECCIONADO DENTRO DE LA LISTA DE USUARIOS SOLICITADOS
            ((ListView)sender).SelectedItem = null;
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO CUANDO SE SELECCIONA UNA OPCION DE BUSQUEDA, ESTO CON LA FINALIDAD DE LIMITAR AL USUARIO
        //A USAR EL TECLADO NUMERICO CUANDO SELECCIONE BUSQUEDA POR ID
        private void OnSelectedOpciones(object sender, EventArgs args)
        {
            //============================================
            //============================================
            //Seleccion INDICA QUE OPCION FUE SELECCIONADA
            Picker picker = sender as Picker;
            Seleccion = picker.SelectedIndex;

            if (picker.SelectedIndex < 2)
                EntryDatos.Keyboard = Keyboard.Numeric;
            else
                EntryDatos.Keyboard = Keyboard.Text;

            //PASAMOS EL VALOR DE LA OPCION SELECCIONADA A LA PAGINA VIEWMODEL "QueryAdminViewModel "
            ConexionDatos.OpcionSeleccionada = Seleccion;
        }

        //===============================================================================================================================
        //===============================================================================================================================
        //LLAMADO A LA PAGINA DE INFORMACION
        [Obsolete]
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PaginaInformacionQueryAdmin());
        }
    }
}