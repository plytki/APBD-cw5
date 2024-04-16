namespace APBD_API_v1.Classes;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public double Weight { get; set; }
    public string FurColor { get; set; }
}

public enum Category
{
    
    Dog,
    Cat
    
}