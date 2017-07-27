using Machine.Specifications;

namespace Collectively.Services.Groups.Tests
{
    [Subject("Dummy")]
    public class DummyTests
    {
        It should_be_true = () => true.ShouldBeTrue();
    }
}