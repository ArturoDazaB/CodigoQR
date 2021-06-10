using MttoApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MttoApp
{
    //EN ESTA CLASE INTRODUCIREMOS LAS OPERACIONES LOGICAS
    //QUE LA APLICACION UTILIZA EN TODO EL CODIGO
    public class Metodos
    {
        //================================================================================================================
        //================================================================================================================
        //FUNCION ENCARGADA DE CONVERTIR EN MAYUSCULA LA INICIAL DE LAS PALABRAS QUE CONFORMAN UN TEXTO
        //NOTA: POR LOS MOMENTOS EL METODO REALIZA LA CONVERSION CUANDO EL TEXTO ENVIADO POSEE UN MAXIMO DE 3 PALABRAS
        public static string Mayuscula(string word)
        {
            //Se hace un llamado a la funcion Mayuscula almacenada en la clase Metodos
            //Esta recibe una variable String, detecta donde hay espacios en blanco, y
            //transforma en mayuscula los caracteres asignados a representar la primera
            //letra del nombre la cual puede que esten en minuscula, mientras que al resto
            //de caracteres los transforma en minisculas.

            //SE CREAN E INICIALIZAN LAS VARIABLES LOCALES DEL METODO
            int cuantosespaciosblanco = 0;  //=> VARIABLE QUE IDENTIFICA CUANTOS ESPACIOS EN BLANCO EXISTEN EN LA CADENA DE TEXTO
            int cont = 0;                   //=> VARIABLE QUE CUENTA CUANTOS ESPACIOS EN BLANCO EXISTEN EN LA CADENA DE TEXTOS ENVIADA
            bool flag = false;              //=> VARIABLE QUE FUNCIONA COMO BANDERA Y SE DISPARA (TRUE) CUANDO SE DETECTE AL MENOS UN ESPACIO EN BLANCO
            string resultado = string.Empty;    //=> VARIABLE CADENA DE TEXTO QUE RETORNARA LA CADENA DE TEXTO CON LAS INICIALES DE CADA PALABRA EN MAYUSCULA
            string primero = string.Empty,      //=> VARIABLE CADENA DE TEXTO QUE RECIBIRA LA PRIMERA PALABRA DE LA CADENA DE TEXTO
                   segundo = string.Empty,      //=> VARIABLE CADENA DE TEXTO QUE RECIBIRA LA SEGUNDA PALABRA DE LA CADENA DE TEXTO
                   tercero = string.Empty;      //=> VARIABLE CADENA DE TEXTO QUE RECIBIRA LA TERCERA PALABRA DE LA CADENA DE TEXTO
            char[] first = null,            //=> VARIABLE VECTOR DE CARACTERES QUE DESCOMPONDRA LA PRIMERA PALABRA DE LA CADENA DE TEXTO EN UN VECTOR DE CARACTERES
                   second = null;           //=> VARIABLE VECTOR DE CARACTERES QUE DESCOMPONDRA LA PRIMERA PALABRA DE LA CADENA DE TEXTO EN UN VECTOR DE CARACTERES

            //SE RECORRE LA VARIABLE STRING QUE ES ENVIADA COMO PARAMETROS PARA IDENTIFICAR CUANTOS CARACTERS EN BLANCO EXISTEN
            //ESTO LO LOGRAMOS HACIENDO LLAMADO DEL METODO LOCAL "CuantosEspaciosBlanco"
            cuantosespaciosblanco = Metodos.CuantosEspaciosBlanco(word);

            //SE CREA E INICIALIZA EL VECTOR DE ENTEROS QUE CONTENDRAN LA POSICION EXACTA DE LOS ESPACIOS EN BLANCO DENTRO DE
            //LA VARIABLE DEL TIPO STRING QUE ES ENVIADA COMO PARAMETRO (word)
            int[] posicionespaciosblanco = new int[cuantosespaciosblanco];

            //SE RECORRE LA CADENA DE CARACTERES PARA UBICAR EN QUE POSICION DE LA CADENA DE CARACTERES SE ENCUENTRAN LOS ESPACIOS
            //EN BLANCO QUE FUERON ENCONTRADOS PREVIAMENTE
            for (int i = 0; i < word.Length; i++)
            {
                //SE EVALUA SI EN LA POSICION QUE NOS ENCONTRAMOS EXISTE COMO UN ESPACIO EN BLANCO
                if (word[i] == ' ')
                {
                    //SE DA SET (TRUE) A LA BANDERA => SE ENCONTRO AL MENOS UN ESPACIO EN BLANCO
                    flag = true;
                    //SE GUARDA LA POSICION EN LA CUAL SE ENCUENTRA ESE ESPACIO EN BLANCO
                    posicionespaciosblanco[cont] = i;
                    //SE PROSIGUE A MOVERSE A LA SIGUIENTE POSICION DEL VECTOR DE ENTEROS
                    cont++;

                    //SE EVALUA SI EL CONTADOR YA RECORRIO TODOS LOS ESPACIOS EN BLANCO PREVIAMENTE CONTADOS
                    if (cont == cuantosespaciosblanco)
                    {
                        //DE HABER RECORRIDO TODOS LOS ESPACIOS EN BLANCO SE PROCEDE A CERRAR EL CICLO FOR
                        break;
                    }
                }
            }

            //SI SE ENCONTRO AL MENOS UN ESPACIO EN BLANCO SE PROCEDE A
            if (flag)
            {
                //SE EVALUA SI EL TEXTO INGRESADO TIENE MAS DE UN ESPACIO EN BLANCO
                if (cuantosespaciosblanco >= 1)
                {
                    //SI TIENE MAS DE UN ESPACIO EN BLANCO (USUALMENTE USADO EN NOMBRES) SE PROCEDE A
                    //IDENTIFICAR CUALES SON LAS PALABRAS QUE FORMAN EL TEXTO COMPLETO
                    switch (cuantosespaciosblanco)
                    {
                        //------------------------------------------------------------------------------------------------------------------------------------------
                        //CASO UN SOLO ESPACIO EN BLANCO (USUALMENTE DOS NOMBRES)
                        case 1:
                            //SE CREAN E INICIALIZAN LAS VARIABLES QUE CONTENRAN LA PRIMERA
                            first = new char[(word.Length - (word.Length - posicionespaciosblanco[0]))];

                            //SE RECORRE EL TEXTO COMPLETO
                            for (int i = 0; i < posicionespaciosblanco[0]; i++)
                            {
                                first[i] = word[i];
                            }

                            //TRANSFORMAMOS EL VECTOR DE CARACTERES EN SU EQUIVALENTE STRING
                            primero = new string(first).ToLower();

                            //SE COLOCA LA PRIMERA LETRA DE LA PALABRA EN MAYUSCULA Y EL RESTO EN MINUSCULA
                            primero = char.ToUpper(primero[0]) + primero.Substring(1).ToLower();

                            //PARA LA SEGUNDA PALABRA QUE CONFORMA EL TEXTO SOLO ES NECESARIO TOMAR LA PALABRA RESTANTE Y COLOCAR LA PRIMERA LETRA EN MAYUSCULA
                            segundo = char.ToUpper(word[(posicionespaciosblanco[0] + 1)]) + word.Substring((posicionespaciosblanco[0] + 2)).ToLower();

                            //POR ULTIMO SE JUNTAN LAS DOS VARIABLES EN UNA SOLA CON SU ESPACIO EN BLANCO SEPARADOR
                            resultado = primero + ' ' + segundo;

                            break;
                        //------------------------------------------------------------------------------------------------------------------------------------------
                        //CASO DOS ESPACIOS EN BLANCO
                        case 2:
                            //SE DEBE IDENTIFICAR SI A CONTINUACION DEL SEGUNDO ESPACIO EN BLANCO EXISTE O NO UNA LETRA (O PALABRA)
                            //SE CREAN E INICIALIZAN LAS VARIABLES QUE CONTENRAN LA PRIMERA
                            first = new char[(word.Length - (word.Length - posicionespaciosblanco[0]))];

                            //SE RECORRE EL TEXTO COMPLETO
                            for (int i = 0; i < posicionespaciosblanco[0]; i++)
                            {
                                first[i] = word[i];
                            }

                            //SE VUELVE A RECORRER EL TEXTO COMPLETO
                            for (int i = posicionespaciosblanco[0]; i < posicionespaciosblanco[1]; i++)
                            {
                                second[i] = word[i];
                            }

                            //TRANSFORMAMOS EL VECTOR DE CARACTERES EN SU EQUIVALENTE STRING
                            primero = new string(first).ToLower();
                            segundo = new string(second).ToLower();

                            //SE COLOCA LA PRIMERA LETRA DE LA PALABRA EN MAYUSCULA Y EL RESTO EN MINUSCULA
                            primero = char.ToUpper(primero[0]) + primero.Substring(1).ToLower();
                            segundo = char.ToUpper(segundo[0]) + segundo.Substring(1).ToLower();

                            //PARA LA TERCERA PALABRA QUE CONFORMA EL TEXTO SOLO ES NECESARIO TOMAR LA PALABRA RESTANTE Y COLOCAR LA PRIMERA LETRA EN MAYUSCULA
                            tercero = char.ToUpper(word[(posicionespaciosblanco[1] + 1)]) + word.Substring((posicionespaciosblanco[1] + 2)).ToLower();

                            //POR ULTIMO SE JUNTAN LAS DOS VARIABLES EN UNA SOLA CON SU ESPACIO EN BLANCO SEPARADOR
                            resultado = primero + ' ' + segundo + ' ' + tercero;
                            break;
                            //------------------------------------------------------------------------------------------------------------------------------------------
                    }
                }
            }

            //SI NO SE ENCONTRO NINGUN ESPACIO EN BLANCO
            if (!flag)
                //SE PROCEDE A TOMAR TODO EL TEXTO Y TRANSFORMAR EN MAYUSCULA SOLO LA PRIMERA LETRA
                resultado = char.ToUpper(word[0]) + word.Substring(1);

            //SE RETORNA LA INFORMACION QUE CONTIENE LA VARIABLE "resultado"
            return resultado;
        }

        //================================================================================================================
        //================================================================================================================
        //Busqueda de espacios en blanco
        //FUNCION CREADA PARA VERIFICAR QUE CADENAS DE TEXTO NO TENGAN ESPACIOS EN BLANCO (' ' o " ")
        public static bool EspacioBlanco(string word)
        {
            //SE CREA E INICIALIZA (FALSE) LA BANDERA
            bool flag = false;

            //SE VERIFICA SI LA VARIABLE DEL TIPO TEXTO ENVIADA COMO PARAMETRO NO SE ENCUENTRA VACIA O NULA
            if (!string.IsNullOrEmpty(word))
            {
                //SE RECORRE CADA UNO DE LOS CARACTERES QUE CREAN LA VARIABLE DEL TIPO STRING
                for (int i = 0; i < word.Length; i++)
                {
                    //SI EL CARACTER QUE SE ENCUENTRA EVALUADO ES UN ESPACIO EN BLANCO (IDENTIFICADO COMO " " Ó ' ')
                    if (word[i] == ' ')
                    {
                        //SI EL CARACTER ES UN ESPACIO EN BLANCO SE PROCEDE A DAR SET (TRUE) A LA BANDERA
                        flag = true;
                        //SE DETIENE EL RECORRIDO DE LA VARIABLE DEL TIPO STRING
                        break;
                    }
                }
            }

            //SE RETORNA EL VALOR DE LA BANDERA
            //TRUE => SE CONSIGUIO UN ESPACIO EN BLANCO
            //FALSE => NO SE CONSIGUIO NINGUN ESPACIO EN BLANCO
            return flag;
        }

        //================================================================================================================
        //================================================================================================================
        //FUNCION QUE CUENTA Y RETORNA LA CANTIDAD DE ESPACIOS EN BLANCO EXISTENTES EN UN TEXTO
        public static int CuantosEspaciosBlanco(string word)
        {
            //SE CREA E INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA COMO CONTADOR
            int cont = 0;

            //SE EVALUA QUE EL PARAMETRO DEL TIPO STRING NO ES VACIO O NULO
            if (!string.IsNullOrEmpty(word))
            {
                //SE RECORRE TODO EL TEXTO MEDIANTE UN CICLO CLOR
                for (int i = 0; i < word.Length; i++)
                {
                    //EN CADA POSICION DEL TEXTO SE EVALUA SI EL CARACTER REPRESENTA
                    //UN ESPACIO EN BLANCO (EVALUANDO SI ESA POSICION ES IGUAL A ' ')
                    if (word[i] == ' ')
                    {
                        //SI EL CARACTER ES UN ESPACIO EN BLANCO SE SUMA UNA UNIDAD AL CONTADOR
                        cont++;
                    }
                }
            }

            //SE RETORNA LA CANTIDAD DE VECES QUE SE HA DETECTADO UN ESPACIO EN BLANCO
            return cont;
        }

        //================================================================================================================
        //================================================================================================================
        //METODO QUE VERIFICA CUANDO EXISTE ALGUN CARACTER NO DESEADO
        public static bool Caracteres(string word)
        {
            //SE CREA E INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA COMO UNA BANDERA
            bool flag = false;

            //SE VERIFICA QUE EL PARAMETRO ENVIADO, DEL TIPO STRING, NO SEA VACIO O NULO
            if (!string.IsNullOrEmpty(word))
            {
                //SE RECORRE TODO EL TEXTO MEDIANTE UN CICLO FOR
                for (int i = 0; i < word.Length; i++)
                {
                    //SE EVALUA SI EL CARACTER EN EL QUE SE ENCUENTRAN AL MOMENTO DE RECORRIDO
                    //POSEE O ES ALGUNO DE LOS SIGUIENTES CARACTERES
                    if (word[i] == ('!') || word[i] == ('@') || word[i] == ('#') ||
                        word[i] == ('$') || word[i] == ('%') || word[i] == ('&') ||
                        word[i] == ('(') || word[i] == (')') ||
                        word[i] == ('=') || word[i] == ('/') || word[i] == ('|'))
                    {
                        //DE SER UN CARACTER NO PERMITIDO SE CAMBIA EL VALOR DE LA BANDERA
                        //(TRUE: INDICANDO QUE SE HA ENCONTRADO AL MENOS UN CARACTER)
                        flag = true;
                        //SE DETIENE EL PROCESO DE BUSQUEDA DE CARACTERES
                        break;
                    }
                }
            }

            //SE RETORNA EL VALOR DE LA BANDERA:
            //TRUE => SE ENCONTRO UN CARACTER NO PERMITIDO
            //FALSE => NO SE ENCONTRO NINGUN CARACTER PERMITIDO
            return flag;
        }

        //===============================================================================
        //===============================================================================
        //Busqueda de CEDULA (ID)
        public static bool MatchCedula(List<Personas> personas, string word)
        {
            //SE CREA E INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA COMO BANDERA
            bool flag = false;

            //SE RECORRE CADA UNO DE LOS ITEMS DE LA LISTA QUE ES ENVIADA COMO PARAMETRO
            foreach (Personas persona in personas)
            {
                //SE EVALUA LA CONDICION: LA PROPIEDAD "cedula" DEL ITEM "persona"
                //ES IGUAL AL PARAMETRO DEL TIPO STRING CON EL QUE ES COMPARADO
                if (word == persona.Cedula.ToString())
                {
                    //SE GENERA UN MENSAJE INFORMATIVO POR CONSOLA
                    Mensaje("Se consiguio un registro que contiene el mismo ID que se intenta registrar");
                    //SE CAMBIA EL VALOR DE LA BANERA DEBIDO A QUE SE CONSIGUIO
                    //UN REGISTRO QUE CONTIENE EL ID DEL USUARIO QUE DESEA REGISTRAR
                    flag = true;
                    //SE DETIENE EL CICLO DE BUSQUEDA
                    break;
                }
            }

            //SE RETORNA EL VALOR DE LA BANDERA:
            //TRUE => SE CONSIGUIO UN REGISTRO CON EL MISMO ID DEL USUARIO QUE SE DESEA REGISTRAR
            //FALSE => NO SE CONSIGUIO NINGUN REGISTRO QUE CONTENTA EL MISMO ID DEL USUARIO QUE SE DESEA REGISTRAR
            return flag;
        }

        //===============================================================================
        //===============================================================================
        //Busqueda de USERNAME (UserName)
        //MISMO PROCEDIMIENTO USADO EN EL METODO "MatchCedula"
        public static bool MatchUsername(List<Usuarios> usuarios, string word)
        {
            bool flag = false;

            foreach (Usuarios usuario in usuarios)
            {
                if (word == usuario.Username.ToLower())
                {
                    Mensaje("SE CONSIGUIO UN REGISTRO PREVIO QUE CONTIENE EL MISMO NOMBRE DE USUARIO QUE EL USUARIO QUE SE DESEA REGISTRAR");
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        //===============================================================================
        //===============================================================================
        //Busqueda de Numero de Ficha (NumeroFicha)
        public static bool MatchNumeroFicha(List<Personas> personas, string word)
        {
            //SE CREA E INICIALIZA LA VARIABLE LOCAL QUE FUNCIONARA COMO BANDERA
            bool flag = false;

            foreach (Personas registro in personas)
            {
                if (registro.NumeroFicha.ToString() == word)
                {
                    Mensaje("Se consiguio un registro que contiene el mismo numero de ficha que se intenta registrar");
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        //===============================================================================
        //===============================================================================
        //Busqueda de tableros (TableroID)
        public static bool MatchTableroID(List<Tableros> Registros, string tableroid)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE FUNCIONARA COMO BANDERA
            bool flag = false;

            //SE CADA ITEM PERTENECIENTE A LA LISTA "Registros" QUE ES ENVIADA COMO PARAMETRO
            foreach (Tableros tablero in Registros)
            {
                //SE VERIFICA QUE LA PROPIEDAD "TableroID" DEL ITEM QUE SE ESTA EVALUANDO
                //ES IGUAL AL PARAMETRO DEL TIPO STRING QUE TAMBIEN ES ENVIADO JUNTO A LA LISTA
                if (tablero.TableroID.ToLower() == tableroid.ToLower())
                {
                    Mensaje("SE CONSIGUIO UN REGISTRO PREVIO QUE CONTIENE EL MISMO ID QUE EL NUEVO TABLERO QUE SE INTENTA REGISTRAR");
                    //SI SE CONSIGUIO UN REGISTRO YA EXISTENTE CON EL ID QUE SE INTENTA REGISTRAR SE DA SET A LA BANERA (TRUE)
                    flag = true;
                    //SE DETIENE EL RECORRIDO DE LA LISTA
                    break;
                }
            }

            //SE RETORNA LA INFORMACION QUE CONTIENE LA BANDERA
            //TRUE => SE CONSIGUIO UN REGISTRO PREVIO QUE CONTIENE EL ID DEL NUEVO TABLERO QUE SE INTENTA REGISTRAR
            //FALSE => NO SE CONSIGUIO NINGUN REGISTRO QUE CONTENGA EL ID DEL NUEVO TABLERO QUE SE INTENTA REGISTRAR
            return flag;
        }

        //===============================================================================
        //===============================================================================
        //Busqueda de tableros (CodigoQRData)
        public static bool MatchCodigoQRData(List<Tableros> registros, string codigoqrdata)
        {
            //SE CREA E INICIALIZA LA VARIABLE QUE FUNCIONARA COMO BANDERA
            bool flag = false;

            //SE CADA ITEM PERTENECIENTE A LA LISTA "Registros" QUE ES ENVIADA COMO PARAMETRO
            foreach (Tableros tablero in registros)
            {
                //SE COMPARA LA PROPIEDAD "CodigoQRData" DE CADA UNO DE LOS ITEMS DE LA LISTA DE REGISTROS
                //CON EL PARAMETRO "codigoqrdata" (EL CUAL VIENE COMO ATRIBUTO DEL TABLERO QUE SE INTENTA REGISTRAR).
                if (tablero.CodigoQRData == codigoqrdata)
                {
                    Mensaje("SE CONSIGUIO UN REGISTRO PREVIO QUE CONTIENE LA MISMA DATA DEL CODIGO QR DEL TABLERO QUE SE INTENTA REGISTRAR");
                    //SI SE CONSIGUIO UN REGISTRO QUE CONTENGA LA MISMA INFORMACION DE CODIGO QR SE DA SET A LA BANDERA
                    flag = true;
                    //SE DETIENE EL RECORRIDO DE LA LISTA DE REGISTRO
                    break;
                }
            }

            //SE RETORNA EL VALOR DE LA BANDERA
            //TRUE => SE CONSIGUIO UN REGISTRO QUE CONTIENE LAS MISMA INFORMACION DEL TABLERO QUE SE INTENTA REGISTRAR
            //FALSE => NO SE CONSIGUIO NINGUN REGISTRO QUE CONTENTA LA MISMA INFORMACION DEL TABLERO QUE SE INTENTA REGISTRAR
            return flag;
        }

        //===============================================================================
        //===============================================================================
        //METODO QUE SEPARA DOS PALABRAS DE UN TEXTO Y RETORNA LA PRIMERA PALABRA
        public static string FirstString(string text)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            int posicion = 0;
            bool flag = false;
            string resultado = string.Empty;

            //CICLO PARA RECORRER TODAS LAS POSICIONES DE LA CADENA DE CARACTERES (STRING)
            for (int i = 0; i < text.Length; i++)
            {
                //SE EVALUA SI LA POSICION DE LA CADENA DE CARACTERES EN LA QUE NOS ENCONTRAMOS ES UN ESPACIO EN BLANCO
                if (text[i] == ' ')
                {
                    //DAMOS SET A LA BANDERA
                    flag = true;

                    //GUARDAMOS LA POSICION EN LA QUE SE ENCUENTRA EL ESPACIO EN BLANCO
                    posicion = i;

                    //DETENEMOS EL RECORRIDO DEL CICLO
                    break;
                }
            }

            //EVALUAMOS LA BANDERA
            if (flag)
            {
                //CREACION E INIZIALIZACION DE LA VARIABLE AUXILIAR
                char[] resultadoarray = new char[posicion];

                //LA BANDERA ESTA ACTIVADA: SE ENCONTRO UN ESPACIO EN BLANCO
                for (int i = 0; i < posicion; i++)
                {
                    //SE CREA UN VECTOR DE CARACTERES PARA TOMAR Y ORGANIZAR TODOS LOS CARACTERES QUE SE ENCUENTRAN ANTES DEL ESPACIO EN BLANCO
                    resultadoarray[i] = text[i];
                }

                //SE TOMA EL VECTOR Y LA VARIABLE RESULTADO RECIBE LA CONVERSION DE CHAR[] A STRING
                resultado = new string(resultadoarray);
            }
            else
            {
                //LA BANERA ESTA DESACTIVADA: NO SE ENCONTRO UN ESPACIO EN BLANCO. SE RETORNA EL MISMO TEXTO
                resultado = char.ToUpper(text[0]) + text.Substring(1);
            }

            return resultado;
        }

        //===============================================================================
        //===============================================================================
        //METODO QUE SEPARA DOS PALABRAS DE UN TEXTO Y RETORNA LA SEGUNDA PALABRA
        public static string SecondString(string text)
        {
            //CREACION E INICIALIZACION DE VARIABLES
            int posicion = 0;
            bool flag = false;
            string resultado;

            //SE DETECTA EN QUE PUNTO DE LA CADENA DE CARACTERES
            for (int i = 0; i < text.Length; i++)
            {
                //SE EVALUA SI LA POSICION EN LA QUE NOS ENCONTRAMOS ES UN ESPACIO EN BLANCO
                if (text[i] == ' ')
                {
                    //SE GUARDA LA POSICION EN LA QUE SE ENCUENTRA EL ESPACIO EN BLANCO
                    posicion = i;
                    //SE DA SET A LA BANDERA
                    flag = true;
                    //SE DETIENE EL CICLO FOR
                    break;
                }
            }

            //SE EVALUA EL ESTADO DE LA BANDERA
            if (flag)
            {
                //SE CONSIGUIO UN ESPACIO EN BLANCO Y SE PROCEDE A CORTAR EL TEXTO Y ELIMINAR TODO CONTENIDO QUE SE ENCUENTRE POSICIONES
                //ANTES DEL ESPACIO EN BLANCL
                resultado = char.ToUpper(text[posicion + 1]) + text.Substring((posicion + 2)).ToLower();
            }
            else
            {
                //NO SE CONSIGUIO NINGUN ESPACIO EN BLANCO (ES DECIR UN SOLO NOMBRE)
                resultado = char.ToUpper(text[0]) + text.Substring(text[1]).ToLower();
            }

            return resultado;
        }

        //===============================================================================
        //===============================================================================

        public static string EncryptString(string text2Encrypt)
        {
            //SE CREAN E INICIALIZAN LAS VARIABLES LOCALES USADAS
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            //SE INICIA EL CICLO TRY...CATCH
            try
            {
                //TRANSFORMAMOS/CONVERTIMOS LAS CLAVES PUBLICAS Y PRIVADAS, ADEMAS
                //DEL TEXTO A ENCRIPTAR, A SU EQUIVALENTE EN VECTOR DE BYTES
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(App.SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(App.PublicKey);
                inputbyteArray = System.Text.Encoding.UTF8.GetBytes(text2Encrypt);

                //PROCESO DE ENCRIPTACION
                using (DESCryptoServiceProvider enc = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, enc.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
            }
            //SI OCURRE ALGUNA EXCEPCION EN ALGUN PROCESO O LLAMADO DE PROCESO EN EL SEGMENTO
            //TRY... EL SEGMENTO CATCH CAPTURARA ESTAS SITACIONES
            catch (Exception ex) when (ex is ArgumentException ||
                                      ex is CryptographicException ||
                                      ex is CryptographicUnexpectedOperationException)
            {
                ToReturn = "\n\nHa ocurrido un error al intentar encriptar el texto: " + ex.Message + "\n\n";
            }

            return ToReturn;
        }

        public static string DecryptString(string text2Decrypt)
        {
            //SE CREAN E INICIALIZAN LAS VARIABLES LOCALES USADAS
            string ToReturn = string.Empty;
            byte[] secretkeyByte = { };
            byte[] publickeybyte = { };
            byte[] inputbyteArray = { };
            MemoryStream ms = null;
            CryptoStream cs = null;

            //SE INICIA EL CICLO TRY...CATCH
            try
            {
                //TRANSFORMAMOS/CONVERTIMOS LAS CLAVES PUBLICAS Y PRIVADAS, ADEMAS
                //DEL TEXTO A ENCRIPTAR, A SU EQUIVALENTE EN VECTOR DE BYTES
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(App.SecretKey);
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(App.PublicKey);
                inputbyteArray = new byte[text2Decrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(text2Decrypt.Replace(" ", "+"));

                //PROCESO DE DESENCRIPTACION
                using (DESCryptoServiceProvider dec = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, dec.CreateDecryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());

                }
            }
            //SI OCURRE ALGUNA EXCEPCION EN ALGUN PROCESO O LLAMADO DE PROCESO EN EL SEGMENTO
            //TRY... EL SEGMENTO CATCH CAPTURARA ESTAS SITACIONES
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is FormatException ||
                                       ex is CryptographicException ||
                                       ex is CryptographicUnexpectedOperationException)
            {
                ToReturn = "\n\nHa ocurrido un error al intentar encriptar el texto: " + ex.Message + "\n\n";
            }

            return ToReturn;
        }

        //===============================================================================
        //===============================================================================
        //MENSAJE POR CONSOLA
        private static void Mensaje(string mensaje)
        {
            //METODO LLAMADO CUANDO SE REQUIERE LA IMPRESION DE UN MENSAJE POR PANTALLA
            Console.WriteLine("\n\n==============================================================");
            Console.WriteLine("==============================================================");
            Console.WriteLine("\n" + mensaje + "\n");
            Console.WriteLine("==============================================================");
            Console.WriteLine("==============================================================\n\n");
        }
    }
}