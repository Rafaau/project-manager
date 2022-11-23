using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel.Interfaces;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Core.Services;
public class MessageServiceTests
{
  private readonly MessageService _sut;
  private readonly IRepository<Message> _messageRepository = Substitute.For<IRepository<Message>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly IRepository<Project2> _projectRepository = Substitute.For<IRepository<Project2>>();
  private readonly ILoggerAdapter<MessageService> _logger = Substitute.For<ILoggerAdapter<MessageService>>();

  public MessageServiceTests()
  {
    _sut = new MessageService(_messageRepository, _userRepository, _projectRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllMessages_ShouldReturnEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(new List<Message>());

    // Act
    var result = await _sut.RetrieveAllMessages();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldReturnMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var expectedMessages = FakeMessageList();
    _messageRepository.ListAsync()
      .Returns(expectedMessages);

    // Act
    var result = await _sut.RetrieveAllMessages();

    // Assert
    result.Should().BeEquivalentTo(expectedMessages);
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(new List<Message>());

    // Act
    await _sut.RetrieveAllMessages();

    // Assert
    _logger.Received(1).LogInformation(Arg.Is("Retrieving all messages"));
    _logger.Received(1).LogInformation(Arg.Is("All messages retrieved in {0}ms"), Arg.Any<long>());
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllMessages();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving all messages"));
  }
  #endregion

  #region Create
  [Fact]
  public async Task PostMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.AddAsync(Arg.Any<Message>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.PostMessage(FakeMessage());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Any<NpgsqlException>(), Arg.Is("Something went wrong while posting message"));
  }

  [Fact]
  public async Task PostMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var messageToPost = FakeMessage();
    _messageRepository.AddAsync(Arg.Any<Message>())
      .Returns(messageToPost);

    // Act
    var result = await _sut.PostMessage(messageToPost);

    // Assert
    result.Should().Be(messageToPost);
  }

  [Fact]
  public async Task PostMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var messageToPost = FakeMessage();
    _messageRepository.AddAsync(Arg.Any<Message>())
      .Returns(messageToPost);

    // Act
    await _sut.PostMessage(messageToPost);

    // Assert
    _logger.Received(1).LogInformation("Posting message");
    _logger.Received(1).LogInformation("Message posted in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task EditMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.EditMessage(FakeMessage());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while editing message (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task EditMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var messageToEdit = FakeMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Returns(messageToEdit);

    // Act
    var result = await _sut.EditMessage(messageToEdit);

    // Assert
    result.Should().BeEquivalentTo(messageToEdit);
  }

  [Fact]
  public async Task EditMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var messageToEdit = FakeMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Returns(messageToEdit);

    // Act
    await _sut.EditMessage(messageToEdit);

    // Assert
    _logger.Received(1).LogInformation("Editing message (id: {0})", 1);
    _logger.Received(1).LogInformation("Message (id: {0}) edited in {1}ms", 1, Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteMessage(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage("Something went wrong");
    _logger.Received(1).LogError(Arg.Any<NpgsqlException>(), Arg.Is("Something went wrong while deleting message (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task DeleteMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Returns(FakeMessage());

    // Act
    var result = await _sut.DeleteMessage(1);

    // Assert
    result.Should().BeEquivalentTo(FakeMessage());
  }

  [Fact]
  public async Task DeleteMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<Message>>())
      .Returns(FakeMessage());

    // Act
    await _sut.DeleteMessage(1);

    // Assert
    _logger.Received(1).LogInformation("Deleting message (id: {0})", 1);
    _logger.Received(1).LogInformation("Message (id: {0}) deleted in {1}ms", 1, Arg.Any<long>());
  }
  #endregion
}
