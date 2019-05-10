using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebSampleAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        
        //api/empolyees?gender=all
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender="All")
        {
            //username value comes from basicAuthenticationAttribute.cs line 37
            string username = Thread.CurrentPrincipal.Identity.Name;

            using (PlayDBEntities entities=new PlayDBEntities())
            {
                //switch(gender.ToLower())
                switch (username.ToLower())
                {
                    //case "all": return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());

                    case "male":return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender == "male").ToList());

                    case "female": return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender == "female").ToList());

                    default:return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Value of Gender be Male, Female or All " + gender + " is invalid");    
                }

            }
        }
         
        [HttpGet]
        public HttpResponseMessage EmployeeById(int id)
        {
            using (PlayDBEntities entities = new PlayDBEntities())
            {
                var entity= entities.Employees.FirstOrDefault(e => e.ID == id);
                if(entity!=null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id=" + id.ToString() + "Not Found");
                }
            }
            
        }

        [HttpPost]
        public HttpResponseMessage AddNewEmployee ([FromBody]Employee employee)
        {
            try
            {
                using (PlayDBEntities entities = new PlayDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpDelete]
        public HttpResponseMessage DeletEmployee (int id)
        {
            try
            {
                using (PlayDBEntities entities = new PlayDBEntities())
                {


                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if(entity==null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with ID" + id.ToString()+"not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateEmplyee (int id,[FromBody]Employee employee)
        {
            try
            {
                using (PlayDBEntities entities = new PlayDBEntities())
                {


                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID" + id.ToString() + "not found to update");
                    }
                    else
                    {
                        entity.FirstName=employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
