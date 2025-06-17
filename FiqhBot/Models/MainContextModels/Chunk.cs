using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pgvector; // برای Vector


namespace FiqhBot.Models.MainContextModels
{
    public class Chunk
    {
        public string Id { get; set; }               // مثلاً: tahlil_khamenei_255_1
        public string ContentText { get; set; }      // متن قطعه
        public string[] Tags { get; set; }           // تگ‌ها برای جستجوی موضوعی
        public Vector Embedding { get; set; }       // بردار معنایی (vector(768))

        public int MasalehId { get; set; }
        public Masaleh Masaleh { get; set; }
    }
}
