<Query Kind="Expression">
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

//Aggregates

//.Count() -- counts the number of instances in the collection
//Sum(x => .....) sums (totals) a numeric field (numeric expressions) in a collection.
//.Min(x =>...) finds the minimum value of a collection for a field
//.Max(x => ...) finds the maximum value of a collection for a field
//.Average(x => ...) finds the average value of a numeric field(numeric expression in a collection

//Important!!!!!!!!!!!!!!!!!!!!!!!!

//Aggregates work only on a collection of values.
//Aggregates DO NOT work on a single instance (non declared collection)

//.Sum, .Min, .Max, and .Average MUST have at least one record in their collection.
//.Sum and .Average MUST work on numeric fields and the fields CANNOT be null.

//Syntax : Method

//collectionset.aggregate(x => expression) we can also do
//collectionset.Select(...).Aggregate()
//Collectionset.Count() //.Count() does not contain an expression

//for Sum, Min, Max and Average: the result is a single value

//you can use multiple aggregates on a single column 
//Example  -  .Sum(x => expression).Min(x => expression)

//Find the average playing time(length) of tracks in our music collection

//thought process
//average is an aggregate
//what is the collection? - the Tracks table is a collection
//What is the expression? - Length of play is in Milliseconds

Tracks.Average(x => x.Milliseconds) //- for each record, give me the average of milliseconds. Each x has multiple fields

//Do it a different way
Tracks.Select(x => x.Milliseconds).Average() // works because it is a single list of numbers

//Let us try

//Tracks.Average() // Aborts because no specific field was referred to on the track record


//List all albums of the 60s showing the title, artist and various aggregates for albums containing tracks.
//For each album show the number of tracks, the total price of all tracks and the average playing length of the album tracks

//Thought process
//Start at Album
//can I get the artist name(.Artist)
// can I get a collection of tracks for an album(x.Tracks)
//can I get the number of tracks in the collection (.Count())
//can I get the total price of the price (.Sum())
//can I get the average of the play length (.Average())
Albums
    .Where(x => x.Tracks.Count() > 0 
        && (x.ReleaseYear > 1959 && x.ReleaseYear < 1970))
    .Select(x => new
    	{
    		Title = x.Title,
            Artist = x.Artist.Name,
    		NumberofTracks = x.Tracks.Count(),
    		TotalPrice  = x.Tracks.Sum(tr => tr.UnitPrice),
    		AverageTrackLength = x.Tracks.Select(tr => tr.Milliseconds).Average()
    	}
        )



//Under issues - click milestone - click new milestone - back underneath issues - new issue -title- exercise 1 question one - comment or paste question or your thought process- assign to my selft - create a label by edit label- new label - exercises. - dropdown milestone and choose your miles stone- underneath issues - for commit, finnished question number 3.