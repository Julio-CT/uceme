namespace Uceme.Foundation.Utilidades
{
    using System.IO;

    internal class GestorFicheros
    {
        public void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
