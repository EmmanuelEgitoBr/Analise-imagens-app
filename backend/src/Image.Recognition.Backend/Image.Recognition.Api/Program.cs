using Amazon.S3;
using Image.Recognition.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiInfrastructure();
builder.Services.AddMongoInfrastructure(builder.Configuration);
builder.Services.AddAwsInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/list-buckets", async (IAmazonS3 s3Client) =>
{
    var response = await s3Client.ListBucketsAsync();
    return response.Buckets.Select(b => b.BucketName).ToList();
});

app.Run();
