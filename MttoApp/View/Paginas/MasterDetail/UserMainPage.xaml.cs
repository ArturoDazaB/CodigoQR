using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MttoApp.Model;
using MttoApp.View.Paginas.MasterDetail;

namespace MttoApp.View.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserMainPage : MasterDetailPage
    {
        private UltimaConexion UltimaConexion, UltimaConexionAux;

        //CONSTRUCTOR DE LA CLASE "UserMainPage" (APP STAND ALONE)
        //NOTA: ESTE CONSTRUCTOR ES INVOCADO CUANDO LA APLICACION TRABAJA STAND ALONE
        public UserMainPage(Personas Persona, Usuarios Usuario)
        {
            InitializeComponent();

            //====================================================================
            //====================================================================
            //SE GENERA UN NUEVO REGISTRO EN LA TABLA UltimaConexion

            UltimaConexion = new UltimaConexion().NewUltimaConexion(Persona, Usuario);

            //====================================================================
            //====================================================================
            //SE GENERA LA APAERTURA DE LA BASE DE DATOS
            using (SQLiteConnection connection = new SQLiteConnection(App.FileName))
            {
                //CREACION E INICIALIZACION DE VARIABLES USADAS DENTRO DE LA CONEXION CON LA BASE DE DATOS
                List<UltimaConexion> UltimaConexionUsuario = new List<UltimaConexion>();

                //DE NO EXISTIR SE CREA LA TABLA "UltimaConexion"
                connection.CreateTable<UltimaConexion>();

                //INSERTAMOS EL NUEVO REGISTRO
                connection.Insert(UltimaConexion);

                //RECORREMOS CADA UNO DE LOS REGISTRO DENTRO DE LA TABLA "UltimaConexion"
                foreach (UltimaConexion registro in connection.Table<UltimaConexion>().ToList())
                {
                    //EVALUAMOS SI LA CEDULA DEL REGISTRO EN EL QUE NOS ENCONTRAMOS ES IGUAL A LA CEDULA DEL USUARIO QUE ACABA DE INGRESAR
                    if (registro.Cedula == Usuario.Cedula)
                    {
                        //SI LA CEDULA DEL REGISTRO ES IGUAL A LA CEDULA DEL USUARIO LOGGEADO SE AGREGA A LA LISTA "UltimaConexionUsuario"
                        UltimaConexionUsuario.Add(registro);
                    }
                }

                //EVALUAMOS LA LISTA "UltimaConexionUsuario"
                if (UltimaConexionUsuario.Any() &&  //SE EVALUA SI HAY REGISTROS DENTRO DE LA LISTA
                    UltimaConexionUsuario.Count > 1)    //SE EVALUA SI HAY MAS DE UN REGISTRO DENTRO DE LA LISTA
                {
                    //CREAMOS E INICIALIZAMOD UNA VARIABLE CONTADOR
                    int cont = 0;
                    //RECORREMOS TODOS LOS REGISTROS DENTRO DE LA LISTA "UltimaConexionUsuario"
                    foreach (UltimaConexion registro in UltimaConexionUsuario)
                    {
                        //SI NOS ENCONTRAMOS EN LA PENULTIMA POSICION O EL PENULTIMO REGISTRO DETENEMOS EL RECORRIDO
                        //TOMAMOS LA INFORMACION DE ESE REGISTRO Y CERRAMOS EL CICLO
                        if (cont == (UltimaConexionUsuario.Count() - 2))
                        {
                            //LA VARIABLE "UltimaConexionAux" ALMACENA ESTE REGISTRO Y LO ENVIA COMO PARAMETRO PARA LA PAGINA "DetailPage.xaml.cs"
                            UltimaConexionAux = new UltimaConexion().NewUltimaConexion(registro);
                            //PARAMOS EL CICLO
                            break;
                        }
                        //EL CONTADOR CRECE EN UNA UNIDAD
                        cont++;
                    }
                }
                else
                {
                    //NO EXISTE NINGUN REGISTRO DEL USUARIO LOGGEADO DENTRO DE LA TABLA "UltimaConexion", SE ENVIA COMO PARAMETRO
                    //LA VARIABLE "UltimaConexionAux".
                    UltimaConexionAux = new UltimaConexion().NewUltimaConexion(UltimaConexion);
                }
            }

            //SE INDICAN E INICIALIZAN QUIENES VAN A SER LAS PAGINAS MasterPage y DetailPage, ADEMAS DE ENVIAR LOS PARAMETROS
            //SOLICITADOS AL CONSTRUCTOR DE CADA PAGINA

            //SE NOTIFICA A LA APLICACION QUE LA PAGINA ORIGEN DE LAS PAGINAS "MasterPage.xaml.cs" Y "DetailPage.xaml.cs"
            App.MasterDetail = this;

            //SE INDICA E INVOCA LAS PAGINAS QUE FUNCIONARAN COMO "MasterPage.xaml.cs" Y "DetailPage.xaml.cs"
            this.Master = new MasterPage(Persona, Usuario, UltimaConexionAux);
            this.Detail = new NavigationPage(new DetailPage(Persona, Usuario, UltimaConexionAux));

            //SE GENERA EL MENSAJE "Bienvenido" AL MOMENTO DE INGRESAR
            Toast.MakeText(Android.App.Application.Context, "Bienvenido " + Usuario.Username, ToastLength.Short).Show();
        }

        //CONSTRUCTOR DE LA CLASE "UserMainPage" (APP CONSUMO DE API)
        //NOTA: ESTE CONSTRUCTOR ES INVOCADO CUANDO LA APLICACION TRABAJA MEDIANTE EL CONSUMO DE APIS
        public UserMainPage(Personas Persona, Usuarios Usuario, DateTime UltimaFechaIngreso)
        {
            InitializeComponent();
            //SE INDICAN E INICIALIZAN QUIENES VAN A SER LAS PAGINAS MasterPage y DetailPage, ADEMAS DE ENVIAR LOS PARAMETROS
            //SOLICITADOS AL CONSTRUCTOR DE CADA PAGINA =>

            //SE NOTIFICA A LA APLICACION QUE LA PAGINA ORIGEN DE LAS PAGINAS "MasterPage.xaml.cs" Y "DetailPage.xaml.cs"
            App.MasterDetail = this;

            //SE INDICA E INVOCA LAS PAGINAS QUE FUNCIONARAN COMO "MasterPage.xaml.cs" Y "DetailPage.xaml.cs"
            this.Master = new MasterPage(Persona, Usuario, UltimaFechaIngreso);
            this.Detail = new NavigationPage(new DetailPage(Persona, Usuario, UltimaFechaIngreso));

            //SE GENERA EL MENSAJE "Bienvenido" AL MOMENTO DE INGRESAR
            Toast.MakeText(Android.App.Application.Context, "Bienvenido " + Usuario.Username, ToastLength.Short).Show();
        }
    }
}