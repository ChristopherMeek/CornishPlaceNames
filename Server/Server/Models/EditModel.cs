using Domain;

namespace Server.Models
{
    internal class EditModel 
    {
        public Place Place { get; set; }

        public EditModel(Place place)
        {
            Place = place;
        }
    }
}