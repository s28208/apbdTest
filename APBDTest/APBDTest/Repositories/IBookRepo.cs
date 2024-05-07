using APBDTest.Models;

namespace APBDTest.Repositories;

public interface IBookRepo
{
    public Task<bool> isBookExists(int idBook);
    public Task<bool> isBookExistsByName(string title);

    
    public Task<BookGenres> getBookGenresById(int idBook);
    public Task addNewBookWithGenres(string title, List<String> genreses);
    public Task addBookWithGenres(string title, List<String> genreses);



}