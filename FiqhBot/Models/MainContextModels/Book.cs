namespace FiqhBot.Models.MainContextModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }           
        public string Author { get; set; }       
        public string Madhhab { get; set; }      

        public List<Chapter> Chapters { get; set; }
    }
}
