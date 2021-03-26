namespace MttoApp.Servicios
{
    public interface IPicture
    {
        void SavePicture(string filename, byte[] imgdata);
    }
}