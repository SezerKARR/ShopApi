namespace Shop.Api.Controllers;

using Application.Dtos.Comment;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CommentController(ICommentService commentService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<ReadCommentDto>>> GetAllCommentsAsync() {
        var response = await commentService.GetCommentsAsync();
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadCommentDto>> GetCommentByIdAsync([FromRoute] int id) {
        var response = await commentService.GetCommentByIdAsync(id);
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }


    [HttpPost]
    public async Task<ActionResult<ReadCommentDto>> CreateCommentAsync([FromForm] CreateCommentDto createCommentDto) {
        if (!ModelState.IsValid) { return BadRequest(ModelState); }
        try
        {
            var response = await commentService.CreateCommentAsync(createCommentDto);
            if (response.Success)
            {
                var comment = response.Resource;
                if (comment == null) { return BadRequest("comment Null"); }
                return CreatedAtAction("GetCommentById", new
                {
                    id = comment.Id
                }, comment);
            }
            else { return BadRequest(response.Message); }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex + "Internal server error");
        }
    }


}