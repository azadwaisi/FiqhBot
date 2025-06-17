namespace FiqhBot.Models.MainContextModels
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }           // مثلاً: احکام وضو

        public int BookId { get; set; }
        public Book Book { get; set; }

        public List<Masaleh> MasalehList { get; set; }
    }
}
