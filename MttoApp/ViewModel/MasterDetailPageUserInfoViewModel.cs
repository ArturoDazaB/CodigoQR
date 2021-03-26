using MttoApp.Model;
using System;

namespace MttoApp.ViewModel
{
    internal class MasterDetailPageUserInfoViewModel
    {
        //===============================================================================================
        //===============================================================================================
        //VARIABLES LOCALES

        //===============================================================================================
        //===============================================================================================
        //------------------------------------------OBJETOS LOCALES--------------------------------------
        //CREACION DEL OBJETO MODEL DE LA CLASE "MasterDetailPageUserInfoModel.cs"
        public MasterDetailPageUserInfoModel MasterPageUserInfo;

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (APP STAND ALONE)
        public MasterDetailPageUserInfoViewModel(Personas Persona, Usuarios Usuario, UltimaConexion ultimaconexion)
        {
            //SE INSTANCIA EL OBJETO MODEL DE LA CLASE "MasterDetailPageUserInfoModel.cs" CON LA
            //INFORMACION DEL USUARIO LOGGEADO
            MasterPageUserInfo = new MasterDetailPageUserInfoModel(Persona, Usuario, ultimaconexion);
        }

        //===============================================================================================
        //===============================================================================================
        //CONSTRUCTOR DE LA CLASE (APP CONSUMO DE API)
        public MasterDetailPageUserInfoViewModel(Personas Persona, Usuarios Usuario, DateTime ultimafechaingreso)
        {
            //SE INSTANCIA EL OBJETO MODEL DE LA CLASE "MasterDetailPageUserInfoModel.cs" CON LA
            //INFORMACION DEL USUARIO LOGGEADO
            MasterPageUserInfo = new MasterDetailPageUserInfoModel(Persona, Usuario, ultimafechaingreso);
        }

        //===============================================================================================
        //===============================================================================================
        //-----------------------------------PROPIEDADES DE LA CLASE-------------------------------------
        //TEXTO QUE REPRESENTARA EL NOMBRE DE USUARIO
        public string UserName { get { return MasterPageUserInfo.UserName; } }
        //TEXTO QUE REPRESENTARA EL NOMBRE COMPLETO DEL USUARIO
        public string FullName { get { return MasterPageUserInfo.FullName; } }
        //TEXTO QUE REPRESENTARA LA FECHA Y HORA DE LA CONEXION PREVIA A LA ACTUAL POR PARTE DEL USUARIO
        //NOTA: SI ES LA PRIMERA VEZ QUE EL USUARIO NAVEGA EN LA PLATAFORMA LA FECHA Y HORA MOSTRADAS 
        //SERÁ LA ACTUAL
        public string UltimaConexion { get { return MasterPageUserInfo.UltimaConexion; } }
        //ICONO DE USUARIO (UNICO ICONO DISPONIBLE ACTUALMENTE)
        public string UserIconFile { get { return MasterPageUserInfo.UserIconFile; } }

        //===============================================================================================
        //===============================================================================================
        //-----------------------------------PROPIEDADES DE LA PAGINA------------------------------------
        //-----------------------------------------------------------------------------------------------
        //TAMAÑO DE LAS LETRAS
        public int LabelFontSize { get { return App.LabelFontSize; } }
        public int EntryFontSize { get { return App.EntryFontSize; } }
        public int HeaderFontSize { get { return App.HeaderFontSize; } }
        public int SmallHeaderFontSize { get { return App.SmallHeaderFontSize; } }
    }
}