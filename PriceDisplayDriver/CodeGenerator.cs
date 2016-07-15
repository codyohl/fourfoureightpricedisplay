using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             *  +-2-+
             *  |   |
             *  1   3
             *  |   |
             *  +-4-+
             *  |   |
             *  0   5
             *  |   |
             *  +-6-+
             */
            List<List<int>> segments = new List<List<int>>
            {
                new List<int> { 0, 1, 2, 3, 5, 6 },    /* 0 */
                new List<int> { 3, 5 },                /* 1 */
                new List<int> { 2, 3, 4, 0, 6 },       /* 2 */
                new List<int> { 2, 3, 4, 5, 6 },       /* 3 */
                new List<int> { 1, 4, 3, 5 },          /* 4 */
                new List<int> { 2, 1, 4, 5, 6 },       /* 5 */
                new List<int> { 2, 1, 0, 6, 5, 4 },    /* 6 */
                new List<int> { 1, 2, 3, 5 },          /* 7 */
                new List<int> { 1, 2, 3, 4, 0, 6, 5 }, /* 8 */
                new List<int> { 1, 2, 3, 4, 5, 6 },    /* 9 */
            };

            List<List<int>> digitToPinMapping = new List<List<int>>
            {
                new List<int> { 2, 3, 4, 5, 6, 7, 8 },
                new List<int> { 12, 13 ,14, 15, 20, 21, 22 },
                new List<int> { 23, 24, 25, 27, 30, 31, 32 },
            };

            GenerateArray(segments);
            Console.WriteLine(string.Join(",", digitToPinMapping.Aggregate(Enumerable.Empty<int>(), (x, y) => x.Concat(y) )));
        }

        /**
         * The generated array is an basically an array of lists
         * The first ten entries maps to the head of the list
         * The lists are -1 terminated
         */
        private static void GenerateArray(List<List<int>> segments)
        {
            List<int> segmentArray = new List<int>();

            // Placeholder, the initial 10 entries are index to the sublists
            for (int i = 0; i < segments.Count; i++)
            {
                segmentArray.Add(0);
            }
            for (int i = 0; i < segments.Count; i++)
            {
                segmentArray[i] = segmentArray.Count;
                foreach (var entry in segments[i])
                {
                    segmentArray.Add(entry);
                }
                segmentArray.Add(-1);
            }

            Console.WriteLine(string.Join(",", segmentArray));
        }
    }
}
