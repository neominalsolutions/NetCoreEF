using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetCoreEF.Application.Dtos;
using NetCoreEF.Areas.Admin.Models;
using NetCoreEF.Attributes;
using NetCoreEF.Data;

namespace NetCoreEF.Areas.Admin.Controllers
{

    [Area("Admin")]
  public class CategoryController : AdminBaseController
    {
        private readonly NorthwindContext db;
        public IDataProtector _protector;
        private IMapper mapper;
        private ILogger<CategoryController> logger;

        public CategoryController(NorthwindContext db, IDataProtectionProvider provider, IMapper mapper, ILogger<CategoryController> logger, IMediator mediator):base(mediator)
        {
            this.db = db;
            _protector = provider.CreateProtector("kategoriId");
            this.mapper = mapper;
            this.logger = logger;
            this.mediator = mediator;
        }



        [Route("admin/kategoriler", Name = "kategoriListe")]
        public async Task<IActionResult> List()
        {
            //var model = await db.Categories.Select(a => new CategoryDto
            //{
            //  CategoryName = a.CategoryName,
            //  ProtectorId = this._protector.Protect(a.CategoryId.ToString()),
            //  Description = a.Description

            //}).ToListAsync();
            var entities = await db.Categories.ToListAsync();
            var model = mapper.Map<List<CategoryDto>>(entities);
            model.ForEach((item) =>
            {
                item.ProtectorId = _protector.Protect(item.CategoryId.ToString());
            });


            return View(model);
        }

        //[HttpGet]
        //public IActionResult List()
        //{
        //  // async olmayan bir action method içerisinde async bir methodu çağırma şekli
        //  var model =  db.Categories.ToListAsync().GetAwaiter().GetResult();

        //  return View(model);
        //}

        [HttpGet]
        [Route("admin/kategori/{cId}", Name = "kategoriDetay")]
        public async Task<IActionResult> Detail(string cId)
        {
            string entityId = _protector.Unprotect(cId);
            int id = Convert.ToInt32(entityId);

            if (cId == null)
                return BadRequest();  // 400 doğru bir parametre değer gelmedi

            var entity = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            var model = mapper.Map<CategoryDto>(entity);

            if (model == null)
                return NotFound(); // 404Page döner



            return View(model);
        }

        [HttpGet]
        [Route("admin/kategori-ekle", Name = "kategoriEkle")]
        public IActionResult Create()
        {
            //ViewBag.Succeded = null;

            return View();
        }


        //[HttpPost]
        //[Route("kategori-ekle", Name = "kategoriEkle")]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Create(CategoryCreateDto model)
        //{

        //  var entity = mapper.Map<Category>(model);

        //  if (entity == null)
        //    return NotFound();

        //  try
        //  {
        //    if(ModelState.IsValid)
        //    {
        //      await db.Categories.AddAsync(entity);
        //      // State Added
        //      var state = db.Entry(entity).State; // ChangeTracker Mekanizması nasıl çalışıyor

        //      int result = await db.SaveChangesAsync();

        //      if (result > 0)
        //      {
        //        ViewBag.Message = "Kayıt Başarılı oldu";
        //        ViewBag.Succeded = true;
        //      }
        //      else
        //      {
        //        ViewBag.Succeded = false;
        //        ViewBag.Message = "Tekrar Deneyiniz";
        //        this.logger.LogError("Kayıt esnasında bir hata oluştu");
        //      }
        //    }

        //  }
        //  catch (Exception ex)
        //  {
        //    ViewBag.Succeded = false;
        //    ViewBag.Message = ex.Message;
        //    this.logger.LogError(ex.Message);
        //  }

        //  return View();
        //}

        // validasyondan geçemeyen aşağıdaki koda giremez

        [HttpPost]
        [Route("admin/kategori-ekle", Name = "kategoriEkle")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ExceptionFilterAttribute))]

        public async Task<IActionResult> Create(CategoryCreateDto model)
        {

            var entity = mapper.Map<Category>(model);

            if (entity == null)
                return NotFound();

            await db.Categories.AddAsync(entity);
            // State Added
            var state = db.Entry(entity).State; // ChangeTracker Mekanizması nasıl çalışıyor
            int result = await db.SaveChangesAsync();

            if (result > 0)
            {
                ViewBag.Message = "Kayıt Başarılı oldu";
                ViewBag.Succeded = true;
            }
            else
            {
                ViewBag.Succeded = false;
                ViewBag.Message = "Tekrar Deneyiniz";
            }

            return View();
        }


        [HttpGet]
        public IActionResult Update(string key)
        {
            string entityId = _protector.Unprotect(key);
            int id = Convert.ToInt32(entityId);

            var entity = db.Categories.Find(id);

            if (entity == null)
                return NotFound();

            var model = mapper.Map<CategoryUpdateRequestDto>(entity);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ExceptionFilterAttribute))]
        public async Task<IActionResult> Update(CategoryUpdateRequestDto model)
        {
            var response = await mediator.Send(model);

            ViewBag.Succeded = response.IsSucceded;
            ViewBag.Message = response.IsSucceded ? response.SuccessMessage : response.ErrorMessage;

            // 1 sayfadan başka bir sayafaya yönlendiğimizde elimizde yönlendiğimiz sayfadan veri taşımak için kullanırız.

            TempData["Message"] = response.IsSucceded ? response.SuccessMessage : response.ErrorMessage;
            TempData["Succeded"] = response.IsSucceded;
            // sonucu yönlendirip liste sayfasında result görücem.

            //return Redirect("kategoriler");
            return RedirectToAction("List"); // yani şuan CategoryControllerda olduğum için sadece List actioName yazmam yeterli, farklı bir controller action'a yönlendirmek gerekirse o zaman controller, areas gibi değerleri yazmam lazım.
                                             //return View();
        }

        // NetCore tarafında Ajax ile Json Result çalışırken [FromBody] yandi request body üzerinden veri alacağımız anlamına geliyor. [FromHeader] header dan değer oku, [FromQuery] querstring parameters oku [FromRoute] route üzerinden oku
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequestObject model)
        {
            string entityId = _protector.Unprotect(model.Key);
            int id = Convert.ToInt32(entityId);

            var entity = await db.Categories.FindAsync(id);
            db.Categories.Remove(entity);
            int result = await db.SaveChangesAsync();
            var response = new { IsSuccess = result > 0 ? true : false, successMessage = result > 0 ? "Başarılı" : "", errorMessage = result == 0 ? "Hatalı" : "" };

            return Json(response);
        }





    }
}
