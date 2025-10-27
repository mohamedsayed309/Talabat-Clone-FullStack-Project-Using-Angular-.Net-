using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        ///private readonly IGenericRepository<Product> _productsRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandRepo;
        ///private readonly IGenericRepository<ProductCategory> _categoryRepo;

        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(
            ///IGenericRepository<Product> productsRepo,
            ///IGenericRepository<ProductBrand>brandRepo,
            ///IGenericRepository<ProductCategory>categoryRepo,
            IProductService productService,
            IMapper mapper
            )
        {
            ///_productsRepo = productsRepo;
            ///_brandRepo = brandRepo;
            ///_categoryRepo = categoryRepo;
            _productService = productService;
            _mapper = mapper;
            
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            

            var products = await _productService.GetProductsAsync(specParams);

            var count = await _productService.GetCountAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]//To imporve Swagger Documentation
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {

            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
