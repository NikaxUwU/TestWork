using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TestWork.Data;
using TestWork.Model;

namespace TestWork.Controllers
{

    [ApiController]
    [Route("api/images")]

    public class ImagesController : ControllerBase
    {

        private readonly AddDbContext content;

        public ImagesController(AddDbContext content)
        {
            this.content = content;
        }

        //Маршрут для получения всех изображений из таблицы.
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Img>>> GetAllImages()
        {
            var all_images = await content.Images.ToListAsync();

            return Ok(all_images);
        }

        //Маршрут для добавления нового изображения в таблицу.
        [HttpPost("add")]
        public async Task<ActionResult<Img>> AddImage([FromBody] Img image)
        {
            content.Images.Add(image);
            await content.SaveChangesAsync();

            return NoContent();
        }

        //Маршрут для обновления пути у выбранного объекта таблицы бд.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] Img image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            content.Entry(image).State = EntityState.Modified;
            await content.SaveChangesAsync();

            return NoContent();
        }

        //Маршрут для удаления ззаписи из таблицы по айди.
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await content.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            content.Images.Remove(image);
            await content.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
