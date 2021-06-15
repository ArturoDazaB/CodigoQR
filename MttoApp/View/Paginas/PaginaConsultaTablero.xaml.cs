using MttoApp.Model;
using MttoApp.View.Paginas.PaginasInformacion;
using MttoApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConsultaTablero : ContentPage
    {
        //=================================================================================================
        //=================================================================================================
        //SE DECLARACION DE OBJETOS
        private RegistroTableroViewModel DatosPagina;

        private Personas Persona;
        private Usuarios Usuario;

        //SE DECLARAN LAS CONSTANTES
        private const int HeightRow = 45;

        //=================================================================================================
        //=================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaConsultaTablero(Personas persona, Usuarios usuario)
        {
            InitializeComponent();

            //SE INSTANCIAN LOS OBJETOS CON LA INFORMACION DEL USUARIO QUE SE ENCUENTRA LOGGEADO
            Persona = new Personas().NewPersona(persona);
            Usuario = new Usuarios().NewUsuario(usuario);

            //SE GENERA LA CONEXION CON LA CLASE VIEWMODEL
            BindingContext = DatosPagina = new RegistroTableroViewModel(Persona, Usuario, false);

            //SE DA SET (FALSE) A LOS FRAMES QUE CONTENDRAN LA INFORMACION DEL TABLERO, LOS ITEMS
            //EL CODIGO QR Y EL BOTON DE ELIMINACION DE TABLERO.
            FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = false;
            BotonEliminar.IsVisible = false;

            //SE DA SET (FALSE) AL ActivityIndicator QUE INDICARA AL USUARIO CUANDO SE ESTA CUMPLIENDO ALGUN PROCESO
            ActivityIndicator.IsVisible = ActivityIndicator.IsRunning = false;
            listViewItems.ItemsSource = null;
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA ESCANEAR (CAMARA)
        async private void Escanear(object sender, EventArgs e)
        {
            //SE CREA E INICIALIZA LA VARIABLE "SearchStatus"
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA FUE SATISFACTORIO (SE OBTUVO UN RESULTADO) => TRUE
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA NO FUE SATISFACTORIO (NO SE OBTUVO UN RESULTADO) => FALSE
            bool SearchStatus = false;

            //===============================================================
            //===============================================================
            //SE NOTIFICA DESDE DONDE SE HACE LA CONSULTA DE TABLERO
            DatosPagina.TipoDeConsulta = "CONSULTA_ESCANER";

            //===============================================================
            //===============================================================
            //SE LLENAN EL OBJETO Opciones CON LAS OPCIONES QUE
            //SE VERAN DISPONIBLES EN LA PANTALLA DE SCANEO
            MobileBarcodeScanningOptions Opciones = new MobileBarcodeScanningOptions()
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
            };

            //===============================================================
            //===============================================================
            //SE CREA LA PAGINA SCAN
            ZXingScannerPage PaginaScan = new ZXingScannerPage(Opciones)
            {
                DefaultOverlayShowFlashButton = true,
                DefaultOverlayBottomText = "Escanea el codigo QR",
            };

            //===============================================================
            //===============================================================
            //SE GENERA UN LLAMADO DE NAVEGACION A LA PAGINA SCAN
            await App.MasterDetail.Navigation.PushModalAsync(PaginaScan);

            PaginaScan.OnScanResult += (result) =>
            {
                //SI SE TIENE UN RESULTADO SE DETIENE EL SCANEO
                PaginaScan.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //SE CIERRA LA PAGINA DE SCANEO
                    await App.MasterDetail.Navigation.PopModalAsync();

                    //SE OCULTA EL FRAME QUE CONTENDRAN LOS RESULTADOS DE BUSQUEDA
                    FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible
                        = FrameCodigoQR.IsVisible = BotonEliminar.IsVisible = false;

                    //SE ACTIVA EL "ActivityIndicator" MIENTRAS DE MANERA ASINCRONA SE REALIZA EL LLAMADO DEL TABLERO
                    ActivityIndicator.IsVisible = true;
                    ActivityIndicator.IsRunning = true;
                    await Task.Run(async () =>
                    {
                        //SE HACE UN LLAMADO AL METODO BUSQUEDA DEL OBJETO "DatosPagina" EL CUAL RETORNARA:
                        //TRUE = SE CONSIGUIO EL TABLERO
                        //FALSE = NO SE CONSIGUIO EL TABLERO O HUBO ALGUN ERROR EN EL PROCESO DE BUSQUEDA
                        SearchStatus = await DatosPagina.BuscarTablero(result.Text);
                        ActivityIndicator.IsRunning = false;
                    });

                    ActivityIndicator.IsVisible = false;

                    if (SearchStatus)
                    {
                        //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                        FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = SearchStatus;
                        //SI EL USUARIO TIENE UN NIVEL SUPERIOR O IGUAL A 5 SE HABILITA EL BOTON DE ELIMINAR
                        if (Usuario.NivelUsuario >= 5)
                            BotonEliminar.IsVisible = SearchStatus;
                        //------------------------------------------------------------------------------------------------
                        //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                        idtablero.Text = DatosPagina.TableroID;
                        sapid.Text = DatosPagina.SapID;
                        filialtablero.Text = DatosPagina.Filial;
                        areatablero.Text = DatosPagina.Area;
                        ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                        listViewItems.ItemsSource = DatosPagina.Items;
                        listViewItems.HeightRequest = (DatosPagina.Items.Count * HeightRow);
                        codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                        //------------------------------------------------------------------------------------------------
                    }
                    else
                    {
                        //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                        FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = SearchStatus;
                        //SI EL USUARIO TIENE UN NIVEL SUPERIOR O IGUAL A 5 SE HABILITA EL BOTON DE ELIMINAR
                        if (Usuario.NivelUsuario >= 5)
                            BotonEliminar.IsVisible = SearchStatus;
                        //SE VUELVE A REAJUSTAR EL TAMAÑO DE LA LISTA DE ITEMS
                        listViewItems.HeightRequest = 0;
                        //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                        //Toast.MakeText(Android.App.Application.Context, DatosPagina.HttpErrorResponse, ToastLength.Long).Show();
                        DatosPagina.MensajePantalla(DatosPagina.HttpErrorResponse);
                    }
                    //=============================================================================
                    //=============================================================================
                });
            };
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA BUSCAR POR ID DEL TABLERO
        async private void ConsultaID(object sender, EventArgs e)
        {
            //SE CREA E INICIALIZA LA VARIABLE "SearchStatus"
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA FUE SATISFACTORIO (SE OBTUVO UN RESULTADO) => TRUE
            //VARIABLE QUE INDICARA SI EL ESTATUS DE BUSQUEDA NO FUE SATISFACTORIO (NO SE OBTUVO UN RESULTADO) => FALSE
            bool SearchStatus = false;

            //===============================================================
            //===============================================================
            //SE NOTIFICA DESDE DONDE SE HACE LA CONSULTA DE TABLERO
            DatosPagina.TipoDeConsulta = "CONSULTA_POR_ID";

            //===============================================================
            //===============================================================
            //SE EVALUA SI EL VALOR DEL ENTRY "entryTableroID" TIENE ALGUN VALOR
            //SE EVALUA QUE SE HAYA SELECCIONADO UNA OPCION DE BUSQUEDA
            if (!string.IsNullOrEmpty(entryTableroID.Text) &&
                PickerOpciones.SelectedIndex > -1)
            {
                //SE OCULTA EL FRAME QUE CONTENDRAN LOS RESULTADOS DE BUSQUEDA
                FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = BotonEliminar.IsVisible = false;

                //SE ACTIVA EL "ActivityIndicator" MIENTRAS DE MANERA ASINCRONA SE REALIZA EL LLAMADO DEL TABLERO
                ActivityIndicator.IsVisible = true;
                ActivityIndicator.IsRunning = true;
                await Task.Run(async () =>
                {
                    //SE HACE UN LLAMADO AL METODO BUSQUEDA DEL OBJETO "DatosPagina" EL CUAL RETORNARA:
                    //TRUE = SE CONSIGUIO EL TABLERO
                    //FALSE = NO SE CONSIGUIO EL TABLERO O HUBO ALGUN ERROR EN EL PROCESO DE BUSQUEDA
                    DatosPagina.OpcionConsultaID = PickerOpciones.SelectedIndex;
                    SearchStatus = await DatosPagina.BuscarTablero(entryTableroID.Text);
                    ActivityIndicator.IsRunning = false;
                });

                //SE DESACTIVA LA VISIBILIDAD DEL OBJETO "ActivityIndicator"
                ActivityIndicator.IsVisible = false;

                //EVALUAMOS SI EL TABLERO SE ENCUENTRA EN LA BASE DE DATOS
                if (SearchStatus)
                {
                    //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                    FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = SearchStatus;
                    //SI EL USUARIO TIENE UN NIVEL SUPERIOR O IGUAL A 5 SE HABILITA EL BOTON DE ELIMINAR
                    if (Usuario.NivelUsuario >= 5)
                        BotonEliminar.IsVisible = SearchStatus;
                    //------------------------------------------------------------------------------------------------
                    //SE LLENAN (MANUALMENTE) CADA UNO DE LOS CAMPOS DE INFORMACION
                    idtablero.Text = DatosPagina.TableroID;
                    sapid.Text = DatosPagina.SapID;
                    filialtablero.Text = DatosPagina.Filial;
                    areatablero.Text = DatosPagina.Area;
                    ultimaconsultatablero.Text = DatosPagina.UltimaFechaConsulta.ToString();
                    listViewItems.ItemsSource = DatosPagina.Items;
                    listViewItems.HeightRequest = (DatosPagina.Items.Count * HeightRow);
                    codigoqrtablero.Source = ImageSource.FromStream(() => new MemoryStream(DatosPagina.CodigoQRbyte));
                    //------------------------------------------------------------------------------------------------
                }
                else
                {
                    //SE CAMBIA LA VISIBILIDAD DEL "FrameResultado" CON LOS RESULTADOS
                    FrameInformacionBasica.IsVisible = FrameItemsTablero.IsVisible = FrameCodigoQR.IsVisible = SearchStatus;
                    //SI EL USUARIO TIENE UN NIVEL SUPERIOR O IGUAL A 5 SE HABILITA EL BOTON DE ELIMINAR
                    if (Usuario.NivelUsuario >= 5)
                        BotonEliminar.IsVisible = SearchStatus;
                    //SE VUELVE A REAJUSTAR EL TAMAÑO DE LA LISTA DE ITEMS
                    listViewItems.HeightRequest = 0;
                    //SE INFORMA AL USUARIO QUE EL TABLERO QUE ACABA DE SER ESCANEADO NO FUE LOCALIZADO
                    //Toast.MakeText(Android.App.Application.Context, DatosPagina.HttpErrorResponse, ToastLength.Long).Show();
                    DatosPagina.MensajePantalla(DatosPagina.HttpErrorResponse);
                }
            }
            //SI EL VALOR DEL ENTRY "entryTableroID" ESTA VACIO O NULO SE NOTIFICA AL USUARIO
            else
            {
                //-----------------------------------------------------------------------------------------
                //DE RETORNAR FALSA O FALLAR ALGUNA DE LAS EVALUACIONES QUE SE REALIZARON EN EL
                //CONDICIONAL ANTERIOR SE RETORNA UN MENSAJE INFORMANDOLE AL USUARIO CUAL DE LAS
                //CONDICIONES PLANTEADAS NO SE CUMPLE
                if (PickerOpciones.SelectedIndex == -1)
                    DatosPagina.MensajePantalla(DatosPagina.PickerSelectedIndex);
                //-----------------------------------------------------------------------------------------
                if (string.IsNullOrEmpty(entryTableroID.Text))
                    DatosPagina.MensajePantalla(DatosPagina.ConsultaID);
                //-----------------------------------------------------------------------------------------
            }
        }

        //=================================================================================================
        //=================================================================================================
        //METODO PARA GUARDAR EL CODIGO QR DENTRO DE LA GELERIA DEL TELEFONO
        private void GuardarCodigoQR(object sender, EventArgs e)
        {
            DatosPagina.SaveImage();
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL SELECCIONAR UN ITEM DE LA LISTA DE ITEMS
        async private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            //IDETIFICAMOS EL OBJETO SELECCIONADO COMO UN OBJETO DE TIPO "ItemTablero"
            var item_2_modify = e.Item as ItemTablero;

            //SI SE SELECCIONA UN ITEM DENTRO DE LA LISTA DE ITEMS SE PROCEDE A DESPLEGAR LAS OPCIONES EXISTENTES
            //PARA ESE REGISTRO DE ITEM EN ESPECIFICO
            var actionSheet = await DisplayActionSheet("Opciones", "Cancelar", null, "Modificar", "Eliminar");

            //TOMAMOS LA OPCION RETORNADA Y LA EVALUAREMOS CON UN SWITCH
            switch (actionSheet)
            {
                //OPCION MODIFICAR (UPDATE)
                case "Modificar":
                    await Task.Run(async () =>
                    {
                        //HACEMOS UN LLAMADO A LA PAGINA TIPO POP UP "PaginaModificacionItems"
                        await Navigation.PushPopupAsync(new PaginaModificacionItems(Persona, Usuario, item_2_modify));
                    });
                    break;
                //OPCION ELIMINAR (DELETE)
                case "Eliminar":
                    //REALIZAMOS UNA PREGUNTA DE CONFIRMACION AL USUARIO PARA ELIMINAR DICHO ELEMENTO
                    var response = await DisplayAlert("Alerta", "¿Está seguro que desea eliminar el/los ítem(s) del tablero?", "Si", "No, regresar");

                    //EVALUAMOS LA RESPUESTA OBTENIDA
                    if (response)
                    {
                        // INTERTE CODIGO DE LLAMADO A LA FUNCION ELIMINAR ITEM EN EL RESPECTIVO VIEWMODEL
                    }

                    break;
            }

            //SE DESELECCIONA LA OPCION SELECCIONADA POR EL USUARIO
            ((ListView)sender).SelectedItem = null;

            //INICIALIZACION DEL PATRON "Publisher - Subscriber" (Subscriptor en este caso)
            MessagingCenter.Subscribe<PaginaModificacionItems, List<ItemTablero>>
            (this, App.ItemUpdate, (p, items) =>
            {
                //VOLVEMOS NULA LA FUENTE DE LA LISTVIEW "listViewItems"
                listViewItems.ItemsSource = null;
                //DIMENSIONAMOS EL TAMAÑO DEL LISTVIEW "listViewItems"
                listViewItems.HeightRequest = (items.Count * HeightRow);
                //ASIGNAMOS LA LISTA DE ITEMS DEL TABLERO CONSULTADO
                //LUEGO DE LA MODIFICACION O ELIMINACION DEL/LOS ITEM(s)
                listViewItems.ItemsSource = items;
            });
        }

        //========================================================================================================
        //========================================================================================================
        //METODO ACTIVADO AL PRESIONAR EL BOTON ELIMINAR
        async private void EliminarTablero(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Alerta", "¿Esta seguro que desea eliminar el tablero junto con toda su informacion?", "Si", "No, regresar");
        }

        //==========================================================================
        //==========================================================================
        //FUNCION PARA LLAMAR A LA PAGINA DE INFORMACION
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new PaginaInformacionConsultaTablero());
        }
    }
}