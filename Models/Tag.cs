namespace IJustWatched.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagText { get; set; }

        public override string ToString()
        {
            return $"#{TagText.ToLower()}";
        }
    }
}