# AwsDynamo

Step 1 : CreateTable

```
public class Product : DomainEntity<string>
{
    public const string tableName = "dbo_Product";

    public string Name { set; get; }
}

```

Step 2 : Create repository

public interface ITestRepository : IGenericRepository<Test, string>
 {

 }
public class TestRepository : GenericRepository<Test, string>, ITestRepository
{
  public TestRepository(IAmazonDynamoDB client) : base(client)
  {
  }
}
Step 3 : Add AWS Config

 "AWS": {
    "Profile": "",
    "Region": "",
    "AccessKey": "",
    "SecretKey": "",
    "BucketName": ""
  }

Step 4 : Register Service and Register Application

 public void ConfigureServices(IServiceCollection services)
 {
      services.AddDefaultAWSOptions(DynamoHelper.GetAwsOptions(configuration));
      services.AddAWSService<IAmazonDynamoDB>();

       services.AddDynamoOptions()
                .UseDynamoDb<Test, string>(x => x.TableName = Test.tableName)

      services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
      services.AddScoped<ITestRepository, TestRepository>();
 }


 public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
{
      var testDbInitializer = serviceProvider.GetService<IDynamoDbInitializer<Test, string>>();
      testDbInitializer.EnsureCreatedAsync().Wait();
}

Step 5 : Using 

public class TestController : Controller
{
     private readonly ITestRepository _testRepository;
     public TestController(ITestRepository testRepository)
     {
         _testRepository = testRepository
     }
}
