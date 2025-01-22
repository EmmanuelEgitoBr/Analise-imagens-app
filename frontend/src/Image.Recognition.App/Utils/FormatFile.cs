namespace Image.Recognition.App.Utils
{
    public class FormatFile
    {
        public static string SetFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);

            return $"imagem_base{fileExtension}";
        }
    }
}
