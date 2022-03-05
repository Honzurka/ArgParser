class Parser {

}

interface ISettings<T> {
    public T GetValue();
}

classIntSetings<T> : ISettings<int> {
    string[] names;
    string description;


    public T GetValue();
}



interface IOptions {
    
}

