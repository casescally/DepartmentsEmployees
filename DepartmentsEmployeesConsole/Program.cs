using DepartmentsEmployees.Data;
using DepartmentsEmployees.Models;
using System;
using System.Collections.Generic;


namespace DepartmentsEmployeesConsole
{
    public class Program
    {
        static void Main(string[] args)
        {

            //Get all departments
            DepartmentRepository departmentRepo = new DepartmentRepository();

            Console.WriteLine("Getting All Departments:");
            Console.WriteLine();

            List<Department> allDepartments = departmentRepo.GetAllDepartments();

            foreach (Department dept in allDepartments)
            {
                Console.WriteLine($"{dept.Id} {dept.DeptName}");
            }

            //Get department by ID
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Department with Id 1");

            Department singleDepartment = departmentRepo.GetDepartmentById(1);

            Console.WriteLine($"{singleDepartment.Id} {singleDepartment.DeptName}");

            EmployeeRepository employeeRepo = new EmployeeRepository();


            //Get all employees
            Console.WriteLine("Getting All Employees:");
            Console.WriteLine();

            List<Employee> allEmployees = employeeRepo.GetAllEmployees();

            foreach (Employee emp in allEmployees)
            {
                Console.WriteLine($"{emp.Id} {emp.FirstName} {emp.LastName}");
            }



            //Get all employees with department
            Console.WriteLine("Getting All Employees with department:");
            Console.WriteLine();

            List<Employee> allEmployeesWithDepartment = employeeRepo.GetAllEmployees();

            foreach (Employee emp in allEmployeesWithDepartment)
            {
                Console.WriteLine($"{emp.Id} {emp.FirstName} {emp.LastName} {emp.Department.DeptName}");
            }



            //Get employee by ID
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Employee with Id 1");
            Employee singleEmployee = employeeRepo.GetEmployeeById(1);
            Console.WriteLine($"{singleEmployee.Id} {singleEmployee.FirstName} {singleEmployee.LastName} {singleEmployee.Department}");



            //Update employee
            string updateChoice = Console.ReadLine();
            int updateChoiceInt = int.Parse(updateChoice);
            string employeeToUpdate = Console.ReadLine();
            int employeeToUpdateInt = int.Parse(employeeToUpdate);
            var repo = new EmployeeRepository();
            if (updateChoiceInt == 1) { 
                Console.WriteLine("First Name");
            var updatedFirstName = Console.ReadLine();

            Console.WriteLine("Last Name");
            var updatedLastName = Console.ReadLine();

            Console.WriteLine("Department");
            int updatedDepartment = int.Parse(Console.ReadLine());

            var updatedEmployee = new Employee()
            {
                FirstName = updatedFirstName,
                LastName = updatedLastName,
                DepartmentId = updatedDepartment
            };

            repo.CreateNewEmployee(updatedEmployee);
        };


            //Create new employee
            Console.WriteLine("First Name");
            var newEmployeeFirstName = Console.ReadLine();

            Console.WriteLine("Last Name");
            var newEmployeeLastName = Console.ReadLine();

            Console.WriteLine("Department");
            int newEmployeeDepartment = int.Parse(Console.ReadLine());

            var newEmployee = new Employee()
            {
                FirstName = newEmployeeFirstName,
                LastName = newEmployeeLastName,
                DepartmentId = newEmployeeDepartment
            };

            repo.CreateNewEmployee(newEmployee);



            //Delete Employee
            Console.WriteLine("----------------------------");
            Console.WriteLine("Delete Employee? 1. Yes 2. No");
            string deleteChoice = Console.ReadLine();
            int deleteChoiceInt = int.Parse(deleteChoice);
            string employeeToDelete = Console.ReadLine();
            int employeeToDeleteInt = int.Parse(employeeToDelete);
            if (deleteChoiceInt == 1)
            {
                repo.DeleteEmployee(employeeToDeleteInt);
            }
        }
    }
}
