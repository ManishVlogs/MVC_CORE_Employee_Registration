using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CORE_Employee_Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_CORE_Employee_Registration.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _db;
        public EmployeeController(EmployeeContext db)
        {
            _db = db;
        }
        public IActionResult EmployeeList()
        {
            try
            {
                //var EmpList = _db.tbl_EmployeeDtl.ToList();//extracting data from single table .
                var EmpList = from a in _db.tbl_EmployeeDtl
                              join de in _db.Departments on a.DeptID equals de.ID
                              into dep
                              from de in dep.DefaultIfEmpty()

                              select new Employee
                              {
                                  ID = a.ID,
                                  Name = a.Name,
                                  Fname = a.Fname,
                                  Mobile = a.Mobile,
                                  Email = a.Email,
                                  description = a.description,
                                  DeptID = a.DeptID,
                                  Department = de == null ? "" : de.Department
                              };
                return View(EmpList);
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        //this method will add the new employee
        public IActionResult CreateEmployee(Employee obj)
        {
            LoadDepartment();
            return View(obj);
        }


        //this method will save the data
        //Note:- for saving it is required or good habbit to create a method with async.
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (obj.ID == 0)
                    {
                        _db.tbl_EmployeeDtl.Add(obj);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        _db.Entry(obj).State = EntityState.Modified;//to update the data this simple step.
                        await _db.SaveChangesAsync();
                    }

                    return RedirectToAction("EmployeeList");
                }
                return View();
            }
            catch (Exception ex)
            {

                return RedirectToAction("EmployeeList");
            }
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var emp = await _db.tbl_EmployeeDtl.FindAsync(id);
                if (emp != null)
                {
                    _db.tbl_EmployeeDtl.Remove(emp);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("EmployeeList");
            }
            catch (Exception ex)
            {

                return RedirectToAction("EmployeeList");
            }
        }
        //this method will give all the list of department  from the table into the dropdown down list 
        private void LoadDepartment()
        {
            try
            {
                List<Departments> deptList = new List<Departments>();
                deptList = _db.Departments.ToList();
                deptList.Insert(0, new Departments { ID = 0, Department = "Please Select" });//at zero index 
                ViewBag.DeartmemtList = deptList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
