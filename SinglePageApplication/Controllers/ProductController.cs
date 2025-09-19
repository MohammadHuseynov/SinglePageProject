using Microsoft.AspNetCore.Mvc;
using SinglePageApplication.ApplicationServices.DTOs;
using SinglePageApplication.ApplicationServices.Services.Contracts;

namespace SinglePageApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApplicationService _productApplicationService;

        public ProductController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }


        #region [- Index() -]
        public ActionResult Index()
        {

            return View();
        }

        #endregion

        #region [- Post() -]
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] PostProductDto dto)
        //{

        //    var postProductDto = new PostProductDto() { Title = dto.Title };
        //    var getResponse = await _productApplicationService.Post(postProductDto);

        //    if (ModelState.IsValid)
        //    {
        //        var postResponse = await _productApplicationService.Post(dto);
        //        return postResponse.IsSuccessful ? Ok() : BadRequest();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        return Conflict(dto);
        //    }

        //    {
        //        return BadRequest();
        //    }
        //}


        // Route: POST /Product/Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto postProductDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResponse = await _productApplicationService.Post(postProductDto);

            if (!serviceResponse.IsSuccessful)
            {

                return BadRequest(new { message = serviceResponse.ErrorMessage });
            }


            return new JsonResult(new { id = serviceResponse.Result });
        }
        #endregion

        #region [- Get() -]
        public async Task<IActionResult> Get(GetByIdProductDto getByIdProductDto)
        {

            var getResponse = await _productApplicationService.GetByIdProduct(getByIdProductDto);
            var response = getResponse.Result;
            if (response is null)
            {
                return Json("NotFound");
            }
            return Json(response);
        }
        #endregion

        #region [- GetAll() -]
        public async Task<IActionResult> GetAll()
        {

            var getAllResponse = await _productApplicationService.GetAllProduct();
            var response = getAllResponse.Result.GetByIdProductDto;
            return Json(response);

        }
        #endregion

        #region [- PUT() -]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PutProductDto putProductDto)
        {

            if (putProductDto.Id == Guid.Empty)
            {
                return BadRequest(new { message = "Product ID is required." });
            }

            var serviceResponse = await _productApplicationService.Put(putProductDto);


            if (!serviceResponse.IsSuccessful)
            {
                return NotFound(new { message = serviceResponse.ErrorMessage });
            }

            return Ok();
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteProductDto deleteProductDto)
        {

            if (deleteProductDto.Id == Guid.Empty)
            {
                return BadRequest(new { message = "Product ID is required." });
            }

            var serviceResponse = await _productApplicationService.Delete(deleteProductDto);

            if (!serviceResponse.IsSuccessful)
            {
                return NotFound(new { message = serviceResponse.ErrorMessage });
            }

            return Ok();
        }
        #endregion





    }
}
