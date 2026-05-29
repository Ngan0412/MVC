namespace MVC.Services;

// 1. Định nghĩa các Interface
public interface ITransientService { Guid Id { get; } }
public interface IScopedService { Guid Id { get; } }
public interface ISingletonService { Guid Id { get; } }

// 2. Triển khai các Class thực tế
public class TransientService : ITransientService
{
    public Guid Id { get; } = Guid.NewGuid(); // Tự sinh mã mới khi New
}

public class ScopedService : IScopedService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class SingletonService : ISingletonService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public interface ITestDIService
{
    (Guid transientId, Guid scopedId, Guid singletonId) GetServiceIds();
}

public class TestDIService : ITestDIService
{
    private readonly ITransientService _transient;
    private readonly IScopedService _scoped;
    private readonly ISingletonService _singleton;

    // Ép MainService nhận các dịch vụ lần thứ nhất
    public TestDIService(ITransientService transient, IScopedService scoped, ISingletonService singleton)
    {
        _transient = transient;
        _scoped = scoped;
        _singleton = singleton;
    }

    public (Guid transientId, Guid scopedId, Guid singletonId) GetServiceIds()
    {
        return (_transient.Id, _scoped.Id, _singleton.Id);
    }
}