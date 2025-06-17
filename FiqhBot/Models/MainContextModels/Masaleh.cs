namespace FiqhBot.Models.MainContextModels
{
    public class Masaleh
    {
        public int Id { get; set; }
        public string Number { get; set; }           // مثلاً: 255
        public string Title { get; set; }            // عنوان مسأله (اختیاری)

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        public List<Chunk> Chunks { get; set; }
    }
}
