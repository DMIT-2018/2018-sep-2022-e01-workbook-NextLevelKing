<Query Kind="Program">
  <Connection>
    <ID>a3e3431a-be32-4564-8fcd-e08389e36724</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	//sometime this is referred to as sub-queries.
	//
	//Simply put, it is a query within a query[...]

	//Example - List all sales support employees showing their full names(last, first), title and phone.
	//For each employee, show a list of customers they support.
	//Show the customer fullname(last, first), city and state.


	//modelling

	//employee 1, title, phone
	// sub list - customer 2000, city, state,
	//			- customer 2109 city, state
	//			- customer 5000 city, state

	//employee 2, title, phone
	// sub list - customer 203, city, state,
	//			- customer 2102 city, state
	//			- customer 5044 city, state
	
	//there appears to be two separate list that need to be ithin one dataset collection
	
	//list of employees
	//list of employee customers
	
	//concern is that the lists are intermixed!!!
	
	//From C# point of view in a class definition
	//first: This is a composite class
	// the class is describing an employee
	// each instance of the employee will have a list of employee customers
	
	//Lets try this.
	
	//class EmployeeList - we need
	// fullname(property)
	//Title(property)
	//Phone(property)
	//collection of customers associated with that employee(property: List<T>)
	
	
	//second class definition
	//class CustomerList
	//fullname(property)
	//city(property)
	//state(property)
	
	var results = Employees
						.Where(e => e.Title.Contains("Sales Support"))
						.Select(e => new EmployeeItem
						{
							FullName = e.LastName + ", " + e.FirstName,  // note e represents any employee at any time.
							Title = e.Title,
							phone = e.Phone,
							CustomerList = e.SupportRepCustomers
												.Select(c => new CustomerItem
												{
													FullName = c.LastName + ", " + c.FirstName,
													City = c.City,
													State = c.State
												}
												) // make sure to not include a semi-colon here.
							
						}
						
						
						
						
						);
						
		results.Dump();
						
						
}

//Nested Queries in program

public class CustomerItem{     //note class defines a single instance, while list is a collection of items. So use CustomerItem is better than CustomerList
	public string FullName { get; set; }
	public string City { get; set; }
	public string State { get; set; }
}

public class EmployeeItem{
	public string FullName { get; set; }
	public string Title { get; set; }
	public string phone{get; set;}
	public IEnumerable<CustomerItem> CustomerList {get; set;}
}