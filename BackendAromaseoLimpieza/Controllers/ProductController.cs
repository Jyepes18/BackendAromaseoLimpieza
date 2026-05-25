using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace BackendAromaseoLimpieza.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var result = await _productService.CreateProduct(product);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
        var result = await _productService.GetProductById(id);
        return Ok(result);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        var result = await _productService.UpdateProduct(product);
        return Ok(result);
    }

    [HttpPost]
    [Route("page/{page}/{pageSize}")]
    public async Task<IActionResult> GetProducts([FromRoute] int page, [FromRoute] int pageSize, [FromBody] ProductFilter filter)
    {
        var result = await _productService.GetProducts(page, pageSize, filter);
        return Ok(result);
    }
}
