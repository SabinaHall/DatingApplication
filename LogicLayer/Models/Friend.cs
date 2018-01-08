namespace DatingApp.Models
{
    //Friend-tabellen i databasen. Den har två st User-id så man vet vem som har börjat följa vem. 
    public class Friend
    {
        public int Id { get; set; }
        public virtual User From { get; set; }
        public virtual User To { get; set; }
    }
}