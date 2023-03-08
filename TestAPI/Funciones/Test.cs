using MimeKit.Text;

namespace TestAPI.Funciones
{
    public  class Test
    {

        public readonly int Id;
        public readonly string Name;
        public readonly string Surname;
        private Test(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private Test(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        public static Test Create(int id, string name)
        {
            return new Test(id, name);
        }

        public static Test Create(int id, string name, string surname)
        {
            return new Test(id, name, surname);
        }
    }    


    //Extender la clase
    public static class TextExtension
    {
        public static string NameParser(this Test test)
        {
            return $"{test.Name}_{test.Id}";
        }
    }
        

    public class Test2
    {
        public void Foo()
        {
            Test.Create(1, "Iñigo").NameParser();
        }
    }

}
