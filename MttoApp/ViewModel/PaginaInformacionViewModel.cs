namespace MttoApp.ViewModel
{
    public class PaginaInformacionViewModel
    {
        //============================================================================================================
        //============================================================================================================
        //CLASE CREADA PARA MANIPULAR TODA LA CONFIGURACION DE PARAMETROS DE LAS PAGINAS DE INFORMACION

        //============================================================================================================
        //CREACION E INICIALIZACION DE VARIABLES LOCALES
        private string SourceOfCalling = string.Empty;  //=> VARIABLE UTILIZADA PARA IDENTIFICAR DESDE QUE PAGINA DE 
                                                        // CONFIGURACION ("PaginaConfiguracion" y "PaginaConfiguracionAdmin")

        //============================================================================================================
        //============================================================================================================
        //PROPIEDADES DE LA CLASE
        //-----------------------------------------TAMAÑO DE LA FUENTE------------------------------------------------
        public int HeaderFontSize { get { return App.HeaderFontSize; } } //=> TAMAÑO DE TEXTO PARA TITULOS
        public int EntryFontSize { get { return App.EntryFontSize; } } //=> TAMAÑO DE TEXTO PARA ENTRADAS
        public int LabelFontSize { get { return App.LabelFontSize; } } //=> TAMAÑO DE TEXTO PARA ETIQUETAS Y SUBTITULOS

        //--------------------------------------------COLOR DE FONDO--------------------------------------------------
        public string BackGroundColor { get { return App.BackGroundColorPopUp; } } //=> COLOR DE FONDO PAGINAS INFORMACION (POP-UP)
        public string FrameColor { get { return App.FrameColorPopUp; } } //=> COLOR PARA EL MARCO DE LAS PAGINAS INFORMACION (POP-UP)

        //-----------------------------------------------IMAGENES-----------------------------------------------------
        public string CloseButton //=> NOMBRE DEL ARCHIVO (Imagen) QUE REPRESENTARA EL BOTON DE CLAUSURA DE LAS PAGINAS DE TIPO POP-UP
        {
            //https://iconos8.es/icons/set/close-window"
            //Cerrar ventana icon by a target="_blank"
            //href "https://iconos8.es"*/

            get { return "Cerrar24px2.png"; } 
        }

        //------------------------------------------TEXTOS DINAMICOS--------------------------------------------------
        //PROPIEDAD QUE EVALUA DESDE QUE CLASE SE ESTA LLAMANDO PARA LUEGO DECIDIR QUE TEXTO RETORNAR
        public string ListaDatosModificables
        {
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" DE LOS CAMPOS EDITABLES PARA LA CLASE
            //"PaginaInformacionConfiguracion.cs" CUANDO ES INVOCADA DESDE LAS CLASES "PaginaConfiguacion.cs" Y "PaginaConfiguracionAdmin.cs"
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LA CLASE "PaginaInformacionConfiguracion.cs"
            //Y LA CLASE "PaginaInformacionViewModel". ESTA INVOCACION SE REALIZA EN LA CLASE DE DISEÑO "PaginaInformacionConfiguracion.xaml"
            //-----------------------------------------------------------------------------------------------------------------
            get
            {
                //SE CREA E INICIALIZA LA VARIABLE QUE CONTENDRA LA INFORMACION A RETORNAR
                string Lista = string.Empty;

                //SE EVALUA QUE LA VARIABLE GLOBAL "SourceOfCalling" NO SE ENCUENTRE VACIA O NULA
                if (!string.IsNullOrEmpty(SourceOfCalling)) //=> true => "SourceOfCalling" NO ES NULA NI VACIA
                {
                    //EVALUAMOS LA INFORMACION CONTENIDA EN "SourceOfCalling"
                    switch (SourceOfCalling.ToUpper())
                    {
                        //"CONFIGURACION" => "PaginaInformacionConfiguracion" INVOCADA DESDE LA CLASE "PaginaConfiguacion"
                        case "CONFIGURACION":
                            //EN LA PAGINA "PaginaConfiguracion" EL USUARIO QUE SE ENCUENTRE LOGEADO SOLO PODRA MODIFICAR
                            //LOS SIGUIENTES CAMPOS QUE CONTIENEN INFORMACION DEL MISMO USUARIO
                            Lista = "       -Numero telefonico.\n" +
                                    "       -Correo electronico.\n" +
                                    "       -Contraseña.";
                            break;
                        //"CONFIGURACION" => "PaginaInformacionConfiguracion" INVOCADA DESDE LA CLASE "PaginaConfiguacionAdmin"
                        case "CONFIGURACIONADMIN":
                            //EN LA PAGINA "PaginaConfiguracionAdmin" EL USUARIO (ACTUALMENTE ADMINISTRATOR) PODRA MODIFICAR
                            //LOS SIGUIENTES CAMPOS DEL USUARIO QUE HAYA SIDO CONSULTADO PREVIAMENTE EN LA PAGINA "PaginaQueryAdmin"
                            Lista = "       -Nombre(s).\n" +
                                    "       -Apellido(s).\n" +
                                    "       -Fecha de nacimiento.\n" +
                                    "       -Numero telefonico.\n" +
                                    "       -Correo electronico.\n" +
                                    "       -Nivel de Usuario.\n" +
                                    "       -Contraseña.";
                            break;
                    }
                }

                //SE RETORNA LA INFORMACION DE LA LISTA
                return Lista;
            }
        }

        //PROPIEDAD QUE RETIENE LA LISTA DE LOS METODOS DE BUSQUEDA DE USUARIO DENTRO DE LA PAGINA QUERY ADMIn
        public string ListaMetodosDeConsulta
        {
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" DE LAS OPCIONES DE BUSQUEDA/CONSULTA
            //QUE SE ENCONTRARAN DISPONIBLES EN LA PAGINA "PaginaQueryAdmin"
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LA CLASE "PaginaQueryAdmin.xaml.cs"
            //Y LA CLASE "PaginaInformacionViewModel.cs". ESTA INVOCACION SE REALIZA EN LA CLASE DE DISEÑO "PaginaQueryAdmin.xaml"
            //-----------------------------------------------------------------------------------------------------------------
            get
            {   
                return "       -Consulta por ID (Cedula)\n" +
                       "       -Consulta por Ficha\n"+
                       "       -Consulta por Nombre(s)\n"+
                       "       -Consulta por Apellido(s)\n"+
                       "       -Consulta por Usuario";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS ATRIBUTOS DE LA TABLA "ModificacionesUsuarios"
        public string ListaUltimasModificaciones
        {
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" DE LOS CAMPOS QUE SE REGISTRARAN EN LA TABLA
            //"ModificacionesUsuarios" CADA QUE SE GENERE UNA MODIFICACION A LA INFORMACION DE UN USUARIO (MISMA 
            //LISTA PARA LAS PAGINAS "PaginaConfiguracionAdmin" Y "PaginaConfiguracion".
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LAS CLASES "PaginaConfiguracion.xaml.cs" 
            //Y "PaginaConfiguracionAdmin.xaml.cs" Y LA CLASE "PaginaInformacionViewModel.cs". ESTA INVOCACION SE REALIZA EN LA 
            //CLASE DE DISEÑO "PaginaQueryAdmin.xaml".
            //-----------------------------------------------------------------------------------------------------------------
            get
            {
                return "       -ID del usuario que acaba de ser modificado.\n" +
                       "       -ID del usuario que realizo la modificación.\n" +
                       "       -Fecha en la que se realizo la modificación.\n" +
                       "       -Atributo(s)/campo(s) modificado."; ;
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS NIVELES DE USUARIO
        public string ListaNivelesUsuario
        {
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" DE LOS NIVELES DE USUARIO ACTUALMENTE DISPONIBLES
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LA CLASE "PaginaRegistro.xaml.cs" Y LA CLASE
            //"PaginaInformacionViewModel.cs". ESTA INVOCACION SE REALIZA EN LA CLASE DE DISEÑO "PaginaRegistro.xaml"
            //-----------------------------------------------------------------------------------------------------------------
            get
            {
                return "       -Nivel Bajo (0).\n" +
                       "       -Nivel Medio (5).\n" +
                       "       -Nivel Alto (10).\n";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LAS OPCIONES DE METODO DE CONSULTA DE TABLEROS
        public string ListaOpcionesEscaneo
        {   
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" LAS OPCIONES DE CONSULTA PARA CONSULTAR TABLEROS
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LA CLASE "PaginaConsultaTablero.xaml.cs" Y LA
            //CLASE "PaginaInformacionViewModel.cs". ESTA INVOCACION SE REALIZA EN LA CLASE DE DISEÑO "PaginaConsultaTablero.xaml"
            //-----------------------------------------------------------------------------------------------------------------
            get
            {
                return "       -Consulta por escaneo.\n" +
                       "       -Consulta por ID (ID del tablero o ID Sap).";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO (LISTA) LA INFORMACION QUE SE DESPLIEGA EN PANTALLA
        public string ListaInformacionTablero
        {
            //EN ESTA PROPIEDAD SE RETORNA UN TEXTO EN FORMA DE "Lista" LA INFORMACION DEL TABLERO QUE SERA DESPLEGADA
            //-----------------------------------------------------------------------------------------------------------------
            //NOTA: PROPIEDAD QUE ES INVOCADA MEDIANTE UN ENLACE (Binding) HECHO ENTRE LA CLASE "PaginaConsultaTablero.xaml.cs"
            //Y LA CLASE "PaginaInformacionViewModel.cs". ESTA INVOCACION SE REALIZA EN LA CLASE DE DISEÑO 
            //"PaginaConsultaTablero.xaml"
            //-----------------------------------------------------------------------------------------------------------------
            get
            {
                return "       -Codigo del Tablero (ID).\n" +
                       "       -Filial (filial a la que pertenece).\n" +
                       "       -Area (area/zona de la filial).\n" +
                       "       -Ultima consulta del tablero (fecha).\n" +
                       "       -Items del tablero (lista).\n"+
                       "       -Codigo QR asignado al tablero (imagen).";
            }
        }

        //PROPIEDAD QUE RETORNA EN UN TEXTO LOS CARACTERES NO PERMITIDOS
        public string Caracteres { get { return "       " + App.ForbiddenCharacters; ; } }

        //PROPIEDAD QUE RETORNA EL TEXTO QUE FUNCIONARA DE TITULO A LA PAGINA POP UP EMERGENTE
        public string TituloPH { get { return "INFORMACIÓN"; } }

        //============================================================================================================
        //============================================================================================================
        //CONSTRUCTOR DE LA CLASE
        public PaginaInformacionViewModel(string sourceofcalling)
        {
            //IDENFICAMOS DE QUE PAGINA SE ESTA HACIENDO EL LLAMADO DE LA CLASE
            SourceOfCalling = sourceofcalling.ToUpper();
        }

        //============================================================================================================
        //============================================================================================================
        //FUNCION QUE RETORNA EN FORMA DE TEXTO LOS CARACTERES NO PERMITIDOS
        public static string CaracteresNoPermitidos() { return "       " + App.ForbiddenCharacters; }
    }
}