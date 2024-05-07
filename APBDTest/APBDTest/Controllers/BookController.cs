using APBDTest.Models;
using APBDTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APBDTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepo _bookRepo;
    public BookController(IBookRepo bookRepo)
    {
        _bookRepo = bookRepo;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenresByBook(int id)
    {
        if (!await _bookRepo.isBookExists(id))
        {
            return NotFound("Not found book with given id - {id}");
        };
        
        return Ok();
    }
//Nie dziala Task w metodzie, jezeli rozkomentowac to podswieca blad :(
    /*[HttpPost]
    public async Tast<IActionResult> AddBookWitGeenres(BookGenresDTO bookGenresDto)
    {
        if (!await _bookRepo.isBookExistsByName(bookGenresDto.Title))
        {
            await _bookRepo.addNewBookWithGenres(bookGenresDto.Title, bookGenresDto.Genres);
        }
        else
        {
            await _bookRepo.addBookWithGenres(bookGenresDto.Title, bookGenresDto.Genres);
        }

        return Ok();
    }*/
}