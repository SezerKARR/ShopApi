namespace Shop.Api.Controllers;

using Application.Dtos.Product;
using Application.Services;
using Domain.Models.Request;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController] 
public class ProductController: Controller {
    readonly ILogger<ProductService> _logger;
    readonly IProductService _productService;
    public ProductController(IProductService productService, ILogger<ProductService> logger) {
        _logger = logger;
        _productService = productService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ReadProductDto>>> GetAllProductsAsync() {
       var products = await _productService.GetProductsAsync();
       if (products.Success)
       {
           return Ok(products.Resource);
       }
       return BadRequest(products.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadProductDto>> GetProductById(int id,int includes=-1) {
        Console.WriteLine("product fetch by id");
        var product = await _productService.GetProductByIdAsync(id,includes);
        if (product.Success)
        {
            return Ok(product.Resource);
        }
        return BadRequest(product.Message);
    }
   
    [HttpPost]
    public async Task<ActionResult<ReadProductDto?>> CreateProduct([FromBody]CreateProductDto createProductDto) {
        Console.WriteLine(createProductDto.CategoryId);
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            var response= await _productService.CreateProductAsync(createProductDto);
            if (response.Resource!=null)
            {
               var product = response.Resource;
    
              
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id },
                product);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex + "Internal server error");
        }
        
    }
      /// <summary>
        /// Belirtilen ID'ye sahip ürünü ilişkili verileriyle getirir.
        /// </summary>
        /// <param name="id">Ürün ID'si.</param>
        /// <param name="includes">İlişkili verileri belirtir (Flags enum: None, Category, Brand, Comments vb.).</param>
        /// <returns>Ürün bilgisi.</returns>
        /// <response code="200">Ürün başarıyla bulundu ve döndürüldü.</response>
        /// <response code="404">Belirtilen ID ile ürün bulunamadı.</response>
        /// <response code="500">Ürün alınırken bir sunucu hatası oluştu.</response>
        [HttpGet("idinc/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       

        public async Task<ActionResult<ReadProductDto>> GetProductById(int id,  [FromQuery, SwaggerParameter("Bitwise flags: 1=Category, 2=Brand, 4=CreatedByUser, etc. Combine like: 3 = Category | Brand")] ProductIncludes includes = ProductIncludes.None) // includes artık [FromQuery] ile alınır
        {
            _logger.LogInformation("Getting product by ID: {ProductId} with includes: {Includes}", id, includes);
            // Servis katmanı artık ProductIncludes enum'ını doğrudan alabilir.
            var response = await _productService.GetProductByIdIncAsync(id, includes);

            if (!response.Success)
            {
                // Servis katmanı, ürün bulunamadığında bunu belirtmeli.
                // Burada varsayımsal bir "NotFound" mesajı kontrolü yapıyoruz veya Resource null mu diye bakıyoruz.
                // En iyisi, servis yanıtında özel bir durum kodu/flag olmasıdır.
                bool isNotFound = response.Resource == null; // VEYA response.ErrorCode == ErrorCodes.NotFound gibi

                if (isNotFound)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found.", id);
                    // Ürün bulunamazsa NotFound (404) döndürülür.
                    return NotFound();
                }

                // Diğer başarısızlık durumları sunucu hatası olarak kabul edilir.
                _logger.LogError("Failed to get product by ID {ProductId}: {Message}", id, response.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ürün alınırken bir sunucu hatası oluştu.");
            }

            // Başarılı durumda OK (200) ve ürün bilgisi döndürülür.
            return Ok(response.Resource);
        }
    
    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<List<ReadProductDto>>> GetProductsByCategory([FromRoute] int categoryId,int includes) {
        var products = await _productService.GetProductByCategoryIdAsync(categoryId,includes);
        if (products.Success)
        {
            return Ok(products.Resource);
        }
        return BadRequest(products.Message);
    }
    [HttpPost("upload")]
    public async Task<ActionResult<ReadProductDto>> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath = path });
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReadProductDto>> DeleteProduct([FromRoute] int id) {
        Console.WriteLine("Delete Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _productService.DeleteProductAsync(id);
        if (response.Success)
        {
            var product = response.Resource;
            return Ok(product);
        }
        return BadRequest(response.Message);


    }
    [HttpPost("by-filters")]
    public async Task<ActionResult<List<ReadProductDto>>> GetProductsByFilterValues([FromBody] ProductFilterRequest productFilterRequest) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await _productService.GetFilteredProducts(productFilterRequest);
            if (response.Success)
            {
                var products = response.Resource;
                return Ok(products);
            }
            return BadRequest(new { message = response.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching products", details = ex.Message });
        }
    }

    
    
}