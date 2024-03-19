using DEMO;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using NetX.ServiceCore;

namespace NetX.RBAC.Service
{
    [GrpcEntry]
    public class DemoServiceCore : DEMO.DEMOService.DEMOServiceBase
    {
        private readonly DemoDbContext _dbContext;

        public DemoServiceCore(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<TestResponse> Test(TestRequest request, ServerCallContext context)
        {
            await _dbContext.sys_dept.AddAsync(new sys_dept()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test",
                ParentId = "0"

            });
            await _dbContext.SaveChangesAsync();
            return new TestResponse()
            {
                 IsSuccess = true
            };
        }
    }
}
