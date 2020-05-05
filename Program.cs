namespace JSonParser
{
    class Program
    {
        static void Main(string[] args)
            => new Loader().LoadJson().GetAwaiter().GetResult();
    }
}
