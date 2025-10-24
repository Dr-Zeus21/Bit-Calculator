using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Input;
using BrendensFuntimeApp.core;
using LearningApp1.Core;

namespace BrendensFuntimeApp.ViewModel
{
    public class CalculatorViewModel : ObservableObject
    {
        public ObservableCollection<NibbleViewModel> Nibbles
        {
            get { return _nibbles; }
            set
            {
                _nibbles = value;
                OnPropertyChanged(); 
            }
        }

        public long DecimalValue
        {
            get { return _decimalValue; }
            set
            {
                _decimalValue = value;
                OnPropertyChanged();

                if (!_updatingNumbers)
                {
                    _updatingNumbers = true;

                    HexValue = DecimalValue.ToString("X");
                    UpdateBits();

                    _updatingNumbers = false;
                }
            }
        }

        public string HexValue
        {
            get { return _hexValue; }
            set
            {
                _hexValue = value;
                OnPropertyChanged();

                if (!_updatingNumbers && _hexValue.Length != 0)
                {
                    _updatingNumbers = true;

                    // Have to get a little funky to convert to signed decimal
                    ulong unsignedValue = Convert.ToUInt64(_hexValue, 16);
                    ulong signMask = 1UL << 63; // Using this to check if the 64th bit is 1 or 0

                    if ((unsignedValue & signMask) == 0) // is this a positive number?
                    {
                        DecimalValue = (long)unsignedValue;
                    }
                    else
                    {
                        DecimalValue = unchecked((long)(unsignedValue - (1UL << 63) - (1UL << 63)));
                    }

                    UpdateBits();

                    _updatingNumbers = false;
                }
            }
        }

        public CalculatorViewModel()
        {
            Nibbles = new ObservableCollection<NibbleViewModel>();

            for (byte i = 0; i < 16; i++)
            {
                NibbleViewModel nibble = new NibbleViewModel((byte)((i)*4)); // Multiply nibble number by 4 to get starting bit position

                Nibbles.Insert(0, nibble); // insert at the front so the front-most nibble is 15
                foreach (BitViewModel bitViewModel in nibble.Bits)
                {
                    bitViewModel.PropertyChanged += Bit_PropertyChanged;
                }
            }
        }

        private ObservableCollection<NibbleViewModel> _nibbles;
        private long _decimalValue = 0;
        private string _hexValue = "0";

        // block execution of other input while using one type of input
        private bool _updatingNumbers = false;

        private void Bit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BitViewModel.BitValue))
            {
                BitViewModel bit = (BitViewModel)sender;
                UpdateOutputs(bit);
            }
        }

        private void UpdateOutputs(BitViewModel bit)
        {
            if (!_updatingNumbers)
            {
                long inputValue = 1;

                // last bit is negative, so turning it on adds a negative and turing it off adds a positive
                if (bit.BitPosition == 63)
                {
                    inputValue *= -1;
                }

                // if turing off the bit
                if (bit.BitValue == false)
                {
                    inputValue *= -1;
                }

                inputValue *= (long)Math.Pow(2, bit.BitPosition); // get 2^(BitPosition), multiply by inputValue to make it negative or positive

                _updatingNumbers = true;

                DecimalValue += inputValue;
                HexValue = DecimalValue.ToString("X"); // this weird toString returns a number as a string formatted to read as Hex

                _updatingNumbers = false;
            }
        }

        private void UpdateBits()
        {
            string binary = Convert.ToString(DecimalValue, 2).PadLeft(64, '0');

            foreach (NibbleViewModel nibble in Nibbles)
            {
                foreach (BitViewModel bit in nibble.Bits)
                {
                    bit.BitValue = binary[binary.Length - bit.BitPosition - 1] == '1';
                }
            }
        }
    }
} 