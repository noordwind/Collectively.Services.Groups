using FluentAssertions;
using Machine.Specifications;

namespace Collectively.Services.Groups.Tests.EndToEnd
{
    [Subject("Dummy")]
    public class DummyTests
    {
        It should_be_true = () => true.ShouldBeTrue();
    }
}