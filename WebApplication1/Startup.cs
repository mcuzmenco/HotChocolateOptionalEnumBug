using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddRouting()
				.AddGraphQLServer()
				.AddDocumentFromString(@"
	                type Query {
	                  book(id: Int): Book
	                }

					type Mutation {
						addBook(input: BookInput!): Book
					}
					
	                type Book {
					  id: Int!
	                  title: String!
	                  author: String!
	                  genre: Genre!
	                }

	                input BookInput {
	                  title: String!
	                  author: String!
	                  genre: Genre
					}

	                enum Genre {
	                  UNKNOWN
	                  NON_FICTION
	                  FICTION
	                }
	            ")
				.BindRuntimeType<Query>()
				.BindRuntimeType<Mutation>()
				.BindRuntimeType<BookInput>()
				.InitializeOnStartup();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(
				c => c.MapGraphQL()
					.WithOptions(
						new GraphQLServerOptions {
							Tool = { Enable = false },
						}
					)
			);
		}
	}
}
