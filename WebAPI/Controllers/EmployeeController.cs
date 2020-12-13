using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, convert(varchar(10),DOJ,120) as DOJ, PhotoFilenName from dbo.Employees";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"insert into dbo.Employees(EmployeeName, Department, DOJ, PhotoFilenName) values (
                                '" + emp.EmployeeName + @"',
                                '" + emp.Department + @"',
                                '" + emp.DateOfJoining + @"',
                                '" + emp.PhotoFileName + @"'
                            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"update dbo.Employees set 
                            EmployeeName= '" + emp.EmployeeName + @"',
                                Department= '" + emp.Department + @"',
                                DOJ= '" + emp.DateOfJoining + @"',
                                PhotoFilenName= '" + emp.PhotoFileName + @"'
                            where EmployeeId = " + emp.EmployeeId;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Employees where EmployeeId = " + id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;
                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"select DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(query, myCon);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }

    }
}
