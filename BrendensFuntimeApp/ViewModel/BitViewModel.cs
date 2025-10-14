using LearningApp1.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BrendensFuntimeApp.ViewModel
{
    public class BitViewModel : ObservableObject
    {
        public bool BitValue
        {
            get => (_bitValue);
            set
            {
                _bitValue = value;
                OnPropertyChanged(nameof(BitValue));
            }
        }
        public byte BitPosition
        {
            get => (_bitPosition);
            set
            {
                _bitPosition = value;
                OnPropertyChanged(); // Do I need to call this, since this isn't changing?
            }
        }

        public BitViewModel(byte bitPosition)
        {
            BitPosition = bitPosition;
        }

        private bool _bitValue = false;
        private byte _bitPosition;
    }
}
