using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using EmployeeAPI.Models;  
namespace EmployeeAPI.Controllers 
{     
    [Route("api/[controller]")]     
    [ApiController]     
    public class EmployeeController : ControllerBase     
    {        
        private readonly  EmployeeContext dbEmployee = new EmployeeContext(); 

        //For reference  
        /*       
        public EmployeeController(EmployeeContext context)         
        {             _context = context;              
            if (_context.Employees.Count() == 0)             
            {                 
            _context.Employees.Add(new Employees { Name = "Item1" });                 _context.SaveChanges();             
            }         
        }*/ 

        #region Get Employees
        [HttpGet]
        [Route ("GetEmployees")]
        public IActionResult GetEmployees()
        {
            var empList = from e in dbEmployee.Employees
                          select e;

            return Ok(empList);
        }
        #endregion
        
        #region Search Employees
        [HttpGet]
        [Route("SearchEmployees/{empID}")]
        public IActionResult SearchEmployee(int id)
        {
            try
            {
            var searchResult = from e in dbEmployee.Employees
                               where e.ID == id
                               select e;
            if (e != null)
            {
                return Ok(searchResult);
            }
            else{
                return BadRequest("Could not find employee with that ID");
            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add Employee
        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult CreateEmployee(int Id, string fName, string lName, int age)
        {
            var newEmployee = new Employee();

            newEmployee.eID = Id;
            newEmployee.eFirstName = fName;
            newEmployee.eLastName = lName;
            newEmployee.eAge = age;


            //Prevention of just hitting Enter over and over
            if (newEmployee != null)
            {
                dbEmployee.Employees.Add(newEmployee);
                dbEmployee.SaveChanges();

                return Created("", newEmployee);
            }
            else
            {
                return BadRequest("Error: Could not make new Employee");
            }
        }
        #endregion

//I'm iffy about this one...
        #region Update Employee
        [HttpPut]
        [Route("UpdateEmployee/{empID}")]
        public IActionResult UpdateEmployee(int id, Employee employee)
        {
            try
            {
                /*if(id != employee.eID){
                    return BadRequest("No employee exists by that ID")
                }*/
            var searchResult = from e in dbEmployee.Employees
                               where e.ID.Contains(id)
                               select e;
            if (e != null)
            {
                searchResult.ID = employee.eID;
                searchResult.fName = employee.eFirstName;
                searchResult.lName = employee.eLastName;
                searchResult.age = employee.eAge;
            }
            else{
                return BadRequest("Could not find employee with that ID");
            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
        #endregion

        #region Delete Employee
        [HttpDelete]
        [Route("delete-employee/{empID}")]
        public IActionResult DeleteEmployee(int empID)
        {
            var emp = (from e in dbEmployee.Employees
                         where e.ID == empID
                         select e).SingleOrDefault();

            if (empID != null)
            {
                dbEmployee.Employees.Remove(Employee);
                dbEmployee.SaveChanges();
                return Ok("Employee deleted.");
            }
            else
            {
                return BadRequest("Invalid Order ID.");
            }
        }
        #endregion
        
    } 
}