using System.Collections.Generic;

namespace MttoApp.Model
{
    internal class QueryAdminModel
    {
        //TITULO DE LA SECCION DE BUSQUEDA
        public string Titulo { get; set; }

        //LISTA CON TODAS LAS OPCIONES DE BUSQUEDA DISPONIBLE
        public List<string> Opciones { get; set; }

        public List<QueryAdminModel> GetConfiguracion()
        {
            //SE INSTANCIA LA LISTA
            List<QueryAdminModel> Lista = new List<QueryAdminModel>();

            //SE LLENA LA LISTA CON LOS VALORES NECESARIOS
            Lista.Add
                (new QueryAdminModel()
                {
                    Titulo = "Busqueda",
                    Opciones = new List<string>() 
                    { 
                        "ID",               //=> OPCION CONSULTA POR ID
                        "Numero de Ficha",  //=> OPCION CONSULTA POR NUMERO DE FICHA
                        "Nombre",           //=> OPCION CONSULTA POR NOMBRE
                        "Apellido",         //=> OPCION CONSULTA POR APELLIDOS
                        "Usuario",          //=> OPCION CONSULTA POR NOMBRE DE USUARIO
                    },
                });

            //SE RETORNA LA LISTA
            return Lista;
        }
    }
}