using System.ComponentModel.DataAnnotations;

namespace APBDTest.Models;

public class BookGenresDTO
{
    [Required] public String Title { get; set; }
    [Required] public List<String> Genres { get; set; }
}