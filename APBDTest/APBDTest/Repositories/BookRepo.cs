using APBDTest.Models;
using Microsoft.Data.SqlClient;

namespace APBDTest.Repositories;

public class BookRepo : IBookRepo
{
    private readonly IConfiguration _configuration;
    public BookRepo(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> isBookExists(int idBook)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        await connection.OpenAsync();
        command.CommandText = "Select 1 from books where PK = @idBook";
        command.Parameters.AddWithValue("@idBook", idBook);
        var isId = await command.ExecuteScalarAsync();
        if (isId is null) throw new Exception("Book is not exists");
        return true;
        
    }
    public async Task<bool> isBookExistsByName(string title)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        await connection.OpenAsync();
        command.CommandText = "Select 1 from books where title = @titleBook";
        command.Parameters.AddWithValue("@titleBook", title);
        var isId = await command.ExecuteScalarAsync();
        if (isId is null) return false;
        return true;
        
    }

    public async Task<BookGenres> getBookGenresById(int idBook)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "Select books.PK, books.title from books where PK = @idBook";
        command.Parameters.AddWithValue("@idBook", idBook);
        await connection.OpenAsync();
        var reader = command.ExecuteReader();
        int idBookOrder = reader.GetOrdinal("PK");
        int titleOrdinal = reader.GetOrdinal("title");

        List<String> listGenres = new List<string>();

        BookGenres bookGenres = new BookGenres()
        {
            PK = reader.GetInt32(idBookOrder),
            Title = reader.GetString(titleOrdinal)
        };
        
        command.CommandText = "Select name from genres join books_genres on books_genres.FK_genres = genres.PK where books_genres.FK_book = @idBook"
        ;
        command.Parameters.AddWithValue("@idBook", idBook);
        reader = command.ExecuteReader();
        int genresOrdinal = reader.GetOrdinal("name");
        while (reader.Read())
        {
           listGenres.Add(reader.GetString(titleOrdinal)); 
        }

        bookGenres.Genres = listGenres;
        return bookGenres;
    }

    public async Task addNewBookWithGenres(string title, List<String> genreses)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = @"Insert into books VALUES (@title); 
                                    Select @@IDENTITY ";
        command.Parameters.AddWithValue("@title", title);
        await connection.OpenAsync();
        var iDNewBook = await connection.BeginTransactionAsync();
        foreach (var idgenres in genreses)
        {
            command.CommandText = "Insert into books_genres VALUES (@idBook, @idGenres)";
            command.Parameters.AddWithValue("@idBook", iDNewBook);
            command.Parameters.AddWithValue("@idGenres", idgenres);
            await connection.OpenAsync();
        }
    }

    public async Task addBookWithGenres(string title, List<String> genreses)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "Select PK from books where title = @titleBook";
        command.Parameters.AddWithValue("@titleBook", title);
        await connection.OpenAsync();
        var idBook = await command.ExecuteScalarAsync();
        foreach (var idgenres in genreses)
        {
            command.CommandText = "Insert into books_genres VALUES (@idBook, @idGenres)";
            command.Parameters.AddWithValue("@idBook", idBook);
            command.Parameters.AddWithValue("@idGenres", idgenres);
            await connection.OpenAsync();
        }
    }

}