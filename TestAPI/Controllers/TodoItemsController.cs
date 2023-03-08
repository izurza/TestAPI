using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TestAPI.Funciones;
using TestAPI.Models;
using System.IO;
using System.Drawing.Imaging;

namespace TestAPI.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TodoContext _context;
        //private readonly MailSender _mailSender;

        public TodoItemsController(IMapper mapper, TodoContext context)//, MailSender mailSender)
        {
            _mapper = mapper;
            _context = context;      
            //_mailSender = mailSender;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _context.TodoItems
                .Select(x => _mapper.Map<TodoItemDTO>(x))
                .ToListAsync();
            return Ok(results);
        }

        //GET: recibir foto
        [HttpGet("getPhoto/{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            MemoryStream ms = new MemoryStream(todoItem.Image);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            image.Save("photo.png",ImageFormat.Png);//se guarda en carpeta raiz

            return todoItem is not null ? Ok(_mapper.Map<Models.TodoItemDTO>(todoItem)) : NotFound();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            return todoItem is not null ? Ok(_mapper.Map<Models.TodoItemDTO>(todoItem)) : NotFound();
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem is null)
            {
                return NotFound(todoItem);
            }

            _mapper.Map<TodoItemDTO, TodoItem>(todoItemDTO, todoItem);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(todoItemDTO);
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }
        }



        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem([FromServices] MailSender mailSender, TodoItemDTO todoItemDTO)
        {
            var todoItem = _mapper.Map<Models.TodoItem>(todoItemDTO);
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //mailSender.SendEmailWithMailKitWithOptions();
            //mailSender.SendEmailWithMailKitWithConfiguration;
            //MailSender.SendEmail(todoItem);

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                _mapper.Map<TodoItemDTO>(todoItem));       
        }

        //[Route("api/PostDefaultImage")]
        //[HttpPost]
        //public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItem todoItemDTO)
        //{
        //    var todoItem = _mapper.Map<Models.TodoItem>(todoItemDTO);
        //    _context.TodoItems.Add(todoItem);
        //    await _context.SaveChangesAsync();



        //    return CreatedAtAction(
        //        nameof(GetTodoItem),
        //        new { id = todoItem.Id },
        //        _mapper.Map<TodoItemDTO>(todoItem));
        //}



        //Prueba de uso IFormFile para subir un archivo png
        [HttpPost("fileupload")]
        public IActionResult FileUpload(IFormFile file)
        {

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///Pruebas de propiedades IFormFile/////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Console.WriteLine("Content Disposition ->" + file.ContentDisposition);
            //Console.WriteLine("Content Type ->" + file.ContentType);
            //Console.WriteLine("File Name ->" + file.FileName);
            //Console.WriteLine("Headers ->" + file.Headers);
            //foreach (var header in file.Headers)
            //{
            //    Console.WriteLine(header);
            //}
            //Console.WriteLine("Length ->" + file.Length);
            //Console.WriteLine("Name ->" + file.Name);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            
            Stream stream = file.OpenReadStream();
            byte[] read = new byte[stream.Length];
            for (int i = 0; i<stream.Length; i++)
            {
                read[i]=((byte)stream.ReadByte());
            }
            
            //Parseo de byte[] a string en Base64
            string b64image = System.Convert.ToBase64String(read);

            //Parseo de un string en Base64 a byt[]
            byte[] b = System.Convert.FromBase64String(b64image);
            
            return Ok();
        }


        //Evolucion de prueba de uso IFormFile
        //Se añade un DTO2 para triangular el Type de Imagen entre byte[], string e IFormFile
        [HttpPost("fileupload2")]
        public async Task<IActionResult> FileUpload([FromForm] TodoItemDTO2 todoItemDTO2)
        {

            //Mapeo manual entre DTO2 y DTO
            var todoItemDTO = new TodoItemDTO();
            todoItemDTO.Id = todoItemDTO2.Id;
            todoItemDTO.Name = todoItemDTO2.Name;
            todoItemDTO.IsComplete = todoItemDTO2.IsComplete;

            //Parseo de byte[] a string en Base64
            Stream stream = todoItemDTO2.Image.OpenReadStream();
            byte[] read = new byte[stream.Length];
            for (int i = 0; i < stream.Length; i++)
            {
                read[i] = ((byte)stream.ReadByte());
            }
            todoItemDTO.Image = System.Convert.ToBase64String(read);

            //Mapeo con AutoMapper a Objeto modelo
            var todoItem = _mapper.Map<TodoItem>(todoItemDTO);

            _context.TodoItems.Add(todoItem);

            await _context.SaveChangesAsync();

            //return Ok();
            //retornamos en DTO porque img->string
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                _mapper.Map<TodoItemDTO>(todoItem));
        }



        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem is null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool TodoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        
            new TodoItemDTO()
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    
    }
}
