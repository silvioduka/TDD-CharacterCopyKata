using CharacterCopyKata.Tests;

namespace CharacterCopyKata
{
    public class Copier
    {
        private readonly ISource _source;
        private readonly IDestination _destination;

        public Copier(ISource source, IDestination destination)
        {
            _source = source;
            _destination = destination;
        }

        public void Copy()
        {
            var receivedChar = _source.ReadChar();

            while (receivedChar != '\n')
            {
                _destination.WriteChar(receivedChar);
                receivedChar = _source.ReadChar();
            }
        }
    }
}
