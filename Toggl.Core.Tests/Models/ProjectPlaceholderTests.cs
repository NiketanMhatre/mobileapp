using FluentAssertions;
using Toggl.Core.Tests.Mocks;
using Xunit;

namespace Toggl.Core.Tests
{
    public sealed class ProjectPlaceholder
    {
        [Fact]
        public void TheCreatedProjectPlaceholderIsNotActive()
        {
            var timeEntry = new MockTimeEntry { WorkspaceId = 456 };

            var project = Models.Project.CreatePlaceholder(123, timeEntry);

            project.Id.Should().Be(123);
            project.WorkspaceId.Should().Be(456);
            project.Active.Should().BeFalse();
        }
    }
}
