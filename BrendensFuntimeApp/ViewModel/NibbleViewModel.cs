
using System.Windows.Controls;
using System.Windows;
using BrendensFuntimeApp.ViewModel;
using LearningApp1.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;

namespace BrendensFuntimeApp.core
{
    public class NibbleViewModel : ObservableObject
    {
        public ObservableCollection<BitViewModel> Bits
        {
            get { return _bits; }
            set
            {
                _bits = value;
                OnPropertyChanged();
            }
        }

        public byte StartingBitPosition
        {
            get => (_startingBitPosition);
            set
            {
                _startingBitPosition = value;
                OnPropertyChanged(); // Do I need to call this, since this isn't changing?
            }
        }

        public byte NibbleValue
        {
            get => (_nibbleValue);
            set
            {
                _nibbleValue = value;
                OnPropertyChanged(); 
            }
        }

        public NibbleViewModel(byte startingBitPosition)
        {
            Bits = new ObservableCollection<BitViewModel>();

            StartingBitPosition = startingBitPosition;
            NibbleValue = 0;

            for (byte i = 0; i < 4; i++)
            {
                BitViewModel bit = new BitViewModel((byte)(startingBitPosition + i));

                Bits.Insert(0, bit); // insert at the front so the front-most bit is SBP + 3
            }
        }

        private ObservableCollection<BitViewModel> _bits;
        private byte _startingBitPosition;
        private byte _nibbleValue;
    }
}
