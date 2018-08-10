using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace test.Controllers
{
    public class AdminController : UmbracoAuthorizedApiController
    {
        DemoDbEntities db = new DemoDbEntities();
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetIndex()
        {
            try
            {
                using (var db = new DemoDbEntities())
                {

                    var products = db.Products.Select(x => new
                    {
                        Product_ID = x.ProductID,
                        Product_Name = x.Name,
                        Product_Price = x.Price

                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, products);
                };
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetById(Guid? id)
        {
            try
            {
                using(var db = new DemoDbEntities())
                {
                    var p_id = db.Products.Find(id);
                    

                    if(p_id != null)
                    {
                        //return Request.CreateResponse(HttpStatusCode.OK, p_id);
                        var product = db.Products.Where(x => x.ProductID.Equals(p_id)).
                            Select(x => new {
                                Product_name = x.Name,
                                product_price = x.Price
                        });

                        return Request.CreateResponse(HttpStatusCode.OK, product);

                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "id = " + id.ToString() + " was not found.");
                    }
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody] Product product)
        {
            try
            {
                if(product != null)
                {
                    using (var db = new DemoDbEntities())
                    {
                        product.ProductID = Guid.NewGuid();
                        db.Products.Add(product);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.Created);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest + "Operation can't be done");
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Put(Guid? id, [FromBody] Product product)
        {
            var p = db.Products.Find(id);

            try
            {
                if (p != null)
                {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Id = " + id.ToString() + " was not found..");
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(Guid? id)
        {
            try
            {
                var p = db.Products.Find(id);

                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "id = " + id.ToString() + " was not found to be deleted.");
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}
