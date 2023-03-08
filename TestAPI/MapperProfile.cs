using AutoMapper;
using TestAPI.Funciones;

namespace TestAPI
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<string, byte[]>().ConvertUsing(new ByteArrayTypeConverter());
            CreateMap<byte[], string>().ConvertUsing(new Base64TypeConverter());

            CreateMap<Models.TodoItemDTO, Models.TodoItem>();
            CreateMap<Models.TodoItem, Models.TodoItemDTO>();

        }

    }
}
