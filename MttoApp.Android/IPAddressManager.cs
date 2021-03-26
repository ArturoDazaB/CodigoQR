using MttoApp.Servicios;
using System;
using System.Net;

[assembly: Xamarin.Forms.Dependency(typeof(MttoApp.Droid.IPAddressManager))]

namespace MttoApp.Droid
{
    internal class IPAddressManager : IIPAddressManager
    {
        //FUNCION QUE RETORNA LA DIRECCION IP DEL DISPOSITIVO MOVIL
        public string GetIPAddress()
        {
            //SE APERTURA UN CICLO TRY... CATCH
            try
            {
                //SE CREA E INICIALIZA UN VECTOR DE DIRECCIONES IP CON UNA LISTA DE DIRECCIONES IP DEL DISPOSITIVO
                IPAddress[] direcciones = Dns.GetHostAddresses(Dns.GetHostName());

                //SE EVALUA QUE EL VECTOR NO ESTE VACIO Y QUE LA PRIMERA POSICION NO SE ENCUENTRE VACIA
                if (direcciones != null && direcciones[0] != null)
                {
                    //DE NO SER NULO SE RETORNA LA DIRECCION IP
                    return direcciones[0].ToString();
                }
            }
            //DE OCURRIR ALGUNA EXCEPCION EN EL PROCESO DE INICIALIZACION DEL VECTOR NOS DIRIGIMOS A LA SECCION CATCH
            catch (Exception ex) when (ex is ArgumentNullException ||
                                      ex is ArgumentOutOfRangeException ||
                                      ex is System.Net.Sockets.SocketException ||
                                      ex is ArgumentException)
            {
                //SE RETORNA UN MENSAJE INDICATIVO DE ERROR
                return "ERROR";
            }

            //SI NO SE OBTUVO NINGUN RESULTADO Y NO OCURRIO NINGUNA EXCEPCION SE RETORNA UN TEXTO VACIO
            return "";
        }
    }
}