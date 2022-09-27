<Query Kind="Statements">
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

//Problems
//One needs to have processed information from a collection to use agaisnt the same collection.

//Solution to this type of proble is to use multiple queries
//	the early query(ies) will produce the needed information/criteria to executew agaisnt the same collection in a later query(ies)
// basically we need to do some processing


//Query 1 - will generate data/information that will be used in the next query(query#2)

//Display the employees that have the most customers to support
//Display the employee name and number of customers that employee supports.

//What is NOT wanted is a list of all employees sorted by number of customers supported.

//One could create a list of all employees with the customer support count, ordered descending by support count. BUT this is not what is requested.

//To solve this, we need the following information
// a) I need to know the maximum number of customers that any particular employee is supporting
//b) I need to take that piece of data and compare to all employees

//A) Get a list of all employees and the count of the customers each supports
//B) From that list, I can obtain the largest number
//C) Using the number, review all the employees and their count, reporting ONLY the busiest employees.

var Preprocessemployeelist = Employees
									.Select(x => new
									{
										Name = x.FirstName + " " + x.LastName,
										CustomerCount = x.SupportRepCustomers.Count()
									}
									)
									//.Dump()
									;
									
// Remember in method syntax, we don have to do Select all the time. We use select to create a subset.
	var highCount = Preprocessemployeelist
							.Max(x => x.CustomerCount)
							//.Dump()
							;
	
	var BusyEmployees = Preprocessemployeelist
									.Where(x => x.CustomerCount == highCount)
									.Dump()
									;
//Shortened version of the code above.

//var BusyEmployees = Preprocessemployeelist
//								.Where(x => x.CustomerCount == Preprocessemployeelist.Max(x => x.CustomerCount))
//								.Dump()
//								;