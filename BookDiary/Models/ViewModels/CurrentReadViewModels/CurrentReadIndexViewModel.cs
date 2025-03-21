namespace BookDiary.Models.ViewModels.CurrentReadViewModels
{
    public class CurrentReadIndexViewModel
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public int BookId { get; set; } 
        public string BookName { get; set; }
        public string CoverImageURL { get; set; }   
        public int Pages { get; set; }
        public int TotalPages {  get; set; }    
    }
}
