using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DepartmentsEmployees.Models;

namespace DepartmentsEmployees.Data
{

    ///  An object to contain all database interactions.

    public class EmployeeRepository
    {

        ///  Represents a connection to the database.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.

        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DepartmentsEmployees;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }



        //Returns all employees
        public List<Employee> GetAllEmployees()
        {
            // 1. Open a connection to the database
            // 2. Create a SQL SELECT  statement as a C# string
            // 3. Execute that SQL statement against the database
            // 4. From the database, we get "raw data" back. We need to parse this as a C# object
            // 5. Close the connection to the database
            // 6. Return the Employee object


            // This opens the connection. SQLConnection is the TUNNEL
            using (SqlConnection conn = Connection)
            {
                // This opens the GATES on either side of the TUNNEL
                conn.Open();

                // SQLCommand is the list of instructions to give to a truck driver when they get to the other side of the TUNNEL
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here is the sql command that we want to be run when the driver gets to the database
                    cmd.CommandText = @"
                        SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, d.Id, d.DeptName
                        FROM Employee e
                        LEFT JOIN Department d
                        ON e.DepartmentId = d.Id";

                    // ExecuteReader actually has the driver go to the database and executes that command. The driver then comes back with a bunch of data from the database. This is held in the this variable called "reader"
                    SqlDataReader reader = cmd.ExecuteReader();

                    // This is just us initializing the list that we'll eventually return
                    List<Employee> allEmployees = new List<Employee>();


                    // The reader will read the returned data from the database one row at a time. This is why we put it in a while loop
                    while (reader.Read())
                    {
                        // Get ordinal returns us what "position" the Id column is in
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        // The reader isn't smart enough to know automatically what TYPE of data it's reading.
                        // For that reason we have to tell it, by saying `GetInt32`, `GetString`, GetDate`, etc
                        int firstNameColumn = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumn);

                        int lastNameColumn = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumn);

                        int departmentIdColumn = reader.GetOrdinal("DepartmentId");
                        int departmentValue = reader.GetInt32(departmentIdColumn);

                        int departmentNameColumn = reader.GetOrdinal("DeptName");
                        string departmentNameValue = reader.GetString(departmentNameColumn);

                        // Now that all the data is parsed, we create a new C# object
                        var employee = new Employee()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            DepartmentId = departmentValue,
                            Department = new Department()
                            {
                                Id = departmentValue,
                                DeptName = departmentNameValue
                            }
                        };

                        // Now that we have a parsed C# object, we can add it to the list and continue with the while loop
                        allEmployees.Add(employee);
                    }

                    // Now we can close the connection
                    reader.Close();

                    // and return all employees
                    return allEmployees;
                }
            }
        }


        ///  Returns a single employee with the given id.
        public Employee GetEmployeeById(int employeeId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName FROM Employee WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", employeeId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = employeeId,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                        };
                    }

                    reader.Close();

                    return employee;
                }
            }
        }


        // Create a new employee
        public Employee CreateNewEmployee(Employee employeeToAdd)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Employee (FirstName, LastName, DepartmentId)
                        OUTPUT INSERTED.Id
                        VALUES (@firstName, @lastName, @departmentId)";

                    cmd.Parameters.Add(new SqlParameter("@firstName", employeeToAdd.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", employeeToAdd.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", employeeToAdd.DepartmentId));

                    int id = (int)cmd.ExecuteScalar();

                    employeeToAdd.Id = id;

                    return employeeToAdd;
                }
            }
        }

        ///  Updates the employee with the given id
        /// 
        public void UpdateEmployee(int employeeId, Employee employee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Employee
                        SET FirstName = @firstName, LastName = @lastName, DepartmentId = @departmentId
                        WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@id", employeeId));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        ///  Delete the employee with the given id
        public void DeleteEmployee(int employeeId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Department WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", employeeId));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}