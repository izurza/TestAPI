using AutoMapper;
using System.IO;
namespace TestAPI.Funciones
{
    public class Base64TypeConverter : ITypeConverter<byte[] , string>
    {
        public string Convert(byte[] source, string destination, ResolutionContext context)
        {
            using (MemoryStream m = new MemoryStream())
            {
                return System.Convert.ToBase64String(source);
            } 
        }
    }
    public class ByteArrayTypeConverter : ITypeConverter<string, byte[]>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
        { 
            return System.Convert.FromBase64String(source);
        }
    }

   
}
