namespace Tucan3D_GameEngine.Utils
{
    public class StringUtils
    {
        public static string GetBetweenStrings(string text, string start, string end)
        {
            int p1 = text.IndexOf(start) + start.Length;
            int p2 = text.IndexOf(end, p1);

            if (end == "") return (text.Substring(p1));
            else return text.Substring(p1, p2 - p1);                      
        }
        
        public static string GetTailOfString(string source, int tailLenght)
        {
            if(tailLenght >= source.Length)
                return source;
            
            return source.Substring(source.Length - tailLenght);
        }
    }
}