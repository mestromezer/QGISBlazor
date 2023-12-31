using Microsoft.EntityFrameworkCore;
using QGISEFApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QGISEFApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QGISEFApiContext") 
    ?? throw new InvalidOperationException("Connection string 'QGISEFApiContext' not found.")
    , x => x.UseNetTopologySuite()
    ));
// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddCors();
//builder.Services.AddSingleton(NtsGeometryServices.Instance);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.Run();
