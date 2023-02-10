using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Kmp
    {
        private int[][] _prefixFunctions;
        private List<CellState>[] _patterns;

        public Kmp(params List<CellState>[] patterns)
        {
            _patterns = patterns;
            _prefixFunctions = new int[patterns.Length][];
            for (var index = 0; index < patterns.Length; index++)
            {
                var pattern = patterns[index];
                var prefixFunction = new int[pattern.Count];
                var j = 0;
                for (var i = 1; i < pattern.Count; i++)
                {
                    while (j > 0 && pattern[i] != pattern[j])
                    {
                        j = prefixFunction[j - 1];
                    }

                    if (pattern[i] == pattern[j])
                    {
                        j++;
                    }

                    prefixFunction[i] = j;
                }

                _prefixFunctions[index] = prefixFunction;
            }
        }

        public bool Match(List<CellState> text)
        {
            for (int index = 0; index < _patterns.Length; index++)
            {
                var pattern = _patterns[index]; 
                var prefixFunction = _prefixFunctions[index]; 
                var j = 0;
                for (var i = 0; i < text.Count; i++)
                {
                    while (j > 0 && text[i] != pattern[j])
                    {
                        j = prefixFunction[j - 1];
                    }
                    if (text[i] == pattern[j])
                    {
                        j++;
                    }
                    if (j == pattern.Count)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

    }
}