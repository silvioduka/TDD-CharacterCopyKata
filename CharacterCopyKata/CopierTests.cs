using NSubstitute;

using NUnit.Framework;

namespace CharacterCopyKata.Tests
{
    [TestFixture]
    public class CopierTests
    {
        public class Copy
        {
            public class AfterNewline
            {
                [Test]
                public void GivenAnyCharacters_ShouldWriteNone()
                {
                    // Arrange
                    var source = CreateSourceMock('\n','a', 'b', 'c', 'd');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(0).WriteChar(Arg.Any<char>());
                }
            }

            public class BeforeNewline
            {
                [TestCase('a', '\n')]
                [TestCase('!', '\n')]
                [TestCase('B', '\n')]
                public void GivenSingleCharacter_ShouldWriteThatCharacter(char firstChar, params char[] nextChar)
                {
                    // Arrange
                    var source = CreateSourceMock(firstChar, nextChar);
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(1).WriteChar(firstChar);
                    destination.Received(1).WriteChar(Arg.Any<char>());
                }
           
                [Test]
                public void GivenNoCharacter_ShouldNotWriteAnyCharacters()
                {
                    // Arrange
                    var source = CreateSourceMock('\n');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(0).WriteChar(Arg.Any<char>());
                }

                [Test]
                public void GivenTwoCharacters_ShouldWriteBothCharacters()
                {
                    // Arrange
                    var source = CreateSourceMock('a', 'b', '\n');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(2).WriteChar(Arg.Any<char>());
                    destination.Received(1).WriteChar('a');
                    destination.Received(1).WriteChar('b');
                }
            
                [Test]
                public void GivenManyCharacters_ShouldWriteAllThoseCharacters()
                {
                    // Arrange
                    var source = CreateSourceMock('a', 'b', 'c', 'd', '\n');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(4).WriteChar(Arg.Any<char>());
                    destination.Received(1).WriteChar('a');
                    destination.Received(1).WriteChar('b');
                    destination.Received(1).WriteChar('c');
                    destination.Received(1).WriteChar('d');
                }

                [Test]
                public void GivenRepeatedCharacters_ShouldWriteAllThoseCharacters()
                {
                    // Arrange
                    var source = CreateSourceMock('a', 'b', 'a', 'c', '\n');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    destination.Received(4).WriteChar(Arg.Any<char>());
                    destination.Received(2).WriteChar('a');
                    destination.Received(1).WriteChar('b');
                    destination.Received(1).WriteChar('c');
                }

                [Test]
                public void ShouldWriteAllThoseCharactersInTheOrderSupplied()
                {
                    // Arrange
                    var source = CreateSourceMock('a', 'b', 'a', 'c', 'd', 'b', '\n');
                    var destination = CreateDestinationMock();

                    var sut = CreateSut(source, destination);

                    // Act
                    sut.Copy();

                    // Assert
                    Received.InOrder(() =>
                    {
                        destination.WriteChar('a');
                        destination.WriteChar('b');
                        destination.WriteChar('a');
                        destination.WriteChar('c');
                        destination.WriteChar('d');
                        destination.WriteChar('b');
                    });
                }
            }
        }

        private static ISource CreateSourceMock(char firstChar, params char[] nextChar)
        {
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(firstChar, nextChar);
            return source;
        }

        private static Copier CreateSut(ISource source, IDestination destination)
        {
            var sut = new Copier(source, destination);
            return sut;
        }

        private static IDestination CreateDestinationMock()
        {
            var destination = Substitute.For<IDestination>();
            return destination;
        }
    }
}
