namespace ContosoUniversityBlazor.Application.System.Commands.SeedData;

using ContosoUniversityBlazor.Application.Common.Interfaces;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class SeedDataCommand : IRequest
{
}

public class SeedDataCommandHandler : IRequestHandler<SeedDataCommand>
{
    private readonly ISchoolContext _schoolContext;

    public SeedDataCommandHandler(ISchoolContext schoolContext)
    {
        _schoolContext = schoolContext;
    }

    public async Task<Unit> Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        var seeder = new DataSeeder(_schoolContext);
        await seeder.Seed();

        return Unit.Value;
    }
}
