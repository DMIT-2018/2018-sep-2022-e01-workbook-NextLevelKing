
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ChinookSystem.BLL;
#endregion

namespace ChinookSystem
{
    //your class needs to be public so it can be used outside of this project.
    //it is been called by the webApp
    //this class also needs to be static
    public static class ChinookExtensions
    {
        //the first parameter refers to the method we want to extend and you refer it as this. then give it a variable name
        
        //Action is receiving from program cs the option parametrs we are sending in which happens to be "options"
        
        //Don's comment starts here - Method name can be anything, it must match the builder.Services.xxx(options => ..
        //statement in your program.cs
        //the first parameter is the class that you are attempting 
        // to extend.

        //the second parameter is the options value in your call statement.
        //it is receiving the connection string for your application.
        public static void ChinookSystemBackedDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            //register the DbContext class with the service collection
            services.AddDbContext<ChinookContext>(options);

            //add any services that you create in the class library 
            //using Add.Transient<serviceclassname>(....);
            // we new to create a BLL class
            //we need to register the service method from BLL now
            // every class we create, we just copy this and change the class name.
            services.AddTransient<TrackServices>((serviceProvider) =>
            {
                //retrieve the registered DbContext done with 
                //AddContext

                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new TrackServices(context);
            });

            services.AddTransient<PlaylistTrackServices>((serviceProvider) =>
            {
                //retrieve the registered DbContext done with 
                //AddContext

                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new PlaylistTrackServices(context);
            });
        }
    }
}
